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
        public string MatKhau { get; set; }
        public bool NghiViec { get; set; }
    }
}
