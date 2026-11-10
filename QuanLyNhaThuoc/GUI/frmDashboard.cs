using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] Dashboard tổng quan (chỉ quản lý):
    /// - 5 thẻ số liệu: doanh thu hôm nay, doanh thu tháng này, số hóa đơn tháng, tiền nhập tháng, thuốc cần chú ý.
    /// - 4 biểu đồ: doanh thu & tiền nhập theo tháng, top 5 thuốc bán chạy,
    ///   doanh thu NV theo quý (2 NV thấp nhất tô đỏ - so sánh với báo cáo Y/C 9),
    ///   5 hóa đơn thấp nhất so với phần còn lại (so sánh với báo cáo Y/C 8).
    /// </summary>
    /// [10/11/2026 lần 5] Giao diện thiết kế bằng Designer (frmDashboard.Designer.cs): 5 thẻ số liệu + 4 Chart đặt sẵn,
    /// code chỉ đổ số liệu vào.
    public partial class frmDashboard : Form
    {
        private readonly BaoCaoBLL bcBLL = new BaoCaoBLL();
        private bool dangKhoiTao = true;

        public frmDashboard()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            btnLamMoi.Tag = btnLamMoi.BackColor;
            nudNam.Value = DateTime.Today.Year;
            cboQuy.SelectedIndex = (DateTime.Today.Month - 1) / 3;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dangKhoiTao = false;
            TaiDuLieu();
        }

        private void nudNam_ValueChanged(object sender, EventArgs e) { TaiDuLieu(); }
        private void cboQuy_SelectedIndexChanged(object sender, EventArgs e) { TaiDuLieu(); }
        private void btnLamMoi_Click(object sender, EventArgs e) { TaiDuLieu(); }

        private static void DatThe(Label tieuDe, Label giaTri, Label phu, string a, string b, string c)
        {
            tieuDe.Text = a;
            giaTri.Text = b;
            phu.Text = c;
        }

        private static string Tien(object v)
        {
            decimal d = v == null || v == DBNull.Value ? 0 : Convert.ToDecimal(v);
            return d.ToString("N0") + " đ";
        }

        private void TaiDuLieu()
        {
            if (dangKhoiTao) return;
            int nam = (int)nudNam.Value;
            int quy = cboQuy.SelectedIndex + 1;
            try
            {
                Cursor = Cursors.WaitCursor;

                // Thẻ số liệu
                DataTable tq = bcBLL.DashboardTongQuan();
                if (tq.Rows.Count > 0)
                {
                    DataRow r = tq.Rows[0];
                    DatThe(lblTheTieuDe1, lblTheGiaTri1, lblThePhu1, "Doanh thu hôm nay", Tien(r["DoanhThuHomNay"]), r["SoHoaDonHomNay"] + " hóa đơn");
                    DatThe(lblTheTieuDe2, lblTheGiaTri2, lblThePhu2, "Doanh thu tháng " + DateTime.Today.Month, Tien(r["DoanhThuThangNay"]), r["SoHoaDonThangNay"] + " hóa đơn");
                    DatThe(lblTheTieuDe3, lblTheGiaTri3, lblThePhu3, "Tiền nhập tháng " + DateTime.Today.Month, Tien(r["TienNhapThangNay"]), "theo phiếu nhập");
                    DatThe(lblTheTieuDe4, lblTheGiaTri4, lblThePhu4, "Khách hàng", Convert.ToInt32(r["SoKhachHang"]).ToString("N0"), "đang tích điểm");
                    DatThe(lblTheTieuDe5, lblTheGiaTri5, lblThePhu5, "Thuốc cần chú ý", Convert.ToInt32(r["SoThuocCanhBao"]).ToString("N0"), "sắp hết hàng / cận hạn");
                }

                // Biểu đồ
                chartThang.Titles[0].Text = "Doanh thu & tiền nhập theo tháng – năm " + nam;
                BieuDoHelper.VeDoanhThuTheoThang(chartThang, bcBLL.DoanhThuTheoThang(nam));

                chartTopThuoc.Titles[0].Text = "Top 5 thuốc bán chạy – năm " + nam;
                BieuDoHelper.VeTopThuoc(chartTopThuoc, bcBLL.TopThuocBanChay(nam, 5));

                chartNhanVien.Titles[0].Text = "Doanh thu nhân viên – Quý " + quy + "/" + nam;
                BieuDoHelper.VeDoanhThuNhanVien(chartNhanVien, bcBLL.DoanhThuNhanVienTheoQuy(quy, nam, true));

                chartHoaDon.Titles[0].Text = "5 hóa đơn thấp nhất so với phần còn lại – năm " + nam;
                BieuDoHelper.VeSoSanhHoaDon(chartHoaDon, bcBLL.HoaDonBanTheoNam(nam));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Lỗi tải dashboard: " + ex.Message +
                                "\n\nHãy chạy lại file CSDL_QuanLyNhaThuoc.sql để tạo các proc sp_Dashboard_*.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
