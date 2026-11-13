using System;
using QuanLyNhaThuoc.DAL;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>[10/11/2026 lần 5] Lớp BLL cho form "Cấu hình kết nối" (GUI không gọi thẳng DAL).</summary>
    public static class KetNoiBLL
    {
        public static string[] MayChuGoiY { get { return CauHinhKetNoi.MAY_CHU_GOI_Y; } }
        public static string MayChu { get { return CauHinhKetNoi.MayChu; } }
        public static string TenCSDL { get { return CauHinhKetNoi.TenCSDL; } }
        public static bool XacThucWindows { get { return CauHinhKetNoi.XacThucWindows; } }
        public static string TaiKhoan { get { return CauHinhKetNoi.TaiKhoan; } }
        public static string MatKhau { get { return CauHinhKetNoi.MatKhau; } }
        public static string DuongDanFile { get { return CauHinhKetNoi.DuongDanFile; } }

        /// <summary>
        /// Khi mở chương trình: có cấu hình thì thử kết nối; chưa có thì tự dò máy chủ.
        /// Trả về null nếu kết nối được, ngược lại là lý do (để hiện form cấu hình).
        /// </summary>
        public static string KiemTraKhiKhoiDong()
        {
            if (CauHinhKetNoi.DaCoCauHinh)
                return CauHinhKetNoi.ThuKetNoiHienTai();
            if (CauHinhKetNoi.TuDoMayChu())
                return null;
            return "Không tự tìm thấy SQL Server có CSDL \"" + CauHinhKetNoi.TenCSDL + "\" (đã thử: " +
                   string.Join(", ", CauHinhKetNoi.MAY_CHU_GOI_Y) + ").";
        }

        public static string ThuKetNoi(string mayChu, string tenCSDL, bool windows, string taiKhoan, string matKhau)
        {
            string loi = KiemTraNhap(mayChu, tenCSDL, windows, taiKhoan);
            if (loi != null) return loi;
            return CauHinhKetNoi.ThuKetNoi(CauHinhKetNoi.TaoChuoi(mayChu, tenCSDL, windows, taiKhoan, matKhau, 5));
        }

        public static void Luu(string mayChu, string tenCSDL, bool windows, string taiKhoan, string matKhau)
        {
            string loi = KiemTraNhap(mayChu, tenCSDL, windows, taiKhoan);
            if (loi != null) throw new ArgumentException(loi);
            CauHinhKetNoi.MayChu = mayChu.Trim();
            CauHinhKetNoi.TenCSDL = tenCSDL.Trim();
            CauHinhKetNoi.XacThucWindows = windows;
            CauHinhKetNoi.TaiKhoan = windows ? "" : (taiKhoan ?? "").Trim();
            CauHinhKetNoi.MatKhau = windows ? "" : (matKhau ?? "");
            CauHinhKetNoi.Luu();
        }

        private static string KiemTraNhap(string mayChu, string tenCSDL, bool windows, string taiKhoan)
        {
            if (string.IsNullOrWhiteSpace(mayChu)) return "Vui lòng nhập tên máy chủ SQL Server.";
            if (string.IsNullOrWhiteSpace(tenCSDL)) return "Vui lòng nhập tên CSDL.";
            if (!windows && string.IsNullOrWhiteSpace(taiKhoan)) return "Vui lòng nhập tài khoản SQL Server (vd: sa).";
            return null;
        }
    }
}
