using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace QuanLyNhaThuoc.GUI
{
    /// Hiển thị mã QR chuyển khoản (VietQR) của cửa hàng.
    /// Bấm "Đã thanh toán" -> DialogResult.OK, frmBanHang sẽ lưu hóa đơn.
    public partial class frmThanhToan : Form
    {
        // ====== SỬA THÔNG TIN TÀI KHOẢN CỬA HÀNG Ở ĐÂY ======
        private const string BANK_ID = "970422";             // Mã BIN ngân hàng (970422 = MB)
        private const string SO_TAI_KHOAN = "0363600798";
        private const string TEN_TAI_KHOAN = "NHA THUOC 1";
        private const string FILE_QR_DU_PHONG = "qr_cuahang.png"; // đặt cạnh file .exe, dùng khi không có mạng

        private PictureBox picQR;
        private Label lblTrangThaiQR;

        public frmThanhToan(string maHDB, decimal soTien)
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.Text = "Thanh toán chuyển khoản";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(420, 585);
            this.BackColor = Color.White;

            InitializeComponentCustom(maHDB, soTien);
            TaiQR(maHDB, soTien);
        }

        private Label NhanGiua(string text, int top, int height, Font font, Color mau)
        {
            Label l = new Label()
            {
                Text = text,
                Left = 0,
                Top = top,
                Width = 420,
                Height = height,
                Font = font,
                ForeColor = mau,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(l);
            return l;
        }

        private void InitializeComponentCustom(string maHDB, decimal soTien)
        {
            this.SuspendLayout();

            NhanGiua("QUÉT MÃ QR ĐỂ THANH TOÁN", 15, 30, new Font("Segoe UI", 13, FontStyle.Bold), Color.Black);
            NhanGiua("Hóa đơn: " + maHDB, 48, 24, new Font("Segoe UI", 10), Color.DimGray);
            NhanGiua(soTien.ToString("N0") + " đ", 74, 38, new Font("Segoe UI", 18, FontStyle.Bold), Color.FromArgb(220, 53, 69));

            picQR = new PictureBox()
            {
                Left = 50,
                Top = 120,
                Width = 320,
                Height = 320,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(picQR);

            lblTrangThaiQR = NhanGiua("Đang tải mã QR...", 445, 22, new Font("Segoe UI", 9, FontStyle.Italic), Color.Gray);
            NhanGiua(TEN_TAI_KHOAN + "  –  STK: " + SO_TAI_KHOAN, 468, 22, new Font("Segoe UI", 10, FontStyle.Bold), Color.Black);
            NhanGiua("Nội dung CK: Thanh toan " + maHDB, 490, 22, new Font("Segoe UI", 9), Color.DimGray);

            Button btnDaThanhToan = new Button()
            {
                Text = "Đã thanh toán",
                Left = 30,
                Top = 525,
                Width = 220,
                Height = 42,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            Button btnHuy = new Button()
            {
                Text = "Hủy",
                Left = 260,
                Top = 525,
                Width = 130,
                Height = 42,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnDaThanhToan);
            this.Controls.Add(btnHuy);
            this.AcceptButton = btnDaThanhToan;
            this.CancelButton = btnHuy;

            this.ResumeLayout(false);
        }

        private void TaiQR(string maHDB, decimal soTien)
        {
            // VietQR Quick Link: tạo ảnh QR có sẵn số tiền + nội dung chuyển khoản
            string url = "https://img.vietqr.io/image/" + BANK_ID + "-" + SO_TAI_KHOAN + "-compact2.png"
                       + "?amount=" + decimal.Truncate(soTien)
                       + "&addInfo=" + Uri.EscapeDataString("Thanh toan " + maHDB)
                       + "&accountName=" + Uri.EscapeDataString(TEN_TAI_KHOAN);

            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; // cần cho .NET Framework cũ
                picQR.LoadCompleted += (s, e) =>
                {
                    if (e.Error != null || e.Cancelled) HienThiQRDuPhong();
                    else lblTrangThaiQR.Text = "";
                };
                picQR.LoadAsync(url);
            }
            catch
            {
                HienThiQRDuPhong();
            }
        }

        private void HienThiQRDuPhong()
        {
            string duongDan = Path.Combine(Application.StartupPath, FILE_QR_DU_PHONG);
            if (File.Exists(duongDan))
            {
                using (FileStream fs = new FileStream(duongDan, FileMode.Open, FileAccess.Read))
                    picQR.Image = new Bitmap(Image.FromStream(fs));
                lblTrangThaiQR.Text = "Không có mạng – dùng QR tĩnh, vui lòng nhập đúng số tiền.";
            }
            else
            {
                lblTrangThaiQR.Text = "Không tải được mã QR. Kiểm tra mạng hoặc file " + FILE_QR_DU_PHONG + ".";
            }
        }
    }
}