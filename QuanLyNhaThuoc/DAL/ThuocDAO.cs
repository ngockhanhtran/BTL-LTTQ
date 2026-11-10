using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaThuoc.DAL
{
    public class ThuocDAO
    {
        DBConnection db = new DBConnection();
        // 1. Lấy toàn bộ danh sách thuốc
        // [10/11/2026] Trước đây: SELECT * FROM Thuoc -> không có cột CongDung nên ô
        // "Công dụng" ở form Tra cứu luôn trống. Nay gọi sp_TimKiemThuoc với từ khóa rỗng
        // (proc trả về tất cả thuốc + cột CongDung).
        public DataTable GetDanhSachThuoc()
        {
            return TimKiemThuoc("");
        }
        // 2. 
        public DataTable TimKiemThuoc(string tuKhoa)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 100){Value = (object)tuKhoa ?? ""}
            };
            // Goi proc trong sql
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemThuoc", conn))
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
        public bool ThemThuoc(string maThuoc, string tenThuoc, string maDVT, string maDangDC,
                              string thanhPhan, decimal donGiaNhap, int slHienCo, DateTime ngaySX,
                              string maNuocSX, DateTime hanSD, string chongChiDinh)
        {
            // Yeu cau 3. Gia ban = 110% x Gia nhap
            decimal giaBan = donGiaNhap * 1.1m;

            string sql = @"INSERT INTO Thuoc (MaThuoc, TenThuoc, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan, SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh) 
                           VALUES (@MaThuoc, @TenThuoc, @MaDVT, @MaDangDC, @ThanhPhan, @DonGiaNhap, @GiaBan, @SLHienCo, @NgaySX, @MaNuocSX, @HanSD, @ChongChiDinh)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaThuoc", maThuoc),
                new SqlParameter("@TenThuoc", tenThuoc),
                new SqlParameter("@MaDVT", maDVT),
                new SqlParameter("@MaDangDC", maDangDC),
                new SqlParameter("@ThanhPhan", thanhPhan),
                new SqlParameter("@DonGiaNhap", donGiaNhap),
                new SqlParameter("@GiaBan", giaBan),
                new SqlParameter("@SLHienCo", slHienCo),
                new SqlParameter("@NgaySX", ngaySX),
                new SqlParameter("@MaNuocSX", maNuocSX),
                new SqlParameter("@HanSD", hanSD),
                new SqlParameter("@ChongChiDinh", chongChiDinh)
            };

            return db.ExecuteNonQuery(sql, parameters);
        }

        // =====================================================================
        //  [10/11/2026] MÃ VẠCH, CÔNG DỤNG, DANH MỤC THUỐC
        // =====================================================================

        /// <summary>Tìm thuốc để bán theo MÃ VẠCH (ưu tiên) hoặc MÃ THUỐC. Không có -> null.</summary>
        public QuanLyNhaThuoc.DTO.ThuocBan TimThuocBan(string ma)
        {
            DataTable dt = SqlHelper.Query(@"
                SELECT TOP 1 MaThuoc, TenThuoc, MaVach, GiaBan, SLHienCo, HanSD
                FROM Thuoc
                WHERE MaVach = @Ma OR MaThuoc = @Ma
                ORDER BY CASE WHEN MaVach = @Ma THEN 0 ELSE 1 END",
                SqlHelper.PChuoi("@Ma", (ma ?? "").Trim()));
            if (dt.Rows.Count == 0) return null;
            DataRow r = dt.Rows[0];
            return new QuanLyNhaThuoc.DTO.ThuocBan
            {
                MaThuoc = r["MaThuoc"].ToString().Trim(),
                TenThuoc = r["TenThuoc"].ToString(),
                MaVach = r["MaVach"] == DBNull.Value ? null : r["MaVach"].ToString(),
                GiaBan = r["GiaBan"] == DBNull.Value ? 0 : Convert.ToDecimal(r["GiaBan"]),
                SLHienCo = r["SLHienCo"] == DBNull.Value ? 0 : Convert.ToInt32(r["SLHienCo"]),
                HanSD = r["HanSD"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["HanSD"])
            };
        }

        /// <summary>Mã thuốc đang dùng mã vạch này (null nếu chưa ai dùng).</summary>
        public string LayMaThuocTheoMaVach(string maVach)
        {
            object o = SqlHelper.Scalar("SELECT MaThuoc FROM Thuoc WHERE MaVach = @MaVach",
                                        SqlHelper.PChuoi("@MaVach", (maVach ?? "").Trim()));
            return o == null ? null : o.ToString().Trim();
        }

        public string SinhMaMoi()
        {
            return SqlHelper.SinhMaTiepTheo("Thuoc", "MaThuoc", "TH", 3);
        }

        public bool TonTai(string maThuoc)
        {
            return SqlHelper.Scalar("SELECT 1 FROM Thuoc WHERE MaThuoc = @Ma", SqlHelper.PChuoi("@Ma", maThuoc)) != null;
        }

        /// <summary>Danh sách cho form Danh mục thuốc: kèm tên ĐVT, dạng ĐC, nước SX, công dụng.</summary>
        public DataTable LayDanhSachQuanLy(string tuKhoa)
        {
            return SqlHelper.Query(@"
                SELECT t.MaThuoc, t.TenThuoc, t.MaVach, t.MaDVT, dvt.TenDVT, t.MaDangDC, dc.TenDangDC,
                       t.ThanhPhan, t.DonGiaNhap, t.GiaBan, t.SLHienCo, t.NgaySX, t.HanSD,
                       t.MaNuocSX, nsx.TenNuocSX, t.ChongChiDinh,
                       STUFF((SELECT N', ' + cd.TenCongDung
                              FROM Thuoc_CongDung tcd JOIN CongDung cd ON cd.MaCongDung = tcd.MaCongDung
                              WHERE tcd.MaThuoc = t.MaThuoc
                              ORDER BY cd.MaCongDung
                              FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, N'') AS CongDung
                FROM Thuoc t
                LEFT JOIN DonViTinh dvt  ON dvt.MaDVT = t.MaDVT
                LEFT JOIN DangDieuChe dc ON dc.MaDangDC = t.MaDangDC
                LEFT JOIN NuocSX nsx     ON nsx.MaNuocSX = t.MaNuocSX
                WHERE @TuKhoa = N''
                   OR t.MaThuoc LIKE N'%' + @TuKhoa + N'%'
                   OR t.TenThuoc LIKE N'%' + @TuKhoa + N'%'
                   OR t.MaVach LIKE N'%' + @TuKhoa + N'%'
                   OR t.ThanhPhan LIKE N'%' + @TuKhoa + N'%'
                ORDER BY t.MaThuoc",
                SqlHelper.PChuoi("@TuKhoa", (tuKhoa ?? "").Trim()));
        }

        public DataTable LayTatCaCongDung()
        {
            return SqlHelper.Query("SELECT MaCongDung, TenCongDung FROM CongDung ORDER BY TenCongDung");
        }

        public List<string> LayMaCongDungCuaThuoc(string maThuoc)
        {
            DataTable dt = SqlHelper.Query("SELECT MaCongDung FROM Thuoc_CongDung WHERE MaThuoc = @Ma",
                                           SqlHelper.PChuoi("@Ma", maThuoc));
            return dt.Rows.Cast<DataRow>().Select(r => r[0].ToString().Trim()).ToList();
        }

        /// <summary>Tìm mã công dụng theo MÃ hoặc TÊN (không phân biệt hoa thường). Không có -> null.</summary>
        public string TimMaCongDung(string maHoacTen)
        {
            using (SqlConnection conn = SqlHelper.MoKetNoi())
                return TimMaCongDung(conn, null, maHoacTen);
        }

        /// <summary>Thêm công dụng mới (tự sinh mã CDxx). Tên đã có thì trả về mã cũ.</summary>
        public string ThemCongDung(string ten)
        {
            using (SqlConnection conn = SqlHelper.MoKetNoi())
            {
                bool taoMoi;
                return TimHoacTaoCongDung(conn, null, ten, out taoMoi);
            }
        }

        /// <summary>Gán lại danh sách công dụng cho thuốc (bỏ cái không còn chọn, thêm cái mới; giữ ghi chú cũ).</summary>
        public void CapNhatCongDung(string maThuoc, IEnumerable<string> dsMaCongDung)
        {
            using (SqlConnection conn = SqlHelper.MoKetNoi())
            using (SqlTransaction tx = conn.BeginTransaction())
            {
                try
                {
                    DatCongDung(conn, tx, maThuoc, dsMaCongDung);
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public bool CapNhatChongChiDinh(string maThuoc, string chongChiDinh)
        {
            return SqlHelper.NonQuery("UPDATE Thuoc SET ChongChiDinh = @CCD WHERE MaThuoc = @Ma",
                                      SqlHelper.P("@CCD", chongChiDinh), SqlHelper.PChuoi("@Ma", maThuoc)) > 0;
        }

        /// <summary>Thêm thuốc mới (tồn kho = 0, tồn kho chỉ tăng qua Nhập hàng) + gán công dụng.</summary>
        public void Them(QuanLyNhaThuoc.DTO.ThuocDTO t)
        {
            using (SqlConnection conn = SqlHelper.MoKetNoi())
            using (SqlTransaction tx = conn.BeginTransaction())
            {
                try
                {
                    SqlHelper.NonQuery(conn, tx, @"
                        INSERT INTO Thuoc (MaThuoc, TenThuoc, MaVach, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan,
                                           SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh)
                        VALUES (@MaThuoc, @TenThuoc, @MaVach, @MaDVT, @MaDangDC, @ThanhPhan, @DonGiaNhap, @GiaBan,
                                0, @NgaySX, @MaNuocSX, @HanSD, @ChongChiDinh)",
                        ThamSoThuoc(t));
                    DatCongDung(conn, tx, t.MaThuoc, t.CongDung);
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        /// <summary>Sửa thông tin thuốc (KHÔNG sửa tồn kho) + gán lại công dụng.</summary>
        public void Sua(QuanLyNhaThuoc.DTO.ThuocDTO t)
        {
            using (SqlConnection conn = SqlHelper.MoKetNoi())
            using (SqlTransaction tx = conn.BeginTransaction())
            {
                try
                {
                    SqlHelper.NonQuery(conn, tx, @"
                        UPDATE Thuoc
                        SET TenThuoc = @TenThuoc, MaVach = @MaVach, MaDVT = @MaDVT, MaDangDC = @MaDangDC,
                            ThanhPhan = @ThanhPhan, DonGiaNhap = @DonGiaNhap, GiaBan = @GiaBan,
                            NgaySX = @NgaySX, MaNuocSX = @MaNuocSX, HanSD = @HanSD, ChongChiDinh = @ChongChiDinh
                        WHERE MaThuoc = @MaThuoc",
                        ThamSoThuoc(t));
                    DatCongDung(conn, tx, t.MaThuoc, t.CongDung);
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        /// <summary>Xóa thuốc. Ném SqlException 547 nếu thuốc đã có trong hóa đơn / phiếu nhập / lịch sử hủy.</summary>
        public void Xoa(string maThuoc)
        {
            using (SqlConnection conn = SqlHelper.MoKetNoi())
            using (SqlTransaction tx = conn.BeginTransaction())
            {
                try
                {
                    SqlHelper.NonQuery(conn, tx, "DELETE FROM Thuoc_CongDung WHERE MaThuoc = @Ma", SqlHelper.PChuoi("@Ma", maThuoc));
                    SqlHelper.NonQuery(conn, tx, "DELETE FROM Thuoc WHERE MaThuoc = @Ma", SqlHelper.PChuoi("@Ma", maThuoc));
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        // ---------- hàm dùng chung trong transaction (cả import Excel cũng dùng) ----------

        internal static string TimMaCongDung(SqlConnection conn, SqlTransaction tx, string maHoacTen)
        {
            string s = (maHoacTen ?? "").Trim();
            if (s == "") return null;
            object o = SqlHelper.Scalar(conn, tx, @"
                SELECT TOP 1 MaCongDung FROM CongDung
                WHERE MaCongDung = @X OR TenCongDung = @X
                ORDER BY CASE WHEN MaCongDung = @X THEN 0 ELSE 1 END",
                SqlHelper.PChuoi("@X", s));
            return o == null ? null : o.ToString().Trim();
        }

        internal static string TimHoacTaoCongDung(SqlConnection conn, SqlTransaction tx, string maHoacTen, out bool taoMoi)
        {
            taoMoi = false;
            string s = (maHoacTen ?? "").Trim();
            if (s == "") return null;
            string ma = TimMaCongDung(conn, tx, s);
            if (ma != null) return ma;

            ma = SqlHelper.SinhMaTiepTheo(conn, tx, "CongDung", "MaCongDung", "CD", 2);
            SqlHelper.NonQuery(conn, tx, "INSERT INTO CongDung (MaCongDung, TenCongDung) VALUES (@Ma, @Ten)",
                               SqlHelper.PChuoi("@Ma", ma), SqlHelper.PChuoi("@Ten", s));
            taoMoi = true;
            return ma;
        }

        /// <summary>Gán thêm công dụng cho thuốc (không xóa công dụng cũ).</summary>
        internal static void ThemCongDungChoThuoc(SqlConnection conn, SqlTransaction tx, string maThuoc, string maCongDung)
        {
            SqlHelper.NonQuery(conn, tx, @"
                IF NOT EXISTS (SELECT 1 FROM Thuoc_CongDung WHERE MaThuoc = @MaThuoc AND MaCongDung = @MaCD)
                    INSERT INTO Thuoc_CongDung (MaThuoc, MaCongDung) VALUES (@MaThuoc, @MaCD)",
                SqlHelper.PChuoi("@MaThuoc", maThuoc), SqlHelper.PChuoi("@MaCD", maCongDung));
        }

        /// <summary>Đặt danh sách công dụng của thuốc đúng bằng dsMaCongDung.</summary>
        private static void DatCongDung(SqlConnection conn, SqlTransaction tx, string maThuoc, IEnumerable<string> dsMaCongDung)
        {
            HashSet<string> moi = new HashSet<string>(
                (dsMaCongDung ?? Enumerable.Empty<string>()).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()),
                StringComparer.OrdinalIgnoreCase);

            DataTable cu = SqlHelper.Query(conn, tx, "SELECT MaCongDung FROM Thuoc_CongDung WHERE MaThuoc = @Ma",
                                           SqlHelper.PChuoi("@Ma", maThuoc));
            foreach (DataRow r in cu.Rows)
            {
                string ma = r[0].ToString().Trim();
                if (!moi.Contains(ma))
                    SqlHelper.NonQuery(conn, tx, "DELETE FROM Thuoc_CongDung WHERE MaThuoc = @MaThuoc AND MaCongDung = @MaCD",
                                       SqlHelper.PChuoi("@MaThuoc", maThuoc), SqlHelper.PChuoi("@MaCD", ma));
            }
            foreach (string ma in moi)
                ThemCongDungChoThuoc(conn, tx, maThuoc, ma);
        }

        private static SqlParameter[] ThamSoThuoc(QuanLyNhaThuoc.DTO.ThuocDTO t)
        {
            return new SqlParameter[]
            {
                SqlHelper.P("@MaThuoc", t.MaThuoc),
                SqlHelper.P("@TenThuoc", t.TenThuoc),
                SqlHelper.P("@MaVach", t.MaVach),
                SqlHelper.P("@MaDVT", t.MaDVT),
                SqlHelper.P("@MaDangDC", t.MaDangDC),
                SqlHelper.P("@ThanhPhan", t.ThanhPhan),
                SqlHelper.P("@DonGiaNhap", t.DonGiaNhap),
                SqlHelper.P("@GiaBan", t.GiaBan),
                SqlHelper.P("@NgaySX", t.NgaySX),
                SqlHelper.P("@MaNuocSX", t.MaNuocSX),
                SqlHelper.P("@HanSD", t.HanSD),
                SqlHelper.P("@ChongChiDinh", t.ChongChiDinh)
            };
        }
    }

}
