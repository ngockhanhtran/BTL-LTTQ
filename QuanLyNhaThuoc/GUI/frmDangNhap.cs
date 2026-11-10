using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] Form đăng nhập: mã nhân viên + mật khẩu + "Quên mật khẩu?".
    /// Thiết kế không viền: nửa trái gradient thương hiệu, nửa phải ô nhập liệu.
    /// Kéo form bằng cách giữ chuột ở vùng trống. Enter = đăng nhập, Esc = thoát.
    /// </summary>
    public class frmDangNhap : Form
    {
        private readonly NhanVienBLL nvBLL = new NhanVienBLL();

        private TextBox txtMaNV, txtMatKhau;
        private Label lblLoi;
        private Button btnDangNhap, btnDong, btnHienMK;

        private static readonly Color MAU_VIEN = Color.FromArgb(206, 212, 218);
        private static readonly Color MAU_O_NHAP = Color.FromArgb(248, 249, 250);

        // ===== WinAPI: kéo form không viền, chữ gợi ý trong TextBox, bo góc Windows 11 =====
        [DllImport("user32.dll")] private static extern bool ReleaseCapture();
        [DllImport("user32.dll")] private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        [DllImport("dwmapi.dll")] private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);
        private const int WM_NCLBUTTONDOWN = 0xA1, HT_CAPTION = 0x2, EM_SETCUEBANNER = 0x1501;

        public frmDangNhap()
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.Text = "Đăng nhập - Nhà thuốc 1";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(820, 480);
            this.BackColor = Color.White;
            this.KeyPreview = true;

            InitializeComponentCustom();
        }

        // Bóng đổ quanh form không viền
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= 0x20000; // CS_DROPSHADOW
                return cp;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try
            {
                int boGoc = 2; // DWMWCP_ROUND (Windows 11; Windows 10 bỏ qua)
                DwmSetWindowAttribute(Handle, 33, ref boGoc, sizeof(int));
            }
            catch { /* Windows cũ không có dwmapi -> bỏ qua */ }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            DatGoiY(txtMaNV, "Ví dụ: QL001");
            DatGoiY(txtMatKhau, "Nhập mật khẩu");
            txtMaNV.Focus();
        }

        private void InitializeComponentCustom()
        {
            this.SuspendLayout();

            // ================= NỬA TRÁI: THƯƠNG HIỆU =================
            Panel pnlTrai = new Panel() { Dock = DockStyle.Left, Width = 360 };
            pnlTrai.Paint += (s, e) =>
            {
                Rectangle r = pnlTrai.ClientRectangle;
                if (r.Width <= 0 || r.Height <= 0) return;
                using (LinearGradientBrush br = new LinearGradientBrush(r, Color.FromArgb(0, 122, 204), Color.FromArgb(0, 168, 150), 55F))
                    e.Graphics.FillRectangle(br, r);
                // vài vòng tròn trang trí
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush b = new SolidBrush(Color.FromArgb(28, Color.White)))
                {
                    e.Graphics.FillEllipse(b, r.Width - 150, -70, 240, 240);
                    e.Graphics.FillEllipse(b, -90, r.Height - 160, 220, 220);
                }
            };
            pnlTrai.MouseDown += KeoForm;

            Label lblIcon = new Label()
            {
                Text = "💊",
                Font = new Font("Segoe UI Emoji", 40F),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Left = 40,
                Top = 70
            };
            Label lblTenCH = new Label()
            {
                Text = "NHÀ THUỐC 1",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Left = 40,
                Top = 160
            };
            Label lblMoTa = new Label()
            {
                Text = "Hệ thống quản lý cửa hàng bán thuốc tây",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(230, 245, 255),
                BackColor = Color.Transparent,
                Left = 42,
                Top = 205,
                Width = 290,
                Height = 50
            };
            Label lblTinhNang = new Label()
            {
                Text = "✓  Tra cứu & bán thuốc, quét mã vạch\n✓  Tích điểm khách hàng, in hóa đơn PDF\n✓  Quản lý kho, báo cáo & dashboard",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Left = 42,
                Top = 275,
                Width = 300,
                Height = 80
            };
            Label lblChanTrang = new Label()
            {
                Text = "© 2026  •  BTL Lập trình trực quan",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(210, 235, 250),
                BackColor = Color.Transparent,
                AutoSize = true,
                Left = 42,
                Top = 440
            };
            foreach (Control c in new Control[] { lblIcon, lblTenCH, lblMoTa, lblTinhNang, lblChanTrang })
            {
                c.MouseDown += KeoForm;
                pnlTrai.Controls.Add(c);
            }

            // ================= NỬA PHẢI: ĐĂNG NHẬP =================
            Panel pnlPhai = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White };
            pnlPhai.MouseDown += KeoForm;

            btnDong = new Button()
            {
                Text = "✕",
                Width = 44,
                Height = 34,
                Top = 6,
                Left = 820 - 360 - 50,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                BackColor = Color.White,
                TabStop = false,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnDong.FlatAppearance.BorderSize = 0;
            btnDong.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 17, 35);
            btnDong.MouseEnter += (s, e) => btnDong.ForeColor = Color.White;
            btnDong.MouseLeave += (s, e) => btnDong.ForeColor = Color.Gray;
            btnDong.Click += (s, e) => Close();

            Label lblTieuDe = new Label()
            {
                Text = "Đăng nhập",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = GiaoDien.MauChu,
                AutoSize = true,
                Left = 46,
                Top = 52
            };
            Label lblPhu = new Label()
            {
                Text = "Chào mừng trở lại! Vui lòng đăng nhập để tiếp tục.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Left = 50,
                Top = 100
            };

            Label lblMa = TaoNhanO("Mã nhân viên", 145);
            Panel pnlMa = TaoONhap("👤", 170, out txtMaNV);
            txtMaNV.CharacterCasing = CharacterCasing.Upper;
            txtMaNV.MaxLength = 10;

            Label lblMk = TaoNhanO("Mật khẩu", 230);
            Panel pnlMk = TaoONhap("🔒", 255, out txtMatKhau);
            txtMatKhau.UseSystemPasswordChar = true;
            txtMatKhau.MaxLength = 50;
            txtMatKhau.Width = 262;

            btnHienMK = new Button()
            {
                Text = "👁",
                Font = new Font("Segoe UI Emoji", 10F),
                Width = 40,
                Height = 34,
                Left = 315,
                Top = 5,
                FlatStyle = FlatStyle.Flat,
                BackColor = MAU_O_NHAP,
                ForeColor = Color.Gray,
                TabStop = false,
                Cursor = Cursors.Hand
            };
            btnHienMK.FlatAppearance.BorderSize = 0;
            btnHienMK.Click += (s, e) =>
            {
                txtMatKhau.UseSystemPasswordChar = !txtMatKhau.UseSystemPasswordChar;
                btnHienMK.ForeColor = txtMatKhau.UseSystemPasswordChar ? Color.Gray : GiaoDien.MauXanh;
                txtMatKhau.Focus();
                txtMatKhau.SelectionStart = txtMatKhau.TextLength;
            };
            pnlMk.Controls.Add(btnHienMK);
            btnHienMK.BringToFront();

            lblLoi = new Label()
            {
                Left = 50,
                Top = 306,
                Width = 360,
                Height = 22,
                Font = new Font("Segoe UI", 9F),
                ForeColor = GiaoDien.MauDo
            };

            LinkLabel lnkQuenMK = new LinkLabel()
            {
                Text = "Quên mật khẩu?",
                Font = new Font("Segoe UI", 9.5F),
                AutoSize = false,
                Width = 140,
                Height = 22,
                Left = 50 + 360 - 140,
                Top = 330,
                TextAlign = ContentAlignment.MiddleRight,
                LinkColor = GiaoDien.MauXanh,
                ActiveLinkColor = GiaoDien.MauXanhDam,
                LinkBehavior = LinkBehavior.HoverUnderline,
                TabStop = false
            };
            lnkQuenMK.LinkClicked += (s, e) => MoQuenMatKhau();

            btnDangNhap = new Button()
            {
                Text = "ĐĂNG NHẬP",
                Left = 50,
                Top = 362,
                Width = 360,
                Height = 46,
                BackColor = GiaoDien.MauXanh,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.FlatAppearance.MouseOverBackColor = GiaoDien.MauXanhDam;
            btnDangNhap.Click += (s, e) => DangNhap();

            Label lblGoiY = new Label()
            {
                Text = "Mật khẩu mặc định: mã NV + 11  (vd: QL001 → QL00111)",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.DarkGray,
                Left = 50,
                Top = 425,
                Width = 360,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlPhai.Controls.AddRange(new Control[] { btnDong, lblTieuDe, lblPhu, lblMa, pnlMa, lblMk, pnlMk, lblLoi, lnkQuenMK, btnDangNhap, lblGoiY });

            this.Controls.Add(pnlPhai);
            this.Controls.Add(pnlTrai);

            this.AcceptButton = btnDangNhap;
            this.CancelButton = btnDong;
            this.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Color.FromArgb(200, 200, 200)))
                    e.Graphics.DrawRectangle(p, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
            };

            this.ResumeLayout(false);
        }

        private Label TaoNhanO(string text, int top)
        {
            return new Label()
            {
                Text = text,
                Left = 50,
                Top = top,
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(73, 80, 87)
            };
        }

        /// <summary>Ô nhập có viền bo, icon bên trái, viền đổi màu xanh khi đang nhập.</summary>
        private Panel TaoONhap(string icon, int top, out TextBox txt)
        {
            Panel p = new Panel() { Left = 50, Top = top, Width = 360, Height = 44, BackColor = MAU_O_NHAP, Tag = MAU_VIEN };
            p.Paint += (s, e) =>
            {
                Color mau = p.Tag is Color ? (Color)p.Tag : MAU_VIEN;
                using (Pen pen = new Pen(mau, 1.5F))
                    e.Graphics.DrawRectangle(pen, 1, 1, p.Width - 3, p.Height - 3);
            };

            Label lblIcon = new Label()
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 11F),
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                Left = 10,
                Top = 10,
                Width = 28,
                Height = 24
            };
            TextBox t = new TextBox()
            {
                BorderStyle = BorderStyle.None,
                Left = 44,
                Top = 12,
                Width = 300,
                Font = new Font("Segoe UI", 11.5F),
                BackColor = MAU_O_NHAP
            };
            t.Enter += (s, e) => { p.Tag = GiaoDien.MauXanh; p.BackColor = Color.White; t.BackColor = Color.White; DongBoNutMat(); p.Invalidate(); };
            t.Leave += (s, e) => { p.Tag = MAU_VIEN; p.BackColor = MAU_O_NHAP; t.BackColor = MAU_O_NHAP; DongBoNutMat(); p.Invalidate(); };
            t.TextChanged += (s, e) => lblLoi.Text = "";
            p.Click += (s, e) => t.Focus();
            lblIcon.Click += (s, e) => t.Focus();

            p.Controls.Add(lblIcon);
            p.Controls.Add(t);
            txt = t;
            return p;
        }

        private void DongBoNutMat()
        {
            if (btnHienMK != null && btnHienMK.Parent != null) btnHienMK.BackColor = btnHienMK.Parent.BackColor;
        }

        private static void DatGoiY(TextBox t, string goiY)
        {
            if (t.IsHandleCreated) SendMessage(t.Handle, EM_SETCUEBANNER, (IntPtr)1, goiY);
        }

        private void KeoForm(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
        }

        // ================== XỬ LÝ ==================

        private void BaoLoi(string loi, Control oCanSua)
        {
            lblLoi.Text = "⚠  " + loi;
            if (oCanSua != null) oCanSua.Focus();
        }

        private void DangNhap()
        {
            lblLoi.Text = "";
            string ma = txtMaNV.Text.Trim();
            if (ma == "") { BaoLoi("Vui lòng nhập mã nhân viên.", txtMaNV); return; }
            if (txtMatKhau.Text == "") { BaoLoi("Vui lòng nhập mật khẩu.", txtMatKhau); return; }

            btnDangNhap.Enabled = false;
            btnDangNhap.Text = "Đang đăng nhập...";
            Cursor = Cursors.WaitCursor;
            try
            {
                string loi;
                if (nvBLL.DangNhap(ma, txtMatKhau.Text, out loi))
                {
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }
                bool saiMatKhau = loi != null && loi.Contains("Mật khẩu");
                BaoLoi(loi, saiMatKhau ? (Control)txtMatKhau : txtMaNV);
                if (saiMatKhau) txtMatKhau.SelectAll();
            }
            catch (SqlException ex)
            {
                BaoLoi("Không kết nối được cơ sở dữ liệu.", null);
                MessageBox.Show(this,
                    "Không kết nối được SQL Server.\n\n" + ex.Message +
                    "\n\nKiểm tra:\n• SQL Server đang chạy\n• Đã chạy file CSDL_QuanLyNhaThuoc.sql\n• Data Source trong DAL/DBConnection.cs đúng tên server (ô Server name của SSMS)",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                BaoLoi("Lỗi: " + ex.Message, null);
            }
            finally
            {
                Cursor = Cursors.Default;
                if (!IsDisposed)
                {
                    btnDangNhap.Enabled = true;
                    btnDangNhap.Text = "ĐĂNG NHẬP";
                }
            }
        }

        private void MoQuenMatKhau()
        {
            using (frmQuenMatKhau f = new frmQuenMatKhau(txtMaNV.Text.Trim()))
                f.ShowDialog(this);
        }
    }
}
