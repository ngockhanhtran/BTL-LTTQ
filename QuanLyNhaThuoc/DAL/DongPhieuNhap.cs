using System;
using System.Collections.Generic;

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

        // [10/11/2026] Cột tùy chọn mới
        public string MaVach { get; set; }           // mã vạch in trên hộp (gán cho thuốc, kể cả thuốc đã có)
        public string SoLo { get; set; } = "";       // [10/11/2026 lần 5] số lô (NgaySX, HanSD ở trên = của lô này)
        public string CongDung { get; set; }         // tên công dụng, nhiều cái cách nhau bằng dấu ;

        /// <summary>[10/11/2026] Tách cột CongDung theo dấu ; (bỏ trống, bỏ trùng).</summary>
        public List<string> DanhSachCongDung
        {
            get
            {
                List<string> ds = new List<string>();
                foreach (string s in (CongDung ?? "").Split(';'))
                {
                    string t = s.Trim();
                    if (t != "" && !ds.Exists(x => string.Equals(x, t, StringComparison.OrdinalIgnoreCase))) ds.Add(t);
                }
                return ds;
            }
        }

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
        public int SoCongDungMoi { get; set; }      // [10/11/2026]
    }
}