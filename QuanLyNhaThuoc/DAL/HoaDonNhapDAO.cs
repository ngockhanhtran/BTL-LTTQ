using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace QuanLyNhaThuoc.DAL
{
    public class HoaDonNhapDAO
    {
        private DBConnection db = new DBConnection();

        // =====================================================================
        //  CÁC HÀM CŨ - GIỮ NGUYÊN
        // =====================================================================

        // THÊM HÓA ĐƠN NHẬP MỚI
        public bool ThemHoaDonNhap(string maHDN, DateTime ngayNhap, string maNV, string maNCC, decimal tongTien)
        {
            string sql = "INSERT INTO HoaDonNhap (MaHDN, NgayNhap, MaNV, MaNCC, TongTien) VALUES (@MaHDN, @NgayNhap, @MaNV, @MaNCC, @TongTien)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaHDN", maHDN),
                new SqlParameter("@NgayNhap", ngayNhap),
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@MaNCC", maNCC),
                new SqlParameter("@TongTien", tongTien)
            };
            return db.ExecuteNonQuery(sql, parameters);
        }

        // THÊM CHI TIẾT HĐN + TỰ ĐỘNG TĂNG TỒN KHO + CẬP NHẬT GIÁ NHẬP + GIÁ BÁN = 110% X GIÁ NHẬP
        public bool ThemChiTietHDN(string maHDN, string maThuoc, int slNhap, decimal donGia, decimal khuyenMai, decimal thanhTien)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    string sqlCT = "INSERT INTO ChiTietHDN (MaHDN, MaThuoc, SLNhap, DonGia, KhuyenMai, ThanhTien) VALUES (@MaHDN, @MaThuoc, @SLNhap, @DonGia, @KhuyenMai, @ThanhTien)";
                    SqlCommand cmdCT = new SqlCommand(sqlCT, conn, transaction);
                    cmdCT.Parameters.AddWithValue("@MaHDN", maHDN);
                    cmdCT.Parameters.AddWithValue("@MaThuoc", maThuoc);
                    cmdCT.Parameters.AddWithValue("@SLNhap", slNhap);
                    cmdCT.Parameters.AddWithValue("@DonGia", donGia);
                    cmdCT.Parameters.AddWithValue("@KhuyenMai", khuyenMai);
                    cmdCT.Parameters.AddWithValue("@ThanhTien", thanhTien);
                    cmdCT.ExecuteNonQuery();

                    decimal giaBanMoi = donGia * 1.1m;
                    string sqlUpdateThuoc = @"UPDATE Thuoc 
                                             SET SLHienCo = SLHienCo + @SLNhap, 
                                                 DonGiaNhap = @DonGia, 
                                                 GiaBan = @GiaBan 
                                             WHERE MaThuoc = @MaThuoc";
                    SqlCommand cmdThuoc = new SqlCommand(sqlUpdateThuoc, conn, transaction);
                    cmdThuoc.Parameters.AddWithValue("@SLNhap", slNhap);
                    cmdThuoc.Parameters.AddWithValue("@DonGia", donGia);
                    cmdThuoc.Parameters.AddWithValue("@GiaBan", giaBanMoi);
                    cmdThuoc.Parameters.AddWithValue("@MaThuoc", maThuoc);
                    cmdThuoc.ExecuteNonQuery();

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

        // yêu cầu 5: tìm kiếm hóa đơn nhập theo mã hàng, nhân viên, nhà cung cấp
        public DataTable TimKiemHoaDonNhap(string tuKhoa)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TuKhoa", SqlDbType.NVarChar) { Value = tuKhoa }
            };

            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemHoaDonNhap", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // =====================================================================
        //  MỚI: TRA CỨU NHANH (dùng để hiển thị tên tự động trên form)
        //  Trả về null nếu không tìm thấy
        // =====================================================================

        public string LayTenNhaCungCap(string maNCC)
        {
            return LayMotGiaTri("SELECT TenNCC FROM NhaCungCap WHERE MaNCC = @Ma", maNCC);
        }

        public string LayTenNhanVien(string maNV)
        {
            return LayMotGiaTri("SELECT TenNV FROM NhanVien WHERE MaNV = @Ma", maNV);
        }

        public string LayTenThuoc(string maThuoc)
        {
            return LayMotGiaTri("SELECT TenThuoc FROM Thuoc WHERE MaThuoc = @Ma", maThuoc);
        }

        public bool TonTaiHoaDonNhap(string maHDN)
        {
            return LayMotGiaTri("SELECT MaHDN FROM HoaDonNhap WHERE MaHDN = @Ma", maHDN) != null;
        }

        public bool TonTaiDonViTinh(string maDVT)
        {
            return LayMotGiaTri("SELECT MaDVT FROM DonViTinh WHERE MaDVT = @Ma", maDVT) != null;
        }

        public bool TonTaiDangDieuChe(string maDangDC)
        {
            return LayMotGiaTri("SELECT MaDangDC FROM DangDieuChe WHERE MaDangDC = @Ma", maDangDC) != null;
        }

        public bool TonTaiNuocSX(string maNuocSX)
        {
            return LayMotGiaTri("SELECT MaNuocSX FROM NuocSX WHERE MaNuocSX = @Ma", maNuocSX) != null;
        }

        private string LayMotGiaTri(string sql, string ma)
        {
            if (string.IsNullOrWhiteSpace(ma)) return null;
            using (SqlConnection conn = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@Ma", SqlDbType.VarChar, 50).Value = ma.Trim();
                conn.Open();
                object kq = cmd.ExecuteScalar();
                return (kq == null || kq == DBNull.Value) ? null : kq.ToString();
            }
        }

        // =====================================================================
        //  MỚI: NHẬP HÀNG THỦ CÔNG (1 thuốc / 1 lần bấm) - TẤT CẢ TRONG 1 TRANSACTION
        //  - Phiếu chưa có  -> tạo phiếu mới (MaNV thủ kho, MaNCC lấy từ form)
        //  - Phiếu đã có    -> thêm thuốc vào phiếu đó (MaNV, MaNCC phải khớp)
        //  - Tăng tồn kho, cập nhật giá nhập, giá bán = 110% giá nhập
        //  - Tự tính lại TongTien của phiếu = SUM(ThanhTien)
        // =====================================================================
        public bool NhapHangThuCong(string maHDN, string maNV, string maNCC, string maThuoc,
                                    int slNhap, decimal donGia, decimal khuyenMai, out bool taoPhieuMoi)
        {
            taoPhieuMoi = false;
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlTransaction tx = conn.BeginTransaction();
                try
                {
                    // 1. Kiểm tra phiếu đã tồn tại chưa
                    bool coPhieu = false;
                    string nvCu = null, nccCu = null;
                    using (SqlCommand cmd = new SqlCommand("SELECT MaNV, MaNCC FROM HoaDonNhap WHERE MaHDN = @MaHDN", conn, tx))
                    {
                        cmd.Parameters.Add(ThamSo("@MaHDN", maHDN));
                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                coPhieu = true;
                                nvCu = rd["MaNV"] as string;
                                nccCu = rd["MaNCC"] as string;
                            }
                        }
                    }

                    if (!coPhieu)
                    {
                        ThucThi(conn, tx,
                            "INSERT INTO HoaDonNhap (MaHDN, NgayNhap, MaNV, MaNCC, TongTien) VALUES (@MaHDN, @NgayNhap, @MaNV, @MaNCC, 0)",
                            ThamSo("@MaHDN", maHDN), ThamSo("@NgayNhap", DateTime.Now),
                            ThamSo("@MaNV", maNV), ThamSo("@MaNCC", maNCC));
                        taoPhieuMoi = true;
                    }
                    else if (!GiongMa(nvCu, maNV) || !GiongMa(nccCu, maNCC))
                    {
                        throw new InvalidOperationException(
                            "Phiếu " + maHDN + " đã tồn tại với Mã NV '" + nvCu + "' và Mã NCC '" + nccCu + "'.\n" +
                            "Muốn thêm thuốc vào phiếu này thì Mã NV và Mã NCC phải trùng khớp, hoặc hãy dùng mã phiếu khác.");
                    }

                    // 2. Thuốc đã có trong phiếu này chưa (khóa chính MaHDN + MaThuoc)
                    object daCo = LayGiaTriTx(conn, tx,
                        "SELECT 1 FROM ChiTietHDN WHERE MaHDN = @MaHDN AND MaThuoc = @MaThuoc",
                        ThamSo("@MaHDN", maHDN), ThamSo("@MaThuoc", maThuoc));
                    if (daCo != null)
                        throw new InvalidOperationException("Thuốc " + maThuoc + " đã có trong phiếu " + maHDN + ".");

                    // 3. Thêm chi tiết + tăng tồn kho + cập nhật giá
                    ThemChiTietVaCapNhatKho(conn, tx, maHDN, maThuoc, slNhap, donGia, khuyenMai);

                    // 4. Tính lại tổng tiền phiếu
                    CapNhatTongTienPhieu(conn, tx, maHDN);

                    tx.Commit();
                    return true;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        // =====================================================================
        //  MỚI: IMPORT PHIẾU NHẬP TỪ EXCEL - TOÀN BỘ FILE TRONG 1 TRANSACTION
        //  (1 dòng lỗi -> hủy toàn bộ, không ghi dở dang)
        //  - Nhiều phiếu trong 1 file (gom theo MaHDN)
        //  - NCC chưa có + có TenNCC  -> tự thêm NCC mới
        //  - Thuốc chưa có + có TenThuoc -> tự thêm thuốc mới, rồi cộng tồn kho
        //  - [10/11/2026] Cột MaVach -> gán mã vạch; cột CongDung -> gán công dụng (tự thêm công dụng mới)
        // =====================================================================
        public ThongKeImport ImportPhieuNhap(List<DongPhieuNhap> dsDong)
        {
            ThongKeImport tk = new ThongKeImport();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlTransaction tx = conn.BeginTransaction();
                try
                {
                    foreach (var phieu in dsDong.GroupBy(d => d.MaHDN, StringComparer.OrdinalIgnoreCase))
                    {
                        DongPhieuNhap dau = phieu.First();

                        if (LayGiaTriTx(conn, tx, "SELECT 1 FROM HoaDonNhap WHERE MaHDN = @Ma", ThamSo("@Ma", dau.MaHDN)) != null)
                            throw new InvalidOperationException("Phiếu " + dau.MaHDN + " đã tồn tại trong CSDL.");

                        // Nhà cung cấp
                        if (LayGiaTriTx(conn, tx, "SELECT 1 FROM NhaCungCap WHERE MaNCC = @Ma", ThamSo("@Ma", dau.MaNCC)) == null)
                        {
                            if (string.IsNullOrWhiteSpace(dau.TenNCC))
                                throw new InvalidOperationException("Dòng " + dau.DongExcel + ": NCC " + dau.MaNCC + " chưa có trong CSDL và thiếu TenNCC.");
                            ThucThi(conn, tx, "INSERT INTO NhaCungCap (MaNCC, TenNCC) VALUES (@MaNCC, @TenNCC)",
                                ThamSo("@MaNCC", dau.MaNCC), ThamSo("@TenNCC", dau.TenNCC));
                            tk.SoNCCMoi++;
                        }

                        // Phiếu nhập
                        DateTime ngayNhap = phieu.Select(d => d.NgayNhap).FirstOrDefault(n => n.HasValue) ?? DateTime.Now;
                        ThucThi(conn, tx,
                            "INSERT INTO HoaDonNhap (MaHDN, NgayNhap, MaNV, MaNCC, TongTien) VALUES (@MaHDN, @NgayNhap, @MaNV, @MaNCC, 0)",
                            ThamSo("@MaHDN", dau.MaHDN), ThamSo("@NgayNhap", ngayNhap),
                            ThamSo("@MaNV", dau.MaNV), ThamSo("@MaNCC", dau.MaNCC));

                        // Chi tiết từng thuốc
                        foreach (DongPhieuNhap d in phieu)
                        {
                            if (LayGiaTriTx(conn, tx, "SELECT 1 FROM Thuoc WHERE MaThuoc = @Ma", ThamSo("@Ma", d.MaThuoc)) == null)
                            {
                                if (string.IsNullOrWhiteSpace(d.TenThuoc))
                                    throw new InvalidOperationException("Dòng " + d.DongExcel + ": thuốc mới " + d.MaThuoc + " thiếu TenThuoc.");

                                // Thêm thuốc mới với tồn kho = 0, bước sau sẽ cộng SLNhap và cập nhật giá
                                // [10/11/2026] thêm cột MaVach
                                ThucThi(conn, tx,
                                    @"INSERT INTO Thuoc (MaThuoc, TenThuoc, MaVach, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan, SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh)
                                      VALUES (@MaThuoc, @TenThuoc, @MaVach, @MaDVT, @MaDangDC, @ThanhPhan, 0, 0, 0, @NgaySX, @MaNuocSX, @HanSD, @ChongChiDinh)",
                                    ThamSo("@MaThuoc", d.MaThuoc), ThamSo("@TenThuoc", d.TenThuoc),
                                    ThamSo("@MaVach", d.MaVach),
                                    ThamSo("@MaDVT", d.MaDVT), ThamSo("@MaDangDC", d.MaDangDC),
                                    ThamSo("@ThanhPhan", d.ThanhPhan), ThamSo("@NgaySX", d.NgaySX),
                                    ThamSo("@MaNuocSX", d.MaNuocSX), ThamSo("@HanSD", d.HanSD),
                                    ThamSo("@ChongChiDinh", d.ChongChiDinh));
                                tk.SoThuocMoi++;
                            }
                            else if (!string.IsNullOrWhiteSpace(d.MaVach))
                            {
                                // [10/11/2026] thuốc đã có: gán / cập nhật mã vạch theo file
                                ThucThi(conn, tx, "UPDATE Thuoc SET MaVach = @MaVach WHERE MaThuoc = @MaThuoc",
                                    ThamSo("@MaVach", d.MaVach), ThamSo("@MaThuoc", d.MaThuoc));
                            }

                            // [10/11/2026] gán công dụng (tự thêm công dụng chưa có trong CSDL)
                            foreach (string tenCD in d.DanhSachCongDung)
                            {
                                bool taoMoi;
                                string maCD = ThuocDAO.TimHoacTaoCongDung(conn, tx, tenCD, out taoMoi);
                                if (taoMoi) tk.SoCongDungMoi++;
                                if (maCD != null) ThuocDAO.ThemCongDungChoThuoc(conn, tx, d.MaThuoc, maCD);
                            }

                            ThemChiTietVaCapNhatKho(conn, tx, d.MaHDN, d.MaThuoc, d.SLNhap, d.DonGia, d.KhuyenMai);
                        }

                        CapNhatTongTienPhieu(conn, tx, dau.MaHDN);
                        tk.SoPhieu++;
                    }

                    tx.Commit();
                    return tk;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        // =====================================================================
        //  HÀM DÙNG CHUNG (trong transaction)
        // =====================================================================

        private static void ThemChiTietVaCapNhatKho(SqlConnection conn, SqlTransaction tx, string maHDN, string maThuoc,
                                                    int slNhap, decimal donGia, decimal khuyenMai)
        {
            decimal thanhTien = slNhap * donGia - khuyenMai;
            if (thanhTien < 0) thanhTien = 0;

            ThucThi(conn, tx,
                "INSERT INTO ChiTietHDN (MaHDN, MaThuoc, SLNhap, DonGia, KhuyenMai, ThanhTien) VALUES (@MaHDN, @MaThuoc, @SLNhap, @DonGia, @KhuyenMai, @ThanhTien)",
                ThamSo("@MaHDN", maHDN), ThamSo("@MaThuoc", maThuoc), ThamSo("@SLNhap", slNhap),
                ThamSo("@DonGia", donGia), ThamSo("@KhuyenMai", khuyenMai), ThamSo("@ThanhTien", thanhTien));

            int soDong = ThucThi(conn, tx,
                @"UPDATE Thuoc
                  SET SLHienCo = SLHienCo + @SLNhap,
                      DonGiaNhap = @DonGia,
                      GiaBan = @GiaBan
                  WHERE MaThuoc = @MaThuoc",
                ThamSo("@SLNhap", slNhap), ThamSo("@DonGia", donGia),
                ThamSo("@GiaBan", Math.Round(donGia * 1.1m, 2)), ThamSo("@MaThuoc", maThuoc));

            if (soDong == 0)
                throw new InvalidOperationException("Không tìm thấy thuốc " + maThuoc + " trong CSDL.");
        }

        private static void CapNhatTongTienPhieu(SqlConnection conn, SqlTransaction tx, string maHDN)
        {
            ThucThi(conn, tx,
                @"UPDATE HoaDonNhap
                  SET TongTien = (SELECT ISNULL(SUM(ThanhTien), 0) FROM ChiTietHDN WHERE MaHDN = @MaHDN)
                  WHERE MaHDN = @MaHDN",
                ThamSo("@MaHDN", maHDN));
        }

        private static int ThucThi(SqlConnection conn, SqlTransaction tx, string sql, params SqlParameter[] ps)
        {
            using (SqlCommand cmd = new SqlCommand(sql, conn, tx))
            {
                cmd.Parameters.AddRange(ps);
                return cmd.ExecuteNonQuery();
            }
        }

        private static object LayGiaTriTx(SqlConnection conn, SqlTransaction tx, string sql, params SqlParameter[] ps)
        {
            using (SqlCommand cmd = new SqlCommand(sql, conn, tx))
            {
                cmd.Parameters.AddRange(ps);
                object kq = cmd.ExecuteScalar();
                return kq == DBNull.Value ? null : kq;
            }
        }

        // Chuỗi rỗng / null -> DBNull (để cột cho phép NULL nhận giá trị NULL)
        private static SqlParameter ThamSo(string ten, object giaTri)
        {
            if (giaTri == null) return new SqlParameter(ten, DBNull.Value);
            string s = giaTri as string;
            if (s != null)
            {
                if (string.IsNullOrWhiteSpace(s)) return new SqlParameter(ten, DBNull.Value);
                giaTri = s.Trim();
            }
            return new SqlParameter(ten, giaTri);
        }

        private static bool GiongMa(string a, string b)
        {
            return string.Equals((a ?? "").Trim(), (b ?? "").Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}