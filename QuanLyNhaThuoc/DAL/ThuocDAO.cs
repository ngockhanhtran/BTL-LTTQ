using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaThuoc.DAL
{
    public class ThuocDAO
    {
        DBConnection db = new DBConnection();
        // 1. 
        public DataTable GetDanhSachThuoc()
        {
            string sql = "SELECT * FROM Thuoc";
            return db.ExecuteQuery(sql);
        }
        // 2. 
        public DataTable TimKiemThuoc(string tuKhoa)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TuKhoa", SqlDbType.NVarChar){Value = tuKhoa}
            };
            // Goi proc trong sql
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemThuoc", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
        public bool ThemThuoc(string maThuoc, string tenThuoc, string maDVT, string maDangDC,
                              string thanhPhan, decimal donGiaNhap, int slHienCo, DateTime ngaySX,
                              string maNuocSX, DateTime hanSD, string chongChiDinh)
        {
            // Yeu cau 3. Gia ban = 110% x Gia nhap
            decimal giaBan = donGiaNhap * 1.1m;

            string sql = @"INSERT INTO Thuoc (MaThuoc, TenThuoc, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan, SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh) 
                           VALUES (@MaThuoc, @TenThuoc, @MaDVT, @MaDangDC, @ThanhPhan, @DonGiaNhap, @GiaBan, @SLHienCo, @NgaySX, @MaNuocSX, @HanSD, @ChongChiDinh)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaThuoc", maThuoc),
                new SqlParameter("@TenThuoc", tenThuoc),
                new SqlParameter("@MaDVT", maDVT),
                new SqlParameter("@MaDangDC", maDangDC),
                new SqlParameter("@ThanhPhan", thanhPhan),
                new SqlParameter("@DonGiaNhap", donGiaNhap),
                new SqlParameter("@GiaBan", giaBan),
                new SqlParameter("@SLHienCo", slHienCo),
                new SqlParameter("@NgaySX", ngaySX),
                new SqlParameter("@MaNuocSX", maNuocSX),
                new SqlParameter("@HanSD", hanSD),
                new SqlParameter("@ChongChiDinh", chongChiDinh)
            };

            return db.ExecuteNonQuery(sql, parameters);
        }
    }

}
