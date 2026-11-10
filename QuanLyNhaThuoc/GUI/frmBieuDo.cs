using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] Cửa sổ biểu đồ so sánh mở từ trang Báo cáo (nút "📊 Biểu đồ so sánh"):
    /// - Báo cáo Y/C 8: 5 hóa đơn thấp nhất (đỏ) so với trung bình và hóa đơn cao nhất của năm.
    /// - Báo cáo Y/C 9: doanh thu tất cả nhân viên trong quý, 2 NV thấp nhất tô đỏ, có đường trung bình.
    /// Có nút lưu biểu đồ ra ảnh PNG.
    /// </summary>
    public class frmBieuDo : Form
    {
        private readonly Chart chart;

        private frmBieuDo(string tieuDe)
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Text = tieuDe;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(900, 560);
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.BackColor = Color.White;

            chart = BieuDoHelper.TaoChart(tieuDe);

            Panel pnlDuoi = new Panel() { Dock = DockStyle.Bottom, Height = 50, BackColor = Color.FromArgb(244, 246, 249) };
            Button btnLuuAnh = GiaoDien.TaoNut("💾 Lưu ảnh PNG", GiaoDien.MauXanhLa, 150, 34);
            btnLuuAnh.Left = 12; btnLuuAnh.Top = 8;
            btnLuuAnh.Click += (s, e) => LuuAnh();
            Button btnDong = GiaoDien.TaoNut("Đóng", GiaoDien.MauXam, 100, 34);
            btnDong.Top = 8;
            btnDong.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDong.Left = pnlDuoi.Width - 112;
            btnDong.DialogResult = DialogResult.Cancel;
            pnlDuoi.Controls.Add(btnLuuAnh);
            pnlDuoi.Controls.Add(btnDong);
            pnlDuoi.Resize += (s, e) => btnDong.Left = pnlDuoi.ClientSize.Width - 112;

            this.Controls.Add(chart);
            this.Controls.Add(pnlDuoi);
            this.CancelButton = btnDong;
        }

        /// <summary>Biểu đồ so sánh cho báo cáo Y/C 9 (Top 2 NV bán ít nhất theo quý).</summary>
        public static void MoBieuDoNhanVien(IWin32Window owner, int quy, int nam, bool chiTinhNVCoBan)
        {
            using (frmBieuDo f = new frmBieuDo("So sánh doanh thu nhân viên – Quý " + quy + "/" + nam))
            {
                BieuDoHelper.VeDoanhThuNhanVien(f.chart, new BaoCaoBLL().DoanhThuNhanVienTheoQuy(quy, nam, chiTinhNVCoBan));
                f.ShowDialog(owner);
            }
        }

        /// <summary>Biểu đồ so sánh cho báo cáo Y/C 8 (Top 5 hóa đơn bán ít nhất theo năm).</summary>
        public static void MoBieuDoHoaDon(IWin32Window owner, int nam)
        {
            using (frmBieuDo f = new frmBieuDo("5 hóa đơn có tổng tiền thấp nhất so với phần còn lại – năm " + nam))
            {
                BieuDoHelper.VeSoSanhHoaDon(f.chart, new BaoCaoBLL().HoaDonBanTheoNam(nam));
                f.ShowDialog(owner);
            }
        }

        private void LuuAnh()
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Ảnh PNG (*.png)|*.png", FileName = "BieuDo_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".png" })
            {
                if (sfd.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    chart.SaveImage(sfd.FileName, ChartImageFormat.Png);
                    MessageBox.Show(this, "Đã lưu biểu đồ.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(this, "Lỗi lưu ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
    }
}
