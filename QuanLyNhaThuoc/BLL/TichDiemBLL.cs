using System;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>
    /// [10/11/2026] Tính điểm tích lũy / đổi điểm. Quy định chi tiết xem DTO/TichDiemDTO.cs (QuyDinhTichDiem).
    /// </summary>
    public static class TichDiemBLL
    {
        /// <param name="soLanDaMua">số hóa đơn khách đã mua trước đó</param>
        public static string TenHang(int soLanDaMua)
        {
            if (soLanDaMua >= QuyDinhTichDiem.MOC_HANG_VANG) return "Vàng";
            if (soLanDaMua >= QuyDinhTichDiem.MOC_HANG_BAC) return "Bạc";
            return "Thành viên";
        }

        public static decimal HeSo(int soLanDaMua)
        {
            if (soLanDaMua >= QuyDinhTichDiem.MOC_HANG_VANG) return QuyDinhTichDiem.HE_SO_VANG;
            if (soLanDaMua >= QuyDinhTichDiem.MOC_HANG_BAC) return QuyDinhTichDiem.HE_SO_BAC;
            return QuyDinhTichDiem.HE_SO_THANH_VIEN;
        }

        /// <summary>Điểm thưởng theo mốc lần mua (lần mua thứ 5, 10, 15...).</summary>
        public static int DiemThuongLanMua(int soLanDaMua)
        {
            int lanNay = soLanDaMua + 1;
            return lanNay % QuyDinhTichDiem.CHU_KY_THUONG == 0 ? QuyDinhTichDiem.DIEM_THUONG_CHU_KY : 0;
        }

        /// <summary>Điểm cộng cho hóa đơn = điểm theo tiền x hệ số hạng + thưởng mốc lần mua.</summary>
        public static int TinhDiemCong(decimal tongThanhToan, int soLanDaMua)
        {
            int diemGoc = tongThanhToan <= 0 ? 0 : (int)Math.Floor(tongThanhToan / QuyDinhTichDiem.SO_TIEN_MOI_DIEM);
            int diemTheoHang = (int)Math.Floor(diemGoc * HeSo(soLanDaMua));
            return diemTheoHang + DiemThuongLanMua(soLanDaMua);
        }

        /// <summary>Diễn giải điểm cộng, vd: "12 điểm (hạng Bạc x1.2) + 20 thưởng lần mua thứ 5".</summary>
        public static string GiaiThichDiemCong(decimal tongThanhToan, int soLanDaMua)
        {
            int diemGoc = tongThanhToan <= 0 ? 0 : (int)Math.Floor(tongThanhToan / QuyDinhTichDiem.SO_TIEN_MOI_DIEM);
            int diemTheoHang = (int)Math.Floor(diemGoc * HeSo(soLanDaMua));
            int thuong = DiemThuongLanMua(soLanDaMua);
            string s = diemTheoHang + " điểm (hạng " + TenHang(soLanDaMua) + " x" + HeSo(soLanDaMua).ToString("0.0#") + ")";
            if (thuong > 0) s += " + " + thuong + " thưởng lần mua thứ " + (soLanDaMua + 1);
            return s;
        }

        /// <summary>Số điểm tối đa được dùng cho hóa đơn có số tiền (sau giảm giá %) là tongSauGiamGia.</summary>
        public static int DiemDoiToiDa(int diemHienCo, decimal tongSauGiamGia)
        {
            if (diemHienCo < QuyDinhTichDiem.DIEM_TOI_THIEU_DE_DOI || tongSauGiamGia <= 0) return 0;
            int theoTien = (int)Math.Floor(tongSauGiamGia * QuyDinhTichDiem.TY_LE_GIAM_TOI_DA / QuyDinhTichDiem.GIA_TRI_MOI_DIEM);
            return Math.Max(0, Math.Min(diemHienCo, theoTien));
        }

        public static decimal QuyRaTien(int diem)
        {
            return diem * QuyDinhTichDiem.GIA_TRI_MOI_DIEM;
        }

        /// <summary>Mô tả ngắn quy định để hiển thị trên form.</summary>
        public static string MoTaQuyDinh()
        {
            return "• " + QuyDinhTichDiem.SO_TIEN_MOI_DIEM.ToString("N0") + "đ = 1 điểm; hạng Bạc (từ "
                 + QuyDinhTichDiem.MOC_HANG_BAC + " lần mua) x" + QuyDinhTichDiem.HE_SO_BAC.ToString("0.0#")
                 + ", hạng Vàng (từ " + QuyDinhTichDiem.MOC_HANG_VANG + " lần) x" + QuyDinhTichDiem.HE_SO_VANG.ToString("0.0#") + ".\n"
                 + "• Thưởng " + QuyDinhTichDiem.DIEM_THUONG_CHU_KY + " điểm mỗi lần mua thứ "
                 + QuyDinhTichDiem.CHU_KY_THUONG + ", " + (QuyDinhTichDiem.CHU_KY_THUONG * 2) + ", ...\n"
                 + "• 1 điểm = " + QuyDinhTichDiem.GIA_TRI_MOI_DIEM.ToString("N0") + "đ, cần tối thiểu "
                 + QuyDinhTichDiem.DIEM_TOI_THIEU_DE_DOI + " điểm, giảm tối đa "
                 + (QuyDinhTichDiem.TY_LE_GIAM_TOI_DA * 100).ToString("0") + "% hóa đơn.";
        }
    }
}
