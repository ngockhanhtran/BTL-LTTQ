using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [11/11/2026 lần 7] Dự báo nhập hàng (quản lý + thủ kho).
    /// Từ tốc độ bán N ngày gần nhất -> "Panadol còn đủ bán 6 ngày, nên nhập 120 hộp".
    /// Tích chọn -> "Lập phiếu nhập Excel" (đúng mẫu import) -> trang Nhập hàng -> Import.
    /// Công thức ở BLL/DuBaoNhapHangBLL.Tinh (có unit test).
    /// </summary>
    public partial class frmDuBaoNhapHang : Form
    {
        private readonly DuBaoNhapHangBLL bll = new DuBaoNhapHangBLL();
        private List<KetQuaDuBao> ketQua = new List<KetQuaDuBao>();

        /// <summary>Bấm "Trang Nhập hàng" (frmMain chuyển trang).</summary>
        public event EventHandler YeuCauNhapHang;

        public frmDuBaoNhapHang()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            foreach (Button b in new[] { btnTinh, btnXuatPhieu, btnMoNhapHang }) b.Tag = b.BackColor;
        }

        private async void frmDuBaoNhapHang_Load(object sender, EventArgs e)
        {
            await TinhAsync();
        }

        private async void btnTinh_Click(object sender, EventArgs e)
        {
            await TinhAsync();
        }

        private void chkChiCanNhap_CheckedChanged(object sender, EventArgs e)
        {
            HienThi();
        }

        private async Task TinhAsync()
        {
            int soNgay = (int)nudPhanTich.Value, duTru = (int)nudDuTru.Value;
            GiaoDien.BatTatNut(btnTinh, false);
            try
            {
                using (KhungDangTai.Hien(dgvDuBao, "Đang phân tích số liệu bán…"))
                    ketQua = await Task.Run(() => bll.DuBao(soNgay, duTru));
                HienThi();
            }
            catch (Exception ex)
            {
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi dự báo");
            }
            finally
            {
                GiaoDien.BatTatNut(btnTinh, true);
            }
        }

        private void HienThi()
        {
            dgvDuBao.Rows.Clear();
            IEnumerable<KetQuaDuBao> ds = chkChiCanNhap.Checked ? ketQua.Where(k => k.DeXuatNhap > 0) : ketQua;
            foreach (KetQuaDuBao k in ds)
            {
                int i = dgvDuBao.Rows.Add(k.DeXuatNhap > 0, k.SoLieu.MaThuoc, k.SoLieu.TenThuoc, k.SoLieu.TenDVT,
                                          k.SoLieu.TonBanDuoc.ToString("N0"), k.SoLieu.DaBan.ToString("N0"),
                                          k.TocDoBanNgay.ToString("0.##"),
                                          k.SoNgayConDu.HasValue ? k.SoNgayConDu.Value.ToString("0.#") : "—",
                                          k.DeXuatNhap > 0 ? k.DeXuatNhap.ToString("N0") : "",
                                          k.TenMucDo, k.LoiKhuyen);
                DataGridViewRow r = dgvDuBao.Rows[i];
                r.Tag = k;
                r.Cells[colChon.Index].ReadOnly = k.DeXuatNhap <= 0;
                ToMau(r, k.MucDo);
            }
            CapNhatTomTat();
        }

        private void ToMau(DataGridViewRow r, MucDoDuBao m)
        {
            Color nen, chu;
            switch (m)
            {
                case MucDoDuBao.HetHang: nen = Color.FromArgb(248, 215, 218); chu = Color.FromArgb(132, 32, 41); break;
                case MucDoDuBao.CanNhapGap: nen = Color.FromArgb(255, 228, 230); chu = Color.FromArgb(176, 42, 55); break;
                case MucDoDuBao.NenNhap: nen = Color.FromArgb(255, 243, 205); chu = Color.FromArgb(133, 100, 4); break;
                case MucDoDuBao.DuHang: nen = Color.FromArgb(232, 245, 233); chu = Color.FromArgb(30, 110, 50); break;
                default: nen = Color.White; chu = Color.Gray; break;
            }
            r.Cells[colMucDo.Index].Style.BackColor = nen;
            r.Cells[colMucDo.Index].Style.ForeColor = chu;
            r.Cells[colMucDo.Index].Style.Font = new Font(dgvDuBao.Font, FontStyle.Bold);
            if (m == MucDoDuBao.HetHang || m == MucDoDuBao.CanNhapGap)
                r.Cells[colConDu.Index].Style.ForeColor = chu;
        }

        private List<KetQuaDuBao> DongDaChon()
        {
            return dgvDuBao.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Tag is KetQuaDuBao && Convert.ToBoolean(r.Cells[colChon.Index].Value ?? false))
                .Select(r => (KetQuaDuBao)r.Tag).ToList();
        }

        private void CapNhatTomTat()
        {
            int gap = ketQua.Count(k => k.MucDo == MucDoDuBao.HetHang || k.MucDo == MucDoDuBao.CanNhapGap);
            int nen = ketQua.Count(k => k.MucDo == MucDoDuBao.NenNhap);
            List<KetQuaDuBao> chon = DongDaChon();
            decimal tien = chon.Sum(k => k.DeXuatNhap * k.SoLieu.DonGiaNhap);
            lblTomTat.Text = string.Format("{0} thuốc hết / cần nhập gấp,  {1} thuốc nên nhập.\nĐã chọn {2} thuốc, ước tính tiền nhập ≈ {3:N0} đ (theo giá nhập gần nhất).",
                                           gap, nen, chon.Count, tien);
            GiaoDien.BatTatNut(btnXuatPhieu, chon.Count > 0);
        }

        // Ô checkbox: ghi giá trị ngay khi tích (mặc định DataGridView chỉ ghi khi rời ô)
        private void dgvDuBao_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvDuBao.IsCurrentCellDirty && dgvDuBao.CurrentCell is DataGridViewCheckBoxCell)
                dgvDuBao.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvDuBao_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colChon.Index) CapNhatTomTat();
        }

        private void btnXuatPhieu_Click(object sender, EventArgs e)
        {
            List<KetQuaDuBao> chon = DongDaChon();
            if (chon.Count == 0) return;

            using (SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = "PhieuNhap_DuBao_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx"
            })
            {
                if (sfd.ShowDialog(this.TopLevelControl ?? this) != DialogResult.OK) return;
                try
                {
                    int soPhieu = bll.XuatPhieuNhapExcel(chon, sfd.FileName, PhienDangNhap.MaNV);
                    if (MessageBox.Show(this, "Đã lập " + soPhieu + " phiếu nhập (mỗi nhà cung cấp 1 phiếu) cho " + chon.Count(k => k.DeXuatNhap > 0) + " thuốc.\n\n" +
                                        "Kiểm tra lại số lượng, đơn giá (ô tô vàng là ô còn thiếu), rồi vào trang Nhập hàng > Import phiếu nhập Excel.\n\nMở file ngay?",
                                        "Lập phiếu nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    GiaoDien.ThongBaoLoi(this, ex, "Lỗi lập phiếu nhập");
                }
            }
        }

        private void btnMoNhapHang_Click(object sender, EventArgs e)
        {
            if (YeuCauNhapHang != null) YeuCauNhapHang(this, EventArgs.Empty);
        }
    }
}
