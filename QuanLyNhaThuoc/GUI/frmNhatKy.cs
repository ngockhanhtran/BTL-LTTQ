using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026 lần 5] Nhật ký hoạt động (chỉ quản lý) - giao diện thiết kế bằng Designer.
    /// Ai đăng nhập (đúng / sai), ai sửa giá, thanh toán, trả hàng, nhập hàng, sao lưu... lọc theo ngày / NV / hành động.
    /// </summary>
    public partial class frmNhatKy : Form
    {
        private const string TAT_CA = "(Tất cả)";
        private bool dangNap;

        private static readonly Dictionary<string, string> TEN_COT = new Dictionary<string, string>
        {
            { "ThoiGian", "Thời gian" }, { "MaNV", "Mã NV" }, { "TenNV", "Nhân viên" }, { "HanhDong", "Hành động" },
            { "DoiTuong", "Đối tượng" }, { "ChiTiet", "Chi tiết" }, { "MayTinh", "Máy tính" }
        };

        public frmNhatKy()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            foreach (Button b in new[] { btnLoc, btnHomNay, btnXuatExcel }) b.Tag = b.BackColor;
            dtpTu.Value = DateTime.Today.AddDays(-7);
            dtpDen.Value = DateTime.Today;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            NapBoLoc();
            TaiNhatKy();
        }

        private void NapBoLoc()
        {
            dangNap = true;
            try
            {
                DataTable nv = NhatKyBLL.LayDanhSachNhanVien();
                DataRow tatCa = nv.NewRow();
                tatCa["MaNV"] = "";
                tatCa["HienThi"] = TAT_CA;
                nv.Rows.InsertAt(tatCa, 0);
                cboNhanVien.DisplayMember = "HienThi";
                cboNhanVien.ValueMember = "MaNV";
                cboNhanVien.DataSource = nv;

                cboHanhDong.Items.Clear();
                cboHanhDong.Items.Add(TAT_CA);
                foreach (DataRow r in NhatKyBLL.LayDanhSachHanhDong().Rows) cboHanhDong.Items.Add(r[0].ToString());
                cboHanhDong.SelectedIndex = 0;
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải bộ lọc"); }
            finally { dangNap = false; }
        }

        private void TaiNhatKy()
        {
            if (dangNap) return;
            try
            {
                string maNV = cboNhanVien.SelectedValue == null ? "" : cboNhanVien.SelectedValue.ToString();
                string hanhDong = cboHanhDong.SelectedIndex <= 0 ? "" : cboHanhDong.Text;
                DataTable dt = NhatKyBLL.LayDanhSach(dtpTu.Value, dtpDen.Value, maNV, hanhDong, txtTuKhoa.Text);
                dgvNhatKy.DataSource = dt;
                lblTong.Text = dt.Rows.Count >= 2000
                    ? "Hiển thị 2.000 dòng mới nhất - hãy thu hẹp khoảng ngày để xem hết."
                    : "Có " + dt.Rows.Count.ToString("N0") + " dòng nhật ký.";
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải nhật ký"); }
        }

        private void btnLoc_Click(object sender, EventArgs e) { TaiNhatKy(); }

        private void btnHomNay_Click(object sender, EventArgs e)
        {
            dtpTu.Value = DateTime.Today;
            dtpDen.Value = DateTime.Today;
            TaiNhatKy();
        }

        private void txtTuKhoa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TaiNhatKy(); }
        }

        private void dgvNhatKy_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            GiaoDien.DinhDangCot(dgvNhatKy, TEN_COT, new[] { "MaNK" }, null, null);
            if (dgvNhatKy.Columns.Contains("ThoiGian"))
            {
                dgvNhatKy.Columns["ThoiGian"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                dgvNhatKy.Columns["ThoiGian"].FillWeight = 60;
            }
            if (dgvNhatKy.Columns.Contains("ChiTiet")) dgvNhatKy.Columns["ChiTiet"].FillWeight = 220;
        }

        // Tô màu: đăng nhập sai (đỏ), đổi giá (vàng), hủy / trả hàng (cam)
        private void dgvNhatKy_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string hd = GiaoDien.GiaTri(dgvNhatKy.Rows[e.RowIndex], "HanhDong");
            if (hd == NhatKyBLL.DANG_NHAP_SAI) { e.CellStyle.ForeColor = GiaoDien.MauDo; }
            else if (hd == "Đổi giá thuốc") { e.CellStyle.BackColor = GiaoDien.VangNen; }
            else if (hd == NhatKyBLL.HUY_HOA_DON || hd == NhatKyBLL.TRA_HANG || hd == NhatKyBLL.KHOI_PHUC)
            { e.CellStyle.ForeColor = GiaoDien.MauCam; }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dgvNhatKy.Rows.Count == 0) { MessageBox.Show(this, "Không có dữ liệu để xuất."); return; }
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel (*.xlsx)|*.xlsx", FileName = "NhatKy_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx" })
            {
                if (sfd.ShowDialog(this.TopLevelControl ?? this) != DialogResult.OK) return;
                try
                {
                    XuatExcel.TuDataGridView(dgvNhatKy, sfd.FileName, "NHẬT KÝ HOẠT ĐỘNG",
                        "Từ " + dtpTu.Value.ToString("dd/MM/yyyy") + " đến " + dtpDen.Value.ToString("dd/MM/yyyy"), lblTong.Text);
                    MessageBox.Show(this, "Đã xuất file Excel.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi xuất Excel"); }
            }
        }
    }
}
