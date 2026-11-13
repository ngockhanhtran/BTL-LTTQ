using System;

namespace QuanLyNhaThuoc.DTO
{
    /// <summary>[10/11/2026] Thông tin 1 nhân viên (form Quản lý nhân viên).</summary>
    public class NhanVienDTO
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string MaTrinhDo { get; set; }
        public string MaChuyenMon { get; set; }
        public string MaVaiTro { get; set; }
        /// <summary>
        /// [10/11/2026 lần 5] Mật khẩu MỚI nhập trên form (chữ thường, chỉ nằm trong bộ nhớ).
        /// Sửa NV: để trống = giữ mật khẩu cũ. BLL băm thành MatKhauHash + MatKhauSalt trước khi lưu.
        /// </summary>
        public string MatKhau { get; set; }
        public bool NghiViec { get; set; }

        // [10/11/2026 lần 5] do BLL điền, DAO lưu xuống CSDL
        public string MatKhauHash { get; set; }
        public string MatKhauSalt { get; set; }
        public bool PhaiDoiMK { get; set; }
    }
}
