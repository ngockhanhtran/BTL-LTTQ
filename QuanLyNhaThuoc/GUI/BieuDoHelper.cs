using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] Vẽ biểu đồ bằng control Chart có sẵn của .NET Framework
    /// (tham chiếu System.Windows.Forms.DataVisualization - không cần cài NuGet).
    /// Dùng chung cho frmDashboard và frmBieuDo (nút "Biểu đồ so sánh" ở trang Báo cáo).
    /// </summary>
    internal static class BieuDoHelper
    {
        public static readonly Color MAU_CHINH = Color.FromArgb(0, 122, 204);
        public static readonly Color MAU_PHU = Color.FromArgb(230, 126, 34);
        public static readonly Color MAU_NHAN_MANH = Color.FromArgb(220, 53, 69);   // phần tử "thấp nhất" (Top 2 NV / Top 5 HĐ)
        public static readonly Color MAU_XANH_LA = Color.FromArgb(40, 167, 69);
        public static readonly Color MAU_XAM = Color.FromArgb(160, 167, 175);

        public static Chart TaoChart(string tieuDe)
        {
            Chart c = new Chart() { Dock = DockStyle.Fill, BackColor = Color.White };
            ChartArea ca = new ChartArea("Default") { BackColor = Color.White };
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.Interval = 1;
            ca.AxisX.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            ca.AxisX.LabelStyle.IsStaggered = false;
            ca.AxisX.IsLabelAutoFit = true;
            ca.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep45 | LabelAutoFitStyles.WordWrap;
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            ca.AxisY.LabelStyle.Format = "#,##0";
            ca.AxisY.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            c.ChartAreas.Add(ca);
            // Tên "Default" khớp giá trị mặc định Series.ChartArea / Series.Legend -> không lỗi "không tìm thấy"
            c.Legends.Add(new Legend("Default") { Docking = Docking.Bottom, Alignment = StringAlignment.Center, Font = new Font("Segoe UI", 8.5F) });
            c.Titles.Add(new Title(tieuDe, Docking.Top, new Font("Segoe UI", 10.5F, FontStyle.Bold), Color.FromArgb(33, 37, 41)));
            return c;
        }

        /// <summary>Xóa series, chú thích tự thêm, đường kẻ và dòng thông báo cũ trước khi vẽ lại.</summary>
        private static void LamSach(Chart c, bool hienChuThich)
        {
            c.Series.Clear();
            c.Legends[0].CustomItems.Clear();
            c.Legends[0].Enabled = hienChuThich;
            c.ChartAreas[0].AxisY.StripLines.Clear();
            while (c.Titles.Count > 1) c.Titles.RemoveAt(c.Titles.Count - 1);
        }

        private static void ThongBaoTrong(Chart c, string thongBao)
        {
            c.Titles.Add(new Title(thongBao, Docking.Top, new Font("Segoe UI", 9F, FontStyle.Italic), Color.Gray));
        }

        /// <summary>Doanh thu (cột) + tiền nhập (đường) 12 tháng. dt: Thang, DoanhThu, SoHoaDon, TienNhap.</summary>
        public static void VeDoanhThuTheoThang(Chart c, DataTable dt)
        {
            LamSach(c, true);

            Series sBan = new Series("Doanh thu bán") { ChartType = SeriesChartType.Column, Color = MAU_CHINH };
            sBan["PointWidth"] = "0.6";
            Series sNhap = new Series("Tiền nhập hàng") { ChartType = SeriesChartType.Line, Color = MAU_PHU, BorderWidth = 3,
                                                         MarkerStyle = MarkerStyle.Circle, MarkerSize = 6 };
            foreach (DataRow r in dt.Rows)
            {
                string thang = "T" + r["Thang"];
                sBan.Points.AddXY(thang, Convert.ToDouble(r["DoanhThu"]));
                sNhap.Points.AddXY(thang, Convert.ToDouble(r["TienNhap"]));
            }
            foreach (DataPoint p in sBan.Points) p.ToolTip = p.AxisLabel + ": #VALY{#,##0} đ";
            foreach (DataPoint p in sNhap.Points) p.ToolTip = p.AxisLabel + " – nhập: #VALY{#,##0} đ";
            c.Series.Add(sBan);
            c.Series.Add(sNhap);
        }

        /// <summary>Top thuốc bán chạy (thanh ngang). dt: MaThuoc, TenThuoc, SLBan, DoanhThu.</summary>
        public static void VeTopThuoc(Chart c, DataTable dt)
        {
            LamSach(c, false);
            Series s = new Series("Số lượng bán") { ChartType = SeriesChartType.Bar, Color = MAU_XANH_LA, IsValueShownAsLabel = true, LabelFormat = "#,##0" };
            s["PointWidth"] = "0.6";
            // Bar vẽ từ dưới lên -> đảo thứ tự để thuốc bán nhiều nhất nằm trên cùng
            foreach (DataRow r in dt.Rows.Cast<DataRow>().Reverse())
            {
                int i = s.Points.AddXY(r["TenThuoc"].ToString(), Convert.ToDouble(r["SLBan"]));
                s.Points[i].ToolTip = r["TenThuoc"] + ": " + Convert.ToDouble(r["SLBan"]).ToString("#,##0") +
                                      " – doanh thu " + Convert.ToDouble(r["DoanhThu"]).ToString("#,##0") + " đ";
            }
            c.Series.Add(s);
            if (dt.Rows.Count == 0) ThongBaoTrong(c, "Chưa có dữ liệu bán hàng trong năm");
        }

        /// <summary>
        /// So sánh doanh thu TẤT CẢ nhân viên trong quý; 2 NV thấp nhất (báo cáo Y/C 9) tô đỏ.
        /// dt: MaNV, TenNV, SoHoaDon, DoanhThu (tăng dần).
        /// </summary>
        public static void VeDoanhThuNhanVien(Chart c, DataTable dt)
        {
            LamSach(c, dt.Rows.Count > 0);
            Series s = new Series("Doanh thu") { ChartType = SeriesChartType.Column, IsValueShownAsLabel = true, LabelFormat = "#,##0" };
            s["PointWidth"] = "0.6";
            double tong = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                double v = Convert.ToDouble(r["DoanhThu"]);
                tong += v;
                int k = s.Points.AddXY(r["TenNV"].ToString(), v);
                s.Points[k].Color = i < 2 ? MAU_NHAN_MANH : MAU_CHINH;
                s.Points[k].ToolTip = r["MaNV"] + " - " + r["TenNV"] + ": " + v.ToString("#,##0") + " đ (" + r["SoHoaDon"] + " hóa đơn)";
            }
            c.Series.Add(s);

            if (dt.Rows.Count > 0)
            {
                // Đường trung bình để dễ so sánh
                double tb = tong / dt.Rows.Count;
                StripLine sl = new StripLine
                {
                    IntervalOffset = tb,
                    StripWidth = 0,
                    BorderColor = Color.DimGray,
                    BorderDashStyle = ChartDashStyle.Dash,
                    BorderWidth = 2,
                    Text = "Trung bình " + tb.ToString("#,##0") + " đ",
                    TextAlignment = StringAlignment.Far,
                    Font = new Font("Segoe UI", 8F)
                };
                c.ChartAreas[0].AxisY.StripLines.Add(sl);
                c.Legends[0].CustomItems.Add(MAU_NHAN_MANH, "2 NV bán ít nhất (báo cáo Y/C 9)");
                c.Legends[0].CustomItems.Add(MAU_CHINH, "Các NV còn lại");
                s.IsVisibleInLegend = false;
            }
            else ThongBaoTrong(c, "Chưa có hóa đơn bán trong quý");
        }

        /// <summary>
        /// So sánh 5 hóa đơn có tổng tiền thấp nhất (báo cáo Y/C 8) với phần còn lại của năm.
        /// dt: MaHDB, NgayBan, TongTien (tăng dần).
        /// </summary>
        public static void VeSoSanhHoaDon(Chart c, DataTable dt)
        {
            LamSach(c, dt.Rows.Count > 0);
            Series s = new Series("Tổng tiền") { ChartType = SeriesChartType.Column, IsValueShownAsLabel = true, LabelFormat = "#,##0" };
            s["PointWidth"] = "0.6";

            int n = dt.Rows.Count;
            int soTop = Math.Min(5, n);
            for (int i = 0; i < soTop; i++)
            {
                DataRow r = dt.Rows[i];
                double v = Convert.ToDouble(r["TongTien"]);
                int k = s.Points.AddXY(r["MaHDB"].ToString().Trim(), v);
                s.Points[k].Color = MAU_NHAN_MANH;
                s.Points[k].ToolTip = r["MaHDB"] + " – " + Convert.ToDateTime(r["NgayBan"]).ToString("dd/MM/yyyy") + ": " + v.ToString("#,##0") + " đ";
            }

            if (n > soTop)
            {
                double[] conLai = dt.Rows.Cast<DataRow>().Skip(soTop).Select(r => Convert.ToDouble(r["TongTien"])).ToArray();
                int k = s.Points.AddXY("TB " + conLai.Length + " HĐ còn lại", conLai.Average());
                s.Points[k].Color = MAU_XAM;
                s.Points[k].ToolTip = "Trung bình " + conLai.Length + " hóa đơn còn lại";
                k = s.Points.AddXY("HĐ cao nhất", conLai.Max());
                s.Points[k].Color = MAU_XANH_LA;
                s.Points[k].ToolTip = "Hóa đơn có tổng tiền cao nhất trong năm";
            }
            c.Series.Add(s);

            if (n > 0)
            {
                c.Legends[0].CustomItems.Add(MAU_NHAN_MANH, "5 HĐ tổng tiền thấp nhất (báo cáo Y/C 8)");
                if (n > soTop)
                {
                    c.Legends[0].CustomItems.Add(MAU_XAM, "Trung bình các HĐ còn lại");
                    c.Legends[0].CustomItems.Add(MAU_XANH_LA, "HĐ cao nhất");
                }
                s.IsVisibleInLegend = false;
            }
            else ThongBaoTrong(c, "Chưa có hóa đơn bán trong năm");
        }
    }
}
