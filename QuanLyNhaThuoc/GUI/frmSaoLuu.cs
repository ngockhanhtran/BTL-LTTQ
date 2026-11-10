using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026 lần 5] Sao lưu / khôi phục CSDL (chỉ quản lý) - giao diện thiết kế bằng Designer.
    /// File .bak do dịch vụ SQL Server ghi nên mặc định dùng thư mục Backup của SQL Server
    /// (thư mục khác như Desktop thường bị từ chối quyền).
    /// </summary>
    public partial class frmSaoLuu : Form
    {
        private readonly SaoLuuBLL bll = new SaoLuuBLL();

        /// <summary>Khôi phục xong -> form chính đăng xuất (dữ liệu đã thay đổi hoàn toàn).</summary>
        public event EventHandler DaKhoiPhuc;

        private static readonly Dictionary<string, string> TEN_COT = new Dictionary<string, string>
        {
            { "ThoiGian", "Thời gian" }, { "DuongDan", "File" }, { "KichThuocMB", "Kích thước (MB)" }, { "GhiChu", "Ghi chú" }
        };

        public frmSaoLuu()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            btnSaoLuu.Tag = btnSaoLuu.BackColor;
            btnKhoiPhuc.Tag = btnKhoiPhuc.BackColor;
            lblGhiChu.Text = "Lưu ý: lệnh BACKUP / RESTORE do dịch vụ SQL Server thực hiện trên MÁY CHỦ, nên thư mục phải nằm trên máy chạy SQL Server " +
                             "và tài khoản dịch vụ có quyền ghi. Báo lỗi \"Access is denied\" -> dùng thư mục Backup mặc định của SQL Server.";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            lblCSDL.Text = "CSDL: " + bll.TenCSDL + "  •  máy chủ: " + KetNoiBLL.MayChu;
            txtThuMuc.Text = bll.LayThuMucMacDinh() ?? @"C:\";
            txtTenFile.Text = bll.TenFileGoiY();
            TaiLichSu();
        }

        private void TaiLichSu()
        {
            try { dgvLichSu.DataSource = bll.LichSuSaoLuu(); }
            catch (Exception ex) { lblLichSu.Text = "Không đọc được lịch sử sao lưu: " + ex.Message; }
        }

        private void dgvLichSu_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            GiaoDien.DinhDangCot(dgvLichSu, TEN_COT, null, null, null);
            if (dgvLichSu.Columns.Contains("ThoiGian")) dgvLichSu.Columns["ThoiGian"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            if (dgvLichSu.Columns.Contains("DuongDan")) dgvLichSu.Columns["DuongDan"].FillWeight = 250;
        }

        private void dgvLichSu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            txtFileKP.Text = GiaoDien.GiaTri(dgvLichSu.Rows[e.RowIndex], "DuongDan");
        }

        private void btnChonThuMuc_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog f = new FolderBrowserDialog { Description = "Chọn thư mục lưu file sao lưu (trên máy chạy SQL Server)" })
            {
                if (Directory.Exists(txtThuMuc.Text)) f.SelectedPath = txtThuMuc.Text;
                if (f.ShowDialog(this.TopLevelControl ?? this) == DialogResult.OK) txtThuMuc.Text = f.SelectedPath;
            }
        }

        private void btnChonFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog f = new OpenFileDialog { Filter = "File sao lưu SQL Server (*.bak)|*.bak|Tất cả (*.*)|*.*" })
            {
                if (Directory.Exists(txtThuMuc.Text)) f.InitialDirectory = txtThuMuc.Text;
                if (f.ShowDialog(this.TopLevelControl ?? this) == DialogResult.OK) txtFileKP.Text = f.FileName;
            }
        }

        private void btnSaoLuu_Click(object sender, EventArgs e)
        {
            string duongDan;
            try { duongDan = Path.Combine(txtThuMuc.Text.Trim(), txtTenFile.Text.Trim()); }
            catch (ArgumentException) { MessageBox.Show(this, "Thư mục hoặc tên file có ký tự không hợp lệ."); return; }

            Cursor = Cursors.WaitCursor;
            try
            {
                bll.SaoLuu(duongDan);
                Cursor = Cursors.Default;
                MessageBox.Show(this, "Đã sao lưu CSDL vào:\n" + duongDan, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenFile.Text = bll.TenFileGoiY();
                txtFileKP.Text = duongDan;
                TaiLichSu();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi sao lưu");
            }
        }

        private void btnKhoiPhuc_Click(object sender, EventArgs e)
        {
            string file = txtFileKP.Text.Trim();
            if (file == "") { MessageBox.Show(this, "Chọn file sao lưu (.bak) cần khôi phục."); return; }
            if (MessageBox.Show(this, "KHÔI PHỤC CSDL từ file:\n" + file + "\n\nToàn bộ dữ liệu hiện tại sẽ bị GHI ĐÈ, mọi máy đang dùng " +
                    "sẽ bị ngắt kết nối và bạn sẽ được đăng xuất.\nNên sao lưu bản hiện tại trước.\n\nTiếp tục?", "Xác nhận khôi phục",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            Cursor = Cursors.WaitCursor;
            try
            {
                bll.KhoiPhuc(file);
                Cursor = Cursors.Default;
                MessageBox.Show(this, "Khôi phục thành công. Vui lòng đăng nhập lại.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DaKhoiPhuc?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi khôi phục");
            }
        }
    }
}
