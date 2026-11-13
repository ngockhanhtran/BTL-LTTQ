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
    /// [10/11/2026 lần 5] Giao diện thiết kế bằng Designer (frmChonCongDung.Designer.cs).
    public partial class frmChonCongDung : Form
    {
        private readonly ThuocBLL thuocBLL = new ThuocBLL();
        private readonly HashSet<string> chonBanDau;

        private DataTable dtCongDung;
        private readonly HashSet<string> dangChon = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private bool dangNap;

        /// <summary>Danh sách MaCongDung đã chọn (có giá trị sau khi bấm Đồng ý).</summary>
        public List<string> DaChon { get; private set; }

        public frmChonCongDung(string tenThuoc, IEnumerable<string> maDaChon)
        {
            InitializeComponent();
            foreach (Button b in new[] { btnThemMoi, btnOK }) b.Tag = b.BackColor;
            lblThuoc.Text = tenThuoc ?? "";

            chonBanDau = new HashSet<string>(maDaChon ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);
            foreach (string m in chonBanDau) dangChon.Add(m);
            NapDanhSach();
        }

        private void txtLoc_TextChanged(object sender, EventArgs e) { HienThi(); }

        private void clbCongDung_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (dangNap) return;
            string ma = ((MucCongDung)clbCongDung.Items[e.Index]).Ma;
            if (e.NewValue == CheckState.Checked) dangChon.Add(ma); else dangChon.Remove(ma);
            BeginInvoke((Action)CapNhatDem);
        }

        private void btnThemMoi_Click(object sender, EventArgs e) { ThemMoi(); }

        private void txtMoi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; ThemMoi(); }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DaChon = dangChon.ToList();
            DialogResult = DialogResult.OK;
            Close();
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
            clbCongDung.BeginUpdate();
            try
            {
                clbCongDung.Items.Clear();
                foreach (DataRow r in dtCongDung.Rows)
                {
                    MucCongDung m = new MucCongDung { Ma = r["MaCongDung"].ToString().Trim(), Ten = r["TenCongDung"].ToString() };
                    if (loc != "" && m.Ten.IndexOf(loc, StringComparison.CurrentCultureIgnoreCase) < 0
                                  && m.Ma.IndexOf(loc, StringComparison.OrdinalIgnoreCase) < 0) continue;
                    int i = clbCongDung.Items.Add(m);
                    if (dangChon.Contains(m.Ma)) clbCongDung.SetItemChecked(i, true);
                }
            }
            finally
            {
                clbCongDung.EndUpdate();
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
