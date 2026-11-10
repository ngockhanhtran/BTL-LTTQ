using System.Collections.Generic;

namespace QuanLyNhaThuoc.DTO
{
    /// <summary>[10/11/2026] Mô tả 1 cột của bảng danh mục (để form frmDanhMuc tự sinh ô nhập).</summary>
    public class CotDanhMuc
    {
        public string Ten { get; set; }          // tên cột trong CSDL
        public string TieuDe { get; set; }       // nhãn hiển thị
        public bool BatBuoc { get; set; }
        public int DoDaiToiDa { get; set; }
        public bool ChiDoc { get; set; }         // chỉ hiển thị, không cho sửa, không ghi xuống CSDL
        public bool LaSo { get; set; }           // căn phải, định dạng N0

        public CotDanhMuc(string ten, string tieuDe, bool batBuoc, int doDaiToiDa)
        {
            Ten = ten;
            TieuDe = tieuDe;
            BatBuoc = batBuoc;
            DoDaiToiDa = doDaiToiDa;
        }
    }

    /// <summary>[10/11/2026] Cấu hình 1 bảng danh mục đơn giản (Khách hàng, NCC, Công dụng...).</summary>
    public class CauHinhDanhMuc
    {
        public string TieuDe { get; set; }       // tiêu đề form
        public string Bang { get; set; }         // tên bảng CSDL
        public string TienToMa { get; set; }     // tiền tố sinh mã tự động (KH, NCC, CD...)
        public int DoDaiSoMa { get; set; }       // số chữ số tối thiểu của mã
        public string ThongBaoKhongXoaDuoc { get; set; }

        /// <summary>Cột đầu tiên là khóa chính, cột thứ 2 là tên (dùng cho ComboBox).</summary>
        public List<CotDanhMuc> Cot { get; } = new List<CotDanhMuc>();

        public CotDanhMuc CotKhoa { get { return Cot[0]; } }
        public CotDanhMuc CotTen { get { return Cot[1]; } }
    }

    /// <summary>[10/11/2026] Các danh mục dùng form chung frmDanhMuc.</summary>
    public static class DanhSachDanhMuc
    {
        public static CauHinhDanhMuc KhachHang()
        {
            CauHinhDanhMuc c = new CauHinhDanhMuc
            {
                TieuDe = "Quản lý Khách hàng",
                Bang = "KhachHang",
                TienToMa = "KH",
                DoDaiSoMa = 3,
                ThongBaoKhongXoaDuoc = "Khách hàng đã có hóa đơn mua hàng nên không thể xóa."
            };
            c.Cot.Add(new CotDanhMuc("MaKhach", "Mã khách", true, 10));
            c.Cot.Add(new CotDanhMuc("TenKhach", "Tên khách", true, 50));
            c.Cot.Add(new CotDanhMuc("DienThoai", "Điện thoại", false, 20));
            c.Cot.Add(new CotDanhMuc("DiaChi", "Địa chỉ", false, 255));
            c.Cot.Add(new CotDanhMuc("DiemTichLuy", "Điểm tích lũy", false, 10) { ChiDoc = true, LaSo = true });
            return c;
        }

        public static CauHinhDanhMuc NhaCungCap()
        {
            CauHinhDanhMuc c = new CauHinhDanhMuc
            {
                TieuDe = "Quản lý Nhà cung cấp",
                Bang = "NhaCungCap",
                TienToMa = "NCC",
                DoDaiSoMa = 2,
                ThongBaoKhongXoaDuoc = "Nhà cung cấp đã có phiếu nhập hàng nên không thể xóa."
            };
            c.Cot.Add(new CotDanhMuc("MaNCC", "Mã NCC", true, 10));
            c.Cot.Add(new CotDanhMuc("TenNCC", "Tên nhà cung cấp", true, 100));
            c.Cot.Add(new CotDanhMuc("DienThoai", "Điện thoại", false, 20));
            c.Cot.Add(new CotDanhMuc("DiaChi", "Địa chỉ", false, 255));
            return c;
        }

        public static CauHinhDanhMuc CongDung()
        {
            CauHinhDanhMuc c = new CauHinhDanhMuc
            {
                TieuDe = "Quản lý Công dụng thuốc",
                Bang = "CongDung",
                TienToMa = "CD",
                DoDaiSoMa = 2,
                ThongBaoKhongXoaDuoc = "Công dụng đang được gán cho thuốc. Hãy bỏ công dụng này khỏi các thuốc trước khi xóa."
            };
            c.Cot.Add(new CotDanhMuc("MaCongDung", "Mã công dụng", true, 10));
            c.Cot.Add(new CotDanhMuc("TenCongDung", "Tên công dụng", true, 100));
            return c;
        }

        public static CauHinhDanhMuc DonViTinh()
        {
            CauHinhDanhMuc c = new CauHinhDanhMuc
            {
                TieuDe = "Danh mục Đơn vị tính",
                Bang = "DonViTinh",
                TienToMa = "DVT",
                DoDaiSoMa = 2,
                ThongBaoKhongXoaDuoc = "Đơn vị tính đang được dùng cho thuốc nên không thể xóa."
            };
            c.Cot.Add(new CotDanhMuc("MaDVT", "Mã ĐVT", true, 10));
            c.Cot.Add(new CotDanhMuc("TenDVT", "Tên đơn vị tính", true, 50));
            return c;
        }

        public static CauHinhDanhMuc DangDieuChe()
        {
            CauHinhDanhMuc c = new CauHinhDanhMuc
            {
                TieuDe = "Danh mục Dạng điều chế",
                Bang = "DangDieuChe",
                TienToMa = "DC",
                DoDaiSoMa = 2,
                ThongBaoKhongXoaDuoc = "Dạng điều chế đang được dùng cho thuốc nên không thể xóa."
            };
            c.Cot.Add(new CotDanhMuc("MaDangDC", "Mã dạng ĐC", true, 10));
            c.Cot.Add(new CotDanhMuc("TenDangDC", "Tên dạng điều chế", true, 50));
            return c;
        }

        public static CauHinhDanhMuc NuocSX()
        {
            CauHinhDanhMuc c = new CauHinhDanhMuc
            {
                TieuDe = "Danh mục Nước sản xuất",
                Bang = "NuocSX",
                TienToMa = "NSX",
                DoDaiSoMa = 2,
                ThongBaoKhongXoaDuoc = "Nước sản xuất đang được dùng cho thuốc nên không thể xóa."
            };
            c.Cot.Add(new CotDanhMuc("MaNuocSX", "Mã nước SX", true, 10));
            c.Cot.Add(new CotDanhMuc("TenNuocSX", "Tên nước sản xuất", true, 50));
            return c;
        }
    }
}
