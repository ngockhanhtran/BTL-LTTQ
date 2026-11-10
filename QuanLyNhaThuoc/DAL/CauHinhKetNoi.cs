using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>
    /// [10/11/2026 lần 5] Cấu hình kết nối SQL Server - KHÔNG còn sửa chuỗi kết nối trong code.
    /// - Lưu ở %AppData%\QuanLyNhaThuoc\ketnoi.cfg (mỗi máy 1 file, không đưa lên GitHub).
    /// - Lần đầu chạy: tự dò các tên máy chủ hay gặp (.\SQLEXPRESS, ., (localdb)\MSSQLLocalDB).
    /// - Dò không được: Program mở form "Cấu hình kết nối" (GUI/frmCauHinhKetNoi).
    /// - Mật khẩu SQL (nếu dùng SQL Server Authentication) được mã hóa bằng DPAPI của Windows.
    /// </summary>
    public static class CauHinhKetNoi
    {
        public const string CSDL_MAC_DINH = "QuanLyNhaThuoc";

        /// <summary>Tên máy chủ thường gặp, thử lần lượt khi chưa có cấu hình.</summary>
        public static readonly string[] MAY_CHU_GOI_Y = { @".\SQLEXPRESS", ".", @"(localdb)\MSSQLLocalDB", "localhost" };

        public static string MayChu { get; set; } = @".\SQLEXPRESS";
        public static string TenCSDL { get; set; } = CSDL_MAC_DINH;
        public static bool XacThucWindows { get; set; } = true;
        public static string TaiKhoan { get; set; } = "";
        public static string MatKhau { get; set; } = "";

        /// <summary>true khi đã đọc được file cấu hình (hoặc vừa lưu).</summary>
        public static bool DaCoCauHinh { get; private set; }

        private static readonly string ThuMuc =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "QuanLyNhaThuoc");
        public static readonly string DuongDanFile = Path.Combine(ThuMuc, "ketnoi.cfg");

        static CauHinhKetNoi()
        {
            Doc();
        }

        public static string ChuoiKetNoi
        {
            get { return TaoChuoi(MayChu, TenCSDL, XacThucWindows, TaiKhoan, MatKhau, 15); }
        }

        public static string TaoChuoi(string mayChu, string csdl, bool windows, string taiKhoan, string matKhau, int timeoutGiay)
        {
            SqlConnectionStringBuilder b = new SqlConnectionStringBuilder
            {
                DataSource = string.IsNullOrWhiteSpace(mayChu) ? "." : mayChu.Trim(),
                InitialCatalog = string.IsNullOrWhiteSpace(csdl) ? CSDL_MAC_DINH : csdl.Trim(),
                IntegratedSecurity = windows,
                ConnectTimeout = timeoutGiay,
                ApplicationName = "QuanLyNhaThuoc"
            };
            if (!windows)
            {
                b.UserID = taiKhoan ?? "";
                b.Password = matKhau ?? "";
            }
            return b.ConnectionString;
        }

        /// <summary>Thử kết nối. Thành công -> null; thất bại -> thông báo lỗi dễ hiểu.</summary>
        public static string ThuKetNoi(string chuoi)
        {
            try
            {
                using (SqlConnection c = new SqlConnection(chuoi))
                {
                    c.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT OBJECT_ID('LoThuoc', 'U')", c))
                    {
                        if (cmd.ExecuteScalar() == DBNull.Value)
                            return "Kết nối được nhưng CSDL chưa có bảng mới (LoThuoc).\nHãy chạy lại file CSDL_QuanLyNhaThuoc.sql.";
                    }
                }
                return null;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 4060)
                    return "Máy chủ đúng nhưng chưa có CSDL \"" + new SqlConnectionStringBuilder(chuoi).InitialCatalog +
                           "\".\nHãy mở SSMS và chạy file CSDL_QuanLyNhaThuoc.sql.";
                if (ex.Number == 18456)
                    return "Sai tài khoản hoặc mật khẩu SQL Server.";
                if (ex.Number == -1 || ex.Number == 2 || ex.Number == 53 || ex.Number == 26)
                    return "Không tìm thấy máy chủ SQL Server \"" + new SqlConnectionStringBuilder(chuoi).DataSource +
                           "\".\nKiểm tra tên máy chủ (xem ở cửa sổ Connect của SSMS) và dịch vụ SQL Server đang chạy.";
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string ThuKetNoiHienTai()
        {
            return ThuKetNoi(ChuoiKetNoi);
        }

        /// <summary>
        /// Chưa có file cấu hình: thử lần lượt các máy chủ gợi ý (Windows Authentication),
        /// máy nào có CSDL thì lưu lại. Trả về true nếu tìm được.
        /// </summary>
        public static bool TuDoMayChu()
        {
            foreach (string may in MAY_CHU_GOI_Y)
            {
                string chuoi = TaoChuoi(may, TenCSDL, true, "", "", 3);
                if (ThuKetNoi(chuoi) == null)
                {
                    MayChu = may;
                    XacThucWindows = true;
                    Luu();
                    return true;
                }
            }
            return false;
        }

        // ================== ĐỌC / GHI FILE ==================

        public static void Doc()
        {
            DaCoCauHinh = false;
            try
            {
                if (!File.Exists(DuongDanFile)) return;
                Dictionary<string, string> d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (string dong in File.ReadAllLines(DuongDanFile, Encoding.UTF8))
                {
                    int i = dong.IndexOf('=');
                    if (i > 0) d[dong.Substring(0, i).Trim()] = dong.Substring(i + 1).Trim();
                }
                string v;
                if (d.TryGetValue("MayChu", out v) && v != "") MayChu = v;
                if (d.TryGetValue("TenCSDL", out v) && v != "") TenCSDL = v;
                if (d.TryGetValue("XacThucWindows", out v)) XacThucWindows = v != "0";
                if (d.TryGetValue("TaiKhoan", out v)) TaiKhoan = v;
                if (d.TryGetValue("MatKhau", out v)) MatKhau = GiaiMa(v);
                DaCoCauHinh = true;
            }
            catch
            {
                DaCoCauHinh = false;      // file hỏng -> coi như chưa cấu hình
            }
        }

        public static void Luu()
        {
            Directory.CreateDirectory(ThuMuc);
            string[] dong =
            {
                "# Cau hinh ket noi SQL Server - Quan ly nha thuoc (tu tao, co the xoa de cau hinh lai)",
                "MayChu=" + MayChu,
                "TenCSDL=" + TenCSDL,
                "XacThucWindows=" + (XacThucWindows ? "1" : "0"),
                "TaiKhoan=" + (XacThucWindows ? "" : TaiKhoan),
                "MatKhau=" + (XacThucWindows ? "" : MaHoa(MatKhau))
            };
            File.WriteAllLines(DuongDanFile, dong, Encoding.UTF8);
            DaCoCauHinh = true;
            SqlConnection.ClearAllPools();
        }

        // DPAPI: chỉ đúng tài khoản Windows đã lưu mới giải mã được
        private static string MaHoa(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            byte[] b = ProtectedData.Protect(Encoding.UTF8.GetBytes(s), null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(b);
        }

        private static string GiaiMa(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            try
            {
                byte[] b = ProtectedData.Unprotect(Convert.FromBase64String(s), null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(b);
            }
            catch
            {
                return "";
            }
        }
    }
}
