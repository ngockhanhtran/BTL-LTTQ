using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using QuanLyNhaThuoc.BLL;

namespace QuanLyNhaThuoc.GUI
{
    public partial class frmBaoCao : Form
    {
        private enum LoaiBaoCao { HanSuDung, NhapTheoThang, Top5HDBan, Top2NV, SapHetHang, CanhBao }

        // Phát ra khi bấm "Nhập thêm hàng". Form chính nên đăng ký sự kiện này để mở trang
        // Nhập hàng theo cách điều hướng của form chính. e.DanhSachThuoc = các thuốc hết / gần hết hàng.
        // Nếu không ai đăng ký, form sẽ tự tìm form nhập hàng trong project và mở thay.
        public event EventHandler<YeuCauNhapHangEventArgs> YeuCauNhapHang;

        private static readonly CultureInfo VN = new CultureInfo("vi-VN");

        // Màu
        private static readonly Color MAU_NUT = Color.FromArgb(0, 122, 204);
        private static readonly Color MAU_NUT_CHON = Color.FromArgb(0, 70, 140);
        private static readonly Color MAU_NUT_CANH_BAO = Color.FromArgb(230, 126, 34);
        private static readonly Color MAU_NUT_CANH_BAO_CHON = Color.FromArgb(175, 85, 10);
        private static readonly Color MAU_XANH_LA = Color.FromArgb(40, 167, 69);
        private static readonly Color MAU_HUY = Color.FromArgb(200, 35, 51);

        private static readonly Color DO_NEN = Color.FromArgb(255, 199, 206);
        private static readonly Color DO_CHU = Color.FromArgb(156, 0, 6);
        private static readonly Color DO_CHON = Color.FromArgb(200, 35, 51);
        private static readonly Color VANG_NEN = Color.FromArgb(255, 235, 156);
        private static readonly Color VANG_CHU = Color.FromArgb(120, 85, 0);
        private static readonly Color VANG_CHON = Color.FromArgb(230, 180, 20);

        private readonly BaoCaoBLL bcBLL = new BaoCaoBLL();
        private LoaiBaoCao baoCaoHienTai = LoaiBaoCao.HanSuDung;
        private bool dangKhoiTao = true;

        private DataGridView dgvBaoCao;
        private Label lblCanhBao, lblTieuDe, lblTongKet;
        private Button btnXuatExcel, btnHuyHetHan, btnNhapThem;
        private Button btnBieuDo;   // [10/11/2026] biểu đồ so sánh cho báo cáo Top 5 HĐ / Top 2 NV
        private readonly Dictionary<LoaiBaoCao, Button> nutBaoCao = new Dictionary<LoaiBaoCao, Button>();

        // Bộ lọc: mỗi nhóm = nhãn + control, ẩn/hiện theo báo cáo đang chọn
        private ComboBox cboTrangThaiHSD, cboThang, cboQuy, cboLoaiCanhBao;
        private NumericUpDown nudSoNgayCanhBao, nudNam, nudNguongSL;
        private CheckBox chkChiNVCoBan;
        private Control grpTrangThaiHSD, grpSoNgayCanhBao, grpThang, grpQuy, grpNam, grpChiNVCoBan, grpNguongSL, grpLoaiCanhBao;

        // Số liệu cảnh báo mới nhất (cập nhật mỗi lần tải)
        private int soThuocHetHan, soThuocCanHan, soThuocHetHang, soThuocGanHetHang;

        // Tên cột tiếng Việt
        private static readonly Dictionary<string, string> TEN_COT = new Dictionary<string, string>
        {
            { "STT", "STT" },
            { "MaThuoc", "Mã thuốc" }, { "TenThuoc", "Tên thuốc" }, { "TenDVT", "ĐVT" },
            { "SLHienCo", "SL hiện có" }, { "GiaBan", "Giá bán" }, { "DonGiaNhap", "Giá nhập" },
            { "NgaySX", "Ngày SX" }, { "HanSD", "Hạn SD" }, { "SoNgayConLai", "Còn lại (ngày)" },
            { "TinhTrang", "Tình trạng" }, { "LoaiCanhBao", "Cảnh báo" }, { "GiaTriTon", "Giá trị tồn (đ)" },
            { "SLCanNhapThem", "Cần nhập thêm" }, { "NCCGanNhat", "NCC nhập gần nhất" },
            { "MaHDN", "Mã HĐN" }, { "NgayNhap", "Ngày nhập" }, { "MaNCC", "Mã NCC" }, { "TenNCC", "Nhà cung cấp" },
            { "SoMatHang", "Số mặt hàng" }, { "TongSLNhap", "Tổng SL nhập" },
            { "MaHDB", "Mã HĐB" }, { "NgayBan", "Ngày bán" }, { "MaKhach", "Mã KH" }, { "TenKhach", "Khách hàng" },
            { "TongSLBan", "Tổng SL bán" },
            { "MaNV", "Mã NV" }, { "TenNV", "Nhân viên" }, { "SoHoaDon", "Số hóa đơn" },
            { "TongTien", "Tổng tiền (đ)" }, { "TongDoanhThu", "Tổng doanh thu (đ)" }
        };
        private static readonly HashSet<string> COT_AN = new HashSet<string> { "MucCanhBao", "LaHetHan", "LaCanHan", "LaHetHang", "LaGanHetHang" };
        private static readonly HashSet<string> COT_TIEN = new HashSet<string> { "GiaBan", "DonGiaNhap", "TongTien", "TongDoanhThu", "GiaTriTon" };
        private static readonly HashSet<string> COT_SO = new HashSet<string> { "SLHienCo", "SoNgayConLai", "SLCanNhapThem", "SoMatHang", "TongSLNhap", "TongSLBan", "SoHoaDon" };
        private static readonly HashSet<string> COT_NGAY = new HashSet<string> { "NgaySX", "HanSD" };
        private static readonly HashSet<string> COT_NGAY_GIO = new HashSet<string> { "NgayNhap", "NgayBan" };

