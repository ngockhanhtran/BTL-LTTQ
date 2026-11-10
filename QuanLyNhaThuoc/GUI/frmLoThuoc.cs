using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026 lần 5] Xem các LÔ của 1 thuốc (hoặc tất cả lô) - giao diện thiết kế bằng Designer.
    /// Mở từ trang Tra cứu thuốc (nút "Xem lô") và Danh mục thuốc.
    /// </summary>
    public partial class frmLoThuoc : Form
    {
        private readonly ThuocBLL thuocBLL = new ThuocBLL();
        private readonly string maThuoc;

        private static readonly Dictionary<string, string> TEN_COT = new Dictionary<string, string>
        {
            { "MaThuoc", "Mã thuốc" }, { "TenThuoc", "Tên thuốc" }, { "SoLo", "Số lô" }, { "NgaySX", "Ngày SX" },
            { "HanSD", "Hạn SD" }, { "SoNgayConLai", "Còn (ngày)" }, { "SLNhap", "SL nhập" }, { "SLCon", "SL còn" },
            { "DonGiaNhap", "Giá nhập" }, { "GiaTriTon", "Giá trị tồn" }, { "MaHDN", "Phiếu nhập" },
            { "NgayNhap", "Ngày nhập" }, { "ThuTuFEFO", "Thứ tự bán" }, { "TinhTrang", "Tình trạng" }
        };

        /// <param name="maThuoc">null = xem tất cả lô của mọi thuốc</param>
        public frmLoThuoc(string maThuoc, string tenThuoc)
        {
            InitializeComponent();
            this.maThuoc = string.IsNullOrWhiteSpace(maThuoc) ? null : maThuoc.Trim();
            lblTieuDe.Text = this.maThuoc == null ? "Tất cả lô thuốc" : "Lô thuốc: " + this.maThuoc + " - " + tenThuoc;
            Text = lblTieuDe.Text;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            TaiLo();
        }

        private void chkConHang_CheckedChanged(object sender, EventArgs e) { TaiLo(); }

        private void TaiLo()
        {
            try
            {
                DataTable dt = thuocBLL.LayLoThuoc(maThuoc, chkConHang.Checked);
                dgvLo.DataSource = dt;

                List<DataRow> ds = dt.AsEnumerable().ToList();
                int tong = ds.Sum(r => Convert.ToInt32(r["SLCon"]));
                int conHan = ds.Where(r => Convert.ToInt32(r["MucCanhBao"]) < 2 && Convert.ToInt32(r["SLCon"]) > 0)
                               .Sum(r => Convert.ToInt32(r["SLCon"]));
                int hetHan = ds.Count(r => Convert.ToInt32(r["MucCanhBao"]) == 2);
                lblTomTat.Text = ds.Count + " lô  •  tồn " + tong.ToString("N0") + "  •  bán được (còn hạn) " + conHan.ToString("N0") +
                                 (hetHan > 0 ? "  •  " + hetHan + " lô ĐÃ HẾT HẠN cần hủy" : "");
            }
            catch (Exception ex) { GiaoDien.ThongBaoLoi(this, ex, "Lỗi tải lô thuốc"); }
        }

        private void dgvLo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            List<string> an = new List<string> { "MaLo", "MucCanhBao" };
            if (maThuoc != null) { an.Add("MaThuoc"); an.Add("TenThuoc"); }
            GiaoDien.DinhDangCot(dgvLo, TEN_COT, an, new[] { "DonGiaNhap", "GiaTriTon" }, new[] { "NgaySX", "HanSD" });
            if (dgvLo.Columns.Contains("NgayNhap")) dgvLo.Columns["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void dgvLo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string muc = GiaoDien.GiaTri(dgvLo.Rows[e.RowIndex], "MucCanhBao");
            if (muc == "2")
            {
                e.CellStyle.BackColor = System.Drawing.Color.FromArgb(255, 199, 206);
                e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(156, 0, 6);
            }
            else if (muc == "1")
            {
                e.CellStyle.BackColor = GiaoDien.VangNen;
            }
            else if (GiaoDien.GiaTri(dgvLo.Rows[e.RowIndex], "SLCon") == "0")
            {
                e.CellStyle.ForeColor = GiaoDien.XamChu;
            }
        }
    }
}
