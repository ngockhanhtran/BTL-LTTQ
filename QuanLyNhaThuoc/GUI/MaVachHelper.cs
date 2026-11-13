using System.Collections.Generic;
using System.Drawing;
using QuanLyNhaThuoc.BLL;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [11/11/2026 lần 7] Vẽ / đọc mã vạch bằng thư viện ZXing.Net (NuGet "ZXing.Net").
    /// - Vẽ: EAN-13 nếu mã hợp lệ, còn lại Code 128; hoặc mã QR.
    /// - Đọc: ảnh từ webcam -> chuỗi mã (EAN-13/8, UPC, Code 128/39, QR).
    /// </summary>
    internal static class MaVachHelper
    {
        public static Bitmap VeMaVach(string ma, int rong, int cao)
        {
            BarcodeWriter w = new BarcodeWriter
            {
                Format = MaVachBLL.LaEan13HopLe(ma) ? BarcodeFormat.EAN_13 : BarcodeFormat.CODE_128,
                Options = new EncodingOptions { Width = rong, Height = cao, Margin = 2, PureBarcode = true }
            };
            return w.Write(ma);
        }

        public static Bitmap VeQR(string noiDung, int canh)
        {
            BarcodeWriter w = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions { Width = canh, Height = canh, Margin = 1, CharacterSet = "UTF-8" }
            };
            return w.Write(noiDung);
        }

        /// <summary>Bộ đọc dùng lại cho mọi khung hình (tạo 1 lần).</summary>
        public static BarcodeReader TaoBoDoc()
        {
            return new BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new List<BarcodeFormat>
                    {
                        // [12/11/2026] bỏ EAN-8 / UPC-E: mã ngắn, ảnh webcam mờ hay bị đọc nhầm thành mã 8 số lạ
                        BarcodeFormat.EAN_13, BarcodeFormat.UPC_A,
                        BarcodeFormat.CODE_128, BarcodeFormat.CODE_39, BarcodeFormat.QR_CODE
                    }
                }
            };
        }

        /// <summary>Đọc mã trong ảnh; không thấy -> null.</summary>
        public static string Doc(BarcodeReader boDoc, Bitmap anh)
        {
            Result kq = boDoc.Decode(anh);
            if (kq != null) return kq.Text;

            // [12/11/2026] Mã vạch sọc (1D) chỉ đọc được khi gần NẰM NGANG trong ảnh (AutoRotate chỉ xoay 90 độ).
            // Cầm nghiêng 15 - 40 độ là không đọc được -> thử xoay ảnh thêm vài góc rồi đọc lại.
            foreach (float goc in GOC_THU)
            {
                using (Bitmap xoay = Xoay(anh, goc))
                {
                    kq = boDoc.Decode(xoay);
                    if (kq != null) return kq.Text;
                }
            }
            return null;
        }

        private static readonly float[] GOC_THU = { -20f, 20f, -35f, 35f };

        private static Bitmap Xoay(Bitmap goc, float doXoay)
        {
            Bitmap b = new Bitmap(goc.Width, goc.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.Clear(Color.White);
                g.TranslateTransform(goc.Width / 2f, goc.Height / 2f);
                g.RotateTransform(doXoay);
                g.TranslateTransform(-goc.Width / 2f, -goc.Height / 2f);
                g.DrawImage(goc, 0, 0, goc.Width, goc.Height);
            }
            return b;
        }
    }
}
