using QuanLyNhaThuoc.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaThuoc.BLL
{
    public class BaoCaoBLL
    {
        // Ngưỡng mặc định
        public const int NGUONG_SAP_HET_HANG = 30;   // SL hiện có < 30 => sắp hết hàng
        public const int SO_NGAY_SAP_HET_HAN = 30;   // còn <= 30 ngày => sắp hết hạn

        // Trạng thái hạn sử dụng (khớp với @TrangThai của sp_BaoCaoHanSuDung)
        public const int HSD_TAT_CA = -1;
        public const int HSD_CON_HAN = 1;
        public const int HSD_HET_HAN = 0;

        // Mức cảnh báo (cột MucCanhBao trả về từ proc)
        public const int MUC_BINH_THUONG = 0;
        public const int MUC_SAP_HET = 1;   // vàng
        public const int MUC_DA_HET = 2;    // đỏ

        // Loại cảnh báo (khớp với @Loai của sp_DanhSachCanhBao)
        public const int CB_TAT_CA = 0;
        public const int CB_HET_HAN = 1;
        public const int CB_CAN_HAN = 2;
        public const int CB_HET_HANG = 3;
        public const int CB_GAN_HET_HANG = 4;

        private BaoCaoDAO baoCaoDAO = new BaoCaoDAO();

        public DataTable BaoCaoHanSuDung(int trangThai, int soNgayCanhBao = SO_NGAY_SAP_HET_HAN)
        {
            if (trangThai != HSD_TAT_CA && trangThai != HSD_CON_HAN && trangThai != HSD_HET_HAN)
                throw new ArgumentException("Trạng thái hạn sử dụng không hợp lệ.");
            if (soNgayCanhBao < 0)
                throw new ArgumentException("Số ngày cảnh báo không được âm.");
            return baoCaoDAO.BaoCaoHanSuDung(trangThai, soNgayCanhBao);
        }

        public DataSet BaoCaoNhapHangTheoThang(int thang, int nam)
        {
            KiemTraThang(thang);
            KiemTraNam(nam);
            return baoCaoDAO.BaoCaoNhapHangTheoThang(thang, nam);
        }

        public DataTable BaoCaoTop5HDBItNhatTheoNam(int nam)
        {
            KiemTraNam(nam);
            return baoCaoDAO.BaoCaoTop5HDBItNhatTheoNam(nam);
        }

        public DataTable BaoCaoTop2NVBanItNhatTheoQuy(int quy, int nam, bool chiTinhNVCoBan = true)
        {
            if (quy < 1 || quy > 4)
                throw new ArgumentException("Quý phải nằm trong khoảng 1 - 4.");
            KiemTraNam(nam);
            return baoCaoDAO.BaoCaoTop2NVBanItNhatTheoQuy(quy, nam, chiTinhNVCoBan);
        }

        public DataTable CanhBaoThuocSapHet(int nguongSL = NGUONG_SAP_HET_HANG)
        {
            if (nguongSL <= 0)
                throw new ArgumentException("Ngưỡng số lượng phải lớn hơn 0.");
            return baoCaoDAO.CanhBaoThuocSapHet(nguongSL);
        }

        public DataTable DanhSachCanhBao(int loai = CB_TAT_CA,
                                         int soNgayCanhBao = SO_NGAY_SAP_HET_HAN,
                                         int nguongSL = NGUONG_SAP_HET_HANG)
        {
            if (loai < CB_TAT_CA || loai > CB_GAN_HET_HANG)
                throw new ArgumentException("Loại cảnh báo không hợp lệ.");
            return baoCaoDAO.DanhSachCanhBao(loai, soNgayCanhBao, nguongSL);
        }

        // Hủy toàn bộ thuốc đã hết hạn còn tồn kho. maNV: người thực hiện (có thể null)
        public DataTable HuyThuocHetHan(string maNV = null, string lyDo = "Hết hạn sử dụng")
        {
            if (maNV != null && maNV.Trim().Length == 0) maNV = null;
            return baoCaoDAO.HuyThuocHetHan(maNV, lyDo);
        }

        // ================== [10/11/2026] DASHBOARD / BIỂU ĐỒ ==================

        public DataTable DashboardTongQuan()
        {
            return baoCaoDAO.DashboardTongQuan(NGUONG_SAP_HET_HANG, SO_NGAY_SAP_HET_HAN);
        }

        public DataTable DoanhThuTheoThang(int nam)
        {
            KiemTraNam(nam);
            return baoCaoDAO.DoanhThuTheoThang(nam);
        }

        public DataTable TopThuocBanChay(int nam, int soLuong = 5)
        {
            KiemTraNam(nam);
            if (soLuong <= 0) soLuong = 5;
            return baoCaoDAO.TopThuocBanChay(nam, soLuong);
        }

        public DataTable DoanhThuNhanVienTheoQuy(int quy, int nam, bool chiTinhNVCoBan = true)
        {
            if (quy < 1 || quy > 4)
                throw new ArgumentException("Quý phải nằm trong khoảng 1 - 4.");
            KiemTraNam(nam);
            return baoCaoDAO.DoanhThuNhanVienTheoQuy(quy, nam, chiTinhNVCoBan);
        }

        public DataTable HoaDonBanTheoNam(int nam)
        {
            KiemTraNam(nam);
            return baoCaoDAO.HoaDonBanTheoNam(nam);
        }

        // Đếm số dòng có cờ = 1 (các cột LaHetHan, LaCanHan, LaHetHang, LaGanHetHang)
        public static int DemCo(DataTable dt, string cot)
        {
            if (dt == null || !dt.Columns.Contains(cot)) return 0;
            int dem = 0;
            foreach (DataRow r in dt.Rows)
            {
                if (r[cot] != DBNull.Value && Convert.ToInt32(r[cot]) == 1) dem++;
            }
            return dem;
        }

        // Đếm số dòng theo mức cảnh báo (dùng cho dòng tổng kết / banner)
        public static int DemTheoMuc(DataTable dt, int muc)
        {
            if (dt == null || !dt.Columns.Contains("MucCanhBao")) return 0;
            int dem = 0;
            foreach (DataRow r in dt.Rows)
            {
                if (r["MucCanhBao"] != DBNull.Value && Convert.ToInt32(r["MucCanhBao"]) == muc) dem++;
            }
            return dem;
        }

        private static void KiemTraThang(int thang)
        {
            if (thang < 1 || thang > 12)
                throw new ArgumentException("Tháng phải nằm trong khoảng 1 - 12.");
        }

        private static void KiemTraNam(int nam)
        {
            if (nam < 2000 || nam > 2100)
                throw new ArgumentException("Năm không hợp lệ.");
        }
    }
}