using System.Collections.Generic;
using System.Data;
using System.Linq;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>[11/11/2026 lần 7] Số liệu bán để dự báo nhập hàng (sp_DuBao_SoLieuBan).</summary>
    public class DuBaoDAO
    {
        public List<SoLieuBanThuoc> LaySoLieuBan(int soNgay)
        {
            DataTable dt = SqlHelper.QueryProc("sp_DuBao_SoLieuBan", SqlHelper.P("@SoNgay", soNgay));
            return dt.Rows.Cast<DataRow>().Select(r => new SoLieuBanThuoc
            {
                MaThuoc = DocDong.Chuoi(r, "MaThuoc"),
                TenThuoc = DocDong.Chuoi(r, "TenThuoc"),
                TenDVT = DocDong.Chuoi(r, "TenDVT") ?? "",
                DonGiaNhap = DocDong.Tien(r, "DonGiaNhap"),
                TonBanDuoc = DocDong.So(r, "TonBanDuoc"),
                DaBan = DocDong.So(r, "DaBan"),
                MaNCCGanNhat = DocDong.Chuoi(r, "MaNCCGanNhat"),
                TenNCCGanNhat = DocDong.Chuoi(r, "TenNCCGanNhat")
            }).ToList();
        }

        /// <summary>Mã phiếu nhập kế tiếp (HDNxxx) để lập phiếu nhập Excel từ đề xuất.</summary>
        public string MaPhieuNhapTiepTheo()
        {
            return SqlHelper.SinhMaTiepTheo("HoaDonNhap", "MaHDN", "HDN", 3);
        }
    }
}
