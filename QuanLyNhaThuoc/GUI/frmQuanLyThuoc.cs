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
    /// [10/11/2026] Danh mục thuốc (quản lý): thêm / sửa / xóa thuốc, mã vạch, công dụng.
    /// - Giá bán tự tính = 110% giá nhập (yêu cầu 3).
    /// - SL hiện có KHÔNG sửa tay: thuốc mới = 0, tồn kho chỉ thay đổi qua Nhập hàng / Bán hàng (yêu cầu 1).
    /// - Thuốc đã có trong hóa đơn / phiếu nhập không xóa được (giữ lịch sử).
    /// - Lưới: thuốc hết hạn tô đỏ, sắp hết hàng (< 30) tô vàng.
    /// </summary>
    public class frmQuanLyThuoc : Form
    {
        private readonly ThuocBLL thuocBLL = new ThuocBLL();
        private readonly DanhMucBLL dmBLL = new DanhMucBLL();

        private DataGridView dgv;
        private TextBox txtTimKiem, txtMaThuoc, txtTenThuoc, txtMaVach, txtThanhPhan, txtGiaBan, txtSLHienCo, txtChongChiDinh, txtCongDung;
        private ComboBox cboDVT, cboDangDC, cboNuocSX;
        private NumericUpDown nudGiaNhap;
        private DateTimePicker dtpNgaySX, dtpHanSD;
        private Label lblCheDo;
        private Button btnXoa;

        private bool dangThemMoi;
        private List<string> congDungDangChon = new List<string>();
        private Dictionary<string, string> tenCongDung = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, string> TEN_COT = new Dictionary<string, string>
        {
            { "MaThuoc", "Mã thuốc" }, { "TenThuoc", "Tên thuốc" }, { "MaVach", "Mã vạch" }, { "TenDVT", "ĐVT" },
            { "TenDangDC", "Dạng ĐC" }, { "ThanhPhan", "Thành phần" }, { "DonGiaNhap", "Giá nhập" },
            { "GiaBan", "Giá bán" }, { "SLHienCo", "SL hiện có" }, { "NgaySX", "Ngày SX" }, { "HanSD", "Hạn SD" },
            { "TenNuocSX", "Nước SX" }, { "ChongChiDinh", "Chống chỉ định" }, { "CongDung", "Công dụng" }
        };
        private static readonly string[] COT_AN = { "MaDVT", "MaDangDC", "MaNuocSX" };

        public frmQuanLyThuoc()
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Text = "Danh mục Thuốc";
            InitializeComponentCustom();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            NapCombo();
            TaiDanhSach(null);
        }

        // ================================================================
        // GIAO DIỆN
        // ================================================================
        private void InitializeComponentCustom()
        {
            this.SuspendLayout();
            this.BackColor = GiaoDien.MauNen;
            this.Dock = DockStyle.Fill;

            Panel pnlTren = new Panel() { Dock = DockStyle.Top, Height = 56, BackColor = Color.White };
            Label lblTim = new Label() { Text = "Tìm (mã, tên, mã vạch, thành phần):", Left = 15, Top = 18, AutoSize = true, Font = GiaoDien.TaoFont(9.5F) };
            txtTimKiem = new TextBox() { Left = 245, Top = 15, Width = 240, Font = GiaoDien.TaoFont(10F) };
            txtTimKiem.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TaiDanhSach(null); } };
            Button btnTim = GiaoDien.TaoNut("Tìm", GiaoDien.MauXanh, 70, 30);
            btnTim.Left = 495; btnTim.Top = 13;
            btnTim.Click += (s, e) => TaiDanhSach(null);
            Label lblChuThich = new Label()
            {
                Text = "■ Đỏ: đã hết hạn    ■ Vàng: sắp hết hàng (< " + BaoCaoBLL.NGUONG_SAP_HET_HANG + ")",
                Left = 585,
                Top = 19,
                AutoSize = true,
                Font = GiaoDien.TaoFont(8.5F, FontStyle.Italic),
                ForeColor = Color.DimGray
            };
            pnlTren.Controls.AddRange(new Control[] { lblTim, txtTimKiem, btnTim, lblChuThich });

            dgv = GiaoDien.TaoLuoi();
            dgv.DataBindingComplete += (s, e) =>
                GiaoDien.DinhDangCot(dgv, TEN_COT, COT_AN, new[] { "DonGiaNhap", "GiaBan" }, new[] { "NgaySX", "HanSD" });
            dgv.CellFormatting += Dgv_CellFormatting;
            dgv.SelectionChanged += (s, e) => { if (!dangThemMoi) HienThiDongDangChon(); };
            Panel pnlLuoi = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 10, 0) };
            pnlLuoi.Controls.Add(dgv);

            Panel pnlPhai = new Panel() { Dock = DockStyle.Right, Width = 400, BackColor = Color.White, AutoScroll = true };
            TaoKhungNhapLieu(pnlPhai);

            Panel pnlGiua = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 0) };
            pnlGiua.Controls.Add(pnlLuoi);
            pnlGiua.Controls.Add(pnlPhai);

            this.Controls.Add(pnlGiua);
            this.Controls.Add(pnlTren);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void TaoKhungNhapLieu(Panel p)
        {
            int x1 = 15, x2 = 125, rong = 245, y = 12;
            p.Controls.Add(new Label() { Text = "Thông tin thuốc", Left = x1, Top = y, AutoSize = true, Font = GiaoDien.TaoFont(12F, FontStyle.Bold) });
            lblCheDo = new Label() { Left = x1, Top = y + 26, Width = 355, Height = 20, Font = GiaoDien.TaoFont(9F, FontStyle.Italic), ForeColor = GiaoDien.MauXam };
            p.Controls.Add(lblCheDo);
            y += 54;

            p.Controls.Add(GiaoDien.TaoNhan("Mã thuốc (*):", x1, y, 108));
            txtMaThuoc = GiaoDien.TaoO(x2, y, rong, true); txtMaThuoc.MaxLength = 10; txtMaThuoc.CharacterCasing = CharacterCasing.Upper;
            p.Controls.Add(txtMaThuoc); y += 34;

            p.Controls.Add(GiaoDien.TaoNhan("Tên thuốc (*):", x1, y, 108));
            txtTenThuoc = GiaoDien.TaoO(x2, y, rong); txtTenThuoc.MaxLength = 100;
            p.Controls.Add(txtTenThuoc); y += 34;

            p.Controls.Add(GiaoDien.TaoNhan("Mã vạch:", x1, y, 108));
            txtMaVach = GiaoDien.TaoO(x2, y, rong); txtMaVach.MaxLength = 50;
            p.Controls.Add(txtMaVach); y += 34;

            p.Controls.Add(GiaoDien.TaoNhan("Đơn vị tính:", x1, y, 108));
            cboDVT = TaoCombo(x2, y, rong); p.Controls.Add(cboDVT); y += 34;

            p.Controls.Add(GiaoDien.TaoNhan("Dạng điều chế:", x1, y, 108));
            cboDangDC = TaoCombo(x2, y, rong); p.Controls.Add(cboDangDC); y += 34;

            p.Controls.Add(GiaoDien.TaoNhan("Thành phần:", x1, y, 108));
            txtThanhPhan = GiaoDien.TaoO(x2, y, rong); txtThanhPhan.MaxLength = 255;
            txtThanhPhan.Multiline = true; txtThanhPhan.Height = 44; txtThanhPhan.ScrollBars = ScrollBars.Vertical;
            p.Controls.Add(txtThanhPhan); y += 52;

            p.Controls.Add(GiaoDien.TaoNhan("Giá nhập:", x1, y, 108));
            nudGiaNhap = new NumericUpDown() { Left = x2, Top = y, Width = 130, Maximum = 1000000000, Minimum = 0, ThousandsSeparator = true, Font = GiaoDien.TaoFont(10F), Increment = 1000 };
            nudGiaNhap.ValueChanged += (s, e) => txtGiaBan.Text = (nudGiaNhap.Value * 1.1m).ToString("N0") + " đ";
            p.Controls.Add(nudGiaNhap); y += 34;

            p.Controls.Add(GiaoDien.TaoNhan("Giá bán (110%):", x1, y, 108));
            txtGiaBan = GiaoDien.TaoO(x2, y, 130, true);
            p.Controls.Add(txtGiaBan); y += 34;

            p.Controls.Add(GiaoDien.TaoNhan("SL hiện có:", x1, y, 108));
            txtSLHienCo = GiaoDien.TaoO(x2, y, 80, true);
            p.Controls.Add(txtSLHienCo);
            p.Controls.Add(new Label() { Text = "(tăng qua Nhập hàng)", Left = x2 + 85, Top = y + 4, AutoSize = true, Font = GiaoDien.TaoFont(8F, FontStyle.Italic), ForeColor = Color.Gray });
            y += 34;

            p.Controls.Add(GiaoDien.TaoNhan("Ngày SX:", x1, y, 108));
            dtpNgaySX = TaoNgay(x2, y); p.Controls.Add(dtpNgaySX); y += 34;

            p.Controls.Add(GiaoDien.TaoNhan("Hạn sử dụng:", x1, y, 108));
            dtpHanSD = TaoNgay(x2, y); p.Controls.Add(dtpHanSD); y += 34;

            p.Controls.Add(GiaoDien.TaoNhan("Nước SX:", x1, y, 108));
            cboNuocSX = TaoCombo(x2, y, rong); p.Controls.Add(cboNuocSX); y += 34;

            p.Controls.Add(GiaoDien.TaoNhan("Chống chỉ định:", x1, y, 108));
            txtChongChiDinh = GiaoDien.TaoO(x2, y, rong); txtChongChiDinh.MaxLength = 255;
            txtChongChiDinh.Multiline = true; txtChongChiDinh.Height = 44; txtChongChiDinh.ScrollBars = ScrollBars.Vertical;
            p.Controls.Add(txtChongChiDinh); y += 52;

            p.Controls.Add(GiaoDien.TaoNhan("Công dụng:", x1, y, 108));
            txtCongDung = GiaoDien.TaoO(x2, y, rong, true);
            txtCongDung.Multiline = true; txtCongDung.Height = 44; txtCongDung.ScrollBars = ScrollBars.Vertical;
            p.Controls.Add(txtCongDung); y += 48;
            Button btnChonCD = GiaoDien.TaoNut("✎ Chọn công dụng...", GiaoDien.MauXanh, 160, 28);
            btnChonCD.Left = x2; btnChonCD.Top = y;
            btnChonCD.Font = GiaoDien.TaoFont(8.5F, FontStyle.Bold);
            btnChonCD.Click += (s, e) => ChonCongDung();
            p.Controls.Add(btnChonCD); y += 42;

            Button btnThem = GiaoDien.TaoNut("➕ Thêm mới", GiaoDien.MauXanh, 170, 38);
            btnThem.Left = x1; btnThem.Top = y;
            btnThem.Click += (s, e) => BatDauThemMoi();
            Button btnLuu = GiaoDien.TaoNut("💾 Lưu", GiaoDien.MauXanhLa, 170, 38);
            btnLuu.Left = x1 + 185; btnLuu.Top = y;
            btnLuu.Click += (s, e) => Luu();
            y += 46;
            btnXoa = GiaoDien.TaoNut("🗑 Xóa", GiaoDien.MauDo, 170, 38);
            btnXoa.Left = x1; btnXoa.Top = y;
            btnXoa.Click += (s, e) => Xoa();
            Button btnLamMoi = GiaoDien.TaoNut("↻ Làm mới", GiaoDien.MauXam, 170, 38);
            btnLamMoi.Left = x1 + 185; btnLamMoi.Top = y;
            btnLamMoi.Click += (s, e) => { txtTimKiem.Clear(); NapCombo(); TaiDanhSach(null); };
            p.Controls.AddRange(new Control[] { btnThem, btnLuu, btnXoa, btnLamMoi });
        }

        private static ComboBox TaoCombo(int x, int y, int rong)
        {
            return new ComboBox() { Left = x, Top = y, Width = rong, DropDownStyle = ComboBoxStyle.DropDownList, Font = GiaoDien.TaoFont(10F) };
        }

        private static DateTimePicker TaoNgay(int x, int y)
        {
            return new DateTimePicker()
            {
                Left = x,
                Top = y,
                Width = 150,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                ShowCheckBox = true,
                Font = GiaoDien.TaoFont(10F)
            };
        }

        // ================================================================
        // DỮ LIỆU
        // ================================================================
        private void NapCombo()
        {
            try
            {
                GanCombo(cboDVT, dmBLL.LayDanhSachChon(DanhSachDanhMuc.DonViTinh()));
                GanCombo(cboDangDC, dmBLL.LayDanhSachChon(DanhSachDanhMuc.DangDieuChe()));
                GanCombo(cboNuocSX, dmBLL.LayDanhSachChon(DanhSachDanhMuc.NuocSX()));
                NapTenCongDung();
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải danh mục"); }
        }

        private static void GanCombo(ComboBox cbo, DataTable dt)
        {
            cbo.DisplayMember = "Ten";
            cbo.ValueMember = "Ma";
            cbo.DataSource = dt;
            cbo.SelectedIndex = -1;
        }

        private void NapTenCongDung()
        {
            tenCongDung.Clear();
            foreach (DataRow r in thuocBLL.LayTatCaCongDung().Rows)
                tenCongDung[r["MaCongDung"].ToString().Trim()] = r["TenCongDung"].ToString();
        }

        private void TaiDanhSach(string maChon)
        {
            try
            {
                if (maChon == null && dgv.CurrentRow != null) maChon = GiaoDien.GiaTri(dgv.CurrentRow, "MaThuoc");
                dangThemMoi = false;
                dgv.DataSource = thuocBLL.LayDanhSachQuanLy(txtTimKiem.Text);
                GiaoDien.ChonDong(dgv, "MaThuoc", maChon);
                HienThiDongDangChon();
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải danh sách thuốc"); }
        }

        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow r = dgv.Rows[e.RowIndex];
            DateTime hsd;
            int sl;
            bool hetHan = DateTime.TryParse(GiaoDien.GiaTri(r, "HanSD"), out hsd) && hsd.Date < DateTime.Today;
            bool sapHet = int.TryParse(GiaoDien.GiaTri(r, "SLHienCo"), out sl) && sl < BaoCaoBLL.NGUONG_SAP_HET_HANG;
            if (hetHan)
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 199, 206);
                e.CellStyle.ForeColor = Color.FromArgb(156, 0, 6);
            }
            else if (sapHet)
            {
                e.CellStyle.BackColor = GiaoDien.VangNen;
                e.CellStyle.ForeColor = GiaoDien.VangChu;
            }
        }

        private void HienThiDongDangChon()
        {
            DataGridViewRow r = dgv.CurrentRow;
            if (r == null)
            {
                XoaO();
                lblCheDo.Text = "Chưa có thuốc – bấm \"Thêm mới\".";
                GiaoDien.BatTatNut(btnXoa, false);
                return;
            }

            txtMaThuoc.Text = GiaoDien.GiaTri(r, "MaThuoc").Trim();
            txtTenThuoc.Text = GiaoDien.GiaTri(r, "TenThuoc");
            txtMaVach.Text = GiaoDien.GiaTri(r, "MaVach");
            GiaoDien.ChonCombo(cboDVT, GiaoDien.GiaTri(r, "MaDVT"));
            GiaoDien.ChonCombo(cboDangDC, GiaoDien.GiaTri(r, "MaDangDC"));
            GiaoDien.ChonCombo(cboNuocSX, GiaoDien.GiaTri(r, "MaNuocSX"));
            txtThanhPhan.Text = GiaoDien.GiaTri(r, "ThanhPhan");
            decimal gia;
            nudGiaNhap.Value = decimal.TryParse(GiaoDien.GiaTri(r, "DonGiaNhap"), out gia) ? Math.Min(Math.Max(gia, 0), nudGiaNhap.Maximum) : 0;
            txtGiaBan.Text = (decimal.TryParse(GiaoDien.GiaTri(r, "GiaBan"), out gia) ? gia : nudGiaNhap.Value * 1.1m).ToString("N0") + " đ";
            txtSLHienCo.Text = GiaoDien.GiaTri(r, "SLHienCo");
            DatNgay(dtpNgaySX, GiaoDien.GiaTri(r, "NgaySX"));
            DatNgay(dtpHanSD, GiaoDien.GiaTri(r, "HanSD"));
            txtChongChiDinh.Text = GiaoDien.GiaTri(r, "ChongChiDinh");
            txtCongDung.Text = GiaoDien.GiaTri(r, "CongDung");
            try { congDungDangChon = thuocBLL.LayMaCongDungCuaThuoc(txtMaThuoc.Text); }
            catch { congDungDangChon = new List<string>(); }

            GiaoDien.DatChiDoc(txtMaThuoc, true);
            lblCheDo.Text = "Đang sửa: " + txtMaThuoc.Text;
            GiaoDien.BatTatNut(btnXoa, true);
        }

        private static void DatNgay(DateTimePicker dtp, string giaTri)
        {
            DateTime d;
            if (DateTime.TryParse(giaTri, out d)) { dtp.Value = d; dtp.Checked = true; }
            else { dtp.Value = DateTime.Today; dtp.Checked = false; }
        }

        private void XoaO()
        {
            txtMaThuoc.Clear(); txtTenThuoc.Clear(); txtMaVach.Clear(); txtThanhPhan.Clear();
            txtChongChiDinh.Clear(); txtCongDung.Clear(); txtSLHienCo.Text = "0";
            cboDVT.SelectedIndex = -1; cboDangDC.SelectedIndex = -1; cboNuocSX.SelectedIndex = -1;
            nudGiaNhap.Value = 0;
            txtGiaBan.Text = "0 đ";
            DatNgay(dtpNgaySX, null);
            DatNgay(dtpHanSD, null);
            congDungDangChon = new List<string>();
        }

        private void BatDauThemMoi()
        {
            dangThemMoi = true;
            XoaO();
            GiaoDien.DatChiDoc(txtMaThuoc, false);
            try { txtMaThuoc.Text = thuocBLL.SinhMaThuocMoi(); } catch { /* không sinh được thì tự nhập */ }
            lblCheDo.Text = "Đang THÊM MỚI thuốc – điền thông tin rồi bấm Lưu";
            GiaoDien.BatTatNut(btnXoa, false);
            txtTenThuoc.Focus();
        }

        private void ChonCongDung()
        {
            string ten = (txtMaThuoc.Text + " - " + txtTenThuoc.Text).Trim(' ', '-');
            using (frmChonCongDung f = new frmChonCongDung(ten, congDungDangChon))
            {
                if (f.ShowDialog(this.TopLevelControl ?? this) != DialogResult.OK) return;
                congDungDangChon = f.DaChon;
            }
            try { NapTenCongDung(); } catch { }
            txtCongDung.Text = string.Join(", ", congDungDangChon.Select(m => tenCongDung.ContainsKey(m) ? tenCongDung[m] : m));
            lblCheDo.Text = (dangThemMoi ? "Đang THÊM MỚI" : "Đang sửa: " + txtMaThuoc.Text) + " – công dụng sẽ được lưu khi bấm Lưu";
        }

        private ThuocDTO DocForm()
        {
            ThuocDTO t = new ThuocDTO
            {
                MaThuoc = txtMaThuoc.Text.Trim(),
                TenThuoc = txtTenThuoc.Text.Trim(),
                MaVach = txtMaVach.Text.Trim(),
                MaDVT = GiaoDien.GiaTriCombo(cboDVT),
                MaDangDC = GiaoDien.GiaTriCombo(cboDangDC),
                ThanhPhan = txtThanhPhan.Text.Trim(),
                DonGiaNhap = nudGiaNhap.Value,
                NgaySX = dtpNgaySX.Checked ? dtpNgaySX.Value.Date : (DateTime?)null,
                HanSD = dtpHanSD.Checked ? dtpHanSD.Value.Date : (DateTime?)null,
                MaNuocSX = GiaoDien.GiaTriCombo(cboNuocSX),
                ChongChiDinh = txtChongChiDinh.Text.Trim()
            };
            t.CongDung.AddRange(congDungDangChon);
            return t;
        }

        private void Luu()
        {
            if (!dangThemMoi && dgv.CurrentRow == null)
            {
                MessageBox.Show(this, "Chọn 1 thuốc để sửa hoặc bấm \"Thêm mới\".", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ThuocDTO t = DocForm();
            try
            {
                if (dangThemMoi)
                {
                    thuocBLL.Them(t);
                    MessageBox.Show(this, "Đã thêm thuốc " + t.MaThuoc + " (tồn kho = 0).\nDùng trang Nhập hàng để nhập số lượng.",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    thuocBLL.Sua(t);
                    MessageBox.Show(this, "Đã lưu thuốc " + t.MaThuoc + ".", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                TaiDanhSach(t.MaThuoc);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi lưu thuốc"); }
        }

        private void Xoa()
        {
            if (dangThemMoi || dgv.CurrentRow == null) return;
            string ma = GiaoDien.GiaTri(dgv.CurrentRow, "MaThuoc").Trim();
            string ten = GiaoDien.GiaTri(dgv.CurrentRow, "TenThuoc");
            if (MessageBox.Show(this, "Xóa thuốc " + ma + " - " + ten + "?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            try
            {
                if (thuocBLL.Xoa(ma)) TaiDanhSach(null);
                else MessageBox.Show(this, "Thuốc " + ma + " đã có trong hóa đơn bán / phiếu nhập / lịch sử hủy nên không thể xóa (cần giữ lịch sử).",
                                     "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi xóa thuốc"); }
        }
    }
}
