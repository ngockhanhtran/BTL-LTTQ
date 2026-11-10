using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>[10/11/2026] Nhân viên đang đăng nhập tự đổi mật khẩu của mình.</summary>
    public class frmDoiMatKhau : Form
    {
        private readonly NhanVienBLL nvBLL = new NhanVienBLL();
        private TextBox txtCu, txtMoi, txtNhapLai;
        private Label lblLoi;

        public frmDoiMatKhau()
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.Text = "Đổi mật khẩu - " + PhienDangNhap.MaNV;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(400, 330);
            this.BackColor = Color.White;

            int y = 20;
            Label lblTieuDe = new Label() { Text = "Đổi mật khẩu", Font = GiaoDien.TaoFont(14F, FontStyle.Bold), Left = 25, Top = y, AutoSize = true };
            y += 45;
            txtCu = TaoO("Mật khẩu hiện tại", ref y);
            txtMoi = TaoO("Mật khẩu mới (ít nhất " + NhanVienBLL.DO_DAI_MK_TOI_THIEU + " ký tự)", ref y);
            txtNhapLai = TaoO("Nhập lại mật khẩu mới", ref y);

            lblLoi = new Label() { Left = 25, Top = y, Width = 350, Height = 20, Font = GiaoDien.TaoFont(9F), ForeColor = GiaoDien.MauDo };
            y += 26;

            Button btnLuu = GiaoDien.TaoNut("Lưu mật khẩu", GiaoDien.MauXanhLa, 170, 38);
            btnLuu.Left = 25; btnLuu.Top = y;
            btnLuu.Click += (s, e) => Luu();
            Button btnHuy = GiaoDien.TaoNut("Hủy", GiaoDien.MauXam, 110, 38);
            btnHuy.Left = 265; btnHuy.Top = y;
            btnHuy.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblTieuDe, lblLoi, btnLuu, btnHuy });
            this.AcceptButton = btnLuu;
            this.CancelButton = btnHuy;
        }

        private TextBox TaoO(string nhan, ref int y)
        {
            Label l = new Label() { Text = nhan, Left = 25, Top = y, AutoSize = true, Font = GiaoDien.TaoFont(9.5F) };
            TextBox t = new TextBox() { Left = 25, Top = y + 22, Width = 350, Font = GiaoDien.TaoFont(11F), UseSystemPasswordChar = true, MaxLength = 50 };
            t.TextChanged += (s, e) => { if (lblLoi != null) lblLoi.Text = ""; };
            this.Controls.Add(l);
            this.Controls.Add(t);
            y += 60;
            return t;
        }

        private void Luu()
        {
            try
            {
                string loi = nvBLL.DoiMatKhau(PhienDangNhap.MaNV, txtCu.Text, txtMoi.Text, txtNhapLai.Text);
                if (loi != null) { lblLoi.Text = "⚠  " + loi; return; }
                MessageBox.Show(this, "Đổi mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                lblLoi.Text = "⚠  Lỗi: " + ex.Message;
            }
        }
    }
}
