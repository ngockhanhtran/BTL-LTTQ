using System;
using System.Data;
using System.Linq;
using QuanLyNhaThuoc.DAL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>[10/11/2026] Nghiệp vụ đăng nhập, quên / đổi mật khẩu, quản lý nhân viên.</summary>
    public class NhanVienBLL
    {
        public const int DO_DAI_MK_TOI_THIEU = 6;
        public const int DO_DAI_MK_TOI_DA = 50;

        private readonly NhanVienDAO dao = new NhanVienDAO();

        /// <summary>Mật khẩu mặc định = mã NV + "11" (vd QL001 -> QL00111).</summary>
        public static string MatKhauMacDinh(string maNV)
        {
            return (maNV ?? "").Trim() + "11";
        }

        // ===================== ĐĂNG NHẬP =====================

        /// <summary>Đúng tài khoản -> lưu vào PhienDangNhap và trả về true; sai -> loi chứa lý do.</summary>
        public bool DangNhap(string maNV, string matKhau, out string loi)
        {
            loi = null;
            maNV = (maNV ?? "").Trim();
            if (maNV == "") { loi = "Vui lòng nhập mã nhân viên."; return false; }
            if (string.IsNullOrEmpty(matKhau)) { loi = "Vui lòng nhập mật khẩu."; return false; }

            DataRow r = dao.LayTaiKhoan(maNV);
            if (r == null) { loi = "Mã nhân viên không tồn tại."; return false; }
            if (Convert.ToBoolean(r["NghiViec"])) { loi = "Tài khoản đã bị khóa (nhân viên đã nghỉ việc)."; return false; }

            string mkDung = r["MatKhau"] == DBNull.Value ? "" : r["MatKhau"].ToString();
            if (mkDung == "") { loi = "Tài khoản chưa có mật khẩu. Vui lòng liên hệ quản lý."; return false; }
            if (!string.Equals(mkDung, matKhau, StringComparison.Ordinal)) { loi = "Mật khẩu không đúng."; return false; }

            string vaiTro = r["MaVaiTro"] == DBNull.Value ? "" : r["MaVaiTro"].ToString().Trim();
            if (vaiTro == "") { loi = "Tài khoản chưa được phân quyền. Vui lòng liên hệ quản lý."; return false; }

            PhienDangNhap.DangNhap(r["MaNV"].ToString(), r["TenNV"].ToString(), vaiTro,
                                   r["TenVaiTro"] == DBNull.Value ? vaiTro : r["TenVaiTro"].ToString());
            return true;
        }

        // ===================== QUÊN / ĐỔI MẬT KHẨU =====================

        /// <summary>
        /// Gửi yêu cầu cấp lại mật khẩu tới quản lý. Xác minh bằng số điện thoại đã đăng ký
        /// (NV chưa có số điện thoại trong CSDL thì chỉ cần mã NV).
        /// </summary>
        public bool GuiYeuCauQuenMatKhau(string maNV, string dienThoai, out string thongBao)
        {
            maNV = (maNV ?? "").Trim();
            if (maNV == "") { thongBao = "Vui lòng nhập mã nhân viên."; return false; }

            DataRow r = dao.LayTaiKhoan(maNV);
            if (r == null) { thongBao = "Mã nhân viên không tồn tại."; return false; }
            if (Convert.ToBoolean(r["NghiViec"])) { thongBao = "Tài khoản đã bị khóa (nhân viên đã nghỉ việc)."; return false; }

            string sdtDung = ChiLaySo(r["DienThoai"] == DBNull.Value ? "" : r["DienThoai"].ToString());
            if (sdtDung != "")
            {
                string sdtNhap = ChiLaySo(dienThoai);
                if (sdtNhap == "") { thongBao = "Vui lòng nhập số điện thoại đã đăng ký để xác minh."; return false; }
                if (sdtNhap != sdtDung) { thongBao = "Số điện thoại không khớp với hồ sơ nhân viên."; return false; }
            }

            dao.GuiYeuCauQuenMatKhau(maNV);
            thongBao = "Đã gửi yêu cầu cấp lại mật khẩu tới quản lý.\nVui lòng liên hệ quản lý để nhận mật khẩu mới.";
            return true;
        }

        /// <summary>NV tự đổi mật khẩu. Trả về null nếu thành công, ngược lại là thông báo lỗi.</summary>
        public string DoiMatKhau(string maNV, string matKhauCu, string matKhauMoi, string nhapLai)
        {
            DataRow r = dao.LayTaiKhoan(maNV);
            if (r == null) return "Không tìm thấy tài khoản.";
            string mkDung = r["MatKhau"] == DBNull.Value ? "" : r["MatKhau"].ToString();
            if (!string.Equals(mkDung, matKhauCu ?? "", StringComparison.Ordinal)) return "Mật khẩu hiện tại không đúng.";
            string loi = KiemTraMatKhau(matKhauMoi);
            if (loi != null) return loi;
            if (matKhauMoi != nhapLai) return "Nhập lại mật khẩu mới không khớp.";
            if (matKhauMoi == mkDung) return "Mật khẩu mới phải khác mật khẩu hiện tại.";
            dao.DoiMatKhau(maNV, matKhauMoi);
            return null;
        }

        public static string KiemTraMatKhau(string mk)
        {
            if (string.IsNullOrEmpty(mk)) return "Mật khẩu không được để trống.";
            if (mk.Length < DO_DAI_MK_TOI_THIEU) return "Mật khẩu phải có ít nhất " + DO_DAI_MK_TOI_THIEU + " ký tự.";
            if (mk.Length > DO_DAI_MK_TOI_DA) return "Mật khẩu tối đa " + DO_DAI_MK_TOI_DA + " ký tự.";
            if (mk.Trim() != mk) return "Mật khẩu không được có khoảng trắng ở đầu hoặc cuối.";
            return null;
        }

        // ===================== QUẢN LÝ NHÂN VIÊN =====================

        public DataTable LayDanhSach(string tuKhoa) { return dao.LayDanhSach(tuKhoa); }
        public DataTable LayTrinhDo() { return dao.LayTrinhDo(); }
        public DataTable LayChuyenMon() { return dao.LayChuyenMon(); }
        public DataTable LayVaiTro() { return dao.LayVaiTro(); }
        public int DemYeuCauQuenMatKhau() { return dao.DemYeuCauQuenMatKhau(); }

        /// <summary>Mã gợi ý theo vai trò: QL -> QL002, TK -> TK002, BH -> NV04...</summary>
        public string SinhMaMoi(string maVaiTro)
        {
            string tienTo = maVaiTro == VaiTroNV.QUAN_LY ? "QL" : maVaiTro == VaiTroNV.THU_KHO ? "TK" : "NV";
            return dao.SinhMaMoi(tienTo);
        }

        public void Them(NhanVienDTO nv)
        {
            KiemTra(nv, true);
            if (dao.TonTai(nv.MaNV)) throw new ArgumentException("Mã nhân viên " + nv.MaNV + " đã tồn tại.");
            dao.Them(nv);
        }

        /// <param name="xuLyYeuCau">true = đánh dấu đã cấp lại mật khẩu cho NV đang yêu cầu</param>
        public void Sua(NhanVienDTO nv, bool xuLyYeuCau)
        {
            KiemTra(nv, false);
            DataRow cu = dao.LayTaiKhoan(nv.MaNV);
            if (cu == null) throw new ArgumentException("Không tìm thấy nhân viên " + nv.MaNV + ".");

            bool laChinhMinh = string.Equals(nv.MaNV, PhienDangNhap.MaNV, StringComparison.OrdinalIgnoreCase);
            if (laChinhMinh && nv.NghiViec)
                throw new ArgumentException("Bạn không thể tự khóa tài khoản của chính mình.");
            if (laChinhMinh && nv.MaVaiTro != VaiTroNV.QUAN_LY && PhienDangNhap.LaQuanLy)
                throw new ArgumentException("Bạn không thể tự bỏ quyền quản lý của chính mình.");

            bool dangLaQL = cu["MaVaiTro"] != DBNull.Value && cu["MaVaiTro"].ToString().Trim() == VaiTroNV.QUAN_LY
                            && !Convert.ToBoolean(cu["NghiViec"]);
            bool sauKhiSuaLaQL = nv.MaVaiTro == VaiTroNV.QUAN_LY && !nv.NghiViec;
            if (dangLaQL && !sauKhiSuaLaQL && dao.DemQuanLyDangLam(nv.MaNV) == 0)
                throw new ArgumentException("Phải còn ít nhất 1 quản lý đang làm việc.");

            dao.Sua(nv, xuLyYeuCau);
        }

        /// <summary>
        /// Xóa NV. Trả về false nếu NV đã có hóa đơn / phiếu nhập (không xóa được do khóa ngoại);
        /// khi đó form sẽ hỏi chuyển sang "nghỉ việc".
        /// </summary>
        public bool Xoa(string maNV)
        {
            KiemTraTruocKhiKhoa(maNV);
            try
            {
                dao.Xoa(maNV);
                return true;
            }
            catch (Exception ex)
            {
                if (SqlHelper.LaLoiKhoaNgoai(ex)) return false;
                throw;
            }
        }

        public void DatNghiViec(string maNV)
        {
            KiemTraTruocKhiKhoa(maNV);
            dao.DatNghiViec(maNV, true);
        }

        private void KiemTraTruocKhiKhoa(string maNV)
        {
            if (string.Equals(maNV, PhienDangNhap.MaNV, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Bạn không thể xóa / khóa tài khoản đang đăng nhập.");
            DataRow cu = dao.LayTaiKhoan(maNV);
            if (cu == null) throw new ArgumentException("Không tìm thấy nhân viên " + maNV + ".");
            bool laQL = cu["MaVaiTro"] != DBNull.Value && cu["MaVaiTro"].ToString().Trim() == VaiTroNV.QUAN_LY
                        && !Convert.ToBoolean(cu["NghiViec"]);
            if (laQL && dao.DemQuanLyDangLam(maNV) == 0)
                throw new ArgumentException("Phải còn ít nhất 1 quản lý đang làm việc.");
        }

        private static void KiemTra(NhanVienDTO nv, bool themMoi)
        {
            nv.MaNV = (nv.MaNV ?? "").Trim().ToUpperInvariant();
            nv.TenNV = (nv.TenNV ?? "").Trim();
            if (nv.MaNV == "") throw new ArgumentException("Mã nhân viên không được để trống.");
            if (nv.MaNV.Length > 10) throw new ArgumentException("Mã nhân viên tối đa 10 ký tự.");
            if (themMoi && nv.MaNV.Any(char.IsWhiteSpace)) throw new ArgumentException("Mã nhân viên không được chứa khoảng trắng.");
            if (nv.TenNV == "") throw new ArgumentException("Họ tên không được để trống.");
            if (nv.TenNV.Length > 50) throw new ArgumentException("Họ tên tối đa 50 ký tự.");
            if (string.IsNullOrWhiteSpace(nv.MaVaiTro)) throw new ArgumentException("Vui lòng chọn vai trò.");
            if (nv.DienThoai != null && nv.DienThoai.Trim().Length > 20) throw new ArgumentException("Số điện thoại tối đa 20 ký tự.");
            if (nv.DiaChi != null && nv.DiaChi.Trim().Length > 255) throw new ArgumentException("Địa chỉ tối đa 255 ký tự.");
            if (nv.NgaySinh.HasValue && nv.NgaySinh.Value.Date > DateTime.Today.AddYears(-15))
                throw new ArgumentException("Ngày sinh không hợp lệ (nhân viên phải từ 15 tuổi trở lên).");
            string loi = KiemTraMatKhau(nv.MatKhau);
            if (loi != null) throw new ArgumentException(loi);
        }

        private static string ChiLaySo(string s)
        {
            return new string((s ?? "").Where(char.IsDigit).ToArray());
        }
    }
}
