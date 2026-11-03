using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    public partial class frmThuoc : Form
    {
        private readonly ThuocBLL thuocBLL = new ThuocBLL();
        private readonly HoaDonBanBLL hdbBLL = new HoaDonBanBLL();

        private DataGridView dgvThuoc;
        private TextBox txtTuKhoa;
        private TextBox txtMaThuoc, txtTenThuoc, txtThanhPhan, txtGiaNhap, txtGiaBan, txtSLHienCo, txtCongDung, txtChongChiDinh;
        private Label lblMaHDB, lblGioHang;
        private NumericUpDown nudSoLuong;
        private Button btnTimKiem, btnLamMoi, btnThemVaoHD, btnThanhToanHD;

        // Thuốc đang chọn trên lưới
        private string maThuocChon, tenThuocChon;
        private decimal giaBanChon;
        private int slTonChon;

        // Hóa đơn đang lập (chưa lưu DB cho tới khi thanh toán xong)
        private HoaDonTam hoaDonHienTai;

        // Form chính gán hàm này để chuyển sang trang bán hàng.
        // Trả về true nếu trang bán hàng đã nhận hóa đơn.
        // Nếu không gán, frmThuoc tự mở frmBanHang thành cửa sổ riêng.
        public Func<HoaDonTam, bool> MoTrangBanHang { get; set; }

        private static frmBanHang frmBanHangRieng;

        public frmThuoc()
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.Text = "Tra cứu Thuốc";
            InitializeComponentCustom();
            LoadData();
            KhoiTaoHoaDonMoi();

            // Quay lại trang này thì nạp lại tồn kho mới nhất
            this.VisibleChanged += (s, e) => { if (this.Visible) LoadData(); };
        }

        private void InitializeComponentCustom()
        {
            this.SuspendLayout();

            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Dock = DockStyle.Fill;

            Panel pnlLeft = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(15) };
            Panel pnlRight = new Panel() { Dock = DockStyle.Right, Width = 380, BackColor = Color.White, Padding = new Padding(15), AutoScroll = true };

            // PANEL TRÁI: tìm kiếm dock Top, lưới dock Fill 
            Panel pnlSearch = new Panel() { Dock = DockStyle.Top, Height = 70 };
            Label lblSearch = new Label() { Text = "Tra cứu thuốc (Tên/Thành phần/Công dụng):", Font = new Font("Segoe UI", 9, FontStyle.Bold), Left = 0, Top = 0, AutoSize = true };
            txtTuKhoa = new TextBox() { Left = 0, Top = 27, Width = 320, Font = new Font("Segoe UI", 10) };
            txtTuKhoa.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TimKiem(); }
            };
            btnTimKiem = new Button() { Text = "Tìm kiếm", Left = 330, Top = 25, Width = 90, Height = 30, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnTimKiem.Click += (s, e) => TimKiem();
            btnLamMoi = new Button() { Text = "Làm mới", Left = 430, Top = 25, Width = 90, Height = 30, BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnLamMoi.Click += (s, e) => { txtTuKhoa.Clear(); LoadData(); };
            pnlSearch.Controls.Add(lblSearch);
            pnlSearch.Controls.Add(txtTuKhoa);
            pnlSearch.Controls.Add(btnTimKiem);
            pnlSearch.Controls.Add(btnLamMoi);

            dgvThuoc = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };
            dgvThuoc.SelectionChanged += DgvThuoc_SelectionChanged;

            pnlLeft.Controls.Add(dgvThuoc);
            pnlLeft.Controls.Add(pnlSearch);

            // PANEL PHẢI: THÔNG TIN CHI TIẾT (CHỈ XEM)
            Label lblFormTitle = new Label() { Text = "Thông Tin Chi Tiết Thuốc", Font = new Font("Segoe UI", 11, FontStyle.Bold), Left = 15, Top = 15, AutoSize = true };
            pnlRight.Controls.Add(lblFormTitle);

            int y = 55;
            txtMaThuoc = AddInfo(pnlRight, "Mã thuốc:", ref y, false);
            txtTenThuoc = AddInfo(pnlRight, "Tên thuốc:", ref y, false);
            txtThanhPhan = AddInfo(pnlRight, "Thành phần:", ref y, true);
            txtGiaNhap = AddInfo(pnlRight, "Giá nhập:", ref y, false);
            txtGiaBan = AddInfo(pnlRight, "Giá bán:", ref y, false);
            txtSLHienCo = AddInfo(pnlRight, "SL hiện có:", ref y, false);
            txtCongDung = AddInfo(pnlRight, "Công dụng:", ref y, true);
            txtChongChiDinh = AddInfo(pnlRight, "Chống chỉ định:", ref y, true);

            // PHẦN LẬP HÓA ĐƠN
            Label sep = new Label() { Left = 15, Top = y + 2, Width = 330, Height = 2, BorderStyle = BorderStyle.Fixed3D };
            pnlRight.Controls.Add(sep);
            y += 15;

            Label lblHDTitle = new Label() { Text = "Lập hóa đơn bán", Font = new Font("Segoe UI", 10, FontStyle.Bold), Left = 15, Top = y, AutoSize = true };
            pnlRight.Controls.Add(lblHDTitle);
            y += 28;

            // Label 1: mã hóa đơn sinh tự động
            lblMaHDB = new Label() { Left = 15, Top = y, Width = 330, Height = 22, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(0, 122, 204) };
            pnlRight.Controls.Add(lblMaHDB);
            y += 32;

            // Label 2: số lượng thuốc muốn thêm
            Label lblSL = new Label() { Text = "Số lượng:", Left = 15, Top = y + 3, Width = 98, Font = new Font("Segoe UI", 9) };
            nudSoLuong = new NumericUpDown() { Left = 115, Top = y, Width = 100, Minimum = 1, Maximum = 100000, Value = 1, Font = new Font("Segoe UI", 10) };
            pnlRight.Controls.Add(lblSL);
            pnlRight.Controls.Add(nudSoLuong);
            y += 40;

            btnThemVaoHD = CreateButton("Thêm vào hóa đơn", 15, y, 330, Color.FromArgb(40, 167, 69));
            btnThemVaoHD.Click += BtnThemVaoHD_Click;
            pnlRight.Controls.Add(btnThemVaoHD);
            y += 46;

            btnThanhToanHD = CreateButton("Thanh toán với hóa đơn", 15, y, 330, Color.FromArgb(0, 122, 204));
            btnThanhToanHD.Click += BtnThanhToanHD_Click;
            pnlRight.Controls.Add(btnThanhToanHD);
            y += 48;

            lblGioHang = new Label() { Left = 15, Top = y, Width = 330, Height = 40, Font = new Font("Segoe UI", 9, FontStyle.Italic), ForeColor = Color.DimGray };
            pnlRight.Controls.Add(lblGioHang);

            // Fill thêm trước => dock sau cùng, nằm gọn bên trái panel phải
            this.Controls.Add(pnlLeft);
            this.Controls.Add(pnlRight);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox AddInfo(Panel pnl, string labelText, ref int y, bool nhieuDong)
        {
            int cao = nhieuDong ? 52 : 25;
            Label lbl = new Label() { Text = labelText, Left = 15, Top = y + 3, Width = 98, Font = new Font("Segoe UI", 9) };
            TextBox txt = new TextBox()
            {
                Left = 115,
                Top = y,
                Width = 230,
                Font = new Font("Segoe UI", 10),
                ReadOnly = true,
                TabStop = false,
                BackColor = Color.FromArgb(248, 249, 250),
                Multiline = nhieuDong,
                ScrollBars = nhieuDong ? ScrollBars.Vertical : ScrollBars.None
            };
            if (nhieuDong) txt.Height = cao;
            pnl.Controls.Add(lbl);
            pnl.Controls.Add(txt);
            y += cao + 12;
            return txt;
        }

        private Button CreateButton(string text, int left, int top, int width, Color color)
        {
            return new Button()
            {
                Text = text,
                Left = left,
                Top = top,
                Width = width,
                Height = 38,
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
        }

        // ================== DỮ LIỆU THUỐC ==================

        private void LoadData()
        {
            try { dgvThuoc.DataSource = thuocBLL.LayDanhSachThuoc(); }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
        }

        private void TimKiem()
        {
            try { dgvThuoc.DataSource = thuocBLL.TimKiemThuoc(txtTuKhoa.Text.Trim()); }
            catch (Exception ex) { MessageBox.Show("Lỗi tra cứu: " + ex.Message); }
        }

        // Đọc giá trị ô theo tên cột; thử lần lượt các tên, cột nào có thì dùng
        private string Cell(DataGridViewRow row, params string[] tenCot)
        {
            foreach (string c in tenCot)
            {
                if (!dgvThuoc.Columns.Contains(c)) continue;
                object v = row.Cells[c].Value;
                return (v == null || v == DBNull.Value) ? "" : v.ToString();
            }
            return "";
        }

        private static string DinhDangTien(string giaTri)
        {
            decimal d;
            return decimal.TryParse(giaTri, out d) ? d.ToString("N0") + " đ" : giaTri;
        }

        private void DgvThuoc_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvThuoc.CurrentRow;
            if (row == null || row.Index < 0) { XoaChiTiet(); return; }

            maThuocChon = Cell(row, "MaThuoc").Trim();
            tenThuocChon = Cell(row, "TenThuoc");
            decimal.TryParse(Cell(row, "GiaBan"), out giaBanChon);
            int.TryParse(Cell(row, "SLHienCo"), out slTonChon);

            txtMaThuoc.Text = maThuocChon;
            txtTenThuoc.Text = tenThuocChon;
            txtThanhPhan.Text = Cell(row, "ThanhPhan");
            txtGiaNhap.Text = DinhDangTien(Cell(row, "DonGiaNhap"));
            txtGiaBan.Text = DinhDangTien(Cell(row, "GiaBan"));
            txtSLHienCo.Text = slTonChon.ToString();
            // Tên cột công dụng trong CSDL có thể khác nhau -> thử vài tên thường gặp
            txtCongDung.Text = Cell(row, "CongDung", "SuDung", "CachDung", "CongDungThuoc");
            txtChongChiDinh.Text = Cell(row, "ChongChiDinh");
        }

        private void XoaChiTiet()
        {
            maThuocChon = null; tenThuocChon = null; giaBanChon = 0; slTonChon = 0;
            txtMaThuoc.Clear(); txtTenThuoc.Clear(); txtThanhPhan.Clear(); txtGiaNhap.Clear();
            txtGiaBan.Clear(); txtSLHienCo.Clear(); txtCongDung.Clear(); txtChongChiDinh.Clear();
        }

        // ================== HÓA ĐƠN ==================

        private void KhoiTaoHoaDonMoi()
        {
            try
            {
                hoaDonHienTai = new HoaDonTam(hdbBLL.SinhMaHoaDonMoi());
            }
            catch (Exception ex)
            {
                hoaDonHienTai = null;
                MessageBox.Show("Không sinh được mã hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            nudSoLuong.Value = 1;
            CapNhatNhanHoaDon();
        }

        private void CapNhatNhanHoaDon()
        {
            if (hoaDonHienTai == null)
            {
                lblMaHDB.Text = "Mã hóa đơn: (chưa sinh được)";
                lblGioHang.Text = "";
                return;
            }
            lblMaHDB.Text = "Mã hóa đơn: " + hoaDonHienTai.MaHDB;
            lblGioHang.Text = hoaDonHienTai.Dong.Count == 0
                ? "Hóa đơn chưa có thuốc nào."
                : $"Đã có {hoaDonHienTai.Dong.Count} loại thuốc – tạm tính {hoaDonHienTai.TongTienHang:N0} đ";
        }

        private void BtnThemVaoHD_Click(object sender, EventArgs e)
        {
            if (hoaDonHienTai == null) KhoiTaoHoaDonMoi();
            if (hoaDonHienTai == null) return;

            if (string.IsNullOrEmpty(maThuocChon))
            {
                MessageBox.Show("Vui lòng chọn một thuốc trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (giaBanChon <= 0)
            {
                MessageBox.Show("Thuốc này chưa có giá bán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuong = (int)nudSoLuong.Value;
            int daCo = hoaDonHienTai.SoLuongCua(maThuocChon);
            if (daCo + soLuong > slTonChon)
            {
                MessageBox.Show($"Không đủ tồn kho! Còn {slTonChon}, hóa đơn đã có {daCo}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            hoaDonHienTai.ThemThuoc(maThuocChon, tenThuocChon, soLuong, giaBanChon);
            nudSoLuong.Value = 1;
            CapNhatNhanHoaDon();
        }

        private void BtnThanhToanHD_Click(object sender, EventArgs e)
        {
            if (hoaDonHienTai == null || hoaDonHienTai.Dong.Count == 0)
            {
                MessageBox.Show("Hóa đơn chưa có thuốc nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool daNhan;
            if (MoTrangBanHang != null)
            {
                daNhan = MoTrangBanHang(hoaDonHienTai);
            }
            else
            {
                if (frmBanHangRieng == null || frmBanHangRieng.IsDisposed)
                    frmBanHangRieng = new frmBanHang() { Size = new Size(1100, 650), StartPosition = FormStartPosition.CenterScreen };
                daNhan = frmBanHangRieng.NapHoaDon(hoaDonHienTai);
                if (daNhan) { frmBanHangRieng.Show(); frmBanHangRieng.Activate(); }
            }

            // Hóa đơn đã chuyển đi -> mã hóa đơn tăng lên 1 cho đơn tiếp theo
            if (daNhan) KhoiTaoHoaDonMoi();
        }
    }
}