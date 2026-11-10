using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>
    /// [10/11/2026 lần 5] Trả hàng / hủy hóa đơn bán.
    /// Mỗi lần trả = 1 phiếu PhieuTraHang (mã PTHxxx). Từng dòng gọi sp_TraHangDong:
    /// giảm SL + thành tiền trên ChiTietHDB (báo cáo doanh thu tự đúng), trả hàng về đúng lô đã xuất.
    /// Trả hết mọi dòng -> hóa đơn được đánh dấu DaHuy = 1 (các báo cáo bỏ qua).
    /// </summary>
    public class TraHangDAO
    {
        public DataTable TimHoaDon(string tuKhoa, DateTime tuNgay, DateTime denNgay)
        {
            return SqlHelper.Query(@"
                SELECT h.MaHDB, h.NgayBan, h.MaNV, nv.TenNV, h.MaKhach, kh.TenKhach, h.TongTien,
                       ISNULL(pt.TienDaHoan, 0) AS TienDaHoan, h.MaDonThuoc, h.DaHuy,
                       CASE WHEN h.DaHuy = 1 THEN N'Đã hủy'
                            WHEN pt.TienDaHoan IS NOT NULL THEN N'Đã trả 1 phần'
                            ELSE N'Bình thường' END AS TrangThai
                FROM HoaDonBan h
                LEFT JOIN NhanVien nv  ON nv.MaNV = h.MaNV
                LEFT JOIN KhachHang kh ON kh.MaKhach = h.MaKhach
                LEFT JOIN (SELECT MaHDB, SUM(TienHoan) AS TienDaHoan FROM PhieuTraHang GROUP BY MaHDB) pt ON pt.MaHDB = h.MaHDB
                WHERE h.NgayBan >= @TuNgay AND h.NgayBan < @DenNgay
                  AND (@TuKhoa = N'' OR h.MaHDB LIKE N'%' + @TuKhoa + N'%' OR h.MaKhach LIKE N'%' + @TuKhoa + N'%'
                       OR kh.TenKhach LIKE N'%' + @TuKhoa + N'%' OR kh.DienThoai LIKE N'%' + @TuKhoa + N'%')
                ORDER BY h.NgayBan DESC, h.MaHDB DESC",
                SqlHelper.P("@TuNgay", tuNgay.Date),
                SqlHelper.P("@DenNgay", denNgay.Date.AddDays(1)),
                SqlHelper.PChuoi("@TuKhoa", (tuKhoa ?? "").Trim()));
        }

        public DataRow LayHoaDon(string maHDB)
        {
            DataTable dt = SqlHelper.Query(@"
                SELECT h.MaHDB, h.NgayBan, h.MaNV, h.MaKhach, kh.TenKhach, h.TongTien, h.DiemCong, h.DiemSuDung, h.DaHuy,
                       ISNULL((SELECT SUM(TienHoan) FROM PhieuTraHang p WHERE p.MaHDB = h.MaHDB), 0) AS TienDaHoan,
                       ISNULL((SELECT SUM(DiemTru) FROM PhieuTraHang p WHERE p.MaHDB = h.MaHDB), 0) AS DiemDaTru
                FROM HoaDonBan h
                LEFT JOIN KhachHang kh ON kh.MaKhach = h.MaKhach
                WHERE h.MaHDB = @Ma",
                SqlHelper.PChuoi("@Ma", maHDB));
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>Các dòng của hóa đơn: SL đã mua, đã trả, còn lại (có thể trả), lô đã xuất.</summary>
        public DataTable LayChiTiet(string maHDB)
        {
            return SqlHelper.Query(@"
                SELECT c.MaThuoc, t.TenThuoc,
                       c.SoLuong + ISNULL(tr.SLDaTra, 0) AS SLDaMua,
                       ISNULL(tr.SLDaTra, 0)             AS SLDaTra,
                       c.SoLuong                         AS SLConLai,
                       CASE WHEN c.SoLuong > 0 THEN ROUND(c.ThanhTien / c.SoLuong, 0) ELSE 0 END AS DonGiaThuc,
                       c.ThanhTien                       AS ThanhTienConLai,
                       STUFF((SELECT N', ' + l.SoLo + N' (' + CAST(x.SoLuong AS NVARCHAR(10)) + N')'
                              FROM ChiTietHDB_Lo x JOIN LoThuoc l ON l.MaLo = x.MaLo
                              WHERE x.MaHDB = c.MaHDB AND x.MaThuoc = c.MaThuoc
                              ORDER BY l.HanSD
                              FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, N'') AS LoDaXuat
                FROM ChiTietHDB c
                JOIN Thuoc t ON t.MaThuoc = c.MaThuoc
                LEFT JOIN (SELECT ct.MaThuoc, SUM(ct.SoLuong) AS SLDaTra
                           FROM ChiTietTraHang ct JOIN PhieuTraHang p ON p.MaPTH = ct.MaPTH
                           WHERE p.MaHDB = @Ma GROUP BY ct.MaThuoc) tr ON tr.MaThuoc = c.MaThuoc
                WHERE c.MaHDB = @Ma
                ORDER BY t.TenThuoc",
                SqlHelper.PChuoi("@Ma", maHDB));
        }

        public DataTable LayPhieuTra(string maHDB)
        {
            return SqlHelper.Query(@"
                SELECT p.MaPTH, p.NgayTra, p.MaNV, nv.TenNV, p.LyDo, p.TienHoan, p.DiemTru, p.DiemHoan,
                       CASE WHEN p.LaHuyHoaDon = 1 THEN N'Hủy hóa đơn' ELSE N'Trả 1 phần' END AS Loai,
                       STUFF((SELECT N', ' + ct.MaThuoc + N' x' + CAST(ct.SoLuong AS NVARCHAR(10))
                              FROM ChiTietTraHang ct WHERE ct.MaPTH = p.MaPTH
                              FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, N'') AS ThuocTra
                FROM PhieuTraHang p
                LEFT JOIN NhanVien nv ON nv.MaNV = p.MaNV
                WHERE p.MaHDB = @Ma
                ORDER BY p.NgayTra",
                SqlHelper.PChuoi("@Ma", maHDB));
        }

        /// <summary>
        /// Lập phiếu trả hàng trong 1 transaction. huyHoaDon = true: trả toàn bộ phần còn lại,
        /// trả lại điểm khách đã dùng và hóa đơn chuyển sang "Đã hủy".
        /// </summary>
        public KetQuaTraHang TraHang(string maHDB, List<DongTraHang> dsTra, string lyDo, string maNV, bool huyHoaDon)
        {
            KetQuaTraHang kq = new KetQuaTraHang();
            using (SqlConnection conn = SqlHelper.MoKetNoi())
            using (SqlTransaction tx = conn.BeginTransaction())
            {
                try
                {
                    // Khóa hóa đơn để 2 máy không trả cùng lúc
                    DataTable dtHD = SqlHelper.Query(conn, tx, @"
                        SELECT MaKhach, TongTien, DiemCong, DiemSuDung, DaHuy
                        FROM HoaDonBan WITH (UPDLOCK, ROWLOCK) WHERE MaHDB = @Ma",
                        SqlHelper.PChuoi("@Ma", maHDB));
                    if (dtHD.Rows.Count == 0) throw new InvalidOperationException("Không tìm thấy hóa đơn " + maHDB + ".");
                    DataRow hd = dtHD.Rows[0];
                    if (Convert.ToBoolean(hd["DaHuy"])) throw new InvalidOperationException("Hóa đơn " + maHDB + " đã bị hủy trước đó.");

                    string maKhach = hd["MaKhach"] == DBNull.Value ? null : hd["MaKhach"].ToString().Trim();
                    decimal tongTruoc = hd["TongTien"] == DBNull.Value ? 0 : Convert.ToDecimal(hd["TongTien"]);
                    int diemCong = Convert.ToInt32(hd["DiemCong"]);
                    int diemSuDung = Convert.ToInt32(hd["DiemSuDung"]);
                    object o = SqlHelper.Scalar(conn, tx, "SELECT SUM(TienHoan) FROM PhieuTraHang WHERE MaHDB = @Ma", SqlHelper.PChuoi("@Ma", maHDB));
                    decimal daHoanTruoc = o == null ? 0 : Convert.ToDecimal(o);
                    o = SqlHelper.Scalar(conn, tx, "SELECT SUM(DiemTru) FROM PhieuTraHang WHERE MaHDB = @Ma", SqlHelper.PChuoi("@Ma", maHDB));
                    int diemDaTru = o == null ? 0 : Convert.ToInt32(o);
                    decimal tongGoc = tongTruoc + daHoanTruoc;

                    // Hủy cả hóa đơn: lấy hết các dòng còn lại
                    if (huyHoaDon)
                    {
                        dsTra = new List<DongTraHang>();
                        foreach (DataRow r in SqlHelper.Query(conn, tx,
                                     "SELECT MaThuoc, SoLuong FROM ChiTietHDB WHERE MaHDB = @Ma AND SoLuong > 0",
                                     SqlHelper.PChuoi("@Ma", maHDB)).Rows)
                            dsTra.Add(new DongTraHang { MaThuoc = r["MaThuoc"].ToString().Trim(), SoLuong = Convert.ToInt32(r["SoLuong"]) });
                    }
                    if (dsTra == null || dsTra.Count == 0) throw new InvalidOperationException("Không có thuốc nào để trả.");

                    // Phiếu trả
                    kq.MaPTH = SqlHelper.SinhMaTiepTheo(conn, tx, "PhieuTraHang", "MaPTH", "PTH", 3);
                    SqlHelper.NonQuery(conn, tx, @"
                        INSERT INTO PhieuTraHang (MaPTH, MaHDB, NgayTra, MaNV, LyDo, TienHoan, DiemTru, DiemHoan, LaHuyHoaDon)
                        VALUES (@MaPTH, @MaHDB, GETDATE(), @MaNV, @LyDo, 0, 0, 0, 0)",
                        SqlHelper.PChuoi("@MaPTH", kq.MaPTH), SqlHelper.PChuoi("@MaHDB", maHDB),
                        SqlHelper.P("@MaNV", maNV), SqlHelper.P("@LyDo", lyDo));

                    foreach (DongTraHang d in dsTra)
                    {
                        if (d.SoLuong <= 0) continue;
                        SqlParameter pTien = new SqlParameter("@TienHoan", SqlDbType.Decimal) { Precision = 18, Scale = 2, Direction = ParameterDirection.Output };
                        SqlHelper.ExecProc(conn, tx, "sp_TraHangDong",
                            SqlHelper.PChuoi("@MaPTH", kq.MaPTH), SqlHelper.PChuoi("@MaHDB", maHDB),
                            SqlHelper.PChuoi("@MaThuoc", d.MaThuoc), SqlHelper.P("@SoLuong", d.SoLuong), pTien);
                        kq.TienHoan += pTien.Value == DBNull.Value ? 0 : Convert.ToDecimal(pTien.Value);
                    }

                    // Còn dòng nào chưa trả hết không?
                    object con = SqlHelper.Scalar(conn, tx, "SELECT COUNT(*) FROM ChiTietHDB WHERE MaHDB = @Ma AND SoLuong > 0",
                                                  SqlHelper.PChuoi("@Ma", maHDB));
                    kq.DaHuyHoaDon = Convert.ToInt32(con) == 0;

                    // Điểm: trả hết -> trừ toàn bộ điểm còn lại đã cộng + hoàn điểm đã dùng;
                    //       trả 1 phần -> trừ điểm theo tỷ lệ tiền hoàn / tổng tiền gốc
                    if (kq.DaHuyHoaDon)
                    {
                        kq.DiemTru = Math.Max(0, diemCong - diemDaTru);
                        kq.DiemHoan = diemSuDung;
                    }
                    else if (tongGoc > 0)
                    {
                        kq.DiemTru = (int)Math.Floor(diemCong * kq.TienHoan / tongGoc);
                        kq.DiemTru = Math.Max(0, Math.Min(kq.DiemTru, diemCong - diemDaTru));
                    }

                    if (maKhach != null && (kq.DiemTru > 0 || kq.DiemHoan > 0))
                    {
                        object diemCo = SqlHelper.Scalar(conn, tx, "SELECT DiemTichLuy FROM KhachHang WITH (UPDLOCK) WHERE MaKhach = @Ma",
                                                         SqlHelper.PChuoi("@Ma", maKhach));
                        int co = diemCo == null ? 0 : Convert.ToInt32(diemCo);
                        int moi = co - kq.DiemTru + kq.DiemHoan;
                        if (moi < 0)
                        {
                            kq.GhiChu.Add("Khách đã dùng bớt điểm nên chỉ trừ được " + (co + kq.DiemHoan) + " điểm (thiếu " + (-moi) + ").");
                            moi = 0;
                        }
                        SqlHelper.NonQuery(conn, tx, "UPDATE KhachHang SET DiemTichLuy = @Diem WHERE MaKhach = @Ma",
                                           SqlHelper.P("@Diem", moi), SqlHelper.PChuoi("@Ma", maKhach));
                        kq.DiemConLai = moi;
                    }
                    else if (maKhach != null)
                    {
                        object diemCo = SqlHelper.Scalar(conn, tx, "SELECT DiemTichLuy FROM KhachHang WHERE MaKhach = @Ma",
                                                         SqlHelper.PChuoi("@Ma", maKhach));
                        kq.DiemConLai = diemCo == null ? 0 : Convert.ToInt32(diemCo);
                    }

                    SqlHelper.NonQuery(conn, tx, @"
                        UPDATE PhieuTraHang SET TienHoan = @Tien, DiemTru = @Tru, DiemHoan = @Hoan, LaHuyHoaDon = @Huy
                        WHERE MaPTH = @MaPTH",
                        SqlHelper.P("@Tien", kq.TienHoan), SqlHelper.P("@Tru", kq.DiemTru), SqlHelper.P("@Hoan", kq.DiemHoan),
                        SqlHelper.P("@Huy", kq.DaHuyHoaDon), SqlHelper.PChuoi("@MaPTH", kq.MaPTH));

                    if (kq.DaHuyHoaDon)
                        SqlHelper.NonQuery(conn, tx, @"
                            UPDATE HoaDonBan SET DaHuy = 1, NgayHuy = GETDATE(), MaNVHuy = @MaNV, LyDoHuy = @LyDo
                            WHERE MaHDB = @Ma",
                            SqlHelper.P("@MaNV", maNV), SqlHelper.P("@LyDo", lyDo), SqlHelper.PChuoi("@Ma", maHDB));

                    tx.Commit();
                    return kq;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}
