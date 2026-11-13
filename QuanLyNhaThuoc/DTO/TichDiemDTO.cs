using System;
using System.Collections.Generic;

namespace QuanLyNhaThuoc.DTO
{
    /// <summary>
    /// [10/11/2026] Quy định tích điểm khách hàng. Muốn đổi quy định chỉ cần sửa các hằng số ở đây.
    ///
    /// TÍCH ĐIỂM (cộng sau mỗi hóa đơn):
    ///   1. Theo hóa đơn  : mỗi 10.000đ khách THỰC TRẢ = 1 điểm gốc.
    ///   2. Theo số lần mua: hạng thành viên nhân hệ số điểm gốc
    ///        - Thành viên: đã mua 0 - 4 lần  -> x1.0
    ///        - Bạc       : đã mua 5 - 9 lần  -> x1.2
    ///        - Vàng      : đã mua từ 10 lần  -> x1.5
    ///      và thưởng thêm 20 điểm ở lần mua thứ 5, 10, 15, ...
    ///
    /// ĐỔI ĐIỂM (giảm tiền ngay trên hóa đơn):
    ///   - 1 điểm = 500đ.
    ///   - Phải có ít nhất 20 điểm mới được đổi.
    ///   - Giảm tối đa 50% số tiền hóa đơn (sau khi đã giảm giá %).
    ///   - Điểm cộng của hóa đơn tính trên số tiền SAU khi đã trừ tiền đổi điểm.
    /// </summary>
    public static class QuyDinhTichDiem
    {
        public const decimal SO_TIEN_MOI_DIEM = 10000m;
        public const decimal GIA_TRI_MOI_DIEM = 500m;
        public const int DIEM_TOI_THIEU_DE_DOI = 20;
        public const decimal TY_LE_GIAM_TOI_DA = 0.5m;

        public const int MOC_HANG_BAC = 5;
        public const int MOC_HANG_VANG = 10;
        public const decimal HE_SO_THANH_VIEN = 1.0m;
        public const decimal HE_SO_BAC = 1.2m;
        public const decimal HE_SO_VANG = 1.5m;

        public const int CHU_KY_THUONG = 5;        // lần mua thứ 5, 10, 15...
        public const int DIEM_THUONG_CHU_KY = 20;
    }

    /// <summary>[10/11/2026] Thông tin khách hàng + điểm (trang Bán hàng).</summary>
    public class ThongTinKhach
    {
        public string MaKhach { get; set; }
        public string TenKhach { get; set; }
        public string DienThoai { get; set; }
        public int DiemTichLuy { get; set; }
        /// <summary>Số hóa đơn khách đã mua TRƯỚC hóa đơn đang lập.</summary>
        public int SoLanMua { get; set; }
    }

    /// <summary>[10/11/2026] Dữ liệu in hóa đơn (PDF / máy in).</summary>
    public class ThongTinInHoaDon
    {
        public string MaHDB { get; set; }
        public DateTime NgayBan { get; set; }
        public string NhanVien { get; set; }
        public string KhachHang { get; set; }
        public string HangThanhVien { get; set; }
        public List<DongHoaDon> Dong { get; set; } = new List<DongHoaDon>();
        public decimal TienHang { get; set; }
        public decimal GiamGiaPhanTram { get; set; }
        public decimal TienGiam { get; set; }
        public int DiemSuDung { get; set; }
        public decimal TienGiamDiem { get; set; }
        public decimal TongThanhToan { get; set; }
        public int DiemCong { get; set; }
        public int? DiemConLai { get; set; }
    }
}
