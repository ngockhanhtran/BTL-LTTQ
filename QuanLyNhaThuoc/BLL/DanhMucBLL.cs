using System;
using System.Collections.Generic;
using System.Data;
using QuanLyNhaThuoc.DAL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>[10/11/2026] Nghiệp vụ chung cho các danh mục đơn giản (form frmDanhMuc).</summary>
    public class DanhMucBLL
    {
        private readonly DanhMucDAO dao = new DanhMucDAO();

        public DataTable LayDanhSach(CauHinhDanhMuc ch, string tuKhoa) { return dao.LayDanhSach(ch, tuKhoa); }
        public DataTable LayDanhSachChon(CauHinhDanhMuc ch) { return dao.LayDanhSachChon(ch); }
        public string SinhMaMoi(CauHinhDanhMuc ch) { return dao.SinhMaMoi(ch); }

        public void Them(CauHinhDanhMuc ch, Dictionary<string, object> giaTri)
        {
            KiemTra(ch, giaTri);
            string ma = Convert.ToString(giaTri[ch.CotKhoa.Ten]);
            if (dao.TonTai(ch, ma)) throw new ArgumentException(ch.CotKhoa.TieuDe + " \"" + ma + "\" đã tồn tại.");
            try { dao.Them(ch, giaTri); }
            catch (Exception ex)
            {
                if (SqlHelper.LaLoiTrungKhoa(ex)) throw new ArgumentException("Dữ liệu bị trùng: " + ex.Message);
                throw;
            }
        }

        public void Sua(CauHinhDanhMuc ch, Dictionary<string, object> giaTri)
        {
            KiemTra(ch, giaTri);
            dao.Sua(ch, giaTri);
        }

        /// <summary>Trả về false nếu bản ghi đang được dùng ở bảng khác (không xóa được).</summary>
        public bool Xoa(CauHinhDanhMuc ch, string ma)
        {
            try
            {
                dao.Xoa(ch, ma);
                return true;
            }
            catch (Exception ex)
            {
                if (SqlHelper.LaLoiKhoaNgoai(ex)) return false;
                throw;
            }
        }

        private static void KiemTra(CauHinhDanhMuc ch, Dictionary<string, object> giaTri)
        {
            foreach (CotDanhMuc c in ch.Cot)
            {
                if (c.ChiDoc) continue;
                object v;
                giaTri.TryGetValue(c.Ten, out v);
                string s = (Convert.ToString(v) ?? "").Trim();
                giaTri[c.Ten] = s;
                if (c.BatBuoc && s == "") throw new ArgumentException(c.TieuDe + " không được để trống.");
                if (c.DoDaiToiDa > 0 && s.Length > c.DoDaiToiDa)
                    throw new ArgumentException(c.TieuDe + " tối đa " + c.DoDaiToiDa + " ký tự.");
            }
        }
    }
}
