using System;
using System.Collections.Generic;
using System.Data;
using QuanLyNhaThuoc.DAL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.BLL
{
    public class ThuocBLL
    {
        ThuocDAO thuocDAO = new ThuocDAO();

        public DataTable LayDanhSachThuoc()
        {
            return thuocDAO.GetDanhSachThuoc();
        }

        public DataTable TimKiemThuoc(string tuKhoa)
        {
            return thuocDAO.TimKiemThuoc(tuKhoa);
        }

        public bool ThemThuoc(string maThuoc, string tenThuoc, string maDVT, string maDangDC,
                              string thanhPhan, decimal donGiaNhap, int slHienCo, DateTime ngaySX,
                              string maNuocSX, DateTime hanSD, string chongChiDinh)
        {
            if (string.IsNullOrEmpty(maThuoc) || string.IsNullOrEmpty(tenThuoc))
            {
                throw new Exception("Mã thuốc và tên thuốc không được để trống!");
            }
            return thuocDAO.ThemThuoc(maThuoc, tenThuoc, maDVT, maDangDC, thanhPhan, donGiaNhap, slHienCo, ngaySX, maNuocSX, hanSD, chongChiDinh);
        }

        // =====================================================================
        //  [10/11/2026] MÃ VẠCH, CÔNG DỤNG, DANH MỤC THUỐC
        // =====================================================================

        public DataTable LayDanhSachQuanLy(string tuKhoa) { return thuocDAO.LayDanhSachQuanLy(tuKhoa); }
        public DataTable LayTatCaCongDung() { return thuocDAO.LayTatCaCongDung(); }
        public List<string> LayMaCongDungCuaThuoc(string maThuoc) { return thuocDAO.LayMaCongDungCuaThuoc(maThuoc); }

        /// <summary>Mã thuốc gợi ý khi thêm mới (TH001 lớn nhất -> TH016...).</summary>
        public string SinhMaThuocMoi() { return thuocDAO.SinhMaMoi(); }

        /// <summary>Thêm công dụng mới theo tên (tự sinh mã). Trả về mã công dụng.</summary>
        public string ThemCongDung(string ten)
        {
            ten = (ten ?? "").Trim();
            if (ten == "") throw new ArgumentException("Tên công dụng không được để trống.");
            if (ten.Length > 100) throw new ArgumentException("Tên công dụng tối đa 100 ký tự.");
            return thuocDAO.ThemCongDung(ten);
        }

        public void CapNhatCongDung(string maThuoc, List<string> dsMaCongDung)
        {
            if (string.IsNullOrWhiteSpace(maThuoc)) throw new ArgumentException("Chưa chọn thuốc.");
            thuocDAO.CapNhatCongDung(maThuoc.Trim(), dsMaCongDung);
        }

        public void CapNhatChongChiDinh(string maThuoc, string chongChiDinh)
        {
            if (string.IsNullOrWhiteSpace(maThuoc)) throw new ArgumentException("Chưa chọn thuốc.");
            if (chongChiDinh != null && chongChiDinh.Trim().Length > 255)
                throw new ArgumentException("Chống chỉ định tối đa 255 ký tự.");
            thuocDAO.CapNhatChongChiDinh(maThuoc.Trim(), chongChiDinh);
        }

        /// <summary>
        /// Tìm thuốc để bán theo mã vạch / mã thuốc. Trả về null + loi nếu không bán được
        /// (không tìm thấy, đã hết hạn, chưa có giá bán).
        /// </summary>
        public ThuocBan TimThuocDeBan(string ma, out string loi)
        {
            loi = null;
            ma = (ma ?? "").Trim();
            if (ma == "") { loi = "Chưa nhập mã vạch / mã thuốc."; return null; }

            ThuocBan t = thuocDAO.TimThuocBan(ma);
            if (t == null) { loi = "Không tìm thấy thuốc có mã vạch hoặc mã thuốc \"" + ma + "\"."; return null; }
            // [10/11/2026 lần 5] quản lý theo lô: chỉ bán từ các lô CÒN HẠN (FEFO)
            if (t.SLBanDuoc <= 0)
            {
                loi = t.SLHienCo > 0
                    ? "Thuốc \"" + t.TenThuoc + "\" chỉ còn lô đã HẾT HẠN (" + t.SLHienCo + "), không được bán. Hãy hủy lô hết hạn."
                    : "Thuốc \"" + t.TenThuoc + "\" đã hết hàng.";
                return null;
            }
            if (t.GiaBan <= 0) { loi = "Thuốc \"" + t.TenThuoc + "\" chưa có giá bán."; return null; }
            return t;
        }

        public void Them(ThuocDTO t)
        {
            KiemTra(t);
            if (thuocDAO.TonTai(t.MaThuoc)) throw new ArgumentException("Mã thuốc " + t.MaThuoc + " đã tồn tại.");
            thuocDAO.Them(t);
            NhatKyBLL.Ghi(NhatKyBLL.THUOC, t.MaThuoc, "Thêm thuốc " + t.TenThuoc + (t.KeDon ? " (kê đơn)" : ""));
        }

        public void Sua(ThuocDTO t)
        {
            KiemTra(t);
            if (!thuocDAO.TonTai(t.MaThuoc)) throw new ArgumentException("Không tìm thấy thuốc " + t.MaThuoc + ".");
            thuocDAO.Sua(t);
            // đổi giá được trigger trg_Thuoc_NhatKyGia ghi riêng
            NhatKyBLL.Ghi(NhatKyBLL.THUOC, t.MaThuoc, "Sửa thông tin thuốc " + t.TenThuoc + (t.KeDon ? " (kê đơn)" : ""));
        }

        /// <summary>[10/11/2026 lần 5] Các lô của thuốc (null = tất cả lô), thứ tự bán FEFO.</summary>
        public DataTable LayLoThuoc(string maThuoc, bool chiConHang)
        {
            return thuocDAO.LayLoThuoc(string.IsNullOrWhiteSpace(maThuoc) ? null : maThuoc.Trim(), chiConHang);
        }

        /// <summary>Trả về false nếu thuốc đã có trong hóa đơn / phiếu nhập (không xóa được).</summary>
        public bool Xoa(string maThuoc)
        {
            try
            {
                thuocDAO.Xoa(maThuoc);
                NhatKyBLL.Ghi(NhatKyBLL.THUOC, maThuoc, "Xóa thuốc");
                return true;
            }
            catch (Exception ex)
            {
                if (SqlHelper.LaLoiKhoaNgoai(ex)) return false;
                throw;
            }
        }

        private void KiemTra(ThuocDTO t)
        {
            t.MaThuoc = (t.MaThuoc ?? "").Trim().ToUpperInvariant();
            t.TenThuoc = (t.TenThuoc ?? "").Trim();
            t.MaVach = string.IsNullOrWhiteSpace(t.MaVach) ? null : t.MaVach.Trim();

            if (t.MaThuoc == "") throw new ArgumentException("Mã thuốc không được để trống.");
            if (t.MaThuoc.Length > 10) throw new ArgumentException("Mã thuốc tối đa 10 ký tự.");
            if (t.TenThuoc == "") throw new ArgumentException("Tên thuốc không được để trống.");
            if (t.TenThuoc.Length > 100) throw new ArgumentException("Tên thuốc tối đa 100 ký tự.");
            if (t.MaVach != null && t.MaVach.Length > 50) throw new ArgumentException("Mã vạch tối đa 50 ký tự.");
            if (t.ThanhPhan != null && t.ThanhPhan.Trim().Length > 255) throw new ArgumentException("Thành phần tối đa 255 ký tự.");
            if (t.ChongChiDinh != null && t.ChongChiDinh.Trim().Length > 255) throw new ArgumentException("Chống chỉ định tối đa 255 ký tự.");
            if (t.DonGiaNhap < 0) throw new ArgumentException("Giá nhập không được âm.");
            if (t.NgaySX.HasValue && t.HanSD.HasValue && t.HanSD.Value.Date <= t.NgaySX.Value.Date)
                throw new ArgumentException("Hạn sử dụng phải sau ngày sản xuất.");

            if (t.MaVach != null)
            {
                string maDangDung = thuocDAO.LayMaThuocTheoMaVach(t.MaVach);
                if (maDangDung != null && !string.Equals(maDangDung, t.MaThuoc, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Mã vạch " + t.MaVach + " đã được dùng cho thuốc " + maDangDung + ".");
            }
        }
    }
}
