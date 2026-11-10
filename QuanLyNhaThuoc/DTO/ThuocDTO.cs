using System;
using System.Collections.Generic;

namespace QuanLyNhaThuoc.DTO
{
    /// <summary>[10/11/2026] Thông tin thuốc dùng cho form Danh mục thuốc (thêm / sửa).</summary>
    public class ThuocDTO
    {
        public string MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public string MaVach { get; set; }
        public string MaDVT { get; set; }
        public string MaDangDC { get; set; }
        public string ThanhPhan { get; set; }
        public decimal DonGiaNhap { get; set; }
        public DateTime? NgaySX { get; set; }
        public string MaNuocSX { get; set; }
        public DateTime? HanSD { get; set; }
        public string ChongChiDinh { get; set; }
        public bool KeDon { get; set; }              // [10/11/2026 lần 5] thuốc kê đơn

        /// <summary>Danh sách MaCongDung của thuốc.</summary>
        public List<string> CongDung { get; set; } = new List<string>();

        /// <summary>Yêu cầu 3: giá bán = 110% giá nhập.</summary>
        public decimal GiaBan { get { return Math.Round(DonGiaNhap * 1.1m, 2); } }
    }

    /// <summary>[10/11/2026] Thuốc tìm được khi quét mã vạch / nhập mã ở trang Bán hàng.</summary>
    public class ThuocBan
    {
        public string MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public string MaVach { get; set; }
        public decimal GiaBan { get; set; }
        public int SLHienCo { get; set; }
        public DateTime? HanSD { get; set; }
        public int SLBanDuoc { get; set; }           // [10/11/2026 lần 5] SL các lô CÒN HẠN (bán theo FEFO)
        public bool KeDon { get; set; }              // [10/11/2026 lần 5]
    }
}
