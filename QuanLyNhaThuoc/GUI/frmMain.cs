using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhaThuoc.GUI
{
    public partial class frmMain : Form
    {
        private Panel pnlSidebar;
        private Panel pnlHeader;
        private Panel pnlContainer;
        private Label lblTitle;
        private Label lblUser;

        // Giữ 1 bản duy nhất của 2 trang này để hóa đơn đang lập không bị mất khi chuyển trang
        private frmThuoc fThuoc;
        private frmBanHang fBanHang;

        public frmMain()
        {
            // Scale toàn bộ form theo DPI màn hình (thiết kế ở 96 DPI = 100%)
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            InitializeComponentCustom();
            this.Load += FrmMain_Load;
        }

        private void InitializeComponentCustom()
        {
            this.SuspendLayout();

            this.Text = "Hệ Thống Quản Lý Nhà Thuốc - KHANHCHANN EDITION";
            this.Size = new Size(1250, 780);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(244, 246, 249);

            // 1. Sidebar bên trái
            pnlSidebar = new Panel()
            {
                Dock = DockStyle.Left,
                Width = 240,
                BackColor = Color.FromArgb(20, 30, 48)
            };

            Label lblLogo = new Label()
            {
                Text = "💊 Nhà thuốc 1",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Left = 20,
                Top = 25,
                AutoSize = true
            };
            pnlSidebar.Controls.Add(lblLogo);

            AddSidebarButton("📦 Tra cứu Thuốc", 90, (s, e) => OpenChildForm(LayFormThuoc()));
            AddSidebarButton("🛒 Bán hàng (POS)", 155, (s, e) => OpenChildForm(LayFormBanHang()));
            AddSidebarButton("📥 Quản lý Nhập Hàng", 220, (s, e) => OpenChildForm(new frmNhapHang()));
            AddSidebarButton("📊 Báo cáo và Thống kê", 285, (s, e) => OpenChildForm(new frmBaoCao()));

            // 2. Header phía trên — dùng Dock thay cho tính toán Left thủ công
            pnlHeader = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White
            };

            lblTitle = new Label()
            {
                Text = "Quản lý danh mục Thuốc",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Dock = DockStyle.Left,
                AutoSize = true,
                Padding = new Padding(25, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };

            lblUser = new Label()
            {
                Text = "Xin chào, Dược sĩ",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Dock = DockStyle.Right,
                AutoSize = true,
                Padding = new Padding(0, 0, 30, 0),
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblUser);

            // 3. Container hiển thị Form con
            pnlContainer = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(244, 246, 249),
                Padding = new Padding(20)
            };

            // Thứ tự Dock: control thêm SAU được dock TRƯỚC.
            // => Sidebar chiếm trọn bên trái, Header nằm trên phần còn lại, Container lấp đầy phần còn lại.
            this.Controls.Add(pnlContainer);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlSidebar);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Không để form (sau khi scale DPI) vượt quá kích thước màn hình
            Rectangle wa = Screen.FromControl(this).WorkingArea;
            if (Width > wa.Width || Height > wa.Height)
            {
                Width = Math.Min(Width, wa.Width);
                Height = Math.Min(Height, wa.Height);
                Location = new Point(wa.Left + (wa.Width - Width) / 2, wa.Top + (wa.Height - Height) / 2);
            }
            this.MinimumSize = new Size(Math.Min(1000, wa.Width), Math.Min(650, wa.Height));

            // Mở form con sau khi form chính đã scale xong
            OpenChildForm(LayFormThuoc());
        }

        // ================== NỐI TRANG TRA CỨU <-> BÁN HÀNG ==================

        private frmThuoc LayFormThuoc()
        {
            if (fThuoc == null || fThuoc.IsDisposed)
            {
                fThuoc = new frmThuoc();
                // Bấm "Thanh toán với hóa đơn" ở trang Tra cứu -> nạp hóa đơn sang trang Bán hàng và chuyển trang
                fThuoc.MoTrangBanHang = hd =>
                {
                    frmBanHang bh = LayFormBanHang();
                    if (!bh.NapHoaDon(hd)) return false;
                    OpenChildForm(bh);
                    return true;
                };
            }
            return fThuoc;
        }

        private frmBanHang LayFormBanHang()
        {
            if (fBanHang == null || fBanHang.IsDisposed)
                fBanHang = new frmBanHang();
            return fBanHang;
        }

        private void AddSidebarButton(string text, int top, EventHandler onClick)
        {
            Button btn = new Button()
            {
                Text = text,
                Left = 15,
                Top = top,
                Width = 210,
                Height = 48,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            btn.Click += onClick;
            pnlSidebar.Controls.Add(btn);
        }

        private void OpenChildForm(Form childForm)
        {
            // Gỡ form đang hiển thị: 2 trang Tra cứu / Bán hàng chỉ ẩn đi (giữ dữ liệu),
            // các trang khác (Nhập hàng, Báo cáo) thì giải phóng như trước.
            for (int i = pnlContainer.Controls.Count - 1; i >= 0; i--)
            {
                Control c = pnlContainer.Controls[i];
                if (c == childForm) continue;

                c.Hide();
                pnlContainer.Controls.RemoveAt(i);
                if (c != fThuoc && c != fBanHang) c.Dispose();
            }

            if (!pnlContainer.Controls.Contains(childForm))
            {
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                pnlContainer.Controls.Add(childForm);
            }
            childForm.Show();
            childForm.BringToFront();
            lblTitle.Text = childForm.Text;
        }
    }
}