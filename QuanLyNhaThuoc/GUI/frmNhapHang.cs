using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    // [10/11/2026 lần 5] Giao diện thiết kế bằng Designer (frmNhapHang.Designer.cs).
    //   Mỗi lần nhập tạo 1 LÔ thuốc: thêm Số lô, Ngày SX, Hạn SD (bắt buộc) -> bán theo FEFO.
    public partial class frmNhapHang : Form
    {
        private HoaDonNhapBLL hdnBLL = new HoaDonNhapBLL();

        private static readonly Color MauXanh = Color.FromArgb(40, 167, 69);
        private static readonly Color MauDo = Color.FromArgb(220, 53, 69);

        public frmNhapHang()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            dtpHanSD.Value = DateTime.Today.AddYears(2);
            dtpNgaySX.Value = DateTime.Today;
            dtpNgaySX.Checked = false;

            // [10/11/2026] Mã NV phụ trách = người đang đăng nhập. Thủ kho không sửa được, quản lý được chọn người khác.
            if (QuanLyNhaThuoc.DTO.PhienDangNhap.DaDangNhap)
            {
                txtMaNV.Text = QuanLyNhaThuoc.DTO.PhienDangNhap.MaNV;
                if (QuanLyNhaThuoc.DTO.PhienDangNhap.LaThuKho)
                {
                    txtMaNV.ReadOnly = true;
                    txtMaNV.BackColor = Color.FromArgb(240, 242, 245);
                }
            }
        }

        // Gõ mã -> tự hiển thị tên (hoặc cảnh báo nếu chưa có trong CSDL)
        private void txtMaNV_TextChanged(object sender, EventArgs e)
        {
            HienThiGoiY(txtMaNV, lblTenNV, hdnBLL.LayTenNhanVien, "Không có nhân viên này trong CSDL");
        }

        private void txtMaNCC_TextChanged(object sender, EventArgs e)
        {
            HienThiGoiY(txtMaNCC, lblTenNCC, hdnBLL.LayTenNhaCungCap, "Nhà cung cấp chưa có trong CSDL");
        }

        private void txtMaThuoc_TextChanged(object sender, EventArgs e)
        {
            HienThiGoiY(txtMaThuoc, lblTenThuoc, hdnBLL.LayTenThuoc, "Cảnh báo: thuốc chưa từng bán (chưa có trong CSDL)");
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
                bool kq = hdnBLL.NhapHangThuCong(maHDN, txtMaNV.Text, txtMaNCC.Text, txtMaThuoc.Text, slNhap, donGia,
                                                 txtSoLo.Text, dtpNgaySX.Checked ? dtpNgaySX.Value.Date : (DateTime?)null,
                                                 dtpHanSD.Value.Date, out taoPhieuMoi);       // [10/11/2026 lần 5] lô
                if (kq)
                {
                    MessageBox.Show("Nhập hàng thành công! Đã tạo lô mới (HSD " + dtpHanSD.Value.ToString("dd/MM/yyyy") +
                                    "), tăng tồn kho và cập nhật giá mới.\n" +
                                    (taoPhieuMoi ? "Đã tạo phiếu nhập mới " : "Đã thêm thuốc vào phiếu ") + maHDN + ".",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Giữ lại Mã HĐN / NV / NCC để nhập tiếp thuốc khác vào cùng phiếu
                    txtMaThuoc.Clear();
                    txtSLNhap.Clear();
                    txtDonGia.Clear();
                    txtSoLo.Clear();
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
                                 "- Công dụng mới: " + kq.SoCongDungMoi + "\n" +                     // [10/11/2026]
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
                                "- " + tk.SoNCCMoi + " nhà cung cấp mới\n" +
                                "- " + tk.SoCongDungMoi + " công dụng mới",                          // [10/11/2026]
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