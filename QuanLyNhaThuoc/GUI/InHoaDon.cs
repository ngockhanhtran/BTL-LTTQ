using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] In hóa đơn bán lẻ khổ A5 (đưa cho khách).
    /// - XuatPdf: in ra máy in ảo "Microsoft Print to PDF" có sẵn trên Windows 10/11 -> file .pdf
    ///   (không cần cài thêm thư viện NuGet, tiếng Việt hiển thị đúng vì dùng font Windows).
    /// - XemTruocVaIn: mở cửa sổ xem trước, bấm biểu tượng máy in để in ra máy in thật.
    /// Hóa đơn dài tự sang trang.
    /// </summary>
    public class InHoaDon
    {
        // ====== THÔNG TIN CỬA HÀNG IN TRÊN HÓA ĐƠN (sửa ở đây) ======
        public const string TEN_CUA_HANG = "NHÀ THUỐC 1";
        public const string DIA_CHI = "Địa chỉ: 123 Đường ABC, Quận XYZ, Hà Nội";
        public const string DIEN_THOAI = "Điện thoại: 0363 600 798";
        public const string MAY_IN_PDF = "Microsoft Print to PDF";

        private readonly ThongTinInHoaDon hd;
        private int dongDangIn;     // dòng thuốc tiếp theo cần in (khi sang trang)
        private int trangSo;

        public InHoaDon(ThongTinInHoaDon duLieu)
        {
            hd = duLieu;
        }

        public static bool CoMayInPdf()
        {
            return PrinterSettings.InstalledPrinters.Cast<string>()
                .Any(p => string.Equals(p, MAY_IN_PDF, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>In ra file PDF. Trả về true nếu file đã được tạo.</summary>
        public bool XuatPdf(string duongDan)
        {
            if (File.Exists(duongDan)) File.Delete(duongDan);   // tránh nhầm với file cũ

            using (PrintDocument pd = TaoTaiLieu())
            {
                pd.PrinterSettings.PrinterName = MAY_IN_PDF;
                pd.PrinterSettings.PrintToFile = true;
                pd.PrinterSettings.PrintFileName = duongDan;
                pd.PrintController = new StandardPrintController();   // không hiện hộp "Đang in..."
                DatKhoGiay(pd);
                pd.Print();
            }

            // Máy in PDF ghi file bất đồng bộ -> chờ tối đa ~5 giây
            for (int i = 0; i < 50 && !File.Exists(duongDan); i++) Thread.Sleep(100);
            return File.Exists(duongDan);
        }

        /// <summary>Xem trước; trong cửa sổ xem trước bấm biểu tượng máy in để in.</summary>
        public void XemTruocVaIn(IWin32Window owner)
        {
            using (PrintDocument pd = TaoTaiLieu())
            using (PrintPreviewDialog dlg = new PrintPreviewDialog())
            {
                DatKhoGiay(pd);
                dlg.Document = pd;
                dlg.Text = "Xem trước hóa đơn " + hd.MaHDB;
                dlg.Width = 700;
                dlg.Height = 850;
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.ShowIcon = false;
                ((Form)dlg).WindowState = FormWindowState.Normal;
                dlg.ShowDialog(owner);
            }
        }

        private PrintDocument TaoTaiLieu()
        {
            PrintDocument pd = new PrintDocument();
            pd.DocumentName = "HoaDon_" + hd.MaHDB;
            pd.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);   // 0.4 inch
            pd.BeginPrint += (s, e) => { dongDangIn = 0; trangSo = 0; };
            pd.PrintPage += VeTrang;
            return pd;
        }

        // Chọn khổ A5 nếu máy in hỗ trợ, không thì giữ khổ mặc định (thường là A4)
        private static void DatKhoGiay(PrintDocument pd)
        {
            try
            {
                foreach (PaperSize ps in pd.PrinterSettings.PaperSizes)
                {
                    if (ps.Kind == PaperKind.A5)
                    {
                        pd.DefaultPageSettings.PaperSize = ps;
                        return;
                    }
                }
            }
            catch { /* máy chưa cài máy in nào -> dùng mặc định */ }
        }

        // ================== VẼ 1 TRANG ==================
        private void VeTrang(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle vung = e.MarginBounds;
            float x = vung.Left, y = vung.Top, rong = vung.Width;
            trangSo++;

            using (Font fTieuDe = new Font("Segoe UI", 15F, FontStyle.Bold))
            using (Font fDam = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (Font fThuong = new Font("Segoe UI", 9F))
            using (Font fNho = new Font("Segoe UI", 8F))
            using (Font fNghieng = new Font("Segoe UI", 8.5F, FontStyle.Italic))
            using (Font fTong = new Font("Segoe UI", 12F, FontStyle.Bold))
            using (Pen butKe = new Pen(Color.Gray, 1))
            using (Pen butDut = new Pen(Color.Gray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
            using (StringFormat giua = new StringFormat { Alignment = StringAlignment.Center })
            using (StringFormat phai = new StringFormat { Alignment = StringAlignment.Far })
            using (StringFormat catBot = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap })
            {
                // ---------- ĐẦU HÓA ĐƠN (chỉ trang 1) ----------
                if (trangSo == 1)
                {
                    g.DrawString(TEN_CUA_HANG, fTieuDe, Brushes.Black, new RectangleF(x, y, rong, 28), giua); y += 28;
                    g.DrawString(DIA_CHI, fNho, Brushes.DimGray, new RectangleF(x, y, rong, 16), giua); y += 15;
                    g.DrawString(DIEN_THOAI, fNho, Brushes.DimGray, new RectangleF(x, y, rong, 16), giua); y += 22;
                    g.DrawLine(butDut, x, y, x + rong, y); y += 8;
                    g.DrawString("HÓA ĐƠN BÁN LẺ", fTong, Brushes.Black, new RectangleF(x, y, rong, 24), giua); y += 28;

                    y = VeDongThongTin(g, "Số hóa đơn:", hd.MaHDB, x, y, rong, fDam, fThuong);
                    y = VeDongThongTin(g, "Ngày bán:", hd.NgayBan.ToString("dd/MM/yyyy HH:mm"), x, y, rong, fDam, fThuong);
                    y = VeDongThongTin(g, "Thu ngân:", hd.NhanVien, x, y, rong, fDam, fThuong);
                    y = VeDongThongTin(g, "Khách hàng:", hd.KhachHang +
                        (string.IsNullOrEmpty(hd.HangThanhVien) ? "" : "  (hạng " + hd.HangThanhVien + ")"), x, y, rong, fDam, fThuong);
                    y += 6;
                }
                else
                {
                    g.DrawString("Hóa đơn " + hd.MaHDB + " (tiếp theo)", fDam, Brushes.Black, x, y); y += 22;
                }

                // ---------- BẢNG THUỐC ----------
                // Cột: STT | Tên thuốc | SL | Đơn giá | Thành tiền
                float cSTT = 28, cSL = 36, cGia = 70, cTien = 80;
                float cTen = rong - cSTT - cSL - cGia - cTien;
                float x1 = x, x2 = x1 + cSTT, x3 = x2 + cTen, x4 = x3 + cSL, x5 = x4 + cGia;
                float caoDong = 20;

                g.FillRectangle(Brushes.Gainsboro, x, y, rong, caoDong);
                g.DrawString("STT", fDam, Brushes.Black, new RectangleF(x1, y + 2, cSTT, caoDong), giua);
                g.DrawString("Tên thuốc", fDam, Brushes.Black, new RectangleF(x2 + 3, y + 2, cTen, caoDong));
                g.DrawString("SL", fDam, Brushes.Black, new RectangleF(x3, y + 2, cSL, caoDong), giua);
                g.DrawString("Đơn giá", fDam, Brushes.Black, new RectangleF(x4, y + 2, cGia - 3, caoDong), phai);
                g.DrawString("Thành tiền", fDam, Brushes.Black, new RectangleF(x5, y + 2, cTien - 3, caoDong), phai);
                y += caoDong;

                float chanTrang = vung.Bottom - 30;    // chừa chỗ ghi số trang
                float caoPhanTong = 150;               // cần cho phần tổng tiền ở trang cuối

                while (dongDangIn < hd.Dong.Count)
                {
                    if (y + caoDong > chanTrang)
                    {
                        VeSoTrang(g, vung, fNho, phai);
                        e.HasMorePages = true;
                        return;
                    }

                    DongHoaDon d = hd.Dong[dongDangIn];
                    g.DrawString((dongDangIn + 1).ToString(), fThuong, Brushes.Black, new RectangleF(x1, y + 2, cSTT, caoDong), giua);
                    g.DrawString(d.TenThuoc, fThuong, Brushes.Black, new RectangleF(x2 + 3, y + 2, cTen - 4, caoDong), catBot);
                    g.DrawString(d.SoLuong.ToString("N0"), fThuong, Brushes.Black, new RectangleF(x3, y + 2, cSL, caoDong), giua);
                    g.DrawString(d.GiaBan.ToString("N0"), fThuong, Brushes.Black, new RectangleF(x4, y + 2, cGia - 3, caoDong), phai);
                    g.DrawString(d.ThanhTien.ToString("N0"), fThuong, Brushes.Black, new RectangleF(x5, y + 2, cTien - 3, caoDong), phai);
                    y += caoDong;
                    g.DrawLine(Pens.Gainsboro, x, y, x + rong, y);
                    dongDangIn++;
                }

                // ---------- TỔNG TIỀN ----------
                if (y + caoPhanTong > chanTrang + 25)
                {
                    VeSoTrang(g, vung, fNho, phai);
                    e.HasMorePages = true;   // phần tổng sang trang mới (vòng while sẽ không vẽ thêm dòng nào)
                    return;
                }

                y += 6;
                g.DrawLine(butKe, x, y, x + rong, y); y += 6;
                float xNhan = x + rong * 0.45F, rongNhan = rong * 0.30F, xSo = x + rong * 0.75F, rongSo = rong * 0.25F;
                y = VeDongTien(g, "Tiền hàng:", hd.TienHang.ToString("N0") + " đ", xNhan, rongNhan, xSo, rongSo, y, fThuong, phai);
                if (hd.TienGiam > 0)
                    y = VeDongTien(g, "Giảm giá (" + hd.GiamGiaPhanTram.ToString("0.##") + "%):", "- " + hd.TienGiam.ToString("N0") + " đ",
                                   xNhan, rongNhan, xSo, rongSo, y, fThuong, phai);
                if (hd.TienGiamDiem > 0)
                    y = VeDongTien(g, "Đổi " + hd.DiemSuDung + " điểm:", "- " + hd.TienGiamDiem.ToString("N0") + " đ",
                                   xNhan, rongNhan, xSo, rongSo, y, fThuong, phai);
                y += 2;
                g.DrawString("TỔNG THANH TOÁN:", fDam, Brushes.Black, new RectangleF(x + rong * 0.30F, y + 3, rong * 0.42F, 22), phai);
                g.DrawString(hd.TongThanhToan.ToString("N0") + " đ", fTong, Brushes.Firebrick, new RectangleF(xSo - 30, y, rongSo + 30, 24), phai);
                y += 30;

                g.DrawLine(butDut, x, y, x + rong, y); y += 6;
                string diem = "Điểm tích lũy: +" + hd.DiemCong + " điểm" +
                              (hd.DiemConLai.HasValue ? "   •   Điểm hiện có: " + hd.DiemConLai.Value.ToString("N0") : "");
                g.DrawString(diem, fThuong, Brushes.Black, new RectangleF(x, y, rong, 18), giua); y += 22;
                g.DrawString("Cảm ơn quý khách và hẹn gặp lại!", fDam, Brushes.Black, new RectangleF(x, y, rong, 18), giua); y += 18;
                g.DrawString("Vui lòng kiểm tra thuốc và hạn sử dụng trước khi rời quầy. Dùng thuốc theo chỉ dẫn của dược sĩ.",
                             fNghieng, Brushes.DimGray, new RectangleF(x, y, rong, 32), giua);

                if (trangSo > 1) VeSoTrang(g, vung, fNho, phai);
                e.HasMorePages = false;
            }
        }

        private static float VeDongThongTin(Graphics g, string nhan, string giaTri, float x, float y, float rong, Font fNhan, Font fGiaTri)
        {
            g.DrawString(nhan, fNhan, Brushes.Black, x, y);
            g.DrawString(giaTri ?? "", fGiaTri, Brushes.Black, new RectangleF(x + 85, y, rong - 85, 18));
            return y + 18;
        }

        private static float VeDongTien(Graphics g, string nhan, string giaTri, float xNhan, float rongNhan, float xSo, float rongSo,
                                        float y, Font f, StringFormat phai)
        {
            g.DrawString(nhan, f, Brushes.Black, new RectangleF(xNhan, y, rongNhan, 18), phai);
            g.DrawString(giaTri, f, Brushes.Black, new RectangleF(xSo, y, rongSo, 18), phai);
            return y + 19;
        }

        private void VeSoTrang(Graphics g, Rectangle vung, Font f, StringFormat phai)
        {
            g.DrawString("Trang " + trangSo, f, Brushes.Gray, new RectangleF(vung.Left, vung.Bottom - 16, vung.Width, 16), phai);
        }
    }
}
