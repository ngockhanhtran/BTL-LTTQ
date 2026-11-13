using System;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;

namespace QuanLyNhaThuoc.GUI
{
    /// <summary>
    /// [11/11/2026 lần 7] Quét mã vạch bằng webcam laptop - không cần máy quét.
    /// - AForge.Video.DirectShow: lấy khung hình từ webcam (luồng riêng của AForge).
    /// - ZXing.Net: đọc mã vạch / QR trong khung hình (luồng nền, ~6 lần / giây) -> giao diện không giật.
    /// - Đọc được mã -> gọi XuLyMa (trang Bán hàng thêm thuốc vào hóa đơn), kêu "bíp".
    ///   Cùng 1 mã chỉ nhận lại sau 2,5 giây (tránh 1 lần đưa hộp thuốc mà thêm nhiều lần).
    /// </summary>
    public partial class frmQuetCamera : Form
    {
        /// <summary>Xử lý mã quét được, trả về thông báo hiện lên (vd "✓ Đã thêm 1 x Panadol"). Lỗi thì bắt đầu bằng "✗".</summary>
        public Func<string, string> XuLyMa { get; set; }

        /// <summary>Mã quét được gần nhất (khi không quét liên tục).</summary>
        public string MaVuaQuet { get; private set; }

        private FilterInfoCollection dsCamera;
        private VideoCaptureDevice camera;
        private readonly object khoa = new object();
        private Bitmap khungMoiNhat;                 // khung hình mới nhất để giải mã
        private CancellationTokenSource huyDoc;
        private string maTruoc;
        private string maUngVien;            // [12/11/2026] mã vừa đọc, chờ đọc lại lần 2 cho chắc
        private int soLanKhop;
        private DateTime lucTruoc = DateTime.MinValue;

        public frmQuetCamera()
        {
            InitializeComponent();
            btnDong.Tag = btnDong.BackColor;
        }

        private void frmQuetCamera_Load(object sender, EventArgs e)
        {
            try
            {
                dsCamera = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            }
            catch (Exception ex)
            {
                lblKetQua.Text = "✗ Không đọc được danh sách camera: " + ex.Message;
                return;
            }
            if (dsCamera.Count == 0)
            {
                lblKetQua.ForeColor = Color.Firebrick;
                lblKetQua.Text = "✗ Không tìm thấy webcam nào trên máy.";
                return;
            }
            foreach (FilterInfo f in dsCamera) cboCamera.Items.Add(f.Name);
            cboCamera.SelectedIndex = 0;          // -> cboCamera_SelectedIndexChanged mở camera

            huyDoc = new CancellationTokenSource();
            CancellationToken ct = huyDoc.Token;
            Task.Run(() => VongDocMa(ct));
        }

        private void cboCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            DungCamera();
            if (cboCamera.SelectedIndex < 0 || dsCamera == null) return;
            try
            {
                camera = new VideoCaptureDevice(dsCamera[cboCamera.SelectedIndex].MonikerString);
                // chọn độ phân giải gần 1280 nhất (đủ nét để đọc mã vạch, không quá nặng)
                VideoCapabilities tot = null;
                foreach (VideoCapabilities c in camera.VideoCapabilities)
                    if (tot == null || Math.Abs(c.FrameSize.Width - 1280) < Math.Abs(tot.FrameSize.Width - 1280)) tot = c;
                if (tot != null) camera.VideoResolution = tot;
                camera.NewFrame += Camera_NewFrame;
                camera.Start();
                lblKetQua.ForeColor = Color.FromArgb(33, 37, 41);
                lblKetQua.Text = "Đang quét…";
            }
            catch (Exception ex)
            {
                lblKetQua.ForeColor = Color.Firebrick;
                lblKetQua.Text = "✗ Không mở được camera: " + ex.Message;
            }
        }

        // Chạy trên luồng của AForge: KHÔNG được đụng trực tiếp vào control -> BeginInvoke
        private void Camera_NewFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap hien = (Bitmap)e.Frame.Clone();
            Bitmap doc = (Bitmap)e.Frame.Clone();
            lock (khoa)
            {
                if (khungMoiNhat != null) khungMoiNhat.Dispose();
                khungMoiNhat = doc;
            }
            if (IsDisposed || !IsHandleCreated) { hien.Dispose(); return; }
            try
            {
                BeginInvoke((Action)(() =>
                {
                    if (IsDisposed) { hien.Dispose(); return; }
                    Image cu = picCamera.Image;
                    picCamera.Image = hien;
                    if (cu != null) cu.Dispose();
                }));
            }
            catch (InvalidOperationException)
            {
                hien.Dispose();       // form đang đóng
            }
        }

        private void VongDocMa(CancellationToken ct)
        {
            BarcodeReader boDoc = MaVachHelper.TaoBoDoc();
            while (!ct.IsCancellationRequested)
            {
                Bitmap anh = null;
                lock (khoa)
                {
                    if (khungMoiNhat != null) { anh = khungMoiNhat; khungMoiNhat = null; }
                }
                if (anh == null) { Thread.Sleep(60); continue; }

                string ma = null;
                try { ma = MaVachHelper.Doc(boDoc, anh); }
                catch { /* khung hình lỗi: bỏ qua */ }
                finally { anh.Dispose(); }

                if (!string.IsNullOrWhiteSpace(ma))
                {
                    ma = ma.Trim();
                    // [12/11/2026] phải đọc ra CÙNG 1 mã ở 2 khung hình liên tiếp mới nhận (chống đọc nhầm khi ảnh mờ)
                    if (ma == maUngVien) soLanKhop++;
                    else { maUngVien = ma; soLanKhop = 1; }
                    if (soLanKhop < 2) { Thread.Sleep(60); continue; }
                    soLanKhop = 0;
                    bool trung = ma == maTruoc && (DateTime.Now - lucTruoc).TotalSeconds < 2.5;
                    if (!trung)
                    {
                        maTruoc = ma;
                        lucTruoc = DateTime.Now;
                        try { BeginInvoke((Action)(() => NhanMa(ma))); }
                        catch (InvalidOperationException) { return; }
                    }
                    else lucTruoc = DateTime.Now;
                }
                Thread.Sleep(150);
            }
        }

        private void NhanMa(string ma)
        {
            if (IsDisposed) return;
            MaVuaQuet = ma;
            string thongBao = XuLyMa != null ? XuLyMa(ma) : "Đã quét: " + ma;
            bool loi = thongBao != null && thongBao.StartsWith("✗");
            lblKetQua.ForeColor = loi ? Color.FromArgb(200, 35, 51) : Color.FromArgb(40, 140, 60);
            lblKetQua.Text = "[" + ma + "]  " + thongBao;
            if (loi) SystemSounds.Hand.Play(); else SystemSounds.Asterisk.Play();

            if (!chkLienTuc.Checked && !loi)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void DungCamera()
        {
            if (camera == null) return;
            camera.NewFrame -= Camera_NewFrame;
            if (camera.IsRunning)
            {
                camera.SignalToStop();
                camera.WaitForStop();
            }
            camera = null;
        }

        private void frmQuetCamera_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (huyDoc != null) huyDoc.Cancel();
            DungCamera();
            lock (khoa)
            {
                if (khungMoiNhat != null) { khungMoiNhat.Dispose(); khungMoiNhat = null; }
            }
            Image cu = picCamera.Image;
            picCamera.Image = null;
            if (cu != null) cu.Dispose();
        }
    }
}
