using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using ClosedXML.Excel;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    // [10/11/2026] Bổ sung:
    //   1. Ô QUÉT MÃ VẠCH (máy quét gõ mã + Enter, hoặc gõ tay mã vạch / mã thuốc) -> thêm thuốc vào hóa đơn.
    //      Chưa có hóa đơn thì tự tạo mã mới; hóa đơn trước đã thanh toán thì tự bắt đầu hóa đơn mới.
    //   2. TÍCH ĐIỂM: nhập mã khách cũ -> hiện điểm, hạng, số lần mua; ô "Dùng điểm" để đổi điểm lấy giảm giá;
    //      hiện số điểm sẽ được cộng. Quy định ở DTO/TichDiemDTO.cs.
    //   3. XUẤT PDF + XEM TRƯỚC & IN hóa đơn (InHoaDon.cs) sau khi thanh toán.
    //   4. Nhân viên mặc định = người đang đăng nhập; chỉ Quản lý được chọn NV khác.
    // Giao diện thiết kế bằng Designer (frmBanHang.Designer.cs) - mở bằng View Designer để kéo thả.
    public partial class frmBanHang : Form
    {
        private readonly HoaDonBanBLL hdbBLL = new HoaDonBanBLL();
        private readonly ThuocBLL thuocBLL = new ThuocBLL();

        private HoaDonTam hoaDon;          // hóa đơn nhận từ trang Tra cứu (hoặc tạo khi quét mã vạch)
        private bool daThanhToan;
        private DateTime ngayBan;
        private string tenKhachHienTai;    // tên khách CŨ lấy từ CSDL (null nếu chưa tra / khách mới)
        private bool khachMoi;             // true = mã khách chưa có trong CSDL, phải nhập tay tên

        private ThongTinKhach khachCu;     // [10/11/2026] điểm + số lần mua của khách cũ
        private bool dangCapNhatDiem;
        private int diemCongDaLuu;         // sau khi thanh toán
        private int diemConLaiDaLuu;
        private string hangKhiThanhToan;
        private readonly bool choChonNhanVien = PhienDangNhap.LaQuanLy || !PhienDangNhap.DaDangNhap;

        /// <summary>[10/11/2026] Còn hóa đơn đang lập chưa thanh toán (để cảnh báo khi đăng xuất).</summary>
        public bool DangCoHoaDonChuaThanhToan
        {
            get { return hoaDon != null && !daThanhToan && hoaDon.Dong.Count > 0; }
        }

        public frmBanHang()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            // Phần không đặt được trong Designer
            lblQuyDinh.Text = TichDiemBLL.MoTaQuyDinh();
            // Tag = màu gốc của nút, dùng khi bật lại nút (BatTatNut)
            foreach (Button b in new[] { btnThanhToan, btnLamMoi, btnXuatExcel, btnXuatPdf, btnIn, btnQuet })
                b.Tag = b.BackColor;

            LoadNhanVien();
            DatLaiForm();
        }

        // ================== SỰ KIỆN GẮN TỪ DESIGNER ==================

        private void nudDungDiem_ValueChanged(object sender, EventArgs e)
        {
            if (dangCapNhatDiem || hoaDon == null || daThanhToan) return;
            hoaDon.DiemSuDung = (int)nudDungDiem.Value;
            CapNhatTongTien();
        }

        private void nudGiamGia_ValueChanged(object sender, EventArgs e)
        {
            if (hoaDon == null || daThanhToan) return;
            hoaDon.GiamGiaPhanTram = nudGiamGia.Value;
            CapNhatTongTien();
        }

        // Tự tra tên khách sau khi ngừng gõ 0.4s (tmrKhach.Interval = 400 trong Designer)
        private void tmrKhach_Tick(object sender, EventArgs e)
        {
            tmrKhach.Stop();
            TraCuuKhachHang();
        }

        private void txtMaKhach_TextChanged(object sender, EventArgs e)
        {
            if (daThanhToan) return;
            tenKhachHienTai = null;
            khachMoi = false;
            khachCu = null;
            txtTenKhach.Clear();
            DatCheDoTenKhach(false);
            lblKhachInfo.Text = "";
            CapNhatTongTien();
            tmrKhach.Stop();
            tmrKhach.Start();
        }

        private void txtQuet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; QuetMa(); }
        }

        private void btnQuet_Click(object sender, EventArgs e) { QuetMa(); }

        // [10/11/2026 lần 5] mã đơn thuốc (bắt buộc khi hóa đơn có thuốc kê đơn)
        private void txtMaDonThuoc_TextChanged(object sender, EventArgs e)
        {
            if (hoaDon != null && !daThanhToan) hoaDon.MaDonThuoc = txtMaDonThuoc.Text.Trim();
        }

        private void CapNhatKeDon()
        {
            bool coKeDon = hoaDon != null && hoaDon.CoThuocKeDon;
            lblKeDon.Text = coKeDon ? "℞ Có thuốc kê đơn – BẮT BUỘC nhập mã đơn" : "Chỉ cần khi có thuốc kê đơn (℞)";
            lblKeDon.ForeColor = coKeDon ? Color.FromArgb(200, 35, 51) : Color.DimGray;
            txtMaDonThuoc.BackColor = daThanhToan ? Color.FromArgb(240, 242, 245)
                                    : coKeDon && txtMaDonThuoc.Text.Trim() == "" ? Color.FromArgb(255, 243, 205) : Color.White;
        }
        private void btnXuatExcel_Click(object sender, EventArgs e) { XuatExcel(); }
        private void btnXuatPdf_Click(object sender, EventArgs e) { XuatPdf(); }
        private void btnIn_Click(object sender, EventArgs e) { XemTruocVaIn(); }

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
            hoaDon.DiemSuDung = 0;
            try { hdbBLL.KiemTraKeDon(hoaDon); } catch { }      // [10/11/2026 lần 5] đánh dấu thuốc kê đơn theo CSDL
            txtMaDonThuoc.Text = hoaDon.MaDonThuoc ?? "";
            daThanhToan = false;
            khachMoi = false;
            khachCu = null;
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
                {
                    int i = dgvChiTiet.Rows.Add(stt++, d.MaThuoc, (d.KeDon ? "℞ " : "") + d.TenThuoc, d.SoLuong,
                                                d.GiaBan.ToString("N0"), d.ThanhTien.ToString("N0"));
                    if (d.KeDon) dgvChiTiet.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(200, 35, 51);   // [10/11/2026 lần 5]
                }
            }
            CapNhatTongTien();
            CapNhatKeDon();
        }

        // [10/11/2026] Cập nhật điểm (giới hạn đổi điểm theo tổng tiền) rồi tới các dòng tổng tiền
        private void CapNhatTongTien()
        {
            CapNhatDiem();

            decimal tienHang = hoaDon == null ? 0 : hoaDon.TongTienHang;
            decimal tienGiam = hoaDon == null ? 0 : hoaDon.TienGiam;
            decimal tienDiem = hoaDon == null ? 0 : hoaDon.TienGiamDiem;
            decimal tong = hoaDon == null ? 0 : hoaDon.TongThanhToan;

            lblTienHang.Text = Tien(tienHang);
            lblTienGiam.Text = (tienGiam > 0 ? "- " : "") + Tien(tienGiam);
            lblTienDiem.Text = tienDiem > 0 ? "- " + Tien(tienDiem) + "  (" + hoaDon.DiemSuDung + " điểm)" : Tien(0);
            lblTongTien.Text = Tien(tong);

            // Điểm dự kiến được cộng
            if (daThanhToan)
            {
                lblDiemSeCong.Text = "⭐ Đã cộng +" + diemCongDaLuu + " điểm. Khách hiện có " + diemConLaiDaLuu.ToString("N0") + " điểm.";
            }
            else if (hoaDon == null || hoaDon.Dong.Count == 0)
            {
                lblDiemSeCong.Text = "";
            }
            else if (khachCu == null && !khachMoi)
            {
                lblDiemSeCong.Text = "Nhập mã khách hàng để tích điểm cho hóa đơn này.";
            }
            else
            {
                int soLan = khachCu == null ? 0 : khachCu.SoLanMua;
                lblDiemSeCong.Text = "⭐ Dự kiến cộng +" + TichDiemBLL.TinhDiemCong(tong, soLan) + " điểm: " +
                                     TichDiemBLL.GiaiThichDiemCong(tong, soLan);
            }
        }

        // [10/11/2026] Hiện điểm của khách + giới hạn ô "Dùng điểm"
        private void CapNhatDiem()
        {
            dangCapNhatDiem = true;
            try
            {
                if (daThanhToan)
                {
                    nudDungDiem.Enabled = false;
                    lblDiemKhach.Text = "Điểm hiện có: " + diemConLaiDaLuu.ToString("N0") + "  •  Hạng " + hangKhiThanhToan;
                    return;
                }

                if (khachCu == null || hoaDon == null)
                {
                    nudDungDiem.Value = 0;
                    nudDungDiem.Maximum = 0;
                    nudDungDiem.Enabled = false;
                    if (hoaDon != null) hoaDon.DiemSuDung = 0;
                    lblDiemKhach.Text = khachMoi ? "Khách mới – bắt đầu tích điểm từ hóa đơn này."
                                      : khachCu != null ? "Điểm hiện có: " + khachCu.DiemTichLuy.ToString("N0")
                                      : "Nhập mã khách cũ để xem và đổi điểm.";
                    lblQuyDoi.Text = "";
                    return;
                }

                int toiDa = TichDiemBLL.DiemDoiToiDa(khachCu.DiemTichLuy, hoaDon.TongSauGiamGia);
                nudDungDiem.Value = Math.Min(nudDungDiem.Value, toiDa);
                nudDungDiem.Maximum = toiDa;
                nudDungDiem.Enabled = toiDa > 0;
                hoaDon.DiemSuDung = (int)nudDungDiem.Value;

                lblDiemKhach.Text = "Điểm hiện có: " + khachCu.DiemTichLuy.ToString("N0") + "  •  Hạng " +
                                    TichDiemBLL.TenHang(khachCu.SoLanMua) + " (" + khachCu.SoLanMua + " lần mua)";
                if (toiDa > 0)
                    lblQuyDoi.Text = "= -" + Tien(hoaDon.TienGiamDiem) + " (tối đa " + toiDa + ")";
                else if (khachCu.DiemTichLuy < QuyDinhTichDiem.DIEM_TOI_THIEU_DE_DOI)
                    lblQuyDoi.Text = "Cần ≥ " + QuyDinhTichDiem.DIEM_TOI_THIEU_DE_DOI + " điểm để đổi";
                else
                    lblQuyDoi.Text = "Chưa đổi được";
            }
            finally
            {
                dangCapNhatDiem = false;
            }
        }

        private void CapNhatTrangThai()
        {
            bool coHang = hoaDon != null && hoaDon.Dong.Count > 0;

            BatTatNut(btnThanhToan, coHang && !daThanhToan);
            BatTatNut(btnXuatExcel, daThanhToan);
            BatTatNut(btnXuatPdf, daThanhToan);
            BatTatNut(btnIn, daThanhToan);

            cboNhanVien.Enabled = !daThanhToan && choChonNhanVien;
            txtMaKhach.ReadOnly = daThanhToan;
            txtMaDonThuoc.ReadOnly = daThanhToan;                     // [10/11/2026 lần 5]
            CapNhatKeDon();
            DatCheDoTenKhach(khachMoi && !daThanhToan);
            nudGiamGia.Enabled = hoaDon != null && !daThanhToan;

            if (hoaDon == null)
            {
                lblTrangThai.Text = "Chưa có hóa đơn – thêm thuốc từ Tra cứu hoặc quét mã vạch.";
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
            CapNhatTongTien();
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
                ChonNhanVienDangNhap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên: " + ex.Message);
            }
        }

        // [10/11/2026] Mặc định chọn nhân viên đang đăng nhập
        private void ChonNhanVienDangNhap()
        {
            if (!PhienDangNhap.DaDangNhap || cboNhanVien.DataSource == null) return;
            DataTable dt = (DataTable)cboNhanVien.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (string.Equals(dt.Rows[i]["MaNV"].ToString().Trim(), PhienDangNhap.MaNV, StringComparison.OrdinalIgnoreCase))
                {
                    cboNhanVien.SelectedIndex = i;
                    return;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ChonNhanVienDangNhap();   // ComboBox có thể bị đặt lại vị trí khi gắn vào form
        }

        //Ô tên khách: chỉ cho nhập khi là khách mới
        private void DatCheDoTenKhach(bool choNhap)
        {
            txtTenKhach.ReadOnly = !choNhap;
            txtTenKhach.TabStop = choNhap;
            txtTenKhach.BackColor = choNhap ? Color.White : Color.FromArgb(240, 242, 245);
        }

        // Kiểm tra mã khách trong CSDL:
        //  - Đã tồn tại  -> tự điền tên, khóa ô tên, hiện điểm tích lũy [10/11/2026].
        //  - Chưa tồn tại -> mở ô tên cho nhập tay (khách mới, lưu khi thanh toán)
        private void TraCuuKhachHang()
        {
            string ma = txtMaKhach.Text.Trim();
            tenKhachHienTai = null;
            khachMoi = false;
            khachCu = null;
            txtTenKhach.Clear();
            DatCheDoTenKhach(false);
            lblKhachInfo.Text = "";
            if (ma == "") { CapNhatTongTien(); return; }

            try
            {
                ThongTinKhach kh = hdbBLL.LayThongTinKhach(ma);
                if (kh != null)
                {
                    khachCu = kh;
                    tenKhachHienTai = kh.TenKhach;
                    txtTenKhach.Text = kh.TenKhach;
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
            CapNhatTongTien();
        }

        // ================== [10/11/2026] QUÉT MÃ VẠCH ==================

        private void QuetMa()
        {
            string ma = txtQuet.Text.Trim();
            if (ma == "") { txtQuet.Focus(); return; }

            // Hóa đơn trước đã thanh toán -> bắt đầu hóa đơn mới cho khách tiếp theo
            if (daThanhToan) DatLaiForm();

            ThuocBan t;
            string loi;
            try
            {
                t = thuocBLL.TimThuocDeBan(ma, out loi);
            }
            catch (Exception ex)
            {
                BaoQuet("Lỗi: " + ex.Message, true);
                return;
            }
            if (t == null) { BaoQuet(loi, true); txtQuet.SelectAll(); txtQuet.Focus(); return; }

            if (hoaDon == null)
            {
                try
                {
                    hoaDon = new HoaDonTam(hdbBLL.SinhMaHoaDonMoi());
                    hoaDon.GiamGiaPhanTram = nudGiamGia.Value;
                    ngayBan = DateTime.Now;
                    txtNgayLap.Text = ngayBan.ToString("dd/MM/yyyy HH:mm");
                }
                catch (Exception ex)
                {
                    BaoQuet("Không sinh được mã hóa đơn: " + ex.Message, true);
                    return;
                }
            }

            int sl = (int)nudSLQuet.Value;
            int daCo = hoaDon.SoLuongCua(t.MaThuoc);
            if (daCo + sl > t.SLBanDuoc)    // [10/11/2026 lần 5] chỉ tính lô còn hạn
            {
                BaoQuet($"Không đủ hàng còn hạn \"{t.TenThuoc}\": còn {t.SLBanDuoc}, hóa đơn đã có {daCo}.", true);
                txtQuet.SelectAll();
                txtQuet.Focus();
                return;
            }

            hoaDon.ThemThuoc(t.MaThuoc, t.TenThuoc, sl, t.GiaBan, t.KeDon);
            HienThiHoaDon();
            CapNhatTrangThai();
            ChonDongThuoc(t.MaThuoc);
            BaoQuet($"✓ Đã thêm {sl} x {t.TenThuoc} ({Tien(t.GiaBan)})" + (t.KeDon ? " – ℞ thuốc kê đơn, nhập mã đơn thuốc" : ""), false);

            txtQuet.Clear();
            nudSLQuet.Value = 1;
            txtQuet.Focus();
        }

        private void BaoQuet(string thongBao, bool loi)
        {
            lblQuet.Text = thongBao;
            lblQuet.ForeColor = loi ? Color.FromArgb(200, 35, 51) : Color.FromArgb(40, 140, 60);
            if (loi) SystemSounds.Beep.Play();
        }

        private void ChonDongThuoc(string maThuoc)
        {
            foreach (DataGridViewRow r in dgvChiTiet.Rows)
            {
                if (Convert.ToString(r.Cells[colMaThuoc.Index].Value) == maThuoc)
                {
                    dgvChiTiet.CurrentCell = r.Cells[colTenThuoc.Index];
                    return;
                }
            }
        }

        // ================== SỰ KIỆN ==================

        private void DgvChiTiet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || hoaDon == null || daThanhToan) return;

            string ma = dgvChiTiet.Rows[e.RowIndex].Cells[colMaThuoc.Index].Value.ToString();
            string ten = dgvChiTiet.Rows[e.RowIndex].Cells[colTenThuoc.Index].Value.ToString();
            if (MessageBox.Show($"Bỏ thuốc \"{ten}\" khỏi hóa đơn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            hoaDon.XoaThuoc(ma);
            HienThiHoaDon();
            CapNhatTrangThai();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
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

            // [10/11/2026 lần 5] thuốc kê đơn -> phải có mã đơn thuốc
            try
            {
                hoaDon.MaDonThuoc = txtMaDonThuoc.Text.Trim();
                string loiKeDon = hdbBLL.KiemTraKeDon(hoaDon);
                if (loiKeDon != null)
                {
                    HienThiHoaDon();
                    MessageBox.Show(loiKeDon, "Thuốc kê đơn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaDonThuoc.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra thuốc kê đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // [10/11/2026] lấy lại điểm mới nhất của khách (có thể vừa mua ở máy khác)
                if (!khachMoi)
                {
                    int dungTruoc = hoaDon.DiemSuDung;
                    khachCu = hdbBLL.LayThongTinKhach(maKhach);
                    CapNhatTongTien();
                    if (hoaDon.DiemSuDung != dungTruoc)
                        MessageBox.Show($"Số điểm khách được dùng đã thay đổi thành {hoaDon.DiemSuDung} điểm. Tổng tiền: {Tien(hoaDon.TongThanhToan)}.",
                            "Điểm tích lũy", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                int diemCong, diemConLai;
                hdbBLL.LuuHoaDon(hoaDon, cboNhanVien.SelectedValue.ToString(), maKhach, tenKhachMoi, ngayBan,
                                 khachMoi ? null : khachCu, out diemCong, out diemConLai);

                int soLanDaMua = khachCu == null ? 0 : khachCu.SoLanMua;
                hangKhiThanhToan = TichDiemBLL.TenHang(soLanDaMua + 1);
                diemCongDaLuu = diemCong;
                diemConLaiDaLuu = diemConLai;

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

                MessageBox.Show($"Thanh toán hóa đơn {hoaDon.MaHDB} thành công!\nTồn kho đã được trừ.\n" +
                                $"Khách được cộng {diemCong} điểm, hiện có {diemConLai:N0} điểm.\n\n" +
                                "Bạn có thể xuất hóa đơn Excel / PDF hoặc in cho khách.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lưu hóa đơn thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
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
            khachCu = null;
            tenKhachHienTai = null;
            diemCongDaLuu = 0;
            diemConLaiDaLuu = 0;
            txtNgayLap.Clear();
            txtMaKhach.ReadOnly = false;
            txtMaKhach.Clear();
            txtMaDonThuoc.ReadOnly = false;
            txtMaDonThuoc.Clear();
            nudGiamGia.Value = 0;
            lblKhachInfo.Text = "";
            HienThiHoaDon();
            CapNhatTrangThai();
        }

        // ================== [10/11/2026] PDF / IN ==================

        private ThongTinInHoaDon TaoDuLieuIn()
        {
            ThongTinInHoaDon tt = new ThongTinInHoaDon
            {
                MaHDB = hoaDon.MaHDB,
                NgayBan = ngayBan,
                NhanVien = cboNhanVien.Text,
                KhachHang = txtMaKhach.Text.Trim() + " - " + tenKhachHienTai,
                HangThanhVien = hangKhiThanhToan,
                TienHang = hoaDon.TongTienHang,
                GiamGiaPhanTram = hoaDon.GiamGiaPhanTram,
                TienGiam = hoaDon.TienGiam,
                DiemSuDung = hoaDon.DiemSuDung,
                TienGiamDiem = hoaDon.TienGiamDiem,
                TongThanhToan = hoaDon.TongThanhToan,
                DiemCong = diemCongDaLuu,
                DiemConLai = diemConLaiDaLuu
            };
            tt.Dong.AddRange(hoaDon.Dong);
            return tt;
        }

        private void XuatPdf()
        {
            if (!daThanhToan || hoaDon == null) return;
            if (!InHoaDon.CoMayInPdf())
            {
                MessageBox.Show("Máy chưa có máy in ảo \"Microsoft Print to PDF\".\n" +
                                "Bật trong: Control Panel > Programs > Turn Windows features on or off > Microsoft Print to PDF.\n\n" +
                                "Hoặc dùng nút \"Xem trước & In\" rồi chọn máy in PDF khác.",
                                "Không xuất được PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = "HoaDon_" + hoaDon.MaHDB + ".pdf"
            })
            {
                if (sfd.ShowDialog(this.TopLevelControl ?? this) != DialogResult.OK) return;
                try
                {
                    Cursor = Cursors.WaitCursor;
                    bool xong = new InHoaDon(TaoDuLieuIn()).XuatPdf(sfd.FileName);
                    Cursor = Cursors.Default;
                    if (!xong)
                    {
                        MessageBox.Show("Đã gửi lệnh in sang \"Microsoft Print to PDF\" nhưng chưa thấy file được tạo.\nVui lòng kiểm tra lại thư mục đã chọn.",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (MessageBox.Show("Xuất hóa đơn PDF thành công! Mở file ngay?", "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Lỗi xuất PDF: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void XemTruocVaIn()
        {
            if (!daThanhToan || hoaDon == null) return;
            try
            {
                new InHoaDon(TaoDuLieuIn()).XemTruocVaIn(this.TopLevelControl ?? this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                        int dongTongDau = r;
                        ws.Cell(r, 5).Value = "Tiền hàng:";
                        ws.Cell(r, 6).Value = (double)hoaDon.TongTienHang;
                        r++;
                        ws.Cell(r, 5).Value = "Giảm giá (" + hoaDon.GiamGiaPhanTram.ToString("0.##") + "%):";
                        ws.Cell(r, 6).Value = (double)hoaDon.TienGiam;
                        r++;
                        // [10/11/2026] đổi điểm
                        ws.Cell(r, 5).Value = "Đổi điểm (" + hoaDon.DiemSuDung + " điểm):";
                        ws.Cell(r, 6).Value = (double)hoaDon.TienGiamDiem;
                        r++;
                        ws.Cell(r, 5).Value = "TỔNG TIỀN:";
                        ws.Cell(r, 6).Value = (double)hoaDon.TongThanhToan;
                        ws.Range(r, 5, r, 6).Style.Font.SetBold().Font.SetFontColor(XLColor.Red);
                        ws.Range(dongTongDau, 6, r, 6).Style.NumberFormat.Format = "#,##0";
                        r += 2;
                        ws.Cell(r, 1).Value = "Điểm tích lũy: +" + diemCongDaLuu + " điểm, hiện có " + diemConLaiDaLuu.ToString("N0") + " điểm.";
                        ws.Cell(r, 1).Style.Font.SetItalic();

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
