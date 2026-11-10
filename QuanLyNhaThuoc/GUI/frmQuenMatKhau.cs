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
    /// </summary>
    public class frmQuenMatKhau : Form
    {
        private readonly NhanVienBLL nvBLL = new NhanVienBLL();
        private TextBox txtMaNV, txtDienThoai;
        private Label lblLoi;

        public frmQuenMatKhau(string maNV)
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.Text = "Quên mật khẩu";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(440, 330);
            this.BackColor = Color.White;

            InitializeComponentCustom();
            txtMaNV.Text = maNV ?? "";
        }

        private void InitializeComponentCustom()
        {
            Label lblIcon = new Label() { Text = "🔑", Font = new Font("Segoe UI Emoji", 22F), Left = 25, Top = 18, AutoSize = true };
            Label lblTieuDe = new Label() { Text = "Quên mật khẩu", Font = GiaoDien.TaoFont(15F, FontStyle.Bold), ForeColor = GiaoDien.MauChu, Left = 75, Top = 24, AutoSize = true };
            Label lblMoTa = new Label()
            {
                Text = "Nhập mã nhân viên và số điện thoại đã đăng ký. Yêu cầu sẽ được gửi tới quản lý; " +
                       "quản lý sẽ cấp lại mật khẩu mới cho bạn.",
                Font = GiaoDien.TaoFont(9.5F),
                ForeColor = Color.DimGray,
                Left = 27,
                Top = 72,
                Width = 390,
                Height = 42
            };

            Label lblMa = new Label() { Text = "Mã nhân viên", Font = GiaoDien.TaoFont(9.5F, FontStyle.Bold), Left = 27, Top = 125, AutoSize = true };
            txtMaNV = new TextBox() { Left = 27, Top = 148, Width = 385, Font = GiaoDien.TaoFont(11F), CharacterCasing = CharacterCasing.Upper, MaxLength = 10 };

            Label lblSdt = new Label() { Text = "Số điện thoại đã đăng ký", Font = GiaoDien.TaoFont(9.5F, FontStyle.Bold), Left = 27, Top = 185, AutoSize = true };
            txtDienThoai = new TextBox() { Left = 27, Top = 208, Width = 385, Font = GiaoDien.TaoFont(11F), MaxLength = 20 };

            lblLoi = new Label() { Left = 27, Top = 242, Width = 390, Height = 20, Font = GiaoDien.TaoFont(9F), ForeColor = GiaoDien.MauDo };

            Button btnGui = GiaoDien.TaoNut("Gửi yêu cầu", GiaoDien.MauXanh, 180, 40);
            btnGui.Left = 27; btnGui.Top = 272;
            btnGui.Click += (s, e) => GuiYeuCau();

            Button btnHuy = GiaoDien.TaoNut("Đóng", GiaoDien.MauXam, 120, 40);
            btnHuy.Left = 292; btnHuy.Top = 272;
            btnHuy.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblIcon, lblTieuDe, lblMoTa, lblMa, txtMaNV, lblSdt, txtDienThoai, lblLoi, btnGui, btnHuy });
            this.AcceptButton = btnGui;
            this.CancelButton = btnHuy;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (txtMaNV.Text == "") txtMaNV.Focus(); else txtDienThoai.Focus();
        }

        private void GuiYeuCau()
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
