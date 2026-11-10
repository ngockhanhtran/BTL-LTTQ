using System;
using System.Data;
using System.IO;
using QuanLyNhaThuoc.DAL;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>[10/11/2026 lần 5] Sao lưu / khôi phục CSDL (chỉ quản lý).</summary>
    public class SaoLuuBLL
    {
        private readonly SaoLuuDAO dao = new SaoLuuDAO();

        public string TenCSDL { get { return CauHinhKetNoi.TenCSDL; } }

        public string LayThuMucMacDinh()
        {
            try { return dao.LayThuMucMacDinh(); }
            catch { return null; }
        }

        /// <summary>Tên file gợi ý: QuanLyNhaThuoc_20261110_2130.bak</summary>
        public string TenFileGoiY()
        {
            return TenCSDL + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".bak";
        }

        public void SaoLuu(string duongDan)
        {
            duongDan = KiemTraDuongDan(duongDan);
            if (!duongDan.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("File sao lưu phải có đuôi .bak");
            dao.SaoLuu(duongDan);
            NhatKyBLL.Ghi(NhatKyBLL.SAO_LUU, TenCSDL, duongDan);
        }

        public void KhoiPhuc(string duongDan)
        {
            duongDan = KiemTraDuongDan(duongDan);
            dao.KiemTraFile(duongDan);          // file hỏng thì dừng TRƯỚC khi ngắt kết nối
            dao.KhoiPhuc(duongDan);
            // ghi sau khi khôi phục (nhật ký trước đó đã nằm trong bản sao lưu cũ)
            NhatKyBLL.Ghi(NhatKyBLL.KHOI_PHUC, TenCSDL, "Khôi phục từ " + duongDan);
        }

        public DataTable LichSuSaoLuu()
        {
            return dao.LichSuSaoLuu();
        }

        private static string KiemTraDuongDan(string duongDan)
        {
            duongDan = (duongDan ?? "").Trim();
            if (duongDan == "") throw new ArgumentException("Chưa chọn đường dẫn file sao lưu.");
            if (!Path.IsPathRooted(duongDan)) throw new ArgumentException("Đường dẫn phải là đường dẫn đầy đủ (vd C:\\SaoLuu\\QLNT.bak).");
            return duongDan;
        }
    }
}
