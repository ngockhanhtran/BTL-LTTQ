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

                    string sqlCT = "INSERT INTO ChiTietHĐB (MaHDB, MaThuoc, SoLuong, GiamGia, ThanhTien) VALUES (@MaHDB, @MaThuoc, @SoLuong, @GiamGia, @ThanhTien)";
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
            using (SqlCommand cmd = new SqlCommand("SELECT MaHDB FROM HoaDonBan", conn))
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
        public DataTable GetDanhSachNhanVien()
        {
            string sql = "SELECT MaNV, TenNV, RTRIM(MaNV) + ' - ' + TenNV AS HienThi FROM NhanVien ORDER BY MaNV";
            return db.ExecuteQuery(sql);
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
        public int? GetSoLuongTon(string maThuoc)
        {
            using (SqlConnection conn = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT SLHienCo FROM Thuoc WHERE MaThuoc = @MaThuoc", conn))
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
        public void LuuHoaDon(HoaDonTam hd, string maNV, string maKhach, string tenKhachMoi, DateTime ngayBan)
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

                        SqlCommand cmdHD = new SqlCommand(
                            "INSERT INTO HoaDonBan (MaHDB, NgayBan, MaNV, MaKhach, TongTien) VALUES (@MaHDB, @NgayBan, @MaNV, @MaKhach, @TongTien)",
                            conn, tran);
                        cmdHD.Parameters.AddWithValue("@MaHDB", hd.MaHDB);
                        cmdHD.Parameters.AddWithValue("@NgayBan", ngayBan);
                        cmdHD.Parameters.AddWithValue("@MaNV", maNV);
                        cmdHD.Parameters.AddWithValue("@MaKhach", maKhach);
                        cmdHD.Parameters.AddWithValue("@TongTien", hd.TongThanhToan);
                        cmdHD.ExecuteNonQuery();

                        foreach (DongHoaDon d in hd.Dong)
                        {
                            // Khóa dòng thuốc để tránh 2 máy cùng bán vượt tồn
                            SqlCommand cmdTon = new SqlCommand(
                                "SELECT SLHienCo FROM Thuoc WITH (UPDLOCK, ROWLOCK) WHERE MaThuoc = @MaThuoc", conn, tran);
                            cmdTon.Parameters.AddWithValue("@MaThuoc", d.MaThuoc);
                            object o = cmdTon.ExecuteScalar();
                            if (o == null || o == DBNull.Value)
                                throw new Exception($"Thuốc {d.MaThuoc} không còn trong danh mục!");
                            int ton = Convert.ToInt32(o);
                            if (ton < d.SoLuong)
                                throw new Exception($"Thuốc {d.TenThuoc} không đủ tồn kho (Còn: {ton}, cần: {d.SoLuong})!");

                            SqlCommand cmdCT = new SqlCommand(
                                "INSERT INTO ChiTietHDB (MaHDB, MaThuoc, SoLuong, GiamGia, ThanhTien) VALUES (@MaHDB, @MaThuoc, @SoLuong, @GiamGia, @ThanhTien)",
                                conn, tran);
                            cmdCT.Parameters.AddWithValue("@MaHDB", hd.MaHDB);
                            cmdCT.Parameters.AddWithValue("@MaThuoc", d.MaThuoc);
                            cmdCT.Parameters.AddWithValue("@SoLuong", d.SoLuong);
                            cmdCT.Parameters.AddWithValue("@GiamGia", hd.GiamGiaPhanTram);
                            cmdCT.Parameters.AddWithValue("@ThanhTien", hd.ThanhTienSauGiam(d));
                            cmdCT.ExecuteNonQuery();

                            SqlCommand cmdUp = new SqlCommand(
                                "UPDATE Thuoc SET SLHienCo = SLHienCo - @SoLuong WHERE MaThuoc = @MaThuoc", conn, tran);
                            cmdUp.Parameters.AddWithValue("@SoLuong", d.SoLuong);
                            cmdUp.Parameters.AddWithValue("@MaThuoc", d.MaThuoc);
                            cmdUp.ExecuteNonQuery();
                        }

                        tran.Commit();
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