using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.DAL
{
    public class HoaDonBanDAO
    {
        private DBConnection db = new DBConnection();

        // ================== GIỮ NGUYÊN 2 HÀM CŨ ==================

        // thêm hóa đơn bán
        public bool ThemHoaDonBan(string maHDB, DateTime ngayBan, string maNV, string maKhach, decimal tongTien)
        {
            string sql = "INSERT INTO HoaDonBan (MaHDB, NgayBan, MaNV, MaKhach, TongTien) VALUES (@MaHDB, @NgayBan, @MaNV, @MaKhach, @TongTien)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaHDB", maHDB),
                new SqlParameter("@NgayBan", ngayBan),
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@MaKhach", maKhach),
                new SqlParameter("@TongTien", tongTien)
            };
            return db.ExecuteNonQuery(sql, parameters);
        }

        // thêm chi tiết hóa đơn bán + tự động trừ tồn kho
        public bool ThemChiTietHDB(string maHDB, string maThuoc, int soLuong, decimal giamGia, decimal thanhTien)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    string sqlCheck = "SELECT SLHienCo FROM Thuoc WHERE MaThuoc = @MaThuoc";
                    SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn, transaction);
                    cmdCheck.Parameters.AddWithValue("@MaThuoc", maThuoc);
                    int slTon = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (slTon < soLuong)
                        throw new Exception($"Số lượng tồn kho của thuốc {maThuoc} không đủ (Còn: {slTon})!");

                    // [10/11/2026] sửa lỗi gõ sai tên bảng "ChiTietHĐB" (chữ Đ tiếng Việt) -> ChiTietHDB
                    string sqlCT = "INSERT INTO ChiTietHDB (MaHDB, MaThuoc, SoLuong, GiamGia, ThanhTien) VALUES (@MaHDB, @MaThuoc, @SoLuong, @GiamGia, @ThanhTien)";
                    SqlCommand cmdCT = new SqlCommand(sqlCT, conn, transaction);
                    cmdCT.Parameters.AddWithValue("@MaHDB", maHDB);
                    cmdCT.Parameters.AddWithValue("@MaThuoc", maThuoc);
                    cmdCT.Parameters.AddWithValue("@SoLuong", soLuong);
                    cmdCT.Parameters.AddWithValue("@GiamGia", giamGia);
                    cmdCT.Parameters.AddWithValue("@ThanhTien", thanhTien);
                    cmdCT.ExecuteNonQuery();

                    string sqlUpdate = "UPDATE Thuoc SET SLHienCo = SLHienCo - @SoLuong WHERE MaThuoc = @MaThuoc";
                    SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn, transaction);
                    cmdUpdate.Parameters.AddWithValue("@SoLuong", soLuong);
                    cmdUpdate.Parameters.AddWithValue("@MaThuoc", maThuoc);
                    cmdUpdate.ExecuteNonQuery();

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // ================== CÁC HÀM MỚI ==================

        /// <summary>Lấy toàn bộ mã hóa đơn bán đã có để sinh mã kế tiếp.</summary>
        public List<string> GetDanhSachMaHDB()
        {
            List<string> ds = new List<string>();
            using (SqlConnection conn = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT MaHDB FROM HoaDonBan WHERE MaHDB NOT LIKE 'PF%'", conn))   // [11/11/2026 lần 7] bỏ hóa đơn giả PFxxxxxxxx (đo hiệu năng)
            {
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        if (rd[0] != DBNull.Value) ds.Add(rd[0].ToString().Trim());
                }
            }
            return ds;
        }

        // Danh sách nhân viên cho ComboBox (cột HienThi = "MaNV - TenNV")
        // [10/11/2026] Chỉ lấy NV đang làm việc và có quyền bán (quản lý / bán hàng), bỏ thủ kho.
        public DataTable GetDanhSachNhanVien()
        {
            string sql = @"SELECT MaNV, TenNV, RTRIM(MaNV) + ' - ' + TenNV AS HienThi
                           FROM NhanVien
                           WHERE NghiViec = 0 AND ISNULL(MaVaiTro, 'BH') <> 'TK'
                           ORDER BY MaNV";
            return db.ExecuteQuery(sql);
        }

        // [10/11/2026] Thông tin khách + điểm tích lũy + số lần đã mua (null nếu khách chưa có)
        public ThongTinKhach GetThongTinKhach(string maKhach)
        {
            DataTable dt = SqlHelper.Query(@"
                SELECT k.MaKhach, k.TenKhach, k.DienThoai, k.DiemTichLuy,
                       (SELECT COUNT(*) FROM HoaDonBan h WHERE h.MaKhach = k.MaKhach AND h.DaHuy = 0) AS SoLanMua
                FROM KhachHang k
                WHERE k.MaKhach = @MaKhach",
                SqlHelper.PChuoi("@MaKhach", (maKhach ?? "").Trim()));
            if (dt.Rows.Count == 0) return null;
            DataRow r = dt.Rows[0];
            return new ThongTinKhach
            {
                MaKhach = r["MaKhach"].ToString().Trim(),
                TenKhach = r["TenKhach"].ToString(),
                DienThoai = r["DienThoai"] == DBNull.Value ? "" : r["DienThoai"].ToString(),
                DiemTichLuy = r["DiemTichLuy"] == DBNull.Value ? 0 : Convert.ToInt32(r["DiemTichLuy"]),
                SoLanMua = Convert.ToInt32(r["SoLanMua"])
            };
        }

        // Trả về tên khách hàng, hoặc null nếu không tồn tại
        public string GetTenKhachHang(string maKhach)
        {
            using (SqlConnection conn = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT TenKhach FROM KhachHang WHERE MaKhach = @MaKhach", conn))
            {
                cmd.Parameters.AddWithValue("@MaKhach", maKhach);
                conn.Open();
                object kq = cmd.ExecuteScalar();
                return (kq == null || kq == DBNull.Value) ? null : kq.ToString();
            }
        }

        // Số lượng tồn hiện tại, hoặc null nếu thuốc không tồn tại
        // [10/11/2026 lần 5] = số lượng các lô CÒN HẠN (lô hết hạn không được bán)
        public int? GetSoLuongTon(string maThuoc)
        {
            using (SqlConnection conn = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT dbo.fn_SLBanDuoc(MaThuoc, CAST(GETDATE() AS DATE)) FROM Thuoc WHERE MaThuoc = @MaThuoc", conn))
            {
                cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);
                conn.Open();
                object kq = cmd.ExecuteScalar();
                if (kq == null || kq == DBNull.Value) return null;
                return Convert.ToInt32(kq);
            }
        }

        // Lưu cả hóa đơn + toàn bộ chi tiết + trừ tồn kho trong MỘT transaction
        // Lỗi ở bất kỳ dòng nào thì không lưu gì cả
        // tenKhachMoi != null  =>  khách chưa có trong CSDL, thêm mới vào bảng KhachHang trước
        // [10/11/2026] Thêm tích điểm:
        //   - Trừ hd.DiemSuDung và cộng diemCong vào KhachHang.DiemTichLuy (cùng transaction)
        //   - Lưu DiemSuDung / TienGiamDiem / DiemCong vào HoaDonBan
        //   - ChiTietHDB.ThanhTien lấy từ hd.PhanBoThanhTien() (đã trừ giảm giá % + đổi điểm)
        //   Trả về số điểm còn lại của khách sau hóa đơn.
        // [10/11/2026 lần 5] Xuất kho theo LÔ bằng sp_XuatKhoFEFO (lô hết hạn trước bán trước, bỏ lô đã hết hạn);
        //   lưu mã đơn thuốc (hd.MaDonThuoc) khi có thuốc kê đơn.
        public int LuuHoaDon(HoaDonTam hd, string maNV, string maKhach, string tenKhachMoi, DateTime ngayBan, int diemCong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 0. Khách mới -> thêm vào bảng KhachHang (nằm chung transaction với hóa đơn)
                        if (tenKhachMoi != null)
                        {
                            SqlCommand cmdKH = new SqlCommand(
                                @"IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKhach = @MaKhach)
                                      INSERT INTO KhachHang (MaKhach, TenKhach) VALUES (@MaKhach, @TenKhach)",
                                conn, tran);
                            cmdKH.Parameters.AddWithValue("@MaKhach", maKhach);
                            cmdKH.Parameters.AddWithValue("@TenKhach", tenKhachMoi);
                            cmdKH.ExecuteNonQuery();
                        }

                        // [10/11/2026] Kiểm tra lại điểm của khách (khóa dòng, tránh 2 máy cùng dùng điểm)
                        if (hd.DiemSuDung > 0)
                        {
                            object oDiem = SqlHelper.Scalar(conn, tran,
                                "SELECT DiemTichLuy FROM KhachHang WITH (UPDLOCK, ROWLOCK) WHERE MaKhach = @MaKhach",
                                SqlHelper.PChuoi("@MaKhach", maKhach));
                            int diemCo = oDiem == null ? 0 : Convert.ToInt32(oDiem);
                            if (diemCo < hd.DiemSuDung)
                                throw new Exception($"Khách hàng chỉ còn {diemCo} điểm, không đủ {hd.DiemSuDung} điểm để đổi!");
                        }

                        SqlCommand cmdHD = new SqlCommand(
                            @"INSERT INTO HoaDonBan (MaHDB, NgayBan, MaNV, MaKhach, TongTien, DiemSuDung, TienGiamDiem, DiemCong, MaDonThuoc)
                              VALUES (@MaHDB, @NgayBan, @MaNV, @MaKhach, @TongTien, @DiemSuDung, @TienGiamDiem, @DiemCong, @MaDonThuoc)",
                            conn, tran);
                        cmdHD.Parameters.AddWithValue("@MaHDB", hd.MaHDB);
                        cmdHD.Parameters.AddWithValue("@NgayBan", ngayBan);
                        cmdHD.Parameters.AddWithValue("@MaNV", maNV);
                        cmdHD.Parameters.AddWithValue("@MaKhach", maKhach);
                        cmdHD.Parameters.AddWithValue("@TongTien", hd.TongThanhToan);
                        cmdHD.Parameters.AddWithValue("@DiemSuDung", hd.DiemSuDung);
                        cmdHD.Parameters.AddWithValue("@TienGiamDiem", hd.TienGiamDiem);
                        cmdHD.Parameters.AddWithValue("@DiemCong", diemCong);
                        cmdHD.Parameters.Add(SqlHelper.P("@MaDonThuoc", hd.MaDonThuoc));     // [10/11/2026 lần 5]
                        cmdHD.ExecuteNonQuery();

                        // [10/11/2026] Thành tiền thực của từng dòng (tổng = đúng TongThanhToan)
                        List<decimal> thanhTienDong = hd.PhanBoThanhTien();

                        for (int i = 0; i < hd.Dong.Count; i++)
                        {
                            DongHoaDon d = hd.Dong[i];
                            // Khóa dòng thuốc để tránh 2 máy cùng bán vượt tồn
                            // [10/11/2026 lần 5] chỉ tính các lô CÒN HẠN
                            SqlCommand cmdTon = new SqlCommand(
                                @"SELECT dbo.fn_SLBanDuoc(MaThuoc, CAST(GETDATE() AS DATE))
                                  FROM Thuoc WITH (UPDLOCK, ROWLOCK) WHERE MaThuoc = @MaThuoc", conn, tran);
                            cmdTon.Parameters.AddWithValue("@MaThuoc", d.MaThuoc);
                            object o = cmdTon.ExecuteScalar();
                            if (o == null || o == DBNull.Value)
                                throw new Exception($"Thuốc {d.MaThuoc} không còn trong danh mục!");
                            int ton = Convert.ToInt32(o);
                            if (ton < d.SoLuong)
                                throw new Exception($"Thuốc {d.TenThuoc} không đủ hàng còn hạn (Còn: {ton}, cần: {d.SoLuong})!");

                            SqlCommand cmdCT = new SqlCommand(
                                "INSERT INTO ChiTietHDB (MaHDB, MaThuoc, SoLuong, GiamGia, ThanhTien) VALUES (@MaHDB, @MaThuoc, @SoLuong, @GiamGia, @ThanhTien)",
                                conn, tran);
                            cmdCT.Parameters.AddWithValue("@MaHDB", hd.MaHDB);
                            cmdCT.Parameters.AddWithValue("@MaThuoc", d.MaThuoc);
                            cmdCT.Parameters.AddWithValue("@SoLuong", d.SoLuong);
                            cmdCT.Parameters.AddWithValue("@GiamGia", hd.GiamGiaPhanTram);
                            cmdCT.Parameters.AddWithValue("@ThanhTien", thanhTienDong[i]);
                            cmdCT.ExecuteNonQuery();

                            // [10/11/2026 lần 5] trừ kho theo lô FEFO (proc tự cập nhật lại Thuoc.SLHienCo / HanSD)
                            SqlHelper.ExecProc(conn, tran, "sp_XuatKhoFEFO",
                                SqlHelper.PChuoi("@MaHDB", hd.MaHDB),
                                SqlHelper.PChuoi("@MaThuoc", d.MaThuoc),
                                SqlHelper.P("@SoLuong", d.SoLuong),
                                SqlHelper.P("@NgayBan", ngayBan));
                        }

                        // [10/11/2026] Cập nhật điểm tích lũy của khách
                        SqlHelper.NonQuery(conn, tran,
                            "UPDATE KhachHang SET DiemTichLuy = DiemTichLuy - @Dung + @Cong WHERE MaKhach = @MaKhach",
                            SqlHelper.P("@Dung", hd.DiemSuDung), SqlHelper.P("@Cong", diemCong),
                            SqlHelper.PChuoi("@MaKhach", maKhach));
                        object oConLai = SqlHelper.Scalar(conn, tran,
                            "SELECT DiemTichLuy FROM KhachHang WHERE MaKhach = @MaKhach",
                            SqlHelper.PChuoi("@MaKhach", maKhach));

                        tran.Commit();
                        return oConLai == null ? 0 : Convert.ToInt32(oConLai);
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}