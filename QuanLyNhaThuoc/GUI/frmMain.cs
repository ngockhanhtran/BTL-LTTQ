using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    public partial class frmMain : Form
    {
        private Panel pnlSidebar;
        private Panel pnlHeader;
        private Panel pnlContainer;
        private FlowLayoutPanel pnlMenu;          // [10/11/2026] menu cuộn được, dựng theo vai trò
        private Label lblTitle;
        private Label lblUser;
        private Label lblYeuCauMK;                // [10/11/2026] báo yêu cầu quên mật khẩu (quản lý)

        // Giữ 1 bản duy nhất của 2 trang này để hóa đơn đang lập không bị mất khi chuyển trang
        private frmThuoc fThuoc;
        private frmBanHang fBanHang;

        // [10/11/2026]
        private readonly NhanVienBLL nvBLL = new NhanVienBLL();
        private Button btnDangChon, btnTraCuu, btnNhapHang, btnQuanLyNV;
        private static readonly Color MAU_MENU = Color.FromArgb(189, 195, 199);
        private static readonly Color MAU_MENU_CHON = Color.FromArgb(41, 128, 185);

        /// <summary>[10/11/2026] true khi người dùng bấm "Đăng xuất" (Program sẽ mở lại form đăng nhập).</summary>
        public bool DangXuat { get; private set; }

        public frmMain()
        {
            // Scale toàn bộ form theo DPI màn hình (thiết kế ở 96 DPI = 100%)
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            InitializeComponentCustom();
            this.Load += FrmMain_Load;
        }

        private void InitializeComponentCustom()
        {
            this.SuspendLayout();

            this.Text = "Hệ Thống Quản Lý Nhà Thuốc - " + (PhienDangNhap.TenNV ?? "") +
                        (string.IsNullOrEmpty(PhienDangNhap.TenVaiTro) ? "" : " (" + PhienDangNhap.TenVaiTro + ")");
            this.Size = new Size(1280, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(244, 246, 249);

            // ================== 1. SIDEBAR BÊN TRÁI ==================
            pnlSidebar = new Panel()
            {
                Dock = DockStyle.Left,
                Width = 240,
                BackColor = Color.FromArgb(20, 30, 48)
            };

            Label lblLogo = new Label()
            {
                Text = "💊 Nhà thuốc 1",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 70,
                Padding = new Padding(20, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlMenu = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(12, 0, 0, 10)
            };

            Panel pnlTaiKhoan = TaoKhungTaiKhoan();

            // Thêm Fill trước -> dock sau cùng (nằm giữa logo và khung tài khoản)
            pnlSidebar.Controls.Add(pnlMenu);
            pnlSidebar.Controls.Add(pnlTaiKhoan);
            pnlSidebar.Controls.Add(lblLogo);

            TaoMenuTheoVaiTro();

            // ================== 2. HEADER PHÍA TRÊN ==================
            pnlHeader = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White
            };

            lblTitle = new Label()
            {
                Text = "Quản lý danh mục Thuốc",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Dock = DockStyle.Left,
                AutoSize = true,
                Padding = new Padding(25, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };

            lblUser = new Label()
            {
                Text = "Xin chào, " + (PhienDangNhap.TenNV ?? "Dược sĩ"),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Dock = DockStyle.Right,
                AutoSize = true,
                Padding = new Padding(0, 0, 30, 0),
                TextAlign = ContentAlignment.MiddleRight
            };

            // [10/11/2026] Nhãn vàng báo có NV quên mật khẩu (chỉ quản lý thấy), bấm vào -> trang Quản lý NV
            lblYeuCauMK = new Label()
            {
                Dock = DockStyle.Right,
                AutoSize = false,
                Width = 290,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                BackColor = GiaoDien.VangNen,
                ForeColor = GiaoDien.VangChu,
                Cursor = Cursors.Hand,
                Visible = false
            };
            lblYeuCauMK.Click += (s, e) => { if (btnQuanLyNV != null) btnQuanLyNV.PerformClick(); };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblYeuCauMK);
            pnlHeader.Controls.Add(lblUser);

            // ================== 3. CONTAINER HIỂN THỊ FORM CON ==================
            pnlContainer = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(244, 246, 249),
                Padding = new Padding(20)
            };

            // Thứ tự Dock: control thêm SAU được dock TRƯỚC.
            // => Sidebar chiếm trọn bên trái, Header nằm trên phần còn lại, Container lấp đầy phần còn lại.
            this.Controls.Add(pnlContainer);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlSidebar);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // [10/11/2026] Khung dưới sidebar: tên, vai trò, nút Đổi mật khẩu, Đăng xuất
        private Panel TaoKhungTaiKhoan()
        {
            Panel p = new Panel() { Dock = DockStyle.Bottom, Height = 132, BackColor = Color.FromArgb(14, 22, 36) };

            Label lblTen = new Label()
            {
                Text = "👤 " + (PhienDangNhap.TenNV ?? "(chưa đăng nhập)"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Left = 15,
                Top = 10,
                Width = 215,
                Height = 22,
                AutoEllipsis = true
            };
            Label lblVaiTro = new Label()
            {
                Text = (PhienDangNhap.MaNV ?? "") + "  •  " + (PhienDangNhap.TenVaiTro ?? ""),
                ForeColor = Color.FromArgb(149, 165, 166),
                Font = new Font("Segoe UI", 9F),
                Left = 15,
                Top = 33,
                Width = 215,
                Height = 20
            };
            Button btnDoiMK = TaoNutTaiKhoan("🔑  Đổi mật khẩu", 58);
            btnDoiMK.Click += (s, e) =>
            {
                using (frmDoiMatKhau f = new frmDoiMatKhau()) f.ShowDialog(this);
            };
            Button btnDangXuat = TaoNutTaiKhoan("⎋  Đăng xuất", 93);
            btnDangXuat.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 57, 43);
            btnDangXuat.Click += (s, e) => XacNhanDangXuat();

            p.Controls.AddRange(new Control[] { lblTen, lblVaiTro, btnDoiMK, btnDangXuat });
            return p;
        }

        private Button TaoNutTaiKhoan(string text, int top)
        {
            Button b = new Button()
            {
                Text = text,
                Left = 12,
                Top = top,
                Width = 216,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                ForeColor = MAU_MENU,
                Font = new Font("Segoe UI", 9.5F),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = MAU_MENU_CHON;
            return b;
        }

        // [10/11/2026] Menu theo vai trò:
        //   Bán hàng : Tra cứu thuốc, Bán hàng
        //   Thủ kho  : Tra cứu thuốc (sửa công dụng / chống chỉ định), Nhập hàng
        //   Quản lý  : tất cả + Dashboard, Báo cáo, Quản lý nhân viên, Danh mục
        private void TaoMenuTheoVaiTro()
        {
            bool ql = PhienDangNhap.LaQuanLy || !PhienDangNhap.DaDangNhap;

            ThemNhomMenu("CỬA HÀNG");
            btnTraCuu = ThemMucMenu("📦 Tra cứu Thuốc", () => LayFormThuoc());
            if (PhienDangNhap.DuocBanHang)
                ThemMucMenu("🛒 Bán hàng (POS)", () => LayFormBanHang());

            if (PhienDangNhap.DuocNhapHang)
            {
                ThemNhomMenu("KHO");
                btnNhapHang = ThemMucMenu("📥 Quản lý Nhập Hàng", () => new frmNhapHang());
            }

            if (ql)
            {
                ThemNhomMenu("QUẢN TRỊ");
                ThemMucMenu("📈 Dashboard", () => new frmDashboard());
                ThemMucMenu("📊 Báo cáo và Thống kê", () => TaoFormBaoCao());
                btnQuanLyNV = ThemMucMenu("👥 Quản lý Nhân viên", () => TaoFormQuanLyNV());

                ThemNhomMenu("DANH MỤC");
                ThemMucMenu("💊 Danh mục Thuốc", () => new frmQuanLyThuoc());
                ThemMucMenu("🧑 Khách hàng", () => new frmDanhMuc(DanhSachDanhMuc.KhachHang()));
                ThemMucMenu("🏭 Nhà cung cấp", () => new frmDanhMuc(DanhSachDanhMuc.NhaCungCap()));
                ThemMucMenu("🩺 Công dụng thuốc", () => new frmDanhMuc(DanhSachDanhMuc.CongDung()));

                // Danh mục phụ: mở menu con
                Button btnKhac = ThemMucMenu("🗂 Danh mục khác  ▸", null);
                ContextMenuStrip menuKhac = new ContextMenuStrip() { Font = new Font("Segoe UI", 10F) };
                ThemMenuCon(menuKhac, btnKhac, "Đơn vị tính", DanhSachDanhMuc.DonViTinh);
                ThemMenuCon(menuKhac, btnKhac, "Dạng điều chế", DanhSachDanhMuc.DangDieuChe);
                ThemMenuCon(menuKhac, btnKhac, "Nước sản xuất", DanhSachDanhMuc.NuocSX);
                btnKhac.Click += (s, e) => menuKhac.Show(btnKhac, new Point(btnKhac.Width, 0));
            }
        }

        private void ThemMenuCon(ContextMenuStrip menu, Button nutCha, string text, Func<CauHinhDanhMuc> cauHinh)
        {
            menu.Items.Add(text, null, (s, e) =>
            {
                ChonNut(nutCha);
                OpenChildForm(new frmDanhMuc(cauHinh()));
            });
        }

        private void ThemNhomMenu(string text)
        {
            pnlMenu.Controls.Add(new Label()
            {
                Text = text,
                ForeColor = Color.FromArgb(108, 122, 137),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Width = 210,
                Height = 26,
                TextAlign = ContentAlignment.BottomLeft,
                Margin = new Padding(6, 8, 0, 2)
            });
        }

        /// <param name="taoForm">null = nút không mở form (tự gắn sự kiện Click)</param>
        private Button ThemMucMenu(string text, Func<Form> taoForm)
        {
            Button btn = new Button()
            {
                Text = text,
                Width = 210,
                Height = 42,
                Margin = new Padding(3, 2, 3, 2),
                FlatStyle = FlatStyle.Flat,
                ForeColor = MAU_MENU,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = MAU_MENU_CHON;
            if (taoForm != null)
            {
                btn.Click += (s, e) =>
                {
                    ChonNut(btn);
                    OpenChildForm(taoForm());
                };
            }
            pnlMenu.Controls.Add(btn);
            return btn;
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
            ChonNut(btnTraCuu);
            OpenChildForm(LayFormThuoc());
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
                    ChonNut(TimNutMenu("🛒"));
                    OpenChildForm(bh);
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
                BeginInvoke((Action)(() =>
                {
                    ChonNut(btnNhapHang);
                    OpenChildForm(new frmNhapHang());
                }));
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

        private Button TimNutMenu(string batDau)
        {
            foreach (Control c in pnlMenu.Controls)
            {
                Button b = c as Button;
                if (b != null && b.Text.StartsWith(batDau, StringComparison.Ordinal)) return b;
            }
            return null;
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
            if (btnQuanLyNV == null) return;
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
