using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [11/11/2026 lần 7] Vòng xoay "Đang tải…" phủ lên 1 vùng (bảng, biểu đồ) trong lúc chờ dữ liệu.
    /// Dùng với async/await: dữ liệu tải ở luồng nền (Task.Run), giao diện KHÔNG bị đơ,
    /// các nút khác vẫn bấm được (chỉ vùng đang tải bị che).
    ///   using (KhungDangTai.Hien(dgvBaoCao, "Đang tải báo cáo…"))
    ///   {
    ///       dt = await Task.Run(() => bll.LayBaoCao(...));
    ///   }
    /// </summary>
    internal sealed class KhungDangTai : Control
    {
        private readonly Control vung;
        private readonly Timer dongHo = new Timer { Interval = 40 };
        private readonly string thongBao;
        private int goc;
        private readonly DateTime batDau = DateTime.Now;

        private KhungDangTai(Control vung, string thongBao)
        {
            this.vung = vung;
            this.thongBao = thongBao;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            BackColor = Color.FromArgb(248, 249, 251);
            Cursor = Cursors.AppStarting;
            dongHo.Tick += (s, e) => { goc = (goc + 12) % 360; Invalidate(); };
        }

        /// <summary>Che vùng đang tải. Dispose (cuối khối using) thì gỡ ra.</summary>
        public static IDisposable Hien(Control vung, string thongBao = "Đang tải…")
        {
            if (vung == null || vung.Parent == null) return new KhongLamGi();
            KhungDangTai k = new KhungDangTai(vung, thongBao);
            k.Bounds = vung.Bounds;
            k.Anchor = vung.Anchor;
            vung.Parent.Controls.Add(k);
            k.BringToFront();
            vung.LocationChanged += k.DongBoViTri;
            vung.SizeChanged += k.DongBoViTri;
            k.dongHo.Start();
            return new Go(k);
        }

        private void DongBoViTri(object sender, EventArgs e)
        {
            if (!IsDisposed) Bounds = vung.Bounds;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int r = 18;
            Point tam = new Point(Width / 2, Height / 2 - 14);
            Rectangle vong = new Rectangle(tam.X - r, tam.Y - r, r * 2, r * 2);
            using (Pen nen = new Pen(Color.FromArgb(220, 225, 232), 5))
            using (Pen chay = new Pen(Color.FromArgb(0, 122, 204), 5) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                g.DrawEllipse(nen, vong);
                g.DrawArc(chay, vong, goc, 100);
            }

            double giay = (DateTime.Now - batDau).TotalSeconds;
            string chu = thongBao + (giay >= 1 ? "  (" + giay.ToString("0") + " giây)" : "");
            using (Font f = new Font("Segoe UI", 10F))
            {
                SizeF kt = g.MeasureString(chu, f);
                g.DrawString(chu, f, Brushes.DimGray, (Width - kt.Width) / 2, tam.Y + r + 10);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dongHo.Stop();
                dongHo.Dispose();
                vung.LocationChanged -= DongBoViTri;
                vung.SizeChanged -= DongBoViTri;
            }
            base.Dispose(disposing);
        }

        private sealed class Go : IDisposable
        {
            private KhungDangTai k;
            public Go(KhungDangTai k) { this.k = k; }
            public void Dispose()
            {
                if (k == null) return;
                if (!k.IsDisposed)
                {
                    if (k.Parent != null) k.Parent.Controls.Remove(k);
                    k.Dispose();
                }
                k = null;
            }
        }

        private sealed class KhongLamGi : IDisposable { public void Dispose() { } }
    }
}
