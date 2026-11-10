using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.DAL
{
    public class DBConnection
    {
        // [10/11/2026 lần 5] Chuỗi kết nối KHÔNG còn viết cứng ở đây nữa (mỗi máy một tên SQL Server khác nhau).
        // Lấy từ DAL/CauHinhKetNoi.cs: tự dò lần đầu, sửa được bằng form "Cấu hình kết nối".
        // => File này giống nhau trên mọi máy, đưa lên GitHub thoải mái.
        // Hàm mở kế nối
        public SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(CauHinhKetNoi.ChuoiKetNoi);
            // [10/11/2026 lần 5] mở kết nối xong thì báo cho SQL Server biết ai đang thao tác
            // (trigger nhật ký đổi giá đọc lại bằng dbo.fn_NguoiDungHienTai)
            conn.StateChange += GanNguoiDungVaoKetNoi;
            return conn;
        }

        private static void GanNguoiDungVaoKetNoi(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState != ConnectionState.Open) return;
            SqlConnection conn = sender as SqlConnection;
            if (conn == null) return;
            try
            {
                byte[] ma = Encoding.ASCII.GetBytes(PhienDangNhap.MaNV ?? "");
                if (ma.Length == 0) ma = new byte[] { 0 };
                using (SqlCommand cmd = new SqlCommand("SET CONTEXT_INFO @Ma", conn))
                {
                    cmd.Parameters.Add("@Ma", SqlDbType.VarBinary, 128).Value = ma;
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // không gán được thì nhật ký chỉ thiếu tên người sửa, không chặn chương trình
            }
        }

        // Truy vấn trả về DataTable (Dùng để select, xem danh sách, báo cáo
        public DataTable ExecuteQuery(string sql, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Nếu là Stored Procedure, đổi CommandType thành StoredProcedure
                    // cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
        // Không trả về dữ liệu (insert, update,delete)
        public bool ExecuteNonQuery(string sql, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if(parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thực thi CSDL: " + ex.Message);
            }
        }
    }
}
