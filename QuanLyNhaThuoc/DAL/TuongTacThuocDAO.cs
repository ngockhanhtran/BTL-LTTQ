using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>
    /// [11/11/2026 lần 7] Bảng TuongTacThuoc: cặp thuốc không nên dùng chung (MaThuoc1 &lt; MaThuoc2).
    /// DỮ LIỆU MẪU ĐỂ DEMO, không phải cơ sở dữ liệu y khoa.
    /// </summary>
    public class TuongTacThuocDAO
    {
        public List<CanhBaoTuongTac> KiemTra(IEnumerable<string> dsMaThuoc)
        {
            string ds = string.Join(",", dsMaThuoc.Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.Trim()));
            DataTable dt = SqlHelper.QueryProc("sp_KiemTraTuongTac",
                new SqlParameter("@DanhSachMa", SqlDbType.NVarChar, -1) { Value = ds });
            return dt.Rows.Cast<DataRow>().Select(DocCanhBao).ToList();
        }

        public DataTable DanhSach(string tuKhoa)
        {
            return SqlHelper.QueryProc("sp_DanhSachTuongTac", SqlHelper.PChuoi("@TuKhoa", tuKhoa ?? ""));
        }

        public bool TonTai(string ma1, string ma2)
        {
            return SqlHelper.Scalar("SELECT 1 FROM TuongTacThuoc WHERE MaThuoc1 = @A AND MaThuoc2 = @B",
                SqlHelper.PChuoi("@A", ma1), SqlHelper.PChuoi("@B", ma2)) != null;
        }

        public void Them(string ma1, string ma2, int mucDo, string moTa)
        {
            SqlHelper.NonQuery("INSERT INTO TuongTacThuoc (MaThuoc1, MaThuoc2, MucDo, MoTa) VALUES (@A, @B, @M, @T)",
                SqlHelper.PChuoi("@A", ma1), SqlHelper.PChuoi("@B", ma2), SqlHelper.P("@M", mucDo), new SqlParameter("@T", SqlDbType.NVarChar, 500) { Value = moTa ?? "" });
        }

        public int Sua(string ma1, string ma2, int mucDo, string moTa)
        {
            return SqlHelper.NonQuery("UPDATE TuongTacThuoc SET MucDo = @M, MoTa = @T WHERE MaThuoc1 = @A AND MaThuoc2 = @B",
                SqlHelper.PChuoi("@A", ma1), SqlHelper.PChuoi("@B", ma2), SqlHelper.P("@M", mucDo), new SqlParameter("@T", SqlDbType.NVarChar, 500) { Value = moTa ?? "" });
        }

        public int Xoa(string ma1, string ma2)
        {
            return SqlHelper.NonQuery("DELETE FROM TuongTacThuoc WHERE MaThuoc1 = @A AND MaThuoc2 = @B",
                SqlHelper.PChuoi("@A", ma1), SqlHelper.PChuoi("@B", ma2));
        }

        /// <summary>Nạp dữ liệu mẫu demo (cặp có cả 2 thuốc trong CSDL). Trả về số cặp vừa thêm.</summary>
        public int NapDuLieuMau()
        {
            DataTable dt = SqlHelper.QueryProc("sp_NapTuongTacMau");
            return dt.Rows.Count > 0 ? DocDong.So(dt.Rows[0], "SoCapDaThem") : 0;
        }

        private static CanhBaoTuongTac DocCanhBao(DataRow r)
        {
            return new CanhBaoTuongTac
            {
                MaThuoc1 = DocDong.Chuoi(r, "MaThuoc1"),
                TenThuoc1 = DocDong.Chuoi(r, "TenThuoc1"),
                MaThuoc2 = DocDong.Chuoi(r, "MaThuoc2"),
                TenThuoc2 = DocDong.Chuoi(r, "TenThuoc2"),
                MucDo = DocDong.So(r, "MucDo"),
                TenMucDo = DocDong.Chuoi(r, "TenMucDo"),
                MoTa = DocDong.Chuoi(r, "MoTa")
            };
        }
    }
}
