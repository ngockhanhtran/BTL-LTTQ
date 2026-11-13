using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>
    /// [10/11/2026] Hàm dùng chung cho các DAO mới.
    /// - Lấy kết nối từ DBConnection => chuỗi kết nối vẫn chỉ sửa ở 1 chỗ (DBConnection.cs).
    /// - Khác DBConnection.ExecuteNonQuery: KHÔNG bọc lỗi, giữ nguyên SqlException
    ///   để nhận biết lỗi khóa ngoại (547) / trùng khóa (2627, 2601).
    /// </summary>
    public static class SqlHelper
    {
        public static SqlConnection MoKetNoi()
        {
            SqlConnection conn = new DBConnection().GetConnection();
            conn.Open();
            return conn;
        }

        /// <summary>Tham số: null hoặc chuỗi rỗng -> DBNull (cột cho phép NULL nhận NULL).</summary>
        public static SqlParameter P(string ten, object giaTri)
        {
            if (giaTri == null) return new SqlParameter(ten, DBNull.Value);
            string s = giaTri as string;
            if (s != null)
            {
                s = s.Trim();
                if (s.Length == 0) return new SqlParameter(ten, DBNull.Value);
                return new SqlParameter(ten, s);
            }
            return new SqlParameter(ten, giaTri);
        }

        /// <summary>Tham số chuỗi KHÔNG đổi sang NULL (dùng cho từ khóa tìm kiếm, mật khẩu...).</summary>
        public static SqlParameter PChuoi(string ten, string giaTri)
        {
            return new SqlParameter(ten, SqlDbType.NVarChar, 255) { Value = giaTri ?? "" };
        }

        // ================== KHÔNG TRANSACTION ==================

        public static DataTable Query(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = MoKetNoi())
                return Query(conn, null, sql, ps);
        }

        public static DataTable QueryProc(string tenProc, params SqlParameter[] ps)
        {
            using (SqlConnection conn = MoKetNoi())
            using (SqlCommand cmd = TaoLenh(conn, null, tenProc, CommandType.StoredProcedure, ps))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static object Scalar(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = MoKetNoi())
                return Scalar(conn, null, sql, ps);
        }

        public static int NonQuery(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = MoKetNoi())
                return NonQuery(conn, null, sql, ps);
        }

        // ================== TRONG TRANSACTION ==================

        public static DataTable Query(SqlConnection conn, SqlTransaction tx, string sql, params SqlParameter[] ps)
        {
            using (SqlCommand cmd = TaoLenh(conn, tx, sql, CommandType.Text, ps))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        /// <summary>Trả về null nếu không có dòng nào hoặc giá trị là DBNull.</summary>
        public static object Scalar(SqlConnection conn, SqlTransaction tx, string sql, params SqlParameter[] ps)
        {
            using (SqlCommand cmd = TaoLenh(conn, tx, sql, CommandType.Text, ps))
            {
                object kq = cmd.ExecuteScalar();
                return kq == DBNull.Value ? null : kq;
            }
        }

        public static int NonQuery(SqlConnection conn, SqlTransaction tx, string sql, params SqlParameter[] ps)
        {
            using (SqlCommand cmd = TaoLenh(conn, tx, sql, CommandType.Text, ps))
                return cmd.ExecuteNonQuery();
        }

        /// <summary>[10/11/2026 lần 5] Gọi stored procedure trong transaction (tham số OUTPUT đọc lại sau khi gọi).</summary>
        public static int ExecProc(SqlConnection conn, SqlTransaction tx, string tenProc, params SqlParameter[] ps)
        {
            using (SqlCommand cmd = TaoLenh(conn, tx, tenProc, CommandType.StoredProcedure, ps))
            {
                int kq = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();     // để dùng lại SqlParameter (đọc giá trị OUTPUT) sau khi lệnh bị hủy
                return kq;
            }
        }

        public static DataTable QueryProc(SqlConnection conn, SqlTransaction tx, string tenProc, params SqlParameter[] ps)
        {
            using (SqlCommand cmd = TaoLenh(conn, tx, tenProc, CommandType.StoredProcedure, ps))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private static SqlCommand TaoLenh(SqlConnection conn, SqlTransaction tx, string sql, CommandType loai, SqlParameter[] ps)
        {
            SqlCommand cmd = new SqlCommand(sql, conn, tx) { CommandType = loai };
            if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
            return cmd;
        }

        // ================== SINH MÃ TỰ ĐỘNG ==================

        /// <summary>
        /// Sinh mã kế tiếp theo tiền tố, vd: tienTo "CD" + CD10 lớn nhất -> CD11.
        /// Giữ độ dài phần số của mã lớn nhất hiện có (NV03 -> NV04, QL001 -> QL002);
        /// chưa có mã nào thì dùng doDaiToiThieu chữ số (TK + 3 -> TK001).
        /// bang / cot là tên cố định trong code (không lấy từ người dùng).
        /// </summary>
        public static string SinhMaTiepTheo(SqlConnection conn, SqlTransaction tx, string bang, string cot,
                                            string tienTo, int doDaiToiThieu)
        {
            DataTable dt = Query(conn, tx,
                "SELECT [" + cot + "] FROM [" + bang + "] WHERE [" + cot + "] LIKE @TienTo + '%'",
                PChuoi("@TienTo", tienTo));

            int soLonNhat = 0;
            int doDai = doDaiToiThieu;
            Regex re = new Regex("^" + Regex.Escape(tienTo) + @"(\d+)$", RegexOptions.IgnoreCase);
            foreach (DataRow r in dt.Rows)
            {
                Match m = re.Match(Convert.ToString(r[0]).Trim());
                if (!m.Success) continue;
                int so;
                if (!int.TryParse(m.Groups[1].Value, out so)) continue;
                if (so >= soLonNhat)
                {
                    soLonNhat = so;
                    doDai = m.Groups[1].Value.Length;
                }
            }
            return tienTo + (soLonNhat + 1).ToString().PadLeft(doDai, '0');
        }

        public static string SinhMaTiepTheo(string bang, string cot, string tienTo, int doDaiToiThieu)
        {
            using (SqlConnection conn = MoKetNoi())
                return SinhMaTiepTheo(conn, null, bang, cot, tienTo, doDaiToiThieu);
        }

        // ================== NHẬN BIẾT LỖI ==================

        /// <summary>
        /// Lỗi khóa ngoại (xóa bản ghi đang được bảng khác dùng).
        /// [11/11/2026 lần 6] Lỗi 547 dùng chung cho khóa ngoại VÀ ràng buộc CHECK
        /// -> loại trừ CHECK, nếu không sửa giá âm sẽ bị báo nhầm "đang được sử dụng".
        /// </summary>
        public static bool LaLoiKhoaNgoai(Exception ex)
        {
            SqlException s = ex as SqlException;
            return s != null && s.Number == 547 && !LaLoiRangBuoc(ex);
        }

        /// <summary>
        /// [11/11/2026 lần 6] Lỗi vi phạm ràng buộc CHECK (CK_...) - xem Phần 7 file CSDL.
        /// Tên ràng buộc luôn bắt đầu bằng "CK_" và nằm trong ngoặc kép ở thông báo lỗi.
        /// </summary>
        public static bool LaLoiRangBuoc(Exception ex)
        {
            SqlException s = ex as SqlException;
            return s != null && s.Number == 547 && s.Message.IndexOf("\"CK_", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool LaLoiTrungKhoa(Exception ex)
        {
            SqlException s = ex as SqlException;
            return s != null && (s.Number == 2627 || s.Number == 2601);
        }
    }
}
