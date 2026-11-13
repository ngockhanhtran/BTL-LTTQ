using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] Danh mục thuốc (quản lý): thêm / sửa / xóa thuốc, mã vạch, công dụng.
    /// - Giá bán tự tính = 110% giá nhập (yêu cầu 3).
    /// - SL hiện có KHÔNG sửa tay: thuốc mới = 0, tồn kho chỉ thay đổi qua Nhập hàng / Bán hàng (yêu cầu 1).
    /// - Thuốc đã có trong hóa đơn / phiếu nhập không xóa được (giữ lịch sử).
    /// - Lưới: thuốc hết hạn tô đỏ, sắp hết hàng (< 30) tô vàng.
    /// </summary>
    /// [10/11/2026 lần 5] Giao diện thiết kế bằng Designer (frmQuanLyThuoc.Designer.cs). Thêm: cờ thuốc kê đơn,
    /// nút "Xem các lô", NSX / HSD của thuốc đã có lô chỉ xem (lấy theo lô gần hết hạn nhất).
    public partial class frmQuanLyThuoc : Form
    {
        private readonly ThuocBLL thuocBLL = new ThuocBLL();
        private readonly DanhMucBLL dmBLL = new DanhMucBLL();

        private bool dangThemMoi;
        private List<string> congDungDangChon = new List<string>();
        private Dictionary<string, string> tenCongDung = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, string> TEN_COT = new Dictionary<string, string>
        {
            { "MaThuoc", "Mã thuốc" }, { "TenThuoc", "Tên thuốc" }, { "MaVach", "Mã vạch" }, { "TenDVT", "ĐVT" },
            { "TenDangDC", "Dạng ĐC" }, { "ThanhPhan", "Thành phần" }, { "DonGiaNhap", "Giá nhập" },
            { "GiaBan", "Giá bán" }, { "SLHienCo", "SL hiện có" }, { "NgaySX", "Ngày SX" }, { "HanSD", "Hạn SD" },
            { "TenNuocSX", "Nước SX" }, { "ChongChiDinh", "Chống chỉ định" }, { "CongDung", "Công dụng" },
            { "SLBanDuoc", "SL bán được" }, { "SoLo", "Số lô" }, { "KeDon", "Kê đơn" }     // [10/11/2026 lần 5]
        };
        private static readonly string[] COT_AN = { "MaDVT", "MaDangDC", "MaNuocSX" };

        public frmQuanLyThuoc()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            foreach (Button b in new[] { btnTim, btnXemLo, btnChonCD, btnThem, btnLuu, btnXoa, btnLamMoi }) b.Tag = b.BackColor;
            lblChuThich.Text = "■ Đỏ: đã hết hạn    ■ Vàng: sắp hết hàng (< " + BaoCaoBLL.NGUONG_SAP_HET_HANG + ")    ℞: thuốc kê đơn";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            NapCombo();
            TaiDanhSach(null);
        }

        // ================================================================
        // SỰ KIỆN GẮN TỪ DESIGNER
        // ================================================================
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TaiDanhSach(null); }
        }

        private void btnTim_Click(object sender, EventArgs e) { TaiDanhSach(null); }

        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            GiaoDien.DinhDangCot(dgv, TEN_COT, COT_AN, new[] { "DonGiaNhap", "GiaBan" }, new[] { "NgaySX", "HanSD" });
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (!dangThemMoi) HienThiDongDangChon();
        }

        private void nudGiaNhap_ValueChanged(object sender, EventArgs e)
        {
            txtGiaBan.Text = (nudGiaNhap.Value * 1.1m).ToString("N0") + " đ";
        }

        private void btnChonCD_Click(object sender, EventArgs e) { ChonCongDung(); }
        private void btnThem_Click(object sender, EventArgs e) { BatDauThemMoi(); }
        private void btnLuu_Click(object sender, EventArgs e) { Luu(); }
        private void btnXoa_Click(object sender, EventArgs e) { Xoa(); }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            NapCombo();
            TaiDanhSach(null);
        }

        // [10/11/2026 lần 5]
        private void btnXemLo_Click(object sender, EventArgs e)
        {
            string ma = txtMaThuoc.Text.Trim();
            if (dangThemMoi || ma == "") { MessageBox.Show(this, "Chọn 1 thuốc trong danh sách."); return; }
            using (frmLoThuoc f = new frmLoThuoc(ma, txtTenThuoc.Text))
                f.ShowDialog(this.TopLevelControl ?? this);
        }

        // NSX / HSD chỉ sửa tay khi thuốc chưa có lô nào còn hàng
        private void DatSuaNgay(bool choSua)
        {
            dtpNgaySX.Enabled = choSua;
            dtpHanSD.Enabled = choSua;
            lblGhiChuLo.Visible = !choSua;
        }

        // ================================================================
        // DỮ LIỆU
        // ================================================================
        private void NapCombo()
        {
            try
            {
                GanCombo(cboDVT, dmBLL.LayDanhSachChon(DanhSachDanhMuc.DonViTinh()));
                GanCombo(cboDangDC, dmBLL.LayDanhSachChon(DanhSachDanhMuc.DangDieuChe()));
                GanCombo(cboNuocSX, dmBLL.LayDanhSachChon(DanhSachDanhMuc.NuocSX()));
                NapTenCongDung();
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải danh mục"); }
        }

        private static void GanCombo(ComboBox cbo, DataTable dt)
        {
            cbo.DisplayMember = "Ten";
            cbo.ValueMember = "Ma";
            cbo.DataSource = dt;
            cbo.SelectedIndex = -1;
        }

        private void NapTenCongDung()
        {
            tenCongDung.Clear();
            foreach (DataRow r in thuocBLL.LayTatCaCongDung().Rows)
                tenCongDung[r["MaCongDung"].ToString().Trim()] = r["TenCongDung"].ToString();
        }

        private void TaiDanhSach(string maChon)
        {
            try
            {
                if (maChon == null && dgv.CurrentRow != null) maChon = GiaoDien.GiaTri(dgv.CurrentRow, "MaThuoc");
                dangThemMoi = false;
                dgv.DataSource = thuocBLL.LayDanhSachQuanLy(txtTimKiem.Text);
                GiaoDien.ChonDong(dgv, "MaThuoc", maChon);
                HienThiDongDangChon();
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải danh sách thuốc"); }
        }

        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow r = dgv.Rows[e.RowIndex];
            DateTime hsd;
            int sl;
            bool hetHan = DateTime.TryParse(GiaoDien.GiaTri(r, "HanSD"), out hsd) && hsd.Date < DateTime.Today;
            bool sapHet = int.TryParse(GiaoDien.GiaTri(r, "SLHienCo"), out sl) && sl < BaoCaoBLL.NGUONG_SAP_HET_HANG;
            if (hetHan)
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 199, 206);
                e.CellStyle.ForeColor = Color.FromArgb(156, 0, 6);
            }
            else if (sapHet)
            {
                e.CellStyle.BackColor = GiaoDien.VangNen;
                e.CellStyle.ForeColor = GiaoDien.VangChu;
            }
        }

        private void HienThiDongDangChon()
        {
            DataGridViewRow r = dgv.CurrentRow;
            if (r == null)
            {
                XoaO();
                lblCheDo.Text = "Chưa có thuốc – bấm \"Thêm mới\".";
                GiaoDien.BatTatNut(btnXoa, false);
                return;
            }

            txtMaThuoc.Text = GiaoDien.GiaTri(r, "MaThuoc").Trim();
            txtTenThuoc.Text = GiaoDien.GiaTri(r, "TenThuoc");
            txtMaVach.Text = GiaoDien.GiaTri(r, "MaVach");
            GiaoDien.ChonCombo(cboDVT, GiaoDien.GiaTri(r, "MaDVT"));
            GiaoDien.ChonCombo(cboDangDC, GiaoDien.GiaTri(r, "MaDangDC"));
            GiaoDien.ChonCombo(cboNuocSX, GiaoDien.GiaTri(r, "MaNuocSX"));
            txtThanhPhan.Text = GiaoDien.GiaTri(r, "ThanhPhan");
            decimal gia;
            nudGiaNhap.Value = decimal.TryParse(GiaoDien.GiaTri(r, "DonGiaNhap"), out gia) ? Math.Min(Math.Max(gia, 0), nudGiaNhap.Maximum) : 0;
            txtGiaBan.Text = (decimal.TryParse(GiaoDien.GiaTri(r, "GiaBan"), out gia) ? gia : nudGiaNhap.Value * 1.1m).ToString("N0") + " đ";
            txtSLHienCo.Text = GiaoDien.GiaTri(r, "SLHienCo");
            DatNgay(dtpNgaySX, GiaoDien.GiaTri(r, "NgaySX"));
            DatNgay(dtpHanSD, GiaoDien.GiaTri(r, "HanSD"));
            txtChongChiDinh.Text = GiaoDien.GiaTri(r, "ChongChiDinh");
            txtCongDung.Text = GiaoDien.GiaTri(r, "CongDung");
            chkKeDon.Checked = GiaoDien.GiaTri(r, "KeDon") == "True";                     // [10/11/2026 lần 5]
            int soLo;
            DatSuaNgay(!(int.TryParse(GiaoDien.GiaTri(r, "SoLo"), out soLo) && soLo > 0));
            string slBan = GiaoDien.GiaTri(r, "SLBanDuoc");
            if (slBan != "" && slBan != txtSLHienCo.Text) txtSLHienCo.Text += " (" + slBan + ")";
            try { congDungDangChon = thuocBLL.LayMaCongDungCuaThuoc(txtMaThuoc.Text); }
            catch { congDungDangChon = new List<string>(); }

            GiaoDien.DatChiDoc(txtMaThuoc, true);
            lblCheDo.Text = "Đang sửa: " + txtMaThuoc.Text;
            GiaoDien.BatTatNut(btnXoa, true);
        }

        private static void DatNgay(DateTimePicker dtp, string giaTri)
        {
            DateTime d;
            if (DateTime.TryParse(giaTri, out d)) { dtp.Value = d; dtp.Checked = true; }
            else { dtp.Value = DateTime.Today; dtp.Checked = false; }
        }

        private void XoaO()
        {
            txtMaThuoc.Clear(); txtTenThuoc.Clear(); txtMaVach.Clear(); txtThanhPhan.Clear();
            txtChongChiDinh.Clear(); txtCongDung.Clear(); txtSLHienCo.Text = "0";
            cboDVT.SelectedIndex = -1; cboDangDC.SelectedIndex = -1; cboNuocSX.SelectedIndex = -1;
            nudGiaNhap.Value = 0;
            txtGiaBan.Text = "0 đ";
            DatNgay(dtpNgaySX, null);
            DatNgay(dtpHanSD, null);
            congDungDangChon = new List<string>();
            chkKeDon.Checked = false;
            DatSuaNgay(true);
        }

        private void BatDauThemMoi()
        {
            dangThemMoi = true;
            XoaO();
            GiaoDien.DatChiDoc(txtMaThuoc, false);
            try { txtMaThuoc.Text = thuocBLL.SinhMaThuocMoi(); } catch { /* không sinh được thì tự nhập */ }
            lblCheDo.Text = "Đang THÊM MỚI thuốc – điền thông tin rồi bấm Lưu";
            GiaoDien.BatTatNut(btnXoa, false);
            txtTenThuoc.Focus();
        }

        private void ChonCongDung()
        {
            string ten = (txtMaThuoc.Text + " - " + txtTenThuoc.Text).Trim(' ', '-');
            using (frmChonCongDung f = new frmChonCongDung(ten, congDungDangChon))
            {
                if (f.ShowDialog(this.TopLevelControl ?? this) != DialogResult.OK) return;
                congDungDangChon = f.DaChon;
            }
            try { NapTenCongDung(); } catch { }
            txtCongDung.Text = string.Join(", ", congDungDangChon.Select(m => tenCongDung.ContainsKey(m) ? tenCongDung[m] : m));
            lblCheDo.Text = (dangThemMoi ? "Đang THÊM MỚI" : "Đang sửa: " + txtMaThuoc.Text) + " – công dụng sẽ được lưu khi bấm Lưu";
        }

        private ThuocDTO DocForm()
        {
            ThuocDTO t = new ThuocDTO
            {
                MaThuoc = txtMaThuoc.Text.Trim(),
                TenThuoc = txtTenThuoc.Text.Trim(),
                MaVach = txtMaVach.Text.Trim(),
                MaDVT = GiaoDien.GiaTriCombo(cboDVT),
                MaDangDC = GiaoDien.GiaTriCombo(cboDangDC),
                ThanhPhan = txtThanhPhan.Text.Trim(),
                DonGiaNhap = nudGiaNhap.Value,
                NgaySX = dtpNgaySX.Checked ? dtpNgaySX.Value.Date : (DateTime?)null,
                HanSD = dtpHanSD.Checked ? dtpHanSD.Value.Date : (DateTime?)null,
                MaNuocSX = GiaoDien.GiaTriCombo(cboNuocSX),
                ChongChiDinh = txtChongChiDinh.Text.Trim(),
                KeDon = chkKeDon.Checked                    // [10/11/2026 lần 5]
            };
            t.CongDung.AddRange(congDungDangChon);
            return t;
        }

        private void Luu()
        {
            if (!dangThemMoi && dgv.CurrentRow == null)
            {
                MessageBox.Show(this, "Chọn 1 thuốc để sửa hoặc bấm \"Thêm mới\".", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ThuocDTO t = DocForm();
            try
            {
                if (dangThemMoi)
                {
                    thuocBLL.Them(t);
                    MessageBox.Show(this, "Đã thêm thuốc " + t.MaThuoc + " (tồn kho = 0).\nDùng trang Nhập hàng để nhập số lượng.",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    thuocBLL.Sua(t);
                    MessageBox.Show(this, "Đã lưu thuốc " + t.MaThuoc + ".", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                TaiDanhSach(t.MaThuoc);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi lưu thuốc"); }
        }

        private void Xoa()
        {
            if (dangThemMoi || dgv.CurrentRow == null) return;
            string ma = GiaoDien.GiaTri(dgv.CurrentRow, "MaThuoc").Trim();
            string ten = GiaoDien.GiaTri(dgv.CurrentRow, "TenThuoc");
            if (MessageBox.Show(this, "Xóa thuốc " + ma + " - " + ten + "?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            try
            {
                if (thuocBLL.Xoa(ma)) TaiDanhSach(null);
                else MessageBox.Show(this, "Thuốc " + ma + " đã có trong hóa đơn bán / phiếu nhập / lịch sử hủy nên không thể xóa (cần giữ lịch sử).",
                                     "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi xóa thuốc"); }
        }
    }
}
