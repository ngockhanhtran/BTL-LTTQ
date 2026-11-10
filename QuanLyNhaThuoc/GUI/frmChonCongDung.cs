using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] Hộp thoại chọn công dụng cho 1 thuốc (tích chọn nhiều), có thể thêm công dụng mới.
    /// Bấm "Đồng ý" -> DaChon = danh sách MaCongDung được tích. Form gọi tự lưu (hoặc giữ tạm).
    /// </summary>
    public class frmChonCongDung : Form
    {
        private readonly ThuocBLL thuocBLL = new ThuocBLL();
        private readonly HashSet<string> chonBanDau;

        private CheckedListBox clb;
        private TextBox txtLoc, txtMoi;
        private Label lblDem;
        private DataTable dtCongDung;
        private readonly HashSet<string> dangChon = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private bool dangNap;

        /// <summary>Danh sách MaCongDung đã chọn (có giá trị sau khi bấm Đồng ý).</summary>
        public List<string> DaChon { get; private set; }

        public frmChonCongDung(string tenThuoc, IEnumerable<string> maDaChon)
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.Text = "Chọn công dụng";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(440, 540);
            this.BackColor = Color.White;

            chonBanDau = new HashSet<string>(maDaChon ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);
            foreach (string m in chonBanDau) dangChon.Add(m);

            InitializeComponentCustom(tenThuoc);
            NapDanhSach();
        }

        private void InitializeComponentCustom(string tenThuoc)
        {
            Label lblTieuDe = new Label() { Text = "Công dụng của thuốc", Font = GiaoDien.TaoFont(13F, FontStyle.Bold), Left = 20, Top = 15, AutoSize = true };
            Label lblThuoc = new Label() { Text = tenThuoc ?? "", Font = GiaoDien.TaoFont(9.5F), ForeColor = GiaoDien.MauXanh, Left = 20, Top = 45, Width = 400, Height = 20, AutoEllipsis = true };

            Label lblLoc = new Label() { Text = "Lọc:", Left = 20, Top = 78, AutoSize = true, Font = GiaoDien.TaoFont(9.5F) };
            txtLoc = new TextBox() { Left = 60, Top = 75, Width = 360, Font = GiaoDien.TaoFont(10F) };
            txtLoc.TextChanged += (s, e) => HienThi();

            clb = new CheckedListBox() { Left = 20, Top = 108, Width = 400, Height = 300, CheckOnClick = true, Font = GiaoDien.TaoFont(10F), IntegralHeight = false };
            clb.ItemCheck += (s, e) =>
            {
                if (dangNap) return;
                string ma = ((MucCongDung)clb.Items[e.Index]).Ma;
                if (e.NewValue == CheckState.Checked) dangChon.Add(ma); else dangChon.Remove(ma);
                BeginInvoke((Action)CapNhatDem);
            };

            lblDem = new Label() { Left = 20, Top = 412, Width = 400, Height = 20, Font = GiaoDien.TaoFont(9F, FontStyle.Italic), ForeColor = Color.Gray };

            Label lblMoi = new Label() { Text = "Thêm công dụng mới:", Left = 20, Top = 440, AutoSize = true, Font = GiaoDien.TaoFont(9.5F) };
            txtMoi = new TextBox() { Left = 155, Top = 437, Width = 175, Font = GiaoDien.TaoFont(10F), MaxLength = 100 };
            Button btnThemMoi = GiaoDien.TaoNut("+ Thêm", GiaoDien.MauXanh, 85, 27);
            btnThemMoi.Left = 335; btnThemMoi.Top = 436;
            btnThemMoi.Click += (s, e) => ThemMoi();
            txtMoi.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; ThemMoi(); } };

            Button btnOK = GiaoDien.TaoNut("Đồng ý", GiaoDien.MauXanhLa, 150, 38);
            btnOK.Left = 20; btnOK.Top = 485;
            btnOK.Click += (s, e) =>
            {
                DaChon = dangChon.ToList();
                DialogResult = DialogResult.OK;
                Close();
            };
            Button btnHuy = GiaoDien.TaoNut("Hủy", GiaoDien.MauXam, 110, 38);
            btnHuy.Left = 310; btnHuy.Top = 485;
            btnHuy.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblTieuDe, lblThuoc, lblLoc, txtLoc, clb, lblDem, lblMoi, txtMoi, btnThemMoi, btnOK, btnHuy });
            this.CancelButton = btnHuy;
        }

        private class MucCongDung
        {
            public string Ma;
            public string Ten;
            public override string ToString() { return Ten + "  (" + Ma + ")"; }
        }

        private void NapDanhSach()
        {
            try
            {
                dtCongDung = thuocBLL.LayTatCaCongDung();
                HienThi();
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải công dụng"); }
        }

        private void HienThi()
        {
            if (dtCongDung == null) return;
            string loc = txtLoc.Text.Trim();
            dangNap = true;
            clb.BeginUpdate();
            try
            {
                clb.Items.Clear();
                foreach (DataRow r in dtCongDung.Rows)
                {
                    MucCongDung m = new MucCongDung { Ma = r["MaCongDung"].ToString().Trim(), Ten = r["TenCongDung"].ToString() };
                    if (loc != "" && m.Ten.IndexOf(loc, StringComparison.CurrentCultureIgnoreCase) < 0
                                  && m.Ma.IndexOf(loc, StringComparison.OrdinalIgnoreCase) < 0) continue;
                    int i = clb.Items.Add(m);
                    if (dangChon.Contains(m.Ma)) clb.SetItemChecked(i, true);
                }
            }
            finally
            {
                clb.EndUpdate();
                dangNap = false;
            }
            CapNhatDem();
        }

        private void CapNhatDem()
        {
            lblDem.Text = "Đã chọn " + dangChon.Count + " công dụng" +
                          (dangChon.SetEquals(chonBanDau) ? "" : "  (có thay đổi, bấm Đồng ý để áp dụng)");
        }

        private void ThemMoi()
        {
            try
            {
                string ma = thuocBLL.ThemCongDung(txtMoi.Text);
                dangChon.Add(ma);
                txtMoi.Clear();
                txtLoc.Clear();
                NapDanhSach();
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex); }
        }
    }
}
