using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [11/11/2026 lần 7] Trang "Hiệu năng truy vấn" (chỉ quản lý) - bằng chứng cho "truy xuất dữ liệu nhanh":
    ///   1. Sinh 100.000 hóa đơn giả (mã PF..., năm 2018 - 2022, không đụng tồn kho).
    ///   2. Bấm "Đo trước / sau": xóa index -> đo 3 truy vấn -> tạo lại index -> đo lại -> bảng + biểu đồ.
    /// Mọi thao tác chạy ở luồng nền (async/await): giao diện không đơ, vòng xoay "Đang tải…".
    /// Giao diện thiết kế bằng Designer (frmHieuNang.Designer.cs).
    /// </summary>
    public partial class frmHieuNang : Form
    {
        private readonly HieuNangBLL bll = new HieuNangBLL();
        private bool dangChay;

        public frmHieuNang()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            foreach (Button b in new[] { btnSinh, btnXoa, btnDo }) b.Tag = b.BackColor;
            dgvKetQua.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 235, 250);
            dgvKetQua.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private async void frmHieuNang_Load(object sender, EventArgs e)
        {
            await TaiThongTinAsync();
        }

        private async Task TaiThongTinAsync()
        {
            try
            {
                DataRow r = await Task.Run(() => bll.ThongTin());
                if (r == null) return;
                lblThongTin.Text = string.Format("Hiện có {0:N0} hóa đơn bán ({1:N0} hóa đơn giả PF...), {2:N0} dòng chi tiết.   Index: {3}",
                    r["TongHoaDon"], r["HoaDonThu"], r["TongDongChiTiet"],
                    Convert.ToBoolean(r["CoIndex"]) ? "ĐÃ CÓ" : "chưa có");
                btnXoa.Enabled = Convert.ToInt32(r["HoaDonThu"]) > 0 && !dangChay;
                GiaoDien.BatTatNut(btnXoa, btnXoa.Enabled);
            }
            catch (Exception ex)
            {
                lblThongTin.Text = "Không đọc được số liệu: " + ex.Message + "  (hãy chạy lại file CSDL_QuanLyNhaThuoc.sql)";
            }
        }

        private void BatTat(bool chay)
        {
            dangChay = chay;
            GiaoDien.BatTatNut(btnSinh, !chay);
            GiaoDien.BatTatNut(btnDo, !chay);
            GiaoDien.BatTatNut(btnXoa, !chay);
            nudSoHoaDon.Enabled = !chay;
        }

        private async void btnSinh_Click(object sender, EventArgs e)
        {
            int so = (int)nudSoHoaDon.Value;
            if (MessageBox.Show(this, "Sinh " + so.ToString("N0") + " hóa đơn giả (mã PF..., năm 2018 - 2022)?\n\n" +
                                "Dữ liệu này không trừ tồn kho, có thể xóa lại bằng nút \"Xóa dữ liệu giả\".\n" +
                                "Có thể mất 10 - 60 giây tùy máy.", "Sinh dữ liệu",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            BatTat(true);
            lblTrangThai.Text = "Đang sinh " + so.ToString("N0") + " hóa đơn…  (các trang khác vẫn dùng được)";
            try
            {
                int n;
                using (KhungDangTai.Hien(dgvKetQua, "Đang sinh dữ liệu…"))
                    n = await Task.Run(() => bll.SinhDuLieu(so));
                lblTrangThai.Text = "✓ Đã sinh " + n.ToString("N0") + " hóa đơn giả. Bấm \"Đo trước / sau\".";
            }
            catch (Exception ex)
            {
                lblTrangThai.Text = "";
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi sinh dữ liệu");
            }
            finally
            {
                BatTat(false);
                await TaiThongTinAsync();
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Xóa toàn bộ hóa đơn giả (mã PF...)?", "Xóa dữ liệu giả",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            BatTat(true);
            lblTrangThai.Text = "Đang xóa hóa đơn giả…";
            try
            {
                int n;
                using (KhungDangTai.Hien(dgvKetQua, "Đang xóa dữ liệu giả…"))
                    n = await Task.Run(() => bll.XoaDuLieu());
                lblTrangThai.Text = "✓ Đã xóa " + n.ToString("N0") + " hóa đơn giả.";
            }
            catch (Exception ex)
            {
                lblTrangThai.Text = "";
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi xóa dữ liệu");
            }
            finally
            {
                BatTat(false);
                await TaiThongTinAsync();
            }
        }

        private async void btnDo_Click(object sender, EventArgs e)
        {
            BatTat(true);
            // Progress<T> tạo trên luồng giao diện -> Report từ luồng nền vẫn cập nhật nhãn an toàn
            IProgress<string> tienDo = new Progress<string>(s => lblTrangThai.Text = s);
            try
            {
                List<KetQuaDoTruyVan> kq;
                using (KhungDangTai.Hien(dgvKetQua, "Đang đo…"))
                using (KhungDangTai.Hien(chartKetQua, "Đang đo…"))
                    kq = await Task.Run(() => bll.DoTruocSau(tienDo));
                HienKetQua(kq);
                lblTrangThai.Text = "✓ Xong. " + HieuNangBLL.TomTat(kq);
            }
            catch (Exception ex)
            {
                lblTrangThai.Text = "";
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi đo hiệu năng");
            }
            finally
            {
                BatTat(false);
                await TaiThongTinAsync();
            }
        }

        private void HienKetQua(List<KetQuaDoTruyVan> ds)
        {
            dgvKetQua.Rows.Clear();
            foreach (KetQuaDoTruyVan k in ds)
            {
                int i = dgvKetQua.Rows.Add(k.TenTruyVan, k.SoDong.ToString("N0"), k.MsTruoc.ToString("N2"), k.MsSau.ToString("N2"),
                                           k.HeSo >= 1 ? "x" + k.HeSo.ToString("N1") : "-", k.MoTa);
                DataGridViewRow r = dgvKetQua.Rows[i];
                r.Cells[colTruoc.Index].Style.ForeColor = Color.FromArgb(200, 35, 51);
                r.Cells[colSau.Index].Style.ForeColor = Color.FromArgb(40, 140, 60);
                r.Cells[colNhanh.Index].Style.Font = new Font(dgvKetQua.Font, FontStyle.Bold);
            }

            chartKetQua.Series.Clear();
            Series truoc = new Series("Trước (chưa có index)") { ChartType = SeriesChartType.Bar, Color = Color.FromArgb(220, 53, 69), IsValueShownAsLabel = true, LabelFormat = "0.## ms" };
            Series sau = new Series("Sau (có index)") { ChartType = SeriesChartType.Bar, Color = Color.FromArgb(40, 167, 69), IsValueShownAsLabel = true, LabelFormat = "0.## ms" };
            foreach (KetQuaDoTruyVan k in ds)
            {
                truoc.Points.AddXY(k.TenTruyVan, Math.Round(k.MsTruoc, 2));
                sau.Points.AddXY(k.TenTruyVan, Math.Round(k.MsSau, 2));
            }
            chartKetQua.Series.Add(truoc);
            chartKetQua.Series.Add(sau);
            chartKetQua.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartKetQua.ChartAreas[0].AxisY.Title = "mili giây (càng ngắn càng nhanh)";
        }
    }
}
