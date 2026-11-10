using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaThuoc.DAL
{
    public class DBConnection
    {
        private string connString = @"Data Source=.;Initial Catalog=QuanLyNhaThuoc;Integrated Security=True";        // Hàm mở kế nối
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connString);
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
