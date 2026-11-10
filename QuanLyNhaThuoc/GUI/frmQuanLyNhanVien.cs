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
    /// - Xem toàn bộ thông tin NV. [10/11/2026 lần 5] Mật khẩu lưu dạng BĂM (SHA-256 + salt) nên KHÔNG xem được nữa,
    ///   quản lý chỉ ĐẶT LẠI (ô "Mật khẩu mới", để trống = giữ nguyên); NV được đặt lại phải đổi khi đăng nhập.
    /// - NV đã bấm "Quên mật khẩu" -> dòng tô VÀNG, lên đầu danh sách. Chọn NV, nhập mật khẩu mới (hoặc
    ///   bấm "Mặc định"), bấm Lưu -> hết vàng.
    /// - Thêm / sửa / xóa NV. NV đã có hóa đơn không xóa được -> chuyển "Đã nghỉ việc" (khóa đăng nhập).
    /// Giao diện thiết kế bằng Designer (frmQuanLyNhanVien.Designer.cs).
    /// </summary>
    public partial class frmQuanLyNhanVien : Form
    {
        /// <summary>Phát ra sau khi thêm / sửa / xóa (form chính cập nhật số yêu cầu quên mật khẩu).</summary>
        public event EventHandler DuLieuThayDoi;

        private readonly NhanVienBLL nvBLL = new NhanVienBLL();

        private DataTable dtNhanVien;
        private bool dangThemMoi;
        private bool dangNapForm;        // chặn sự kiện khi đang đổ dữ liệu vào ô
        private bool maTuDong;           // mã NV đang là mã gợi ý (chưa sửa tay)
        private bool nvDangYeuCau;       // NV đang chọn có yêu cầu quên mật khẩu

        private static readonly Dictionary<string, string> TEN_COT = new Dictionary<string, string>
        {
            { "MaNV", "Mã NV" }, { "TenNV", "Họ tên" }, { "NgaySinh", "Ngày sinh" }, { "GioiTinh", "Giới tính" },
            { "DienThoai", "Điện thoại" }, { "DiaChi", "Địa chỉ" }, { "TenTrinhDo", "Trình độ" },
            { "TenChuyenMon", "Chuyên môn" }, { "TenVaiTro", "Vai trò" }, { "PhaiDoiMK", "Phải đổi MK" },
            { "TrangThai", "Trạng thái" }
        };
        private static readonly HashSet<string> COT_AN = new HashSet<string>
        {
            "MaTrinhDo", "MaChuyenMon", "MaVaiTro", "YeuCauDoiMK", "NgayYeuCauDoiMK", "NghiViec"
        };

        public frmQuanLyNhanVien()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            // Phần lấy từ hằng số trong BLL (không đặt cứng trong Designer)
            txtMatKhau.MaxLength = NhanVienBLL.DO_DAI_MK_TOI_DA;
            lblGoiYMK.Text = "Để trống = giữ mật khẩu cũ. Tối thiểu " + NhanVienBLL.DO_DAI_MK_TOI_THIEU + " ký tự";
            // Tag = màu gốc của nút (GiaoDien.BatTatNut dùng để bật lại)
            foreach (Button b in new[] { btnTim, btnMatKhauMacDinh, btnThem, btnLuu, btnXoa, btnLamMoi })
                b.Tag = b.BackColor;
            cboLoc.SelectedIndex = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            NapDanhMuc();
            TaiDanhSach(null);
        }

        // ================================================================
        // SỰ KIỆN GẮN TỪ DESIGNER
        // ================================================================
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TaiDanhSach(null); }
        }

        private void btnTim_Click(object sender, EventArgs e) { TaiDanhSach(null); }
        private void cboLoc_SelectedIndexChanged(object sender, EventArgs e) { ApDungLoc(); }
        private void lblYeuCau_Click(object sender, EventArgs e) { cboLoc.SelectedIndex = 3; }

        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            GiaoDien.DinhDangCot(dgv, TEN_COT, COT_AN, null, new[] { "NgaySinh" });
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (!dangThemMoi) HienThiDongDangChon();
        }

        private void txtMaNV_TextChanged(object sender, EventArgs e)
        {
            if (dangNapForm || !dangThemMoi) return;
            maTuDong = false;
        }

        // Thêm mới: đổi vai trò -> gợi ý lại mã NV theo vai trò (QLxxx / TKxxx / NVxx)
        private void cboVaiTro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dangNapForm || !dangThemMoi || !maTuDong) return;
            GoiYMaMoi();
        }

        private void btnMatKhauMacDinh_Click(object sender, EventArgs e)
        {
            txtMatKhau.Text = NhanVienBLL.MatKhauMacDinh(txtMaNV.Text);
        }

        private void btnThem_Click(object sender, EventArgs e) { BatDauThemMoi(); }
        private void btnLuu_Click(object sender, EventArgs e) { Luu(); }
        private void btnXoa_Click(object sender, EventArgs e) { Xoa(); }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboLoc.SelectedIndex = 0;
            TaiDanhSach(null);
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
        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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
                txtMatKhau.Clear();                    // [10/11/2026 lần 5] không đọc được mật khẩu đã băm
                lblMatKhau.Text = "Mật khẩu mới:";
                chkNghiViec.Checked = GiaoDien.GiaTri(r, "NghiViec") == "True";

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
                lblMatKhau.Text = "Mật khẩu (*):";
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
                    MessageBox.Show(this, "Đã thêm nhân viên " + nv.MaNV + ".\nMật khẩu đăng nhập lần đầu: " + nv.MatKhau +
                                          "\n(nhân viên sẽ phải đổi mật khẩu khi đăng nhập lần đầu)",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    bool xuLyYeuCau = false;
                    if (nvDangYeuCau)
                    {
                        if (!string.IsNullOrEmpty(nv.MatKhau))       // có nhập mật khẩu mới = cấp lại
                            xuLyYeuCau = true;
                        else
                        {
                            DialogResult kq = MessageBox.Show(this,
                                "Nhân viên này đang yêu cầu cấp lại mật khẩu nhưng ô \"Mật khẩu mới\" đang trống.\n\n" +
                                "Yes: vẫn đánh dấu đã xử lý (giữ mật khẩu cũ)\nNo: chỉ lưu thông tin, yêu cầu vẫn còn\nCancel: quay lại",
                                "Yêu cầu quên mật khẩu", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (kq == DialogResult.Cancel) return;
                            xuLyYeuCau = kq == DialogResult.Yes;
                        }
                    }
                    bool datLaiMK = !string.IsNullOrEmpty(nv.MatKhau);
                    nvBLL.Sua(nv, xuLyYeuCau);
                    MessageBox.Show(this,
                        datLaiMK ? "Đã đặt lại mật khẩu cho " + nv.MaNV + ": " + nv.MatKhau +
                                   "\nHãy báo cho nhân viên (mật khẩu chỉ hiện 1 lần này; NV phải đổi khi đăng nhập)."
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
