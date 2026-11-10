using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    // Giao diện thiết kế bằng Designer (frmMain.Designer.cs).
    // [10/11/2026] Menu theo vai trò: mọi nút được đặt sẵn trong Designer, khi chạy ẩn / hiện theo người đăng nhập:
    //   Bán hàng : Tra cứu thuốc, Bán hàng
    //   Thủ kho  : Tra cứu thuốc (sửa công dụng / chống chỉ định), Nhập hàng
    //   Quản lý  : tất cả + Dashboard, Báo cáo, Quản lý nhân viên, Danh mục
    public partial class frmMain : Form
    {
        // Giữ 1 bản duy nhất của 2 trang này để hóa đơn đang lập không bị mất khi chuyển trang
        private frmThuoc fThuoc;
        private frmBanHang fBanHang;

        private readonly NhanVienBLL nvBLL = new NhanVienBLL();
        private Button btnDangChon;
        private bool laQuanLy;                    // có quyền quản lý (thấy nhóm QUẢN TRỊ / DANH MỤC)
        private static readonly Color MAU_MENU = Color.FromArgb(189, 195, 199);
        private static readonly Color MAU_MENU_CHON = Color.FromArgb(41, 128, 185);

        /// <summary>[10/11/2026] true khi người dùng bấm "Đăng xuất" (Program sẽ mở lại form đăng nhập).</summary>
        public bool DangXuat { get; private set; }

        public frmMain()
        {
            InitializeComponent();
            HienThongTinDangNhap();
            ApDungPhanQuyen();
        }

        // ================== THÔNG TIN NGƯỜI DÙNG + PHÂN QUYỀN ==================

        private void HienThongTinDangNhap()
        {
            this.Text = "Hệ Thống Quản Lý Nhà Thuốc - " + (PhienDangNhap.TenNV ?? "") +
                        (string.IsNullOrEmpty(PhienDangNhap.TenVaiTro) ? "" : " (" + PhienDangNhap.TenVaiTro + ")");
            lblUser.Text = "Xin chào, " + (PhienDangNhap.TenNV ?? "Dược sĩ");
            lblTen.Text = "👤 " + (PhienDangNhap.TenNV ?? "(chưa đăng nhập)");
            lblVaiTro.Text = (PhienDangNhap.MaNV ?? "") + "  •  " + (PhienDangNhap.TenVaiTro ?? "");
        }

        private void ApDungPhanQuyen()
        {
            laQuanLy = PhienDangNhap.LaQuanLy || !PhienDangNhap.DaDangNhap;

            btnBanHang.Visible = PhienDangNhap.DuocBanHang;
            btnTraHang.Visible = PhienDangNhap.DuocBanHang;          // [10/11/2026 lần 5]

            bool nhapHang = PhienDangNhap.DuocNhapHang;
            lblNhomKho.Visible = nhapHang;
            btnNhapHang.Visible = nhapHang;
            btnLoThuoc.Visible = nhapHang;                           // [10/11/2026 lần 5]

            foreach (Control c in new Control[] { lblNhomQuanTri, btnDashboard, btnBaoCao, btnQuanLyNV, btnNhatKy, btnSaoLuu,
                                                  lblNhomDanhMuc, btnDMThuoc, btnKhachHang, btnNhaCungCap,
                                                  btnCongDung, btnDanhMucKhac })
                c.Visible = laQuanLy;
        }

        // Tô sáng nút đang chọn trên sidebar
        private void ChonNut(Button btn)
        {
            if (btnDangChon != null)
            {
                btnDangChon.ResetBackColor();   // về màu nền sidebar
                btnDangChon.ForeColor = MAU_MENU;
            }
            btnDangChon = btn;
            if (btn != null)
            {
                btn.BackColor = MAU_MENU_CHON;
                btn.ForeColor = Color.White;
            }
        }

        private void MoTrang(Button nut, Form trang)
        {
            ChonNut(nut);
            OpenChildForm(trang);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Không để form (sau khi scale DPI) vượt quá kích thước màn hình
            Rectangle wa = Screen.FromControl(this).WorkingArea;
            if (Width > wa.Width || Height > wa.Height)
            {
                Width = Math.Min(Width, wa.Width);
                Height = Math.Min(Height, wa.Height);
                Location = new Point(wa.Left + (wa.Width - Width) / 2, wa.Top + (wa.Height - Height) / 2);
            }
            this.MinimumSize = new Size(Math.Min(1000, wa.Width), Math.Min(650, wa.Height));

            // Mở form con sau khi form chính đã scale xong
            MoTrang(btnTraCuu, LayFormThuoc());
        }

        // ================== SỰ KIỆN MENU ==================

        private void btnTraCuu_Click(object sender, EventArgs e) { MoTrang(btnTraCuu, LayFormThuoc()); }
        private void btnBanHang_Click(object sender, EventArgs e) { MoTrang(btnBanHang, LayFormBanHang()); }
        private void btnNhapHang_Click(object sender, EventArgs e) { MoTrang(btnNhapHang, new frmNhapHang()); }
        private void btnDashboard_Click(object sender, EventArgs e) { MoTrang(btnDashboard, new frmDashboard()); }
        private void btnBaoCao_Click(object sender, EventArgs e) { MoTrang(btnBaoCao, TaoFormBaoCao()); }
        private void btnQuanLyNV_Click(object sender, EventArgs e) { MoTrang(btnQuanLyNV, TaoFormQuanLyNV()); }
        private void btnDMThuoc_Click(object sender, EventArgs e) { MoTrang(btnDMThuoc, new frmQuanLyThuoc()); }
        private void btnKhachHang_Click(object sender, EventArgs e) { MoTrang(btnKhachHang, new frmDanhMuc(DanhSachDanhMuc.KhachHang())); }
        private void btnNhaCungCap_Click(object sender, EventArgs e) { MoTrang(btnNhaCungCap, new frmDanhMuc(DanhSachDanhMuc.NhaCungCap())); }
        private void btnCongDung_Click(object sender, EventArgs e) { MoTrang(btnCongDung, new frmDanhMuc(DanhSachDanhMuc.CongDung())); }

        // [10/11/2026 lần 5] các trang mới
        private void btnTraHang_Click(object sender, EventArgs e) { MoTrang(btnTraHang, new frmTraHang()); }
        private void btnLoThuoc_Click(object sender, EventArgs e) { MoTrang(btnLoThuoc, new frmLoThuoc(null, null)); }
        private void btnNhatKy_Click(object sender, EventArgs e) { MoTrang(btnNhatKy, new frmNhatKy()); }
        private void btnSaoLuu_Click(object sender, EventArgs e) { MoTrang(btnSaoLuu, TaoFormSaoLuu()); }

        // Khôi phục CSDL xong -> dữ liệu đổi hoàn toàn -> đăng xuất (không hỏi lại)
        private Form TaoFormSaoLuu()
        {
            frmSaoLuu f = new frmSaoLuu();
            f.DaKhoiPhuc += (s, e) => BeginInvoke((Action)(() => { DangXuat = true; Close(); }));
            return f;
        }

        // Danh mục phụ: mở menu con bên phải nút
        private void btnDanhMucKhac_Click(object sender, EventArgs e)
        {
            menuKhac.Show(btnDanhMucKhac, new Point(btnDanhMucKhac.Width, 0));
        }

        private void mnuDonViTinh_Click(object sender, EventArgs e) { MoTrang(btnDanhMucKhac, new frmDanhMuc(DanhSachDanhMuc.DonViTinh())); }
        private void mnuDangDieuChe_Click(object sender, EventArgs e) { MoTrang(btnDanhMucKhac, new frmDanhMuc(DanhSachDanhMuc.DangDieuChe())); }
        private void mnuNuocSX_Click(object sender, EventArgs e) { MoTrang(btnDanhMucKhac, new frmDanhMuc(DanhSachDanhMuc.NuocSX())); }

        // Nhãn vàng "x nhân viên yêu cầu cấp lại mật khẩu" -> mở trang Quản lý nhân viên
        private void lblYeuCauMK_Click(object sender, EventArgs e)
        {
            if (laQuanLy) btnQuanLyNV.PerformClick();
        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            using (frmDoiMatKhau f = new frmDoiMatKhau()) f.ShowDialog(this);
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            XacNhanDangXuat();
        }

        // ================== NỐI TRANG TRA CỨU <-> BÁN HÀNG ==================

        private frmThuoc LayFormThuoc()
        {
            if (fThuoc == null || fThuoc.IsDisposed)
            {
                fThuoc = new frmThuoc();
                // Bấm "Thanh toán với hóa đơn" ở trang Tra cứu -> nạp hóa đơn sang trang Bán hàng và chuyển trang
                fThuoc.MoTrangBanHang = hd =>
                {
                    frmBanHang bh = LayFormBanHang();
                    if (!bh.NapHoaDon(hd)) return false;
                    MoTrang(btnBanHang, bh);
                    return true;
                };
            }
            return fThuoc;
        }

        private frmBanHang LayFormBanHang()
        {
            if (fBanHang == null || fBanHang.IsDisposed)
                fBanHang = new frmBanHang();
            return fBanHang;
        }

        // [10/11/2026] Báo cáo: nút "Nhập thêm hàng" chuyển thẳng sang trang Nhập hàng
        private Form TaoFormBaoCao()
        {
            frmBaoCao f = new frmBaoCao();
            f.YeuCauNhapHang += (s, e) =>
            {
                // Chuyển trang SAU khi xử lý xong cú click (frmBaoCao sẽ bị giải phóng khi chuyển trang)
                BeginInvoke((Action)(() => MoTrang(btnNhapHang, new frmNhapHang())));
            };
            return f;
        }

        // [10/11/2026] Quản lý NV: lưu / xóa xong thì cập nhật lại nhãn "quên mật khẩu"
        private Form TaoFormQuanLyNV()
        {
            frmQuanLyNhanVien f = new frmQuanLyNhanVien();
            f.DuLieuThayDoi += (s, e) => CapNhatYeuCauQuenMK();
            return f;
        }

        private void OpenChildForm(Form childForm)
        {
            // Gỡ form đang hiển thị: 2 trang Tra cứu / Bán hàng chỉ ẩn đi (giữ dữ liệu),
            // các trang khác (Nhập hàng, Báo cáo) thì giải phóng như trước.
            for (int i = pnlContainer.Controls.Count - 1; i >= 0; i--)
            {
                Control c = pnlContainer.Controls[i];
                if (c == childForm) continue;

                c.Hide();
                pnlContainer.Controls.RemoveAt(i);
                if (c != fThuoc && c != fBanHang) c.Dispose();
            }

            if (!pnlContainer.Controls.Contains(childForm))
            {
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                pnlContainer.Controls.Add(childForm);
            }
            childForm.Show();
            childForm.BringToFront();
            lblTitle.Text = childForm.Text;

            CapNhatYeuCauQuenMK();
        }

        // [10/11/2026] Quản lý: hiện số NV đang yêu cầu cấp lại mật khẩu
        private void CapNhatYeuCauQuenMK()
        {
            if (!laQuanLy) return;
            try
            {
                int n = nvBLL.DemYeuCauQuenMatKhau();
                lblYeuCauMK.Visible = n > 0;
                lblYeuCauMK.Text = "🔑 " + n + " nhân viên yêu cầu cấp lại mật khẩu";
                btnQuanLyNV.Text = n > 0 ? "👥 Quản lý Nhân viên  (" + n + ")" : "👥 Quản lý Nhân viên";
                if (btnQuanLyNV != btnDangChon)
                    btnQuanLyNV.ForeColor = n > 0 ? Color.Gold : MAU_MENU;
            }
            catch
            {
                lblYeuCauMK.Visible = false;
            }
        }

        // [10/11/2026] Đăng xuất: cảnh báo nếu còn hóa đơn đang lập dở
        private void XacNhanDangXuat()
        {
            string canhBao = (fBanHang != null && !fBanHang.IsDisposed && fBanHang.DangCoHoaDonChuaThanhToan)
                ? "Hóa đơn đang lập ở trang Bán hàng chưa thanh toán sẽ bị hủy.\n\n" : "";
            if (MessageBox.Show(this, canhBao + "Bạn có chắc muốn đăng xuất?", "Đăng xuất",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            DangXuat = true;
            Close();
        }
    }
}
