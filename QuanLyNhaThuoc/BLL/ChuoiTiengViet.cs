using System.Globalization;
using System.Text;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>
    /// [11/11/2026 lần 7] Xử lý chuỗi tiếng Việt cho tìm kiếm không dấu.
    /// Kết quả giống hàm dbo.fn_BoDau trong CSDL: chữ thường, bỏ dấu, đ -> d.
    ///   BoDau("Giảm Đau") = "giam dau"
    /// Cách làm: tách chữ có dấu thành chữ gốc + dấu (chuẩn Unicode NFD) rồi bỏ các ký tự dấu.
    /// </summary>
    public static class ChuoiTiengViet
    {
        public static string BoDau(string s)
        {
            if (s == null) return null;

            string tach = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder(tach.Length);
            foreach (char c in tach)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark) continue;   // dấu
                if (c == 'đ' || c == 'Đ') { sb.Append('d'); continue; }                                    // đ không tách được
                sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }

        /// <summary>vanBan có chứa tuKhoa không, không phân biệt hoa thường và dấu.</summary>
        public static bool ChuaKhongDau(string vanBan, string tuKhoa)
        {
            string k = BoDau(tuKhoa ?? "").Trim();
            if (k.Length == 0) return true;
            return BoDau(vanBan ?? "").Contains(k);
        }
    }
}
