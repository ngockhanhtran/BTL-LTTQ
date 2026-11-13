namespace QuanLyNhaThuoc.GUI
{
    partial class frmBanHang
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dgvCanhPhai1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvCanhPhai2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvCanhPhai3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tbl = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTrai = new System.Windows.Forms.Panel();
            this.lblTieuDeTrai = new System.Windows.Forms.Label();
            this.lblMaHDB = new System.Windows.Forms.Label();
            this.txtMaHDB = new System.Windows.Forms.TextBox();
            this.lblNgayLap = new System.Windows.Forms.Label();
            this.txtNgayLap = new System.Windows.Forms.TextBox();
            this.lblNhanVien = new System.Windows.Forms.Label();
            this.cboNhanVien = new System.Windows.Forms.ComboBox();
            this.lblMaKhach = new System.Windows.Forms.Label();
            this.txtMaKhach = new System.Windows.Forms.TextBox();
            this.lblTenKhach = new System.Windows.Forms.Label();
            this.txtTenKhach = new System.Windows.Forms.TextBox();
            this.lblKhachInfo = new System.Windows.Forms.Label();
            this.lblMaDonThuoc = new System.Windows.Forms.Label();
            this.txtMaDonThuoc = new System.Windows.Forms.TextBox();
            this.lblKeDon = new System.Windows.Forms.Label();
            this.lblSepDiem = new System.Windows.Forms.Label();
            this.lblDiemTieuDe = new System.Windows.Forms.Label();
            this.lblDiemKhach = new System.Windows.Forms.Label();
            this.lblDungDiem = new System.Windows.Forms.Label();
            this.nudDungDiem = new System.Windows.Forms.NumericUpDown();
            this.lblQuyDoi = new System.Windows.Forms.Label();
            this.lblQuyDinh = new System.Windows.Forms.Label();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.lblHuongDan = new System.Windows.Forms.Label();
            this.pnlPhai = new System.Windows.Forms.Panel();
            this.dgvChiTiet = new System.Windows.Forms.DataGridView();
            this.pnlTong = new System.Windows.Forms.Panel();
            this.pnlQuet = new System.Windows.Forms.Panel();
            this.lblTieuDePhai = new System.Windows.Forms.Label();
            this.colSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiaBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblQuetTieuDe = new System.Windows.Forms.Label();
            this.txtQuet = new System.Windows.Forms.TextBox();
            this.lblSLQuet = new System.Windows.Forms.Label();
            this.nudSLQuet = new System.Windows.Forms.NumericUpDown();
            this.btnQuet = new System.Windows.Forms.Button();
            this.lblQuet = new System.Windows.Forms.Label();
            this.lblGiamGiaNhan = new System.Windows.Forms.Label();
            this.nudGiamGia = new System.Windows.Forms.NumericUpDown();
            this.lblTienHangNhan = new System.Windows.Forms.Label();
            this.lblTienHang = new System.Windows.Forms.Label();
            this.lblTienGiamNhan = new System.Windows.Forms.Label();
            this.lblTienGiam = new System.Windows.Forms.Label();
            this.lblTienDiemNhan = new System.Windows.Forms.Label();
            this.lblTienDiem = new System.Windows.Forms.Label();
            this.lblTongTienNhan = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.lblDiemSeCong = new System.Windows.Forms.Label();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnXuatPdf = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnCamera = new System.Windows.Forms.Button();
            this.pnlGoiY = new System.Windows.Forms.Panel();
            this.flpMuaKem = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMuaKemTieuDe = new System.Windows.Forms.Label();
            this.lblTuongTac = new System.Windows.Forms.Label();
            this.ttGoiY = new System.Windows.Forms.ToolTip(this.components);
            this.tmrKhach = new System.Windows.Forms.Timer(this.components);
            this.tbl.SuspendLayout();
            this.pnlTrai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDungDiem)).BeginInit();
            this.pnlPhai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.pnlTong.SuspendLayout();
            this.pnlQuet.SuspendLayout();
            this.pnlGoiY.SuspendLayout();
            this.flpMuaKem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSLQuet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGiamGia)).BeginInit();
            this.SuspendLayout();
            dgvCanhPhai1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dgvCanhPhai2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dgvCanhPhai3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            // 
            // tbl
            // 
            this.tbl.Controls.Add(this.pnlTrai, 0, 0);
            this.tbl.Controls.Add(this.pnlPhai, 1, 0);
            this.tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58F));
            this.tbl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbl.ColumnCount = 2;
            this.tbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbl.Location = new System.Drawing.Point(0, 0);
            this.tbl.Padding = new System.Windows.Forms.Padding(15);
            this.tbl.RowCount = 1;
            this.tbl.Size = new System.Drawing.Size(1000, 640);
            this.tbl.Name = "tbl";
            this.tbl.TabIndex = 0;
            // 
            // pnlTrai
            // 
            this.pnlTrai.Controls.Add(this.lblTieuDeTrai);
            this.pnlTrai.Controls.Add(this.lblMaHDB);
            this.pnlTrai.Controls.Add(this.txtMaHDB);
            this.pnlTrai.Controls.Add(this.lblNgayLap);
            this.pnlTrai.Controls.Add(this.txtNgayLap);
            this.pnlTrai.Controls.Add(this.lblNhanVien);
            this.pnlTrai.Controls.Add(this.cboNhanVien);
            this.pnlTrai.Controls.Add(this.lblMaKhach);
            this.pnlTrai.Controls.Add(this.txtMaKhach);
            this.pnlTrai.Controls.Add(this.lblTenKhach);
            this.pnlTrai.Controls.Add(this.txtTenKhach);
            this.pnlTrai.Controls.Add(this.lblKhachInfo);
            this.pnlTrai.Controls.Add(this.lblMaDonThuoc);
            this.pnlTrai.Controls.Add(this.txtMaDonThuoc);
            this.pnlTrai.Controls.Add(this.lblKeDon);
            this.pnlTrai.Controls.Add(this.lblSepDiem);
            this.pnlTrai.Controls.Add(this.lblDiemTieuDe);
            this.pnlTrai.Controls.Add(this.lblDiemKhach);
            this.pnlTrai.Controls.Add(this.lblDungDiem);
            this.pnlTrai.Controls.Add(this.nudDungDiem);
            this.pnlTrai.Controls.Add(this.lblQuyDoi);
            this.pnlTrai.Controls.Add(this.lblQuyDinh);
            this.pnlTrai.Controls.Add(this.lblTrangThai);
            this.pnlTrai.Controls.Add(this.lblHuongDan);
            this.pnlTrai.AutoScroll = true;
            this.pnlTrai.BackColor = System.Drawing.Color.White;
            this.pnlTrai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTrai.Location = new System.Drawing.Point(15, 15);
            this.pnlTrai.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlTrai.Size = new System.Drawing.Size(392, 610);
            this.pnlTrai.Name = "pnlTrai";
            this.pnlTrai.TabIndex = 1;
            // 
            // lblTieuDeTrai
            // 
            this.lblTieuDeTrai.AutoSize = true;
            this.lblTieuDeTrai.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDeTrai.Location = new System.Drawing.Point(20, 20);
            this.lblTieuDeTrai.Text = "Thông Tin Hóa Đơn";
            this.lblTieuDeTrai.Name = "lblTieuDeTrai";
            this.lblTieuDeTrai.TabIndex = 2;
            // 
            // lblMaHDB
            // 
            this.lblMaHDB.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaHDB.Location = new System.Drawing.Point(20, 68);
            this.lblMaHDB.Size = new System.Drawing.Size(120, 23);
            this.lblMaHDB.Text = "Mã hóa đơn:";
            this.lblMaHDB.Name = "lblMaHDB";
            this.lblMaHDB.TabIndex = 3;
            // 
            // txtMaHDB
            // 
            this.txtMaHDB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaHDB.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaHDB.Location = new System.Drawing.Point(145, 65);
            this.txtMaHDB.Size = new System.Drawing.Size(230, 25);
            this.txtMaHDB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.txtMaHDB.ReadOnly = true;
            this.txtMaHDB.TabStop = false;
            this.txtMaHDB.Name = "txtMaHDB";
            this.txtMaHDB.TabIndex = 4;
            // 
            // lblNgayLap
            // 
            this.lblNgayLap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayLap.Location = new System.Drawing.Point(20, 108);
            this.lblNgayLap.Size = new System.Drawing.Size(120, 23);
            this.lblNgayLap.Text = "Ngày lập:";
            this.lblNgayLap.Name = "lblNgayLap";
            this.lblNgayLap.TabIndex = 5;
            // 
            // txtNgayLap
            // 
            this.txtNgayLap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNgayLap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNgayLap.Location = new System.Drawing.Point(145, 105);
            this.txtNgayLap.Size = new System.Drawing.Size(230, 25);
            this.txtNgayLap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.txtNgayLap.ReadOnly = true;
            this.txtNgayLap.TabStop = false;
            this.txtNgayLap.Name = "txtNgayLap";
            this.txtNgayLap.TabIndex = 6;
            // 
            // lblNhanVien
            // 
            this.lblNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNhanVien.Location = new System.Drawing.Point(20, 148);
            this.lblNhanVien.Size = new System.Drawing.Size(120, 23);
            this.lblNhanVien.Text = "Nhân viên:";
            this.lblNhanVien.Name = "lblNhanVien";
            this.lblNhanVien.TabIndex = 7;
            // 
            // cboNhanVien
            // 
            this.cboNhanVien.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.cboNhanVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboNhanVien.Location = new System.Drawing.Point(145, 145);
            this.cboNhanVien.Size = new System.Drawing.Size(230, 25);
            this.cboNhanVien.Name = "cboNhanVien";
            this.cboNhanVien.TabIndex = 8;
            // 
            // lblMaKhach
            // 
            this.lblMaKhach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaKhach.Location = new System.Drawing.Point(20, 188);
            this.lblMaKhach.Size = new System.Drawing.Size(120, 23);
            this.lblMaKhach.Text = "Mã khách hàng:";
            this.lblMaKhach.Name = "lblMaKhach";
            this.lblMaKhach.TabIndex = 9;
            // 
            // txtMaKhach
            // 
            this.txtMaKhach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaKhach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaKhach.Location = new System.Drawing.Point(145, 185);
            this.txtMaKhach.Size = new System.Drawing.Size(230, 25);
            this.txtMaKhach.MaxLength = 10;
            this.txtMaKhach.Name = "txtMaKhach";
            this.txtMaKhach.TabIndex = 10;
            this.txtMaKhach.TextChanged += new System.EventHandler(this.txtMaKhach_TextChanged);
            // 
            // lblTenKhach
            // 
            this.lblTenKhach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenKhach.Location = new System.Drawing.Point(20, 228);
            this.lblTenKhach.Size = new System.Drawing.Size(120, 23);
            this.lblTenKhach.Text = "Tên khách hàng:";
            this.lblTenKhach.Name = "lblTenKhach";
            this.lblTenKhach.TabIndex = 11;
            // 
            // txtTenKhach
            // 
            this.txtTenKhach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTenKhach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTenKhach.Location = new System.Drawing.Point(145, 225);
            this.txtTenKhach.Size = new System.Drawing.Size(230, 25);
            this.txtTenKhach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.txtTenKhach.ReadOnly = true;
            this.txtTenKhach.TabStop = false;
            this.txtTenKhach.MaxLength = 50;
            this.txtTenKhach.Name = "txtTenKhach";
            this.txtTenKhach.TabIndex = 12;
            // 
            // lblKhachInfo
            // 
            this.lblKhachInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKhachInfo.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKhachInfo.Location = new System.Drawing.Point(145, 255);
            this.lblKhachInfo.Size = new System.Drawing.Size(230, 20);
            this.lblKhachInfo.Name = "lblKhachInfo";
            this.lblKhachInfo.TabIndex = 13;
            // 
            // lblMaDonThuoc
            // 
            this.lblMaDonThuoc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaDonThuoc.Location = new System.Drawing.Point(20, 284);
            this.lblMaDonThuoc.Size = new System.Drawing.Size(120, 23);
            this.lblMaDonThuoc.Text = "Mã đơn thuốc:";
            this.lblMaDonThuoc.Name = "lblMaDonThuoc";
            this.lblMaDonThuoc.TabIndex = 14;
            // 
            // txtMaDonThuoc
            // 
            this.txtMaDonThuoc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaDonThuoc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaDonThuoc.Location = new System.Drawing.Point(145, 281);
            this.txtMaDonThuoc.MaxLength = 30;
            this.txtMaDonThuoc.Size = new System.Drawing.Size(230, 25);
            this.txtMaDonThuoc.Name = "txtMaDonThuoc";
            this.txtMaDonThuoc.TabIndex = 15;
            this.txtMaDonThuoc.TextChanged += new System.EventHandler(this.txtMaDonThuoc_TextChanged);
            // 
            // lblKeDon
            // 
            this.lblKeDon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKeDon.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeDon.ForeColor = System.Drawing.Color.DimGray;
            this.lblKeDon.Location = new System.Drawing.Point(145, 309);
            this.lblKeDon.Size = new System.Drawing.Size(230, 20);
            this.lblKeDon.Text = "Chỉ cần khi có thuốc kê đơn (℞)";
            this.lblKeDon.Name = "lblKeDon";
            this.lblKeDon.TabIndex = 16;
            // 
            // lblSepDiem
            // 
            this.lblSepDiem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSepDiem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSepDiem.Location = new System.Drawing.Point(20, 333);
            this.lblSepDiem.Size = new System.Drawing.Size(355, 2);
            this.lblSepDiem.Name = "lblSepDiem";
            this.lblSepDiem.TabIndex = 17;
            // 
            // lblDiemTieuDe
            // 
            this.lblDiemTieuDe.AutoSize = true;
            this.lblDiemTieuDe.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiemTieuDe.Location = new System.Drawing.Point(20, 341);
            this.lblDiemTieuDe.Text = "⭐ Điểm tích lũy";
            this.lblDiemTieuDe.Name = "lblDiemTieuDe";
            this.lblDiemTieuDe.TabIndex = 18;
            // 
            // lblDiemKhach
            // 
            this.lblDiemKhach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDiemKhach.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiemKhach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(160)))));
            this.lblDiemKhach.Location = new System.Drawing.Point(20, 365);
            this.lblDiemKhach.Size = new System.Drawing.Size(355, 22);
            this.lblDiemKhach.Name = "lblDiemKhach";
            this.lblDiemKhach.TabIndex = 19;
            // 
            // lblDungDiem
            // 
            this.lblDungDiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDungDiem.Location = new System.Drawing.Point(20, 394);
            this.lblDungDiem.Size = new System.Drawing.Size(120, 23);
            this.lblDungDiem.Text = "Dùng điểm:";
            this.lblDungDiem.Name = "lblDungDiem";
            this.lblDungDiem.TabIndex = 20;
            // 
            // nudDungDiem
            // 
            this.nudDungDiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDungDiem.Location = new System.Drawing.Point(145, 391);
            this.nudDungDiem.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudDungDiem.Size = new System.Drawing.Size(90, 25);
            this.nudDungDiem.ThousandsSeparator = true;
            this.nudDungDiem.Name = "nudDungDiem";
            this.nudDungDiem.TabIndex = 21;
            this.nudDungDiem.ValueChanged += new System.EventHandler(this.nudDungDiem_ValueChanged);
            // 
            // lblQuyDoi
            // 
            this.lblQuyDoi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuyDoi.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuyDoi.ForeColor = System.Drawing.Color.DimGray;
            this.lblQuyDoi.Location = new System.Drawing.Point(240, 394);
            this.lblQuyDoi.Size = new System.Drawing.Size(140, 20);
            this.lblQuyDoi.Name = "lblQuyDoi";
            this.lblQuyDoi.TabIndex = 22;
            // 
            // lblQuyDinh
            // 
            this.lblQuyDinh.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuyDinh.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuyDinh.ForeColor = System.Drawing.Color.Gray;
            this.lblQuyDinh.Location = new System.Drawing.Point(20, 423);
            this.lblQuyDinh.Size = new System.Drawing.Size(355, 66);
            this.lblQuyDinh.Text = "(quy định tích điểm – nạp từ TichDiemBLL khi chạy)";
            this.lblQuyDinh.Name = "lblQuyDinh";
            this.lblQuyDinh.TabIndex = 23;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrangThai.Location = new System.Drawing.Point(20, 493);
            this.lblTrangThai.Size = new System.Drawing.Size(355, 26);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.TabIndex = 24;
            // 
            // lblHuongDan
            // 
            this.lblHuongDan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHuongDan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHuongDan.ForeColor = System.Drawing.Color.DimGray;
            this.lblHuongDan.Location = new System.Drawing.Point(20, 523);
            this.lblHuongDan.Size = new System.Drawing.Size(355, 66);
            this.lblHuongDan.Text = "• Thêm thuốc từ trang Tra cứu hoặc QUÉT MÃ VẠCH bên phải.\r\n• Khách cũ: nhập mã, tên + điểm tự hiện. Khách mới: nhập thêm tên.\r\n• Nhấp đúp một dòng bên phải để bỏ thuốc khỏi đơn.";
            this.lblHuongDan.Name = "lblHuongDan";
            this.lblHuongDan.TabIndex = 25;
            // 
            // pnlPhai
            // 
            this.pnlPhai.Controls.Add(this.dgvChiTiet);
            this.pnlPhai.Controls.Add(this.pnlGoiY);
            this.pnlPhai.Controls.Add(this.pnlTong);
            this.pnlPhai.Controls.Add(this.pnlQuet);
            this.pnlPhai.Controls.Add(this.lblTieuDePhai);
            this.pnlPhai.BackColor = System.Drawing.Color.White;
            this.pnlPhai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPhai.Location = new System.Drawing.Point(415, 15);
            this.pnlPhai.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.pnlPhai.Padding = new System.Windows.Forms.Padding(15);
            this.pnlPhai.Size = new System.Drawing.Size(570, 610);
            this.pnlPhai.Name = "pnlPhai";
            this.pnlPhai.TabIndex = 26;
            // 
            // dgvChiTiet
            // 
            this.dgvChiTiet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSTT,
            this.colMaThuoc,
            this.colTenThuoc,
            this.colSoLuong,
            this.colGiaBan,
            this.colThanhTien});
            this.dgvChiTiet.AllowUserToAddRows = false;
            this.dgvChiTiet.AllowUserToDeleteRows = false;
            this.dgvChiTiet.AllowUserToResizeRows = false;
            this.dgvChiTiet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChiTiet.BackgroundColor = System.Drawing.Color.White;
            this.dgvChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTiet.Location = new System.Drawing.Point(15, 112);
            this.dgvChiTiet.MultiSelect = false;
            this.dgvChiTiet.ReadOnly = true;
            this.dgvChiTiet.RowHeadersVisible = false;
            this.dgvChiTiet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChiTiet.Size = new System.Drawing.Size(540, 211);
            this.dgvChiTiet.Name = "dgvChiTiet";
            this.dgvChiTiet.TabIndex = 27;
            this.dgvChiTiet.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvChiTiet_CellDoubleClick);
            // 
            // pnlTong
            // 
            this.pnlTong.Controls.Add(this.lblGiamGiaNhan);
            this.pnlTong.Controls.Add(this.nudGiamGia);
            this.pnlTong.Controls.Add(this.lblTienHangNhan);
            this.pnlTong.Controls.Add(this.lblTienHang);
            this.pnlTong.Controls.Add(this.lblTienGiamNhan);
            this.pnlTong.Controls.Add(this.lblTienGiam);
            this.pnlTong.Controls.Add(this.lblTienDiemNhan);
            this.pnlTong.Controls.Add(this.lblTienDiem);
            this.pnlTong.Controls.Add(this.lblTongTienNhan);
            this.pnlTong.Controls.Add(this.lblTongTien);
            this.pnlTong.Controls.Add(this.lblDiemSeCong);
            this.pnlTong.Controls.Add(this.btnThanhToan);
            this.pnlTong.Controls.Add(this.btnLamMoi);
            this.pnlTong.Controls.Add(this.btnXuatExcel);
            this.pnlTong.Controls.Add(this.btnXuatPdf);
            this.pnlTong.Controls.Add(this.btnIn);
            this.pnlTong.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTong.Location = new System.Drawing.Point(15, 323);
            this.pnlTong.Size = new System.Drawing.Size(540, 272);
            this.pnlTong.Name = "pnlTong";
            this.pnlTong.TabIndex = 28;
            // 
            // pnlQuet
            // 
            this.pnlQuet.Controls.Add(this.lblQuetTieuDe);
            this.pnlQuet.Controls.Add(this.txtQuet);
            this.pnlQuet.Controls.Add(this.lblSLQuet);
            this.pnlQuet.Controls.Add(this.nudSLQuet);
            this.pnlQuet.Controls.Add(this.btnQuet);
            this.pnlQuet.Controls.Add(this.lblQuet);
            this.pnlQuet.Controls.Add(this.btnCamera);
            this.pnlQuet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(247)))), ((int)(((byte)(255)))));
            this.pnlQuet.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQuet.Location = new System.Drawing.Point(15, 50);
            this.pnlQuet.Padding = new System.Windows.Forms.Padding(8);
            this.pnlQuet.Size = new System.Drawing.Size(540, 62);
            this.pnlQuet.Name = "pnlQuet";
            this.pnlQuet.TabIndex = 29;
            // 
            // lblTieuDePhai
            // 
            this.lblTieuDePhai.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTieuDePhai.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDePhai.Location = new System.Drawing.Point(15, 15);
            this.lblTieuDePhai.Size = new System.Drawing.Size(540, 35);
            this.lblTieuDePhai.Text = "Chi Tiết Hóa Đơn";
            this.lblTieuDePhai.Name = "lblTieuDePhai";
            this.lblTieuDePhai.TabIndex = 30;
            // 
            // colSTT
            // 
            this.colSTT.FillWeight = 8F;
            this.colSTT.HeaderText = "STT";
            this.colSTT.ReadOnly = true;
            this.colSTT.Name = "colSTT";
            // 
            // colMaThuoc
            // 
            this.colMaThuoc.FillWeight = 15F;
            this.colMaThuoc.HeaderText = "Mã thuốc";
            this.colMaThuoc.ReadOnly = true;
            this.colMaThuoc.Name = "colMaThuoc";
            // 
            // colTenThuoc
            // 
            this.colTenThuoc.FillWeight = 37F;
            this.colTenThuoc.HeaderText = "Tên thuốc";
            this.colTenThuoc.ReadOnly = true;
            this.colTenThuoc.Name = "colTenThuoc";
            // 
            // colSoLuong
            // 
            this.colSoLuong.DefaultCellStyle = dgvCanhPhai1;
            this.colSoLuong.FillWeight = 10F;
            this.colSoLuong.HeaderText = "SL";
            this.colSoLuong.ReadOnly = true;
            this.colSoLuong.Name = "colSoLuong";
            // 
            // colGiaBan
            // 
            this.colGiaBan.DefaultCellStyle = dgvCanhPhai2;
            this.colGiaBan.FillWeight = 15F;
            this.colGiaBan.HeaderText = "Giá bán";
            this.colGiaBan.ReadOnly = true;
            this.colGiaBan.Name = "colGiaBan";
            // 
            // colThanhTien
            // 
            this.colThanhTien.DefaultCellStyle = dgvCanhPhai3;
            this.colThanhTien.FillWeight = 18F;
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.ReadOnly = true;
            this.colThanhTien.Name = "colThanhTien";
            // 
            // lblQuetTieuDe
            // 
            this.lblQuetTieuDe.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuetTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(160)))));
            this.lblQuetTieuDe.Location = new System.Drawing.Point(8, 12);
            this.lblQuetTieuDe.Size = new System.Drawing.Size(120, 23);
            this.lblQuetTieuDe.Text = "▌▌▌ Quét mã vạch:";
            this.lblQuetTieuDe.Name = "lblQuetTieuDe";
            this.lblQuetTieuDe.TabIndex = 31;
            // 
            // txtQuet
            // 
            this.txtQuet.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuet.Location = new System.Drawing.Point(130, 9);
            this.txtQuet.Size = new System.Drawing.Size(160, 27);
            this.txtQuet.Name = "txtQuet";
            this.txtQuet.TabIndex = 32;
            this.txtQuet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuet_KeyDown);
            // 
            // lblSLQuet
            // 
            this.lblSLQuet.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSLQuet.Location = new System.Drawing.Point(296, 12);
            this.lblSLQuet.Size = new System.Drawing.Size(26, 23);
            this.lblSLQuet.Text = "SL:";
            this.lblSLQuet.Name = "lblSLQuet";
            this.lblSLQuet.TabIndex = 33;
            // 
            // nudSLQuet
            // 
            this.nudSLQuet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSLQuet.Location = new System.Drawing.Point(322, 9);
            this.nudSLQuet.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudSLQuet.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSLQuet.Size = new System.Drawing.Size(55, 25);
            this.nudSLQuet.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSLQuet.Name = "nudSLQuet";
            this.nudSLQuet.TabIndex = 34;
            // 
            // btnQuet
            // 
            this.btnQuet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnQuet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuet.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuet.ForeColor = System.Drawing.Color.White;
            this.btnQuet.Location = new System.Drawing.Point(383, 8);
            this.btnQuet.Size = new System.Drawing.Size(70, 30);
            this.btnQuet.Text = "Thêm";
            this.btnQuet.UseVisualStyleBackColor = false;
            this.btnQuet.Name = "btnQuet";
            this.btnQuet.TabIndex = 35;
            this.btnQuet.Click += new System.EventHandler(this.btnQuet_Click);
            // 
            // lblQuet
            // 
            this.lblQuet.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuet.ForeColor = System.Drawing.Color.DimGray;
            this.lblQuet.Location = new System.Drawing.Point(8, 40);
            this.lblQuet.Size = new System.Drawing.Size(460, 18);
            this.lblQuet.Text = "Đặt con trỏ vào ô rồi quét, hoặc gõ mã vạch / mã thuốc và nhấn Enter.";
            this.lblQuet.Name = "lblQuet";
            this.lblQuet.TabIndex = 36;
            // 
            // lblGiamGiaNhan
            // 
            this.lblGiamGiaNhan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGiamGiaNhan.Location = new System.Drawing.Point(0, 11);
            this.lblGiamGiaNhan.Size = new System.Drawing.Size(135, 23);
            this.lblGiamGiaNhan.Text = "Giảm giá (%):";
            this.lblGiamGiaNhan.Name = "lblGiamGiaNhan";
            this.lblGiamGiaNhan.TabIndex = 37;
            // 
            // nudGiamGia
            // 
            this.nudGiamGia.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudGiamGia.Location = new System.Drawing.Point(140, 8);
            this.nudGiamGia.Size = new System.Drawing.Size(90, 25);
            this.nudGiamGia.Name = "nudGiamGia";
            this.nudGiamGia.TabIndex = 38;
            this.nudGiamGia.ValueChanged += new System.EventHandler(this.nudGiamGia_ValueChanged);
            // 
            // lblTienHangNhan
            // 
            this.lblTienHangNhan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTienHangNhan.Location = new System.Drawing.Point(0, 44);
            this.lblTienHangNhan.Size = new System.Drawing.Size(135, 23);
            this.lblTienHangNhan.Text = "Tiền hàng:";
            this.lblTienHangNhan.Name = "lblTienHangNhan";
            this.lblTienHangNhan.TabIndex = 39;
            // 
            // lblTienHang
            // 
            this.lblTienHang.AutoSize = true;
            this.lblTienHang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTienHang.Location = new System.Drawing.Point(140, 44);
            this.lblTienHang.Text = "0 đ";
            this.lblTienHang.Name = "lblTienHang";
            this.lblTienHang.TabIndex = 40;
            // 
            // lblTienGiamNhan
            // 
            this.lblTienGiamNhan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTienGiamNhan.Location = new System.Drawing.Point(0, 68);
            this.lblTienGiamNhan.Size = new System.Drawing.Size(135, 23);
            this.lblTienGiamNhan.Text = "Giảm giá:";
            this.lblTienGiamNhan.Name = "lblTienGiamNhan";
            this.lblTienGiamNhan.TabIndex = 41;
            // 
            // lblTienGiam
            // 
            this.lblTienGiam.AutoSize = true;
            this.lblTienGiam.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTienGiam.Location = new System.Drawing.Point(140, 68);
            this.lblTienGiam.Text = "0 đ";
            this.lblTienGiam.Name = "lblTienGiam";
            this.lblTienGiam.TabIndex = 42;
            // 
            // lblTienDiemNhan
            // 
            this.lblTienDiemNhan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTienDiemNhan.Location = new System.Drawing.Point(0, 92);
            this.lblTienDiemNhan.Size = new System.Drawing.Size(135, 23);
            this.lblTienDiemNhan.Text = "Đổi điểm:";
            this.lblTienDiemNhan.Name = "lblTienDiemNhan";
            this.lblTienDiemNhan.TabIndex = 43;
            // 
            // lblTienDiem
            // 
            this.lblTienDiem.AutoSize = true;
            this.lblTienDiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTienDiem.Location = new System.Drawing.Point(140, 92);
            this.lblTienDiem.Text = "0 đ";
            this.lblTienDiem.Name = "lblTienDiem";
            this.lblTienDiem.TabIndex = 44;
            // 
            // lblTongTienNhan
            // 
            this.lblTongTienNhan.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTienNhan.Location = new System.Drawing.Point(0, 126);
            this.lblTongTienNhan.Size = new System.Drawing.Size(135, 23);
            this.lblTongTienNhan.Text = "TỔNG TIỀN:";
            this.lblTongTienNhan.Name = "lblTongTienNhan";
            this.lblTongTienNhan.TabIndex = 45;
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTien.Location = new System.Drawing.Point(140, 120);
            this.lblTongTien.Text = "0 đ";
            this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.TabIndex = 46;
            // 
            // lblDiemSeCong
            // 
            this.lblDiemSeCong.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiemSeCong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(140)))), ((int)(((byte)(60)))));
            this.lblDiemSeCong.Location = new System.Drawing.Point(0, 156);
            this.lblDiemSeCong.Size = new System.Drawing.Size(470, 20);
            this.lblDiemSeCong.Name = "lblDiemSeCong";
            this.lblDiemSeCong.TabIndex = 47;
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThanhToan.ForeColor = System.Drawing.Color.White;
            this.btnThanhToan.Location = new System.Drawing.Point(0, 182);
            this.btnThanhToan.Size = new System.Drawing.Size(150, 40);
            this.btnThanhToan.Text = "Thanh toán";
            this.btnThanhToan.UseVisualStyleBackColor = false;
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.TabIndex = 48;
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(158, 182);
            this.btnLamMoi.Size = new System.Drawing.Size(110, 40);
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.TabIndex = 49;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnXuatExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatExcel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatExcel.ForeColor = System.Drawing.Color.White;
            this.btnXuatExcel.Location = new System.Drawing.Point(276, 182);
            this.btnXuatExcel.Size = new System.Drawing.Size(160, 40);
            this.btnXuatExcel.Text = "Xuất hóa đơn Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = false;
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.TabIndex = 50;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // btnXuatPdf
            // 
            this.btnXuatPdf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(35)))), ((int)(((byte)(51)))));
            this.btnXuatPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatPdf.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatPdf.ForeColor = System.Drawing.Color.White;
            this.btnXuatPdf.Location = new System.Drawing.Point(0, 228);
            this.btnXuatPdf.Size = new System.Drawing.Size(150, 40);
            this.btnXuatPdf.Text = "Xuất hóa đơn PDF";
            this.btnXuatPdf.UseVisualStyleBackColor = false;
            this.btnXuatPdf.Name = "btnXuatPdf";
            this.btnXuatPdf.TabIndex = 51;
            this.btnXuatPdf.Click += new System.EventHandler(this.btnXuatPdf_Click);
            // 
            // btnIn
            // 
            this.btnIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIn.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIn.ForeColor = System.Drawing.Color.White;
            this.btnIn.Location = new System.Drawing.Point(158, 228);
            this.btnIn.Size = new System.Drawing.Size(160, 40);
            this.btnIn.Text = "🖨 Xem trước && In";
            this.btnIn.UseVisualStyleBackColor = false;
            this.btnIn.Name = "btnIn";
            this.btnIn.TabIndex = 52;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnCamera   [11/11/2026 lần 7] quét mã vạch bằng webcam
            // 
            this.btnCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera.ForeColor = System.Drawing.Color.White;
            this.btnCamera.Location = new System.Drawing.Point(458, 8);
            this.btnCamera.Size = new System.Drawing.Size(78, 30);
            this.btnCamera.Text = "📷 Camera";
            this.btnCamera.UseVisualStyleBackColor = false;
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.TabIndex = 36;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // pnlGoiY   [11/11/2026 lần 7] cảnh báo tương tác thuốc + gợi ý mua kèm (ẩn khi không có gì)
            // 
            this.pnlGoiY.Controls.Add(this.flpMuaKem);
            this.pnlGoiY.Controls.Add(this.lblTuongTac);
            this.pnlGoiY.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlGoiY.Location = new System.Drawing.Point(15, 263);
            this.pnlGoiY.Size = new System.Drawing.Size(540, 60);
            this.pnlGoiY.Visible = false;
            this.pnlGoiY.Name = "pnlGoiY";
            this.pnlGoiY.TabIndex = 54;
            // 
            // flpMuaKem
            // 
            this.flpMuaKem.Controls.Add(this.lblMuaKemTieuDe);
            this.flpMuaKem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(247)))), ((int)(((byte)(255)))));
            this.flpMuaKem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMuaKem.Location = new System.Drawing.Point(0, 30);
            this.flpMuaKem.Padding = new System.Windows.Forms.Padding(4, 3, 4, 0);
            this.flpMuaKem.Size = new System.Drawing.Size(540, 30);
            this.flpMuaKem.Name = "flpMuaKem";
            this.flpMuaKem.TabIndex = 1;
            // 
            // lblMuaKemTieuDe
            // 
            this.lblMuaKemTieuDe.AutoSize = true;
            this.lblMuaKemTieuDe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMuaKemTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(160)))));
            this.lblMuaKemTieuDe.Margin = new System.Windows.Forms.Padding(0, 4, 4, 0);
            this.lblMuaKemTieuDe.Text = "💡 Khách hay mua kèm:";
            this.lblMuaKemTieuDe.Name = "lblMuaKemTieuDe";
            this.lblMuaKemTieuDe.TabIndex = 0;
            // 
            // lblTuongTac
            // 
            this.lblTuongTac.AutoEllipsis = true;
            this.lblTuongTac.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblTuongTac.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblTuongTac.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTuongTac.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTuongTac.ForeColor = System.Drawing.Color.White;
            this.lblTuongTac.Location = new System.Drawing.Point(0, 0);
            this.lblTuongTac.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTuongTac.Size = new System.Drawing.Size(540, 30);
            this.lblTuongTac.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTuongTac.Visible = false;
            this.lblTuongTac.Name = "lblTuongTac";
            this.lblTuongTac.TabIndex = 0;
            this.lblTuongTac.Click += new System.EventHandler(this.lblTuongTac_Click);
            // 
            // tmrKhach
            // 
            this.tmrKhach.Interval = 400;
            this.tmrKhach.Tick += new System.EventHandler(this.tmrKhach_Tick);
            // 
            // frmBanHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Text = "Bán Hàng";
            this.Controls.Add(this.tbl);
            this.Name = "frmBanHang";
            ((System.ComponentModel.ISupportInitialize)(this.nudGiamGia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSLQuet)).EndInit();
            this.pnlQuet.ResumeLayout(false);
            this.pnlQuet.PerformLayout();
            this.flpMuaKem.ResumeLayout(false);
            this.flpMuaKem.PerformLayout();
            this.pnlGoiY.ResumeLayout(false);
            this.pnlTong.ResumeLayout(false);
            this.pnlTong.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.pnlPhai.ResumeLayout(false);
            this.pnlPhai.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDungDiem)).EndInit();
            this.pnlTrai.ResumeLayout(false);
            this.pnlTrai.PerformLayout();
            this.tbl.ResumeLayout(false);
            this.tbl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbl;
        private System.Windows.Forms.Panel pnlTrai;
        private System.Windows.Forms.Label lblTieuDeTrai;
        private System.Windows.Forms.Label lblMaHDB;
        private System.Windows.Forms.TextBox txtMaHDB;
        private System.Windows.Forms.Label lblNgayLap;
        private System.Windows.Forms.TextBox txtNgayLap;
        private System.Windows.Forms.Label lblNhanVien;
        private System.Windows.Forms.ComboBox cboNhanVien;
        private System.Windows.Forms.Label lblMaKhach;
        private System.Windows.Forms.TextBox txtMaKhach;
        private System.Windows.Forms.Label lblTenKhach;
        private System.Windows.Forms.TextBox txtTenKhach;
        private System.Windows.Forms.Label lblKhachInfo;
        private System.Windows.Forms.Label lblMaDonThuoc;
        private System.Windows.Forms.TextBox txtMaDonThuoc;
        private System.Windows.Forms.Label lblKeDon;
        private System.Windows.Forms.Label lblSepDiem;
        private System.Windows.Forms.Label lblDiemTieuDe;
        private System.Windows.Forms.Label lblDiemKhach;
        private System.Windows.Forms.Label lblDungDiem;
        private System.Windows.Forms.NumericUpDown nudDungDiem;
        private System.Windows.Forms.Label lblQuyDoi;
        private System.Windows.Forms.Label lblQuyDinh;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.Label lblHuongDan;
        private System.Windows.Forms.Panel pnlPhai;
        private System.Windows.Forms.DataGridView dgvChiTiet;
        private System.Windows.Forms.Panel pnlTong;
        private System.Windows.Forms.Panel pnlQuet;
        private System.Windows.Forms.Label lblTieuDePhai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;
        private System.Windows.Forms.Label lblQuetTieuDe;
        private System.Windows.Forms.TextBox txtQuet;
        private System.Windows.Forms.Label lblSLQuet;
        private System.Windows.Forms.NumericUpDown nudSLQuet;
        private System.Windows.Forms.Button btnQuet;
        private System.Windows.Forms.Label lblQuet;
        private System.Windows.Forms.Label lblGiamGiaNhan;
        private System.Windows.Forms.NumericUpDown nudGiamGia;
        private System.Windows.Forms.Label lblTienHangNhan;
        private System.Windows.Forms.Label lblTienHang;
        private System.Windows.Forms.Label lblTienGiamNhan;
        private System.Windows.Forms.Label lblTienGiam;
        private System.Windows.Forms.Label lblTienDiemNhan;
        private System.Windows.Forms.Label lblTienDiem;
        private System.Windows.Forms.Label lblTongTienNhan;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label lblDiemSeCong;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnXuatPdf;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Timer tmrKhach;
        // [11/11/2026 lần 7]
        private System.Windows.Forms.Button btnCamera;
        private System.Windows.Forms.Panel pnlGoiY;
        private System.Windows.Forms.FlowLayoutPanel flpMuaKem;
        private System.Windows.Forms.Label lblMuaKemTieuDe;
        private System.Windows.Forms.Label lblTuongTac;
        private System.Windows.Forms.ToolTip ttGoiY;
    }
}
