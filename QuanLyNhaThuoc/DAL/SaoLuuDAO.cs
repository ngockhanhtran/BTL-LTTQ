using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>
    /// [10/11/2026 lần 5] Sao lưu / khôi phục CSDL bằng lệnh BACKUP / RESTORE của SQL Server.
    /// Lưu ý: file .bak do DỊCH VỤ SQL Server ghi / đọc (không phải chương trình này), nên thư mục phải
    /// nằm trên máy chạy SQL Server và tài khoản dịch vụ có quyền -> mặc định dùng thư mục Backup của SQL Server.
    /// </summary>
    public class SaoLuuDAO
    {
        private static string TenCSDL { get { return CauHinhKetNoi.TenCSDL; } }

        /// <summary>Kết nối vào master (khôi phục không thể đứng trong chính CSDL đang khôi phục).</summary>
        private static SqlConnection KetNoiMaster()
        {
            SqlConnectionStringBuilder b = new SqlConnectionStringBuilder(CauHinhKetNoi.ChuoiKetNoi)
            {
                InitialCatalog = "master",
                Pooling = false
            };
            SqlConnection c = new SqlConnection(b.ConnectionString);
            c.Open();
            return c;
        }

        /// <summary>Thư mục Backup mặc định của SQL Server (null nếu không đọc được).</summary>
        public string LayThuMucMacDinh()
        {
            using (SqlConnection c = KetNoiMaster())
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CAST(SERVERPROPERTY('InstanceDefaultBackupPath') AS NVARCHAR(400))", c))
                    {
                        object o = cmd.ExecuteScalar();
                        if (o != null && o != DBNull.Value && o.ToString().Trim() != "") return o.ToString().Trim();
                    }
                }
                catch { }
                try
                {
                    using (SqlCommand cmd = new SqlCommand(@"
                        DECLARE @ThuMuc NVARCHAR(400);
                        EXEC master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE',
                             N'Software\Microsoft\MSSQLServer\MSSQLServer', N'BackupDirectory', @ThuMuc OUTPUT;
                        SELECT @ThuMuc;", c))
                    {
                        object o = cmd.ExecuteScalar();
                        if (o != null && o != DBNull.Value && o.ToString().Trim() != "") return o.ToString().Trim();
                    }
                }
                catch { }
            }
            return null;
        }

        public void SaoLuu(string duongDan)
        {
            using (SqlConnection c = KetNoiMaster())
            using (SqlCommand cmd = new SqlCommand(
                "BACKUP DATABASE " + TenCoNgoac(TenCSDL) + " TO DISK = @File WITH INIT, NAME = @Ten, STATS = 10", c))
            {
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@File", SqlDbType.NVarChar, 400).Value = duongDan;
                cmd.Parameters.Add("@Ten", SqlDbType.NVarChar, 128).Value =
                    TenCSDL + " - Sao lưu từ phần mềm " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Kiểm tra file sao lưu đọc được (RESTORE VERIFYONLY) trước khi khôi phục.</summary>
        public void KiemTraFile(string duongDan)
        {
            using (SqlConnection c = KetNoiMaster())
            using (SqlCommand cmd = new SqlCommand("RESTORE VERIFYONLY FROM DISK = @File", c))
            {
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@File", SqlDbType.NVarChar, 400).Value = duongDan;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Khôi phục: ngắt mọi kết nối khác (SINGLE_USER), RESTORE ... WITH REPLACE, rồi mở lại MULTI_USER.</summary>
        public void KhoiPhuc(string duongDan)
        {
            SqlConnection.ClearAllPools();      // trả các kết nối đang giữ trong pool của chương trình
            string ten = TenCoNgoac(TenCSDL);
            using (SqlConnection c = KetNoiMaster())
            {
                try
                {
                    Chay(c, "ALTER DATABASE " + ten + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE", null);
                    Chay(c, "RESTORE DATABASE " + ten + " FROM DISK = @File WITH REPLACE, STATS = 10", duongDan);
                }
                finally
                {
                    try { Chay(c, "ALTER DATABASE " + ten + " SET MULTI_USER", null); } catch { }
                }
            }
            SqlConnection.ClearAllPools();
        }

        /// <summary>Lịch sử sao lưu của CSDL (msdb.dbo.backupset), mới nhất trước.</summary>
        public DataTable LichSuSaoLuu()
        {
            using (SqlConnection c = KetNoiMaster())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT TOP 50 b.backup_finish_date AS ThoiGian,
                       m.physical_device_name     AS DuongDan,
                       CAST(b.backup_size / 1048576.0 AS DECIMAL(10,2)) AS KichThuocMB,
                       b.name                     AS GhiChu
                FROM msdb.dbo.backupset b
                JOIN msdb.dbo.backupmediafamily m ON m.media_set_id = b.media_set_id
                WHERE b.database_name = @Ten AND b.type = 'D'
                ORDER BY b.backup_finish_date DESC", c))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@Ten", SqlDbType.NVarChar, 128).Value = TenCSDL;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private static void Chay(SqlConnection c, string sql, string duongDan)
        {
            using (SqlCommand cmd = new SqlCommand(sql, c))
            {
                cmd.CommandTimeout = 0;
                if (duongDan != null) cmd.Parameters.Add("@File", SqlDbType.NVarChar, 400).Value = duongDan;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Tên CSDL trong [ ] (tên lấy từ cấu hình, chặn ký tự lạ để tránh chèn lệnh).</summary>
        private static string TenCoNgoac(string ten)
        {
            ten = (ten ?? "").Trim();
            foreach (char ch in ten)
                if (!(char.IsLetterOrDigit(ch) || ch == '_' || ch == '-'))
                    throw new InvalidOperationException("Tên CSDL \"" + ten + "\" có ký tự không hợp lệ.");
            if (ten == "") throw new InvalidOperationException("Chưa cấu hình tên CSDL.");
            return "[" + ten + "]";
        }
    }
}
