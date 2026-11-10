using System;
using System.Data;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>[10/11/2026] Đăng nhập, quên mật khẩu, quản lý nhân viên.</summary>
    /// <remarks>[10/11/2026 lần 5] Mật khẩu lưu dạng băm (MatKhauHash + MatKhauSalt), danh sách NV không trả về mật khẩu.</remarks>
    public class NhanVienDAO
    {
        private const string SQL_COT = @"
            SELECT nv.MaNV, nv.TenNV, nv.NgaySinh, nv.GioiTinh, nv.DienThoai, nv.DiaChi,
                   nv.MaTrinhDo, td.TenTrinhDo, nv.MaChuyenMon, cm.TenChuyenMon,
                   nv.MaVaiTro, vt.TenVaiTro, nv.PhaiDoiMK,
                   nv.YeuCauDoiMK, nv.NgayYeuCauDoiMK, nv.NghiViec";
        private const string SQL_TU = @"
            FROM NhanVien nv
            LEFT JOIN TrinhDo td   ON td.MaTrinhDo   = nv.MaTrinhDo
            LEFT JOIN ChuyenMon cm ON cm.MaChuyenMon = nv.MaChuyenMon
            LEFT JOIN VaiTro vt    ON vt.MaVaiTro    = nv.MaVaiTro";
        private const string SQL_DANH_SACH = SQL_COT + SQL_TU;

        /// <summary>Thông tin tài khoản theo mã NV (kèm MatKhauHash, MatKhauSalt để kiểm tra đăng nhập), null nếu không có.</summary>
        public DataRow LayTaiKhoan(string maNV)
        {
            DataTable dt = SqlHelper.Query(SQL_COT + ", nv.MatKhauHash, nv.MatKhauSalt" + SQL_TU + " WHERE nv.MaNV = @MaNV",
                                           SqlHelper.PChuoi("@MaNV", maNV));
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>Danh sách NV, NV có yêu cầu quên mật khẩu lên đầu.</summary>
        public DataTable LayDanhSach(string tuKhoa)
        {
            string sql = SQL_DANH_SACH + @"
            WHERE @TuKhoa = N''
               OR nv.MaNV LIKE N'%' + @TuKhoa + N'%'
               OR nv.TenNV LIKE N'%' + @TuKhoa + N'%'
               OR nv.DienThoai LIKE N'%' + @TuKhoa + N'%'
               OR vt.TenVaiTro LIKE N'%' + @TuKhoa + N'%'
            ORDER BY nv.NghiViec, nv.YeuCauDoiMK DESC, nv.MaVaiTro, nv.MaNV";
            return SqlHelper.Query(sql, SqlHelper.PChuoi("@TuKhoa", (tuKhoa ?? "").Trim()));
        }

        public bool TonTai(string maNV)
        {
            return SqlHelper.Scalar("SELECT 1 FROM NhanVien WHERE MaNV = @MaNV", SqlHelper.PChuoi("@MaNV", maNV)) != null;
        }

        public string LaySoDienThoai(string maNV)
        {
            object o = SqlHelper.Scalar("SELECT DienThoai FROM NhanVien WHERE MaNV = @MaNV", SqlHelper.PChuoi("@MaNV", maNV));
            return o == null ? null : o.ToString();
        }

        /// <summary>Đánh dấu NV đã bấm "Quên mật khẩu" (dòng tô vàng ở trang Quản lý nhân viên).</summary>
        public bool GuiYeuCauQuenMatKhau(string maNV)
        {
            return SqlHelper.NonQuery(
                "UPDATE NhanVien SET YeuCauDoiMK = 1, NgayYeuCauDoiMK = GETDATE() WHERE MaNV = @MaNV AND NghiViec = 0",
                SqlHelper.PChuoi("@MaNV", maNV)) > 0;
        }

        /// <summary>Đổi mật khẩu (đã băm) + xóa cờ yêu cầu quên mật khẩu.</summary>
        /// <param name="phaiDoi">true = NV phải đổi mật khẩu ở lần đăng nhập tới (quản lý đặt lại)</param>
        public bool DoiMatKhau(string maNV, string hash, string salt, bool phaiDoi)
        {
            return SqlHelper.NonQuery(
                @"UPDATE NhanVien SET MatKhauHash = @Hash, MatKhauSalt = @Salt, PhaiDoiMK = @PhaiDoi,
                         YeuCauDoiMK = 0, NgayYeuCauDoiMK = NULL
                  WHERE MaNV = @MaNV",
                SqlHelper.PChuoi("@Hash", hash), SqlHelper.PChuoi("@Salt", salt),
                SqlHelper.P("@PhaiDoi", phaiDoi), SqlHelper.PChuoi("@MaNV", maNV)) > 0;
        }

        public void Them(NhanVienDTO nv)
        {
            SqlHelper.NonQuery(@"
                INSERT INTO NhanVien (MaNV, TenNV, NgaySinh, GioiTinh, DiaChi, MaTrinhDo, MaChuyenMon, DienThoai,
                                      MatKhauHash, MatKhauSalt, PhaiDoiMK, MaVaiTro, YeuCauDoiMK, NgayYeuCauDoiMK, NghiViec)
                VALUES (@MaNV, @TenNV, @NgaySinh, @GioiTinh, @DiaChi, @MaTrinhDo, @MaChuyenMon, @DienThoai,
                        @MatKhauHash, @MatKhauSalt, @PhaiDoiMK, @MaVaiTro, 0, NULL, @NghiViec)",
                ThamSo(nv));
        }

        /// <param name="xuLyYeuCau">true = xóa cờ "quên mật khẩu" (quản lý đã cấp lại mật khẩu)</param>
        /// <remarks>[10/11/2026 lần 5] nv.MatKhauHash = null -> giữ mật khẩu cũ.</remarks>
        public void Sua(NhanVienDTO nv, bool xuLyYeuCau)
        {
            SqlHelper.NonQuery(@"
                UPDATE NhanVien
                SET TenNV = @TenNV, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, DiaChi = @DiaChi,
                    MaTrinhDo = @MaTrinhDo, MaChuyenMon = @MaChuyenMon, DienThoai = @DienThoai,
                    MatKhauHash = ISNULL(@MatKhauHash, MatKhauHash),
                    MatKhauSalt = CASE WHEN @MatKhauHash IS NULL THEN MatKhauSalt ELSE @MatKhauSalt END,
                    PhaiDoiMK   = CASE WHEN @MatKhauHash IS NULL THEN PhaiDoiMK ELSE @PhaiDoiMK END,
                    MaVaiTro = @MaVaiTro, NghiViec = @NghiViec,
                    YeuCauDoiMK     = CASE WHEN @XuLy = 1 THEN 0    ELSE YeuCauDoiMK END,
                    NgayYeuCauDoiMK = CASE WHEN @XuLy = 1 THEN NULL ELSE NgayYeuCauDoiMK END
                WHERE MaNV = @MaNV",
                ThamSo(nv, SqlHelper.P("@XuLy", xuLyYeuCau)));
        }

        /// <summary>Xóa hẳn NV. Ném SqlException 547 nếu NV đã có hóa đơn / phiếu nhập.</summary>
        public void Xoa(string maNV)
        {
            SqlHelper.NonQuery("DELETE FROM NhanVien WHERE MaNV = @MaNV", SqlHelper.PChuoi("@MaNV", maNV));
        }

        public void DatNghiViec(string maNV, bool nghiViec)
        {
            SqlHelper.NonQuery("UPDATE NhanVien SET NghiViec = @NghiViec WHERE MaNV = @MaNV",
                SqlHelper.P("@NghiViec", nghiViec), SqlHelper.PChuoi("@MaNV", maNV));
        }

        public int DemYeuCauQuenMatKhau()
        {
            object o = SqlHelper.Scalar("SELECT COUNT(*) FROM NhanVien WHERE YeuCauDoiMK = 1 AND NghiViec = 0");
            return o == null ? 0 : Convert.ToInt32(o);
        }

        /// <summary>Số quản lý đang làm việc, không tính maNVBoQua.</summary>
        public int DemQuanLyDangLam(string maNVBoQua)
        {
            object o = SqlHelper.Scalar(
                "SELECT COUNT(*) FROM NhanVien WHERE MaVaiTro = @QL AND NghiViec = 0 AND MaNV <> @MaNV",
                SqlHelper.PChuoi("@QL", VaiTroNV.QUAN_LY), SqlHelper.PChuoi("@MaNV", maNVBoQua ?? ""));
            return o == null ? 0 : Convert.ToInt32(o);
        }

        public string SinhMaMoi(string tienTo)
        {
            return SqlHelper.SinhMaTiepTheo("NhanVien", "MaNV", tienTo, 3);
        }

        public DataTable LayTrinhDo()
        {
            return SqlHelper.Query("SELECT MaTrinhDo, TenTrinhDo FROM TrinhDo ORDER BY MaTrinhDo");
        }

        public DataTable LayChuyenMon()
        {
            return SqlHelper.Query("SELECT MaChuyenMon, TenChuyenMon FROM ChuyenMon ORDER BY MaChuyenMon");
        }

        public DataTable LayVaiTro()
        {
            return SqlHelper.Query("SELECT MaVaiTro, TenVaiTro FROM VaiTro ORDER BY CASE MaVaiTro WHEN 'QL' THEN 1 WHEN 'BH' THEN 2 ELSE 3 END");
        }

        private static System.Data.SqlClient.SqlParameter[] ThamSo(NhanVienDTO nv, params System.Data.SqlClient.SqlParameter[] them)
        {
            System.Collections.Generic.List<System.Data.SqlClient.SqlParameter> ds = new System.Collections.Generic.List<System.Data.SqlClient.SqlParameter>
            {
                SqlHelper.P("@MaNV", nv.MaNV),
                SqlHelper.P("@TenNV", nv.TenNV),
                SqlHelper.P("@NgaySinh", nv.NgaySinh),
                SqlHelper.P("@GioiTinh", nv.GioiTinh),
                SqlHelper.P("@DiaChi", nv.DiaChi),
                SqlHelper.P("@MaTrinhDo", nv.MaTrinhDo),
                SqlHelper.P("@MaChuyenMon", nv.MaChuyenMon),
                SqlHelper.P("@DienThoai", nv.DienThoai),
                SqlHelper.P("@MatKhauHash", nv.MatKhauHash),
                SqlHelper.P("@MatKhauSalt", nv.MatKhauSalt),
                SqlHelper.P("@PhaiDoiMK", nv.PhaiDoiMK),
                SqlHelper.P("@MaVaiTro", nv.MaVaiTro),
                SqlHelper.P("@NghiViec", nv.NghiViec)
            };
            if (them != null) ds.AddRange(them);
            return ds.ToArray();
        }
    }
}