        public frmBaoCao()
        {
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.Text = "Báo Cáo & Thống Kê Quản Trị";
            InitializeComponentCustom();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dangKhoiTao = false;
            ChonBaoCao(LoaiBaoCao.HanSuDung);
        }

        // ================================================================
        // GIAO DIỆN
        // ================================================================
        private void InitializeComponentCustom()
        {
            this.SuspendLayout();

            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Dock = DockStyle.Fill;

            // Thanh nút báo cáo
            FlowLayoutPanel pnlTop = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = true,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            nutBaoCao[LoaiBaoCao.HanSuDung] = CreateButton(pnlTop, "1. Hạn Sử Dụng (Y/C 6)", (s, e) => ChonBaoCao(LoaiBaoCao.HanSuDung));
            nutBaoCao[LoaiBaoCao.NhapTheoThang] = CreateButton(pnlTop, "2. Nhập Theo Tháng (Y/C 7)", (s, e) => ChonBaoCao(LoaiBaoCao.NhapTheoThang));
            nutBaoCao[LoaiBaoCao.Top5HDBan] = CreateButton(pnlTop, "3. Top 5 HĐ Bán Ít (Y/C 8)", (s, e) => ChonBaoCao(LoaiBaoCao.Top5HDBan));
            nutBaoCao[LoaiBaoCao.Top2NV] = CreateButton(pnlTop, "4. Top 2 NV Bán Ít (Y/C 9)", (s, e) => ChonBaoCao(LoaiBaoCao.Top2NV));
            nutBaoCao[LoaiBaoCao.SapHetHang] = CreateButton(pnlTop, "5. ⚠ Thuốc Sắp Hết Hàng", (s, e) => ChonBaoCao(LoaiBaoCao.SapHetHang));
            nutBaoCao[LoaiBaoCao.SapHetHang].BackColor = MAU_NUT_CANH_BAO;

            // Banner cảnh báo: bấm vào => mở trang chi tiết cảnh báo
            lblCanhBao = new Label()
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 34,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 15, 0),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false
            };
            lblCanhBao.Click += (s, e) => MoTrangCanhBao();

