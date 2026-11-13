using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>
    /// [11/11/2026 lần 7] Đo hiệu năng truy vấn trước / sau khi có index (trang "Hiệu năng truy vấn").
    /// Các lệnh sinh dữ liệu / tạo index chạy lâu -> CommandTimeout riêng (mặc định chỉ 30 giây).
    /// </summary>
    public class HieuNangDAO
    {
        private const int TIMEOUT_LAU = 900;     // 15 phút

        public DataRow ThongTin()
        {
            DataTable dt = SqlHelper.QueryProc("sp_HieuNang_ThongTin");
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public int SinhDuLieu(int soHoaDon)
        {
            object kq = ChayProcLau("sp_HieuNang_SinhDuLieu", new SqlParameter("@SoHoaDon", SqlDbType.Int) { Value = soHoaDon });
            return kq == null || kq == DBNull.Value ? 0 : Convert.ToInt32(kq);
        }

        public int XoaDuLieu()
        {
            object kq = ChayProcLau("sp_HieuNang_XoaDuLieu");
            return kq == null || kq == DBNull.Value ? 0 : Convert.ToInt32(kq);
        }

        /// <summary>true = tạo các index, false = xóa (chỉ để đo "trước khi có index").</summary>
        public void DatIndex(bool tao)
        {
            ChayProcLau("sp_HieuNang_Index", new SqlParameter("@Tao", SqlDbType.Bit) { Value = tao });
        }

        private static object ChayProcLau(string tenProc, params SqlParameter[] ps)
        {
            using (SqlConnection conn = SqlHelper.MoKetNoi())
            using (SqlCommand cmd = new SqlCommand(tenProc, conn) { CommandType = CommandType.StoredProcedure, CommandTimeout = TIMEOUT_LAU })
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                return cmd.ExecuteScalar();      // cột đầu, dòng đầu của kết quả cuối proc
            }
        }

        /// <summary>
        /// Chạy 3 truy vấn hay dùng trên bảng hóa đơn bán, mỗi truy vấn: 1 lần "làm nóng" (biên dịch kế hoạch,
        /// nạp dữ liệu vào bộ nhớ) rồi đo soLan lần bằng Stopwatch, lấy TRUNG VỊ (bỏ ảnh hưởng của lần chạy bất thường).
        /// </summary>
        public List<KetQuaChay> ChayBoTruyVan(DateTime ngayMau, string maThuocMau, int soLan)
        {
            DateTime dauThang = new DateTime(ngayMau.Year, ngayMau.Month, 1);
            var ds = new List<KetQuaChay>();
            using (SqlConnection conn = SqlHelper.MoKetNoi())
            {
                ds.Add(Do(conn, "Hóa đơn bán trong 1 ngày",
                    "SELECT MaHDB, NgayBan, MaNV, MaKhach, TongTien FROM HoaDonBan WHERE NgayBan >= @Tu AND NgayBan < @Den AND DaHuy = 0",
                    () => new[] { new SqlParameter("@Tu", SqlDbType.DateTime) { Value = ngayMau.Date },
                                  new SqlParameter("@Den", SqlDbType.DateTime) { Value = ngayMau.Date.AddDays(1) } }, soLan));

                ds.Add(Do(conn, "Doanh thu 1 tháng",
                    "SELECT COUNT(*) AS SoHoaDon, SUM(TongTien) AS DoanhThu FROM HoaDonBan WHERE NgayBan >= @Tu AND NgayBan < @Den AND DaHuy = 0",
                    () => new[] { new SqlParameter("@Tu", SqlDbType.DateTime) { Value = dauThang },
                                  new SqlParameter("@Den", SqlDbType.DateTime) { Value = dauThang.AddMonths(1) } }, soLan));

                ds.Add(Do(conn, "Tổng đã bán của 1 thuốc",
                    "SELECT COUNT(*) AS SoLanBan, SUM(SoLuong) AS TongSL, SUM(ThanhTien) AS DoanhThu FROM ChiTietHDB WHERE MaThuoc = @Ma",
                    () => new[] { new SqlParameter("@Ma", SqlDbType.VarChar, 10) { Value = (object)maThuocMau ?? DBNull.Value } }, soLan));
            }
            return ds;
        }

        private static KetQuaChay Do(SqlConnection conn, string ten, string sql, Func<SqlParameter[]> taoThamSo, int soLan)
        {
            int soDong = Chay(conn, sql, taoThamSo());                 // làm nóng
            var lan = new List<double>();
            for (int i = 0; i < Math.Max(1, soLan); i++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                soDong = Chay(conn, sql, taoThamSo());
                sw.Stop();
                lan.Add(sw.Elapsed.TotalMilliseconds);
            }
            return new KetQuaChay { Ten = ten, SoDong = soDong, Ms = TrungVi(lan) };
        }

        private static int Chay(SqlConnection conn, string sql, SqlParameter[] ps)
        {
            using (SqlCommand cmd = new SqlCommand(sql, conn) { CommandTimeout = TIMEOUT_LAU })
            {
                cmd.Parameters.AddRange(ps);
                int n = 0;
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read()) n++;        // đọc hết kết quả như app thật
                }
                return n;
            }
        }

        public static double TrungVi(IList<double> ds)
        {
            if (ds == null || ds.Count == 0) return 0;
            List<double> s = ds.OrderBy(x => x).ToList();
            int giua = s.Count / 2;
            return s.Count % 2 == 1 ? s[giua] : (s[giua - 1] + s[giua]) / 2.0;
        }
    }
}
