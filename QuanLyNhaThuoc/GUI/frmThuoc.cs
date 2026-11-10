using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    // Giao diện thiết kế bằng Designer (frmThuoc.Designer.cs).
    // [10/11/2026] Phân quyền:
    //   - Quản lý / Thủ kho: được sửa CÔNG DỤNG (nút "Sửa công dụng...") và CHỐNG CHỈ ĐỊNH (nút "Lưu chống chỉ định").
    //   - Bán hàng: chỉ xem (như trước).
    //   - Phần "Lập hóa đơn bán" (pnlHoaDon) chỉ hiện cho Quản lý / Bán hàng (thủ kho không bán hàng).
    //   - Không cho thêm thuốc đã HẾT HẠN vào hóa đơn.
    //   - Tiêu đề cột của lưới hiển thị tiếng Việt.
    // [10/11/2026 lần 5] Quản lý theo LÔ: chỉ bán số lượng CÒN HẠN (cột "SL bán được"), nút "Xem các lô";
    //   thuốc kê đơn (℞) được đánh dấu để trang Bán hàng yêu cầu mã đơn thuốc.
    public partial class frmThuoc : Form
    {
        private readonly ThuocBLL thuocBLL = new ThuocBLL();
        private readonly HoaDonBanBLL hdbBLL = new HoaDonBanBLL();

        // Thuốc đang chọn trên lưới
        private string maThuocChon, tenThuocChon;
        private decimal giaBanChon;
        private int slTonChon;
        private int slBanDuocChon;         // [10/11/2026 lần 5] SL các lô còn hạn
        private bool keDonChon;            // [10/11/2026 lần 5]
        private DateTime? hanSDChon;

        // Hóa đơn đang lập (chưa lưu DB cho tới khi thanh toán xong)
        private HoaDonTam hoaDonHienTai;

        // [10/11/2026] quyền của người đang đăng nhập
        private readonly bool choBanHang = PhienDangNhap.DuocBanHang;
        private readonly bool choSuaThongTin = PhienDangNhap.DuocSuaThongTinThuoc;

        // Form chính gán hàm này để chuyển sang trang bán hàng.
        // Trả về true nếu trang bán hàng đã nhận hóa đơn.
        // Nếu không gán, frmThuoc tự mở frmBanHang thành cửa sổ riêng.
        public Func<HoaDonTam, bool> MoTrangBanHang { get; set; }

        private static frmBanHang frmBanHangRieng;

        private static readonly Dictionary<string, string> TEN_COT = new Dictionary<string, string>
        {
            { "MaThuoc", "Mã thuốc" }, { "TenThuoc", "Tên thuốc" }, { "MaVach", "Mã vạch" },
            { "MaDVT", "ĐVT" }, { "MaDangDC", "Dạng ĐC" }, { "ThanhPhan", "Thành phần" },
            { "DonGiaNhap", "Giá nhập" }, { "GiaBan", "Giá bán" }, { "SLHienCo", "SL hiện có" },
            { "NgaySX", "Ngày SX" }, { "MaNuocSX", "Nước SX" }, { "HanSD", "Hạn SD" },
            { "ChongChiDinh", "Chống chỉ định" }, { "CongDung", "Công dụng" },
            { "SLBanDuoc", "SL bán được" }, { "KeDon", "Kê đơn" }      // [10/11/2026 lần 5]
        };

        public frmThuoc()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            ApDungPhanQuyen();
            LoadData();
            if (choBanHang) KhoiTaoHoaDonMoi();
        }

        // [10/11/2026] Ẩn / hiện theo vai trò (Designer đặt sẵn tất cả)
        private void ApDungPhanQuyen()
        {
            btnSuaCongDung.Visible = choSuaThongTin;
            btnLuuCCD.Visible = choSuaThongTin;
            if (choSuaThongTin)
            {
                GiaoDien.DatChiDoc(txtChongChiDinh, false);
                txtChongChiDinh.MaxLength = 255;
            }
            else
            {
                pnlHoaDon.Top -= 46;   // không có hàng nút sửa -> dời phần hóa đơn lên (giống bố cục cũ)
            }
            pnlHoaDon.Visible = choBanHang;
        }

        // Quay lại trang này thì nạp lại tồn kho mới nhất
        private void frmThuoc_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible) TaiLai(maThuocChon);
        }

        private void txtTuKhoa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TimKiem(); }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Clear();
            LoadData();
        }

        // [10/11/2026] tiêu đề cột tiếng Việt + định dạng tiền / ngày
        private void dgvThuoc_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            GiaoDien.DinhDangCot(dgvThuoc, TEN_COT, null, new[] { "DonGiaNhap", "GiaBan" }, new[] { "NgaySX", "HanSD" });
        }

        // ================== DỮ LIỆU THUỐC ==================

        private void LoadData()
        {
            try { dgvThuoc.DataSource = thuocBLL.LayDanhSachThuoc(); }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
        }

        private void TimKiem()
        {
            try { dgvThuoc.DataSource = thuocBLL.TimKiemThuoc(txtTuKhoa.Text.Trim()); }
            catch (Exception ex) { MessageBox.Show("Lỗi tra cứu: " + ex.Message); }
        }

        // [10/11/2026] Nạp lại (giữ từ khóa đang tìm) và chọn lại thuốc đang xem
        private void TaiLai(string maChon)
        {
            if (txtTuKhoa.Text.Trim() == "") LoadData(); else TimKiem();
            GiaoDien.ChonDong(dgvThuoc, "MaThuoc", maChon);
        }

        // Đọc giá trị ô theo tên cột; thử lần lượt các tên, cột nào có thì dùng
        private string Cell(DataGridViewRow row, params string[] tenCot)
        {
            foreach (string c in tenCot)
            {
                if (!dgvThuoc.Columns.Contains(c)) continue;
                object v = row.Cells[c].Value;
                return (v == null || v == DBNull.Value) ? "" : v.ToString();
            }
            return "";
        }

        private static string DinhDangTien(string giaTri)
        {
            decimal d;
            return decimal.TryParse(giaTri, out d) ? d.ToString("N0") + " đ" : giaTri;
        }

        private void DgvThuoc_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvThuoc.CurrentRow;
            if (row == null || row.Index < 0) { XoaChiTiet(); return; }

            maThuocChon = Cell(row, "MaThuoc").Trim();
            tenThuocChon = Cell(row, "TenThuoc");
            decimal.TryParse(Cell(row, "GiaBan"), out giaBanChon);
            int.TryParse(Cell(row, "SLHienCo"), out slTonChon);
            if (!int.TryParse(Cell(row, "SLBanDuoc"), out slBanDuocChon)) slBanDuocChon = slTonChon;
            keDonChon = Cell(row, "KeDon") == "True";
            DateTime hsd;
            hanSDChon = DateTime.TryParse(Cell(row, "HanSD"), out hsd) ? hsd : (DateTime?)null;

            txtMaThuoc.Text = maThuocChon;
            txtTenThuoc.Text = tenThuocChon;
            txtThanhPhan.Text = Cell(row, "ThanhPhan");
            txtGiaNhap.Text = DinhDangTien(Cell(row, "DonGiaNhap"));
            txtGiaBan.Text = DinhDangTien(Cell(row, "GiaBan"));
            txtSLHienCo.Text = slBanDuocChon == slTonChon ? slTonChon.ToString()
                             : slTonChon + " (bán được " + slBanDuocChon + ")";
            txtTenThuoc.Text = keDonChon ? "℞ " + tenThuocChon + " (kê đơn)" : tenThuocChon;
            // Tên cột công dụng trong CSDL có thể khác nhau -> thử vài tên thường gặp
            txtCongDung.Text = Cell(row, "CongDung", "SuDung", "CachDung", "CongDungThuoc");
            txtChongChiDinh.Text = Cell(row, "ChongChiDinh");
        }

        private void XoaChiTiet()
        {
            maThuocChon = null; tenThuocChon = null; giaBanChon = 0; slTonChon = 0; slBanDuocChon = 0; keDonChon = false; hanSDChon = null;
            txtMaThuoc.Clear(); txtTenThuoc.Clear(); txtThanhPhan.Clear(); txtGiaNhap.Clear();
            txtGiaBan.Clear(); txtSLHienCo.Clear(); txtCongDung.Clear(); txtChongChiDinh.Clear();
        }

        // ================== [10/11/2026] SỬA CÔNG DỤNG / CHỐNG CHỈ ĐỊNH ==================

        private void btnSuaCongDung_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maThuocChon))
            {
                MessageBox.Show("Vui lòng chọn một thuốc trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string ma = maThuocChon;
            try
            {
                List<string> daChon = thuocBLL.LayMaCongDungCuaThuoc(ma);
                using (frmChonCongDung f = new frmChonCongDung(ma + " - " + tenThuocChon, daChon))
                {
                    if (f.ShowDialog(this.TopLevelControl ?? this) != DialogResult.OK) return;
                    thuocBLL.CapNhatCongDung(ma, f.DaChon);
                }
                TaiLai(ma);
                MessageBox.Show("Đã cập nhật công dụng cho thuốc " + ma + ".", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex); }
        }

        private void btnLuuCCD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maThuocChon))
            {
                MessageBox.Show("Vui lòng chọn một thuốc trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string ma = maThuocChon;
            try
            {
                thuocBLL.CapNhatChongChiDinh(ma, txtChongChiDinh.Text);
                TaiLai(ma);
                MessageBox.Show("Đã lưu chống chỉ định cho thuốc " + ma + ".", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex); }
        }

        // [10/11/2026 lần 5] các lô của thuốc đang chọn (thứ tự bán FEFO)
        private void btnXemLo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maThuocChon))
            {
                MessageBox.Show("Vui lòng chọn một thuốc trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (frmLoThuoc f = new frmLoThuoc(maThuocChon, tenThuocChon))
                f.ShowDialog(this.TopLevelControl ?? this);
        }

        // ================== HÓA ĐƠN ==================

        private void KhoiTaoHoaDonMoi()
        {
            if (!choBanHang) return;
            try
            {
                hoaDonHienTai = new HoaDonTam(hdbBLL.SinhMaHoaDonMoi());
            }
            catch (Exception ex)
            {
                hoaDonHienTai = null;
                MessageBox.Show("Không sinh được mã hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            nudSoLuong.Value = 1;
            CapNhatNhanHoaDon();
        }

        private void CapNhatNhanHoaDon()
        {
            if (!choBanHang) return;
            if (hoaDonHienTai == null)
            {
                lblMaHDB.Text = "Mã hóa đơn: (chưa sinh được)";
                lblGioHang.Text = "";
                return;
            }
            lblMaHDB.Text = "Mã hóa đơn: " + hoaDonHienTai.MaHDB;
            lblGioHang.Text = hoaDonHienTai.Dong.Count == 0
                ? "Hóa đơn chưa có thuốc nào."
                : $"Đã có {hoaDonHienTai.Dong.Count} loại thuốc – tạm tính {hoaDonHienTai.TongTienHang:N0} đ";
        }

        private void btnThemVaoHD_Click(object sender, EventArgs e)
        {
            if (hoaDonHienTai == null) KhoiTaoHoaDonMoi();
            if (hoaDonHienTai == null) return;

            if (string.IsNullOrEmpty(maThuocChon))
            {
                MessageBox.Show("Vui lòng chọn một thuốc trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // [10/11/2026 lần 5] không bán lô đã hết hạn: chỉ còn lô hết hạn thì chặn
            if (slBanDuocChon <= 0 && slTonChon > 0)
            {
                MessageBox.Show($"Thuốc \"{tenThuocChon}\" chỉ còn lô đã HẾT HẠN, không được bán.\nBấm \"Xem các lô\" để kiểm tra.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (giaBanChon <= 0)
            {
                MessageBox.Show("Thuốc này chưa có giá bán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuong = (int)nudSoLuong.Value;
            int daCo = hoaDonHienTai.SoLuongCua(maThuocChon);
            if (daCo + soLuong > slBanDuocChon)
            {
                MessageBox.Show($"Không đủ hàng còn hạn! Còn {slBanDuocChon}, hóa đơn đã có {daCo}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            hoaDonHienTai.ThemThuoc(maThuocChon, tenThuocChon, soLuong, giaBanChon, keDonChon);
            nudSoLuong.Value = 1;
            CapNhatNhanHoaDon();
        }

        private void btnThanhToanHD_Click(object sender, EventArgs e)
        {
            if (hoaDonHienTai == null || hoaDonHienTai.Dong.Count == 0)
            {
                MessageBox.Show("Hóa đơn chưa có thuốc nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool daNhan;
            if (MoTrangBanHang != null)
            {
                daNhan = MoTrangBanHang(hoaDonHienTai);
            }
            else
            {
                if (frmBanHangRieng == null || frmBanHangRieng.IsDisposed)
                    frmBanHangRieng = new frmBanHang() { Size = new Size(1100, 650), StartPosition = FormStartPosition.CenterScreen };
                daNhan = frmBanHangRieng.NapHoaDon(hoaDonHienTai);
                if (daNhan) { frmBanHangRieng.Show(); frmBanHangRieng.Activate(); }
            }

            // Hóa đơn đã chuyển đi -> mã hóa đơn tăng lên 1 cho đơn tiếp theo
            if (daNhan) KhoiTaoHoaDonMoi();
        }
    }
}
