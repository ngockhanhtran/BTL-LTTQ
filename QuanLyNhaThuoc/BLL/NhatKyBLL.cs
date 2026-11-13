using System;
using System.Data;
using QuanLyNhaThuoc.DAL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>
    /// [10/11/2026 lần 5] Nhật ký hoạt động: ai, lúc nào, làm gì.
    /// Ghi nhật ký KHÔNG được làm hỏng thao tác chính -> lỗi khi ghi bị bỏ qua.
    /// Đổi giá thuốc được trigger trg_Thuoc_NhatKyGia trong CSDL tự ghi.
    /// </summary>
    public static class NhatKyBLL
    {
        // Tên hành động (dùng thống nhất để lọc)
        public const string DANG_NHAP = "Đăng nhập";
        public const string DANG_NHAP_SAI = "Đăng nhập sai";
        public const string DANG_XUAT = "Đăng xuất";
        public const string DOI_MAT_KHAU = "Đổi mật khẩu";
        public const string QUEN_MAT_KHAU = "Quên mật khẩu";
        public const string DAT_LAI_MAT_KHAU = "Đặt lại mật khẩu";
        public const string NHAN_VIEN = "Quản lý nhân viên";
        public const string THUOC = "Danh mục thuốc";
        public const string THANH_TOAN = "Thanh toán";
        public const string TRA_HANG = "Trả hàng";
        public const string HUY_HOA_DON = "Hủy hóa đơn";
        public const string NHAP_HANG = "Nhập hàng";
        public const string HUY_THUOC = "Hủy thuốc hết hạn";
        public const string SAO_LUU = "Sao lưu CSDL";
        public const string KHOI_PHUC = "Khôi phục CSDL";
        // [11/11/2026 lần 7] tính năng nâng cao
        public const string TUONG_TAC = "Tương tác thuốc";
        public const string IN_TEM = "In tem mã vạch";
        public const string HIEU_NANG = "Đo hiệu năng";
        public const string BAN_DU_TUONG_TAC = "Bán dù có tương tác";

        private static readonly NhatKyDAO dao = new NhatKyDAO();

        public static void Ghi(string hanhDong, string doiTuong, string chiTiet)
        {
            GhiCho(PhienDangNhap.MaNV, PhienDangNhap.TenNV, hanhDong, doiTuong, chiTiet);
        }

        /// <summary>Ghi cho NV chỉ định (vd đăng nhập sai: chưa có phiên đăng nhập).</summary>
        public static void GhiCho(string maNV, string tenNV, string hanhDong, string doiTuong, string chiTiet)
        {
            try
            {
                dao.Ghi(string.IsNullOrWhiteSpace(maNV) ? null : maNV.Trim(), tenNV, hanhDong, doiTuong, chiTiet);
            }
            catch
            {
                // bỏ qua: CSDL cũ chưa có bảng nhật ký / mất kết nối...
            }
        }

        public static DataTable LayDanhSach(DateTime tuNgay, DateTime denNgay, string maNV, string hanhDong, string tuKhoa)
        {
            if (denNgay.Date < tuNgay.Date) throw new ArgumentException("\"Đến ngày\" phải sau \"Từ ngày\".");
            return dao.LayDanhSach(tuNgay, denNgay, maNV, hanhDong, tuKhoa);
        }

        public static DataTable LayDanhSachHanhDong() { return dao.LayDanhSachHanhDong(); }
        public static DataTable LayDanhSachNhanVien() { return dao.LayDanhSachNhanVien(); }
    }
}
