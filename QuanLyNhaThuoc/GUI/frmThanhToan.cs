using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace QuanLyNhaThuoc.GUI
{
    /// Hiển thị mã QR chuyển khoản (VietQR) của cửa hàng.
    /// Bấm "Đã thanh toán" -> DialogResult.OK, frmBanHang sẽ lưu hóa đơn.
    /// [10/11/2026 lần 5] Giao diện thiết kế bằng Designer (frmThanhToan.Designer.cs), code chỉ còn nạp số liệu + tải QR.
    public partial class frmThanhToan : Form
    {
        // ====== SỬA THÔNG TIN TÀI KHOẢN CỬA HÀNG Ở ĐÂY ======
        private const string BANK_ID = "970422";             // Mã BIN ngân hàng (970422 = MB)
        private const string SO_TAI_KHOAN = "0363600798";
        private const string TEN_TAI_KHOAN = "NHA THUOC 1";
        private const string FILE_QR_DU_PHONG = "qr_cuahang.png"; // đặt cạnh file .exe, dùng khi không có mạng

        public frmThanhToan(string maHDB, decimal soTien)
        {
            InitializeComponent();
            lblHoaDon.Text = "Hóa đơn: " + maHDB;
            lblSoTien.Text = soTien.ToString("N0") + " đ";
            lblTaiKhoan.Text = TEN_TAI_KHOAN + "  –  STK: " + SO_TAI_KHOAN;
            lblNoiDung.Text = "Nội dung CK: Thanh toan " + maHDB;
            TaiQR(maHDB, soTien);
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
                picQR.LoadAsync(url);
            }
            catch
            {
                HienThiQRDuPhong();
            }
        }

        // Gắn trong Designer (sự kiện LoadCompleted của picQR)
        private void picQR_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null || e.Cancelled) HienThiQRDuPhong();
            else lblTrangThaiQR.Text = "";
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
