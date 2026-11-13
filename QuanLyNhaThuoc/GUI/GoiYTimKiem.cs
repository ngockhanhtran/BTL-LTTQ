using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [11/11/2026 lần 7] Gợi ý tự động khi gõ (AutoComplete) cho 1 ô TextBox - giống ô tìm kiếm của Google.
    /// - Gõ từ 2 ký tự, ngừng gõ 0,25 giây -> hỏi CSDL (luồng nền, không đơ) -> hiện danh sách dưới ô.
    /// - Tìm không dấu: "para" ra Paracetamol, "giam dau" ra thuốc giảm đau.
    /// - Phím: Mũi tên lên / xuống chọn, Enter chọn, Esc đóng. Bấm chuột để chọn.
    /// Ô đã có sự kiện KeyDown riêng thì gọi XuLyPhim(e) ở ĐẦU sự kiện đó (để Enter chọn gợi ý thay vì xử lý cũ).
    /// </summary>
    internal sealed class GoiYTimKiem : IDisposable
    {
        private readonly TextBox o;
        private readonly Func<string, List<GoiYThuoc>> layGoiY;
        private readonly ListBox lst;
        private readonly Timer cho = new Timer { Interval = 250 };
        private int phien;                 // bỏ kết quả cũ khi người dùng đã gõ tiếp
        private bool dangDatChu;

        public event Action<GoiYThuoc> DaChon;

        public GoiYTimKiem(TextBox o, Func<string, List<GoiYThuoc>> layGoiY, bool tuXuLyPhim)
        {
            this.o = o;
            this.layGoiY = layGoiY;

            lst = new ListBox
            {
                Visible = false,
                TabStop = false,
                IntegralHeight = false,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 24
            };
            lst.DrawItem += Lst_DrawItem;
            lst.MouseClick += (s, e) => ChonDangTro();
            lst.MouseMove += (s, e) =>
            {
                int i = lst.IndexFromPoint(e.Location);
                if (i >= 0 && i != lst.SelectedIndex) lst.SelectedIndex = i;
            };

            o.TextChanged += O_TextChanged;
            o.Leave += (s, e) => o.BeginInvoke((Action)AnNeuMatFocus);
            lst.Leave += (s, e) => o.BeginInvoke((Action)AnNeuMatFocus);
            if (tuXuLyPhim) o.KeyDown += (s, e) => XuLyPhim(e);
            cho.Tick += Cho_Tick;
        }

        public bool DangMo { get { return lst.Visible && lst.Items.Count > 0; } }

        /// <summary>Đặt chữ vào ô mà không bật gợi ý.</summary>
        public void DatChu(string s)
        {
            dangDatChu = true;
            try { o.Text = s; o.SelectionStart = o.TextLength; }
            finally { dangDatChu = false; }
            An();
        }

        /// <summary>Trả về true nếu phím đã được xử lý (không cho sự kiện cũ chạy tiếp).</summary>
        public bool XuLyPhim(KeyEventArgs e)
        {
            if (!DangMo) return false;
            switch (e.KeyCode)
            {
                case Keys.Down:
                    lst.SelectedIndex = Math.Min(lst.Items.Count - 1, lst.SelectedIndex + 1);
                    e.SuppressKeyPress = e.Handled = true;
                    return true;
                case Keys.Up:
                    lst.SelectedIndex = Math.Max(0, lst.SelectedIndex - 1);
                    e.SuppressKeyPress = e.Handled = true;
                    return true;
                case Keys.Enter:
                    if (lst.SelectedIndex < 0) { An(); return false; }   // chưa chọn dòng nào -> Enter xử lý như cũ
                    ChonDangTro();
                    e.SuppressKeyPress = e.Handled = true;
                    return true;
                case Keys.Escape:
                    An();
                    e.SuppressKeyPress = e.Handled = true;
                    return true;
            }
            return false;
        }

        private void O_TextChanged(object sender, EventArgs e)
        {
            cho.Stop();
            if (dangDatChu || !o.Focused) { An(); return; }
            if (o.Text.Trim().Length < 2) { phien++; An(); return; }
            cho.Start();
        }

        private async void Cho_Tick(object sender, EventArgs e)
        {
            cho.Stop();
            int phienNay = ++phien;
            string tuKhoa = o.Text.Trim();
            List<GoiYThuoc> ds;
            try
            {
                ds = await Task.Run(() => layGoiY(tuKhoa));
            }
            catch
            {
                return;            // lỗi CSDL: chỉ bỏ gợi ý, không làm phiền người dùng
            }
            if (phienNay != phien || o.IsDisposed || !o.Focused) return;
            HienDanhSach(ds);
        }

        private void HienDanhSach(List<GoiYThuoc> ds)
        {
            if (ds == null || ds.Count == 0) { An(); return; }

            Form f = o.FindForm();
            if (f == null) return;
            if (lst.Parent != f) { f.Controls.Add(lst); }

            lst.BeginUpdate();
            lst.Items.Clear();
            foreach (GoiYThuoc g in ds) lst.Items.Add(g);
            lst.SelectedIndex = -1;
            lst.EndUpdate();

            Point p = f.PointToClient(o.Parent.PointToScreen(new Point(o.Left, o.Bottom)));
            int rong = Math.Max(o.Width, 420);
            if (p.X + rong > f.ClientSize.Width) rong = Math.Max(o.Width, f.ClientSize.Width - p.X - 4);
            lst.Bounds = new Rectangle(p.X, p.Y + 1, rong, Math.Min(ds.Count, 8) * lst.ItemHeight + 4);
            lst.Visible = true;
            lst.BringToFront();
        }

        private void Lst_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            GoiYThuoc g = (GoiYThuoc)lst.Items[e.Index];
            bool chon = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            using (SolidBrush nen = new SolidBrush(chon ? Color.FromArgb(0, 122, 204) : Color.White))
                e.Graphics.FillRectangle(nen, e.Bounds);

            Color mauTen = chon ? Color.White : (g.KeDon ? Color.FromArgb(200, 35, 51) : Color.FromArgb(33, 37, 41));
            Color mauPhu = chon ? Color.FromArgb(220, 235, 250) : (g.SLBanDuoc > 0 ? Color.DimGray : Color.Firebrick);
            string ten = (g.KeDon ? "℞ " : "") + g.TenThuoc;
            string phu = g.GiaBan.ToString("N0") + " đ · " + (g.SLBanDuoc > 0 ? "còn " + g.SLBanDuoc.ToString("N0") : "hết hàng") + " · " + g.MaThuoc;

            using (Font dam = new Font(e.Font, FontStyle.Bold))
            {
                TextRenderer.DrawText(e.Graphics, ten, dam, new Rectangle(e.Bounds.X + 6, e.Bounds.Y, e.Bounds.Width - 200, e.Bounds.Height),
                                      mauTen, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.Left);
            }
            TextRenderer.DrawText(e.Graphics, phu, e.Font, new Rectangle(e.Bounds.Right - 196, e.Bounds.Y, 190, e.Bounds.Height),
                                  mauPhu, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void ChonDangTro()
        {
            GoiYThuoc g = lst.SelectedItem as GoiYThuoc;
            An();
            o.Focus();
            if (g != null && DaChon != null) DaChon(g);
        }

        private void AnNeuMatFocus()
        {
            if (!o.Focused && !lst.Focused) An();
        }

        private void An()
        {
            cho.Stop();
            if (lst.Visible) lst.Visible = false;
        }

        public void Dispose()
        {
            cho.Stop();
            cho.Dispose();
            lst.Dispose();
        }
    }
}
