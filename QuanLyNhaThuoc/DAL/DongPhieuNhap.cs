using System;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>
    /// Một dòng chi tiết phiếu nhập (đọc từ file Excel).
    /// Mỗi dòng = 1 thuốc. Nhiều dòng cùng MaHDN = 1 phiếu nhập.
    /// </summary>
    public class DongPhieuNhap
    {
        public int DongExcel { get; set; }          // số dòng trong file Excel (để báo lỗi)

        // Thông tin phiếu
        public string MaHDN { get; set; }
        public DateTime? NgayNhap { get; set; }
        public string MaNV { get; set; }             // thủ kho phụ trách
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }           // chỉ cần khi NCC chưa có trong CSDL

        // Thông tin thuốc (các cột sau MaThuoc chỉ cần khi là thuốc MỚI)
        public string MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public string MaDVT { get; set; }
        public string MaDangDC { get; set; }
        public string ThanhPhan { get; set; }
        public string MaNuocSX { get; set; }
        public DateTime? NgaySX { get; set; }
        public DateTime? HanSD { get; set; }
        public string ChongChiDinh { get; set; }

        // Chi tiết nhập
        public int SLNhap { get; set; }
        public decimal DonGia { get; set; }
        public decimal KhuyenMai { get; set; }       // số tiền được giảm cho dòng này

        public decimal ThanhTien
        {
            get
            {
                decimal t = SLNhap * DonGia - KhuyenMai;
                return t < 0 ? 0 : t;
            }
        }
    }

    /// <summary>Kết quả sau khi import xong.</summary>
    public class ThongKeImport
    {
        public int SoPhieu { get; set; }
        public int SoThuocMoi { get; set; }
        public int SoNCCMoi { get; set; }
    }
}