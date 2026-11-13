using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;
using QuanLyNhaThuoc.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaThuoc
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// [10/11/2026] Thêm đăng nhập: frmDangNhap -> frmMain (menu theo vai trò).
        /// Bấm "Đăng xuất" ở frmMain thì quay lại màn hình đăng nhập.
        /// [10/11/2026 lần 5]
        ///   - Kiểm tra kết nối CSDL trước khi đăng nhập (tự dò máy chủ, không được thì mở form Cấu hình kết nối).
        ///   - NV mới / vừa được quản lý đặt lại mật khẩu: bắt buộc đổi mật khẩu trước khi vào chương trình.
        ///   - Ghi nhật ký đăng xuất.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Lỗi không bắt được -> hiện thông báo thay vì tắt chương trình
            Application.ThreadException += (s, e) =>
                MessageBox.Show("Đã xảy ra lỗi: " + GiaoDien.DichLoi(e.Exception), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);   // [11/11/2026 lần 6]
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            if (!DamBaoKetNoi()) return;

            NhanVienBLL nvBLL = new NhanVienBLL();
            while (true)
            {
                using (frmDangNhap dn = new frmDangNhap())
                {
                    if (dn.ShowDialog() != DialogResult.OK) return;   // đóng form đăng nhập = thoát
                }

                if (PhienDangNhap.PhaiDoiMK)
                {
                    using (frmDoiMatKhau doiMK = new frmDoiMatKhau(true))
                    {
                        if (doiMK.ShowDialog() != DialogResult.OK)
                        {
                            nvBLL.DangXuat();                            // không đổi -> quay lại đăng nhập
                            continue;
                        }
                    }
                }

                frmMain main = new frmMain();
                Application.Run(main);

                bool dangXuat = main.DangXuat;
                nvBLL.DangXuat();
                if (!dangXuat) return;                               // đóng cửa sổ chính = thoát
            }
        }

        /// <summary>[10/11/2026 lần 5] true nếu kết nối được CSDL (có thể sau khi người dùng cấu hình lại).</summary>
        private static bool DamBaoKetNoi()
        {
            string loi;
            Cursor.Current = Cursors.WaitCursor;
            try { loi = KetNoiBLL.KiemTraKhiKhoiDong(); }
            finally { Cursor.Current = Cursors.Default; }
            if (loi == null) return true;

            using (frmCauHinhKetNoi f = new frmCauHinhKetNoi(loi))
                return f.ShowDialog() == DialogResult.OK;
        }
    }
}
