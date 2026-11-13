using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>[11/11/2026 lần 7] Mã vạch thuốc: lấy mã nội bộ đã cấp, gán mã cho thuốc chưa có.</summary>
    public class MaVachDAO
    {
        /// <summary>Các mã vạch nội bộ đã cấp (EAN-13 đầu số 200 - dải dành cho dùng nội bộ cửa hàng).</summary>
        public List<string> LayMaVachNoiBo()
        {
            DataTable dt = SqlHelper.Query("SELECT MaVach FROM Thuoc WHERE MaVach LIKE '200%' AND LEN(MaVach) = 13");
            return dt.Rows.Cast<DataRow>().Select(r => DocDong.Chuoi(r, "MaVach")).Where(s => s != null).ToList();
        }

        public bool DaDung(string maVach)
        {
            return SqlHelper.Scalar("SELECT 1 FROM Thuoc WHERE MaVach = @V", SqlHelper.PChuoi("@V", maVach)) != null;
        }

        /// <summary>Gán mã vạch cho thuốc CHƯA có mã vạch. Trả về false nếu thuốc đã có mã (không ghi đè).</summary>
        public bool GanMaVach(string maThuoc, string maVach)
        {
            return SqlHelper.NonQuery(
                "UPDATE Thuoc SET MaVach = @V WHERE MaThuoc = @Ma AND (MaVach IS NULL OR LTRIM(RTRIM(MaVach)) = '')",
                SqlHelper.PChuoi("@V", maVach), SqlHelper.PChuoi("@Ma", maThuoc)) > 0;
        }
    }
}
