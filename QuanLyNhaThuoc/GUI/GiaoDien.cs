using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [10/11/2026] Màu sắc + hàm tạo control dùng chung cho các form mới
    /// (cùng tông màu với các form cũ: xanh 0,122,204 - xanh lá 40,167,69 - xám 108,117,125).
    /// </summary>
    internal static class GiaoDien
    {
        public static readonly Color MauNen = Color.FromArgb(244, 246, 249);
        public static readonly Color MauSidebar = Color.FromArgb(20, 30, 48);
        public static readonly Color MauXanh = Color.FromArgb(0, 122, 204);
        public static readonly Color MauXanhDam = Color.FromArgb(0, 90, 160);
        public static readonly Color MauXanhLa = Color.FromArgb(40, 167, 69);
        public static readonly Color MauDo = Color.FromArgb(220, 53, 69);
        public static readonly Color MauXam = Color.FromArgb(108, 117, 125);
        public static readonly Color MauCam = Color.FromArgb(230, 126, 34);
        public static readonly Color MauChu = Color.FromArgb(33, 37, 41);
        public static readonly Color MauOChiDoc = Color.FromArgb(240, 242, 245);

        public static readonly Color VangNen = Color.FromArgb(255, 235, 156);
        public static readonly Color VangChu = Color.FromArgb(120, 85, 0);
        public static readonly Color VangChon = Color.FromArgb(230, 180, 20);
        public static readonly Color XamNen = Color.FromArgb(233, 236, 239);
        public static readonly Color XamChu = Color.FromArgb(134, 142, 150);

        public static Font TaoFont(float co, FontStyle kieu = FontStyle.Regular)
        {
            return new Font("Segoe UI", co, kieu);
        }

        public static Button TaoNut(string text, Color mau, int rong, int cao = 36)
        {
            Button b = new Button()
            {
                Text = text,
                Width = rong,
                Height = cao,
                BackColor = mau,
                ForeColor = Color.White,
                Tag = mau,
                Font = TaoFont(9.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        /// <summary>Bật / tắt nút, khi tắt chuyển màu xám (nút tạo bằng TaoNut).</summary>
        public static void BatTatNut(Button b, bool bat)
        {
            b.Enabled = bat;
            b.BackColor = bat ? (b.Tag is Color ? (Color)b.Tag : MauXanh) : Color.FromArgb(200, 203, 207);
        }

        public static Label TaoNhan(string text, int left, int top, int width = 120)
        {
            return new Label() { Text = text, Left = left, Top = top + 3, Width = width, Font = TaoFont(9.5F), ForeColor = MauChu };
        }

        public static TextBox TaoO(int left, int top, int width, bool chiDoc = false)
        {
            TextBox t = new TextBox()
            {
                Left = left,
                Top = top,
                Width = width,
                Font = TaoFont(10F),
                ReadOnly = chiDoc,
                TabStop = !chiDoc
            };
            if (chiDoc) t.BackColor = MauOChiDoc;
            return t;
        }

        public static void DatChiDoc(TextBox t, bool chiDoc)
        {
            t.ReadOnly = chiDoc;
            t.TabStop = !chiDoc;
            t.BackColor = chiDoc ? MauOChiDoc : Color.White;
        }

        public static DataGridView TaoLuoi()
        {
            DataGridView dgv = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            dgv.ColumnHeadersDefaultCellStyle.Font = TaoFont(9F, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = TaoFont(9.5F);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);
            return dgv;
        }

        /// <summary>Đổi tiêu đề cột sang tiếng Việt + ẩn cột + định dạng tiền / ngày.</summary>
        public static void DinhDangCot(DataGridView dgv, IDictionary<string, string> tieuDe,
                                       ICollection<string> cotAn = null,
                                       ICollection<string> cotTien = null,
                                       ICollection<string> cotNgay = null)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                string ten = string.IsNullOrEmpty(col.DataPropertyName) ? col.Name : col.DataPropertyName;
                string td;
                if (tieuDe != null && tieuDe.TryGetValue(ten, out td)) col.HeaderText = td;
                if (cotAn != null && cotAn.Contains(ten)) col.Visible = false;
                if (cotTien != null && cotTien.Contains(ten))
                {
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (cotNgay != null && cotNgay.Contains(ten))
                {
                    col.DefaultCellStyle.Format = "dd/MM/yyyy";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        /// <summary>Chọn lại dòng có giá trị cột cot = giaTri (sau khi nạp lại dữ liệu).</summary>
        public static void ChonDong(DataGridView dgv, string cot, string giaTri)
        {
            if (string.IsNullOrEmpty(giaTri) || !dgv.Columns.Contains(cot)) return;
            DataGridViewColumn cotHien = dgv.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
            if (cotHien == null) return;
            foreach (DataGridViewRow r in dgv.Rows)
            {
                object v = r.Cells[cot].Value;
                if (v != null && string.Equals(v.ToString().Trim(), giaTri.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    dgv.CurrentCell = r.Cells[cotHien.Index];
                    return;
                }
            }
        }

        public static string GiaTri(DataGridViewRow r, string cot)
        {
            if (r == null || r.DataGridView == null || !r.DataGridView.Columns.Contains(cot)) return "";
            object v = r.Cells[cot].Value;
            return (v == null || v == DBNull.Value) ? "" : v.ToString();
        }

        /// <summary>Đặt giá trị ComboBox (DataSource) theo ValueMember; không có thì bỏ chọn.</summary>
        public static void ChonCombo(ComboBox cbo, string giaTri)
        {
            if (string.IsNullOrEmpty(giaTri)) { cbo.SelectedIndex = -1; return; }
            cbo.SelectedValue = giaTri.Trim();
            if (cbo.SelectedValue == null || !string.Equals(cbo.SelectedValue.ToString().Trim(), giaTri.Trim(), StringComparison.OrdinalIgnoreCase))
                cbo.SelectedIndex = -1;
        }

        public static string GiaTriCombo(ComboBox cbo)
        {
            return cbo.SelectedValue == null ? null : cbo.SelectedValue.ToString().Trim();
        }

        public static void ThongBaoLoi(IWin32Window owner, Exception ex, string tieuDe = "Lỗi")
        {
            if (ex is ArgumentException)
                MessageBox.Show(owner, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show(owner, ex.Message, tieuDe, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
