using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [11/11/2026 lần 7] Quản lý cặp thuốc tương tác (chỉ quản lý). Trang Bán hàng dùng bảng này để
    /// cảnh báo đỏ khi hóa đơn có cả 2 thuốc của 1 cặp. DỮ LIỆU MẪU ĐỂ DEMO.
    /// </summary>
    public partial class frmTuongTacThuoc : Form
    {
        private readonly TuongTacThuocBLL bll = new TuongTacThuocBLL();
        private readonly ThuocBLL thuocBLL = new ThuocBLL();
        private bool dangNapLuoi;          // đang gán DataSource -> bỏ qua SelectionChanged

        private static readonly Dictionary<string, string> TEN_COT = new Dictionary<string, string>
        {
            { "MaThuoc1", "Mã 1" }, { "TenThuoc1", "Thuốc 1" }, { "MaThuoc2", "Mã 2" }, { "TenThuoc2", "Thuốc 2" },
            { "TenMucDo", "Mức độ" }, { "MoTa", "Mô tả" }, { "NgayTao", "Ngày tạo" }
        };

        public frmTuongTacThuoc()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            foreach (Button b in new[] { btnThem, btnSua, btnXoa, btnLamMoi, btnNapMau }) b.Tag = b.BackColor;
            cboMucDo.SelectedIndex = 1;
        }

        private async void frmTuongTacThuoc_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = await Task.Run(() => thuocBLL.LayDanhSachThuoc());
                if (!dt.Columns.Contains("HienThi"))
                    dt.Columns.Add("HienThi", typeof(string), "TenThuoc + '  [' + MaThuoc + ']'");
                GanDanhSachThuoc(cboThuoc1, dt);
                GanDanhSachThuoc(cboThuoc2, dt);
            }
            catch (Exception ex)
            {
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải danh sách thuốc");
            }
            await TaiDanhSachAsync();
            LamMoi();
        }

        // 2 ComboBox dùng 2 DataView riêng -> chọn ô này không làm đổi ô kia
        private static void GanDanhSachThuoc(ComboBox cbo, DataTable dt)
        {
            DataView v = new DataView(dt) { Sort = "TenThuoc" };
            cbo.DisplayMember = "HienThi";
            cbo.ValueMember = "MaThuoc";
            cbo.DataSource = v;
            cbo.SelectedIndex = -1;
        }

        private async Task TaiDanhSachAsync()
        {
            string tk = txtTim.Text.Trim();
            try
            {
                DataTable dt;
                using (KhungDangTai.Hien(dgvTuongTac, "Đang tải…"))
                    dt = await Task.Run(() => bll.DanhSach(tk));
                dangNapLuoi = true;
                dgvTuongTac.DataSource = dt;
                dgvTuongTac.ClearSelection();
                dangNapLuoi = false;
                lblSoCap.Text = dt.Rows.Count + " cặp tương tác";
            }
            catch (Exception ex)
            {
                lblSoCap.Text = "";
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải tương tác thuốc (hãy chạy lại file CSDL_QuanLyNhaThuoc.sql)");
            }
        }

        private void dgvTuongTac_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            GiaoDien.DinhDangCot(dgvTuongTac, TEN_COT, new[] { "MucDo" }, null, new[] { "NgayTao" });
            if (dgvTuongTac.Columns.Contains("MoTa")) dgvTuongTac.Columns["MoTa"].FillWeight = 260;
            foreach (DataGridViewRow r in dgvTuongTac.Rows)
            {
                int m = Convert.ToInt32(r.Cells["MucDo"].Value);
                Color mau = m >= 3 ? Color.FromArgb(200, 35, 51) : m == 2 ? Color.FromArgb(200, 120, 0) : Color.FromArgb(90, 98, 104);
                r.Cells["TenMucDo"].Style.ForeColor = mau;
                r.Cells["TenMucDo"].Style.Font = new Font(dgvTuongTac.Font, FontStyle.Bold);
            }
        }

        private void dgvTuongTac_SelectionChanged(object sender, EventArgs e)
        {
            if (dangNapLuoi || dgvTuongTac.SelectedRows.Count == 0 || !dgvTuongTac.Columns.Contains("MaThuoc1")) return;
            DataGridViewRow r = dgvTuongTac.SelectedRows[0];
            GiaoDien.ChonCombo(cboThuoc1, GiaoDien.GiaTri(r, "MaThuoc1"));
            GiaoDien.ChonCombo(cboThuoc2, GiaoDien.GiaTri(r, "MaThuoc2"));
            int m;
            if (int.TryParse(GiaoDien.GiaTri(r, "MucDo"), out m) && m >= 1 && m <= 3) cboMucDo.SelectedIndex = m - 1;
            txtMoTa.Text = GiaoDien.GiaTri(r, "MoTa");
            cboThuoc1.Enabled = cboThuoc2.Enabled = false;     // sửa: không đổi cặp, chỉ đổi mức / mô tả
            GiaoDien.BatTatNut(btnThem, false);
            GiaoDien.BatTatNut(btnSua, true);
            GiaoDien.BatTatNut(btnXoa, true);
        }

        private void LamMoi()
        {
            dangNapLuoi = true;
            dgvTuongTac.ClearSelection();
            dangNapLuoi = false;
            cboThuoc1.Enabled = cboThuoc2.Enabled = true;
            cboThuoc1.SelectedIndex = -1;
            cboThuoc2.SelectedIndex = -1;
            cboMucDo.SelectedIndex = 1;
            txtMoTa.Clear();
            GiaoDien.BatTatNut(btnThem, true);
            GiaoDien.BatTatNut(btnSua, false);
            GiaoDien.BatTatNut(btnXoa, false);
        }

        private void btnLamMoi_Click(object sender, EventArgs e) { LamMoi(); }

        private async void btnThem_Click(object sender, EventArgs e) { await LuuAsync(true); }
        private async void btnSua_Click(object sender, EventArgs e) { await LuuAsync(false); }

        private async Task LuuAsync(bool themMoi)
        {
            try
            {
                bll.Luu(GiaoDien.GiaTriCombo(cboThuoc1), GiaoDien.GiaTriCombo(cboThuoc2), cboMucDo.SelectedIndex + 1, txtMoTa.Text, themMoi);
                await TaiDanhSachAsync();
                LamMoi();
            }
            catch (Exception ex)
            {
                GiaoDien.ThongBaoLoi(this, ex);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            string a = GiaoDien.GiaTriCombo(cboThuoc1), b = GiaoDien.GiaTriCombo(cboThuoc2);
            if (MessageBox.Show(this, "Xóa cặp tương tác " + a + " - " + b + "?", "Xác nhận",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                bll.Xoa(a, b);
                await TaiDanhSachAsync();
                LamMoi();
            }
            catch (Exception ex)
            {
                GiaoDien.ThongBaoLoi(this, ex);
            }
        }

        private async void btnNapMau_Click(object sender, EventArgs e)
        {
            try
            {
                int n = await Task.Run(() => bll.NapDuLieuMau());
                MessageBox.Show(this, n > 0 ? "Đã nạp thêm " + n + " cặp tương tác mẫu." : "Không có cặp mẫu mới (đã nạp đủ, hoặc CSDL chưa có các thuốc trong dữ liệu mẫu - hãy chạy DuLieuDemo.sql).",
                                "Dữ liệu mẫu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await TaiDanhSachAsync();
            }
            catch (Exception ex)
            {
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi nạp dữ liệu mẫu");
            }
        }

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            tmrTim.Stop();
            tmrTim.Start();
        }

        private async void tmrTim_Tick(object sender, EventArgs e)
        {
            tmrTim.Stop();
            await TaiDanhSachAsync();
        }
    }
}
