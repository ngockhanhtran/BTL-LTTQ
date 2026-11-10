using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using QuanLyNhaThuoc.DAL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.BLL
{
    public class HoaDonBanBLL
    {
        private HoaDonBanDAO hdbDAO = new HoaDonBanDAO();

        // Số thứ tự lớn nhất đã cấp trong phiên chạy chương trình.
        // Cần vì hóa đơn chỉ được lưu DB khi thanh toán xong, nên mã đã cấp
        // (ví dụ HDB014 đang chờ ở trang bán hàng) chưa có trong DB.
        private static int soCuoiDaCap = 0;
        private static readonly object khoa = new object();

        public bool ThemHoaDonBan(string maHDB, DateTime ngayBan, string maNV, string maKhach, decimal tongTien)
        {
            return hdbDAO.ThemHoaDonBan(maHDB, ngayBan, maNV, maKhach, tongTien);
        }

        public bool ThemChiTietHDB(string maHDB, string maThuoc, int soLuong, decimal giamGia, decimal thanhTien)
        {
            return hdbDAO.ThemChiTietHDB(maHDB, maThuoc, soLuong, giamGia, thanhTien);
        }

        /// Sinh mã hóa đơn kế tiếp dựa trên mã lớn nhất trong CSDL.
        /// Giữ nguyên tiền tố và độ dài phần số của mã cũ (HDB009 -> HDB010).
        /// CSDL trống thì bắt đầu từ HDB001.
        public string SinhMaHoaDonMoi()
        {
            List<string> dsMa = hdbDAO.GetDanhSachMaHDB();

            string tienTo = "HDB";
            int doDaiSo = 3;
            int soLonNhat = 0;

            foreach (string ma in dsMa)
            {
                Match m = Regex.Match(ma, @"^(\D*)(\d+)$");
                if (!m.Success) continue;

                int so;
                if (!int.TryParse(m.Groups[2].Value, out so)) continue;
                if (so >= soLonNhat)
                {
                    soLonNhat = so;
                    tienTo = m.Groups[1].Value;
                    doDaiSo = m.Groups[2].Value.Length;
                }
            }

            int soMoi;
            lock (khoa)
            {
                soMoi = Math.Max(soLonNhat, soCuoiDaCap) + 1;
                soCuoiDaCap = soMoi;
            }
            return tienTo + soMoi.ToString().PadLeft(doDaiSo, '0');
        }

        public DataTable LayDanhSachNhanVien()
        {
            return hdbDAO.GetDanhSachNhanVien();
        }

        /// Trả về tên khách hàng, null nếu không tìm thấy
        public string LayTenKhachHang(string maKhach)
        {
            if (string.IsNullOrWhiteSpace(maKhach)) return null;
            return hdbDAO.GetTenKhachHang(maKhach.Trim());
        }

        /// Kiểm tra tồn kho trước khi hiện QR. Trả về thông báo lỗi, hoặc null nếu đủ hàng
        public string KiemTraTonKho(HoaDonTam hd)
        {
            foreach (DongHoaDon d in hd.Dong)
            {
                int? ton = hdbDAO.GetSoLuongTon(d.MaThuoc);
                if (ton == null)
                    return $"Thuốc {d.TenThuoc} không còn trong danh mục!";
                if (ton.Value < d.SoLuong)
                    return $"Thuốc {d.TenThuoc} không đủ tồn kho (Còn: {ton.Value}, cần: {d.SoLuong}).";
            }
            return null;
        }

        /// [10/11/2026] Thông tin khách + điểm + số lần đã mua; null nếu khách chưa có trong CSDL
        public ThongTinKhach LayThongTinKhach(string maKhach)
        {
            if (string.IsNullOrWhiteSpace(maKhach)) return null;
            return hdbDAO.GetThongTinKhach(maKhach.Trim());
        }

        /// tenKhachMoi Tên khách khi là khách MỚI (sẽ được thêm vào bảng KhachHang); null nếu là khách cũ
        /// [10/11/2026] Thêm tích điểm. khachCu = thông tin khách cũ (null nếu khách mới).
        ///   diemCong  : số điểm được cộng sau hóa đơn này
        ///   diemConLai: số điểm của khách sau khi trừ điểm đã dùng và cộng điểm mới
        public void LuuHoaDon(HoaDonTam hd, string maNV, string maKhach, string tenKhachMoi, DateTime ngayBan,
                              ThongTinKhach khachCu, out int diemCong, out int diemConLai)
        {
            if (hd == null || hd.Dong.Count == 0)
                throw new Exception("Hóa đơn chưa có thuốc nào!");
            if (string.IsNullOrWhiteSpace(maNV))
                throw new Exception("Chưa chọn nhân viên!");
            if (string.IsNullOrWhiteSpace(maKhach))
                throw new Exception("Chưa nhập mã khách hàng!");
            if (tenKhachMoi != null && string.IsNullOrWhiteSpace(tenKhachMoi))
                throw new Exception("Khách hàng mới phải có tên!");
            if (maKhach.Trim().Length > 10)
                throw new Exception("Mã khách hàng tối đa 10 ký tự!");
            if (tenKhachMoi != null && tenKhachMoi.Trim().Length > 50)
                throw new Exception("Tên khách hàng tối đa 50 ký tự!");

            // Kiểm tra số điểm đổi
            if (hd.DiemSuDung > 0)
            {
                if (khachCu == null)
                    throw new Exception("Khách hàng mới chưa có điểm để đổi!");
                int toiDa = TichDiemBLL.DiemDoiToiDa(khachCu.DiemTichLuy, hd.TongSauGiamGia);
                if (hd.DiemSuDung > toiDa)
                    throw new Exception($"Chỉ được dùng tối đa {toiDa} điểm cho hóa đơn này!");
            }

            int soLanDaMua = khachCu == null ? 0 : khachCu.SoLanMua;
            diemCong = TichDiemBLL.TinhDiemCong(hd.TongThanhToan, soLanDaMua);

            diemConLai = hdbDAO.LuuHoaDon(hd, maNV.Trim(), maKhach.Trim(),
                                          tenKhachMoi == null ? null : tenKhachMoi.Trim(), ngayBan, diemCong);
        }
    }
}