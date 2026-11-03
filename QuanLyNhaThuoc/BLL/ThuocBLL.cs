using System;
using System.Data;
using QuanLyNhaThuoc.DAL;

namespace QuanLyNhaThuoc.BLL
{
    public class ThuocBLL
    {
        ThuocDAO thuocDAO = new ThuocDAO();

        public DataTable LayDanhSachThuoc()
        {
            return thuocDAO.GetDanhSachThuoc();
        }

        public DataTable TimKiemThuoc(string tuKhoa)
        {
            return thuocDAO.TimKiemThuoc(tuKhoa);
        }

        public bool ThemThuoc(string maThuoc, string tenThuoc, string maDVT, string maDangDC,
                              string thanhPhan, decimal donGiaNhap, int slHienCo, DateTime ngaySX,
                              string maNuocSX, DateTime hanSD, string chongChiDinh)
        {
            if (string.IsNullOrEmpty(maThuoc) || string.IsNullOrEmpty(tenThuoc))
            {
                throw new Exception("Mã thuốc và tên thuốc không được để trống!");
            }
            return thuocDAO.ThemThuoc(maThuoc, tenThuoc, maDVT, maDangDC, thanhPhan, donGiaNhap, slHienCo, ngaySX, maNuocSX, hanSD, chongChiDinh);
        }
    }
}