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
    /// [10/11/2026 lần 5] Giao diện thiết kế bằng Designer (frmBieuDo.Designer.cs): Chart có sẵn ChartArea / Legend "Default".
    public partial class frmBieuDo : Form
    {
        private frmBieuDo(string tieuDe)
        {
            InitializeComponent();
            this.Text = tieuDe;
            chart.Titles[0].Text = tieuDe;
            btnLuuAnh.Tag = btnLuuAnh.BackColor;
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

        private void btnLuuAnh_Click(object sender, EventArgs e)
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
