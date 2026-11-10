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
    public class frmDashboard : Form
    {
        private readonly BaoCaoBLL bcBLL = new BaoCaoBLL();

        private NumericUpDown nudNam;
        private ComboBox cboQuy;
        private FlowLayoutPanel pnlThe;
        private Chart chartThang, chartTopThuoc, chartNhanVien, chartHoaDon;
        private bool dangKhoiTao = true;

        public frmDashboard()
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Text = "Dashboard tổng quan";
            InitializeComponentCustom();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dangKhoiTao = false;
            TaiDuLieu();
        }

        private void InitializeComponentCustom()
        {
            this.SuspendLayout();
            this.BackColor = GiaoDien.MauNen;
            this.Dock = DockStyle.Fill;

            // ---------- Bộ lọc ----------
            Panel pnlLoc = new Panel() { Dock = DockStyle.Top, Height = 50, BackColor = Color.White };
            Label lblNam = new Label() { Text = "Năm:", Left = 15, Top = 16, AutoSize = true, Font = GiaoDien.TaoFont(9.5F) };
            nudNam = new NumericUpDown() { Left = 55, Top = 13, Width = 75, Minimum = 2000, Maximum = 2100, Value = DateTime.Today.Year, Font = GiaoDien.TaoFont(10F), TextAlign = HorizontalAlignment.Center };
            nudNam.ValueChanged += (s, e) => TaiDuLieu();
            Label lblQuy = new Label() { Text = "Quý (biểu đồ nhân viên):", Left = 150, Top = 16, AutoSize = true, Font = GiaoDien.TaoFont(9.5F) };
            cboQuy = new ComboBox() { Left = 310, Top = 13, Width = 80, DropDownStyle = ComboBoxStyle.DropDownList, Font = GiaoDien.TaoFont(9.5F) };
            cboQuy.Items.AddRange(new object[] { "Quý 1", "Quý 2", "Quý 3", "Quý 4" });
            cboQuy.SelectedIndex = (DateTime.Today.Month - 1) / 3;
            cboQuy.SelectedIndexChanged += (s, e) => TaiDuLieu();
            Button btnLamMoi = GiaoDien.TaoNut("↻ Làm mới", GiaoDien.MauXanh, 110, 30);
            btnLamMoi.Left = 410; btnLamMoi.Top = 10;
            btnLamMoi.Click += (s, e) => TaiDuLieu();
            pnlLoc.Controls.AddRange(new Control[] { lblNam, nudNam, lblQuy, cboQuy, btnLamMoi });

            // ---------- Thẻ số liệu ----------
            pnlThe = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                Height = 104,
                Padding = new Padding(10, 10, 10, 0),
                WrapContents = false,
                AutoScroll = true
            };

            // ---------- 4 biểu đồ (lưới 2 x 2) ----------
            TableLayoutPanel tbl = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, Padding = new Padding(10) };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            chartThang = BieuDoHelper.TaoChart("Doanh thu & tiền nhập theo tháng");
            chartTopThuoc = BieuDoHelper.TaoChart("Top 5 thuốc bán chạy");
            chartNhanVien = BieuDoHelper.TaoChart("Doanh thu nhân viên theo quý");
            chartHoaDon = BieuDoHelper.TaoChart("5 hóa đơn thấp nhất so với phần còn lại");
            tbl.Controls.Add(BocKhung(chartThang), 0, 0);
            tbl.Controls.Add(BocKhung(chartTopThuoc), 1, 0);
            tbl.Controls.Add(BocKhung(chartNhanVien), 0, 1);
            tbl.Controls.Add(BocKhung(chartHoaDon), 1, 1);

            this.Controls.Add(tbl);
            this.Controls.Add(pnlThe);
            this.Controls.Add(pnlLoc);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private static Panel BocKhung(Control c)
        {
            Panel p = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(6), Margin = new Padding(5) };
            p.Controls.Add(c);
            return p;
        }

        private Panel TaoThe(string tieuDe, string giaTri, string phu, Color mau)
        {
            Panel p = new Panel() { Width = 205, Height = 84, BackColor = Color.White, Margin = new Padding(5, 0, 5, 0) };
            Panel vach = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = mau };
            Label lblTieuDe = new Label() { Text = tieuDe, Left = 15, Top = 8, Width = 185, Height = 18, Font = GiaoDien.TaoFont(9F), ForeColor = Color.DimGray };
            Label lblGiaTri = new Label() { Text = giaTri, Left = 13, Top = 26, Width = 190, Height = 32, Font = GiaoDien.TaoFont(16F, FontStyle.Bold), ForeColor = mau, AutoEllipsis = true };
            Label lblPhu = new Label() { Text = phu, Left = 15, Top = 60, Width = 185, Height = 18, Font = GiaoDien.TaoFont(8.5F, FontStyle.Italic), ForeColor = Color.Gray };
            p.Controls.AddRange(new Control[] { vach, lblTieuDe, lblGiaTri, lblPhu });
            return p;
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
                pnlThe.SuspendLayout();
                Control[] theCu = pnlThe.Controls.Cast<Control>().ToArray();
                pnlThe.Controls.Clear();
                foreach (Control c in theCu) c.Dispose();
                if (tq.Rows.Count > 0)
                {
                    DataRow r = tq.Rows[0];
                    pnlThe.Controls.Add(TaoThe("Doanh thu hôm nay", Tien(r["DoanhThuHomNay"]), r["SoHoaDonHomNay"] + " hóa đơn", GiaoDien.MauXanh));
                    pnlThe.Controls.Add(TaoThe("Doanh thu tháng " + DateTime.Today.Month, Tien(r["DoanhThuThangNay"]), r["SoHoaDonThangNay"] + " hóa đơn", GiaoDien.MauXanhLa));
                    pnlThe.Controls.Add(TaoThe("Tiền nhập tháng " + DateTime.Today.Month, Tien(r["TienNhapThangNay"]), "theo phiếu nhập", GiaoDien.MauCam));
                    pnlThe.Controls.Add(TaoThe("Khách hàng", Convert.ToInt32(r["SoKhachHang"]).ToString("N0"), "đang tích điểm", Color.FromArgb(111, 66, 193)));
                    pnlThe.Controls.Add(TaoThe("Thuốc cần chú ý", Convert.ToInt32(r["SoThuocCanhBao"]).ToString("N0"), "sắp hết hàng / cận hạn", GiaoDien.MauDo));
                }
                pnlThe.ResumeLayout();

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
