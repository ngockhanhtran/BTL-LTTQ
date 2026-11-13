using System;
using System.Collections.Generic;
using System.Data;
using QuanLyNhaThuoc.DAL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>
    /// [11/11/2026 lần 7] Đo tốc độ truy vấn TRƯỚC / SAU khi có index (bằng chứng cho "truy xuất dữ liệu nhanh").
    ///   1. (Tùy chọn) sinh 100.000 hóa đơn giả để bảng đủ lớn.
    ///   2. Xóa index -> chạy 3 truy vấn, đo bằng Stopwatch (trung vị nhiều lần chạy).
    ///   3. Tạo lại index -> chạy lại -> so sánh, vd "Trước: 85 ms -> Sau: 1,2 ms (nhanh hơn 70 lần)".
    /// Index LUÔN được tạo lại (kể cả khi lỗi giữa chừng) - khối finally.
    /// </summary>
    public class HieuNangBLL
    {
        public const int SO_LAN_DO = 5;
        private readonly HieuNangDAO dao = new HieuNangDAO();

        public DataRow ThongTin() { return dao.ThongTin(); }

        public int SinhDuLieu(int soHoaDon)
        {
            if (soHoaDon < 1000 || soHoaDon > 500000) throw new ArgumentException("Số hóa đơn phải từ 1.000 đến 500.000.");
            int n = dao.SinhDuLieu(soHoaDon);
            NhatKyBLL.Ghi(NhatKyBLL.HIEU_NANG, null, "Sinh " + n.ToString("N0") + " hóa đơn giả (PF...) để đo hiệu năng");
            return n;
        }

        public int XoaDuLieu()
        {
            int n = dao.XoaDuLieu();
            NhatKyBLL.Ghi(NhatKyBLL.HIEU_NANG, null, "Xóa " + n.ToString("N0") + " hóa đơn giả (PF...)");
            return n;
        }

        public List<KetQuaDoTruyVan> DoTruocSau(IProgress<string> tienDo)
        {
            DataRow tt = dao.ThongTin();
            DateTime ngay = tt == null || tt["NgayMau"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(tt["NgayMau"]);
            string maThuoc = tt == null || tt["MaThuocMau"] == DBNull.Value ? null : tt["MaThuocMau"].ToString().Trim();

            List<KetQuaChay> truoc, sau;
            try
            {
                Bao(tienDo, "Đang xóa index để đo TRƯỚC...");
                dao.DatIndex(false);
                Bao(tienDo, "Đang đo khi CHƯA có index (" + SO_LAN_DO + " lần / truy vấn)...");
                truoc = dao.ChayBoTruyVan(ngay, maThuoc, SO_LAN_DO);
            }
            finally
            {
                Bao(tienDo, "Đang tạo lại index...");
                dao.DatIndex(true);
            }
            Bao(tienDo, "Đang đo khi ĐÃ có index...");
            sau = dao.ChayBoTruyVan(ngay, maThuoc, SO_LAN_DO);

            var kq = GhepKetQua(truoc, sau, ngay, maThuoc);
            NhatKyBLL.Ghi(NhatKyBLL.HIEU_NANG, null, TomTat(kq));
            return kq;
        }

        // ================== HÀM THUẦN (có unit test) ==================

        public static List<KetQuaDoTruyVan> GhepKetQua(IList<KetQuaChay> truoc, IList<KetQuaChay> sau, DateTime ngay, string maThuoc)
        {
            string[] moTa =
            {
                "WHERE NgayBan trong ngày " + ngay.ToString("dd/MM/yyyy") + " - dùng IX_HoaDonBan_NgayBan",
                "SUM(TongTien) tháng " + ngay.ToString("MM/yyyy") + " - dùng IX_HoaDonBan_NgayBan (INCLUDE TongTien)",
                "WHERE MaThuoc = '" + maThuoc + "' - dùng IX_ChiTietHDB_MaThuoc"
            };
            var kq = new List<KetQuaDoTruyVan>();
            for (int i = 0; i < Math.Min(truoc.Count, sau.Count); i++)
            {
                kq.Add(new KetQuaDoTruyVan
                {
                    TenTruyVan = truoc[i].Ten,
                    MoTa = i < moTa.Length ? moTa[i] : "",
                    SoDong = sau[i].SoDong,
                    MsTruoc = truoc[i].Ms,
                    MsSau = sau[i].Ms
                });
            }
            return kq;
        }

        public static string TomTat(IEnumerable<KetQuaDoTruyVan> ds)
        {
            var dong = new List<string>();
            foreach (KetQuaDoTruyVan k in ds)
                dong.Add(k.TenTruyVan + ": " + k.MsTruoc.ToString("0.##") + " ms -> " + k.MsSau.ToString("0.##") + " ms (x" + k.HeSo.ToString("0.#") + ")");
            return string.Join("; ", dong);
        }

        private static void Bao(IProgress<string> p, string s) { if (p != null) p.Report(s); }
    }
}
