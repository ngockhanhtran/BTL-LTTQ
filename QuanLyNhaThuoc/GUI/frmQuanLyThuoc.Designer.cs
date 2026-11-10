namespace QuanLyNhaThuoc.GUI
{
    partial class frmQuanLyThuoc
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
            System.Windows.Forms.DataGridViewCellStyle sQTXen = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle sQTTieuDe = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle sQTO = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTren = new System.Windows.Forms.Panel();
            this.lblTim = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnTim = new System.Windows.Forms.Button();
            this.lblChuThich = new System.Windows.Forms.Label();
            this.pnlGiua = new System.Windows.Forms.Panel();
            this.pnlLuoi = new System.Windows.Forms.Panel();
            this.pnlPhai = new System.Windows.Forms.Panel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.lblTieuDeKhung = new System.Windows.Forms.Label();
            this.lblCheDo = new System.Windows.Forms.Label();
            this.lblMaThuoc = new System.Windows.Forms.Label();
            this.txtMaThuoc = new System.Windows.Forms.TextBox();
            this.lblTenThuoc = new System.Windows.Forms.Label();
            this.txtTenThuoc = new System.Windows.Forms.TextBox();
            this.lblMaVach = new System.Windows.Forms.Label();
            this.txtMaVach = new System.Windows.Forms.TextBox();
            this.lblKeDon = new System.Windows.Forms.Label();
            this.chkKeDon = new System.Windows.Forms.CheckBox();
            this.lblDVT = new System.Windows.Forms.Label();
            this.cboDVT = new System.Windows.Forms.ComboBox();
            this.lblDangDC = new System.Windows.Forms.Label();
            this.cboDangDC = new System.Windows.Forms.ComboBox();
            this.lblThanhPhan = new System.Windows.Forms.Label();
            this.txtThanhPhan = new System.Windows.Forms.TextBox();
            this.lblGiaNhap = new System.Windows.Forms.Label();
            this.nudGiaNhap = new System.Windows.Forms.NumericUpDown();
            this.lblGiaBan = new System.Windows.Forms.Label();
            this.txtGiaBan = new System.Windows.Forms.TextBox();
            this.lblSLHienCo = new System.Windows.Forms.Label();
            this.txtSLHienCo = new System.Windows.Forms.TextBox();
            this.btnXemLo = new System.Windows.Forms.Button();
            this.lblNgaySX = new System.Windows.Forms.Label();
            this.dtpNgaySX = new System.Windows.Forms.DateTimePicker();
            this.lblHanSD = new System.Windows.Forms.Label();
            this.dtpHanSD = new System.Windows.Forms.DateTimePicker();
            this.lblGhiChuLo = new System.Windows.Forms.Label();
            this.lblNuocSX = new System.Windows.Forms.Label();
            this.cboNuocSX = new System.Windows.Forms.ComboBox();
            this.lblChongChiDinh = new System.Windows.Forms.Label();
            this.txtChongChiDinh = new System.Windows.Forms.TextBox();
            this.lblCongDung = new System.Windows.Forms.Label();
            this.txtCongDung = new System.Windows.Forms.TextBox();
            this.btnChonCD = new System.Windows.Forms.Button();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.pnlTren.SuspendLayout();
            this.pnlGiua.SuspendLayout();
            this.pnlLuoi.SuspendLayout();
            this.pnlPhai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGiaNhap)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTren
            // 
            sQTXen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            sQTTieuDe.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            sQTTieuDe.BackColor = System.Drawing.SystemColors.Control;
            sQTTieuDe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sQTTieuDe.ForeColor = System.Drawing.SystemColors.WindowText;
            sQTTieuDe.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            sQTTieuDe.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            sQTTieuDe.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            sQTO.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            sQTO.BackColor = System.Drawing.SystemColors.Window;
            sQTO.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sQTO.ForeColor = System.Drawing.SystemColors.ControlText;
            sQTO.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            sQTO.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            sQTO.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.pnlTren.Controls.Add(this.lblTim);
            this.pnlTren.Controls.Add(this.txtTimKiem);
            this.pnlTren.Controls.Add(this.btnTim);
            this.pnlTren.Controls.Add(this.lblChuThich);
            this.pnlTren.BackColor = System.Drawing.Color.White;
            this.pnlTren.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTren.Location = new System.Drawing.Point(0, 0);
            this.pnlTren.Size = new System.Drawing.Size(1000, 56);
            this.pnlTren.Name = "pnlTren";
            this.pnlTren.TabIndex = 0;
            // 
            // lblTim
            // 
            this.lblTim.AutoSize = true;
            this.lblTim.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTim.Location = new System.Drawing.Point(15, 18);
            this.lblTim.Text = "Tìm (mã, tên, mã vạch, thành phần):";
            this.lblTim.Name = "lblTim";
            this.lblTim.TabIndex = 1;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimKiem.Location = new System.Drawing.Point(245, 15);
            this.txtTimKiem.Size = new System.Drawing.Size(240, 25);
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
            this.btnTim.Location = new System.Drawing.Point(495, 13);
            this.btnTim.Size = new System.Drawing.Size(70, 30);
            this.btnTim.Text = "Tìm";
            this.btnTim.UseVisualStyleBackColor = false;
            this.btnTim.Name = "btnTim";
            this.btnTim.TabIndex = 3;
            this.btnTim.Click += new System.EventHandler(this.btnTim_Click);
            // 
            // lblChuThich
            // 
            this.lblChuThich.AutoSize = true;
            this.lblChuThich.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChuThich.ForeColor = System.Drawing.Color.DimGray;
            this.lblChuThich.Location = new System.Drawing.Point(585, 19);
            this.lblChuThich.Text = "■ Đỏ: đã hết hạn    ■ Vàng: sắp hết hàng (< 30)    ℞: thuốc kê đơn";
            this.lblChuThich.Name = "lblChuThich";
            this.lblChuThich.TabIndex = 4;
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
            this.pnlGiua.TabIndex = 5;
            // 
            // pnlLuoi
            // 
            this.pnlLuoi.Controls.Add(this.dgv);
            this.pnlLuoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLuoi.Location = new System.Drawing.Point(0, 10);
            this.pnlLuoi.Padding = new System.Windows.Forms.Padding(0, 10, 10, 0);
            this.pnlLuoi.Size = new System.Drawing.Size(600, 574);
            this.pnlLuoi.Name = "pnlLuoi";
            this.pnlLuoi.TabIndex = 6;
            // 
            // pnlPhai
            // 
            this.pnlPhai.Controls.Add(this.lblTieuDeKhung);
            this.pnlPhai.Controls.Add(this.lblCheDo);
            this.pnlPhai.Controls.Add(this.lblMaThuoc);
            this.pnlPhai.Controls.Add(this.txtMaThuoc);
            this.pnlPhai.Controls.Add(this.lblTenThuoc);
            this.pnlPhai.Controls.Add(this.txtTenThuoc);
            this.pnlPhai.Controls.Add(this.lblMaVach);
            this.pnlPhai.Controls.Add(this.txtMaVach);
            this.pnlPhai.Controls.Add(this.lblKeDon);
            this.pnlPhai.Controls.Add(this.chkKeDon);
            this.pnlPhai.Controls.Add(this.lblDVT);
            this.pnlPhai.Controls.Add(this.cboDVT);
            this.pnlPhai.Controls.Add(this.lblDangDC);
            this.pnlPhai.Controls.Add(this.cboDangDC);
            this.pnlPhai.Controls.Add(this.lblThanhPhan);
            this.pnlPhai.Controls.Add(this.txtThanhPhan);
            this.pnlPhai.Controls.Add(this.lblGiaNhap);
            this.pnlPhai.Controls.Add(this.nudGiaNhap);
            this.pnlPhai.Controls.Add(this.lblGiaBan);
            this.pnlPhai.Controls.Add(this.txtGiaBan);
            this.pnlPhai.Controls.Add(this.lblSLHienCo);
            this.pnlPhai.Controls.Add(this.txtSLHienCo);
            this.pnlPhai.Controls.Add(this.btnXemLo);
            this.pnlPhai.Controls.Add(this.lblNgaySX);
            this.pnlPhai.Controls.Add(this.dtpNgaySX);
            this.pnlPhai.Controls.Add(this.lblHanSD);
            this.pnlPhai.Controls.Add(this.dtpHanSD);
            this.pnlPhai.Controls.Add(this.lblGhiChuLo);
            this.pnlPhai.Controls.Add(this.lblNuocSX);
            this.pnlPhai.Controls.Add(this.cboNuocSX);
            this.pnlPhai.Controls.Add(this.lblChongChiDinh);
            this.pnlPhai.Controls.Add(this.txtChongChiDinh);
            this.pnlPhai.Controls.Add(this.lblCongDung);
            this.pnlPhai.Controls.Add(this.txtCongDung);
            this.pnlPhai.Controls.Add(this.btnChonCD);
            this.pnlPhai.Controls.Add(this.btnThem);
            this.pnlPhai.Controls.Add(this.btnLuu);
            this.pnlPhai.Controls.Add(this.btnXoa);
            this.pnlPhai.Controls.Add(this.btnLamMoi);
            this.pnlPhai.AutoScroll = true;
            this.pnlPhai.BackColor = System.Drawing.Color.White;
            this.pnlPhai.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPhai.Location = new System.Drawing.Point(600, 10);
            this.pnlPhai.Size = new System.Drawing.Size(400, 574);
            this.pnlPhai.Name = "pnlPhai";
            this.pnlPhai.TabIndex = 7;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.AlternatingRowsDefaultCellStyle = sQTXen;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv.ColumnHeadersDefaultCellStyle = sQTTieuDe;
            this.dgv.DefaultCellStyle = sQTO;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 10);
            this.dgv.MultiSelect = false;
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(590, 564);
            this.dgv.Name = "dgv";
            this.dgv.TabIndex = 8;
            this.dgv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_CellFormatting);
            this.dgv.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_DataBindingComplete);
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // lblTieuDeKhung
            // 
            this.lblTieuDeKhung.AutoSize = true;
            this.lblTieuDeKhung.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDeKhung.Location = new System.Drawing.Point(15, 12);
            this.lblTieuDeKhung.Text = "Thông tin thuốc";
            this.lblTieuDeKhung.Name = "lblTieuDeKhung";
            this.lblTieuDeKhung.TabIndex = 9;
            // 
            // lblCheDo
            // 
            this.lblCheDo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheDo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblCheDo.Location = new System.Drawing.Point(15, 38);
            this.lblCheDo.Size = new System.Drawing.Size(355, 20);
            this.lblCheDo.Text = "";
            this.lblCheDo.Name = "lblCheDo";
            this.lblCheDo.TabIndex = 10;
            // 
            // lblMaThuoc
            // 
            this.lblMaThuoc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaThuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblMaThuoc.Location = new System.Drawing.Point(15, 69);
            this.lblMaThuoc.Size = new System.Drawing.Size(108, 23);
            this.lblMaThuoc.Text = "Mã thuốc (*):";
            this.lblMaThuoc.Name = "lblMaThuoc";
            this.lblMaThuoc.TabIndex = 11;
            // 
            // txtMaThuoc
            // 
            this.txtMaThuoc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaThuoc.Location = new System.Drawing.Point(125, 66);
            this.txtMaThuoc.MaxLength = 10;
            this.txtMaThuoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.txtMaThuoc.ReadOnly = true;
            this.txtMaThuoc.TabStop = false;
            this.txtMaThuoc.Size = new System.Drawing.Size(245, 25);
            this.txtMaThuoc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMaThuoc.Name = "txtMaThuoc";
            this.txtMaThuoc.TabIndex = 12;
            // 
            // lblTenThuoc
            // 
            this.lblTenThuoc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenThuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTenThuoc.Location = new System.Drawing.Point(15, 103);
            this.lblTenThuoc.Size = new System.Drawing.Size(108, 23);
            this.lblTenThuoc.Text = "Tên thuốc (*):";
            this.lblTenThuoc.Name = "lblTenThuoc";
            this.lblTenThuoc.TabIndex = 13;
            // 
            // txtTenThuoc
            // 
            this.txtTenThuoc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTenThuoc.Location = new System.Drawing.Point(125, 100);
            this.txtTenThuoc.MaxLength = 100;
            this.txtTenThuoc.Size = new System.Drawing.Size(245, 25);
            this.txtTenThuoc.Name = "txtTenThuoc";
            this.txtTenThuoc.TabIndex = 14;
            // 
            // lblMaVach
            // 
            this.lblMaVach.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaVach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblMaVach.Location = new System.Drawing.Point(15, 137);
            this.lblMaVach.Size = new System.Drawing.Size(108, 23);
            this.lblMaVach.Text = "Mã vạch:";
            this.lblMaVach.Name = "lblMaVach";
            this.lblMaVach.TabIndex = 15;
            // 
            // txtMaVach
            // 
            this.txtMaVach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaVach.Location = new System.Drawing.Point(125, 134);
            this.txtMaVach.MaxLength = 50;
            this.txtMaVach.Size = new System.Drawing.Size(245, 25);
            this.txtMaVach.Name = "txtMaVach";
            this.txtMaVach.TabIndex = 16;
            // 
            // lblKeDon
            // 
            this.lblKeDon.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeDon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblKeDon.Location = new System.Drawing.Point(15, 171);
            this.lblKeDon.Size = new System.Drawing.Size(108, 23);
            this.lblKeDon.Text = "Loại thuốc:";
            this.lblKeDon.Name = "lblKeDon";
            this.lblKeDon.TabIndex = 17;
            // 
            // chkKeDon
            // 
            this.chkKeDon.AutoSize = true;
            this.chkKeDon.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkKeDon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(35)))), ((int)(((byte)(51)))));
            this.chkKeDon.Location = new System.Drawing.Point(125, 170);
            this.chkKeDon.Text = "℞ Thuốc kê đơn (bán phải có mã đơn)";
            this.chkKeDon.UseVisualStyleBackColor = true;
            this.chkKeDon.Name = "chkKeDon";
            this.chkKeDon.TabIndex = 18;
            // 
            // lblDVT
            // 
            this.lblDVT.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDVT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblDVT.Location = new System.Drawing.Point(15, 205);
            this.lblDVT.Size = new System.Drawing.Size(108, 23);
            this.lblDVT.Text = "Đơn vị tính:";
            this.lblDVT.Name = "lblDVT";
            this.lblDVT.TabIndex = 19;
            // 
            // cboDVT
            // 
            this.cboDVT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDVT.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDVT.Location = new System.Drawing.Point(125, 202);
            this.cboDVT.Size = new System.Drawing.Size(245, 25);
            this.cboDVT.Name = "cboDVT";
            this.cboDVT.TabIndex = 20;
            // 
            // lblDangDC
            // 
            this.lblDangDC.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDangDC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblDangDC.Location = new System.Drawing.Point(15, 239);
            this.lblDangDC.Size = new System.Drawing.Size(108, 23);
            this.lblDangDC.Text = "Dạng điều chế:";
            this.lblDangDC.Name = "lblDangDC";
            this.lblDangDC.TabIndex = 21;
            // 
            // cboDangDC
            // 
            this.cboDangDC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDangDC.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDangDC.Location = new System.Drawing.Point(125, 236);
            this.cboDangDC.Size = new System.Drawing.Size(245, 25);
            this.cboDangDC.Name = "cboDangDC";
            this.cboDangDC.TabIndex = 22;
            // 
            // lblThanhPhan
            // 
            this.lblThanhPhan.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThanhPhan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblThanhPhan.Location = new System.Drawing.Point(15, 273);
            this.lblThanhPhan.Size = new System.Drawing.Size(108, 23);
            this.lblThanhPhan.Text = "Thành phần:";
            this.lblThanhPhan.Name = "lblThanhPhan";
            this.lblThanhPhan.TabIndex = 23;
            // 
            // txtThanhPhan
            // 
            this.txtThanhPhan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThanhPhan.Location = new System.Drawing.Point(125, 270);
            this.txtThanhPhan.MaxLength = 255;
            this.txtThanhPhan.Size = new System.Drawing.Size(245, 44);
            this.txtThanhPhan.Multiline = true;
            this.txtThanhPhan.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtThanhPhan.Name = "txtThanhPhan";
            this.txtThanhPhan.TabIndex = 24;
            // 
            // lblGiaNhap
            // 
            this.lblGiaNhap.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGiaNhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblGiaNhap.Location = new System.Drawing.Point(15, 325);
            this.lblGiaNhap.Size = new System.Drawing.Size(108, 23);
            this.lblGiaNhap.Text = "Giá nhập:";
            this.lblGiaNhap.Name = "lblGiaNhap";
            this.lblGiaNhap.TabIndex = 25;
            // 
            // nudGiaNhap
            // 
            this.nudGiaNhap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudGiaNhap.Location = new System.Drawing.Point(125, 322);
            this.nudGiaNhap.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nudGiaNhap.Size = new System.Drawing.Size(130, 25);
            this.nudGiaNhap.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudGiaNhap.ThousandsSeparator = true;
            this.nudGiaNhap.Name = "nudGiaNhap";
            this.nudGiaNhap.TabIndex = 26;
            this.nudGiaNhap.ValueChanged += new System.EventHandler(this.nudGiaNhap_ValueChanged);
            // 
            // lblGiaBan
            // 
            this.lblGiaBan.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGiaBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblGiaBan.Location = new System.Drawing.Point(15, 359);
            this.lblGiaBan.Size = new System.Drawing.Size(108, 23);
            this.lblGiaBan.Text = "Giá bán (110%):";
            this.lblGiaBan.Name = "lblGiaBan";
            this.lblGiaBan.TabIndex = 27;
            // 
            // txtGiaBan
            // 
            this.txtGiaBan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGiaBan.Location = new System.Drawing.Point(125, 356);
            this.txtGiaBan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.txtGiaBan.ReadOnly = true;
            this.txtGiaBan.TabStop = false;
            this.txtGiaBan.Size = new System.Drawing.Size(130, 25);
            this.txtGiaBan.Name = "txtGiaBan";
            this.txtGiaBan.TabIndex = 28;
            // 
            // lblSLHienCo
            // 
            this.lblSLHienCo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSLHienCo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblSLHienCo.Location = new System.Drawing.Point(15, 393);
            this.lblSLHienCo.Size = new System.Drawing.Size(108, 23);
            this.lblSLHienCo.Text = "SL hiện có:";
            this.lblSLHienCo.Name = "lblSLHienCo";
            this.lblSLHienCo.TabIndex = 29;
            // 
            // txtSLHienCo
            // 
            this.txtSLHienCo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSLHienCo.Location = new System.Drawing.Point(125, 390);
            this.txtSLHienCo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.txtSLHienCo.ReadOnly = true;
            this.txtSLHienCo.TabStop = false;
            this.txtSLHienCo.Size = new System.Drawing.Size(80, 25);
            this.txtSLHienCo.Name = "txtSLHienCo";
            this.txtSLHienCo.TabIndex = 30;
            // 
            // btnXemLo
            // 
            this.btnXemLo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnXemLo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXemLo.FlatAppearance.BorderSize = 0;
            this.btnXemLo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXemLo.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXemLo.ForeColor = System.Drawing.Color.White;
            this.btnXemLo.Location = new System.Drawing.Point(213, 389);
            this.btnXemLo.Size = new System.Drawing.Size(120, 27);
            this.btnXemLo.Text = "🧾 Xem các lô";
            this.btnXemLo.UseVisualStyleBackColor = false;
            this.btnXemLo.Name = "btnXemLo";
            this.btnXemLo.TabIndex = 31;
            this.btnXemLo.Click += new System.EventHandler(this.btnXemLo_Click);
            // 
            // lblNgaySX
            // 
            this.lblNgaySX.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgaySX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblNgaySX.Location = new System.Drawing.Point(15, 427);
            this.lblNgaySX.Size = new System.Drawing.Size(108, 23);
            this.lblNgaySX.Text = "Ngày SX:";
            this.lblNgaySX.Name = "lblNgaySX";
            this.lblNgaySX.TabIndex = 32;
            // 
            // dtpNgaySX
            // 
            this.dtpNgaySX.CustomFormat = "dd/MM/yyyy";
            this.dtpNgaySX.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgaySX.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgaySX.Location = new System.Drawing.Point(125, 424);
            this.dtpNgaySX.Size = new System.Drawing.Size(150, 25);
            this.dtpNgaySX.ShowCheckBox = true;
            this.dtpNgaySX.Name = "dtpNgaySX";
            this.dtpNgaySX.TabIndex = 33;
            // 
            // lblHanSD
            // 
            this.lblHanSD.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHanSD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblHanSD.Location = new System.Drawing.Point(15, 461);
            this.lblHanSD.Size = new System.Drawing.Size(108, 23);
            this.lblHanSD.Text = "Hạn sử dụng:";
            this.lblHanSD.Name = "lblHanSD";
            this.lblHanSD.TabIndex = 34;
            // 
            // dtpHanSD
            // 
            this.dtpHanSD.CustomFormat = "dd/MM/yyyy";
            this.dtpHanSD.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpHanSD.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHanSD.Location = new System.Drawing.Point(125, 458);
            this.dtpHanSD.Size = new System.Drawing.Size(150, 25);
            this.dtpHanSD.ShowCheckBox = true;
            this.dtpHanSD.Name = "dtpHanSD";
            this.dtpHanSD.TabIndex = 35;
            // 
            // lblGhiChuLo
            // 
            this.lblGhiChuLo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGhiChuLo.ForeColor = System.Drawing.Color.Gray;
            this.lblGhiChuLo.Location = new System.Drawing.Point(15, 488);
            this.lblGhiChuLo.Size = new System.Drawing.Size(355, 30);
            this.lblGhiChuLo.Text = "Thuốc đã có lô: NSX / HSD lấy theo lô gần hết hạn nhất (sửa qua Nhập hàng).";
            this.lblGhiChuLo.Name = "lblGhiChuLo";
            this.lblGhiChuLo.TabIndex = 36;
            // 
            // lblNuocSX
            // 
            this.lblNuocSX.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNuocSX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblNuocSX.Location = new System.Drawing.Point(15, 523);
            this.lblNuocSX.Size = new System.Drawing.Size(108, 23);
            this.lblNuocSX.Text = "Nước SX:";
            this.lblNuocSX.Name = "lblNuocSX";
            this.lblNuocSX.TabIndex = 37;
            // 
            // cboNuocSX
            // 
            this.cboNuocSX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNuocSX.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboNuocSX.Location = new System.Drawing.Point(125, 520);
            this.cboNuocSX.Size = new System.Drawing.Size(245, 25);
            this.cboNuocSX.Name = "cboNuocSX";
            this.cboNuocSX.TabIndex = 38;
            // 
            // lblChongChiDinh
            // 
            this.lblChongChiDinh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChongChiDinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblChongChiDinh.Location = new System.Drawing.Point(15, 557);
            this.lblChongChiDinh.Size = new System.Drawing.Size(108, 23);
            this.lblChongChiDinh.Text = "Chống chỉ định:";
            this.lblChongChiDinh.Name = "lblChongChiDinh";
            this.lblChongChiDinh.TabIndex = 39;
            // 
            // txtChongChiDinh
            // 
            this.txtChongChiDinh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChongChiDinh.Location = new System.Drawing.Point(125, 554);
            this.txtChongChiDinh.MaxLength = 255;
            this.txtChongChiDinh.Size = new System.Drawing.Size(245, 44);
            this.txtChongChiDinh.Multiline = true;
            this.txtChongChiDinh.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChongChiDinh.Name = "txtChongChiDinh";
            this.txtChongChiDinh.TabIndex = 40;
            // 
            // lblCongDung
            // 
            this.lblCongDung.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCongDung.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblCongDung.Location = new System.Drawing.Point(15, 609);
            this.lblCongDung.Size = new System.Drawing.Size(108, 23);
            this.lblCongDung.Text = "Công dụng:";
            this.lblCongDung.Name = "lblCongDung";
            this.lblCongDung.TabIndex = 41;
            // 
            // txtCongDung
            // 
            this.txtCongDung.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCongDung.Location = new System.Drawing.Point(125, 606);
            this.txtCongDung.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.txtCongDung.ReadOnly = true;
            this.txtCongDung.TabStop = false;
            this.txtCongDung.Size = new System.Drawing.Size(245, 44);
            this.txtCongDung.Multiline = true;
            this.txtCongDung.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCongDung.Name = "txtCongDung";
            this.txtCongDung.TabIndex = 42;
            // 
            // btnChonCD
            // 
            this.btnChonCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnChonCD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChonCD.FlatAppearance.BorderSize = 0;
            this.btnChonCD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChonCD.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChonCD.ForeColor = System.Drawing.Color.White;
            this.btnChonCD.Location = new System.Drawing.Point(125, 654);
            this.btnChonCD.Size = new System.Drawing.Size(160, 28);
            this.btnChonCD.Text = "✎ Chọn công dụng...";
            this.btnChonCD.UseVisualStyleBackColor = false;
            this.btnChonCD.Name = "btnChonCD";
            this.btnChonCD.TabIndex = 43;
            this.btnChonCD.Click += new System.EventHandler(this.btnChonCD_Click);
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(15, 696);
            this.btnThem.Size = new System.Drawing.Size(170, 38);
            this.btnThem.Text = "➕ Thêm mới";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Name = "btnThem";
            this.btnThem.TabIndex = 44;
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
            this.btnLuu.Location = new System.Drawing.Point(200, 696);
            this.btnLuu.Size = new System.Drawing.Size(170, 38);
            this.btnLuu.Text = "💾 Lưu";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.TabIndex = 45;
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
            this.btnXoa.Location = new System.Drawing.Point(15, 742);
            this.btnXoa.Size = new System.Drawing.Size(170, 38);
            this.btnXoa.Text = "🗑 Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.TabIndex = 46;
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
            this.btnLamMoi.Location = new System.Drawing.Point(200, 742);
            this.btnLamMoi.Size = new System.Drawing.Size(170, 38);
            this.btnLamMoi.Text = "↻ Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.TabIndex = 47;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // frmQuanLyThuoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Text = "Danh mục Thuốc";
            this.Controls.Add(this.pnlGiua);
            this.Controls.Add(this.pnlTren);
            this.Name = "frmQuanLyThuoc";
            ((System.ComponentModel.ISupportInitialize)(this.nudGiaNhap)).EndInit();
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
        private System.Windows.Forms.Label lblChuThich;
        private System.Windows.Forms.Panel pnlGiua;
        private System.Windows.Forms.Panel pnlLuoi;
        private System.Windows.Forms.Panel pnlPhai;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lblTieuDeKhung;
        private System.Windows.Forms.Label lblCheDo;
        private System.Windows.Forms.Label lblMaThuoc;
        private System.Windows.Forms.TextBox txtMaThuoc;
        private System.Windows.Forms.Label lblTenThuoc;
        private System.Windows.Forms.TextBox txtTenThuoc;
        private System.Windows.Forms.Label lblMaVach;
        private System.Windows.Forms.TextBox txtMaVach;
        private System.Windows.Forms.Label lblKeDon;
        private System.Windows.Forms.CheckBox chkKeDon;
        private System.Windows.Forms.Label lblDVT;
        private System.Windows.Forms.ComboBox cboDVT;
        private System.Windows.Forms.Label lblDangDC;
        private System.Windows.Forms.ComboBox cboDangDC;
        private System.Windows.Forms.Label lblThanhPhan;
        private System.Windows.Forms.TextBox txtThanhPhan;
        private System.Windows.Forms.Label lblGiaNhap;
        private System.Windows.Forms.NumericUpDown nudGiaNhap;
        private System.Windows.Forms.Label lblGiaBan;
        private System.Windows.Forms.TextBox txtGiaBan;
        private System.Windows.Forms.Label lblSLHienCo;
        private System.Windows.Forms.TextBox txtSLHienCo;
        private System.Windows.Forms.Button btnXemLo;
        private System.Windows.Forms.Label lblNgaySX;
        private System.Windows.Forms.DateTimePicker dtpNgaySX;
        private System.Windows.Forms.Label lblHanSD;
        private System.Windows.Forms.DateTimePicker dtpHanSD;
        private System.Windows.Forms.Label lblGhiChuLo;
        private System.Windows.Forms.Label lblNuocSX;
        private System.Windows.Forms.ComboBox cboNuocSX;
        private System.Windows.Forms.Label lblChongChiDinh;
        private System.Windows.Forms.TextBox txtChongChiDinh;
        private System.Windows.Forms.Label lblCongDung;
        private System.Windows.Forms.TextBox txtCongDung;
        private System.Windows.Forms.Button btnChonCD;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
    }
}
