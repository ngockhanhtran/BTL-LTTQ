namespace QuanLyNhaThuoc.GUI
{
    partial class frmThuoc
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
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.dgvThuoc = new System.Windows.Forms.DataGridView();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtTuKhoa = new System.Windows.Forms.TextBox();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblMaThuoc = new System.Windows.Forms.Label();
            this.txtMaThuoc = new System.Windows.Forms.TextBox();
            this.lblTenThuoc = new System.Windows.Forms.Label();
            this.txtTenThuoc = new System.Windows.Forms.TextBox();
            this.lblThanhPhan = new System.Windows.Forms.Label();
            this.txtThanhPhan = new System.Windows.Forms.TextBox();
            this.lblGiaNhap = new System.Windows.Forms.Label();
            this.txtGiaNhap = new System.Windows.Forms.TextBox();
            this.lblGiaBan = new System.Windows.Forms.Label();
            this.txtGiaBan = new System.Windows.Forms.TextBox();
            this.lblSLHienCo = new System.Windows.Forms.Label();
            this.txtSLHienCo = new System.Windows.Forms.TextBox();
            this.lblCongDung = new System.Windows.Forms.Label();
            this.txtCongDung = new System.Windows.Forms.TextBox();
            this.lblChongChiDinh = new System.Windows.Forms.Label();
            this.txtChongChiDinh = new System.Windows.Forms.TextBox();
            this.btnXemLo = new System.Windows.Forms.Button();
            this.btnSuaCongDung = new System.Windows.Forms.Button();
            this.btnLuuCCD = new System.Windows.Forms.Button();
            this.pnlHoaDon = new System.Windows.Forms.Panel();
            this.lblSep = new System.Windows.Forms.Label();
            this.lblHDTitle = new System.Windows.Forms.Label();
            this.lblMaHDB = new System.Windows.Forms.Label();
            this.lblSL = new System.Windows.Forms.Label();
            this.nudSoLuong = new System.Windows.Forms.NumericUpDown();
            this.btnThemVaoHD = new System.Windows.Forms.Button();
            this.btnThanhToanHD = new System.Windows.Forms.Button();
            this.lblGioHang = new System.Windows.Forms.Label();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThuoc)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlHoaDon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.dgvThuoc);
            this.pnlLeft.Controls.Add(this.pnlSearch);
            this.pnlLeft.BackColor = System.Drawing.Color.White;
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(15);
            this.pnlLeft.Size = new System.Drawing.Size(620, 640);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.TabIndex = 0;
            // 
            // dgvThuoc
            // 
            this.dgvThuoc.AllowUserToAddRows = false;
            this.dgvThuoc.AllowUserToDeleteRows = false;
            this.dgvThuoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvThuoc.BackgroundColor = System.Drawing.Color.White;
            this.dgvThuoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThuoc.Location = new System.Drawing.Point(15, 85);
            this.dgvThuoc.MultiSelect = false;
            this.dgvThuoc.ReadOnly = true;
            this.dgvThuoc.RowHeadersVisible = false;
            this.dgvThuoc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThuoc.Size = new System.Drawing.Size(590, 540);
            this.dgvThuoc.Name = "dgvThuoc";
            this.dgvThuoc.TabIndex = 1;
            this.dgvThuoc.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvThuoc_DataBindingComplete);
            this.dgvThuoc.SelectionChanged += new System.EventHandler(this.DgvThuoc_SelectionChanged);
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Controls.Add(this.txtTuKhoa);
            this.pnlSearch.Controls.Add(this.btnTimKiem);
            this.pnlSearch.Controls.Add(this.btnLamMoi);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(15, 15);
            this.pnlSearch.Size = new System.Drawing.Size(590, 70);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.TabIndex = 2;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.Location = new System.Drawing.Point(0, 0);
            this.lblSearch.Text = "Tra cứu thuốc (Tên/Thành phần/Công dụng):";
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.TabIndex = 3;
            // 
            // txtTuKhoa
            // 
            this.txtTuKhoa.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTuKhoa.Location = new System.Drawing.Point(0, 27);
            this.txtTuKhoa.Size = new System.Drawing.Size(320, 25);
            this.txtTuKhoa.Name = "txtTuKhoa";
            this.txtTuKhoa.TabIndex = 4;
            this.txtTuKhoa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTuKhoa_KeyDown);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(330, 25);
            this.btnTimKiem.Size = new System.Drawing.Size(90, 30);
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.TabIndex = 5;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(430, 25);
            this.btnLamMoi.Size = new System.Drawing.Size(90, 30);
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.TabIndex = 6;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.lblFormTitle);
            this.pnlRight.Controls.Add(this.lblMaThuoc);
            this.pnlRight.Controls.Add(this.txtMaThuoc);
            this.pnlRight.Controls.Add(this.lblTenThuoc);
            this.pnlRight.Controls.Add(this.txtTenThuoc);
            this.pnlRight.Controls.Add(this.lblThanhPhan);
            this.pnlRight.Controls.Add(this.txtThanhPhan);
            this.pnlRight.Controls.Add(this.lblGiaNhap);
            this.pnlRight.Controls.Add(this.txtGiaNhap);
            this.pnlRight.Controls.Add(this.lblGiaBan);
            this.pnlRight.Controls.Add(this.txtGiaBan);
            this.pnlRight.Controls.Add(this.lblSLHienCo);
            this.pnlRight.Controls.Add(this.txtSLHienCo);
            this.pnlRight.Controls.Add(this.lblCongDung);
            this.pnlRight.Controls.Add(this.txtCongDung);
            this.pnlRight.Controls.Add(this.lblChongChiDinh);
            this.pnlRight.Controls.Add(this.txtChongChiDinh);
            this.pnlRight.Controls.Add(this.btnXemLo);
            this.pnlRight.Controls.Add(this.btnSuaCongDung);
            this.pnlRight.Controls.Add(this.btnLuuCCD);
            this.pnlRight.Controls.Add(this.pnlHoaDon);
            this.pnlRight.AutoScroll = true;
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(620, 0);
            this.pnlRight.Padding = new System.Windows.Forms.Padding(15);
            this.pnlRight.Size = new System.Drawing.Size(380, 640);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.TabIndex = 7;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormTitle.Location = new System.Drawing.Point(15, 15);
            this.lblFormTitle.Text = "Thông Tin Chi Tiết Thuốc";
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.TabIndex = 8;
            // 
            // lblMaThuoc
            // 
            this.lblMaThuoc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaThuoc.Location = new System.Drawing.Point(15, 58);
            this.lblMaThuoc.Size = new System.Drawing.Size(98, 23);
            this.lblMaThuoc.Text = "Mã thuốc:";
            this.lblMaThuoc.Name = "lblMaThuoc";
            this.lblMaThuoc.TabIndex = 9;
            // 
            // txtMaThuoc
            // 
            this.txtMaThuoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtMaThuoc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaThuoc.Location = new System.Drawing.Point(115, 55);
            this.txtMaThuoc.ReadOnly = true;
            this.txtMaThuoc.TabStop = false;
            this.txtMaThuoc.Size = new System.Drawing.Size(230, 25);
            this.txtMaThuoc.Name = "txtMaThuoc";
            this.txtMaThuoc.TabIndex = 10;
            // 
            // lblTenThuoc
            // 
            this.lblTenThuoc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenThuoc.Location = new System.Drawing.Point(15, 95);
            this.lblTenThuoc.Size = new System.Drawing.Size(98, 23);
            this.lblTenThuoc.Text = "Tên thuốc:";
            this.lblTenThuoc.Name = "lblTenThuoc";
            this.lblTenThuoc.TabIndex = 11;
            // 
            // txtTenThuoc
            // 
            this.txtTenThuoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtTenThuoc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTenThuoc.Location = new System.Drawing.Point(115, 92);
            this.txtTenThuoc.ReadOnly = true;
            this.txtTenThuoc.TabStop = false;
            this.txtTenThuoc.Size = new System.Drawing.Size(230, 25);
            this.txtTenThuoc.Name = "txtTenThuoc";
            this.txtTenThuoc.TabIndex = 12;
            // 
            // lblThanhPhan
            // 
            this.lblThanhPhan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThanhPhan.Location = new System.Drawing.Point(15, 132);
            this.lblThanhPhan.Size = new System.Drawing.Size(98, 23);
            this.lblThanhPhan.Text = "Thành phần:";
            this.lblThanhPhan.Name = "lblThanhPhan";
            this.lblThanhPhan.TabIndex = 13;
            // 
            // txtThanhPhan
            // 
            this.txtThanhPhan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtThanhPhan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThanhPhan.Location = new System.Drawing.Point(115, 129);
            this.txtThanhPhan.ReadOnly = true;
            this.txtThanhPhan.TabStop = false;
            this.txtThanhPhan.Multiline = true;
            this.txtThanhPhan.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtThanhPhan.Size = new System.Drawing.Size(230, 52);
            this.txtThanhPhan.Name = "txtThanhPhan";
            this.txtThanhPhan.TabIndex = 14;
            // 
            // lblGiaNhap
            // 
            this.lblGiaNhap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGiaNhap.Location = new System.Drawing.Point(15, 196);
            this.lblGiaNhap.Size = new System.Drawing.Size(98, 23);
            this.lblGiaNhap.Text = "Giá nhập:";
            this.lblGiaNhap.Name = "lblGiaNhap";
            this.lblGiaNhap.TabIndex = 15;
            // 
            // txtGiaNhap
            // 
            this.txtGiaNhap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtGiaNhap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGiaNhap.Location = new System.Drawing.Point(115, 193);
            this.txtGiaNhap.ReadOnly = true;
            this.txtGiaNhap.TabStop = false;
            this.txtGiaNhap.Size = new System.Drawing.Size(230, 25);
            this.txtGiaNhap.Name = "txtGiaNhap";
            this.txtGiaNhap.TabIndex = 16;
            // 
            // lblGiaBan
            // 
            this.lblGiaBan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGiaBan.Location = new System.Drawing.Point(15, 233);
            this.lblGiaBan.Size = new System.Drawing.Size(98, 23);
            this.lblGiaBan.Text = "Giá bán:";
            this.lblGiaBan.Name = "lblGiaBan";
            this.lblGiaBan.TabIndex = 17;
            // 
            // txtGiaBan
            // 
            this.txtGiaBan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtGiaBan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGiaBan.Location = new System.Drawing.Point(115, 230);
            this.txtGiaBan.ReadOnly = true;
            this.txtGiaBan.TabStop = false;
            this.txtGiaBan.Size = new System.Drawing.Size(230, 25);
            this.txtGiaBan.Name = "txtGiaBan";
            this.txtGiaBan.TabIndex = 18;
            // 
            // lblSLHienCo
            // 
            this.lblSLHienCo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSLHienCo.Location = new System.Drawing.Point(15, 270);
            this.lblSLHienCo.Size = new System.Drawing.Size(98, 23);
            this.lblSLHienCo.Text = "SL hiện có:";
            this.lblSLHienCo.Name = "lblSLHienCo";
            this.lblSLHienCo.TabIndex = 19;
            // 
            // txtSLHienCo
            // 
            this.txtSLHienCo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtSLHienCo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSLHienCo.Location = new System.Drawing.Point(115, 267);
            this.txtSLHienCo.ReadOnly = true;
            this.txtSLHienCo.TabStop = false;
            this.txtSLHienCo.Size = new System.Drawing.Size(120, 25);
            this.txtSLHienCo.Name = "txtSLHienCo";
            this.txtSLHienCo.TabIndex = 20;
            // 
            // lblCongDung
            // 
            this.lblCongDung.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCongDung.Location = new System.Drawing.Point(15, 307);
            this.lblCongDung.Size = new System.Drawing.Size(98, 23);
            this.lblCongDung.Text = "Công dụng:";
            this.lblCongDung.Name = "lblCongDung";
            this.lblCongDung.TabIndex = 21;
            // 
            // txtCongDung
            // 
            this.txtCongDung.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtCongDung.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCongDung.Location = new System.Drawing.Point(115, 304);
            this.txtCongDung.ReadOnly = true;
            this.txtCongDung.TabStop = false;
            this.txtCongDung.Multiline = true;
            this.txtCongDung.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCongDung.Size = new System.Drawing.Size(230, 52);
            this.txtCongDung.Name = "txtCongDung";
            this.txtCongDung.TabIndex = 22;
            // 
            // lblChongChiDinh
            // 
            this.lblChongChiDinh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChongChiDinh.Location = new System.Drawing.Point(15, 371);
            this.lblChongChiDinh.Size = new System.Drawing.Size(98, 23);
            this.lblChongChiDinh.Text = "Chống chỉ định:";
            this.lblChongChiDinh.Name = "lblChongChiDinh";
            this.lblChongChiDinh.TabIndex = 23;
            // 
            // txtChongChiDinh
            // 
            this.txtChongChiDinh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtChongChiDinh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChongChiDinh.Location = new System.Drawing.Point(115, 368);
            this.txtChongChiDinh.ReadOnly = true;
            this.txtChongChiDinh.TabStop = false;
            this.txtChongChiDinh.Multiline = true;
            this.txtChongChiDinh.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChongChiDinh.Size = new System.Drawing.Size(230, 52);
            this.txtChongChiDinh.Name = "txtChongChiDinh";
            this.txtChongChiDinh.TabIndex = 24;
            // 
            // btnXemLo
            // 
            this.btnXemLo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnXemLo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXemLo.FlatAppearance.BorderSize = 0;
            this.btnXemLo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXemLo.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXemLo.ForeColor = System.Drawing.Color.White;
            this.btnXemLo.Location = new System.Drawing.Point(240, 266);
            this.btnXemLo.Size = new System.Drawing.Size(105, 27);
            this.btnXemLo.Text = "🧾 Xem các lô";
            this.btnXemLo.UseVisualStyleBackColor = false;
            this.btnXemLo.Name = "btnXemLo";
            this.btnXemLo.TabIndex = 25;
            this.btnXemLo.Click += new System.EventHandler(this.btnXemLo_Click);
            // 
            // btnSuaCongDung
            // 
            this.btnSuaCongDung.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSuaCongDung.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSuaCongDung.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuaCongDung.ForeColor = System.Drawing.Color.White;
            this.btnSuaCongDung.Location = new System.Drawing.Point(15, 432);
            this.btnSuaCongDung.Size = new System.Drawing.Size(160, 38);
            this.btnSuaCongDung.Text = "✎ Sửa công dụng...";
            this.btnSuaCongDung.UseVisualStyleBackColor = false;
            this.btnSuaCongDung.Name = "btnSuaCongDung";
            this.btnSuaCongDung.TabIndex = 26;
            this.btnSuaCongDung.Click += new System.EventHandler(this.btnSuaCongDung_Click);
            // 
            // btnLuuCCD
            // 
            this.btnLuuCCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnLuuCCD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuuCCD.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuuCCD.ForeColor = System.Drawing.Color.White;
            this.btnLuuCCD.Location = new System.Drawing.Point(185, 432);
            this.btnLuuCCD.Size = new System.Drawing.Size(160, 38);
            this.btnLuuCCD.Text = "💾 Lưu chống chỉ định";
            this.btnLuuCCD.UseVisualStyleBackColor = false;
            this.btnLuuCCD.Name = "btnLuuCCD";
            this.btnLuuCCD.TabIndex = 27;
            this.btnLuuCCD.Click += new System.EventHandler(this.btnLuuCCD_Click);
            // 
            // pnlHoaDon
            // 
            this.pnlHoaDon.Controls.Add(this.lblSep);
            this.pnlHoaDon.Controls.Add(this.lblHDTitle);
            this.pnlHoaDon.Controls.Add(this.lblMaHDB);
            this.pnlHoaDon.Controls.Add(this.lblSL);
            this.pnlHoaDon.Controls.Add(this.nudSoLuong);
            this.pnlHoaDon.Controls.Add(this.btnThemVaoHD);
            this.pnlHoaDon.Controls.Add(this.btnThanhToanHD);
            this.pnlHoaDon.Controls.Add(this.lblGioHang);
            this.pnlHoaDon.Location = new System.Drawing.Point(0, 478);
            this.pnlHoaDon.Size = new System.Drawing.Size(360, 255);
            this.pnlHoaDon.Name = "pnlHoaDon";
            this.pnlHoaDon.TabIndex = 28;
            // 
            // lblSep
            // 
            this.lblSep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSep.Location = new System.Drawing.Point(15, 2);
            this.lblSep.Size = new System.Drawing.Size(330, 2);
            this.lblSep.Name = "lblSep";
            this.lblSep.TabIndex = 29;
            // 
            // lblHDTitle
            // 
            this.lblHDTitle.AutoSize = true;
            this.lblHDTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHDTitle.Location = new System.Drawing.Point(15, 15);
            this.lblHDTitle.Text = "Lập hóa đơn bán";
            this.lblHDTitle.Name = "lblHDTitle";
            this.lblHDTitle.TabIndex = 30;
            // 
            // lblMaHDB
            // 
            this.lblMaHDB.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaHDB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblMaHDB.Location = new System.Drawing.Point(15, 43);
            this.lblMaHDB.Size = new System.Drawing.Size(330, 22);
            this.lblMaHDB.Text = "Mã hóa đơn:";
            this.lblMaHDB.Name = "lblMaHDB";
            this.lblMaHDB.TabIndex = 31;
            // 
            // lblSL
            // 
            this.lblSL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSL.Location = new System.Drawing.Point(15, 78);
            this.lblSL.Size = new System.Drawing.Size(98, 23);
            this.lblSL.Text = "Số lượng:";
            this.lblSL.Name = "lblSL";
            this.lblSL.TabIndex = 32;
            // 
            // nudSoLuong
            // 
            this.nudSoLuong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSoLuong.Location = new System.Drawing.Point(115, 75);
            this.nudSoLuong.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudSoLuong.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSoLuong.Size = new System.Drawing.Size(100, 25);
            this.nudSoLuong.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSoLuong.Name = "nudSoLuong";
            this.nudSoLuong.TabIndex = 33;
            // 
            // btnThemVaoHD
            // 
            this.btnThemVaoHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnThemVaoHD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemVaoHD.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemVaoHD.ForeColor = System.Drawing.Color.White;
            this.btnThemVaoHD.Location = new System.Drawing.Point(15, 115);
            this.btnThemVaoHD.Size = new System.Drawing.Size(330, 38);
            this.btnThemVaoHD.Text = "Thêm vào hóa đơn";
            this.btnThemVaoHD.UseVisualStyleBackColor = false;
            this.btnThemVaoHD.Name = "btnThemVaoHD";
            this.btnThemVaoHD.TabIndex = 34;
            this.btnThemVaoHD.Click += new System.EventHandler(this.btnThemVaoHD_Click);
            // 
            // btnThanhToanHD
            // 
            this.btnThanhToanHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnThanhToanHD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThanhToanHD.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThanhToanHD.ForeColor = System.Drawing.Color.White;
            this.btnThanhToanHD.Location = new System.Drawing.Point(15, 161);
            this.btnThanhToanHD.Size = new System.Drawing.Size(330, 38);
            this.btnThanhToanHD.Text = "Thanh toán với hóa đơn";
            this.btnThanhToanHD.UseVisualStyleBackColor = false;
            this.btnThanhToanHD.Name = "btnThanhToanHD";
            this.btnThanhToanHD.TabIndex = 35;
            this.btnThanhToanHD.Click += new System.EventHandler(this.btnThanhToanHD_Click);
            // 
            // lblGioHang
            // 
            this.lblGioHang.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGioHang.ForeColor = System.Drawing.Color.DimGray;
            this.lblGioHang.Location = new System.Drawing.Point(15, 209);
            this.lblGioHang.Size = new System.Drawing.Size(330, 40);
            this.lblGioHang.Name = "lblGioHang";
            this.lblGioHang.TabIndex = 36;
            // 
            // frmThuoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Text = "Tra cứu Thuốc";
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlRight);
            this.Name = "frmThuoc";
            this.VisibleChanged += new System.EventHandler(this.frmThuoc_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).EndInit();
            this.pnlHoaDon.ResumeLayout(false);
            this.pnlHoaDon.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThuoc)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.DataGridView dgvThuoc;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtTuKhoa;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblMaThuoc;
        private System.Windows.Forms.TextBox txtMaThuoc;
        private System.Windows.Forms.Label lblTenThuoc;
        private System.Windows.Forms.TextBox txtTenThuoc;
        private System.Windows.Forms.Label lblThanhPhan;
        private System.Windows.Forms.TextBox txtThanhPhan;
        private System.Windows.Forms.Label lblGiaNhap;
        private System.Windows.Forms.TextBox txtGiaNhap;
        private System.Windows.Forms.Label lblGiaBan;
        private System.Windows.Forms.TextBox txtGiaBan;
        private System.Windows.Forms.Label lblSLHienCo;
        private System.Windows.Forms.TextBox txtSLHienCo;
        private System.Windows.Forms.Label lblCongDung;
        private System.Windows.Forms.TextBox txtCongDung;
        private System.Windows.Forms.Label lblChongChiDinh;
        private System.Windows.Forms.TextBox txtChongChiDinh;
        private System.Windows.Forms.Button btnXemLo;
        private System.Windows.Forms.Button btnSuaCongDung;
        private System.Windows.Forms.Button btnLuuCCD;
        private System.Windows.Forms.Panel pnlHoaDon;
        private System.Windows.Forms.Label lblSep;
        private System.Windows.Forms.Label lblHDTitle;
        private System.Windows.Forms.Label lblMaHDB;
        private System.Windows.Forms.Label lblSL;
        private System.Windows.Forms.NumericUpDown nudSoLuong;
        private System.Windows.Forms.Button btnThemVaoHD;
        private System.Windows.Forms.Button btnThanhToanHD;
        private System.Windows.Forms.Label lblGioHang;
    }
}
