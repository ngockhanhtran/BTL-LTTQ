using System;

namespace QuanLyNhaThuoc.DTO
{
    /// <summary>[10/11/2026] Mã vai trò (khớp bảng VaiTro trong CSDL).</summary>
    public static class VaiTroNV
    {
        public const string QUAN_LY = "QL";   // tất cả chức năng + báo cáo, dashboard, nhân viên, danh mục
        public const string BAN_HANG = "BH";  // tra cứu thuốc (chỉ xem) + bán hàng
        public const string THU_KHO = "TK";   // tra cứu thuốc (sửa công dụng, chống chỉ định) + nhập hàng
    }

    /// <summary>
    /// [10/11/2026] Thông tin nhân viên đang đăng nhập (dùng chung toàn chương trình).
    /// </summary>
    public static class PhienDangNhap
    {
        public static string MaNV { get; private set; }
        public static string TenNV { get; private set; }
        public static string MaVaiTro { get; private set; }
        public static string TenVaiTro { get; private set; }
        public static DateTime ThoiDiemDangNhap { get; private set; }

        /// <summary>[10/11/2026 lần 5] Quản lý vừa đặt lại mật khẩu / NV mới -> phải đổi mật khẩu trước khi làm việc.</summary>
        public static bool PhaiDoiMK { get; set; }

        public static bool DaDangNhap { get { return !string.IsNullOrEmpty(MaNV); } }
        public static bool LaQuanLy { get { return MaVaiTro == VaiTroNV.QUAN_LY; } }
        public static bool LaBanHang { get { return MaVaiTro == VaiTroNV.BAN_HANG; } }
        public static bool LaThuKho { get { return MaVaiTro == VaiTroNV.THU_KHO; } }

        // Quyền theo yêu cầu:
        //  - Bán hàng: lập hóa đơn (quản lý + bán hàng)
        //  - Sửa công dụng / chống chỉ định ở trang Tra cứu: quản lý + thủ kho
        //  - Nhập hàng: quản lý + thủ kho
        // Chưa đăng nhập (chạy thử form riêng lẻ) thì cho phép tất cả.
        public static bool DuocBanHang { get { return !DaDangNhap || LaQuanLy || LaBanHang; } }
        public static bool DuocSuaThongTinThuoc { get { return !DaDangNhap || LaQuanLy || LaThuKho; } }
        public static bool DuocNhapHang { get { return !DaDangNhap || LaQuanLy || LaThuKho; } }

        public static void DangNhap(string maNV, string tenNV, string maVaiTro, string tenVaiTro)
        {
            MaNV = (maNV ?? "").Trim();
            TenNV = tenNV;
            MaVaiTro = (maVaiTro ?? "").Trim().ToUpperInvariant();
            TenVaiTro = tenVaiTro;
            ThoiDiemDangNhap = DateTime.Now;
            PhaiDoiMK = false;
        }

        public static void DangXuat()
        {
            MaNV = null;
            TenNV = null;
            MaVaiTro = null;
            TenVaiTro = null;
            PhaiDoiMK = false;
        }
    }
}
