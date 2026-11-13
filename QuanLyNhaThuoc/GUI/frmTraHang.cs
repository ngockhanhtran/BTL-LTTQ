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
    /// [10/11/2026 lần 5] Trả hàng / hủy hóa đơn (giao diện thiết kế bằng Designer).
    /// - Bên trái: tìm hóa đơn theo mã HĐ / khách / khoảng ngày.
    /// - Bên phải: các dòng thuốc, nhập "SL TRẢ" (ô màu vàng) -> Lập phiếu trả hàng.
    /// - "Hủy cả hóa đơn" (chỉ quản lý): trả hết, hoàn điểm đã dùng, hóa đơn không còn tính vào báo cáo.
    /// Hàng trả về đúng LÔ đã xuất (cột "Lô đã xuất").
    /// </summary>
    public partial class frmTraHang : Form
    {
        private readonly TraHangBLL bll = new TraHangBLL();
        private string maHDBChon;
        private bool hoaDonDaHuy;
        private Font fontHuy;              // chữ gạch ngang cho hóa đơn đã hủy (tạo 1 lần)

        private static readonly Dictionary<string, string> TEN_COT_HD = new Dictionary<string, string>
        {
            { "MaHDB", "Mã HĐ" }, { "NgayBan", "Ngày bán" }, { "TenKhach", "Khách hàng" },
            { "TongTien", "Còn lại (đ)" }, { "TrangThai", "Trạng thái" }
        };
        private static readonly HashSet<string> COT_AN_HD = new HashSet<string>
        {
            "MaNV", "TenNV", "MaKhach", "TienDaHoan", "MaDonThuoc", "DaHuy"
        };
        private static readonly Dictionary<string, string> TEN_COT_PT = new Dictionary<string, string>
        {
            { "MaPTH", "Mã phiếu" }, { "NgayTra", "Ngày trả" }, { "TenNV", "Nhân viên" }, { "LyDo", "Lý do" },
            { "TienHoan", "Tiền hoàn" }, { "DiemTru", "Trừ điểm" }, { "DiemHoan", "Hoàn điểm" }, { "Loai", "Loại" },
            { "ThuocTra", "Thuốc trả" }
        };

        public frmTraHang()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            fontHuy = new Font(dgvHoaDon.DefaultCellStyle.Font ?? dgvHoaDon.Font, FontStyle.Strikeout);
            dtpTu.Value = DateTime.Today.AddDays(-QuyDinhTraHang.SO_NGAY_DUOC_TRA);
            dtpDen.Value = DateTime.Today;
            foreach (Button b in new[] { btnTim, btnTraHang, btnHuyHD, btnLamMoi }) b.Tag = b.BackColor;
            btnHuyHD.Visible = TraHangBLL.DuocHuyHoaDon;
            lblQuyDinh.Text = "• Nhân viên bán hàng nhận trả trong " + QuyDinhTraHang.SO_NGAY_DUOC_TRA + " ngày; quản lý không giới hạn, " +
                              "chỉ quản lý được hủy cả hóa đơn.\n• Tiền hoàn = đơn giá thực khách đã trả; điểm đã cộng bị trừ theo tỷ lệ.";
            HienHoaDon(null);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            TaiDanhSach(null);
        }

        // ================== DANH SÁCH HÓA ĐƠN ==================

        private void txtTim_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TaiDanhSach(null); }
        }

        private void btnTim_Click(object sender, EventArgs e) { TaiDanhSach(null); }

        private void btnLamMoi_Click(object sender, EventArgs e) { TaiDanhSach(maHDBChon); }

        private void TaiDanhSach(string maChon)
        {
            try
            {
                dgvHoaDon.DataSource = bll.TimHoaDon(txtTim.Text, dtpTu.Value, dtpDen.Value);
                GiaoDien.ChonDong(dgvHoaDon, "MaHDB", maChon);
                if (dgvHoaDon.Rows.Count == 0) HienHoaDon(null);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tìm hóa đơn"); }
        }

        private void dgvHoaDon_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            GiaoDien.DinhDangCot(dgvHoaDon, TEN_COT_HD, COT_AN_HD, new[] { "TongTien" }, null);
            if (dgvHoaDon.Columns.Contains("NgayBan")) dgvHoaDon.Columns["NgayBan"].DefaultCellStyle.Format = "dd/MM/yy HH:mm";
        }

        // Hóa đơn đã hủy: chữ xám gạch ngang; đã trả 1 phần: vàng
        private void dgvHoaDon_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string tt = GiaoDien.GiaTri(dgvHoaDon.Rows[e.RowIndex], "TrangThai");
            if (tt == "Đã hủy")
            {
                e.CellStyle.ForeColor = GiaoDien.XamChu;
                e.CellStyle.Font = fontHuy;
            }
            else if (tt == "Đã trả 1 phần")
            {
                e.CellStyle.BackColor = GiaoDien.VangNen;
            }
        }

        private void dgvHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow r = dgvHoaDon.CurrentRow;
            HienHoaDon(r == null ? null : GiaoDien.GiaTri(r, "MaHDB").Trim());
        }

        // ================== CHI TIẾT HÓA ĐƠN ==================

        private void HienHoaDon(string maHDB)
        {
            maHDBChon = string.IsNullOrEmpty(maHDB) ? null : maHDB;
            dgvChiTiet.Rows.Clear();
            dgvPhieuTra.DataSource = null;
            txtLyDo.Clear();
            hoaDonDaHuy = false;

            if (maHDBChon == null)
            {
                lblHoaDonChon.Text = "Chọn một hóa đơn ở bên trái";
                CapNhatNut();
                return;
            }

            DataGridViewRow r = dgvHoaDon.CurrentRow;
            hoaDonDaHuy = r != null && GiaoDien.GiaTri(r, "TrangThai") == "Đã hủy";
            lblHoaDonChon.Text = "Hóa đơn " + maHDBChon + (r == null ? "" :
                "  •  " + GiaoDien.GiaTri(r, "TenKhach") + "  •  " + GiaoDien.GiaTri(r, "TrangThai"));
            try
            {
                foreach (DataRow d in bll.LayChiTiet(maHDBChon).Rows)
                    dgvChiTiet.Rows.Add(d["MaThuoc"].ToString().Trim(), d["TenThuoc"], d["SLDaMua"], d["SLDaTra"], d["SLConLai"],
                                        Convert.ToDecimal(d["DonGiaThuc"]).ToString("N0"), d["LoDaXuat"], 0);
                dgvPhieuTra.DataSource = bll.LayPhieuTra(maHDBChon);
                GiaoDien.DinhDangCot(dgvPhieuTra, TEN_COT_PT, new[] { "MaNV" }, new[] { "TienHoan" }, null);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải hóa đơn"); }
            CapNhatNut();
        }

        private void CapNhatNut()
        {
            bool co = maHDBChon != null && !hoaDonDaHuy;
            colSLTra.ReadOnly = !co;
            GiaoDien.BatTatNut(btnTraHang, co);
            GiaoDien.BatTatNut(btnHuyHD, co);
            TinhTienHoan();
        }

        // Chỉ nhận số nguyên 0..SL còn lại
        private void dgvChiTiet_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != colSLTra.Index || e.RowIndex < 0) return;
            string s = Convert.ToString(e.FormattedValue).Trim();
            if (s == "") return;
            int sl;
            int conLai = Convert.ToInt32(dgvChiTiet.Rows[e.RowIndex].Cells[colConLai.Index].Value);
            if (!int.TryParse(s, out sl) || sl < 0 || sl > conLai)
            {
                MessageBox.Show(this, "Số lượng trả phải là số nguyên từ 0 đến " + conLai + ".", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void dgvChiTiet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell c = dgvChiTiet.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (e.ColumnIndex == colSLTra.Index && string.IsNullOrWhiteSpace(Convert.ToString(c.Value))) c.Value = 0;
            TinhTienHoan();
        }

        private List<DongTraHang> DocDongTra()
        {
            List<DongTraHang> ds = new List<DongTraHang>();
            foreach (DataGridViewRow r in dgvChiTiet.Rows)
            {
                int sl;
                if (!int.TryParse(Convert.ToString(r.Cells[colSLTra.Index].Value), out sl) || sl <= 0) continue;
                ds.Add(new DongTraHang
                {
                    MaThuoc = Convert.ToString(r.Cells[colMaThuoc.Index].Value),
                    TenThuoc = Convert.ToString(r.Cells[colTenThuoc.Index].Value),
                    SoLuong = sl
                });
            }
            return ds;
        }

        // Dự kiến (số chính xác do CSDL tính theo thành tiền còn lại của từng dòng)
        private void TinhTienHoan()
        {
            decimal tong = 0;
            foreach (DataGridViewRow r in dgvChiTiet.Rows)
            {
                int sl;
                decimal gia;
                if (int.TryParse(Convert.ToString(r.Cells[colSLTra.Index].Value), out sl) && sl > 0 &&
                    decimal.TryParse(Convert.ToString(r.Cells[colDonGia.Index].Value).Replace(",", "").Replace(".", ""), out gia))
                    tong += sl * gia;
            }
            lblTienHoan.Text = "Tiền hoàn dự kiến: " + tong.ToString("N0") + " đ";
        }

        // ================== TRẢ HÀNG / HỦY ==================

        private void btnTraHang_Click(object sender, EventArgs e)
        {
            if (maHDBChon == null) return;
            dgvChiTiet.EndEdit();
            List<DongTraHang> ds = DocDongTra();
            if (ds.Count == 0)
            {
                MessageBox.Show(this, "Nhập số lượng trả (cột SL TRẢ màu vàng) cho ít nhất 1 thuốc.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string ds1 = string.Join("\n", ds.ConvertAll(d => "  • " + d.TenThuoc + " x " + d.SoLuong));
            if (MessageBox.Show(this, "Lập phiếu trả hàng cho hóa đơn " + maHDBChon + ":\n" + ds1 + "\n\nTiếp tục?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                KetQuaTraHang kq = bll.TraHang(maHDBChon, ds, txtLyDo.Text);
                ThongBaoKetQua(kq);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi trả hàng"); }
        }

        private void btnHuyHD_Click(object sender, EventArgs e)
        {
            if (maHDBChon == null) return;
            if (MessageBox.Show(this, "HỦY TOÀN BỘ hóa đơn " + maHDBChon + "?\n\n- Hoàn lại tiền phần còn lại của hóa đơn\n" +
                    "- Trả thuốc về kho (đúng lô đã xuất)\n- Trừ điểm đã cộng, hoàn điểm khách đã dùng\n" +
                    "- Hóa đơn không còn tính vào doanh thu / báo cáo", "Xác nhận hủy hóa đơn",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            try
            {
                KetQuaTraHang kq = bll.HuyHoaDon(maHDBChon, txtLyDo.Text);
                ThongBaoKetQua(kq);
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi hủy hóa đơn"); }
        }

        private void ThongBaoKetQua(KetQuaTraHang kq)
        {
            string tb = "Đã lập phiếu " + kq.MaPTH + (kq.DaHuyHoaDon ? " - HÓA ĐƠN ĐÃ HỦY" : "") + ".\n\n" +
                        "Tiền hoàn cho khách: " + kq.TienHoan.ToString("N0") + " đ\n" +
                        "Điểm trừ lại: " + kq.DiemTru + "   •   Điểm hoàn: " + kq.DiemHoan + "\n" +
                        "Điểm khách còn: " + kq.DiemConLai.ToString("N0");
            if (kq.GhiChu.Count > 0) tb += "\n\n" + string.Join("\n", kq.GhiChu);
            MessageBox.Show(this, tb, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            TaiDanhSach(maHDBChon);
        }
    }
}
