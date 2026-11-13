using System;
using System.Data;

namespace QuanLyNhaThuoc.DAL
{
    /// <summary>
    /// [11/11/2026 lần 7] Đọc giá trị từ DataRow an toàn (DBNull / thiếu cột -> giá trị mặc định).
    /// </summary>
    internal static class DocDong
    {
        public static string Chuoi(DataRow r, string cot)
        {
            if (!r.Table.Columns.Contains(cot) || r[cot] == DBNull.Value) return null;
            return Convert.ToString(r[cot]).Trim();
        }

        public static int So(DataRow r, string cot)
        {
            if (!r.Table.Columns.Contains(cot) || r[cot] == DBNull.Value) return 0;
            return Convert.ToInt32(r[cot]);
        }

        public static decimal Tien(DataRow r, string cot)
        {
            if (!r.Table.Columns.Contains(cot) || r[cot] == DBNull.Value) return 0;
            return Convert.ToDecimal(r[cot]);
        }

        public static bool Co(DataRow r, string cot)
        {
            if (!r.Table.Columns.Contains(cot) || r[cot] == DBNull.Value) return false;
            return Convert.ToBoolean(r[cot]);
        }

        public static DateTime? Ngay(DataRow r, string cot)
        {
            if (!r.Table.Columns.Contains(cot) || r[cot] == DBNull.Value) return null;
            return Convert.ToDateTime(r[cot]);
        }
    }
}
