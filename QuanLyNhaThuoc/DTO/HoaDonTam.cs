using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaThuoc.DTO
{
    /// <summary>Một dòng thuốc trong hóa đơn đang lập.</summary>
    public class DongHoaDon
    {
        public string MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal ThanhTien => SoLuong * GiaBan;
    }

    // Hóa đơn đang lập (giỏ hàng) - chỉ nằm trong bộ nhớ,
    // được lưu xuống CSDL khi bấm "Đã thanh toán".
    public class HoaDonTam
    {
        private decimal giamGiaPhanTram;

        public HoaDonTam(string maHDB)
        {
            MaHDB = maHDB;
        }

        public string MaHDB { get; private set; }
        public List<DongHoaDon> Dong { get; } = new List<DongHoaDon>();

        //Giảm giá theo % trên toàn hóa đơn (0 - 100)
        public decimal GiamGiaPhanTram
        {
            get { return giamGiaPhanTram; }
            set { giamGiaPhanTram = Math.Max(0, Math.Min(100, value)); }
        }

        public decimal TongTienHang => Dong.Sum(d => d.ThanhTien);
        public decimal TienGiam => Math.Round(TongTienHang * GiamGiaPhanTram / 100m, 0);
        public decimal TongThanhToan => TongTienHang - TienGiam;

        public decimal ThanhTienSauGiam(DongHoaDon d) => Math.Round(d.ThanhTien * (100m - GiamGiaPhanTram) / 100m, 0);

        public int SoLuongCua(string maThuoc) => Dong.Where(d => d.MaThuoc == maThuoc).Sum(d => d.SoLuong);

        //Thêm thuốc; nếu thuốc đã có trong hóa đơn thì cộng dồn số lượng
        public void ThemThuoc(string maThuoc, string tenThuoc, int soLuong, decimal giaBan)
        {
            DongHoaDon dong = Dong.FirstOrDefault(d => d.MaThuoc == maThuoc);
            if (dong != null)
                dong.SoLuong += soLuong;
            else
                Dong.Add(new DongHoaDon { MaThuoc = maThuoc, TenThuoc = tenThuoc, SoLuong = soLuong, GiaBan = giaBan });
        }

        public void XoaThuoc(string maThuoc)
        {
            Dong.RemoveAll(d => d.MaThuoc == maThuoc);
        }
    }
}