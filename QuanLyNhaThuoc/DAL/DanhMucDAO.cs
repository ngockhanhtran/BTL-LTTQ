using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>
    /// [10/11/2026] Thêm / sửa / xóa cho các bảng danh mục đơn giản (Khách hàng, NCC, Công dụng,
    /// Đơn vị tính, Dạng điều chế, Nước SX). Tên bảng / cột lấy từ DanhSachDanhMuc (cố định trong
    /// code, không lấy từ người dùng) nên ghép vào câu SQL được; giá trị luôn truyền bằng tham số.
    /// </summary>
    public class DanhMucDAO
    {
        public DataTable LayDanhSach(CauHinhDanhMuc ch, string tuKhoa)
        {
            string dsCot = string.Join(", ", ch.Cot.Select(c => "[" + c.Ten + "]"));
            string dieuKien = string.Join(" OR ", ch.Cot.Select(c => "CAST([" + c.Ten + "] AS NVARCHAR(255)) LIKE N'%' + @TuKhoa + N'%'"));
            string sql = "SELECT " + dsCot + " FROM [" + ch.Bang + "] WHERE @TuKhoa = N'' OR " + dieuKien +
                         " ORDER BY [" + ch.CotKhoa.Ten + "]";
            return SqlHelper.Query(sql, SqlHelper.PChuoi("@TuKhoa", (tuKhoa ?? "").Trim()));
        }

        /// <summary>Dữ liệu cho ComboBox: cột Ma, Ten, HienThi ("Ma - Ten").</summary>
        public DataTable LayDanhSachChon(CauHinhDanhMuc ch)
        {
            string sql = "SELECT [" + ch.CotKhoa.Ten + "] AS Ma, [" + ch.CotTen.Ten + "] AS Ten, " +
                         "RTRIM([" + ch.CotKhoa.Ten + "]) + N' - ' + [" + ch.CotTen.Ten + "] AS HienThi " +
                         "FROM [" + ch.Bang + "] ORDER BY [" + ch.CotTen.Ten + "]";
            return SqlHelper.Query(sql);
        }

        public bool TonTai(CauHinhDanhMuc ch, string ma)
        {
            return SqlHelper.Scalar("SELECT 1 FROM [" + ch.Bang + "] WHERE [" + ch.CotKhoa.Ten + "] = @Ma",
                                    SqlHelper.PChuoi("@Ma", ma)) != null;
        }

        public void Them(CauHinhDanhMuc ch, Dictionary<string, object> giaTri)
        {
            List<CotDanhMuc> cots = ch.Cot.Where(c => !c.ChiDoc).ToList();
            string sql = "INSERT INTO [" + ch.Bang + "] (" + string.Join(", ", cots.Select(c => "[" + c.Ten + "]")) +
                         ") VALUES (" + string.Join(", ", cots.Select(c => "@" + c.Ten)) + ")";
            SqlHelper.NonQuery(sql, ThamSo(cots, giaTri));
        }

        public void Sua(CauHinhDanhMuc ch, Dictionary<string, object> giaTri)
        {
            List<CotDanhMuc> cots = ch.Cot.Where(c => !c.ChiDoc).ToList();
            string sql = "UPDATE [" + ch.Bang + "] SET " +
                         string.Join(", ", cots.Skip(1).Select(c => "[" + c.Ten + "] = @" + c.Ten)) +
                         " WHERE [" + ch.CotKhoa.Ten + "] = @" + ch.CotKhoa.Ten;
            SqlHelper.NonQuery(sql, ThamSo(cots, giaTri));
        }

        /// <summary>Ném SqlException 547 nếu bản ghi đang được bảng khác tham chiếu.</summary>
        public void Xoa(CauHinhDanhMuc ch, string ma)
        {
            SqlHelper.NonQuery("DELETE FROM [" + ch.Bang + "] WHERE [" + ch.CotKhoa.Ten + "] = @Ma",
                               SqlHelper.PChuoi("@Ma", ma));
        }

        public string SinhMaMoi(CauHinhDanhMuc ch)
        {
            return SqlHelper.SinhMaTiepTheo(ch.Bang, ch.CotKhoa.Ten, ch.TienToMa, ch.DoDaiSoMa);
        }

        private static SqlParameter[] ThamSo(List<CotDanhMuc> cots, Dictionary<string, object> giaTri)
        {
            return cots.Select(c =>
            {
                object v;
                giaTri.TryGetValue(c.Ten, out v);
                return SqlHelper.P("@" + c.Ten, v);
            }).ToArray();
        }
    }
}
