using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026 lần 5] Cấu hình kết nối SQL Server (giao diện thiết kế bằng Designer).
    /// Giảng viên / bạn cùng nhóm chạy trên máy khác chỉ cần chọn đúng tên máy chủ, bấm "Thử kết nối" rồi "Lưu",
    /// không phải sửa DBConnection.cs. Cấu hình lưu trong %AppData%\QuanLyNhaThuoc\ketnoi.cfg.
    /// </summary>
    public partial class frmCauHinhKetNoi : Form
    {
        /// <param name="loi">Lý do không kết nối được khi mở chương trình (null nếu mở từ nút cấu hình).</param>
        public frmCauHinhKetNoi(string loi)
        {
            InitializeComponent();

            cboMayChu.Items.AddRange(KetNoiBLL.MayChuGoiY);
            cboMayChu.Text = KetNoiBLL.MayChu;
            txtCSDL.Text = KetNoiBLL.TenCSDL;
            rdoWindows.Checked = KetNoiBLL.XacThucWindows;
            rdoSql.Checked = !KetNoiBLL.XacThucWindows;
            txtTaiKhoan.Text = KetNoiBLL.TaiKhoan;
            txtMatKhauSql.Text = KetNoiBLL.MatKhau;
            lblFile.Text = "Cấu hình lưu tại: " + KetNoiBLL.DuongDanFile;
            CapNhatXacThuc();

            if (!string.IsNullOrEmpty(loi))
            {
                lblLoi.Text = "⚠ Chưa kết nối được CSDL:\n" + loi;
                lblLoi.Visible = true;
            }
        }

        private void rdoWindows_CheckedChanged(object sender, EventArgs e)
        {
            CapNhatXacThuc();
        }

        private void CapNhatXacThuc()
        {
            bool sql = !rdoWindows.Checked;
            txtTaiKhoan.Enabled = sql;
            txtMatKhauSql.Enabled = sql;
        }

        private void btnThu_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            lblKetQua.Text = "Đang thử kết nối...";
            lblKetQua.ForeColor = Color.DimGray;
            lblKetQua.Refresh();
            string loi = KetNoiBLL.ThuKetNoi(cboMayChu.Text, txtCSDL.Text, rdoWindows.Checked, txtTaiKhoan.Text, txtMatKhauSql.Text);
            Cursor = Cursors.Default;
            if (loi == null)
            {
                lblKetQua.Text = "✓ Kết nối thành công!";
                lblKetQua.ForeColor = GiaoDien.MauXanhLa;
            }
            else
            {
                lblKetQua.Text = "✗ Không kết nối được (xem chi tiết)";
                lblKetQua.ForeColor = GiaoDien.MauDo;
                MessageBox.Show(this, loi, "Thử kết nối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string loi = KetNoiBLL.ThuKetNoi(cboMayChu.Text, txtCSDL.Text, rdoWindows.Checked, txtTaiKhoan.Text, txtMatKhauSql.Text);
            Cursor = Cursors.Default;
            if (loi != null &&
                MessageBox.Show(this, loi + "\n\nVẫn lưu cấu hình này?", "Chưa kết nối được",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            try
            {
                KetNoiBLL.Luu(cboMayChu.Text, txtCSDL.Text, rdoWindows.Checked, txtTaiKhoan.Text, txtMatKhauSql.Text);
                DialogResult = loi == null ? DialogResult.OK : DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                GiaoDien.ThongBaoLoi(this, ex, "Lỗi lưu cấu hình");
            }
        }
    }
}
