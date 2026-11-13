using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using QuanLyNhaThuoc.DAL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>
    /// [11/11/2026 lần 7] DỰ BÁO NHẬP HÀNG từ tốc độ bán.
    ///   Tốc độ bán / ngày = số đã bán trong N ngày gần nhất / N
    ///   Còn đủ bán (ngày)  = tồn bán được (lô còn hạn) / tốc độ bán
    ///   Cần có cho D ngày  = làm tròn LÊN (tốc độ bán x D)
    ///   Nên nhập           = Cần có - tồn (không âm)
    /// Mức: Hết hàng | Cần nhập gấp (còn &lt; 7 ngày) | Nên nhập (còn &lt; D ngày) | Đủ hàng | Không bán trong kỳ.
    /// Ví dụ: 30 ngày bán 120 hộp (4 hộp/ngày), còn 24 hộp -> đủ bán 6 ngày; dự trữ 30 ngày cần 120 -> nên nhập 96 hộp.
    /// Đề xuất xuất ra file Excel ĐÚNG MẪU phiếu nhập -> import ở trang Nhập hàng (khép kín quy trình).
    /// </summary>
    public class DuBaoNhapHangBLL
    {
        public const int NGAY_CAN_GAP = 7;
        private static readonly CultureInfo VN = new CultureInfo("vi-VN");
        private readonly DuBaoDAO dao = new DuBaoDAO();

        public List<KetQuaDuBao> DuBao(int soNgayPhanTich, int soNgayDuTru)
        {
            KiemTraThamSo(soNgayPhanTich, soNgayDuTru);
            return dao.LaySoLieuBan(soNgayPhanTich)
                      .Select(s => Tinh(s, soNgayPhanTich, soNgayDuTru))
                      .OrderBy(k => (int)k.MucDo)
                      .ThenBy(k => k.SoNgayConDu ?? decimal.MaxValue)
                      .ThenByDescending(k => k.DeXuatNhap)
                      .ToList();
        }

        public static void KiemTraThamSo(int soNgayPhanTich, int soNgayDuTru)
        {
            if (soNgayPhanTich < 7 || soNgayPhanTich > 365) throw new ArgumentException("Số ngày phân tích phải từ 7 đến 365.");
            if (soNgayDuTru < 7 || soNgayDuTru > 180) throw new ArgumentException("Số ngày dự trữ phải từ 7 đến 180.");
        }

        // ================== HÀM THUẦN (có unit test) ==================

        public static KetQuaDuBao Tinh(SoLieuBanThuoc s, int soNgayPhanTich, int soNgayDuTru)
        {
            if (s == null) throw new ArgumentNullException("s");
            if (soNgayPhanTich <= 0) throw new ArgumentOutOfRangeException("soNgayPhanTich");
            if (soNgayDuTru <= 0) throw new ArgumentOutOfRangeException("soNgayDuTru");

            int ton = Math.Max(0, s.TonBanDuoc);
            int daBan = Math.Max(0, s.DaBan);
            decimal tocDo = (decimal)daBan / soNgayPhanTich;
            string dvt = DonVi(s.TenDVT);

            KetQuaDuBao kq = new KetQuaDuBao { SoLieu = s, TocDoBanNgay = Math.Round(tocDo, 2) };

            if (daBan == 0)
            {
                kq.SoNgayConDu = null;
                kq.DeXuatNhap = 0;
                kq.MucDo = MucDoDuBao.KhongBan;
                kq.LoiKhuyen = s.TenThuoc + " không bán được trong " + soNgayPhanTich + " ngày qua (còn " + ton + " " + dvt + "), chưa cần nhập.";
            }
            else
            {
                decimal conDu = Math.Round(ton / tocDo, 1);
                int canCo = (int)Math.Ceiling(tocDo * soNgayDuTru);
                kq.SoNgayConDu = conDu;
                kq.DeXuatNhap = Math.Max(0, canCo - ton);

                if (ton == 0) kq.MucDo = MucDoDuBao.HetHang;
                else if (conDu < NGAY_CAN_GAP) kq.MucDo = MucDoDuBao.CanNhapGap;
                else if (conDu < soNgayDuTru) kq.MucDo = MucDoDuBao.NenNhap;
                else kq.MucDo = MucDoDuBao.DuHang;

                if (kq.MucDo == MucDoDuBao.HetHang)
                    kq.LoiKhuyen = s.TenThuoc + " đã hết hàng bán được (bán " + kq.TocDoBanNgay.ToString("0.##", VN) + " " + dvt + "/ngày), nên nhập " + kq.DeXuatNhap + " " + dvt + ".";
                else if (kq.DeXuatNhap > 0)
                    kq.LoiKhuyen = s.TenThuoc + " còn đủ bán " + MoTaSoNgay(conDu) + ", nên nhập " + kq.DeXuatNhap + " " + dvt + ".";
                else
                    kq.LoiKhuyen = s.TenThuoc + " còn đủ bán " + MoTaSoNgay(conDu) + ", chưa cần nhập.";
            }
            kq.TenMucDo = TenMucDo(kq.MucDo);
            return kq;
        }

        public static string TenMucDo(MucDoDuBao m)
        {
            switch (m)
            {
                case MucDoDuBao.HetHang: return "Hết hàng";
                case MucDoDuBao.CanNhapGap: return "Cần nhập gấp";
                case MucDoDuBao.NenNhap: return "Nên nhập";
                case MucDoDuBao.DuHang: return "Đủ hàng";
                default: return "Không bán trong kỳ";
            }
        }

        private static string MoTaSoNgay(decimal soNgay)
        {
            if (soNgay < 1) return "chưa tới 1 ngày";
            return Math.Floor(soNgay).ToString("N0", VN) + " ngày";
        }

        private static string DonVi(string tenDVT)
        {
            string s = (tenDVT ?? "").Trim();
            return s.Length == 0 ? "đơn vị" : s.ToLower(VN);
        }

        /// <summary>Mã kế tiếp giữ nguyên tiền tố và độ dài số: TangMa("HDN009", 2) = "HDN011".</summary>
        public static string TangMa(string ma, int buoc)
        {
            Match m = Regex.Match(ma ?? "", @"^(\D*)(\d+)$");
            if (!m.Success) throw new ArgumentException("Mã \"" + ma + "\" không đúng dạng tiền tố + số.");
            long so = long.Parse(m.Groups[2].Value) + buoc;
            return m.Groups[1].Value + so.ToString().PadLeft(m.Groups[2].Value.Length, '0');
        }

        // ================== XUẤT PHIẾU NHẬP EXCEL ==================

        /// <summary>
        /// Tạo file Excel đúng mẫu import của trang Nhập hàng từ các dòng đề xuất.
        /// Mỗi nhà cung cấp 1 phiếu (mẫu import: 1 phiếu chỉ có 1 MaNCC). Thuốc chưa từng nhập (chưa biết NCC)
        /// để ô MaNCC trống, tô vàng -> người dùng điền trước khi import.
        /// Trả về số phiếu đã tạo.
        /// </summary>
        public int XuatPhieuNhapExcel(IList<KetQuaDuBao> dsChon, string duongDan, string maNV)
        {
            List<KetQuaDuBao> ds = (dsChon ?? new List<KetQuaDuBao>()).Where(k => k.DeXuatNhap > 0).ToList();
            if (ds.Count == 0) throw new ArgumentException("Không có thuốc nào cần nhập trong các dòng đã chọn.");

            var nhom = ds.GroupBy(k => k.SoLieu.MaNCCGanNhat ?? "").OrderBy(g => g.Key == "" ? 1 : 0).ThenBy(g => g.Key).ToList();
            string maDau = dao.MaPhieuNhapTiepTheo();

            string[] cot = { "MaHDN", "NgayNhap", "MaNV", "MaNCC", "MaThuoc", "SLNhap", "DonGia", "KhuyenMai",
                             "GhiChu_TenThuoc", "GhiChu_NhaCungCap", "GhiChu_DuBao" };

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.AddWorksheet("PhieuNhap");
                for (int i = 0; i < cot.Length; i++)
                {
                    IXLCell c = ws.Cell(1, i + 1);
                    c.SetValue(cot[i]);
                    c.Style.Font.Bold = true;
                    c.Style.Fill.BackgroundColor = cot[i].StartsWith("GhiChu") ? XLColor.FromArgb(226, 239, 218) : XLColor.FromArgb(255, 217, 102);
                    c.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                int r = 1;
                for (int p = 0; p < nhom.Count; p++)
                {
                    string maHDN = TangMa(maDau, p);
                    foreach (KetQuaDuBao k in nhom[p].OrderBy(x => x.SoLieu.MaThuoc))
                    {
                        r++;
                        ws.Cell(r, 1).SetValue(maHDN);
                        ws.Cell(r, 2).SetValue(DateTime.Today);
                        ws.Cell(r, 3).SetValue(maNV ?? "");
                        ws.Cell(r, 4).SetValue(k.SoLieu.MaNCCGanNhat ?? "");
                        ws.Cell(r, 5).SetValue(k.SoLieu.MaThuoc);
                        ws.Cell(r, 6).SetValue((double)k.DeXuatNhap);
                        ws.Cell(r, 7).SetValue((double)k.SoLieu.DonGiaNhap);
                        ws.Cell(r, 8).SetValue(0.0);
                        ws.Cell(r, 9).SetValue(k.SoLieu.TenThuoc);
                        ws.Cell(r, 10).SetValue(k.SoLieu.TenNCCGanNhat ?? "(chưa từng nhập - điền MaNCC)");
                        ws.Cell(r, 11).SetValue(k.LoiKhuyen);
                        if (string.IsNullOrEmpty(k.SoLieu.MaNCCGanNhat))
                            ws.Cell(r, 4).Style.Fill.BackgroundColor = XLColor.Yellow;
                        if (k.SoLieu.DonGiaNhap <= 0)
                            ws.Cell(r, 7).Style.Fill.BackgroundColor = XLColor.Yellow;
                    }
                }

                ws.Range(2, 1, Math.Max(2, r), 5).Style.NumberFormat.Format = "@";
                ws.Range(2, 2, Math.Max(2, r), 2).Style.DateFormat.Format = "dd/MM/yyyy";
                ws.Range(2, 6, Math.Max(2, r), 8).Style.NumberFormat.Format = "#,##0";
                ws.SheetView.FreezeRows(1);
                ws.Columns().AdjustToContents();

                IXLWorksheet hd = wb.AddWorksheet("HuongDan");
                hd.Cell(1, 1).SetValue("PHIẾU NHẬP LẬP TỪ DỰ BÁO NHẬP HÀNG - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                hd.Cell(1, 1).Style.Font.Bold = true;
                hd.Cell(3, 1).SetValue("1. Kiểm tra lại số lượng (SLNhap), đơn giá (DonGia); ô vàng là ô còn thiếu, phải điền.");
                hd.Cell(4, 1).SetValue("2. Có thể thêm cột SoLo, NgaySX, HanSD cho từng lô (xem file mẫu ở trang Nhập hàng).");
                hd.Cell(5, 1).SetValue("3. Lưu file -> trang Nhập hàng -> \"Import phiếu nhập Excel\" -> chọn file này.");
                hd.Cell(6, 1).SetValue("Các cột GhiChu_... chỉ để tham khảo, chương trình bỏ qua khi import.");
                hd.Column(1).Width = 110;

                wb.SaveAs(duongDan);
            }

            NhatKyBLL.Ghi(NhatKyBLL.NHAP_HANG, maDau, "Lập " + nhom.Count + " phiếu nhập Excel từ dự báo (" + ds.Count + " thuốc)");
            return nhom.Count;
        }
    }
}
