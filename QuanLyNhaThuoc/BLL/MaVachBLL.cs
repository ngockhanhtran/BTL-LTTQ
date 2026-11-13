using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyNhaThuoc.DAL;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>
    /// [11/11/2026 lần 7] Mã vạch cho thuốc CHƯA có mã vạch (in tem dán lên hộp, quét được khi bán).
    /// Dùng chuẩn EAN-13 với đầu số 200 - 299 (GS1 dành cho dùng NỘI BỘ cửa hàng, không trùng mã thật
    /// của nhà sản xuất): 200 + 9 số thứ tự + 1 chữ số kiểm tra.
    /// </summary>
    public class MaVachBLL
    {
        public const string DAU_SO_NOI_BO = "200";
        private readonly MaVachDAO dao = new MaVachDAO();

        // ================== HÀM THUẦN (có unit test) ==================

        /// <summary>
        /// Chữ số kiểm tra EAN-13: 12 chữ số đầu, vị trí lẻ (1, 3, 5...) nhân 1, vị trí chẵn nhân 3,
        /// kiểm tra = (10 - tổng mod 10) mod 10. Vd "400638133393" -> 1.
        /// </summary>
        public static int ChuSoKiemTraEan13(string muoiHaiSo)
        {
            if (muoiHaiSo == null || muoiHaiSo.Length != 12 || !muoiHaiSo.All(char.IsDigit))
                throw new ArgumentException("Cần đúng 12 chữ số.");
            int tong = 0;
            for (int i = 0; i < 12; i++)
            {
                int so = muoiHaiSo[i] - '0';
                tong += (i % 2 == 0) ? so : so * 3;     // i = 0 là vị trí 1 (lẻ)
            }
            return (10 - tong % 10) % 10;
        }

        public static bool LaEan13HopLe(string ma)
        {
            if (ma == null || ma.Length != 13 || !ma.All(c => c >= '0' && c <= '9')) return false;
            return ChuSoKiemTraEan13(ma.Substring(0, 12)) == ma[12] - '0';
        }

        /// <summary>Mã nội bộ kế tiếp sau các mã đã cấp: 200000000001x, 200000000002x...</summary>
        public static string MaNoiBoTiepTheo(IEnumerable<string> daCap)
        {
            long lonNhat = 0;
            foreach (string ma in daCap ?? Enumerable.Empty<string>())
            {
                if (!LaEan13HopLe(ma) || !ma.StartsWith(DAU_SO_NOI_BO)) continue;
                long so = long.Parse(ma.Substring(3, 9));
                if (so > lonNhat) lonNhat = so;
            }
            if (lonNhat >= 999999999) throw new InvalidOperationException("Đã hết dải mã vạch nội bộ.");
            string muoiHai = DAU_SO_NOI_BO + (lonNhat + 1).ToString().PadLeft(9, '0');
            return muoiHai + ChuSoKiemTraEan13(muoiHai);
        }

        /// <summary>Loại mã sẽ in: EAN-13 nếu hợp lệ, còn lại Code 128 (mã chữ / số tùy ý).</summary>
        public static string LoaiMaVach(string ma)
        {
            return LaEan13HopLe(ma) ? "EAN-13" : "Code 128";
        }

        // ================== CSDL ==================

        /// <summary>Cấp mã vạch nội bộ cho các thuốc chưa có. Trả về (mã thuốc -> mã vạch vừa cấp).</summary>
        public Dictionary<string, string> CapMaChoThuocChuaCo(IEnumerable<string> dsMaThuoc)
        {
            var kq = new Dictionary<string, string>();
            List<string> daCap = dao.LayMaVachNoiBo();
            foreach (string maThuoc in (dsMaThuoc ?? Enumerable.Empty<string>()).Where(m => !string.IsNullOrWhiteSpace(m)).Distinct())
            {
                string ma = MaNoiBoTiepTheo(daCap);
                while (dao.DaDung(ma)) { daCap.Add(ma); ma = MaNoiBoTiepTheo(daCap); }

                if (dao.GanMaVach(maThuoc.Trim(), ma))
                {
                    kq[maThuoc.Trim()] = ma;
                    daCap.Add(ma);
                }
            }
            if (kq.Count > 0)
                NhatKyBLL.Ghi(NhatKyBLL.IN_TEM, null, "Cấp " + kq.Count + " mã vạch nội bộ: " +
                              string.Join(", ", kq.Select(x => x.Key + "=" + x.Value)));
            return kq;
        }
    }
}
