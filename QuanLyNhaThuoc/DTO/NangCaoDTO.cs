using System;

namespace QuanLyNhaThuoc.DTO
{
    // =====================================================================
    // [11/11/2026 lần 7] DTO cho các tính năng nâng cao
    // =====================================================================

    /// <summary>Một dòng gợi ý khi gõ (AutoComplete) - sp_GoiYThuoc.</summary>
    public class GoiYThuoc
    {
        public string MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public decimal GiaBan { get; set; }
        public string MaVach { get; set; }
        public bool KeDon { get; set; }
        public int SLBanDuoc { get; set; }

        /// <summary>Chữ hiện trong danh sách gợi ý.</summary>
        public string HienThi
        {
            get
            {
                string ton = SLBanDuoc > 0 ? "còn " + SLBanDuoc.ToString("N0") : "hết hàng";
                return (KeDon ? "℞ " : "") + TenThuoc + "   ·   " + GiaBan.ToString("N0") + " đ   ·   " + ton + "   [" + MaThuoc + "]";
            }
        }

        public override string ToString() { return HienThi; }
    }

    /// <summary>Thuốc hay được mua cùng - sp_GoiYMuaKem.</summary>
    public class GoiYMuaKem
    {
        public string MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLanCung { get; set; }           // số hóa đơn có cả 2 thuốc
        public int SoHoaDonCoThuoc { get; set; }     // số hóa đơn có thuốc đang xét
        public decimal TyLe { get; set; }            // % = SoLanCung / SoHoaDonCoThuoc
    }

    /// <summary>Một cặp thuốc có tương tác nằm trong hóa đơn - sp_KiemTraTuongTac.</summary>
    public class CanhBaoTuongTac
    {
        public const int LUU_Y = 1;
        public const int THAN_TRONG = 2;
        public const int NGHIEM_TRONG = 3;

        public string MaThuoc1 { get; set; }
        public string TenThuoc1 { get; set; }
        public string MaThuoc2 { get; set; }
        public string TenThuoc2 { get; set; }
        public int MucDo { get; set; }
        public string TenMucDo { get; set; }
        public string MoTa { get; set; }
    }

    /// <summary>Số liệu bán của 1 thuốc trong kỳ phân tích - sp_DuBao_SoLieuBan.</summary>
    public class SoLieuBanThuoc
    {
        public string MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public string TenDVT { get; set; }
        public decimal DonGiaNhap { get; set; }
        public int TonBanDuoc { get; set; }          // tồn còn hạn bán được
        public int DaBan { get; set; }               // đã bán trong kỳ
        public string MaNCCGanNhat { get; set; }
        public string TenNCCGanNhat { get; set; }
    }

    public enum MucDoDuBao
    {
        HetHang = 0,       // đỏ đậm
        CanNhapGap = 1,    // đỏ: còn đủ bán < 7 ngày
        NenNhap = 2,       // vàng: còn đủ bán < số ngày dự trữ
        DuHang = 3,        // xanh
        KhongBan = 4       // xám: không bán được trong kỳ
    }

    /// <summary>Kết quả dự báo nhập hàng của 1 thuốc (tính ở BLL/DuBaoNhapHangBLL).</summary>
    public class KetQuaDuBao
    {
        public SoLieuBanThuoc SoLieu { get; set; }
        public decimal TocDoBanNgay { get; set; }    // đơn vị / ngày
        public decimal? SoNgayConDu { get; set; }    // null = không bán trong kỳ
        public int DeXuatNhap { get; set; }
        public MucDoDuBao MucDo { get; set; }
        public string TenMucDo { get; set; }
        public string LoiKhuyen { get; set; }
    }

    /// <summary>Kết quả đo 1 truy vấn trước / sau khi có index.</summary>
    public class KetQuaDoTruyVan
    {
        public string TenTruyVan { get; set; }
        public string MoTa { get; set; }
        public int SoDong { get; set; }
        public double MsTruoc { get; set; }
        public double MsSau { get; set; }

        /// <summary>Nhanh hơn bao nhiêu lần.</summary>
        public double HeSo { get { return MsSau <= 0 ? 0 : MsTruoc / MsSau; } }
    }

    /// <summary>Thời gian chạy 1 truy vấn (lớp DAL trả về).</summary>
    public class KetQuaChay
    {
        public string Ten { get; set; }
        public int SoDong { get; set; }
        public double Ms { get; set; }               // trung vị các lần chạy
    }
}
