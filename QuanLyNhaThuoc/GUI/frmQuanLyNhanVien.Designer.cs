namespace QuanLyNhaThuoc.GUI
{
    partial class frmQuanLyNhanVien
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
            System.Windows.Forms.DataGridViewCellStyle dgvStyleXen = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvStyleTieuDe = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvStyleO = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTren = new System.Windows.Forms.Panel();
            this.lblTim = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnTim = new System.Windows.Forms.Button();
            this.lblLoc = new System.Windows.Forms.Label();
            this.cboLoc = new System.Windows.Forms.ComboBox();
            this.lblYeuCau = new System.Windows.Forms.Label();
            this.pnlGiua = new System.Windows.Forms.Panel();
            this.pnlLuoi = new System.Windows.Forms.Panel();
            this.pnlPhai = new System.Windows.Forms.Panel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.lblTieuDeKhung = new System.Windows.Forms.Label();
            this.lblCheDo = new System.Windows.Forms.Label();
            this.lblCanhBaoNV = new System.Windows.Forms.Label();
            this.lblMaNV = new System.Windows.Forms.Label();
            this.txtMaNV = new System.Windows.Forms.TextBox();
            this.lblTenNV = new System.Windows.Forms.Label();
            this.txtTenNV = new System.Windows.Forms.TextBox();
            this.lblNgaySinh = new System.Windows.Forms.Label();
            this.dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.lblGioiTinh = new System.Windows.Forms.Label();
            this.cboGioiTinh = new System.Windows.Forms.ComboBox();
            this.lblDienThoai = new System.Windows.Forms.Label();
            this.txtDienThoai = new System.Windows.Forms.TextBox();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.lblTrinhDo = new System.Windows.Forms.Label();
            this.cboTrinhDo = new System.Windows.Forms.ComboBox();
            this.lblChuyenMon = new System.Windows.Forms.Label();
            this.cboChuyenMon = new System.Windows.Forms.ComboBox();
            this.lblVaiTro = new System.Windows.Forms.Label();
            this.cboVaiTro = new System.Windows.Forms.ComboBox();
            this.lblMatKhau = new System.Windows.Forms.Label();
            this.txtMatKhau = new System.Windows.Forms.TextBox();
            this.btnMatKhauMacDinh = new System.Windows.Forms.Button();
            this.lblGoiYMK = new System.Windows.Forms.Label();
            this.chkNghiViec = new System.Windows.Forms.CheckBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.pnlTren.SuspendLayout();
            this.pnlGiua.SuspendLayout();
            this.pnlLuoi.SuspendLayout();
            this.pnlPhai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTren
            // 
            this.pnlTren.Controls.Add(this.lblTim);
            this.pnlTren.Controls.Add(this.txtTimKiem);
            this.pnlTren.Controls.Add(this.btnTim);
            this.pnlTren.Controls.Add(this.lblLoc);
            this.pnlTren.Controls.Add(this.cboLoc);
            this.pnlTren.Controls.Add(this.lblYeuCau);
            this.pnlTren.BackColor = System.Drawing.Color.White;
            this.pnlTren.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTren.Location = new System.Drawing.Point(0, 0);
            this.pnlTren.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.pnlTren.Size = new System.Drawing.Size(1000, 56);
            this.pnlTren.Name = "pnlTren";
            this.pnlTren.TabIndex = 0;
            // 
            // lblTim
            // 
            this.lblTim.AutoSize = true;
            this.lblTim.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTim.Location = new System.Drawing.Point(15, 18);
            this.lblTim.Text = "Tìm (mã, tên, SĐT, vai trò):";
            this.lblTim.Name = "lblTim";
            this.lblTim.TabIndex = 1;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimKiem.Location = new System.Drawing.Point(195, 15);
            this.txtTimKiem.Size = new System.Drawing.Size(220, 25);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.TabIndex = 2;
            this.txtTimKiem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimKiem_KeyDown);
            // 
            // btnTim
            // 
            this.btnTim.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnTim.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTim.FlatAppearance.BorderSize = 0;
            this.btnTim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTim.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTim.ForeColor = System.Drawing.Color.White;
            this.btnTim.Location = new System.Drawing.Point(425, 13);
            this.btnTim.Size = new System.Drawing.Size(70, 30);
            this.btnTim.Text = "Tìm";
            this.btnTim.UseVisualStyleBackColor = false;
            this.btnTim.Name = "btnTim";
            this.btnTim.TabIndex = 3;
            this.btnTim.Click += new System.EventHandler(this.btnTim_Click);
            // 
            // lblLoc
            // 
            this.lblLoc.AutoSize = true;
            this.lblLoc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoc.Location = new System.Drawing.Point(512, 18);
            this.lblLoc.Text = "Hiển thị:";
            this.lblLoc.Name = "lblLoc";
            this.lblLoc.TabIndex = 4;
            // 
            // cboLoc
            // 
            this.cboLoc.Items.AddRange(new object[] {
            "Tất cả",
            "Đang làm việc",
            "Đã nghỉ việc",
            "Đang yêu cầu cấp lại MK"});
            this.cboLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLoc.Location = new System.Drawing.Point(572, 15);
            this.cboLoc.Size = new System.Drawing.Size(200, 25);
            this.cboLoc.Name = "cboLoc";
            this.cboLoc.TabIndex = 5;
            this.cboLoc.SelectedIndexChanged += new System.EventHandler(this.cboLoc_SelectedIndexChanged);
            // 
            // lblYeuCau
            // 
            this.lblYeuCau.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(156)))));
            this.lblYeuCau.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblYeuCau.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblYeuCau.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYeuCau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(85)))), ((int)(((byte)(0)))));
            this.lblYeuCau.Location = new System.Drawing.Point(655, 0);
            this.lblYeuCau.Size = new System.Drawing.Size(330, 56);
            this.lblYeuCau.Text = "🔑 0 NV yêu cầu cấp lại mật khẩu (dòng vàng) – bấm để lọc";
            this.lblYeuCau.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblYeuCau.Visible = false;
            this.lblYeuCau.Name = "lblYeuCau";
            this.lblYeuCau.TabIndex = 6;
            this.lblYeuCau.Click += new System.EventHandler(this.lblYeuCau_Click);
            // 
            // pnlGiua
            // 
            this.pnlGiua.Controls.Add(this.pnlLuoi);
            this.pnlGiua.Controls.Add(this.pnlPhai);
            this.pnlGiua.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGiua.Location = new System.Drawing.Point(0, 56);
            this.pnlGiua.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlGiua.Size = new System.Drawing.Size(1000, 584);
            this.pnlGiua.Name = "pnlGiua";
            this.pnlGiua.TabIndex = 7;
            // 
            // pnlLuoi
            // 
            this.pnlLuoi.Controls.Add(this.dgv);
            this.pnlLuoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLuoi.Location = new System.Drawing.Point(0, 10);
            this.pnlLuoi.Padding = new System.Windows.Forms.Padding(0, 10, 10, 0);
            this.pnlLuoi.Size = new System.Drawing.Size(610, 574);
            this.pnlLuoi.Name = "pnlLuoi";
            this.pnlLuoi.TabIndex = 8;
            // 
            // pnlPhai
            // 
            this.pnlPhai.Controls.Add(this.lblTieuDeKhung);
            this.pnlPhai.Controls.Add(this.lblCheDo);
            this.pnlPhai.Controls.Add(this.lblCanhBaoNV);
            this.pnlPhai.Controls.Add(this.lblMaNV);
            this.pnlPhai.Controls.Add(this.txtMaNV);
            this.pnlPhai.Controls.Add(this.lblTenNV);
            this.pnlPhai.Controls.Add(this.txtTenNV);
            this.pnlPhai.Controls.Add(this.lblNgaySinh);
            this.pnlPhai.Controls.Add(this.dtpNgaySinh);
            this.pnlPhai.Controls.Add(this.lblGioiTinh);
            this.pnlPhai.Controls.Add(this.cboGioiTinh);
            this.pnlPhai.Controls.Add(this.lblDienThoai);
            this.pnlPhai.Controls.Add(this.txtDienThoai);
            this.pnlPhai.Controls.Add(this.lblDiaChi);
            this.pnlPhai.Controls.Add(this.txtDiaChi);
            this.pnlPhai.Controls.Add(this.lblTrinhDo);
            this.pnlPhai.Controls.Add(this.cboTrinhDo);
            this.pnlPhai.Controls.Add(this.lblChuyenMon);
            this.pnlPhai.Controls.Add(this.cboChuyenMon);
            this.pnlPhai.Controls.Add(this.lblVaiTro);
            this.pnlPhai.Controls.Add(this.cboVaiTro);
            this.pnlPhai.Controls.Add(this.lblMatKhau);
            this.pnlPhai.Controls.Add(this.txtMatKhau);
            this.pnlPhai.Controls.Add(this.btnMatKhauMacDinh);
            this.pnlPhai.Controls.Add(this.lblGoiYMK);
            this.pnlPhai.Controls.Add(this.chkNghiViec);
            this.pnlPhai.Controls.Add(this.btnThem);
            this.pnlPhai.Controls.Add(this.btnLuu);
            this.pnlPhai.Controls.Add(this.btnXoa);
            this.pnlPhai.Controls.Add(this.btnLamMoi);
            this.pnlPhai.AutoScroll = true;
            this.pnlPhai.BackColor = System.Drawing.Color.White;
            this.pnlPhai.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPhai.Location = new System.Drawing.Point(610, 10);
            this.pnlPhai.Margin = new System.Windows.Forms.Padding(10);
            this.pnlPhai.Size = new System.Drawing.Size(390, 574);
            this.pnlPhai.Name = "pnlPhai";
            this.pnlPhai.TabIndex = 9;
            // 
            // dgv
            // 
            dgvStyleXen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            dgvStyleTieuDe.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvStyleTieuDe.BackColor = System.Drawing.SystemColors.Control;
            dgvStyleTieuDe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvStyleTieuDe.ForeColor = System.Drawing.SystemColors.WindowText;
            dgvStyleTieuDe.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dgvStyleTieuDe.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dgvStyleTieuDe.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvStyleO.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvStyleO.BackColor = System.Drawing.SystemColors.Window;
            dgvStyleO.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvStyleO.ForeColor = System.Drawing.SystemColors.ControlText;
            dgvStyleO.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dgvStyleO.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dgvStyleO.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.AlternatingRowsDefaultCellStyle = dgvStyleXen;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv.ColumnHeadersDefaultCellStyle = dgvStyleTieuDe;
            this.dgv.DefaultCellStyle = dgvStyleO;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 10);
            this.dgv.MultiSelect = false;
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(600, 564);
            this.dgv.Name = "dgv";
            this.dgv.TabIndex = 10;
            this.dgv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_CellFormatting);
            this.dgv.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_DataBindingComplete);
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // lblTieuDeKhung
            // 
            this.lblTieuDeKhung.AutoSize = true;
            this.lblTieuDeKhung.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDeKhung.Location = new System.Drawing.Point(18, 15);
            this.lblTieuDeKhung.Text = "Thông tin nhân viên";
            this.lblTieuDeKhung.Name = "lblTieuDeKhung";
            this.lblTieuDeKhung.TabIndex = 11;
            // 
            // lblCheDo
            // 
            this.lblCheDo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheDo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblCheDo.Location = new System.Drawing.Point(18, 41);
            this.lblCheDo.Size = new System.Drawing.Size(345, 20);
            this.lblCheDo.Name = "lblCheDo";
            this.lblCheDo.TabIndex = 12;
            // 
            // lblCanhBaoNV
            // 
            this.lblCanhBaoNV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(156)))));
            this.lblCanhBaoNV.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCanhBaoNV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(85)))), ((int)(((byte)(0)))));
            this.lblCanhBaoNV.Location = new System.Drawing.Point(18, 67);
            this.lblCanhBaoNV.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.lblCanhBaoNV.Size = new System.Drawing.Size(345, 50);
            this.lblCanhBaoNV.Visible = false;
            this.lblCanhBaoNV.Name = "lblCanhBaoNV";
            this.lblCanhBaoNV.TabIndex = 13;
            // 
            // lblMaNV
            // 
            this.lblMaNV.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaNV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblMaNV.Location = new System.Drawing.Point(18, 128);
            this.lblMaNV.Size = new System.Drawing.Size(105, 23);
            this.lblMaNV.Text = "Mã NV:";
            this.lblMaNV.Name = "lblMaNV";
            this.lblMaNV.TabIndex = 14;
            // 
            // txtMaNV
            // 
            this.txtMaNV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaNV.Location = new System.Drawing.Point(128, 125);
            this.txtMaNV.MaxLength = 10;
            this.txtMaNV.Size = new System.Drawing.Size(235, 25);
            this.txtMaNV.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMaNV.Name = "txtMaNV";
            this.txtMaNV.TabIndex = 15;
            this.txtMaNV.TextChanged += new System.EventHandler(this.txtMaNV_TextChanged);
            // 
            // lblTenNV
            // 
            this.lblTenNV.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenNV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTenNV.Location = new System.Drawing.Point(18, 164);
            this.lblTenNV.Size = new System.Drawing.Size(105, 23);
            this.lblTenNV.Text = "Họ tên:";
            this.lblTenNV.Name = "lblTenNV";
            this.lblTenNV.TabIndex = 16;
            // 
            // txtTenNV
            // 
            this.txtTenNV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTenNV.Location = new System.Drawing.Point(128, 161);
            this.txtTenNV.MaxLength = 50;
            this.txtTenNV.Size = new System.Drawing.Size(235, 25);
            this.txtTenNV.Name = "txtTenNV";
            this.txtTenNV.TabIndex = 17;
            // 
            // lblNgaySinh
            // 
            this.lblNgaySinh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgaySinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblNgaySinh.Location = new System.Drawing.Point(18, 200);
            this.lblNgaySinh.Size = new System.Drawing.Size(105, 23);
            this.lblNgaySinh.Text = "Ngày sinh:";
            this.lblNgaySinh.Name = "lblNgaySinh";
            this.lblNgaySinh.TabIndex = 18;
            // 
            // dtpNgaySinh
            // 
            this.dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            this.dtpNgaySinh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgaySinh.Location = new System.Drawing.Point(128, 197);
            this.dtpNgaySinh.ShowCheckBox = true;
            this.dtpNgaySinh.Size = new System.Drawing.Size(160, 25);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.TabIndex = 19;
            // 
            // lblGioiTinh
            // 
            this.lblGioiTinh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGioiTinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblGioiTinh.Location = new System.Drawing.Point(18, 236);
            this.lblGioiTinh.Size = new System.Drawing.Size(105, 23);
            this.lblGioiTinh.Text = "Giới tính:";
            this.lblGioiTinh.Name = "lblGioiTinh";
            this.lblGioiTinh.TabIndex = 20;
            // 
            // cboGioiTinh
            // 
            this.cboGioiTinh.Items.AddRange(new object[] {
            "Nam",
            "Nữ",
            "Khác"});
            this.cboGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGioiTinh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboGioiTinh.Location = new System.Drawing.Point(128, 233);
            this.cboGioiTinh.Size = new System.Drawing.Size(120, 25);
            this.cboGioiTinh.Name = "cboGioiTinh";
            this.cboGioiTinh.TabIndex = 21;
            // 
            // lblDienThoai
            // 
            this.lblDienThoai.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDienThoai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblDienThoai.Location = new System.Drawing.Point(18, 272);
            this.lblDienThoai.Size = new System.Drawing.Size(105, 23);
            this.lblDienThoai.Text = "Điện thoại:";
            this.lblDienThoai.Name = "lblDienThoai";
            this.lblDienThoai.TabIndex = 22;
            // 
            // txtDienThoai
            // 
            this.txtDienThoai.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDienThoai.Location = new System.Drawing.Point(128, 269);
            this.txtDienThoai.MaxLength = 20;
            this.txtDienThoai.Size = new System.Drawing.Size(235, 25);
            this.txtDienThoai.Name = "txtDienThoai";
            this.txtDienThoai.TabIndex = 23;
            // 
            // lblDiaChi
            // 
            this.lblDiaChi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiaChi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblDiaChi.Location = new System.Drawing.Point(18, 308);
            this.lblDiaChi.Size = new System.Drawing.Size(105, 23);
            this.lblDiaChi.Text = "Địa chỉ:";
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.TabIndex = 24;
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiaChi.Location = new System.Drawing.Point(128, 305);
            this.txtDiaChi.MaxLength = 255;
            this.txtDiaChi.Size = new System.Drawing.Size(235, 25);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.TabIndex = 25;
            // 
            // lblTrinhDo
            // 
            this.lblTrinhDo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrinhDo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTrinhDo.Location = new System.Drawing.Point(18, 344);
            this.lblTrinhDo.Size = new System.Drawing.Size(105, 23);
            this.lblTrinhDo.Text = "Trình độ:";
            this.lblTrinhDo.Name = "lblTrinhDo";
            this.lblTrinhDo.TabIndex = 26;
            // 
            // cboTrinhDo
            // 
            this.cboTrinhDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrinhDo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTrinhDo.Location = new System.Drawing.Point(128, 341);
            this.cboTrinhDo.Size = new System.Drawing.Size(235, 25);
            this.cboTrinhDo.Name = "cboTrinhDo";
            this.cboTrinhDo.TabIndex = 27;
            // 
            // lblChuyenMon
            // 
            this.lblChuyenMon.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChuyenMon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblChuyenMon.Location = new System.Drawing.Point(18, 380);
            this.lblChuyenMon.Size = new System.Drawing.Size(105, 23);
            this.lblChuyenMon.Text = "Chuyên môn:";
            this.lblChuyenMon.Name = "lblChuyenMon";
            this.lblChuyenMon.TabIndex = 28;
            // 
            // cboChuyenMon
            // 
            this.cboChuyenMon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChuyenMon.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboChuyenMon.Location = new System.Drawing.Point(128, 377);
            this.cboChuyenMon.Size = new System.Drawing.Size(235, 25);
            this.cboChuyenMon.Name = "cboChuyenMon";
            this.cboChuyenMon.TabIndex = 29;
            // 
            // lblVaiTro
            // 
            this.lblVaiTro.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVaiTro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblVaiTro.Location = new System.Drawing.Point(18, 416);
            this.lblVaiTro.Size = new System.Drawing.Size(105, 23);
            this.lblVaiTro.Text = "Vai trò (*):";
            this.lblVaiTro.Name = "lblVaiTro";
            this.lblVaiTro.TabIndex = 30;
            // 
            // cboVaiTro
            // 
            this.cboVaiTro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVaiTro.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboVaiTro.Location = new System.Drawing.Point(128, 413);
            this.cboVaiTro.Size = new System.Drawing.Size(235, 25);
            this.cboVaiTro.Name = "cboVaiTro";
            this.cboVaiTro.TabIndex = 31;
            this.cboVaiTro.SelectedIndexChanged += new System.EventHandler(this.cboVaiTro_SelectedIndexChanged);
            // 
            // lblMatKhau
            // 
            this.lblMatKhau.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatKhau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblMatKhau.Location = new System.Drawing.Point(18, 452);
            this.lblMatKhau.Size = new System.Drawing.Size(105, 23);
            this.lblMatKhau.Text = "Mật khẩu mới:";
            this.lblMatKhau.Name = "lblMatKhau";
            this.lblMatKhau.TabIndex = 32;
            // 
            // txtMatKhau
            // 
            this.txtMatKhau.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMatKhau.Location = new System.Drawing.Point(128, 449);
            this.txtMatKhau.MaxLength = 50;
            this.txtMatKhau.Size = new System.Drawing.Size(150, 25);
            this.txtMatKhau.Name = "txtMatKhau";
            this.txtMatKhau.TabIndex = 33;
            // 
            // btnMatKhauMacDinh
            // 
            this.btnMatKhauMacDinh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnMatKhauMacDinh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMatKhauMacDinh.FlatAppearance.BorderSize = 0;
            this.btnMatKhauMacDinh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMatKhauMacDinh.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMatKhauMacDinh.ForeColor = System.Drawing.Color.White;
            this.btnMatKhauMacDinh.Location = new System.Drawing.Point(283, 448);
            this.btnMatKhauMacDinh.Size = new System.Drawing.Size(80, 27);
            this.btnMatKhauMacDinh.Text = "Mặc định";
            this.btnMatKhauMacDinh.UseVisualStyleBackColor = false;
            this.btnMatKhauMacDinh.Name = "btnMatKhauMacDinh";
            this.btnMatKhauMacDinh.TabIndex = 34;
            this.btnMatKhauMacDinh.Click += new System.EventHandler(this.btnMatKhauMacDinh_Click);
            // 
            // lblGoiYMK
            // 
            this.lblGoiYMK.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGoiYMK.ForeColor = System.Drawing.Color.Gray;
            this.lblGoiYMK.Location = new System.Drawing.Point(128, 479);
            this.lblGoiYMK.Size = new System.Drawing.Size(240, 18);
            this.lblGoiYMK.Text = "Để trống = giữ mật khẩu cũ. Tối thiểu 6 ký tự";
            this.lblGoiYMK.Name = "lblGoiYMK";
            this.lblGoiYMK.TabIndex = 35;
            // 
            // chkNghiViec
            // 
            this.chkNghiViec.AutoSize = true;
            this.chkNghiViec.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNghiViec.Location = new System.Drawing.Point(128, 505);
            this.chkNghiViec.Size = new System.Drawing.Size(214, 21);
            this.chkNghiViec.Text = "Đã nghỉ việc (khóa đăng nhập)";
            this.chkNghiViec.UseVisualStyleBackColor = true;
            this.chkNghiViec.Name = "chkNghiViec";
            this.chkNghiViec.TabIndex = 36;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(18, 545);
            this.btnThem.Size = new System.Drawing.Size(165, 38);
            this.btnThem.Text = "➕ Thêm mới";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Name = "btnThem";
            this.btnThem.TabIndex = 37;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(198, 545);
            this.btnLuu.Size = new System.Drawing.Size(165, 38);
            this.btnLuu.Text = "💾 Lưu";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.TabIndex = 38;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnXoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoa.FlatAppearance.BorderSize = 0;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(18, 591);
            this.btnXoa.Size = new System.Drawing.Size(165, 38);
            this.btnXoa.Text = "🗑 Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.TabIndex = 39;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.FlatAppearance.BorderSize = 0;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(198, 591);
            this.btnLamMoi.Size = new System.Drawing.Size(165, 38);
            this.btnLamMoi.Text = "↻ Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.TabIndex = 40;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // frmQuanLyNhanVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Text = "Quản lý Nhân viên";
            this.Controls.Add(this.pnlGiua);
            this.Controls.Add(this.pnlTren);
            this.Name = "frmQuanLyNhanVien";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.pnlPhai.ResumeLayout(false);
            this.pnlPhai.PerformLayout();
            this.pnlLuoi.ResumeLayout(false);
            this.pnlLuoi.PerformLayout();
            this.pnlGiua.ResumeLayout(false);
            this.pnlGiua.PerformLayout();
            this.pnlTren.ResumeLayout(false);
            this.pnlTren.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTren;
        private System.Windows.Forms.Label lblTim;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.Label lblLoc;
        private System.Windows.Forms.ComboBox cboLoc;
        private System.Windows.Forms.Label lblYeuCau;
        private System.Windows.Forms.Panel pnlGiua;
        private System.Windows.Forms.Panel pnlLuoi;
        private System.Windows.Forms.Panel pnlPhai;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lblTieuDeKhung;
        private System.Windows.Forms.Label lblCheDo;
        private System.Windows.Forms.Label lblCanhBaoNV;
        private System.Windows.Forms.Label lblMaNV;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.Label lblTenNV;
        private System.Windows.Forms.TextBox txtTenNV;
        private System.Windows.Forms.Label lblNgaySinh;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh;
        private System.Windows.Forms.Label lblGioiTinh;
        private System.Windows.Forms.ComboBox cboGioiTinh;
        private System.Windows.Forms.Label lblDienThoai;
        private System.Windows.Forms.TextBox txtDienThoai;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblTrinhDo;
        private System.Windows.Forms.ComboBox cboTrinhDo;
        private System.Windows.Forms.Label lblChuyenMon;
        private System.Windows.Forms.ComboBox cboChuyenMon;
        private System.Windows.Forms.Label lblVaiTro;
        private System.Windows.Forms.ComboBox cboVaiTro;
        private System.Windows.Forms.Label lblMatKhau;
        private System.Windows.Forms.TextBox txtMatKhau;
        private System.Windows.Forms.Button btnMatKhauMacDinh;
        private System.Windows.Forms.Label lblGoiYMK;
        private System.Windows.Forms.CheckBox chkNghiViec;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
    }
}
