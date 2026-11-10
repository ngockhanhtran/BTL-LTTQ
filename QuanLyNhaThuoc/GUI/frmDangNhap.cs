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
    /// Giao diện thiết kế bằng Designer (frmDangNhap.Designer.cs) - mở được bằng kéo thả.
    /// Nền gradient, viền ô nhập, bóng đổ, bo góc được vẽ khi CHẠY (Designer không hiển thị các hiệu ứng này).
    /// Kéo form bằng cách giữ chuột ở vùng trống. Enter = đăng nhập, Esc = thoát.
    /// </summary>
    public partial class frmDangNhap : Form
    {
        private readonly NhanVienBLL nvBLL = new NhanVienBLL();

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
            InitializeComponent();
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

        // ================== VẼ GIAO DIỆN (chỉ thấy khi chạy) ==================

        // Nền gradient + vòng tròn trang trí cho nửa trái
        private void pnlTrai_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = pnlTrai.ClientRectangle;
            if (r.Width <= 0 || r.Height <= 0) return;
            using (LinearGradientBrush br = new LinearGradientBrush(r, Color.FromArgb(0, 122, 204), Color.FromArgb(0, 168, 150), 55F))
                e.Graphics.FillRectangle(br, r);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (SolidBrush b = new SolidBrush(Color.FromArgb(28, Color.White)))
            {
                e.Graphics.FillEllipse(b, r.Width - 150, -70, 240, 240);
                e.Graphics.FillEllipse(b, -90, r.Height - 160, 220, 220);
            }
        }

        // Viền mảnh quanh form
        private void frmDangNhap_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(Color.FromArgb(200, 200, 200)))
                e.Graphics.DrawRectangle(p, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        }

        // Viền ô nhập (xanh khi đang nhập). Panel.Tag giữ màu viền hiện tại.
        private void pnlONhap_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            Color mau = p.Tag is Color ? (Color)p.Tag : MAU_VIEN;
            using (Pen pen = new Pen(mau, 1.5F))
                e.Graphics.DrawRectangle(pen, 1, 1, p.Width - 3, p.Height - 3);
        }

        // Bấm vào khung / icon của ô nhập -> đưa con trỏ vào TextBox bên trong
        private void pnlONhap_Click(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            Panel p = c as Panel ?? c.Parent as Panel;
            if (p == pnlMa) txtMaNV.Focus();
            else if (p == pnlMk) txtMatKhau.Focus();
        }

        private void oNhap_Enter(object sender, EventArgs e)
        {
            DoiMauONhap((TextBox)sender, true);
        }

        private void oNhap_Leave(object sender, EventArgs e)
        {
            DoiMauONhap((TextBox)sender, false);
        }

        private void oNhap_TextChanged(object sender, EventArgs e)
        {
            lblLoi.Text = "";
        }

        private void DoiMauONhap(TextBox t, bool dangNhap)
        {
            Panel p = (Panel)t.Parent;
            Color nen = dangNhap ? Color.White : MAU_O_NHAP;
            p.Tag = dangNhap ? GiaoDien.MauXanh : MAU_VIEN;
            p.BackColor = nen;
            t.BackColor = nen;
            if (p == pnlMk) btnHienMK.BackColor = nen;
            p.Invalidate();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDong_MouseEnter(object sender, EventArgs e)
        {
            btnDong.ForeColor = Color.White;
        }

        private void btnDong_MouseLeave(object sender, EventArgs e)
        {
            btnDong.ForeColor = Color.Gray;
        }

        private void btnHienMK_Click(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !txtMatKhau.UseSystemPasswordChar;
            btnHienMK.ForeColor = txtMatKhau.UseSystemPasswordChar ? Color.Gray : GiaoDien.MauXanh;
            txtMatKhau.Focus();
            txtMatKhau.SelectionStart = txtMatKhau.TextLength;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            DangNhap();
        }

        private void lnkQuenMK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmQuenMatKhau f = new frmQuenMatKhau(txtMaNV.Text.Trim()))
                f.ShowDialog(this);
        }

        // [10/11/2026 lần 5] đổi máy chủ SQL Server mà không cần sửa code
        private void lnkCauHinh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmCauHinhKetNoi f = new frmCauHinhKetNoi(null))
                f.ShowDialog(this);
        }

        private static void DatGoiY(TextBox t, string goiY)
        {
            if (t.IsHandleCreated) SendMessage(t.Handle, EM_SETCUEBANNER, (IntPtr)1, goiY);
        }

        // Giữ chuột trái ở vùng trống để kéo form (form không có thanh tiêu đề)
        private void KeoForm(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
        }

        // ================== XỬ LÝ ĐĂNG NHẬP ==================

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
    }
}
