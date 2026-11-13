using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [11/11/2026 lần 7] In tem mã vạch / QR (quản lý + thủ kho).
    /// - Thuốc chưa có mã vạch: "Cấp mã vạch" -> mã EAN-13 nội bộ đầu số 200 (BLL/MaVachBLL), lưu vào Thuoc.MaVach.
    /// - In: khổ A4, 3 cột x 8 hàng = 24 tem / trang, xem trước rồi in (hoặc in ra PDF).
    /// - Tem dán lên hộp -> quét được ở trang Bán hàng (máy quét hoặc webcam) -> khép kín quy trình.
    /// Vẽ mã bằng ZXing.Net (GUI/MaVachHelper).
    /// </summary>
    public partial class frmInTem : Form
    {
        private const int SO_COT = 3, SO_HANG = 8;

        private readonly ThuocBLL thuocBLL = new ThuocBLL();
        private readonly MaVachBLL maVachBLL = new MaVachBLL();
        private readonly HashSet<string> daChon = new HashSet<string>();
        private DataTable dsThuoc;
        private bool dangNap;

        // dữ liệu khi in
        private List<TemThuoc> temCanIn;
        private Dictionary<string, Bitmap> anhMa;
        private int temDaIn;

        private sealed class TemThuoc
        {
            public string MaThuoc, TenThuoc, MaVach;
            public decimal GiaBan;
        }

        public frmInTem()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            foreach (Button b in new[] { btnCapMa, btnChonTatCa, btnIn }) b.Tag = b.BackColor;
        }

        private async void frmInTem_Load(object sender, EventArgs e)
        {
            await TaiDanhSachAsync();
        }

        private async Task TaiDanhSachAsync()
        {
            string tk = txtTim.Text.Trim();
            try
            {
                using (KhungDangTai.Hien(dgvThuoc, "Đang tải danh sách thuốc…"))
                    dsThuoc = await Task.Run(() => thuocBLL.TimKiemThuoc(tk));
                HienThi();
            }
            catch (Exception ex)
            {
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải danh sách thuốc");
            }
        }

        private void HienThi()
        {
            if (dsThuoc == null) return;
            dangNap = true;
            dgvThuoc.Rows.Clear();
            foreach (DataRow r in dsThuoc.Rows)
            {
                string ma = Convert.ToString(r["MaThuoc"]).Trim();
                string maVach = r.Table.Columns.Contains("MaVach") && r["MaVach"] != DBNull.Value ? r["MaVach"].ToString().Trim() : "";
                if (chkChuaCoMa.Checked && maVach != "") continue;
                decimal gia = r["GiaBan"] == DBNull.Value ? 0 : Convert.ToDecimal(r["GiaBan"]);
                int i = dgvThuoc.Rows.Add(daChon.Contains(ma), ma, Convert.ToString(r["TenThuoc"]),
                                          maVach == "" ? "(chưa có)" : maVach,
                                          maVach == "" ? "" : MaVachBLL.LoaiMaVach(maVach), gia.ToString("N0"));
                if (maVach == "") dgvThuoc.Rows[i].Cells[colMaVach.Index].Style.ForeColor = Color.Firebrick;
            }
            dangNap = false;
            CapNhatTong();
            VeXemTruoc();
        }

        private void CapNhatTong()
        {
            int coMa = 0, chuaCo = 0;
            foreach (DataGridViewRow r in dgvThuoc.Rows)
            {
                if (!Convert.ToBoolean(r.Cells[colIn.Index].Value ?? false)) continue;
                if (LayMaVach(r) == "") chuaCo++; else coMa++;
            }
            lblTongTem.Text = "Đã chọn " + (coMa + chuaCo) + " thuốc → " + (coMa * (int)nudSoTem.Value) + " tem"
                            + (chuaCo > 0 ? "  (" + chuaCo + " thuốc chưa có mã vạch)" : "");
            GiaoDien.BatTatNut(btnIn, coMa > 0);
            GiaoDien.BatTatNut(btnCapMa, chuaCo > 0);
        }

        private string LayMaVach(DataGridViewRow r)
        {
            string s = Convert.ToString(r.Cells[colMaVach.Index].Value);
            return s == "(chưa có)" ? "" : s;
        }

        // ================== SỰ KIỆN ==================

        private void dgvThuoc_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvThuoc.IsCurrentCellDirty && dgvThuoc.CurrentCell is DataGridViewCheckBoxCell)
                dgvThuoc.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvThuoc_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dangNap || e.RowIndex < 0 || e.ColumnIndex != colIn.Index) return;
            DataGridViewRow r = dgvThuoc.Rows[e.RowIndex];
            string ma = Convert.ToString(r.Cells[colMaThuoc.Index].Value);
            if (Convert.ToBoolean(r.Cells[colIn.Index].Value ?? false)) daChon.Add(ma); else daChon.Remove(ma);
            CapNhatTong();
        }

        private void dgvThuoc_SelectionChanged(object sender, EventArgs e) { VeXemTruoc(); }
        private void rdoLoai_CheckedChanged(object sender, EventArgs e) { VeXemTruoc(); }
        private void nudSoTem_ValueChanged(object sender, EventArgs e) { CapNhatTong(); }
        private void chkChuaCoMa_CheckedChanged(object sender, EventArgs e) { HienThi(); }

        private void txtTim_TextChanged(object sender, EventArgs e) { tmrTim.Stop(); tmrTim.Start(); }

        private async void tmrTim_Tick(object sender, EventArgs e)
        {
            tmrTim.Stop();
            await TaiDanhSachAsync();
        }

        private void btnChonTatCa_Click(object sender, EventArgs e)
        {
            bool chonHet = dgvThuoc.Rows.Cast<DataGridViewRow>().Any(r => !Convert.ToBoolean(r.Cells[colIn.Index].Value ?? false));
            dangNap = true;
            foreach (DataGridViewRow r in dgvThuoc.Rows)
            {
                r.Cells[colIn.Index].Value = chonHet;
                string ma = Convert.ToString(r.Cells[colMaThuoc.Index].Value);
                if (chonHet) daChon.Add(ma); else daChon.Remove(ma);
            }
            dangNap = false;
            CapNhatTong();
        }

        private async void btnCapMa_Click(object sender, EventArgs e)
        {
            List<string> canCap = dgvThuoc.Rows.Cast<DataGridViewRow>()
                .Where(r => Convert.ToBoolean(r.Cells[colIn.Index].Value ?? false) && LayMaVach(r) == "")
                .Select(r => Convert.ToString(r.Cells[colMaThuoc.Index].Value)).ToList();
            if (canCap.Count == 0) return;
            if (MessageBox.Show(this, "Cấp mã vạch nội bộ (EAN-13, đầu số 200) cho " + canCap.Count + " thuốc chưa có mã vạch?",
                                "Cấp mã vạch", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                Dictionary<string, string> kq = await Task.Run(() => maVachBLL.CapMaChoThuocChuaCo(canCap));
                MessageBox.Show(this, "Đã cấp " + kq.Count + " mã vạch:\n" + string.Join("\n", kq.Select(x => x.Key + " → " + x.Value)),
                                "Cấp mã vạch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await TaiDanhSachAsync();
            }
            catch (Exception ex)
            {
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi cấp mã vạch");
            }
        }

        // ================== VẼ TEM ==================

        private void VeXemTruoc()
        {
            DataGridViewRow r = dgvThuoc.CurrentRow;
            Image cu = picXemTruoc.Image;
            picXemTruoc.Image = null;
            if (cu != null) cu.Dispose();
            if (r == null || r.Index < 0) return;

            string maVach = LayMaVach(r);
            if (maVach == "") { lblXemTruoc.Text = "Thuốc chưa có mã vạch"; return; }
            lblXemTruoc.Text = "Xem trước 1 tem (" + (rdoQR.Checked ? "QR" : MaVachBLL.LoaiMaVach(maVach)) + ")";

            TemThuoc t = new TemThuoc
            {
                MaThuoc = Convert.ToString(r.Cells[colMaThuoc.Index].Value),
                TenThuoc = Convert.ToString(r.Cells[colTenThuoc.Index].Value),
                MaVach = maVach,
                GiaBan = ParseTien(Convert.ToString(r.Cells[colGiaBan.Index].Value))
            };
            try
            {
                Bitmap bmp = new Bitmap(300, 160);
                using (Graphics g = Graphics.FromImage(bmp))
                using (Bitmap ma = TaoAnhMa(t.MaVach))
                {
                    g.Clear(Color.White);
                    VeTem(g, new RectangleF(2, 2, 295, 155), t, ma);
                }
                picXemTruoc.Image = bmp;
            }
            catch (Exception ex)
            {
                lblXemTruoc.Text = "Không vẽ được mã: " + ex.Message;
            }
        }

        private static decimal ParseTien(string s)
        {
            decimal d;
            return decimal.TryParse((s ?? "").Replace(".", "").Replace(",", ""), out d) ? d : 0;
        }

        private Bitmap TaoAnhMa(string maVach)
        {
            return rdoQR.Checked ? MaVachHelper.VeQR(maVach, 240) : MaVachHelper.VeMaVach(maVach, 480, 140);
        }

        /// <summary>Vẽ 1 tem trong khung o (đơn vị bất kỳ: điểm ảnh khi xem trước, 1/100 inch khi in).</summary>
        private void VeTem(Graphics g, RectangleF o, TemThuoc t, Bitmap anhMaVach)
        {
            g.InterpolationMode = InterpolationMode.NearestNeighbor;     // vạch sắc nét, máy quét đọc được
            g.PixelOffsetMode = PixelOffsetMode.Half;
            using (Pen vien = new Pen(Color.Silver) { DashStyle = DashStyle.Dash })
                g.DrawRectangle(vien, o.X, o.Y, o.Width, o.Height);

            using (Font fTen = new Font("Segoe UI", 8F, FontStyle.Bold))
            using (Font fMa = new Font("Consolas", 7.5F))
            using (Font fGia = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (StringFormat giua = new StringFormat { Alignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap })
            using (StringFormat trai = new StringFormat { Trimming = StringTrimming.EllipsisWord })
            {
                float pad = o.Width * 0.03f;
                string gia = t.GiaBan.ToString("N0") + " đ";
                if (rdoQR.Checked)
                {
                    float canh = Math.Min(o.Height - 2 * pad, o.Width * 0.42f);
                    g.DrawImage(anhMaVach, o.X + pad, o.Y + (o.Height - canh) / 2, canh, canh);
                    RectangleF chu = new RectangleF(o.X + 2 * pad + canh, o.Y + pad, o.Width - canh - 3 * pad, o.Height - 2 * pad);
                    g.DrawString(t.TenThuoc, fTen, Brushes.Black, new RectangleF(chu.X, chu.Y, chu.Width, chu.Height * 0.55f), trai);
                    g.DrawString(t.MaVach, fMa, Brushes.DimGray, chu.X, chu.Y + chu.Height * 0.58f);
                    g.DrawString(gia, fGia, Brushes.Black, chu.X, chu.Y + chu.Height * 0.76f);
                }
                else
                {
                    float y = o.Y + pad;
                    g.DrawString(t.TenThuoc, fTen, Brushes.Black, new RectangleF(o.X + pad, y, o.Width - 2 * pad, o.Height * 0.15f), giua);
                    float caoMa = o.Height * 0.48f;
                    g.DrawImage(anhMaVach, o.X + pad * 2, o.Y + o.Height * 0.18f, o.Width - pad * 4, caoMa);
                    g.DrawString(t.MaVach, fMa, Brushes.Black, new RectangleF(o.X, o.Y + o.Height * 0.67f, o.Width, o.Height * 0.14f), giua);
                    g.DrawString(gia, fGia, Brushes.Black, new RectangleF(o.X, o.Y + o.Height * 0.80f, o.Width, o.Height * 0.18f), giua);
                }
            }
        }

        // ================== IN ==================

        private void btnIn_Click(object sender, EventArgs e)
        {
            temCanIn = new List<TemThuoc>();
            foreach (DataGridViewRow r in dgvThuoc.Rows)
            {
                if (!Convert.ToBoolean(r.Cells[colIn.Index].Value ?? false)) continue;
                string maVach = LayMaVach(r);
                if (maVach == "") continue;
                TemThuoc t = new TemThuoc
                {
                    MaThuoc = Convert.ToString(r.Cells[colMaThuoc.Index].Value),
                    TenThuoc = Convert.ToString(r.Cells[colTenThuoc.Index].Value),
                    MaVach = maVach,
                    GiaBan = ParseTien(Convert.ToString(r.Cells[colGiaBan.Index].Value))
                };
                for (int i = 0; i < (int)nudSoTem.Value; i++) temCanIn.Add(t);
            }
            if (temCanIn.Count == 0) return;

            anhMa = new Dictionary<string, Bitmap>();
            try
            {
                foreach (TemThuoc t in temCanIn)
                    if (!anhMa.ContainsKey(t.MaVach)) anhMa[t.MaVach] = TaoAnhMa(t.MaVach);

                using (PrintDocument pd = new PrintDocument())
                using (PrintPreviewDialog dlg = new PrintPreviewDialog())
                {
                    pd.DocumentName = "Tem ma vach";
                    pd.DefaultPageSettings.Margins = new Margins(25, 25, 30, 30);
                    foreach (PaperSize kho in pd.PrinterSettings.PaperSizes)
                        if (kho.Kind == PaperKind.A4) { pd.DefaultPageSettings.PaperSize = kho; break; }
                    pd.BeginPrint += (s, ev) => temDaIn = 0;
                    pd.PrintPage += Pd_PrintPage;

                    dlg.Document = pd;
                    dlg.Text = "Xem trước tem mã vạch (" + temCanIn.Count + " tem)";
                    dlg.WindowState = FormWindowState.Maximized;
                    dlg.ShowDialog(this.TopLevelControl ?? this);
                }
                NhatKyBLL.Ghi(NhatKyBLL.IN_TEM, null, "Xem trước / in " + temCanIn.Count + " tem (" + (rdoQR.Checked ? "QR" : "mã vạch") + ")");
            }
            catch (Exception ex)
            {
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi in tem");
            }
            finally
            {
                foreach (Bitmap b in anhMa.Values) b.Dispose();
                anhMa = null;
            }
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle vung = e.MarginBounds;
            float rong = vung.Width / (float)SO_COT, cao = vung.Height / (float)SO_HANG;
            int moiTrang = SO_COT * SO_HANG;

            for (int k = 0; k < moiTrang && temDaIn < temCanIn.Count; k++, temDaIn++)
            {
                TemThuoc t = temCanIn[temDaIn];
                RectangleF o = new RectangleF(vung.X + (k % SO_COT) * rong, vung.Y + (k / SO_COT) * cao, rong, cao);
                VeTem(e.Graphics, RectangleF.Inflate(o, -3, -3), t, anhMa[t.MaVach]);
            }
            e.HasMorePages = temDaIn < temCanIn.Count;
        }
    }
}