            // Thanh bộ lọc
            FlowLayoutPanel pnlLoc = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = true,
                BackColor = Color.FromArgb(250, 251, 253),
                Padding = new Padding(12, 8, 12, 4)
            };

            lblTieuDe = new Label()
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Margin = new Padding(3, 7, 20, 3)
            };
            pnlLoc.Controls.Add(lblTieuDe);

            cboLoaiCanhBao = TaoCombo(140, "Tất cả cảnh báo", "Hết hạn", "Cận hạn", "Hết hàng", "Gần hết hàng");
            grpLoaiCanhBao = TaoNhom(pnlLoc, "Lọc theo:", cboLoaiCanhBao);

            cboTrangThaiHSD = TaoCombo(110, "Tất cả", "Còn hạn", "Đã hết hạn");
            grpTrangThaiHSD = TaoNhom(pnlLoc, "Hiển thị:", cboTrangThaiHSD);

            nudSoNgayCanhBao = TaoSo(0, 365, BaoCaoBLL.SO_NGAY_SAP_HET_HAN, 60);
            grpSoNgayCanhBao = TaoNhom(pnlLoc, "Cảnh báo trước (ngày):", nudSoNgayCanhBao);

            cboThang = TaoCombo(90);
            for (int i = 1; i <= 12; i++) cboThang.Items.Add("Tháng " + i);
            cboThang.SelectedIndex = DateTime.Today.Month - 1;
            grpThang = TaoNhom(pnlLoc, "Tháng:", cboThang);

            cboQuy = TaoCombo(80, "Quý 1", "Quý 2", "Quý 3", "Quý 4");
            cboQuy.SelectedIndex = (DateTime.Today.Month - 1) / 3;
            grpQuy = TaoNhom(pnlLoc, "Quý:", cboQuy);

            nudNam = TaoSo(2000, 2100, DateTime.Today.Year, 70);
            grpNam = TaoNhom(pnlLoc, "Năm:", nudNam);

            nudNguongSL = TaoSo(1, 100000, BaoCaoBLL.NGUONG_SAP_HET_HANG, 70);
            grpNguongSL = TaoNhom(pnlLoc, "Số lượng dưới:", nudNguongSL);

            chkChiNVCoBan = new CheckBox()
            {
                Text = "Chỉ tính NV có bán hàng trong quý",
                AutoSize = true,
                Checked = true,
                Font = new Font("Segoe UI", 9F),
                Margin = new Padding(3, 7, 15, 3)
            };
            chkChiNVCoBan.CheckedChanged += (s, e) => TaiBaoCao();
            pnlLoc.Controls.Add(chkChiNVCoBan);
            grpChiNVCoBan = chkChiNVCoBan;

            // Báo cáo 1-5: Xuất Excel | Trang cảnh báo: Hủy thuốc hết hạn + Nhập thêm hàng
            btnXuatExcel = TaoNutLoc(pnlLoc, "Xuất Excel", 120, MAU_XANH_LA, BtnXuatExcel_Click);
            btnHuyHetHan = TaoNutLoc(pnlLoc, "Hủy thuốc hết hạn", 170, MAU_HUY, BtnHuyHetHan_Click);
            btnNhapThem = TaoNutLoc(pnlLoc, "Nhập thêm hàng", 150, MAU_NUT, BtnNhapThem_Click);
            // [10/11/2026] so sánh nhóm "Top" với phần còn lại bằng biểu đồ
            btnBieuDo = TaoNutLoc(pnlLoc, "📊 Biểu đồ so sánh", 165, Color.FromArgb(111, 66, 193), BtnBieuDo_Click);

            // Lưới dữ liệu
            dgvBaoCao = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            dgvBaoCao.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvBaoCao.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvBaoCao.DataBindingComplete += (s, e) => DinhDangCot();
            dgvBaoCao.CellFormatting += DgvBaoCao_CellFormatting;

            Panel pnlGridContainer = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(15, 10, 15, 5) };
            pnlGridContainer.Controls.Add(dgvBaoCao);

            // Dòng tổng kết dưới cùng
            lblTongKet = new Label()
            {
                Dock = DockStyle.Bottom,
                AutoSize = false,
                Height = 36,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 15, 0),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(0, 70, 140),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            // Fill thêm trước => dock sau cùng; các Top thêm sau cùng sẽ nằm trên cùng
            this.Controls.Add(pnlGridContainer);
            this.Controls.Add(lblTongKet);
            this.Controls.Add(pnlLoc);
            this.Controls.Add(lblCanhBao);
            this.Controls.Add(pnlTop);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Button CreateButton(Control parent, string text, EventHandler onClick)
        {
            Button btn = new Button()
            {
                Text = text,
                Width = 210,
                Height = 38,
                Margin = new Padding(5),
                BackColor = MAU_NUT,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btn.Click += onClick;
            parent.Controls.Add(btn);
            return btn;
        }

        private Button TaoNutLoc(Control parent, string text, int width, Color mau, EventHandler onClick)
        {
            Button btn = new Button()
            {
                Text = text,
                Width = width,
                Height = 30,
                Margin = new Padding(5, 3, 5, 3),
                BackColor = mau,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            parent.Controls.Add(btn);
            return btn;
        }

        private ComboBox TaoCombo(int width, params string[] items)
        {
            ComboBox cbo = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = width,
                Font = new Font("Segoe UI", 9F)
            };
            cbo.Items.AddRange(items);
            if (items.Length > 0) cbo.SelectedIndex = 0;
            cbo.SelectedIndexChanged += (s, e) => TaiBaoCao();
            return cbo;
        }

        private NumericUpDown TaoSo(int min, int max, int giaTri, int width)
        {
            NumericUpDown nud = new NumericUpDown()
            {
                Minimum = min,
                Maximum = max,
                Value = giaTri,
                Width = width,
                Font = new Font("Segoe UI", 9F),
                TextAlign = HorizontalAlignment.Center
            };
            nud.ValueChanged += (s, e) => TaiBaoCao();
            return nud;
        }

        // Gói nhãn + control vào 1 nhóm để ẩn/hiện cùng nhau
        private Control TaoNhom(Control parent, string nhan, Control ctl)
        {
            FlowLayoutPanel grp = new FlowLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0, 0, 12, 0)
            };
            Label lbl = new Label()
            {
                Text = nhan,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                Margin = new Padding(3, 8, 3, 3)
            };
            ctl.Margin = new Padding(3, 4, 3, 3);
            grp.Controls.Add(lbl);
            grp.Controls.Add(ctl);
            parent.Controls.Add(grp);
            return grp;
        }

        // ================================================================
        // CHỌN / TẢI BÁO CÁO
        // ================================================================
        private void ChonBaoCao(LoaiBaoCao loai)
        {
            baoCaoHienTai = loai;
            bool laTrangCanhBao = loai == LoaiBaoCao.CanhBao;

            foreach (KeyValuePair<LoaiBaoCao, Button> kv in nutBaoCao)
            {
                bool chon = kv.Key == loai;
                bool laNutCam = kv.Key == LoaiBaoCao.SapHetHang;
                kv.Value.BackColor = laNutCam
                    ? (chon ? MAU_NUT_CANH_BAO_CHON : MAU_NUT_CANH_BAO)
                    : (chon ? MAU_NUT_CHON : MAU_NUT);
            }

            grpLoaiCanhBao.Visible = laTrangCanhBao;
            grpTrangThaiHSD.Visible = loai == LoaiBaoCao.HanSuDung;
            grpSoNgayCanhBao.Visible = loai == LoaiBaoCao.HanSuDung;
            grpThang.Visible = loai == LoaiBaoCao.NhapTheoThang;
            grpQuy.Visible = loai == LoaiBaoCao.Top2NV;
            grpNam.Visible = loai == LoaiBaoCao.NhapTheoThang || loai == LoaiBaoCao.Top5HDBan || loai == LoaiBaoCao.Top2NV;
            grpChiNVCoBan.Visible = loai == LoaiBaoCao.Top2NV;
            grpNguongSL.Visible = loai == LoaiBaoCao.SapHetHang;

            btnXuatExcel.Visible = !laTrangCanhBao;
            btnHuyHetHan.Visible = laTrangCanhBao;
            btnNhapThem.Visible = laTrangCanhBao;
            btnBieuDo.Visible = loai == LoaiBaoCao.Top5HDBan || loai == LoaiBaoCao.Top2NV;   // [10/11/2026]

            switch (loai)
            {
                case LoaiBaoCao.HanSuDung: lblTieuDe.Text = "Thuốc còn hạn / hết hạn sử dụng"; break;
                case LoaiBaoCao.NhapTheoThang: lblTieuDe.Text = "Hóa đơn nhập & tổng tiền nhập theo tháng"; break;
                case LoaiBaoCao.Top5HDBan: lblTieuDe.Text = "Top 5 hóa đơn bán có tổng tiền ít nhất"; break;
                case LoaiBaoCao.Top2NV: lblTieuDe.Text = "2 nhân viên bán được ít tiền nhất theo quý"; break;
                case LoaiBaoCao.SapHetHang: lblTieuDe.Text = "Cảnh báo thuốc sắp hết hàng"; break;
                case LoaiBaoCao.CanhBao: lblTieuDe.Text = "Chi tiết cảnh báo thuốc"; break;
            }

            TaiBaoCao();
        }

        private void MoTrangCanhBao()
        {
            dangKhoiTao = true;               // chặn sự kiện để không tải 2 lần
            cboLoaiCanhBao.SelectedIndex = 0; // Tất cả cảnh báo
            dangKhoiTao = false;
            ChonBaoCao(LoaiBaoCao.CanhBao);
        }

        private void TaiBaoCao()
        {
            if (dangKhoiTao) return;

            int nam = (int)nudNam.Value;
            int thang = cboThang.SelectedIndex + 1;
            int quy = cboQuy.SelectedIndex + 1;
            int soNgay = (int)nudSoNgayCanhBao.Value;
            int nguong = (int)nudNguongSL.Value;

            try
            {
                Cursor = Cursors.WaitCursor;
                DataTable dt = null;
                string tongKet = "";

                switch (baoCaoHienTai)
                {
                    case LoaiBaoCao.HanSuDung:
                        {
                            int[] maTrangThai = { BaoCaoBLL.HSD_TAT_CA, BaoCaoBLL.HSD_CON_HAN, BaoCaoBLL.HSD_HET_HAN };
                            dt = bcBLL.BaoCaoHanSuDung(maTrangThai[Math.Max(0, cboTrangThaiHSD.SelectedIndex)], soNgay);
                            tongKet = string.Format("Tổng: {0} thuốc   |   Đã hết hạn: {1}   |   Sắp hết hạn (≤ {2} ngày): {3}",
                                dt.Rows.Count,
                                BaoCaoBLL.DemTheoMuc(dt, BaoCaoBLL.MUC_DA_HET),
                                soNgay,
                                BaoCaoBLL.DemTheoMuc(dt, BaoCaoBLL.MUC_SAP_HET));
                            break;
                        }
                    case LoaiBaoCao.NhapTheoThang:
                        {
                            DataSet ds = bcBLL.BaoCaoNhapHangTheoThang(thang, nam);
                            dt = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
                            decimal tong = 0;
                            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                tong = Convert.ToDecimal(ds.Tables[1].Rows[0]["TongTienNhapThang"]);
                            tongKet = string.Format("Tháng {0}/{1}: {2} hóa đơn nhập   |   Tổng tiền nhập: {3} đ",
                                thang, nam, dt.Rows.Count, tong.ToString("N0", VN));
                            break;
                        }
                    case LoaiBaoCao.Top5HDBan:
                        {
                            dt = bcBLL.BaoCaoTop5HDBItNhatTheoNam(nam);
                            tongKet = string.Format("Năm {0}: {1} hóa đơn   |   Tổng tiền {1} hóa đơn: {2} đ",
                                nam, dt.Rows.Count, TongCot(dt, "TongTien").ToString("N0", VN));
                            break;
                        }
                    case LoaiBaoCao.Top2NV:
                        {
                            dt = bcBLL.BaoCaoTop2NVBanItNhatTheoQuy(quy, nam, chkChiNVCoBan.Checked);
                            DateTime tuNgay = new DateTime(nam, (quy - 1) * 3 + 1, 1);
                            tongKet = string.Format("Quý {0}/{1} (từ {2:dd/MM/yyyy} đến {3:dd/MM/yyyy})",
                                quy, nam, tuNgay, tuNgay.AddMonths(3).AddDays(-1));
                            break;
                        }
                    case LoaiBaoCao.SapHetHang:
                        {
                            dt = bcBLL.CanhBaoThuocSapHet(nguong);
                            tongKet = string.Format("Có {0} thuốc dưới {1} đơn vị   |   Hết hàng: {2}   |   Sắp hết: {3}",
                                dt.Rows.Count, nguong,
                                BaoCaoBLL.DemTheoMuc(dt, BaoCaoBLL.MUC_DA_HET),
                                BaoCaoBLL.DemTheoMuc(dt, BaoCaoBLL.MUC_SAP_HET));
                            break;
                        }
                    case LoaiBaoCao.CanhBao:
                        {
                            int[] maLoai = { BaoCaoBLL.CB_TAT_CA, BaoCaoBLL.CB_HET_HAN, BaoCaoBLL.CB_CAN_HAN,
                                             BaoCaoBLL.CB_HET_HANG, BaoCaoBLL.CB_GAN_HET_HANG };
                            dt = bcBLL.DanhSachCanhBao(maLoai[Math.Max(0, cboLoaiCanhBao.SelectedIndex)]);
                            tongKet = string.Format("{0} thuốc   |   Hết hạn: {1}   |   Cận hạn (≤ {2} ngày): {3}   |   Hết hàng: {4}   |   Gần hết hàng (< {5}): {6}",
                                dt.Rows.Count,
                                BaoCaoBLL.DemCo(dt, "LaHetHan"),
                                BaoCaoBLL.SO_NGAY_SAP_HET_HAN, BaoCaoBLL.DemCo(dt, "LaCanHan"),
                                BaoCaoBLL.DemCo(dt, "LaHetHang"),
                                BaoCaoBLL.NGUONG_SAP_HET_HANG, BaoCaoBLL.DemCo(dt, "LaGanHetHang"));
                            break;
                        }
                }

                dgvBaoCao.DataSource = dt;
                if (dt == null || dt.Rows.Count == 0) tongKet += "   —   Không có dữ liệu";
                lblTongKet.Text = tongKet;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            CapNhatBannerCanhBao();
        }

        private static decimal TongCot(DataTable dt, string cot)
        {
            decimal tong = 0;
            if (dt == null || !dt.Columns.Contains(cot)) return 0;
            foreach (DataRow r in dt.Rows)
                if (r[cot] != DBNull.Value) tong += Convert.ToDecimal(r[cot]);
            return tong;
        }

        // ================================================================
        // BANNER CẢNH BÁO (luôn hiện ở đầu form)
        // ================================================================
        private void CapNhatBannerCanhBao()
        {
            try
            {
                DataTable dt = bcBLL.DanhSachCanhBao(BaoCaoBLL.CB_TAT_CA);
                soThuocHetHan = BaoCaoBLL.DemCo(dt, "LaHetHan");
                soThuocCanHan = BaoCaoBLL.DemCo(dt, "LaCanHan");
                soThuocHetHang = BaoCaoBLL.DemCo(dt, "LaHetHang");
                soThuocGanHetHang = BaoCaoBLL.DemCo(dt, "LaGanHetHang");

                if (dt.Rows.Count == 0)
                {
                    lblCanhBao.Text = "✔  Không có thuốc hết hạn, cận hạn, hết hàng hay gần hết hàng.";
                    lblCanhBao.BackColor = Color.FromArgb(212, 237, 218);
                    lblCanhBao.ForeColor = Color.FromArgb(21, 87, 36);
                }
                else
                {
                    List<string> phan = new List<string>();
                    if (soThuocHetHan > 0) phan.Add(soThuocHetHan + " thuốc hết hạn");
                    if (soThuocCanHan > 0) phan.Add(soThuocCanHan + " cận hạn");
                    if (soThuocHetHang > 0) phan.Add(soThuocHetHang + " hết hàng");
                    if (soThuocGanHetHang > 0) phan.Add(soThuocGanHetHang + " gần hết hàng");

                    lblCanhBao.Text = "⚠  CẢNH BÁO: " + string.Join("   •   ", phan) + "   —  bấm vào đây để xem chi tiết";
                    bool nghiemTrong = soThuocHetHan > 0 || soThuocHetHang > 0;
                    lblCanhBao.BackColor = nghiemTrong ? DO_NEN : VANG_NEN;
                    lblCanhBao.ForeColor = nghiemTrong ? DO_CHU : VANG_CHU;
                }
                lblCanhBao.Visible = true;

                btnHuyHetHan.Enabled = soThuocHetHan > 0;
                btnHuyHetHan.Text = soThuocHetHan > 0 ? "Hủy thuốc hết hạn (" + soThuocHetHan + ")" : "Hủy thuốc hết hạn";
                btnHuyHetHan.BackColor = soThuocHetHan > 0 ? MAU_HUY : Color.Silver;
            }
            catch
            {
                lblCanhBao.Visible = false;
            }
        }

        // ================================================================
        // NÚT: HỦY THUỐC HẾT HẠN
        // ================================================================
        private void BtnHuyHetHan_Click(object sender, EventArgs e)
        {
            try
            {
                // lấy lại danh sách mới nhất từ CSDL để xác nhận đúng những gì sẽ hủy
                DataTable dtHetHan = bcBLL.DanhSachCanhBao(BaoCaoBLL.CB_HET_HAN);
                if (dtHetHan.Rows.Count == 0)
                {
                    MessageBox.Show("Không có thuốc hết hạn nào cần hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiBaoCao();
                    return;
                }

                List<string> dong = new List<string>();
                int i = 0;
                foreach (DataRow r in dtHetHan.Rows)
                {
                    if (++i > 10) { dong.Add("   ... và " + (dtHetHan.Rows.Count - 10) + " thuốc khác"); break; }
                    dong.Add(string.Format("   • {0} - {1}: SL {2}, hạn {3:dd/MM/yyyy}",
                        r["MaThuoc"], r["TenThuoc"], r["SLHienCo"], r["HanSD"]));
                }

                string xacNhan = string.Format(
                    "Hủy {0} thuốc đã hết hạn (tổng giá trị nhập {1} đ):\n\n{2}\n\n" +
                    "Số lượng tồn của các thuốc này sẽ về 0 và được ghi vào lịch sử hủy.\nBạn có chắc chắn muốn hủy?",
                    dtHetHan.Rows.Count, TongCot(dtHetHan, "GiaTriTon").ToString("N0", VN), string.Join("\n", dong));

                if (MessageBox.Show(xacNhan, "Xác nhận hủy thuốc hết hạn", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;

                // [10/11/2026] ghi nhận người hủy = nhân viên đang đăng nhập
                DataTable kq = bcBLL.HuyThuocHetHan(QuanLyNhaThuoc.DTO.PhienDangNhap.MaNV);
                DataRow k = kq.Rows[0];
                MessageBox.Show(string.Format("Đã hủy {0} thuốc, tổng số lượng {1}, giá trị {2} đ.",
                        k["SoThuocHuy"], Convert.ToInt32(k["TongSLHuy"]).ToString("N0", VN),
                        Convert.ToDecimal(k["TongGiaTriHuy"]).ToString("N0", VN)),
                    "Hủy thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy thuốc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            TaiBaoCao();
        }

        // ================================================================
        // NÚT: NHẬP THÊM HÀNG -> chuyển sang trang Nhập hàng
        // ================================================================
        private void BtnNhapThem_Click(object sender, EventArgs e)
        {
            DataTable dsCanNhap = LayDanhSachCanNhap();

            EventHandler<YeuCauNhapHangEventArgs> handler = YeuCauNhapHang;
            if (handler != null)
            {
                handler(this, new YeuCauNhapHangEventArgs(dsCanNhap));
                return;
            }

            if (!MoFormNhapHangTuDong(dsCanNhap))
            {
                MessageBox.Show("Không tìm thấy trang Nhập hàng trong chương trình.\n" +
                    "Hãy đăng ký sự kiện frmBaoCao.YeuCauNhapHang ở form chính để mở trang Nhập hàng.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Thuốc hết hàng + gần hết hàng: MaThuoc, TenThuoc, SLHienCo, SLCanNhapThem, NCCGanNhat
        private DataTable LayDanhSachCanNhap()
        {
            DataTable kq = new DataTable("ThuocCanNhap");
            kq.Columns.Add("MaThuoc", typeof(string));
            kq.Columns.Add("TenThuoc", typeof(string));
            kq.Columns.Add("SLHienCo", typeof(int));
            kq.Columns.Add("SLCanNhapThem", typeof(int));
            kq.Columns.Add("NCCGanNhat", typeof(string));
            try
            {
                DataTable dt = bcBLL.DanhSachCanhBao(BaoCaoBLL.CB_TAT_CA);
                foreach (DataRow r in dt.Rows)
                {
                    if (Convert.ToInt32(r["LaHetHang"]) == 1 || Convert.ToInt32(r["LaGanHetHang"]) == 1)
                    {
                        kq.Rows.Add(r["MaThuoc"], r["TenThuoc"], r["SLHienCo"],
                            r["SLCanNhapThem"] == DBNull.Value ? 0 : r["SLCanNhapThem"],
                            r["NCCGanNhat"]);
                    }
                }
            }
            catch { /* danh sách gợi ý chỉ là phụ, lỗi thì vẫn chuyển trang */ }
            return kq;
        }

        // Dự phòng khi form chính chưa đăng ký sự kiện: tự tìm form nhập hàng trong project
        private bool MoFormNhapHangTuDong(DataTable dsCanNhap)
        {
            try
            {
                Type[] tatCa;
                try { tatCa = Assembly.GetExecutingAssembly().GetTypes(); }
                catch (ReflectionTypeLoadException ex) { tatCa = ex.Types.Where(t => t != null).ToArray(); }

                Func<Type, bool> laFormDung = t => typeof(Form).IsAssignableFrom(t) && !t.IsAbstract
                                                   && t != this.GetType() && t.GetConstructor(Type.EmptyTypes) != null;

                string[] tenUuTien = { "frmNhapHang", "frmHoaDonNhap", "frmNhapThuoc", "frmPhieuNhap", "frmNhapKho" };
                Type loai = null;
                foreach (string ten in tenUuTien)
                {
                    loai = tatCa.FirstOrDefault(t => laFormDung(t) && string.Equals(t.Name, ten, StringComparison.OrdinalIgnoreCase));
                    if (loai != null) break;
                }
                if (loai == null)
                {
                    loai = tatCa.FirstOrDefault(t => laFormDung(t) &&
                        (t.Name.IndexOf("NhapHang", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         t.Name.IndexOf("HoaDonNhap", StringComparison.OrdinalIgnoreCase) >= 0));
                }
                if (loai == null) return false;

                Form frm = (Form)Activator.CreateInstance(loai);

                // nếu form nhập hàng có hàm nhận danh sách gợi ý thì truyền sang
                MethodInfo nhan = loai.GetMethod("NhanDanhSachCanNhap", new[] { typeof(DataTable) });
                if (nhan != null) nhan.Invoke(frm, new object[] { dsCanNhap });

                Control khung = this.Parent;
                if (!this.TopLevel && khung != null)
                {
                    // form báo cáo đang được nhúng trong panel của form chính -> thay bằng form nhập hàng
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    khung.Controls.Add(frm);
                    frm.Show();
                    frm.BringToFront();
                    this.Close();
                }
                else
                {
                    frm.Show();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không mở được trang Nhập hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true; // đã báo lỗi rồi, không báo thêm
            }
        }

        // ================================================================
        // [10/11/2026] NÚT: BIỂU ĐỒ SO SÁNH (Top 5 HĐ bán ít / Top 2 NV bán ít)
        // ================================================================
        private void BtnBieuDo_Click(object sender, EventArgs e)
        {
            int nam = (int)nudNam.Value;
            try
            {
                if (baoCaoHienTai == LoaiBaoCao.Top5HDBan)
                    frmBieuDo.MoBieuDoHoaDon(this.TopLevelControl ?? this, nam);
                else if (baoCaoHienTai == LoaiBaoCao.Top2NV)
                    frmBieuDo.MoBieuDoNhanVien(this.TopLevelControl ?? this, cboQuy.SelectedIndex + 1, nam, chkChiNVCoBan.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi vẽ biểu đồ: " + ex.Message + "\n\nHãy chạy lại file CSDL_QuanLyNhaThuoc.sql để tạo các proc sp_Dashboard_*.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // NÚT: XUẤT EXCEL
        // ================================================================
        private void BtnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dgvBaoCao.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Xuất báo cáo ra Excel";
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = TenFileMacDinh();
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    Cursor = Cursors.WaitCursor;
                    XuatExcel.TuDataGridView(dgvBaoCao, sfd.FileName, lblTieuDe.Text, MoTaBoLoc(), lblTongKet.Text);
                }
                catch (IOException)
                {
                    MessageBox.Show("Không ghi được file. Nếu file đang mở trong Excel, hãy đóng lại rồi thử lại.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    Cursor = Cursors.Default;
                }

                if (MessageBox.Show("Xuất Excel thành công.\nBạn có muốn mở file ngay không?", "Thành công",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    try { Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true }); }
                    catch { /* máy không có Excel thì bỏ qua */ }
                }
            }
        }

        private string TenFileMacDinh()
        {
            int nam = (int)nudNam.Value;
            string ten;
            switch (baoCaoHienTai)
            {
                case LoaiBaoCao.HanSuDung: ten = "BaoCao_HanSuDung"; break;
                case LoaiBaoCao.NhapTheoThang: ten = string.Format("BaoCao_NhapHang_T{0:00}_{1}", cboThang.SelectedIndex + 1, nam); break;
                case LoaiBaoCao.Top5HDBan: ten = "BaoCao_Top5HoaDonBanIt_" + nam; break;
                case LoaiBaoCao.Top2NV: ten = string.Format("BaoCao_Top2NVBanIt_Q{0}_{1}", cboQuy.SelectedIndex + 1, nam); break;
                case LoaiBaoCao.SapHetHang: ten = "BaoCao_ThuocSapHetHang"; break;
                default: ten = "BaoCao_CanhBaoThuoc"; break;
            }
            return ten + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
        }

        private string MoTaBoLoc()
        {
            int nam = (int)nudNam.Value;
            switch (baoCaoHienTai)
            {
                case LoaiBaoCao.HanSuDung:
                    return string.Format("Hiển thị: {0}  -  Cảnh báo trước {1} ngày", cboTrangThaiHSD.Text, nudSoNgayCanhBao.Value);
                case LoaiBaoCao.NhapTheoThang:
                    return string.Format("Tháng {0}/{1}", cboThang.SelectedIndex + 1, nam);
                case LoaiBaoCao.Top5HDBan:
                    return "Năm " + nam;
                case LoaiBaoCao.Top2NV:
                    return string.Format("Quý {0}/{1}{2}", cboQuy.SelectedIndex + 1, nam,
                        chkChiNVCoBan.Checked ? "  -  chỉ tính NV có bán hàng trong quý" : "");
                case LoaiBaoCao.SapHetHang:
                    return string.Format("Số lượng hiện có dưới {0}", nudNguongSL.Value);
                default:
                    return "Lọc theo: " + cboLoaiCanhBao.Text;
            }
        }

        // ================================================================
        // ĐỊNH DẠNG LƯỚI
        // ================================================================
        private void DinhDangCot()
        {
            foreach (DataGridViewColumn col in dgvBaoCao.Columns)
            {
                string ten = col.DataPropertyName;
                string tieuDe;
                if (TEN_COT.TryGetValue(ten, out tieuDe)) col.HeaderText = tieuDe;

                if (COT_AN.Contains(ten)) { col.Visible = false; continue; }

                col.DefaultCellStyle.FormatProvider = VN;
                if (COT_TIEN.Contains(ten))
                {
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (COT_SO.Contains(ten))
                {
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (COT_NGAY.Contains(ten))
                {
                    col.DefaultCellStyle.Format = "dd/MM/yyyy";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (COT_NGAY_GIO.Contains(ten))
                {
                    col.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (ten == "STT")
                {
                    col.FillWeight = 35;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (ten == "TenThuoc" || ten == "TenNV" || ten == "TenNCC" || ten == "TenKhach"
                         || ten == "NCCGanNhat" || ten == "LoaiCanhBao")
                {
                    col.FillWeight = 160;
                }
            }
        }

        // Tô màu dòng: MucCanhBao = 2 -> đỏ, 1 -> vàng
        private void DgvBaoCao_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !dgvBaoCao.Columns.Contains("MucCanhBao")) return;

            object v = dgvBaoCao.Rows[e.RowIndex].Cells["MucCanhBao"].Value;
            if (v == null || v == DBNull.Value) return;

            int muc = Convert.ToInt32(v);
            if (muc == BaoCaoBLL.MUC_DA_HET)
            {
                e.CellStyle.BackColor = DO_NEN;
                e.CellStyle.ForeColor = DO_CHU;
                e.CellStyle.SelectionBackColor = DO_CHON;
                e.CellStyle.SelectionForeColor = Color.White;
            }
            else if (muc == BaoCaoBLL.MUC_SAP_HET)
            {
                e.CellStyle.BackColor = VANG_NEN;
                e.CellStyle.ForeColor = VANG_CHU;
                e.CellStyle.SelectionBackColor = VANG_CHON;
                e.CellStyle.SelectionForeColor = Color.Black;
            }
        }
    }

    //Dữ liệu gửi kèm khi bấm "Nhập thêm hàng"
    public class YeuCauNhapHangEventArgs : EventArgs
    {
        //Cột: MaThuoc, TenThuoc, SLHienCo, SLCanNhapThem, NCCGanNhat
        public DataTable DanhSachThuoc { get; private set; }

        public YeuCauNhapHangEventArgs(DataTable danhSachThuoc)
        {
            DanhSachThuoc = danhSachThuoc;
        }
    }
}