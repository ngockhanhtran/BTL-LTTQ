using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] Quên mật khẩu: NV nhập mã + số điện thoại đã đăng ký để xác minh.
    /// Yêu cầu được ghi vào CSDL (NhanVien.YeuCauDoiMK = 1) -> trang Quản lý nhân viên tô vàng NV này,
    /// quản lý đặt mật khẩu mới rồi báo lại cho NV.
    /// [10/11/2026 lần 5] Giao diện thiết kế bằng Designer (frmQuenMatKhau.Designer.cs).
    /// </summary>
    public partial class frmQuenMatKhau : Form
    {
        private readonly NhanVienBLL nvBLL = new NhanVienBLL();

        public frmQuenMatKhau(string maNV)
        {
            InitializeComponent();
            btnGui.Tag = btnGui.BackColor;
            txtMaNV.Text = maNV ?? "";
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (txtMaNV.Text == "") txtMaNV.Focus(); else txtDienThoai.Focus();
        }

        private void btnGui_Click(object sender, EventArgs e)
        {
            lblLoi.Text = "";
            try
            {
                string thongBao;
                if (nvBLL.GuiYeuCauQuenMatKhau(txtMaNV.Text, txtDienThoai.Text, out thongBao))
                {
                    MessageBox.Show(this, thongBao, "Đã gửi yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    lblLoi.Text = "⚠  " + thongBao;
                }
            }
            catch (Exception ex)
            {
                lblLoi.Text = "⚠  Lỗi: " + ex.Message;
            }
        }
    }
}
