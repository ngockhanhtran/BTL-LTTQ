using System;
using System.Data;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>[10/11/2026 lần 5] Bảng NhatKyHoatDong: ghi + tra cứu nhật ký.</summary>
    public class NhatKyDAO
    {
        public void Ghi(string maNV, string tenNV, string hanhDong, string doiTuong, string chiTiet)
        {
            SqlHelper.NonQuery(@"
                INSERT INTO NhatKyHoatDong (ThoiGian, MaNV, TenNV, HanhDong, DoiTuong, ChiTiet, MayTinh)
                VALUES (GETDATE(), @MaNV, @TenNV, @HanhDong, @DoiTuong, @ChiTiet, @MayTinh)",
                SqlHelper.P("@MaNV", maNV),
                SqlHelper.P("@TenNV", Cat(tenNV, 50)),
                SqlHelper.P("@HanhDong", Cat(hanhDong, 50)),
                SqlHelper.P("@DoiTuong", Cat(doiTuong, 50)),
                SqlHelper.P("@ChiTiet", Cat(chiTiet, 500)),
                SqlHelper.P("@MayTinh", Cat(Environment.MachineName, 100)));
        }

        /// <param name="maNV">null / rỗng = mọi NV</param>
        /// <param name="hanhDong">null / rỗng = mọi hành động</param>
        public DataTable LayDanhSach(DateTime tuNgay, DateTime denNgay, string maNV, string hanhDong, string tuKhoa)
        {
            return SqlHelper.Query(@"
                SELECT TOP 2000 MaNK, ThoiGian, MaNV, TenNV, HanhDong, DoiTuong, ChiTiet, MayTinh
                FROM NhatKyHoatDong
                WHERE ThoiGian >= @TuNgay AND ThoiGian < @DenNgay
                  AND (@MaNV = N'' OR MaNV = @MaNV)
                  AND (@HanhDong = N'' OR HanhDong = @HanhDong)
                  AND (@TuKhoa = N'' OR ChiTiet LIKE N'%' + @TuKhoa + N'%' OR DoiTuong LIKE N'%' + @TuKhoa + N'%'
                       OR TenNV LIKE N'%' + @TuKhoa + N'%')
                ORDER BY ThoiGian DESC, MaNK DESC",
                SqlHelper.P("@TuNgay", tuNgay.Date),
                SqlHelper.P("@DenNgay", denNgay.Date.AddDays(1)),
                SqlHelper.PChuoi("@MaNV", (maNV ?? "").Trim()),
                SqlHelper.PChuoi("@HanhDong", (hanhDong ?? "").Trim()),
                SqlHelper.PChuoi("@TuKhoa", (tuKhoa ?? "").Trim()));
        }

        public DataTable LayDanhSachHanhDong()
        {
            return SqlHelper.Query("SELECT DISTINCT HanhDong FROM NhatKyHoatDong ORDER BY HanhDong");
        }

        public DataTable LayDanhSachNhanVien()
        {
            return SqlHelper.Query(@"SELECT MaNV, RTRIM(MaNV) + ' - ' + TenNV AS HienThi FROM NhanVien ORDER BY MaNV");
        }

        private static string Cat(string s, int toiDa)
        {
            if (s == null) return null;
            return s.Length <= toiDa ? s : s.Substring(0, toiDa);
        }
    }
}
