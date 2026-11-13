using System.Collections.Generic;
using System.Data;
using System.Linq;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>
    /// [11/11/2026 lần 7] Gợi ý khi gõ (sp_GoiYThuoc) và gợi ý mua kèm (sp_GoiYMuaKem).
    /// </summary>
    public class GoiYDAO
    {
        public List<GoiYThuoc> GoiYThuoc(string tuKhoa, int soDong)
        {
            DataTable dt = SqlHelper.QueryProc("sp_GoiYThuoc",
                SqlHelper.PChuoi("@TuKhoa", tuKhoa), SqlHelper.P("@SoDong", soDong));
            return dt.Rows.Cast<DataRow>().Select(r => new GoiYThuoc
            {
                MaThuoc = DocDong.Chuoi(r, "MaThuoc"),
                TenThuoc = DocDong.Chuoi(r, "TenThuoc"),
                GiaBan = DocDong.Tien(r, "GiaBan"),
                MaVach = DocDong.Chuoi(r, "MaVach"),
                KeDon = DocDong.Co(r, "KeDon"),
                SLBanDuoc = DocDong.So(r, "SLBanDuoc")
            }).ToList();
        }

        public List<GoiYMuaKem> GoiYMuaKem(string maThuoc, IEnumerable<string> daCo, int soDong, int soNgay, int toiThieu)
        {
            string ds = string.Join(",", (daCo ?? Enumerable.Empty<string>()).Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.Trim()));
            DataTable dt = SqlHelper.QueryProc("sp_GoiYMuaKem",
                SqlHelper.PChuoi("@MaThuoc", maThuoc),
                new System.Data.SqlClient.SqlParameter("@DaCo", SqlDbType.NVarChar, -1) { Value = ds },
                SqlHelper.P("@SoDong", soDong),
                SqlHelper.P("@SoNgay", soNgay),
                SqlHelper.P("@ToiThieu", toiThieu));
            return dt.Rows.Cast<DataRow>().Select(r => new GoiYMuaKem
            {
                MaThuoc = DocDong.Chuoi(r, "MaThuoc"),
                TenThuoc = DocDong.Chuoi(r, "TenThuoc"),
                GiaBan = DocDong.Tien(r, "GiaBan"),
                SoLanCung = DocDong.So(r, "SoLanCung"),
                SoHoaDonCoThuoc = DocDong.So(r, "SoHoaDonCoThuoc"),
                TyLe = DocDong.Tien(r, "TyLe")
            }).ToList();
        }
    }
}
