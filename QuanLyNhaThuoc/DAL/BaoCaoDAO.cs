using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaThuoc.DAL
{
    public class BaoCaoDAO
    {
        private DBConnection db = new DBConnection();

        // yêu cầu 6: báo cáo thuốc còn hạn / hết hạn, sắp xếp theo hạn SD ngắn nhất
        // trangThai: -1 tất cả, 1 còn hạn, 0 hết hạn
        // soNgayCanhBao: còn <= số ngày này thì đánh dấu "sắp hết hạn"
        public DataTable BaoCaoHanSuDung(int trangThai, int soNgayCanhBao)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TrangThai", trangThai),
                new SqlParameter("@SoNgayCanhBao", soNgayCanhBao)
            };
            return ExecuteProcedure("sp_BaoCaoHanSuDung", parameters);
        }

        // yêu cầu 7: báo cáo danh sách HDN và tổng tiền nhập theo tháng/năm
        // Tables[0]: danh sách HĐN, Tables[1]: SoHoaDon + TongTienNhapThang
        public DataSet BaoCaoNhapHangTheoThang(int thang, int nam)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Thang", thang),
                new SqlParameter("@Nam", nam)
            };
            return ExecuteProcedureDataSet("sp_BaoCaoNhapHangTheoThang", parameters);
        }

        // yêu cầu 8: top 5 hóa đơn bán có tổng tiền ít nhất theo năm
        public DataTable BaoCaoTop5HDBItNhatTheoNam(int nam)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Nam", nam)
            };
            return ExecuteProcedure("sp_BaoCaoTop5HoaDonBanItNhatTheoNam", parameters);
        }

        // yêu cầu 9: top 2 nhân viên bán ít tiền nhất theo quý/năm
        // chiTinhNVCoBan = true: bỏ qua NV không có hóa đơn nào trong quý (quản lý, thủ kho...)
        public DataTable BaoCaoTop2NVBanItNhatTheoQuy(int quy, int nam, bool chiTinhNVCoBan)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Quy", quy),
                new SqlParameter("@Nam", nam),
                new SqlParameter("@ChiTinhNVCoBan", chiTinhNVCoBan)
            };
            return ExecuteProcedure("sp_BaoCaoTop2NhanVienBanItNhatTheoQuy", parameters);
        }

        // bổ sung: cảnh báo thuốc sắp hết hàng (SLHienCo < nguongSL)
        public DataTable CanhBaoThuocSapHet(int nguongSL)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@NguongSL", nguongSL)
            };
            return ExecuteProcedure("sp_CanhBaoThuocSapHet", parameters);
        }

        // bổ sung: danh sách cảnh báo (trang chi tiết khi bấm banner)
        // loai: 0 tất cả, 1 hết hạn, 2 cận hạn, 3 hết hàng, 4 gần hết hàng
        public DataTable DanhSachCanhBao(int loai, int soNgayCanhBao, int nguongSL)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Loai", loai),
                new SqlParameter("@SoNgayCanhBao", soNgayCanhBao),
                new SqlParameter("@NguongSL", nguongSL)
            };
            return ExecuteProcedure("sp_DanhSachCanhBao", parameters);
        }

        // bổ sung: hủy toàn bộ thuốc hết hạn còn tồn (SL về 0 + ghi LichSuHuyThuoc)
        // trả về 1 dòng: SoThuocHuy, TongSLHuy, TongGiaTriHuy
        public DataTable HuyThuocHetHan(string maNV, string lyDo)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNV", SqlDbType.VarChar, 10) { Value = (object)maNV ?? DBNull.Value },
                new SqlParameter("@LyDo", SqlDbType.NVarChar, 255) { Value = (object)lyDo ?? DBNull.Value }
            };
            return ExecuteProcedure("sp_HuyThuocHetHan", parameters);
        }

        // ================== [10/11/2026] DASHBOARD / BIỂU ĐỒ ==================

        // 1 dòng: DoanhThuHomNay, SoHoaDonHomNay, DoanhThuThangNay, SoHoaDonThangNay,
        //         TienNhapThangNay, SoKhachHang, SoThuocCanhBao
        public DataTable DashboardTongQuan(int nguongSL, int soNgayCanhBao)
        {
            return ExecuteProcedure("sp_Dashboard_TongQuan", new SqlParameter[]
            {
                new SqlParameter("@NguongSL", nguongSL),
                new SqlParameter("@SoNgayCanhBao", soNgayCanhBao)
            });
        }

        // 12 dòng: Thang, DoanhThu, SoHoaDon, TienNhap
        public DataTable DoanhThuTheoThang(int nam)
        {
            return ExecuteProcedure("sp_Dashboard_DoanhThuTheoThang", new SqlParameter[] { new SqlParameter("@Nam", nam) });
        }

        // MaThuoc, TenThuoc, SLBan, DoanhThu
        public DataTable TopThuocBanChay(int nam, int soLuong)
        {
            return ExecuteProcedure("sp_Dashboard_TopThuocBanChay", new SqlParameter[]
            {
                new SqlParameter("@Nam", nam),
                new SqlParameter("@SoLuong", soLuong)
            });
        }

        // MaNV, TenNV, SoHoaDon, DoanhThu (tăng dần theo doanh thu)
        public DataTable DoanhThuNhanVienTheoQuy(int quy, int nam, bool chiTinhNVCoBan)
        {
            return ExecuteProcedure("sp_Dashboard_DoanhThuNhanVienTheoQuy", new SqlParameter[]
            {
                new SqlParameter("@Quy", quy),
                new SqlParameter("@Nam", nam),
                new SqlParameter("@ChiTinhNVCoBan", chiTinhNVCoBan)
            });
        }

        // MaHDB, NgayBan, TongTien (tăng dần)
        public DataTable HoaDonBanTheoNam(int nam)
        {
            return ExecuteProcedure("sp_Dashboard_HoaDonBanTheoNam", new SqlParameter[] { new SqlParameter("@Nam", nam) });
        }

        // hàm dùng chung thực thi proc trả về DataTable
        private DataTable ExecuteProcedure(string procName, SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(procName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // hàm dùng chung thực thi proc trả về nhiều bảng
        private DataSet ExecuteProcedureDataSet(string procName, SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(procName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }
            }
            return ds;
        }
    }
}