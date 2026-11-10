using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] Quản lý nhân viên (chỉ quản lý):
    /// - Xem toàn bộ thông tin NV + mật khẩu đăng nhập.
    /// - NV đã bấm "Quên mật khẩu" -> dòng tô VÀNG, lên đầu danh sách. Chọn NV, nhập mật khẩu mới (hoặc
    ///   bấm "Mặc định"), bấm Lưu -> hết vàng.
    /// - Thêm / sửa / xóa NV. NV đã có hóa đơn không xóa được -> chuyển "Đã nghỉ việc" (khóa đăng nhập).
    /// </summary>
    public class frmQuanLyNhanVien : Form
    {
        /// <summary>Phát ra sau khi thêm / sửa / xóa (form chính cập nhật số yêu cầu quên mật khẩu).</summary>
        public event EventHandler DuLieuThayDoi;

        private readonly NhanVienBLL nvBLL = new NhanVienBLL();

        private DataGridView dgv;
        private TextBox txtTimKiem, txtMaNV, txtTenNV, txtDienThoai, txtDiaChi, txtMatKhau;
        private DateTimePicker dtpNgaySinh;
        private ComboBox cboLoc, cboGioiTinh, cboTrinhDo, cboChuyenMon, cboVaiTro;
        private CheckBox chkNghiViec;
        private Label lblYeuCau, lblCanhBaoNV, lblCheDo;
        private Button btnThem, btnLuu, btnXoa, btnLamMoi, btnMatKhauMacDinh;

        private DataTable dtNhanVien;
        private bool dangThemMoi;
        private bool dangNapForm;        // chặn sự kiện khi đang đổ dữ liệu vào ô
        private bool maTuDong;           // mã NV đang là mã gợi ý (chưa sửa tay)
        private string matKhauCu;        // mật khẩu lúc chọn NV (để biết có đổi không)
        private bool nvDangYeuCau;       // NV đang chọn có yêu cầu quên mật khẩu

        private static readonly Dictionary<string, string> TEN_COT = new Dictionary<string, string>
        {
            { "MaNV", "Mã NV" }, { "TenNV", "Họ tên" }, { "NgaySinh", "Ngày sinh" }, { "GioiTinh", "Giới tính" },
            { "DienThoai", "Điện thoại" }, { "DiaChi", "Địa chỉ" }, { "TenTrinhDo", "Trình độ" },
            { "TenChuyenMon", "Chuyên môn" }, { "TenVaiTro", "Vai trò" }, { "MatKhau", "Mật khẩu" },
            { "TrangThai", "Trạng thái" }
        };
        private static readonly HashSet<string> COT_AN = new HashSet<string>
        {
            "MaTrinhDo", "MaChuyenMon", "MaVaiTro", "YeuCauDoiMK", "NgayYeuCauDoiMK", "NghiViec"
        };

        public frmQuanLyNhanVien()
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Text = "Quản lý Nhân viên";
            InitializeComponentCustom();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            NapDanhMuc();
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

            // ---------- Thanh tìm kiếm ----------
            Panel pnlTren = new Panel() { Dock = DockStyle.Top, Height = 56, BackColor = Color.White, Padding = new Padding(15, 0, 15, 0) };
            Label lblTim = new Label() { Text = "Tìm (mã, tên, SĐT, vai trò):", Left = 15, Top = 18, AutoSize = true, Font = GiaoDien.TaoFont(9.5F) };
            txtTimKiem = new TextBox() { Left = 195, Top = 15, Width = 220, Font = GiaoDien.TaoFont(10F) };
            txtTimKiem.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TaiDanhSach(null); } };
            Button btnTim = GiaoDien.TaoNut("Tìm", GiaoDien.MauXanh, 70, 30);
            btnTim.Left = 425; btnTim.Top = 13;
            btnTim.Click += (s, e) => TaiDanhSach(null);

            Label lblLoc = new Label() { Text = "Hiển thị:", Left = 512, Top = 18, AutoSize = true, Font = GiaoDien.TaoFont(9.5F) };
            cboLoc = new ComboBox() { Left = 572, Top = 15, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Font = GiaoDien.TaoFont(9.5F) };
            cboLoc.Items.AddRange(new object[] { "Tất cả", "Đang làm việc", "Đã nghỉ việc", "Đang yêu cầu cấp lại MK" });
            cboLoc.SelectedIndex = 0;
            cboLoc.SelectedIndexChanged += (s, e) => ApDungLoc();

            lblYeuCau = new Label()
            {
                Dock = DockStyle.Right,
                Width = 330,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = GiaoDien.TaoFont(9.5F, FontStyle.Bold),
                BackColor = GiaoDien.VangNen,
                ForeColor = GiaoDien.VangChu,
                Cursor = Cursors.Hand,
                Visible = false
            };
            lblYeuCau.Click += (s, e) => cboLoc.SelectedIndex = 3;
            pnlTren.Controls.AddRange(new Control[] { lblTim, txtTimKiem, btnTim, lblLoc, cboLoc, lblYeuCau });

            // ---------- Lưới ----------
            dgv = GiaoDien.TaoLuoi();
            dgv.DataBindingComplete += (s, e) => GiaoDien.DinhDangCot(dgv, TEN_COT, COT_AN, null, new[] { "NgaySinh" });
            dgv.CellFormatting += Dgv_CellFormatting;
            dgv.SelectionChanged += (s, e) => { if (!dangThemMoi) HienThiDongDangChon(); };
            Panel pnlLuoi = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 10, 0) };
            pnlLuoi.Controls.Add(dgv);

            // ---------- Khung nhập liệu ----------
            Panel pnlPhai = new Panel() { Dock = DockStyle.Right, Width = 390, BackColor = Color.White, AutoScroll = true, Margin = new Padding(10) };
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
            int x1 = 18, x2 = 128, rong = 235, y = 15;

            p.Controls.Add(new Label() { Text = "Thông tin nhân viên", Left = x1, Top = y, AutoSize = true, Font = GiaoDien.TaoFont(12F, FontStyle.Bold) });
            lblCheDo = new Label() { Left = x1, Top = y + 26, Width = 345, Height = 20, Font = GiaoDien.TaoFont(9F, FontStyle.Italic), ForeColor = GiaoDien.MauXam };
            p.Controls.Add(lblCheDo);
            y += 52;

            lblCanhBaoNV = new Label()
            {
                Left = x1,
                Top = y,
                Width = 345,
                Height = 50,
                Padding = new Padding(8, 4, 8, 4),
                BackColor = GiaoDien.VangNen,
                ForeColor = GiaoDien.VangChu,
                Font = GiaoDien.TaoFont(9F, FontStyle.Bold),
                Visible = false
            };
            p.Controls.Add(lblCanhBaoNV);
            y += 58;

            p.Controls.Add(GiaoDien.TaoNhan("Mã NV:", x1, y, 105));
            txtMaNV = GiaoDien.TaoO(x2, y, rong);
            txtMaNV.CharacterCasing = CharacterCasing.Upper;
            txtMaNV.MaxLength = 10;
            txtMaNV.TextChanged += (s, e) =>
            {
                if (dangNapForm || !dangThemMoi) return;
                maTuDong = false;
            };
            p.Controls.Add(txtMaNV); y += 36;

            p.Controls.Add(GiaoDien.TaoNhan("Họ tên:", x1, y, 105));
            txtTenNV = GiaoDien.TaoO(x2, y, rong); txtTenNV.MaxLength = 50;
            p.Controls.Add(txtTenNV); y += 36;

            p.Controls.Add(GiaoDien.TaoNhan("Ngày sinh:", x1, y, 105));
            dtpNgaySinh = new DateTimePicker()
            {
                Left = x2,
                Top = y,
                Width = 160,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                ShowCheckBox = true,
                Font = GiaoDien.TaoFont(10F)
            };
            p.Controls.Add(dtpNgaySinh); y += 36;

            p.Controls.Add(GiaoDien.TaoNhan("Giới tính:", x1, y, 105));
            cboGioiTinh = new ComboBox() { Left = x2, Top = y, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList, Font = GiaoDien.TaoFont(10F) };
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            p.Controls.Add(cboGioiTinh); y += 36;

            p.Controls.Add(GiaoDien.TaoNhan("Điện thoại:", x1, y, 105));
            txtDienThoai = GiaoDien.TaoO(x2, y, rong); txtDienThoai.MaxLength = 20;
            p.Controls.Add(txtDienThoai); y += 36;

            p.Controls.Add(GiaoDien.TaoNhan("Địa chỉ:", x1, y, 105));
            txtDiaChi = GiaoDien.TaoO(x2, y, rong); txtDiaChi.MaxLength = 255;
            p.Controls.Add(txtDiaChi); y += 36;

            p.Controls.Add(GiaoDien.TaoNhan("Trình độ:", x1, y, 105));
            cboTrinhDo = TaoCombo(x2, y, rong); p.Controls.Add(cboTrinhDo); y += 36;

            p.Controls.Add(GiaoDien.TaoNhan("Chuyên môn:", x1, y, 105));
            cboChuyenMon = TaoCombo(x2, y, rong); p.Controls.Add(cboChuyenMon); y += 36;

            p.Controls.Add(GiaoDien.TaoNhan("Vai trò (*):", x1, y, 105));
            cboVaiTro = TaoCombo(x2, y, rong);
            cboVaiTro.SelectedIndexChanged += (s, e) =>
            {
                // Thêm mới: đổi vai trò -> gợi ý lại mã NV theo vai trò (QLxxx / TKxxx / NVxx)
                if (dangNapForm || !dangThemMoi || !maTuDong) return;
                GoiYMaMoi();
            };
            p.Controls.Add(cboVaiTro); y += 36;

            p.Controls.Add(GiaoDien.TaoNhan("Mật khẩu (*):", x1, y, 105));
            txtMatKhau = GiaoDien.TaoO(x2, y, 150); txtMatKhau.MaxLength = NhanVienBLL.DO_DAI_MK_TOI_DA;
            p.Controls.Add(txtMatKhau);
            btnMatKhauMacDinh = GiaoDien.TaoNut("Mặc định", GiaoDien.MauXam, 80, 27);
            btnMatKhauMacDinh.Left = x2 + 155; btnMatKhauMacDinh.Top = y - 1;
            btnMatKhauMacDinh.Font = GiaoDien.TaoFont(8.5F, FontStyle.Bold);
            btnMatKhauMacDinh.Click += (s, e) => txtMatKhau.Text = NhanVienBLL.MatKhauMacDinh(txtMaNV.Text);
            p.Controls.Add(btnMatKhauMacDinh); y += 30;
            p.Controls.Add(new Label()
            {
                Text = "Mặc định = mã NV + 11, tối thiểu " + NhanVienBLL.DO_DAI_MK_TOI_THIEU + " ký tự",
                Left = x2,
                Top = y,
                Width = 240,
                Height = 18,
                Font = GiaoDien.TaoFont(8F, FontStyle.Italic),
                ForeColor = Color.Gray
            });
            y += 26;

            chkNghiViec = new CheckBox() { Text = "Đã nghỉ việc (khóa đăng nhập)", Left = x2, Top = y, AutoSize = true, Font = GiaoDien.TaoFont(9.5F) };
            p.Controls.Add(chkNghiViec); y += 40;

            btnThem = GiaoDien.TaoNut("➕ Thêm mới", GiaoDien.MauXanh, 165, 38);
            btnThem.Left = x1; btnThem.Top = y;
            btnThem.Click += (s, e) => BatDauThemMoi();
            btnLuu = GiaoDien.TaoNut("💾 Lưu", GiaoDien.MauXanhLa, 165, 38);
            btnLuu.Left = x1 + 180; btnLuu.Top = y;
            btnLuu.Click += (s, e) => Luu();
            y += 46;
            btnXoa = GiaoDien.TaoNut("🗑 Xóa", GiaoDien.MauDo, 165, 38);
            btnXoa.Left = x1; btnXoa.Top = y;
            btnXoa.Click += (s, e) => Xoa();
            btnLamMoi = GiaoDien.TaoNut("↻ Làm mới", GiaoDien.MauXam, 165, 38);
            btnLamMoi.Left = x1 + 180; btnLamMoi.Top = y;
            btnLamMoi.Click += (s, e) => { txtTimKiem.Clear(); cboLoc.SelectedIndex = 0; TaiDanhSach(null); };
            p.Controls.AddRange(new Control[] { btnThem, btnLuu, btnXoa, btnLamMoi });
        }

        private static ComboBox TaoCombo(int x, int y, int rong)
        {
            return new ComboBox() { Left = x, Top = y, Width = rong, DropDownStyle = ComboBoxStyle.DropDownList, Font = GiaoDien.TaoFont(10F) };
        }

        // ================================================================
        // DỮ LIỆU
        // ================================================================
        private void NapDanhMuc()
        {
            try
            {
                dangNapForm = true;
                GanCombo(cboTrinhDo, nvBLL.LayTrinhDo(), "MaTrinhDo", "TenTrinhDo");
                GanCombo(cboChuyenMon, nvBLL.LayChuyenMon(), "MaChuyenMon", "TenChuyenMon");
                GanCombo(cboVaiTro, nvBLL.LayVaiTro(), "MaVaiTro", "TenVaiTro");
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải danh mục"); }
            finally { dangNapForm = false; }
        }

        private static void GanCombo(ComboBox cbo, DataTable dt, string ma, string ten)
        {
            cbo.DisplayMember = ten;
            cbo.ValueMember = ma;
            cbo.DataSource = dt;
            cbo.SelectedIndex = -1;
        }

        private void TaiDanhSach(string maChon)
        {
            try
            {
                if (maChon == null && dgv.CurrentRow != null) maChon = GiaoDien.GiaTri(dgv.CurrentRow, "MaNV");
                dtNhanVien = nvBLL.LayDanhSach(txtTimKiem.Text);

                // Cột "Trạng thái" hiển thị
                dtNhanVien.Columns.Add("TrangThai", typeof(string));
                foreach (DataRow r in dtNhanVien.Rows)
                {
                    bool nghi = Convert.ToBoolean(r["NghiViec"]);
                    bool yeuCau = Convert.ToBoolean(r["YeuCauDoiMK"]);
                    if (nghi) r["TrangThai"] = "Đã nghỉ việc";
                    else if (yeuCau)
                        r["TrangThai"] = "🔑 Quên MK" + (r["NgayYeuCauDoiMK"] == DBNull.Value ? "" :
                                         " (" + Convert.ToDateTime(r["NgayYeuCauDoiMK"]).ToString("dd/MM HH:mm") + ")");
                    else r["TrangThai"] = "Đang làm việc";
                }

                dangThemMoi = false;
                dgv.DataSource = dtNhanVien;
                ApDungLoc();
                GiaoDien.ChonDong(dgv, "MaNV", maChon);
                HienThiDongDangChon();
                CapNhatNhanYeuCau();
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải danh sách"); }
        }

        private void ApDungLoc()
        {
            if (dtNhanVien == null) return;
            string loc = "";
            switch (cboLoc.SelectedIndex)
            {
                case 1: loc = "NghiViec = false"; break;
                case 2: loc = "NghiViec = true"; break;
                case 3: loc = "YeuCauDoiMK = true AND NghiViec = false"; break;
            }
            dtNhanVien.DefaultView.RowFilter = loc;
        }

        private void CapNhatNhanYeuCau()
        {
            int n = 0;
            foreach (DataRow r in dtNhanVien.Rows)
                if (Convert.ToBoolean(r["YeuCauDoiMK"]) && !Convert.ToBoolean(r["NghiViec"])) n++;
            lblYeuCau.Visible = n > 0;
            lblYeuCau.Text = "🔑 " + n + " NV yêu cầu cấp lại mật khẩu (dòng vàng) – bấm để lọc";
        }

        // Dòng vàng = đang yêu cầu cấp lại mật khẩu; chữ xám = đã nghỉ việc
        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !dgv.Columns.Contains("YeuCauDoiMK")) return;
            DataGridViewRow r = dgv.Rows[e.RowIndex];
            bool nghi = GiaoDien.GiaTri(r, "NghiViec") == "True";
            bool yeuCau = GiaoDien.GiaTri(r, "YeuCauDoiMK") == "True";
            if (yeuCau && !nghi)
            {
                e.CellStyle.BackColor = GiaoDien.VangNen;
                e.CellStyle.ForeColor = GiaoDien.VangChu;
                e.CellStyle.SelectionBackColor = GiaoDien.VangChon;
                e.CellStyle.SelectionForeColor = Color.Black;
            }
            else if (nghi)
            {
                e.CellStyle.ForeColor = GiaoDien.XamChu;
                e.CellStyle.BackColor = GiaoDien.XamNen;
            }
        }

        // ================================================================
        // ĐỔ DỮ LIỆU LÊN Ô NHẬP
        // ================================================================
        private void HienThiDongDangChon()
        {
            DataGridViewRow r = dgv.CurrentRow;
            dangNapForm = true;
            try
            {
                if (r == null)
                {
                    XoaO();
                    lblCheDo.Text = "Chọn 1 nhân viên để sửa, hoặc bấm \"Thêm mới\".";
                    txtMaNV.ReadOnly = true;
                    return;
                }

                txtMaNV.Text = GiaoDien.GiaTri(r, "MaNV").Trim();
                txtTenNV.Text = GiaoDien.GiaTri(r, "TenNV");
                DateTime ns;
                if (DateTime.TryParse(GiaoDien.GiaTri(r, "NgaySinh"), out ns))
                {
                    dtpNgaySinh.Value = ns;
                    dtpNgaySinh.Checked = true;
                }
                else
                {
                    dtpNgaySinh.Value = new DateTime(2000, 1, 1);
                    dtpNgaySinh.Checked = false;
                }
                string gt = GiaoDien.GiaTri(r, "GioiTinh");
                cboGioiTinh.SelectedIndex = cboGioiTinh.Items.IndexOf(gt);
                txtDienThoai.Text = GiaoDien.GiaTri(r, "DienThoai");
                txtDiaChi.Text = GiaoDien.GiaTri(r, "DiaChi");
                GiaoDien.ChonCombo(cboTrinhDo, GiaoDien.GiaTri(r, "MaTrinhDo"));
                GiaoDien.ChonCombo(cboChuyenMon, GiaoDien.GiaTri(r, "MaChuyenMon"));
                GiaoDien.ChonCombo(cboVaiTro, GiaoDien.GiaTri(r, "MaVaiTro"));
                txtMatKhau.Text = GiaoDien.GiaTri(r, "MatKhau");
                chkNghiViec.Checked = GiaoDien.GiaTri(r, "NghiViec") == "True";

                matKhauCu = txtMatKhau.Text;
                nvDangYeuCau = GiaoDien.GiaTri(r, "YeuCauDoiMK") == "True" && !chkNghiViec.Checked;
                lblCanhBaoNV.Visible = nvDangYeuCau;
                if (nvDangYeuCau)
                {
                    string luc = GiaoDien.GiaTri(r, "NgayYeuCauDoiMK");
                    DateTime t;
                    lblCanhBaoNV.Text = "🔑 Nhân viên yêu cầu cấp lại mật khẩu" +
                                        (DateTime.TryParse(luc, out t) ? " lúc " + t.ToString("dd/MM/yyyy HH:mm") : "") +
                                        ".\nNhập mật khẩu mới (hoặc bấm \"Mặc định\") rồi bấm Lưu.";
                }

                txtMaNV.ReadOnly = true;
                txtMaNV.BackColor = GiaoDien.MauOChiDoc;
                bool laMinh = string.Equals(txtMaNV.Text, PhienDangNhap.MaNV, StringComparison.OrdinalIgnoreCase);
                lblCheDo.Text = laMinh ? "Đang sửa: tài khoản của bạn" : "Đang sửa: " + txtMaNV.Text;
                btnXoa.Enabled = !laMinh;
                GiaoDien.BatTatNut(btnXoa, !laMinh);
            }
            finally
            {
                dangNapForm = false;
            }
        }

        private void XoaO()
        {
            txtMaNV.Clear(); txtTenNV.Clear(); txtDienThoai.Clear(); txtDiaChi.Clear(); txtMatKhau.Clear();
            dtpNgaySinh.Value = new DateTime(2000, 1, 1);
            dtpNgaySinh.Checked = false;
            cboGioiTinh.SelectedIndex = -1;
            cboTrinhDo.SelectedIndex = -1;
            cboChuyenMon.SelectedIndex = -1;
            cboVaiTro.SelectedIndex = -1;
            chkNghiViec.Checked = false;
            lblCanhBaoNV.Visible = false;
            nvDangYeuCau = false;
            matKhauCu = null;
        }

        private void BatDauThemMoi()
        {
            dangThemMoi = true;
            dangNapForm = true;
            try
            {
                XoaO();
                txtMaNV.ReadOnly = false;
                txtMaNV.BackColor = Color.White;
                cboGioiTinh.SelectedIndex = 0;
                GiaoDien.ChonCombo(cboVaiTro, VaiTroNV.BAN_HANG);
                lblCheDo.Text = "Đang THÊM MỚI nhân viên – điền thông tin rồi bấm Lưu";
                GiaoDien.BatTatNut(btnXoa, false);
            }
            finally { dangNapForm = false; }
            GoiYMaMoi();
            txtTenNV.Focus();
        }

        private void GoiYMaMoi()
        {
            try
            {
                dangNapForm = true;
                string ma = nvBLL.SinhMaMoi(GiaoDien.GiaTriCombo(cboVaiTro));
                txtMaNV.Text = ma;
                txtMatKhau.Text = NhanVienBLL.MatKhauMacDinh(ma);
                maTuDong = true;
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex); }
            finally { dangNapForm = false; }
        }

        private NhanVienDTO DocForm()
        {
            return new NhanVienDTO
            {
                MaNV = txtMaNV.Text.Trim(),
                TenNV = txtTenNV.Text.Trim(),
                NgaySinh = dtpNgaySinh.Checked ? dtpNgaySinh.Value.Date : (DateTime?)null,
                GioiTinh = cboGioiTinh.SelectedItem == null ? null : cboGioiTinh.SelectedItem.ToString(),
                DienThoai = txtDienThoai.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                MaTrinhDo = GiaoDien.GiaTriCombo(cboTrinhDo),
                MaChuyenMon = GiaoDien.GiaTriCombo(cboChuyenMon),
                MaVaiTro = GiaoDien.GiaTriCombo(cboVaiTro),
                MatKhau = txtMatKhau.Text,
                NghiViec = chkNghiViec.Checked
            };
        }

        // ================================================================
        // THÊM / SỬA / XÓA
        // ================================================================
        private void Luu()
        {
            if (!dangThemMoi && dgv.CurrentRow == null)
            {
                MessageBox.Show(this, "Chọn 1 nhân viên để sửa, hoặc bấm \"Thêm mới\".", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            NhanVienDTO nv = DocForm();
            try
            {
                if (dangThemMoi)
                {
                    nvBLL.Them(nv);
                    MessageBox.Show(this, "Đã thêm nhân viên " + nv.MaNV + ".\nMật khẩu đăng nhập: " + nv.MatKhau,
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    bool xuLyYeuCau = false;
                    if (nvDangYeuCau)
                    {
                        if (nv.MatKhau != matKhauCu)
                            xuLyYeuCau = true;
                        else
                        {
                            DialogResult kq = MessageBox.Show(this,
                                "Nhân viên này đang yêu cầu cấp lại mật khẩu nhưng mật khẩu CHƯA thay đổi.\n\n" +
                                "Yes: vẫn đánh dấu đã xử lý (giữ mật khẩu cũ)\nNo: chỉ lưu thông tin, yêu cầu vẫn còn\nCancel: quay lại",
                                "Yêu cầu quên mật khẩu", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (kq == DialogResult.Cancel) return;
                            xuLyYeuCau = kq == DialogResult.Yes;
                        }
                    }
                    nvBLL.Sua(nv, xuLyYeuCau);
                    MessageBox.Show(this,
                        xuLyYeuCau ? "Đã cấp lại mật khẩu cho " + nv.MaNV + ": " + nv.MatKhau + "\nHãy báo mật khẩu mới cho nhân viên."
                                   : "Đã lưu thông tin nhân viên " + nv.MaNV + ".",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                TaiDanhSach(nv.MaNV);
                DuLieuThayDoi?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi lưu nhân viên"); }
        }

        private void Xoa()
        {
            if (dangThemMoi || dgv.CurrentRow == null) return;
            string ma = GiaoDien.GiaTri(dgv.CurrentRow, "MaNV").Trim();
            string ten = GiaoDien.GiaTri(dgv.CurrentRow, "TenNV");
            if (MessageBox.Show(this, "Xóa nhân viên " + ma + " - " + ten + "?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            try
            {
                if (nvBLL.Xoa(ma))
                {
                    MessageBox.Show(this, "Đã xóa nhân viên " + ma + ".", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (MessageBox.Show(this,
                             "Nhân viên " + ma + " đã lập hóa đơn / phiếu nhập nên không thể xóa (cần giữ lịch sử).\n\n" +
                             "Chuyển sang \"Đã nghỉ việc\" để khóa đăng nhập?",
                             "Không thể xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    nvBLL.DatNghiViec(ma);
                }
                TaiDanhSach(ma);
                DuLieuThayDoi?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi xóa nhân viên"); }
        }
    }
}
