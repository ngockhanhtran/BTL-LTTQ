using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] Form quản lý danh mục DÙNG CHUNG (Khách hàng, Nhà cung cấp, Công dụng,
    /// Đơn vị tính, Dạng điều chế, Nước sản xuất). Ô nhập được tạo tự động theo CauHinhDanhMuc
    /// (DTO/DanhMucCauHinh.cs) -> muốn thêm danh mục mới chỉ cần khai báo cấu hình.
    /// </summary>
    public class frmDanhMuc : Form
    {
        private readonly CauHinhDanhMuc cauHinh;
        private readonly DanhMucBLL bll = new DanhMucBLL();

        private DataGridView dgv;
        private TextBox txtTimKiem;
        private Label lblCheDo;
        private Button btnThem, btnLuu, btnXoa;
        private readonly Dictionary<string, TextBox> oNhap = new Dictionary<string, TextBox>();
        private bool dangThemMoi;

        public frmDanhMuc(CauHinhDanhMuc ch)
        {
            cauHinh = ch;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Text = ch.TieuDe;
            InitializeComponentCustom();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            TaiDanhSach(null);
        }

        private void InitializeComponentCustom()
        {
            this.SuspendLayout();
            this.BackColor = GiaoDien.MauNen;
            this.Dock = DockStyle.Fill;

            // ---------- Tìm kiếm ----------
            Panel pnlTren = new Panel() { Dock = DockStyle.Top, Height = 56, BackColor = Color.White };
            Label lblTim = new Label() { Text = "Tìm kiếm:", Left = 15, Top = 18, AutoSize = true, Font = GiaoDien.TaoFont(9.5F) };
            txtTimKiem = new TextBox() { Left = 90, Top = 15, Width = 280, Font = GiaoDien.TaoFont(10F) };
            txtTimKiem.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TaiDanhSach(null); } };
            Button btnTim = GiaoDien.TaoNut("Tìm", GiaoDien.MauXanh, 70, 30);
            btnTim.Left = 380; btnTim.Top = 13;
            btnTim.Click += (s, e) => TaiDanhSach(null);
            Button btnTatCa = GiaoDien.TaoNut("Tất cả", GiaoDien.MauXam, 80, 30);
            btnTatCa.Left = 458; btnTatCa.Top = 13;
            btnTatCa.Click += (s, e) => { txtTimKiem.Clear(); TaiDanhSach(null); };
            pnlTren.Controls.AddRange(new Control[] { lblTim, txtTimKiem, btnTim, btnTatCa });

            // ---------- Lưới ----------
            dgv = GiaoDien.TaoLuoi();
            Dictionary<string, string> tieuDe = cauHinh.Cot.ToDictionary(c => c.Ten, c => c.TieuDe);
            List<string> cotSo = cauHinh.Cot.Where(c => c.LaSo).Select(c => c.Ten).ToList();
            dgv.DataBindingComplete += (s, e) => GiaoDien.DinhDangCot(dgv, tieuDe, null, cotSo, null);
            dgv.SelectionChanged += (s, e) => { if (!dangThemMoi) HienThiDongDangChon(); };
            Panel pnlLuoi = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 10, 0) };
            pnlLuoi.Controls.Add(dgv);

            // ---------- Ô nhập liệu (tự sinh theo cấu hình) ----------
            Panel pnlPhai = new Panel() { Dock = DockStyle.Right, Width = 380, BackColor = Color.White, AutoScroll = true };
            int y = 15;
            pnlPhai.Controls.Add(new Label() { Text = cauHinh.TieuDe, Left = 18, Top = y, Width = 345, Height = 26, Font = GiaoDien.TaoFont(12F, FontStyle.Bold) });
            y += 28;
            lblCheDo = new Label() { Left = 18, Top = y, Width = 345, Height = 20, Font = GiaoDien.TaoFont(9F, FontStyle.Italic), ForeColor = GiaoDien.MauXam };
            pnlPhai.Controls.Add(lblCheDo);
            y += 32;

            foreach (CotDanhMuc c in cauHinh.Cot)
            {
                pnlPhai.Controls.Add(GiaoDien.TaoNhan(c.TieuDe + (c.BatBuoc ? " (*)" : "") + ":", 18, y, 120));
                bool dai = c.DoDaiToiDa > 100;
                TextBox t = GiaoDien.TaoO(140, y, 220, c.ChiDoc);
                if (dai) { t.Multiline = true; t.Height = 50; t.ScrollBars = ScrollBars.Vertical; }
                if (c.DoDaiToiDa > 0 && !c.ChiDoc) t.MaxLength = c.DoDaiToiDa;
                if (c == cauHinh.CotKhoa) t.CharacterCasing = CharacterCasing.Upper;
                pnlPhai.Controls.Add(t);
                oNhap[c.Ten] = t;
                y += dai ? 62 : 38;
            }

            y += 10;
            btnThem = GiaoDien.TaoNut("➕ Thêm mới", GiaoDien.MauXanh, 165, 38);
            btnThem.Left = 18; btnThem.Top = y;
            btnThem.Click += (s, e) => BatDauThemMoi();
            btnLuu = GiaoDien.TaoNut("💾 Lưu", GiaoDien.MauXanhLa, 165, 38);
            btnLuu.Left = 195; btnLuu.Top = y;
            btnLuu.Click += (s, e) => Luu();
            y += 46;
            btnXoa = GiaoDien.TaoNut("🗑 Xóa", GiaoDien.MauDo, 165, 38);
            btnXoa.Left = 18; btnXoa.Top = y;
            btnXoa.Click += (s, e) => Xoa();
            Button btnHuy = GiaoDien.TaoNut("↻ Làm mới", GiaoDien.MauXam, 165, 38);
            btnHuy.Left = 195; btnHuy.Top = y;
            btnHuy.Click += (s, e) => TaiDanhSach(null);
            pnlPhai.Controls.AddRange(new Control[] { btnThem, btnLuu, btnXoa, btnHuy });

            Panel pnlGiua = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 0) };
            pnlGiua.Controls.Add(pnlLuoi);
            pnlGiua.Controls.Add(pnlPhai);

            this.Controls.Add(pnlGiua);
            this.Controls.Add(pnlTren);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox OKhoa { get { return oNhap[cauHinh.CotKhoa.Ten]; } }

        private void TaiDanhSach(string maChon)
        {
            try
            {
                if (maChon == null && dgv.CurrentRow != null) maChon = GiaoDien.GiaTri(dgv.CurrentRow, cauHinh.CotKhoa.Ten);
                dangThemMoi = false;
                dgv.DataSource = bll.LayDanhSach(cauHinh, txtTimKiem.Text);
                GiaoDien.ChonDong(dgv, cauHinh.CotKhoa.Ten, maChon);
                HienThiDongDangChon();
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải dữ liệu"); }
        }

        private void HienThiDongDangChon()
        {
            DataGridViewRow r = dgv.CurrentRow;
            foreach (CotDanhMuc c in cauHinh.Cot)
            {
                string v = r == null ? "" : GiaoDien.GiaTri(r, c.Ten);
                if (c.LaSo && v != "") { decimal d; if (decimal.TryParse(v, out d)) v = d.ToString("N0"); }
                oNhap[c.Ten].Text = c == cauHinh.CotKhoa ? v.Trim() : v;
            }
            GiaoDien.DatChiDoc(OKhoa, true);
            lblCheDo.Text = r == null ? "Chưa có dữ liệu – bấm \"Thêm mới\"." : "Đang sửa: " + OKhoa.Text;
            GiaoDien.BatTatNut(btnXoa, r != null);
        }

        private void BatDauThemMoi()
        {
            dangThemMoi = true;
            foreach (TextBox t in oNhap.Values) t.Clear();
            GiaoDien.DatChiDoc(OKhoa, false);
            try { OKhoa.Text = bll.SinhMaMoi(cauHinh); } catch { /* không sinh được mã thì để người dùng tự nhập */ }
            lblCheDo.Text = "Đang THÊM MỚI – điền thông tin rồi bấm Lưu";
            GiaoDien.BatTatNut(btnXoa, false);
            oNhap[cauHinh.CotTen.Ten].Focus();
        }

        private void Luu()
        {
            if (!dangThemMoi && dgv.CurrentRow == null)
            {
                MessageBox.Show(this, "Chọn 1 dòng để sửa hoặc bấm \"Thêm mới\".", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Dictionary<string, object> giaTri = new Dictionary<string, object>();
            foreach (CotDanhMuc c in cauHinh.Cot)
                if (!c.ChiDoc) giaTri[c.Ten] = oNhap[c.Ten].Text;
            try
            {
                if (dangThemMoi) bll.Them(cauHinh, giaTri);
                else bll.Sua(cauHinh, giaTri);
                string ma = Convert.ToString(giaTri[cauHinh.CotKhoa.Ten]);
                MessageBox.Show(this, (dangThemMoi ? "Đã thêm " : "Đã lưu ") + ma + ".", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSach(ma);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi lưu dữ liệu"); }
        }

        private void Xoa()
        {
            if (dangThemMoi || dgv.CurrentRow == null) return;
            string ma = GiaoDien.GiaTri(dgv.CurrentRow, cauHinh.CotKhoa.Ten).Trim();
            string ten = GiaoDien.GiaTri(dgv.CurrentRow, cauHinh.CotTen.Ten);
            if (MessageBox.Show(this, "Xóa " + ma + " - " + ten + "?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            try
            {
                if (bll.Xoa(cauHinh, ma))
                {
                    TaiDanhSach(null);
                }
                else
                {
                    MessageBox.Show(this, cauHinh.ThongBaoKhongXoaDuoc ?? "Dữ liệu đang được sử dụng nên không thể xóa.",
                                    "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi xóa"); }
        }
    }
}
