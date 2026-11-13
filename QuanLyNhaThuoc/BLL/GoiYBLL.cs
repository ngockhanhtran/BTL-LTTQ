using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyNhaThuoc.DAL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>
    /// [11/11/2026 lần 7] Gợi ý thông minh:
    ///  - Gợi ý khi gõ (AutoComplete): gõ 2 ký tự trở lên -> danh sách thuốc khớp (không dấu).
    ///  - Gợi ý mua kèm: thuốc hay được mua cùng thuốc vừa thêm (phân tích hóa đơn cũ).
    /// Lỗi CSDL (vd chưa chạy file SQL mới) -> trả danh sách rỗng, KHÔNG làm hỏng việc bán hàng.
    /// </summary>
    public class GoiYBLL
    {
        public const int SO_KY_TU_TOI_THIEU = 2;
        public const int SO_NGAY_PHAN_TICH = 365;     // xét hóa đơn 1 năm gần đây
        public const int SO_LAN_TOI_THIEU = 2;        // phải mua cùng ít nhất 2 lần mới gợi ý

        private readonly GoiYDAO dao = new GoiYDAO();

        public static bool DuDaiDeGoiY(string tuKhoa)
        {
            return tuKhoa != null && tuKhoa.Trim().Length >= SO_KY_TU_TOI_THIEU;
        }

        public List<GoiYThuoc> GoiYKhiGo(string tuKhoa, int soDong = 10)
        {
            if (!DuDaiDeGoiY(tuKhoa)) return new List<GoiYThuoc>();
            return dao.GoiYThuoc(tuKhoa.Trim(), soDong);
        }

        public List<GoiYMuaKem> GoiYMuaKem(string maThuocVuaThem, IEnumerable<string> maDaCo, int soDong = 3)
        {
            if (string.IsNullOrWhiteSpace(maThuocVuaThem)) return new List<GoiYMuaKem>();
            try
            {
                return dao.GoiYMuaKem(maThuocVuaThem.Trim(), maDaCo, soDong, SO_NGAY_PHAN_TICH, SO_LAN_TOI_THIEU);
            }
            catch (Exception)
            {
                return new List<GoiYMuaKem>();
            }
        }

        /// <summary>Câu hiển thị, vd: "Vitamin C (35% khách mua Panadol mua kèm)".</summary>
        public static string MoTa(GoiYMuaKem g, string tenThuocGoc)
        {
            return g.TenThuoc + " (" + g.TyLe.ToString("0.#") + "% khách mua " + tenThuocGoc + " mua kèm)";
        }
    }
}
