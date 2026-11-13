using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QuanLyNhaThuoc.DAL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>
    /// [11/11/2026 lần 7] Cảnh báo tương tác thuốc khi bán.
    /// !!! Dữ liệu tương tác là DỮ LIỆU MẪU ĐỂ DEMO, không phải tra cứu y khoa.
    /// - Hóa đơn có cả 2 thuốc của 1 cặp -> hiện cảnh báo đỏ ở trang Bán hàng.
    /// - Mức "Nghiêm trọng" -> khi thanh toán phải xác nhận (ghi nhật ký "Bán dù có tương tác").
    /// </summary>
    public class TuongTacThuocBLL
    {
        public const string GHI_CHU_DU_LIEU_MAU = "Dữ liệu tương tác là DỮ LIỆU MẪU để demo, không thay thế tư vấn của dược sĩ / bác sĩ.";
        private readonly TuongTacThuocDAO dao = new TuongTacThuocDAO();

        /// <summary>Các cặp tương tác trong danh sách thuốc. Lỗi CSDL -> danh sách rỗng (không chặn bán hàng).</summary>
        public List<CanhBaoTuongTac> KiemTra(IEnumerable<string> dsMaThuoc)
        {
            List<string> ds = (dsMaThuoc ?? Enumerable.Empty<string>()).Where(m => !string.IsNullOrWhiteSpace(m))
                                                                       .Select(m => m.Trim()).Distinct().ToList();
            if (ds.Count < 2) return new List<CanhBaoTuongTac>();
            try { return dao.KiemTra(ds); }
            catch (Exception) { return new List<CanhBaoTuongTac>(); }
        }

        public DataTable DanhSach(string tuKhoa) { return dao.DanhSach(tuKhoa); }

        public void Luu(string maA, string maB, int mucDo, string moTa, bool laThemMoi)
        {
            string ma1, ma2;
            ChuanHoaCap(maA, maB, out ma1, out ma2);
            if (mucDo < CanhBaoTuongTac.LUU_Y || mucDo > CanhBaoTuongTac.NGHIEM_TRONG)
                throw new ArgumentException("Mức độ phải từ 1 (Lưu ý) đến 3 (Nghiêm trọng).");
            moTa = (moTa ?? "").Trim();
            if (moTa.Length == 0) throw new ArgumentException("Chưa nhập mô tả tương tác.");
            if (moTa.Length > 500) throw new ArgumentException("Mô tả tối đa 500 ký tự.");

            if (laThemMoi)
            {
                if (dao.TonTai(ma1, ma2)) throw new ArgumentException("Cặp " + ma1 + " - " + ma2 + " đã có. Hãy chọn dòng đó để sửa.");
                dao.Them(ma1, ma2, mucDo, moTa);
                NhatKyBLL.Ghi(NhatKyBLL.TUONG_TAC, ma1 + "-" + ma2, "Thêm cặp tương tác mức " + mucDo);
            }
            else
            {
                if (dao.Sua(ma1, ma2, mucDo, moTa) == 0) throw new ArgumentException("Không tìm thấy cặp " + ma1 + " - " + ma2 + ".");
                NhatKyBLL.Ghi(NhatKyBLL.TUONG_TAC, ma1 + "-" + ma2, "Sửa cặp tương tác, mức " + mucDo);
            }
        }

        public void Xoa(string maA, string maB)
        {
            string ma1, ma2;
            ChuanHoaCap(maA, maB, out ma1, out ma2);
            dao.Xoa(ma1, ma2);
            NhatKyBLL.Ghi(NhatKyBLL.TUONG_TAC, ma1 + "-" + ma2, "Xóa cặp tương tác");
        }

        public int NapDuLieuMau()
        {
            int n = dao.NapDuLieuMau();
            NhatKyBLL.Ghi(NhatKyBLL.TUONG_TAC, null, "Nạp " + n + " cặp tương tác mẫu");
            return n;
        }

        // ================== HÀM THUẦN (có unit test) ==================

        /// <summary>
        /// Mỗi cặp chỉ lưu 1 lần với mã nhỏ hơn đứng trước (CSDL có CHECK MaThuoc1 &lt; MaThuoc2),
        /// nên (B, A) phải đổi thành (A, B).
        /// </summary>
        public static void ChuanHoaCap(string maA, string maB, out string ma1, out string ma2)
        {
            string a = (maA ?? "").Trim().ToUpperInvariant();
            string b = (maB ?? "").Trim().ToUpperInvariant();
            if (a.Length == 0 || b.Length == 0) throw new ArgumentException("Chưa chọn đủ 2 thuốc.");
            if (a == b) throw new ArgumentException("Hai thuốc của cặp tương tác phải khác nhau.");
            if (string.CompareOrdinal(a, b) < 0) { ma1 = a; ma2 = b; }
            else { ma1 = b; ma2 = a; }
        }

        public static int MucDoCaoNhat(IEnumerable<CanhBaoTuongTac> ds)
        {
            return ds == null || !ds.Any() ? 0 : ds.Max(x => x.MucDo);
        }

        /// <summary>Nội dung cảnh báo, mỗi cặp 1 dòng (mức cao trước).</summary>
        public static string TaoThongBao(IEnumerable<CanhBaoTuongTac> ds)
        {
            if (ds == null) return "";
            StringBuilder sb = new StringBuilder();
            foreach (CanhBaoTuongTac c in ds.OrderByDescending(x => x.MucDo))
            {
                if (sb.Length > 0) sb.AppendLine();
                sb.Append("• [" + (c.TenMucDo ?? TenMucDo(c.MucDo)) + "] " + c.TenThuoc1 + " + " + c.TenThuoc2 + ": " + c.MoTa);
            }
            return sb.ToString();
        }

        public static string TenMucDo(int mucDo)
        {
            switch (mucDo)
            {
                case CanhBaoTuongTac.NGHIEM_TRONG: return "Nghiêm trọng";
                case CanhBaoTuongTac.THAN_TRONG: return "Thận trọng";
                default: return "Lưu ý";
            }
        }
    }
}
