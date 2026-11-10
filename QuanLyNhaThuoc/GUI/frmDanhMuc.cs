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
    /// [10/11/2026 lần 5] Khung giao diện thiết kế bằng Designer (frmDanhMuc.Designer.cs): thanh tìm kiếm, lưới, nút.
    /// Riêng các ô nhập liệu vẫn sinh lúc chạy vào pnlONhap vì mỗi danh mục có số cột khác nhau.
    public partial class frmDanhMuc : Form
    {
        private readonly CauHinhDanhMuc cauHinh;
        private readonly DanhMucBLL bll = new DanhMucBLL();

        private readonly Dictionary<string, TextBox> oNhap = new Dictionary<string, TextBox>();
        private readonly Dictionary<string, string> tieuDeCot;
        private readonly List<string> cotSo;
        private bool dangThemMoi;

        public frmDanhMuc(CauHinhDanhMuc ch)
        {
            cauHinh = ch;
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Text = ch.TieuDe;
            lblTieuDeKhung.Text = ch.TieuDe;
            foreach (Button b in new[] { btnTim, btnTatCa, btnThem, btnLuu, btnXoa, btnLamMoi }) b.Tag = b.BackColor;

            tieuDeCot = cauHinh.Cot.ToDictionary(c => c.Ten, c => c.TieuDe);
            cotSo = cauHinh.Cot.Where(c => c.LaSo).Select(c => c.Ten).ToList();
            TaoONhap();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            TaiDanhSach(null);
        }

        // Sinh nhãn + ô nhập theo cấu hình rồi dời hàng nút xuống dưới
        private void TaoONhap()
        {
            int y = 0;
            foreach (CotDanhMuc c in cauHinh.Cot)
            {
                pnlONhap.Controls.Add(GiaoDien.TaoNhan(c.TieuDe + (c.BatBuoc ? " (*)" : "") + ":", 18, y, 120));
                bool dai = c.DoDaiToiDa > 100;
                TextBox t = GiaoDien.TaoO(140, y, 220, c.ChiDoc);
                if (dai) { t.Multiline = true; t.Height = 50; t.ScrollBars = ScrollBars.Vertical; }
                if (c.DoDaiToiDa > 0 && !c.ChiDoc) t.MaxLength = c.DoDaiToiDa;
                if (c == cauHinh.CotKhoa) t.CharacterCasing = CharacterCasing.Upper;
                pnlONhap.Controls.Add(t);
                oNhap[c.Ten] = t;
                y += dai ? 62 : 38;
            }
            pnlONhap.Height = y;
            pnlNut.Top = pnlONhap.Bottom + 10;
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TaiDanhSach(null); }
        }

        private void btnTim_Click(object sender, EventArgs e) { TaiDanhSach(null); }
        private void btnTatCa_Click(object sender, EventArgs e) { txtTimKiem.Clear(); TaiDanhSach(null); }
        private void btnThem_Click(object sender, EventArgs e) { BatDauThemMoi(); }
        private void btnLuu_Click(object sender, EventArgs e) { Luu(); }
        private void btnXoa_Click(object sender, EventArgs e) { Xoa(); }
        private void btnLamMoi_Click(object sender, EventArgs e) { TaiDanhSach(null); }

        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            GiaoDien.DinhDangCot(dgv, tieuDeCot, null, cotSo, null);
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (!dangThemMoi) HienThiDongDangChon();
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
