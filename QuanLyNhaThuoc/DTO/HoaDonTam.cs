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
    //
    // [10/11/2026] Thêm đổi điểm tích lũy:
    //   Tiền hàng - Giảm giá (%) = TongSauGiamGia
    //   TongSauGiamGia - Tiền đổi điểm = TongThanhToan
    //   PhanBoThanhTien(): chia TongThanhToan về từng dòng (ChiTietHDB.ThanhTien) sao cho
    //   tổng các dòng = đúng TongThanhToan -> trigger TongTien và báo cáo không phải sửa.
    public class HoaDonTam
    {
        private decimal giamGiaPhanTram;
        private int diemSuDung;

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

        /// <summary>[10/11/2026] Số điểm khách dùng để giảm tiền (BLL kiểm tra giới hạn).</summary>
        public int DiemSuDung
        {
            get { return diemSuDung; }
            set { diemSuDung = Math.Max(0, value); }
        }

        public decimal TongTienHang => Dong.Sum(d => d.ThanhTien);
        public decimal TienGiam => Math.Round(TongTienHang * GiamGiaPhanTram / 100m, 0);
        public decimal TongSauGiamGia => TongTienHang - TienGiam;

        /// <summary>[10/11/2026] Tiền giảm nhờ đổi điểm (không vượt quá số tiền còn phải trả).</summary>
        public decimal TienGiamDiem => Math.Min(DiemSuDung * QuyDinhTichDiem.GIA_TRI_MOI_DIEM, TongSauGiamGia);

        public decimal TongThanhToan => TongSauGiamGia - TienGiamDiem;

        // Giữ lại hàm cũ (không còn dùng khi lưu hóa đơn, thay bằng PhanBoThanhTien)
        public decimal ThanhTienSauGiam(DongHoaDon d) => Math.Round(d.ThanhTien * (100m - GiamGiaPhanTram) / 100m, 0);

        /// <summary>
        /// [10/11/2026] Thành tiền thực của từng dòng (cùng thứ tự với Dong), tổng = TongThanhToan.
        /// Chia theo tỷ lệ tiền hàng của từng dòng, phần lẻ do làm tròn cộng vào dòng lớn nhất.
        /// </summary>
        public List<decimal> PhanBoThanhTien()
        {
            List<decimal> kq = new List<decimal>();
            if (Dong.Count == 0) return kq;

            decimal tongGoc = TongTienHang;
            decimal canPhanBo = TongThanhToan;

            // Không có giảm giá / đổi điểm: giữ nguyên thành tiền từng dòng
            if (canPhanBo == tongGoc || tongGoc <= 0)
            {
                foreach (DongHoaDon d in Dong) kq.Add(tongGoc <= 0 ? 0 : d.ThanhTien);
                return kq;
            }

            decimal daPhanBo = 0;
            int viTriLonNhat = 0;
            for (int i = 0; i < Dong.Count; i++)
            {
                decimal v = Math.Floor(Dong[i].ThanhTien * canPhanBo / tongGoc);
                kq.Add(v);
                daPhanBo += v;
                if (Dong[i].ThanhTien > Dong[viTriLonNhat].ThanhTien) viTriLonNhat = i;
            }
            kq[viTriLonNhat] += canPhanBo - daPhanBo;   // phần dư luôn >= 0
            return kq;
        }

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
