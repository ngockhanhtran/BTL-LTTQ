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
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Lỗi không bắt được -> hiện thông báo thay vì tắt chương trình
            Application.ThreadException += (s, e) =>
                MessageBox.Show("Đã xảy ra lỗi: " + e.Exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            while (true)
            {
                using (frmDangNhap dn = new frmDangNhap())
                {
                    if (dn.ShowDialog() != DialogResult.OK) return;   // đóng form đăng nhập = thoát
                }

                frmMain main = new frmMain();
                Application.Run(main);

                bool dangXuat = main.DangXuat;
                PhienDangNhap.DangXuat();
                if (!dangXuat) return;                               // đóng cửa sổ chính = thoát
            }
        }
    }
}
