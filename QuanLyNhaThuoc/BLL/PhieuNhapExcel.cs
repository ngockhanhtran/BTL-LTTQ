using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ClosedXML.Excel;               // NuGet: ClosedXML
using QuanLyNhaThuoc.DAL;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>
    /// Đọc / tạo file Excel phiếu nhập hàng.
    /// Định dạng: sheet "PhieuNhap", dòng đầu là tiêu đề (tên cột), mỗi dòng sau là 1 thuốc.
    /// Cột được nhận theo TÊN nên thứ tự cột có thể thay đổi.
    /// </summary>
    public static class PhieuNhapExcel
    {
        private class CotExcel
        {
            public string Ten;
            public bool BatBuoc;
            public string MoTa;
            public CotExcel(string ten, bool batBuoc, string moTa) { Ten = ten; BatBuoc = batBuoc; MoTa = moTa; }
        }

        private static readonly CotExcel[] DS_COT =
        {
            new CotExcel("MaHDN",        true,  "Mã phiếu nhập (tối đa 10 ký tự). Các dòng cùng MaHDN thuộc cùng 1 phiếu."),
            new CotExcel("NgayNhap",     false, "Ngày nhập (dd/MM/yyyy). Bỏ trống = ngày giờ hiện tại."),
            new CotExcel("MaNV",         true,  "Mã nhân viên (thủ kho) phụ trách nhập hàng - phải có trong CSDL."),
            new CotExcel("MaNCC",        true,  "Mã nhà cung cấp."),
            new CotExcel("TenNCC",       false, "Chỉ cần khi NCC chưa có trong CSDL -> hệ thống tự thêm NCC mới."),
            new CotExcel("MaThuoc",      true,  "Mã thuốc (tối đa 10 ký tự)."),
            new CotExcel("TenThuoc",     false, "BẮT BUỘC với thuốc mới (chưa có trong CSDL). Thuốc đã có thì bỏ trống."),
            new CotExcel("MaDVT",        false, "Thuốc mới: mã đơn vị tính (phải có trong bảng DonViTinh)."),
            new CotExcel("MaDangDC",     false, "Thuốc mới: mã dạng điều chế (phải có trong bảng DangDieuChe)."),
            new CotExcel("ThanhPhan",    false, "Thuốc mới: thành phần."),
            new CotExcel("MaNuocSX",     false, "Thuốc mới: mã nước sản xuất (phải có trong bảng NuocSX)."),
            new CotExcel("NgaySX",       false, "Thuốc mới: ngày sản xuất (dd/MM/yyyy)."),
            new CotExcel("HanSD",        false, "Thuốc mới: hạn sử dụng (dd/MM/yyyy), phải sau NgaySX."),
            new CotExcel("ChongChiDinh", false, "Thuốc mới: chống chỉ định."),
            new CotExcel("SLNhap",       true,  "Số lượng nhập, số nguyên > 0 -> được cộng vào tồn kho."),
            new CotExcel("DonGia",       true,  "Đơn giá nhập. Giá bán tự cập nhật = 110% đơn giá nhập."),
            new CotExcel("KhuyenMai",    false, "Số tiền được giảm cho dòng này (VNĐ). Thành tiền = SLNhap x DonGia - KhuyenMai."),
        };

        // =====================================================================
        //  ĐỌC FILE
        // =====================================================================
        public static List<DongPhieuNhap> DocFile(string duongDan, List<string> loi)
        {
            var ds = new List<DongPhieuNhap>();

            // FileShare.ReadWrite: đọc được cả khi file đang mở trong Excel
            using (var fs = new FileStream(duongDan, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var wb = new XLWorkbook(fs))
            {
                IXLWorksheet ws = wb.Worksheets.FirstOrDefault(w => ChuanHoa(w.Name) == "phieunhap") ?? wb.Worksheet(1);

                IXLRow hangTieuDe = ws.FirstRowUsed();
                if (hangTieuDe == null)
                {
                    loi.Add("Sheet \"" + ws.Name + "\" không có dữ liệu.");
                    return ds;
                }

                // Ánh xạ tên cột -> số thứ tự cột
                var viTriCot = new Dictionary<string, int>();
                foreach (IXLCell c in hangTieuDe.CellsUsed())
                {
                    string k = ChuanHoa(c.GetString());
                    if (k.Length > 0 && !viTriCot.ContainsKey(k)) viTriCot[k] = c.Address.ColumnNumber;
                }
                foreach (CotExcel cot in DS_COT.Where(x => x.BatBuoc))
                    if (!viTriCot.ContainsKey(ChuanHoa(cot.Ten))) loi.Add("Thiếu cột bắt buộc: " + cot.Ten);
                if (loi.Count > 0)
                {
                    loi.Add("Hãy dùng nút \"Tải file Excel mẫu\" để lấy đúng định dạng.");
                    return ds;
                }

                int dongDau = hangTieuDe.RowNumber() + 1;
                int dongCuoi = ws.LastRowUsed().RowNumber();

                for (int r = dongDau; r <= dongCuoi; r++)
                {
                    IXLRow row = ws.Row(r);
                    var d = new DongPhieuNhap
                    {
                        DongExcel = r,
                        MaHDN = Chuoi(row, viTriCot, "MaHDN"),
                        MaNV = Chuoi(row, viTriCot, "MaNV"),
                        MaNCC = Chuoi(row, viTriCot, "MaNCC"),
                        TenNCC = Chuoi(row, viTriCot, "TenNCC"),
                        MaThuoc = Chuoi(row, viTriCot, "MaThuoc"),
                        TenThuoc = Chuoi(row, viTriCot, "TenThuoc"),
                        MaDVT = Chuoi(row, viTriCot, "MaDVT"),
                        MaDangDC = Chuoi(row, viTriCot, "MaDangDC"),
                        ThanhPhan = Chuoi(row, viTriCot, "ThanhPhan"),
                        MaNuocSX = Chuoi(row, viTriCot, "MaNuocSX"),
                        ChongChiDinh = Chuoi(row, viTriCot, "ChongChiDinh")
                    };

                    IXLCell oSL = O(row, viTriCot, "SLNhap");
                    IXLCell oGia = O(row, viTriCot, "DonGia");
                    IXLCell oKM = O(row, viTriCot, "KhuyenMai");

                    // Bỏ qua dòng trống hoàn toàn
                    if (d.MaHDN == "" && d.MaThuoc == "" && LaTrong(oSL) && LaTrong(oGia)) continue;

                    string tt = "Dòng " + r + ": ";

                    BatBuoc(d.MaHDN, "MaHDN", tt, loi);
                    BatBuoc(d.MaNV, "MaNV", tt, loi);
                    BatBuoc(d.MaNCC, "MaNCC", tt, loi);
                    BatBuoc(d.MaThuoc, "MaThuoc", tt, loi);

                    DoDai(d.MaHDN, "MaHDN", 10, tt, loi);
                    DoDai(d.MaNV, "MaNV", 10, tt, loi);
                    DoDai(d.MaNCC, "MaNCC", 10, tt, loi);
                    DoDai(d.MaThuoc, "MaThuoc", 10, tt, loi);
                    DoDai(d.MaDVT, "MaDVT", 10, tt, loi);
                    DoDai(d.MaDangDC, "MaDangDC", 10, tt, loi);
                    DoDai(d.MaNuocSX, "MaNuocSX", 10, tt, loi);
                    DoDai(d.TenNCC, "TenNCC", 100, tt, loi);
                    DoDai(d.TenThuoc, "TenThuoc", 100, tt, loi);
                    DoDai(d.ThanhPhan, "ThanhPhan", 255, tt, loi);
                    DoDai(d.ChongChiDinh, "ChongChiDinh", 255, tt, loi);

                    decimal so;
                    if (!DocSo(oSL, out so) || so <= 0 || so != decimal.Truncate(so) || so > int.MaxValue)
                        loi.Add(tt + "SLNhap phải là số nguyên lớn hơn 0.");
                    else
                        d.SLNhap = (int)so;

                    if (!DocSo(oGia, out so) || so < 0)
                        loi.Add(tt + "DonGia phải là số >= 0.");
                    else
                        d.DonGia = so;

                    if (!LaTrong(oKM))
                    {
                        if (!DocSo(oKM, out so) || so < 0)
                            loi.Add(tt + "KhuyenMai phải là số tiền >= 0.");
                        else
                            d.KhuyenMai = so;
                    }
                    if (d.SLNhap > 0 && d.KhuyenMai > d.SLNhap * d.DonGia)
                        loi.Add(tt + "KhuyenMai lớn hơn SLNhap x DonGia.");

                    d.NgayNhap = DocNgay(O(row, viTriCot, "NgayNhap"), "NgayNhap", tt, loi);
                    d.NgaySX = DocNgay(O(row, viTriCot, "NgaySX"), "NgaySX", tt, loi);
                    d.HanSD = DocNgay(O(row, viTriCot, "HanSD"), "HanSD", tt, loi);
                    if (d.NgaySX.HasValue && d.HanSD.HasValue && d.HanSD.Value <= d.NgaySX.Value)
                        loi.Add(tt + "HanSD phải sau NgaySX.");

                    ds.Add(d);
                }
            }
            return ds;
        }

        // =====================================================================
        //  TẠO FILE MẪU
        // =====================================================================
        public static void TaoFileMau(string duongDan)
        {
            using (var wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.AddWorksheet("PhieuNhap");

                for (int i = 0; i < DS_COT.Length; i++)
                {
                    int cot = i + 1;
                    IXLCell c = ws.Cell(1, cot);
                    c.SetValue(DS_COT[i].Ten);
                    c.Style.Font.Bold = true;
                    c.Style.Fill.BackgroundColor = DS_COT[i].BatBuoc ? XLColor.FromArgb(255, 217, 102) : XLColor.FromArgb(221, 235, 247);
                    c.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    string ten = DS_COT[i].Ten;
                    IXLRange vungDuLieu = ws.Range(2, cot, 500, cot);
                    if (ten.StartsWith("Ma")) vungDuLieu.Style.NumberFormat.Format = "@";            // mã: dạng text
                    if (ten == "NgayNhap" || ten == "NgaySX" || ten == "HanSD") vungDuLieu.Style.DateFormat.Format = "dd/MM/yyyy";
                    if (ten == "SLNhap" || ten == "DonGia" || ten == "KhuyenMai") vungDuLieu.Style.NumberFormat.Format = "#,##0";
                }

                // Dòng mẫu 1: thuốc ĐÃ CÓ trong CSDL -> chỉ cần mã + SL + giá
                Ghi(ws, 2, "MaHDN", "HDN001"); Ghi(ws, 2, "NgayNhap", DateTime.Today);
                Ghi(ws, 2, "MaNV", "NV01"); Ghi(ws, 2, "MaNCC", "NCC01");
                Ghi(ws, 2, "MaThuoc", "TH001");
                Ghi(ws, 2, "SLNhap", 50); Ghi(ws, 2, "DonGia", 15000); Ghi(ws, 2, "KhuyenMai", 0);

                // Dòng mẫu 2: thuốc MỚI -> điền thêm thông tin thuốc
                Ghi(ws, 3, "MaHDN", "HDN001"); Ghi(ws, 3, "NgayNhap", DateTime.Today);
                Ghi(ws, 3, "MaNV", "NV01"); Ghi(ws, 3, "MaNCC", "NCC01");
                Ghi(ws, 3, "MaThuoc", "TH002"); Ghi(ws, 3, "TenThuoc", "Efferalgan 500mg");
                Ghi(ws, 3, "MaDVT", "DVT01"); Ghi(ws, 3, "MaDangDC", "DC01");
                Ghi(ws, 3, "ThanhPhan", "Paracetamol 500mg"); Ghi(ws, 3, "MaNuocSX", "NSX01");
                Ghi(ws, 3, "NgaySX", new DateTime(2026, 6, 1)); Ghi(ws, 3, "HanSD", new DateTime(2029, 6, 1));
                Ghi(ws, 3, "ChongChiDinh", "Suy gan nặng");
                Ghi(ws, 3, "SLNhap", 30); Ghi(ws, 3, "DonGia", 45000); Ghi(ws, 3, "KhuyenMai", 50000);

                ws.SheetView.FreezeRows(1);
                ws.Columns().AdjustToContents();

                // Sheet hướng dẫn
                IXLWorksheet hd = wb.AddWorksheet("HuongDan");
                hd.Cell(1, 1).SetValue("HƯỚNG DẪN NHẬP PHIẾU NHẬP HÀNG");
                hd.Cell(1, 1).Style.Font.Bold = true;
                hd.Cell(1, 1).Style.Font.FontSize = 14;

                hd.Cell(3, 1).SetValue("Cột");
                hd.Cell(3, 2).SetValue("Bắt buộc");
                hd.Cell(3, 3).SetValue("Ý nghĩa");
                hd.Range(3, 1, 3, 3).Style.Font.Bold = true;
                hd.Range(3, 1, 3, 3).Style.Fill.BackgroundColor = XLColor.FromArgb(221, 235, 247);

                for (int i = 0; i < DS_COT.Length; i++)
                {
                    hd.Cell(4 + i, 1).SetValue(DS_COT[i].Ten);
                    hd.Cell(4 + i, 2).SetValue(DS_COT[i].BatBuoc ? "Có" : "");
                    hd.Cell(4 + i, 3).SetValue(DS_COT[i].MoTa);
                }

                string[] ghiChu =
                {
                    "- Mỗi dòng là 1 thuốc. Nhiều dòng cùng MaHDN = 1 phiếu nhập (1 phiếu chỉ có 1 MaNV và 1 MaNCC). Trong 1 phiếu không lặp lại MaThuoc.",
                    "- Thuốc ĐÃ CÓ trong CSDL: chỉ cần MaThuoc, SLNhap, DonGia -> tồn kho được cộng thêm, giá nhập và giá bán (110%) được cập nhật.",
                    "- Thuốc MỚI: điền thêm TenThuoc (bắt buộc) và các cột thông tin thuốc -> hệ thống tự thêm thuốc vào CSDL rồi cộng tồn kho.",
                    "- Cột tiêu đề màu vàng là bắt buộc. Không đổi tên dòng tiêu đề (thứ tự cột có thể thay đổi).",
                    "- Toàn bộ file được nhập trong 1 giao dịch: chỉ cần 1 dòng lỗi thì không dòng nào được ghi vào CSDL.",
                    "- Hai dòng có sẵn trong sheet PhieuNhap chỉ là ví dụ, hãy xóa/sửa trước khi import."
                };
                int rGhiChu = 4 + DS_COT.Length + 1;
                for (int i = 0; i < ghiChu.Length; i++) hd.Cell(rGhiChu + i, 1).SetValue(ghiChu[i]);

                hd.Column(1).Width = 16;
                hd.Column(2).Width = 10;
                hd.Column(3).Width = 90;

                ws.SetTabActive();
                wb.SaveAs(duongDan);
            }
        }

        public static void MoFile(string duongDan)
        {
            Process.Start(new ProcessStartInfo(duongDan) { UseShellExecute = true });
        }

        // =====================================================================
        //  HÀM PHỤ
        // =====================================================================
        private static string ChuanHoa(string s)
        {
            return (s ?? "").Trim().Replace(" ", "").Replace("_", "").Replace("*", "").ToLowerInvariant();
        }

        private static IXLCell O(IXLRow row, Dictionary<string, int> viTriCot, string ten)
        {
            int c;
            return viTriCot.TryGetValue(ChuanHoa(ten), out c) ? row.Cell(c) : null;
        }

        private static string Chuoi(IXLRow row, Dictionary<string, int> viTriCot, string ten)
        {
            IXLCell c = O(row, viTriCot, ten);
            return LaTrong(c) ? "" : c.GetString().Trim();
        }

        private static bool LaTrong(IXLCell c)
        {
            return c == null || c.IsEmpty() || c.GetString().Trim() == "";
        }

        private static void BatBuoc(string giaTri, string ten, string tt, List<string> loi)
        {
            if (string.IsNullOrWhiteSpace(giaTri)) loi.Add(tt + "thiếu " + ten + ".");
        }

        private static void DoDai(string giaTri, string ten, int toiDa, string tt, List<string> loi)
        {
            if (giaTri != null && giaTri.Length > toiDa) loi.Add(tt + ten + " dài quá " + toiDa + " ký tự.");
        }

        private static bool DocSo(IXLCell c, out decimal v)
        {
            v = 0;
            if (LaTrong(c)) return false;

            if (c.DataType == XLDataType.Number)
            {
                v = Math.Round((decimal)c.GetDouble(), 2);
                return true;
            }

            // Số được gõ dạng chữ: "15.000", "15,000", "15000", "12.5"
            string s = c.GetString().Trim().Replace(" ", "").Replace("đ", "").Replace("₫", "");
            if (Regex.IsMatch(s, @"^\d{1,3}([.,]\d{3})+$")) s = s.Replace(".", "").Replace(",", "");
            else if (s.Contains(",") && !s.Contains(".")) s = s.Replace(",", ".");
            return decimal.TryParse(s, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                                    CultureInfo.InvariantCulture, out v);
        }

        private static DateTime? DocNgay(IXLCell c, string ten, string tt, List<string> loi)
        {
            if (LaTrong(c)) return null;

            DateTime d;
            bool ok = false;
            if (c.DataType == XLDataType.DateTime) { d = c.GetDateTime(); ok = true; }
            else if (c.DataType == XLDataType.Number) { d = DateTime.FromOADate(c.GetDouble()); ok = true; }
            else
            {
                string[] dinhDang = { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy", "yyyy-MM-dd",
                                      "dd/MM/yyyy HH:mm", "d/M/yyyy H:mm", "yyyy-MM-dd HH:mm:ss" };
                ok = DateTime.TryParseExact(c.GetString().Trim(), dinhDang, CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out d);
            }

            if (!ok || d.Year < 1900 || d.Year > 2100)
            {
                loi.Add(tt + ten + " không phải ngày hợp lệ (dd/MM/yyyy).");
                return null;
            }
            return d;
        }

        private static void Ghi(IXLWorksheet ws, int r, string tenCot, object v)
        {
            int c = Array.FindIndex(DS_COT, x => x.Ten == tenCot) + 1;
            IXLCell o = ws.Cell(r, c);
            if (v is string) o.SetValue((string)v);
            else if (v is DateTime) o.SetValue((DateTime)v);
            else o.SetValue(Convert.ToDouble(v));
        }
    }
}