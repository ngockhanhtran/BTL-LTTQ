using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ClosedXML.Excel;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    public partial class frmBanHang : Form
    {
        private readonly HoaDonBanBLL hdbBLL = new HoaDonBanBLL();

        private TextBox txtMaHDB, txtNgayLap, txtMaKhach, txtTenKhach;
        private ComboBox cboNhanVien;
        private Label lblTrangThai, lblTienHang, lblTienGiam, lblTongTien;
        private NumericUpDown nudGiamGia;
        private DataGridView dgvChiTiet;
        private Button btnThanhToan, btnLamMoi, btnXuatExcel;
        private System.Windows.Forms.Timer tmrKhach;

        private HoaDonTam hoaDon;          // hóa đơn nhận từ trang Tra cứu
        private bool daThanhToan;
        private DateTime ngayBan;
        private string tenKhachHienTai;    // tên khách CŨ lấy từ CSDL (null nếu chưa tra / khách mới)
        private bool khachMoi;             // true = mã khách chưa có trong CSDL, phải nhập tay tên
        private Label lblKhachInfo;

        public frmBanHang()
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.Text = "Bán Hàng";
            InitializeComponentCustom();
            LoadNhanVien();
            DatLaiForm();
        }

        private void InitializeComponentCustom()
        {
            this.SuspendLayout();

            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Dock = DockStyle.Fill;

            TableLayoutPanel tbl = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Padding = new Padding(15) };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ================= NỬA TRÁI: THÔNG TIN HÓA ĐƠN =================
            Panel pnlTrai = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0, 0, 8, 0), AutoScroll = true };
            pnlTrai.Controls.Add(new Label() { Text = "Thông Tin Hóa Đơn", Font = new Font("Segoe UI", 12, FontStyle.Bold), Left = 20, Top = 20, AutoSize = true });

            int y = 70;
            TaoNhan(pnlTrai, "Mã hóa đơn:", y); txtMaHDB = TaoO(pnlTrai, y, true); y += 45;
            TaoNhan(pnlTrai, "Ngày lập:", y); txtNgayLap = TaoO(pnlTrai, y, true); y += 45;

            TaoNhan(pnlTrai, "Nhân viên:", y);
            cboNhanVien = new ComboBox()
            {
                Left = 145,
                Top = y,
                Width = 230,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlTrai.Controls.Add(cboNhanVien);
            y += 45;

            TaoNhan(pnlTrai, "Mã khách hàng:", y); txtMaKhach = TaoO(pnlTrai, y, false); y += 45;
            TaoNhan(pnlTrai, "Tên khách hàng:", y); txtTenKhach = TaoO(pnlTrai, y, true); y += 32;

            // Dòng báo khách cũ / khách mới ngay dưới ô tên
            lblKhachInfo = new Label() { Left = 145, Top = y, Width = 230, Height = 20, Font = new Font("Segoe UI", 8.5F, FontStyle.Italic), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            pnlTrai.Controls.Add(lblKhachInfo);
            y += 30;

            lblTrangThai = new Label() { Left = 20, Top = y, Width = 355, Height = 40, Font = new Font("Segoe UI", 10, FontStyle.Bold), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            pnlTrai.Controls.Add(lblTrangThai);
            y += 45;

            pnlTrai.Controls.Add(new Label()
            {
                Left = 20,
                Top = y,
                Width = 355,
                Height = 70,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                Text = "• Thuốc được thêm từ trang Tra cứu thuốc.\n• Khách cũ: nhập mã, tên tự hiện. Khách mới: nhập thêm tên.\n• Nhấp đúp một dòng bên phải để bỏ thuốc khỏi đơn."
            });

            // Tự tra tên khách sau khi ngừng gõ 0.4s
            tmrKhach = new System.Windows.Forms.Timer() { Interval = 400 };
            tmrKhach.Tick += (s, e) => { tmrKhach.Stop(); TraCuuKhachHang(); };
            txtMaKhach.TextChanged += (s, e) =>
            {
                if (daThanhToan) return;
                tenKhachHienTai = null;
                khachMoi = false;
                txtTenKhach.Clear();
                DatCheDoTenKhach(false);
                lblKhachInfo.Text = "";
                tmrKhach.Stop();
                tmrKhach.Start();
            };

            // ================= NỬA PHẢI: CHI TIẾT + TỔNG TIỀN =================
            Panel pnlPhai = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(8, 0, 0, 0), Padding = new Padding(15) };
            Label lblTieuDePhai = new Label() { Text = "Chi Tiết Hóa Đơn", Dock = DockStyle.Top, Height = 35, Font = new Font("Segoe UI", 12, FontStyle.Bold) };

            dgvChiTiet = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            dgvChiTiet.Columns.Add("STT", "STT");
            dgvChiTiet.Columns.Add("MaThuoc", "Mã thuốc");
            dgvChiTiet.Columns.Add("TenThuoc", "Tên thuốc");
            dgvChiTiet.Columns.Add("SoLuong", "SL");
            dgvChiTiet.Columns.Add("GiaBan", "Giá bán");
            dgvChiTiet.Columns.Add("ThanhTien", "Thành tiền");
            dgvChiTiet.Columns["STT"].FillWeight = 8;
            dgvChiTiet.Columns["MaThuoc"].FillWeight = 15;
            dgvChiTiet.Columns["TenThuoc"].FillWeight = 37;
            dgvChiTiet.Columns["SoLuong"].FillWeight = 10;
            dgvChiTiet.Columns["GiaBan"].FillWeight = 15;
            dgvChiTiet.Columns["ThanhTien"].FillWeight = 18;
            foreach (string c in new[] { "SoLuong", "GiaBan", "ThanhTien" })
                dgvChiTiet.Columns[c].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvChiTiet.CellDoubleClick += DgvChiTiet_CellDoubleClick;

            Panel pnlTong = new Panel() { Dock = DockStyle.Bottom, Height = 205 };
            Font fNhan = new Font("Segoe UI", 10);

            pnlTong.Controls.Add(new Label() { Text = "Giảm giá (%):", Left = 0, Top = 13, Width = 135, Font = fNhan });
            nudGiamGia = new NumericUpDown() { Left = 140, Top = 10, Width = 90, Minimum = 0, Maximum = 100, Font = fNhan };
            nudGiamGia.ValueChanged += (s, e) =>
            {
                if (hoaDon == null || daThanhToan) return;
                hoaDon.GiamGiaPhanTram = nudGiamGia.Value;
                CapNhatTongTien();
            };
            pnlTong.Controls.Add(nudGiamGia);

            pnlTong.Controls.Add(new Label() { Text = "Tiền hàng:", Left = 0, Top = 50, Width = 135, Font = fNhan });
            lblTienHang = new Label() { Left = 140, Top = 50, AutoSize = true, Font = fNhan };
            pnlTong.Controls.Add(lblTienHang);

            pnlTong.Controls.Add(new Label() { Text = "Giảm giá:", Left = 0, Top = 78, Width = 135, Font = fNhan });
            lblTienGiam = new Label() { Left = 140, Top = 78, AutoSize = true, Font = fNhan };
            pnlTong.Controls.Add(lblTienGiam);

            pnlTong.Controls.Add(new Label() { Text = "TỔNG TIỀN:", Left = 0, Top = 112, Width = 135, Font = new Font("Segoe UI", 12, FontStyle.Bold) });
            lblTongTien = new Label() { Left = 140, Top = 106, AutoSize = true, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.FromArgb(220, 53, 69) };
            pnlTong.Controls.Add(lblTongTien);

            btnThanhToan = TaoNut("Thanh toán", 0, 155, 160, Color.FromArgb(0, 122, 204));
            btnLamMoi = TaoNut("Làm mới", 170, 155, 120, Color.FromArgb(108, 117, 125));
            btnXuatExcel = TaoNut("Xuất hóa đơn Excel", 300, 155, 170, Color.FromArgb(40, 167, 69));
            btnThanhToan.Click += BtnThanhToan_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnXuatExcel.Click += (s, e) => XuatExcel();
            pnlTong.Controls.Add(btnThanhToan);
            pnlTong.Controls.Add(btnLamMoi);
            pnlTong.Controls.Add(btnXuatExcel);

            // Fill thêm trước để dock sau cùng
            pnlPhai.Controls.Add(dgvChiTiet);
            pnlPhai.Controls.Add(pnlTong);
            pnlPhai.Controls.Add(lblTieuDePhai);

            tbl.Controls.Add(pnlTrai, 0, 0);
            tbl.Controls.Add(pnlPhai, 1, 0);
            this.Controls.Add(tbl);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void TaoNhan(Panel p, string text, int y)
        {
            p.Controls.Add(new Label() { Text = text, Left = 20, Top = y + 3, Width = 120, Font = new Font("Segoe UI", 10) });
        }

        private TextBox TaoO(Panel p, int y, bool chiDoc)
        {
            TextBox t = new TextBox()
            {
                Left = 145,
                Top = y,
                Width = 230,
                Font = new Font("Segoe UI", 10),
                ReadOnly = chiDoc,
                TabStop = !chiDoc,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            if (chiDoc) t.BackColor = Color.FromArgb(240, 242, 245);
            p.Controls.Add(t);
            return t;
        }

        private Button TaoNut(string text, int left, int top, int width, Color mau)
        {
            return new Button()
            {
                Text = text,
                Left = left,
                Top = top,
                Width = width,
                Height = 42,
                BackColor = mau,
                ForeColor = Color.White,
                Tag = mau,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
        }

        private static void BatTatNut(Button b, bool bat)
        {
            b.Enabled = bat;
            b.BackColor = bat ? (Color)b.Tag : Color.FromArgb(200, 203, 207);
        }

        private static string Tien(decimal d) => d.ToString("N0") + " đ";

        // ================== NHẬN HÓA ĐƠN TỪ TRANG TRA CỨU ==================

        // Gọi từ trang Tra cứu khi bấm "Thanh toán với hóa đơn".
        // Trả về false nếu người dùng không muốn thay hóa đơn đang lập dở.
        public bool NapHoaDon(HoaDonTam hd)
        {
            if (hd == null) return false;

            if (hoaDon != null && hoaDon != hd && !daThanhToan && hoaDon.Dong.Count > 0)
            {
                DialogResult r = MessageBox.Show(
                    $"Hóa đơn {hoaDon.MaHDB} chưa thanh toán. Thay bằng hóa đơn {hd.MaHDB}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (r != DialogResult.Yes) return false;
            }

            hoaDon = hd;
            daThanhToan = false;
            khachMoi = false;
            tenKhachHienTai = null;
            ngayBan = DateTime.Now;
            txtNgayLap.Text = ngayBan.ToString("dd/MM/yyyy HH:mm");
            txtMaKhach.Clear();
            nudGiamGia.Value = hd.GiamGiaPhanTram;

            HienThiHoaDon();
            CapNhatTrangThai();
            return true;
        }

        // ================== HIỂN THỊ ==================

        private void HienThiHoaDon()
        {
            dgvChiTiet.Rows.Clear();
            txtMaHDB.Text = hoaDon == null ? "" : hoaDon.MaHDB;

            if (hoaDon != null)
            {
                int stt = 1;
                foreach (DongHoaDon d in hoaDon.Dong)
                    dgvChiTiet.Rows.Add(stt++, d.MaThuoc, d.TenThuoc, d.SoLuong, d.GiaBan.ToString("N0"), d.ThanhTien.ToString("N0"));
            }
            CapNhatTongTien();
        }

        private void CapNhatTongTien()
        {
            decimal tienHang = hoaDon == null ? 0 : hoaDon.TongTienHang;
            decimal tienGiam = hoaDon == null ? 0 : hoaDon.TienGiam;
            decimal tong = hoaDon == null ? 0 : hoaDon.TongThanhToan;

            lblTienHang.Text = Tien(tienHang);
            lblTienGiam.Text = (tienGiam > 0 ? "- " : "") + Tien(tienGiam);
            lblTongTien.Text = Tien(tong);
        }

        private void CapNhatTrangThai()
        {
            bool coHang = hoaDon != null && hoaDon.Dong.Count > 0;

            BatTatNut(btnThanhToan, coHang && !daThanhToan);
            BatTatNut(btnXuatExcel, daThanhToan);

            cboNhanVien.Enabled = !daThanhToan;
            txtMaKhach.ReadOnly = daThanhToan;
            DatCheDoTenKhach(khachMoi && !daThanhToan);
            nudGiamGia.Enabled = hoaDon != null && !daThanhToan;

            if (hoaDon == null)
            {
                lblTrangThai.Text = "Chưa có hóa đơn – hãy thêm thuốc từ trang Tra cứu thuốc.";
                lblTrangThai.ForeColor = Color.DimGray;
            }
            else if (daThanhToan)
            {
                lblTrangThai.Text = "Trạng thái: ĐÃ THANH TOÁN";
                lblTrangThai.ForeColor = Color.FromArgb(40, 167, 69);
            }
            else
            {
                lblTrangThai.Text = "Trạng thái: Chưa thanh toán";
                lblTrangThai.ForeColor = Color.FromArgb(230, 126, 34);
            }
        }

        // ================== DỮ LIỆU PHỤ ==================

        private void LoadNhanVien()
        {
            try
            {
                DataTable dt = hdbBLL.LayDanhSachNhanVien();
                cboNhanVien.DisplayMember = "HienThi";
                cboNhanVien.ValueMember = "MaNV";
                cboNhanVien.DataSource = dt;
                cboNhanVien.SelectedIndex = dt.Rows.Count > 0 ? 0 : -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên: " + ex.Message);
            }
        }

        //Ô tên khách: chỉ cho nhập khi là khách mới
        private void DatCheDoTenKhach(bool choNhap)
        {
            txtTenKhach.ReadOnly = !choNhap;
            txtTenKhach.TabStop = choNhap;
            txtTenKhach.BackColor = choNhap ? Color.White : Color.FromArgb(240, 242, 245);
        }

        // Kiểm tra mã khách trong CSDL:
        //  - Đã tồn tại  -> tự điền tên, khóa ô tên.
        //  - Chưa tồn tại -> mở ô tên cho nhập tay (khách mới, lưu khi thanh toán)
        private void TraCuuKhachHang()
        {
            string ma = txtMaKhach.Text.Trim();
            tenKhachHienTai = null;
            khachMoi = false;
            txtTenKhach.Clear();
            DatCheDoTenKhach(false);
            lblKhachInfo.Text = "";
            if (ma == "") return;

            try
            {
                string ten = hdbBLL.LayTenKhachHang(ma);
                if (ten != null)
                {
                    tenKhachHienTai = ten;
                    txtTenKhach.Text = ten;
                    lblKhachInfo.ForeColor = Color.FromArgb(40, 167, 69);
                    lblKhachInfo.Text = "✓ Khách hàng cũ – đã tự điền tên";
                }
                else
                {
                    khachMoi = true;
                    DatCheDoTenKhach(true);
                    lblKhachInfo.ForeColor = Color.FromArgb(230, 126, 34);
                    lblKhachInfo.Text = "Khách hàng mới – vui lòng nhập tên";
                }
            }
            catch (Exception ex)
            {
                lblKhachInfo.ForeColor = Color.Firebrick;
                lblKhachInfo.Text = "Lỗi tra cứu: " + ex.Message;
            }
        }

        // ================== SỰ KIỆN ==================

        private void DgvChiTiet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || hoaDon == null || daThanhToan) return;

            string ma = dgvChiTiet.Rows[e.RowIndex].Cells["MaThuoc"].Value.ToString();
            string ten = dgvChiTiet.Rows[e.RowIndex].Cells["TenThuoc"].Value.ToString();
            if (MessageBox.Show($"Bỏ thuốc \"{ten}\" khỏi hóa đơn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            hoaDon.XoaThuoc(ma);
            HienThiHoaDon();
            CapNhatTrangThai();
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (hoaDon == null || hoaDon.Dong.Count == 0)
            {
                MessageBox.Show("Hóa đơn chưa có thuốc nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maKhach = txtMaKhach.Text.Trim();
            if (maKhach == "")
            {
                MessageBox.Show("Vui lòng nhập mã khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKhach.Focus();
                return;
            }
            if (tenKhachHienTai == null && !khachMoi)
            {
                // Người dùng bấm thanh toán ngay sau khi gõ mã -> tra cứu luôn
                tmrKhach.Stop();
                TraCuuKhachHang();
                if (tenKhachHienTai == null && !khachMoi) return; // lỗi tra cứu, đã báo ở lblKhachInfo
            }
            if (khachMoi && txtTenKhach.Text.Trim() == "")
            {
                MessageBox.Show($"Mã khách hàng \"{maKhach}\" chưa có trong hệ thống.\nVui lòng nhập tên khách hàng mới.",
                    "Khách hàng mới", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenKhach.Focus();
                return;
            }

            // Kiểm tra tồn kho TRƯỚC khi cho khách quét QR
            try
            {
                string loi = hdbBLL.KiemTraTonKho(hoaDon);
                if (loi != null)
                {
                    MessageBox.Show(loi, "Không đủ hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra tồn kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (frmThanhToan f = new frmThanhToan(hoaDon.MaHDB, hoaDon.TongThanhToan))
            {
                if (f.ShowDialog(this.TopLevelControl ?? this) != DialogResult.OK) return;
            }

            try
            {
                ngayBan = DateTime.Now;
                string tenKhachMoi = khachMoi ? txtTenKhach.Text.Trim() : null;
                hdbBLL.LuuHoaDon(hoaDon, cboNhanVien.SelectedValue.ToString(), maKhach, tenKhachMoi, ngayBan);

                if (khachMoi)
                {
                    // Khách mới đã được lưu cùng hóa đơn -> từ giờ là khách cũ
                    tenKhachHienTai = tenKhachMoi;
                    khachMoi = false;
                    lblKhachInfo.ForeColor = Color.FromArgb(40, 167, 69);
                    lblKhachInfo.Text = "✓ Đã thêm khách hàng mới";
                }

                daThanhToan = true;
                txtNgayLap.Text = ngayBan.ToString("dd/MM/yyyy HH:mm");
                CapNhatTrangThai();

                MessageBox.Show($"Thanh toán hóa đơn {hoaDon.MaHDB} thành công!\nTồn kho đã được trừ. Bạn có thể xuất hóa đơn ra Excel.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lưu hóa đơn thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            if (hoaDon != null && !daThanhToan && hoaDon.Dong.Count > 0)
            {
                if (MessageBox.Show($"Hủy hóa đơn {hoaDon.MaHDB} đang lập?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            DatLaiForm();
        }

        private void DatLaiForm()
        {
            hoaDon = null;
            daThanhToan = false;
            khachMoi = false;
            tenKhachHienTai = null;
            txtNgayLap.Clear();
            txtMaKhach.ReadOnly = false;
            txtMaKhach.Clear();
            nudGiamGia.Value = 0;
            HienThiHoaDon();
            CapNhatTrangThai();
        }

        // ================== XUẤT EXCEL (ClosedXML) ==================

        private void XuatExcel()
        {
            if (!daThanhToan || hoaDon == null) return;

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = "HoaDon_" + hoaDon.MaHDB + ".xlsx"
            })
            {
                if (sfd.ShowDialog(this.TopLevelControl ?? this) != DialogResult.OK) return;

                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        IXLWorksheet ws = wb.Worksheets.Add("HoaDon");

                        ws.Cell("A1").Value = "NHÀ THUỐC";
                        ws.Range("A1:F1").Merge().Style.Font.SetBold().Font.SetFontSize(16)
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        ws.Cell("A2").Value = "HÓA ĐƠN BÁN LẺ";
                        ws.Range("A2:F2").Merge().Style.Font.SetBold().Font.SetFontSize(13)
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        ws.Cell("A4").Value = "Mã hóa đơn:"; ws.Cell("B4").Value = hoaDon.MaHDB;
                        ws.Cell("A5").Value = "Ngày bán:"; ws.Cell("B5").Value = ngayBan.ToString("dd/MM/yyyy HH:mm");
                        ws.Cell("A6").Value = "Nhân viên:"; ws.Cell("B6").Value = cboNhanVien.Text;
                        ws.Cell("A7").Value = "Khách hàng:"; ws.Cell("B7").Value = txtMaKhach.Text.Trim() + " - " + tenKhachHienTai;
                        ws.Range("A4:A7").Style.Font.SetBold();

                        int dongDau = 9;
                        string[] tieuDe = { "STT", "Mã thuốc", "Tên thuốc", "Số lượng", "Giá bán", "Thành tiền" };
                        for (int i = 0; i < tieuDe.Length; i++)
                            ws.Cell(dongDau, i + 1).Value = tieuDe[i];
                        ws.Range(dongDau, 1, dongDau, 6).Style.Font.SetBold()
                            .Fill.SetBackgroundColor(XLColor.LightGray)
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        int r = dongDau;
                        int stt = 1;
                        foreach (DongHoaDon d in hoaDon.Dong)
                        {
                            r++;
                            ws.Cell(r, 1).Value = stt++;
                            ws.Cell(r, 2).Value = d.MaThuoc;
                            ws.Cell(r, 3).Value = d.TenThuoc;
                            ws.Cell(r, 4).Value = d.SoLuong;
                            ws.Cell(r, 5).Value = (double)d.GiaBan;
                            ws.Cell(r, 6).Value = (double)d.ThanhTien;
                        }
                        IXLRange bang = ws.Range(dongDau, 1, r, 6);
                        bang.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        bang.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        ws.Range(dongDau + 1, 5, r, 6).Style.NumberFormat.Format = "#,##0";

                        r += 2;
                        ws.Cell(r, 5).Value = "Tiền hàng:";
                        ws.Cell(r, 6).Value = (double)hoaDon.TongTienHang;
                        r++;
                        ws.Cell(r, 5).Value = "Giảm giá (" + hoaDon.GiamGiaPhanTram.ToString("0.##") + "%):";
                        ws.Cell(r, 6).Value = (double)hoaDon.TienGiam;
                        r++;
                        ws.Cell(r, 5).Value = "TỔNG TIỀN:";
                        ws.Cell(r, 6).Value = (double)hoaDon.TongThanhToan;
                        ws.Range(r, 5, r, 6).Style.Font.SetBold().Font.SetFontColor(XLColor.Red);
                        ws.Range(r - 2, 6, r, 6).Style.NumberFormat.Format = "#,##0";

                        ws.Columns().AdjustToContents();
                        wb.SaveAs(sfd.FileName);
                    }

                    if (MessageBox.Show("Xuất hóa đơn thành công! Mở file ngay?", "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}