using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using QuanLyNhaThuoc.DAL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>
    /// [10/11/2026 lần 5] Nghiệp vụ trả hàng / hủy hóa đơn.
    /// - Bán hàng: nhận trả trong QuyDinhTraHang.SO_NGAY_DUOC_TRA ngày; quản lý không giới hạn.
    /// - Chỉ QUẢN LÝ được hủy cả hóa đơn.
    /// - Tiền hoàn theo đơn giá thực khách đã trả (đã trừ giảm giá %, đổi điểm).
    /// - Điểm: trả 1 phần -> trừ điểm đã cộng theo tỷ lệ; hủy -> trừ hết điểm đã cộng + hoàn điểm đã dùng.
    /// </summary>
    public class TraHangBLL
    {
        private readonly TraHangDAO dao = new TraHangDAO();

        public DataTable TimHoaDon(string tuKhoa, DateTime tuNgay, DateTime denNgay)
        {
            if (denNgay.Date < tuNgay.Date) throw new ArgumentException("\"Đến ngày\" phải sau \"Từ ngày\".");
            return dao.TimHoaDon(tuKhoa, tuNgay, denNgay);
        }

        public DataTable LayChiTiet(string maHDB) { return dao.LayChiTiet(maHDB); }
        public DataTable LayPhieuTra(string maHDB) { return dao.LayPhieuTra(maHDB); }

        public static bool DuocHuyHoaDon { get { return !PhienDangNhap.DaDangNhap || PhienDangNhap.LaQuanLy; } }

        /// <summary>Kiểm tra hóa đơn còn trả được không. Trả về null nếu được, ngược lại là lý do.</summary>
        public string KiemTraDuocTra(string maHDB)
        {
            DataRow hd = dao.LayHoaDon(maHDB);
            if (hd == null) return "Không tìm thấy hóa đơn " + maHDB + ".";
            if (Convert.ToBoolean(hd["DaHuy"])) return "Hóa đơn " + maHDB + " đã bị hủy.";
            if (!PhienDangNhap.LaQuanLy && PhienDangNhap.DaDangNhap)
            {
                DateTime ngayBan = Convert.ToDateTime(hd["NgayBan"]);
                if ((DateTime.Today - ngayBan.Date).TotalDays > QuyDinhTraHang.SO_NGAY_DUOC_TRA)
                    return "Hóa đơn đã quá " + QuyDinhTraHang.SO_NGAY_DUOC_TRA + " ngày (bán ngày " + ngayBan.ToString("dd/MM/yyyy") +
                           "). Chỉ quản lý được nhận trả hàng.";
            }
            return null;
        }

        public KetQuaTraHang TraHang(string maHDB, List<DongTraHang> dsTra, string lyDo)
        {
            maHDB = (maHDB ?? "").Trim();
            lyDo = (lyDo ?? "").Trim();
            if (maHDB == "") throw new ArgumentException("Chưa chọn hóa đơn.");
            if (lyDo == "") throw new ArgumentException("Vui lòng nhập lý do trả hàng.");
            if (lyDo.Length > 255) throw new ArgumentException("Lý do tối đa 255 ký tự.");
            string loi = KiemTraDuocTra(maHDB);
            if (loi != null) throw new ArgumentException(loi);

            List<DongTraHang> ds = (dsTra ?? new List<DongTraHang>()).Where(d => d.SoLuong > 0).ToList();
            if (ds.Count == 0) throw new ArgumentException("Nhập số lượng trả cho ít nhất 1 thuốc.");

            // đối chiếu với số lượng còn lại trên hóa đơn
            DataTable ct = dao.LayChiTiet(maHDB);
            foreach (DongTraHang d in ds)
            {
                DataRow r = ct.AsEnumerable().FirstOrDefault(x => string.Equals(x["MaThuoc"].ToString().Trim(), d.MaThuoc, StringComparison.OrdinalIgnoreCase));
                if (r == null) throw new ArgumentException("Thuốc " + d.MaThuoc + " không có trong hóa đơn " + maHDB + ".");
                int con = Convert.ToInt32(r["SLConLai"]);
                if (d.SoLuong > con)
                    throw new ArgumentException("Thuốc " + r["TenThuoc"] + " chỉ còn " + con + " trên hóa đơn (đã trả " + r["SLDaTra"] + ").");
            }

            KetQuaTraHang kq = dao.TraHang(maHDB, ds, lyDo, PhienDangNhap.MaNV, false);
            NhatKyBLL.Ghi(kq.DaHuyHoaDon ? NhatKyBLL.HUY_HOA_DON : NhatKyBLL.TRA_HANG, maHDB,
                          kq.MaPTH + ": " + string.Join(", ", ds.Select(d => d.MaThuoc + " x" + d.SoLuong)) +
                          ", hoàn " + kq.TienHoan.ToString("N0") + " đ. Lý do: " + lyDo);
            return kq;
        }

        public KetQuaTraHang HuyHoaDon(string maHDB, string lyDo)
        {
            if (!DuocHuyHoaDon) throw new ArgumentException("Chỉ quản lý được hủy cả hóa đơn.");
            maHDB = (maHDB ?? "").Trim();
            lyDo = (lyDo ?? "").Trim();
            if (maHDB == "") throw new ArgumentException("Chưa chọn hóa đơn.");
            if (lyDo == "") throw new ArgumentException("Vui lòng nhập lý do hủy hóa đơn.");
            if (lyDo.Length > 255) throw new ArgumentException("Lý do tối đa 255 ký tự.");
            string loi = KiemTraDuocTra(maHDB);
            if (loi != null) throw new ArgumentException(loi);

            KetQuaTraHang kq = dao.TraHang(maHDB, null, lyDo, PhienDangNhap.MaNV, true);
            NhatKyBLL.Ghi(NhatKyBLL.HUY_HOA_DON, maHDB, kq.MaPTH + ": hủy cả hóa đơn, hoàn " + kq.TienHoan.ToString("N0") +
                          " đ, trừ " + kq.DiemTru + " điểm, hoàn " + kq.DiemHoan + " điểm. Lý do: " + lyDo);
            return kq;
        }
    }
}
