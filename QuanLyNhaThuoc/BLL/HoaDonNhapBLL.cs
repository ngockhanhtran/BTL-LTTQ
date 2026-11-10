using QuanLyNhaThuoc.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QuanLyNhaThuoc.BLL
{
    public class HoaDonNhapBLL
    {
        private HoaDonNhapDAO hdnDAO = new HoaDonNhapDAO();
        private ThuocDAO thuocDAO = new ThuocDAO();     // [10/11/2026] kiểm tra mã vạch / công dụng khi import

        // ===================== CÁC HÀM CŨ - GIỮ NGUYÊN =====================
        public bool ThemHoaDonNhap(string maHDN, DateTime ngayNhap, string maNV, string maNCC, decimal tongTien)
        {
            return hdnDAO.ThemHoaDonNhap(maHDN, ngayNhap, maNV, maNCC, tongTien);
        }

        public bool ThemChiTietHDN(string maHDN, string maThuoc, int slNhap, decimal donGia, decimal khuyenMai, decimal thanhTien)
        {
            return hdnDAO.ThemChiTietHDN(maHDN, maThuoc, slNhap, donGia, khuyenMai, thanhTien);
        }

        public DataTable TimKiemHoaDonNhap(string tuKhoa)
        {
            return hdnDAO.TimKiemHoaDonNhap(tuKhoa);
        }

        // ===================== MỚI: TRA CỨU TÊN TỰ ĐỘNG =====================
        public string LayTenNhaCungCap(string maNCC) { return hdnDAO.LayTenNhaCungCap(maNCC); }
        public string LayTenNhanVien(string maNV) { return hdnDAO.LayTenNhanVien(maNV); }
        public string LayTenThuoc(string maThuoc) { return hdnDAO.LayTenThuoc(maThuoc); }

        // ===================== MỚI: NHẬP THỦ CÔNG =====================
        // Lỗi nhập liệu -> ArgumentException (form hiển thị dạng cảnh báo)
        // [10/11/2026 lần 5] thêm thông tin LÔ: số lô (tùy chọn), ngày SX (tùy chọn), hạn SD (bắt buộc)
        public bool NhapHangThuCong(string maHDN, string maNV, string maNCC, string maThuoc,
                                    int slNhap, decimal donGia, string soLo, DateTime? ngaySX, DateTime hanSD,
                                    out bool taoPhieuMoi)
        {
            maHDN = (maHDN ?? "").Trim();
            maNV = (maNV ?? "").Trim();
            maNCC = (maNCC ?? "").Trim();
            maThuoc = (maThuoc ?? "").Trim();

            if (maHDN == "" || maNV == "" || maNCC == "" || maThuoc == "")
                throw new ArgumentException("Vui lòng nhập đủ Mã HĐ Nhập, Mã NV, Mã NCC và Mã Thuốc.");
            if (maHDN.Length > 10)
                throw new ArgumentException("Mã HĐ Nhập tối đa 10 ký tự.");
            if (slNhap <= 0)
                throw new ArgumentException("Số lượng nhập phải là số nguyên lớn hơn 0.");
            if (donGia < 0)
                throw new ArgumentException("Đơn giá không được âm.");
            soLo = (soLo ?? "").Trim();
            if (soLo.Length > 30)
                throw new ArgumentException("Số lô tối đa 30 ký tự.");
            if (hanSD.Date <= DateTime.Today)
                throw new ArgumentException("Hạn sử dụng của lô phải sau hôm nay (không nhập lô đã / sắp hết hạn trong ngày).");
            if (ngaySX.HasValue && ngaySX.Value.Date >= hanSD.Date)
                throw new ArgumentException("Hạn sử dụng phải sau ngày sản xuất.");
            if (ngaySX.HasValue && ngaySX.Value.Date > DateTime.Today)
                throw new ArgumentException("Ngày sản xuất không được sau hôm nay.");

            if (hdnDAO.LayTenNhanVien(maNV) == null)
                throw new ArgumentException("Mã nhân viên '" + maNV + "' không tồn tại trong CSDL.");

            if (hdnDAO.LayTenNhaCungCap(maNCC) == null)
                throw new ArgumentException("Nhà cung cấp '" + maNCC + "' chưa có trong CSDL.\n" +
                                            "Nhập thủ công chỉ dùng được NCC đã có (có thể thêm NCC mới qua Import Excel).");

            if (hdnDAO.LayTenThuoc(maThuoc) == null)
                throw new ArgumentException("CẢNH BÁO: Thuốc '" + maThuoc + "' chưa từng bán (chưa có trong CSDL).\n\n" +
                                            "Nhập thủ công chỉ cho phép thuốc đã tồn tại.\n" +
                                            "Để nhập thuốc mới, hãy dùng chức năng \"Import Phiếu Nhập (Excel)\".");

            bool kq = hdnDAO.NhapHangThuCong(maHDN, maNV, maNCC, maThuoc, slNhap, donGia, 0,
                                             soLo, ngaySX, hanSD, out taoPhieuMoi);
            NhatKyBLL.Ghi(NhatKyBLL.NHAP_HANG, maHDN, "Nhập " + slNhap + " " + maThuoc + " giá " + donGia.ToString("N0") +
                          (soLo == "" ? "" : ", lô " + soLo) + ", HSD " + hanSD.ToString("dd/MM/yyyy"));
            return kq;
        }

        // ===================== MỚI: EXCEL =====================
        public void TaoFileMauExcel(string duongDan)
        {
            PhieuNhapExcel.TaoFileMau(duongDan);
        }

        public void MoFile(string duongDan)
        {
            PhieuNhapExcel.MoFile(duongDan);
        }

        /// <summary>
        /// Bước 1: đọc file + kiểm tra toàn bộ (định dạng, trùng lặp, đối chiếu CSDL).
        /// Chưa ghi gì vào CSDL. Trả về danh sách lỗi + bảng xem trước.
        /// </summary>
        public KetQuaDocExcel KiemTraFileExcel(string duongDan)
        {
            var kq = new KetQuaDocExcel();
            kq.DanhSach = PhieuNhapExcel.DocFile(duongDan, kq.Loi);

            if (kq.DanhSach.Count == 0)
            {
                if (kq.Loi.Count == 0) kq.Loi.Add("File không có dòng dữ liệu nào.");
                kq.XemTruoc = TaoBangXemTruoc(kq.DanhSach, null);
                return kq;
            }

            var tenThuocCSDL = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var thuocMoi = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var nccMoi = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var cacheDanhMuc = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            bool DanhMucCo(string loai, string ma, Func<string, bool> kiemTra)
            {
                string khoa = loai + "|" + ma;
                bool co;
                if (!cacheDanhMuc.TryGetValue(khoa, out co))
                {
                    co = kiemTra(ma);
                    cacheDanhMuc[khoa] = co;
                }
                return co;
            }

            // 1. Kiểm tra theo từng phiếu
            foreach (var phieu in kq.DanhSach.Where(d => d.MaHDN != "")
                                             .GroupBy(d => d.MaHDN, StringComparer.OrdinalIgnoreCase))
            {
                DongPhieuNhap dau = phieu.First();

                foreach (DongPhieuNhap d in phieu.Skip(1))
                {
                    if (!Giong(d.MaNV, dau.MaNV) || !Giong(d.MaNCC, dau.MaNCC))
                        kq.Loi.Add("Dòng " + d.DongExcel + ": phiếu " + d.MaHDN + " có MaNV/MaNCC khác dòng " + dau.DongExcel +
                                   " (mỗi phiếu chỉ có 1 nhân viên và 1 nhà cung cấp).");
                }

                foreach (var trung in phieu.Where(d => d.MaThuoc != "")
                                           .GroupBy(d => d.MaThuoc, StringComparer.OrdinalIgnoreCase)
                                           .Where(g => g.Count() > 1))
                {
                    kq.Loi.Add("Phiếu " + dau.MaHDN + ": thuốc " + trung.Key + " bị lặp ở các dòng " +
                               string.Join(", ", trung.Select(x => x.DongExcel)) + " - hãy gộp thành 1 dòng.");
                }

                if (hdnDAO.TonTaiHoaDonNhap(dau.MaHDN))
                    kq.Loi.Add("Dòng " + dau.DongExcel + ": phiếu " + dau.MaHDN + " đã tồn tại trong CSDL.");

                if (dau.MaNV != "" && DanhMucCo("NV", dau.MaNV, ma => hdnDAO.LayTenNhanVien(ma) != null) == false)
                    kq.Loi.Add("Dòng " + dau.DongExcel + ": mã nhân viên " + dau.MaNV + " không tồn tại.");

                if (dau.MaNCC != "" && !nccMoi.Contains(dau.MaNCC) &&
                    !DanhMucCo("NCC", dau.MaNCC, ma => hdnDAO.LayTenNhaCungCap(ma) != null))
                {
                    if (string.IsNullOrWhiteSpace(dau.TenNCC))
                        kq.Loi.Add("Dòng " + dau.DongExcel + ": NCC " + dau.MaNCC + " chưa có trong CSDL - hãy điền TenNCC để tự thêm mới.");
                    else
                        nccMoi.Add(dau.MaNCC);
                }
            }

            // 2. Kiểm tra từng thuốc
            foreach (DongPhieuNhap d in kq.DanhSach)
            {
                if (d.MaThuoc == "") continue;

                string ten;
                if (!tenThuocCSDL.TryGetValue(d.MaThuoc, out ten))
                {
                    ten = hdnDAO.LayTenThuoc(d.MaThuoc);
                    tenThuocCSDL[d.MaThuoc] = ten;
                }
                if (ten != null || thuocMoi.Contains(d.MaThuoc)) continue;   // thuốc đã có / đã khai báo ở dòng trước

                string tt = "Dòng " + d.DongExcel + ": ";
                if (string.IsNullOrWhiteSpace(d.TenThuoc))
                {
                    kq.Loi.Add(tt + "thuốc " + d.MaThuoc + " chưa có trong CSDL - thuốc mới cần điền TenThuoc.");
                    continue;
                }
                if (d.MaDVT != "" && !DanhMucCo("DVT", d.MaDVT, hdnDAO.TonTaiDonViTinh))
                    kq.Loi.Add(tt + "MaDVT " + d.MaDVT + " không có trong bảng DonViTinh.");
                if (d.MaDangDC != "" && !DanhMucCo("DC", d.MaDangDC, hdnDAO.TonTaiDangDieuChe))
                    kq.Loi.Add(tt + "MaDangDC " + d.MaDangDC + " không có trong bảng DangDieuChe.");
                if (d.MaNuocSX != "" && !DanhMucCo("NSX", d.MaNuocSX, hdnDAO.TonTaiNuocSX))
                    kq.Loi.Add(tt + "MaNuocSX " + d.MaNuocSX + " không có trong bảng NuocSX.");

                thuocMoi.Add(d.MaThuoc);
            }

            // 3. [10/11/2026] Mã vạch: không trùng giữa các thuốc trong file và với thuốc khác trong CSDL
            foreach (var nhom in kq.DanhSach.Where(d => !string.IsNullOrWhiteSpace(d.MaVach) && d.MaThuoc != "")
                                            .GroupBy(d => d.MaVach.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                List<string> dsThuoc = nhom.Select(d => d.MaThuoc).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                if (dsThuoc.Count > 1)
                {
                    kq.Loi.Add("Mã vạch " + nhom.Key + " bị dùng cho nhiều thuốc khác nhau (" + string.Join(", ", dsThuoc) +
                               ") ở các dòng " + string.Join(", ", nhom.Select(x => x.DongExcel)) + ".");
                    continue;
                }
                string maDangDung = thuocDAO.LayMaThuocTheoMaVach(nhom.Key);
                if (maDangDung != null && !string.Equals(maDangDung, dsThuoc[0], StringComparison.OrdinalIgnoreCase))
                    kq.Loi.Add("Dòng " + nhom.First().DongExcel + ": mã vạch " + nhom.Key + " đã được dùng cho thuốc " + maDangDung + " trong CSDL.");
            }

            // 4. [10/11/2026] Đếm công dụng mới (chưa có trong CSDL, sẽ tự thêm khi import)
            var congDungMoi = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string cd in kq.DanhSach.SelectMany(d => d.DanhSachCongDung).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (thuocDAO.TimMaCongDung(cd) == null) congDungMoi.Add(cd);
            }

            kq.SoPhieu = kq.DanhSach.Select(d => d.MaHDN.ToUpperInvariant()).Distinct().Count();
            kq.SoThuocMoi = thuocMoi.Count;
            kq.SoNCCMoi = nccMoi.Count;
            kq.SoCongDungMoi = congDungMoi.Count;
            kq.TongTien = kq.DanhSach.Sum(d => d.ThanhTien);
            kq.XemTruoc = TaoBangXemTruoc(kq.DanhSach, tenThuocCSDL);
            return kq;
        }

        /// <summary>Bước 2: ghi vào CSDL (chỉ gọi khi KiemTraFileExcel không có lỗi).</summary>
        public ThongKeImport ImportExcel(KetQuaDocExcel kq)
        {
            if (kq == null || kq.DanhSach.Count == 0 || kq.Loi.Count > 0)
                throw new InvalidOperationException("Dữ liệu Excel chưa hợp lệ, không thể import.");
            ThongKeImport tk = hdnDAO.ImportPhieuNhap(kq.DanhSach);
            NhatKyBLL.Ghi(NhatKyBLL.NHAP_HANG, null, "Import Excel: " + tk.SoPhieu + " phiếu, " + kq.DanhSach.Count +
                          " lô, " + tk.SoThuocMoi + " thuốc mới, tổng " + kq.TongTien.ToString("N0") + " đ");
            return tk;
        }

        private static DataTable TaoBangXemTruoc(List<DongPhieuNhap> ds, Dictionary<string, string> tenThuocCSDL)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Dòng Excel", typeof(int));
            dt.Columns.Add("Mã HĐN");
            dt.Columns.Add("Mã NV");
            dt.Columns.Add("Mã NCC");
            dt.Columns.Add("Mã Thuốc");
            dt.Columns.Add("Tên Thuốc");
            dt.Columns.Add("SL Nhập", typeof(int));
            dt.Columns.Add("Đơn Giá", typeof(decimal));
            dt.Columns.Add("Khuyến Mãi", typeof(decimal));
            dt.Columns.Add("Thành Tiền", typeof(decimal));
            dt.Columns.Add("Số Lô");        // [10/11/2026 lần 5]
            dt.Columns.Add("Hạn SD Lô");    // [10/11/2026 lần 5]
            dt.Columns.Add("Mã Vạch");      // [10/11/2026]
            dt.Columns.Add("Công Dụng");    // [10/11/2026]
            dt.Columns.Add("Trạng Thái");

            foreach (DongPhieuNhap d in ds)
            {
                string tenCSDL = null;
                string trangThai = "";
                if (tenThuocCSDL != null && d.MaThuoc != "")
                {
                    tenThuocCSDL.TryGetValue(d.MaThuoc, out tenCSDL);
                    trangThai = tenCSDL != null ? "Đã có - cộng tồn kho" : "THUỐC MỚI - sẽ thêm vào CSDL";
                }
                dt.Rows.Add(d.DongExcel, d.MaHDN, d.MaNV, d.MaNCC, d.MaThuoc, tenCSDL ?? d.TenThuoc,
                            d.SLNhap, d.DonGia, d.KhuyenMai, d.ThanhTien,
                            d.SoLo == "" ? "(tự đặt)" : d.SoLo,
                            d.HanSD.HasValue ? d.HanSD.Value.ToString("dd/MM/yyyy") : "(mặc định)",
                            d.MaVach, string.Join("; ", d.DanhSachCongDung), trangThai);
            }
            return dt;
        }

        private static bool Giong(string a, string b)
        {
            return string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>Kết quả đọc + kiểm tra file Excel (chưa ghi CSDL).</summary>
    public class KetQuaDocExcel
    {
        public List<DongPhieuNhap> DanhSach { get; set; } = new List<DongPhieuNhap>();
        public List<string> Loi { get; } = new List<string>();
        public DataTable XemTruoc { get; set; }
        public int SoPhieu { get; set; }
        public int SoThuocMoi { get; set; }
        public int SoNCCMoi { get; set; }
        public int SoCongDungMoi { get; set; }      // [10/11/2026]
        public decimal TongTien { get; set; }
    }
}