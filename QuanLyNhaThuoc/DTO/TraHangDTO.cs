using System;
using System.Collections.Generic;

namespace QuanLyNhaThuoc.DTO
{
    /// <summary>[10/11/2026 lần 5] Quy định trả hàng.</summary>
    public static class QuyDinhTraHang
    {
        /// <summary>Nhân viên bán hàng chỉ nhận trả hàng trong số ngày này kể từ ngày mua (quản lý không giới hạn).</summary>
        public const int SO_NGAY_DUOC_TRA = 30;
    }

    /// <summary>[10/11/2026 lần 5] 1 dòng thuốc khách trả.</summary>
    public class DongTraHang
    {
        public string MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public int SoLuong { get; set; }
    }

    /// <summary>[10/11/2026 lần 5] Kết quả sau khi lập phiếu trả hàng.</summary>
    public class KetQuaTraHang
    {
        public string MaPTH { get; set; }
        public decimal TienHoan { get; set; }
        public int DiemTru { get; set; }          // điểm đã cộng cho hóa đơn bị trừ lại
        public int DiemHoan { get; set; }         // điểm khách đã dùng được trả lại (hủy cả hóa đơn)
        public int DiemConLai { get; set; }       // điểm của khách sau khi trả
        public bool DaHuyHoaDon { get; set; }     // toàn bộ hóa đơn đã được trả
        public List<string> GhiChu { get; } = new List<string>();
    }
}
