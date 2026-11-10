using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] Nhân viên đang đăng nhập tự đổi mật khẩu của mình.
    /// [10/11/2026 lần 5] Giao diện thiết kế bằng Designer. Chế độ BẮT BUỘC (batBuoc = true): NV mới hoặc vừa được
    /// quản lý đặt lại mật khẩu phải đổi trước khi vào chương trình (Program.cs).
    /// </summary>
    public partial class frmDoiMatKhau : Form
    {
        private readonly NhanVienBLL nvBLL = new NhanVienBLL();

        public frmDoiMatKhau() : this(false) { }

        public frmDoiMatKhau(bool batBuoc)
        {
            InitializeComponent();
            this.Text = "Đổi mật khẩu - " + PhienDangNhap.MaNV;
            lblO1.Text = "Mật khẩu mới (ít nhất " + NhanVienBLL.DO_DAI_MK_TOI_THIEU + " ký tự)";
            btnLuu.Tag = btnLuu.BackColor;
            if (batBuoc)
            {
                lblTieuDe.Text = "Đặt mật khẩu mới";
                lblBatBuoc.Text = "Mật khẩu của bạn vừa được cấp / đặt lại. Vui lòng đổi sang mật khẩu riêng để tiếp tục.";
                lblBatBuoc.Visible = true;
                btnHuy.Text = "Đăng xuất";
                this.ShowInTaskbar = true;
                this.StartPosition = FormStartPosition.CenterScreen;
            }
        }

        private void oMatKhau_TextChanged(object sender, EventArgs e)
        {
            lblLoi.Text = "";
        }

        private void btnLuu_Click(object sender, EventArgs e)
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
