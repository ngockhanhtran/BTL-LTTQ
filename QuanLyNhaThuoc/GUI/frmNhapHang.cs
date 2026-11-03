using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    public partial class frmNhapHang : Form
    {
        private HoaDonNhapBLL hdnBLL = new HoaDonNhapBLL();
        private TextBox txtMaHDN, txtMaThuoc, txtSLNhap, txtDonGia, txtTuKhoaSearch;
        private DataGridView dgvKetQua;
        private Button btnThemNhap, btnTimKiemHDN;

        // ===== MỚI =====
        private TextBox txtMaNV, txtMaNCC;                  // Mã thủ kho, Mã nhà cung cấp
        private Label lblTenNV, lblTenNCC, lblTenThuoc;     // Tên tự hiển thị theo mã
        private Button btnImportExcel, btnTaiFileMau;

        private static readonly Color MauXanh = Color.FromArgb(40, 167, 69);
        private static readonly Color MauDo = Color.FromArgb(220, 53, 69);

        public frmNhapHang()
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.Text = "Quản lý Nhập Hàng & Kho";
            InitializeComponentCustom();
        }

        private void InitializeComponentCustom()
        {
            this.SuspendLayout();

            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Dock = DockStyle.Fill;

            Panel pnlLeft = new Panel() { Dock = DockStyle.Left, Width = 380, BackColor = Color.White, Padding = new Padding(15), AutoScroll = true };
            Panel pnlRight = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(15) };

            // --- TRÁI: FORM LẬP PHIẾU NHẬP ---
            Label lbl1 = new Label() { Text = "Lập Phiếu Nhập Hàng", Font = new Font("Segoe UI", 11, FontStyle.Bold), Left = 15, Top = 15, AutoSize = true };
            pnlLeft.Controls.Add(lbl1);

            int y = 60;
            AddInput(pnlLeft, "Mã HĐ Nhập:", ref txtMaHDN, ref y);
            lblTenNV = AddInputCoGoiY(pnlLeft, "Mã NV (Kho):", ref txtMaNV, ref y);       // MỚI: thủ kho phụ trách
            lblTenNCC = AddInputCoGoiY(pnlLeft, "Mã NCC:", ref txtMaNCC, ref y);          // MỚI: nhà cung cấp
            lblTenThuoc = AddInputCoGoiY(pnlLeft, "Mã Thuốc:", ref txtMaThuoc, ref y);    // thêm dòng tên/cảnh báo
            AddInput(pnlLeft, "Số Lượng:", ref txtSLNhap, ref y);
            AddInput(pnlLeft, "Đơn Giá:", ref txtDonGia, ref y);

            // Gõ mã -> tự hiển thị tên (hoặc cảnh báo nếu chưa có trong CSDL)
            txtMaNV.TextChanged += (s, e) => HienThiGoiY(txtMaNV, lblTenNV, hdnBLL.LayTenNhanVien,
                                                          "Không có nhân viên này trong CSDL");
            txtMaNCC.TextChanged += (s, e) => HienThiGoiY(txtMaNCC, lblTenNCC, hdnBLL.LayTenNhaCungCap,
                                                           "Nhà cung cấp chưa có trong CSDL");
            txtMaThuoc.TextChanged += (s, e) => HienThiGoiY(txtMaThuoc, lblTenThuoc, hdnBLL.LayTenThuoc,
                                                             "Cảnh báo: thuốc chưa từng bán (chưa có trong CSDL)");

            btnThemNhap = new Button() { Text = "Xác Nhận Nhập (Tăng Tồn Kho)", Left = 15, Top = y + 25, Width = 330, Height = 42, BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnThemNhap.Click += BtnThemNhap_Click;
            pnlLeft.Controls.Add(btnThemNhap);

            // --- TRÁI (MỚI): NHẬP TỪ FILE EXCEL ---
            int yEx = btnThemNhap.Bottom + 30;
            Label lblExcel = new Label() { Text = "Nhập Hàng Từ File Excel", Font = new Font("Segoe UI", 11, FontStyle.Bold), Left = 15, Top = yEx, AutoSize = true };
            Label lblExcelMoTa = new Label()
            {
                Text = "Nhập nhiều phiếu cùng lúc, cho phép cả thuốc mới chưa có trong CSDL (tự thêm thuốc và tăng tồn kho).",
                Left = 15,
                Top = yEx + 28,
                Width = 330,
                Height = 36,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.DimGray
            };
            btnImportExcel = new Button() { Text = "Import Phiếu Nhập (Excel)", Left = 15, Top = yEx + 68, Width = 330, Height = 42, BackColor = Color.FromArgb(33, 115, 70), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnTaiFileMau = new Button() { Text = "Tải file Excel mẫu", Left = 15, Top = yEx + 118, Width = 330, Height = 34, BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, Font = new Font("Segoe UI", 9), FlatStyle = FlatStyle.Flat };
            btnImportExcel.Click += BtnImportExcel_Click;
            btnTaiFileMau.Click += BtnTaiFileMau_Click;
            pnlLeft.Controls.Add(lblExcel);
            pnlLeft.Controls.Add(lblExcelMoTa);
            pnlLeft.Controls.Add(btnImportExcel);
            pnlLeft.Controls.Add(btnTaiFileMau);

            // --- PHẢI: TRA CỨU HÓA ĐƠN NHẬP (YÊU CẦU 5) ---
            // Vùng tìm kiếm dock Top, lưới dock Fill => lưới luôn vừa khít, không bị tràn khỏi panel
            Panel pnlSearch = new Panel() { Dock = DockStyle.Top, Height = 75 };
            Label lbl2 = new Label() { Text = "Tra cứu Hóa Đơn Nhập (Theo Mã/Nhân viên/NCC):", Font = new Font("Segoe UI", 10, FontStyle.Bold), Left = 0, Top = 0, AutoSize = true };
            txtTuKhoaSearch = new TextBox() { Left = 0, Top = 32, Width = 320, Font = new Font("Segoe UI", 10) };
            btnTimKiemHDN = new Button() { Text = "Tìm kiếm HĐN", Left = 330, Top = 30, Width = 120, Height = 30, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnTimKiemHDN.Click += BtnTimKiemHDN_Click;
            pnlSearch.Controls.Add(lbl2);
            pnlSearch.Controls.Add(txtTuKhoaSearch);
            pnlSearch.Controls.Add(btnTimKiemHDN);

            dgvKetQua = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                BackgroundColor = Color.White
            };

            pnlRight.Controls.Add(dgvKetQua);
            pnlRight.Controls.Add(pnlSearch);

            this.Controls.Add(pnlRight);
            this.Controls.Add(pnlLeft);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void AddInput(Panel pnl, string text, ref TextBox txt, ref int y)
        {
            Label lbl = new Label() { Text = text, Left = 15, Top = y, Width = 100, Font = new Font("Segoe UI", 9) };
            txt = new TextBox() { Left = 115, Top = y - 3, Width = 230, Font = new Font("Segoe UI", 10) };
            pnl.Controls.Add(lbl);
            pnl.Controls.Add(txt);
            y += 45;
        }

        // MỚI: ô nhập kèm 1 dòng chữ nhỏ bên dưới để hiện tên / cảnh báo
        private Label AddInputCoGoiY(Panel pnl, string text, ref TextBox txt, ref int y)
        {
            int yGoc = y;
            AddInput(pnl, text, ref txt, ref y);
            Label lblGoiY = new Label()
            {
                Left = 115,
                Top = yGoc + 24,
                Width = 240,
                Height = 32,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = Color.Gray
            };
            pnl.Controls.Add(lblGoiY);
            y += 28;
            return lblGoiY;
        }

        private void HienThiGoiY(TextBox txt, Label lbl, Func<string, string> traCuu, string thongBaoKhongCo)
        {
            string ma = txt.Text.Trim();
            if (ma == "") { lbl.Text = ""; return; }
            try
            {
                string ten = traCuu(ma);
                if (ten != null)
                {
                    lbl.Text = ten;
                    lbl.ForeColor = MauXanh;
                }
                else
                {
                    lbl.Text = thongBaoKhongCo;
                    lbl.ForeColor = MauDo;
                }
            }
            catch (Exception ex)
            {
                lbl.Text = "Lỗi tra cứu: " + ex.Message;
                lbl.ForeColor = Color.DarkRed;
            }
        }

        // ===================== NHẬP THỦ CÔNG =====================
        private void BtnThemNhap_Click(object sender, EventArgs e)
        {
            int slNhap;
            if (!int.TryParse(txtSLNhap.Text.Trim(), out slNhap) || slNhap <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên lớn hơn 0.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSLNhap.Focus();
                return;
            }
            decimal donGia;
            if (!DocTien(txtDonGia.Text, out donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGia.Focus();
                return;
            }

            try
            {
                string maHDN = txtMaHDN.Text.Trim();
                bool taoPhieuMoi;
                bool kq = hdnBLL.NhapHangThuCong(maHDN, txtMaNV.Text, txtMaNCC.Text, txtMaThuoc.Text, slNhap, donGia, out taoPhieuMoi);
                if (kq)
                {
                    MessageBox.Show("Nhập hàng thành công! Đã tự động tăng tồn kho và cập nhật giá mới.\n" +
                                    (taoPhieuMoi ? "Đã tạo phiếu nhập mới " : "Đã thêm thuốc vào phiếu ") + maHDN + ".",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Giữ lại Mã HĐN / NV / NCC để nhập tiếp thuốc khác vào cùng phiếu
                    txtMaThuoc.Clear();
                    txtSLNhap.Clear();
                    txtDonGia.Clear();
                    txtMaThuoc.Focus();

                    dgvKetQua.DataSource = hdnBLL.TimKiemHoaDonNhap(maHDN);
                }
            }
            catch (ArgumentException ex)        // lỗi nhập liệu (thuốc chưa có, NV/NCC sai...)
            {
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private static bool DocTien(string s, out decimal v)
        {
            s = (s ?? "").Trim();
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out v)
                || decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out v);
        }

        // ===================== IMPORT EXCEL =====================
        private void BtnImportExcel_Click(object sender, EventArgs e)
        {
            string duongDan;
            using (OpenFileDialog ofd = new OpenFileDialog() { Title = "Chọn file phiếu nhập", Filter = "File Excel (*.xlsx)|*.xlsx" })
            {
                if (ofd.ShowDialog(this) != DialogResult.OK) return;
                duongDan = ofd.FileName;
            }

            try
            {
                // Bước 1: đọc + kiểm tra, chưa ghi CSDL
                KetQuaDocExcel kq;
                Cursor = Cursors.WaitCursor;
                try { kq = hdnBLL.KiemTraFileExcel(duongDan); }
                finally { Cursor = Cursors.Default; }

                dgvKetQua.DataSource = kq.XemTruoc;     // xem trước nội dung file

                if (kq.Loi.Count > 0)
                {
                    string dsLoi = string.Join("\n", kq.Loi.Take(15));
                    if (kq.Loi.Count > 15) dsLoi += "\n... và " + (kq.Loi.Count - 15) + " lỗi khác.";
                    MessageBox.Show("File chưa hợp lệ, CHƯA có dữ liệu nào được nhập:\n\n" + dsLoi,
                                    "Lỗi dữ liệu Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string xacNhan = "Đọc được " + kq.DanhSach.Count + " dòng thuốc thuộc " + kq.SoPhieu + " phiếu nhập.\n" +
                                 "- Thuốc mới sẽ thêm vào CSDL: " + kq.SoThuocMoi + "\n" +
                                 "- Nhà cung cấp mới: " + kq.SoNCCMoi + "\n" +
                                 "- Tổng tiền: " + kq.TongTien.ToString("N0") + " đ\n\n" +
                                 "Xác nhận nhập kho?";
                if (MessageBox.Show(xacNhan, "Xác nhận import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                // Bước 2: ghi CSDL (1 transaction)
                QuanLyNhaThuoc.DAL.ThongKeImport tk;
                Cursor = Cursors.WaitCursor;
                try { tk = hdnBLL.ImportExcel(kq); }
                finally { Cursor = Cursors.Default; }

                MessageBox.Show("Import thành công!\n" +
                                "- " + tk.SoPhieu + " phiếu nhập\n" +
                                "- " + kq.DanhSach.Count + " dòng thuốc, đã tăng tồn kho\n" +
                                "- " + tk.SoThuocMoi + " thuốc mới được thêm\n" +
                                "- " + tk.SoNCCMoi + " nhà cung cấp mới",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dgvKetQua.DataSource = hdnBLL.TimKiemHoaDonNhap("");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi import: " + ex.Message + "\n\nKhông có dữ liệu nào được ghi vào CSDL.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTaiFileMau_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Title = "Lưu file Excel mẫu", Filter = "File Excel (*.xlsx)|*.xlsx", FileName = "MauPhieuNhap.xlsx" })
            {
                if (sfd.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    hdnBLL.TaoFileMauExcel(sfd.FileName);
                    if (MessageBox.Show("Đã tạo file mẫu. Mở file ngay?", "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        hdnBLL.MoFile(sfd.FileName);
                }
                catch (Exception ex) { MessageBox.Show("Không tạo được file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void BtnTimKiemHDN_Click(object sender, EventArgs e)
        {
            dgvKetQua.DataSource = hdnBLL.TimKiemHoaDonNhap(txtTuKhoaSearch.Text.Trim());
        }
    }
}