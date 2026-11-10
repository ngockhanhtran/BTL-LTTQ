namespace QuanLyNhaThuoc.GUI
{
    partial class frmNhapHang
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
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lblLapPhieu = new System.Windows.Forms.Label();
            this.lblMaHDN = new System.Windows.Forms.Label();
            this.txtMaHDN = new System.Windows.Forms.TextBox();
            this.lblMaNV = new System.Windows.Forms.Label();
            this.txtMaNV = new System.Windows.Forms.TextBox();
            this.lblTenNV = new System.Windows.Forms.Label();
            this.lblMaNCC = new System.Windows.Forms.Label();
            this.txtMaNCC = new System.Windows.Forms.TextBox();
            this.lblTenNCC = new System.Windows.Forms.Label();
            this.lblMaThuoc = new System.Windows.Forms.Label();
            this.txtMaThuoc = new System.Windows.Forms.TextBox();
            this.lblTenThuoc = new System.Windows.Forms.Label();
            this.lblSLNhap = new System.Windows.Forms.Label();
            this.txtSLNhap = new System.Windows.Forms.TextBox();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.lblSoLo = new System.Windows.Forms.Label();
            this.txtSoLo = new System.Windows.Forms.TextBox();
            this.lblNgaySX = new System.Windows.Forms.Label();
            this.dtpNgaySX = new System.Windows.Forms.DateTimePicker();
            this.lblHanSD = new System.Windows.Forms.Label();
            this.dtpHanSD = new System.Windows.Forms.DateTimePicker();
            this.lblGoiYLo = new System.Windows.Forms.Label();
            this.btnThemNhap = new System.Windows.Forms.Button();
            this.lblExcel = new System.Windows.Forms.Label();
            this.lblExcelMoTa = new System.Windows.Forms.Label();
            this.btnImportExcel = new System.Windows.Forms.Button();
            this.btnTaiFileMau = new System.Windows.Forms.Button();
            this.dgvKetQua = new System.Windows.Forms.DataGridView();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblTraCuu = new System.Windows.Forms.Label();
            this.txtTuKhoaSearch = new System.Windows.Forms.TextBox();
            this.btnTimKiemHDN = new System.Windows.Forms.Button();
            this.pnlRight.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKetQua)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.dgvKetQua);
            this.pnlRight.Controls.Add(this.pnlSearch);
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(380, 0);
            this.pnlRight.Padding = new System.Windows.Forms.Padding(15);
            this.pnlRight.Size = new System.Drawing.Size(620, 640);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.TabIndex = 0;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.lblLapPhieu);
            this.pnlLeft.Controls.Add(this.lblMaHDN);
            this.pnlLeft.Controls.Add(this.txtMaHDN);
            this.pnlLeft.Controls.Add(this.lblMaNV);
            this.pnlLeft.Controls.Add(this.txtMaNV);
            this.pnlLeft.Controls.Add(this.lblTenNV);
            this.pnlLeft.Controls.Add(this.lblMaNCC);
            this.pnlLeft.Controls.Add(this.txtMaNCC);
            this.pnlLeft.Controls.Add(this.lblTenNCC);
            this.pnlLeft.Controls.Add(this.lblMaThuoc);
            this.pnlLeft.Controls.Add(this.txtMaThuoc);
            this.pnlLeft.Controls.Add(this.lblTenThuoc);
            this.pnlLeft.Controls.Add(this.lblSLNhap);
            this.pnlLeft.Controls.Add(this.txtSLNhap);
            this.pnlLeft.Controls.Add(this.lblDonGia);
            this.pnlLeft.Controls.Add(this.txtDonGia);
            this.pnlLeft.Controls.Add(this.lblSoLo);
            this.pnlLeft.Controls.Add(this.txtSoLo);
            this.pnlLeft.Controls.Add(this.lblNgaySX);
            this.pnlLeft.Controls.Add(this.dtpNgaySX);
            this.pnlLeft.Controls.Add(this.lblHanSD);
            this.pnlLeft.Controls.Add(this.dtpHanSD);
            this.pnlLeft.Controls.Add(this.lblGoiYLo);
            this.pnlLeft.Controls.Add(this.btnThemNhap);
            this.pnlLeft.Controls.Add(this.lblExcel);
            this.pnlLeft.Controls.Add(this.lblExcelMoTa);
            this.pnlLeft.Controls.Add(this.btnImportExcel);
            this.pnlLeft.Controls.Add(this.btnTaiFileMau);
            this.pnlLeft.AutoScroll = true;
            this.pnlLeft.BackColor = System.Drawing.Color.White;
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(15);
            this.pnlLeft.Size = new System.Drawing.Size(380, 640);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.TabIndex = 1;
            // 
            // lblLapPhieu
            // 
            this.lblLapPhieu.AutoSize = true;
            this.lblLapPhieu.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLapPhieu.Location = new System.Drawing.Point(15, 15);
            this.lblLapPhieu.Text = "Lập Phiếu Nhập Hàng";
            this.lblLapPhieu.Name = "lblLapPhieu";
            this.lblLapPhieu.TabIndex = 2;
            // 
            // lblMaHDN
            // 
            this.lblMaHDN.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaHDN.Location = new System.Drawing.Point(15, 60);
            this.lblMaHDN.Size = new System.Drawing.Size(100, 23);
            this.lblMaHDN.Text = "Mã HĐ Nhập:";
            this.lblMaHDN.Name = "lblMaHDN";
            this.lblMaHDN.TabIndex = 3;
            // 
            // txtMaHDN
            // 
            this.txtMaHDN.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaHDN.Location = new System.Drawing.Point(115, 57);
            this.txtMaHDN.Size = new System.Drawing.Size(230, 25);
            this.txtMaHDN.Name = "txtMaHDN";
            this.txtMaHDN.TabIndex = 4;
            // 
            // lblMaNV
            // 
            this.lblMaNV.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaNV.Location = new System.Drawing.Point(15, 105);
            this.lblMaNV.Size = new System.Drawing.Size(100, 23);
            this.lblMaNV.Text = "Mã NV (Kho):";
            this.lblMaNV.Name = "lblMaNV";
            this.lblMaNV.TabIndex = 5;
            // 
            // txtMaNV
            // 
            this.txtMaNV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaNV.Location = new System.Drawing.Point(115, 102);
            this.txtMaNV.Size = new System.Drawing.Size(230, 25);
            this.txtMaNV.Name = "txtMaNV";
            this.txtMaNV.TabIndex = 6;
            this.txtMaNV.TextChanged += new System.EventHandler(this.txtMaNV_TextChanged);
            // 
            // lblTenNV
            // 
            this.lblTenNV.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenNV.ForeColor = System.Drawing.Color.Gray;
            this.lblTenNV.Location = new System.Drawing.Point(115, 129);
            this.lblTenNV.Size = new System.Drawing.Size(240, 32);
            this.lblTenNV.Text = "";
            this.lblTenNV.Name = "lblTenNV";
            this.lblTenNV.TabIndex = 7;
            // 
            // lblMaNCC
            // 
            this.lblMaNCC.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaNCC.Location = new System.Drawing.Point(15, 178);
            this.lblMaNCC.Size = new System.Drawing.Size(100, 23);
            this.lblMaNCC.Text = "Mã NCC:";
            this.lblMaNCC.Name = "lblMaNCC";
            this.lblMaNCC.TabIndex = 8;
            // 
            // txtMaNCC
            // 
            this.txtMaNCC.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaNCC.Location = new System.Drawing.Point(115, 175);
            this.txtMaNCC.Size = new System.Drawing.Size(230, 25);
            this.txtMaNCC.Name = "txtMaNCC";
            this.txtMaNCC.TabIndex = 9;
            this.txtMaNCC.TextChanged += new System.EventHandler(this.txtMaNCC_TextChanged);
            // 
            // lblTenNCC
            // 
            this.lblTenNCC.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenNCC.ForeColor = System.Drawing.Color.Gray;
            this.lblTenNCC.Location = new System.Drawing.Point(115, 202);
            this.lblTenNCC.Size = new System.Drawing.Size(240, 32);
            this.lblTenNCC.Text = "";
            this.lblTenNCC.Name = "lblTenNCC";
            this.lblTenNCC.TabIndex = 10;
            // 
            // lblMaThuoc
            // 
            this.lblMaThuoc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaThuoc.Location = new System.Drawing.Point(15, 251);
            this.lblMaThuoc.Size = new System.Drawing.Size(100, 23);
            this.lblMaThuoc.Text = "Mã Thuốc:";
            this.lblMaThuoc.Name = "lblMaThuoc";
            this.lblMaThuoc.TabIndex = 11;
            // 
            // txtMaThuoc
            // 
            this.txtMaThuoc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaThuoc.Location = new System.Drawing.Point(115, 248);
            this.txtMaThuoc.Size = new System.Drawing.Size(230, 25);
            this.txtMaThuoc.Name = "txtMaThuoc";
            this.txtMaThuoc.TabIndex = 12;
            this.txtMaThuoc.TextChanged += new System.EventHandler(this.txtMaThuoc_TextChanged);
            // 
            // lblTenThuoc
            // 
            this.lblTenThuoc.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenThuoc.ForeColor = System.Drawing.Color.Gray;
            this.lblTenThuoc.Location = new System.Drawing.Point(115, 275);
            this.lblTenThuoc.Size = new System.Drawing.Size(240, 32);
            this.lblTenThuoc.Text = "";
            this.lblTenThuoc.Name = "lblTenThuoc";
            this.lblTenThuoc.TabIndex = 13;
            // 
            // lblSLNhap
            // 
            this.lblSLNhap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSLNhap.Location = new System.Drawing.Point(15, 324);
            this.lblSLNhap.Size = new System.Drawing.Size(100, 23);
            this.lblSLNhap.Text = "Số Lượng:";
            this.lblSLNhap.Name = "lblSLNhap";
            this.lblSLNhap.TabIndex = 14;
            // 
            // txtSLNhap
            // 
            this.txtSLNhap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSLNhap.Location = new System.Drawing.Point(115, 321);
            this.txtSLNhap.Size = new System.Drawing.Size(230, 25);
            this.txtSLNhap.Name = "txtSLNhap";
            this.txtSLNhap.TabIndex = 15;
            // 
            // lblDonGia
            // 
            this.lblDonGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDonGia.Location = new System.Drawing.Point(15, 369);
            this.lblDonGia.Size = new System.Drawing.Size(100, 23);
            this.lblDonGia.Text = "Đơn Giá:";
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.TabIndex = 16;
            // 
            // txtDonGia
            // 
            this.txtDonGia.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDonGia.Location = new System.Drawing.Point(115, 366);
            this.txtDonGia.Size = new System.Drawing.Size(230, 25);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.TabIndex = 17;
            // 
            // lblSoLo
            // 
            this.lblSoLo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoLo.Location = new System.Drawing.Point(15, 414);
            this.lblSoLo.Size = new System.Drawing.Size(100, 23);
            this.lblSoLo.Text = "Số lô:";
            this.lblSoLo.Name = "lblSoLo";
            this.lblSoLo.TabIndex = 18;
            // 
            // txtSoLo
            // 
            this.txtSoLo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSoLo.Location = new System.Drawing.Point(115, 411);
            this.txtSoLo.MaxLength = 30;
            this.txtSoLo.Size = new System.Drawing.Size(230, 25);
            this.txtSoLo.Name = "txtSoLo";
            this.txtSoLo.TabIndex = 19;
            // 
            // lblNgaySX
            // 
            this.lblNgaySX.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgaySX.Location = new System.Drawing.Point(15, 459);
            this.lblNgaySX.Size = new System.Drawing.Size(100, 23);
            this.lblNgaySX.Text = "Ngày SX:";
            this.lblNgaySX.Name = "lblNgaySX";
            this.lblNgaySX.TabIndex = 20;
            // 
            // dtpNgaySX
            // 
            this.dtpNgaySX.CustomFormat = "dd/MM/yyyy";
            this.dtpNgaySX.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgaySX.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgaySX.Location = new System.Drawing.Point(115, 456);
            this.dtpNgaySX.Size = new System.Drawing.Size(150, 25);
            this.dtpNgaySX.Checked = false;
            this.dtpNgaySX.ShowCheckBox = true;
            this.dtpNgaySX.Name = "dtpNgaySX";
            this.dtpNgaySX.TabIndex = 21;
            // 
            // lblHanSD
            // 
            this.lblHanSD.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHanSD.Location = new System.Drawing.Point(15, 504);
            this.lblHanSD.Size = new System.Drawing.Size(100, 23);
            this.lblHanSD.Text = "Hạn SD (*):";
            this.lblHanSD.Name = "lblHanSD";
            this.lblHanSD.TabIndex = 22;
            // 
            // dtpHanSD
            // 
            this.dtpHanSD.CustomFormat = "dd/MM/yyyy";
            this.dtpHanSD.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpHanSD.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHanSD.Location = new System.Drawing.Point(115, 501);
            this.dtpHanSD.Size = new System.Drawing.Size(150, 25);
            this.dtpHanSD.Name = "dtpHanSD";
            this.dtpHanSD.TabIndex = 23;
            // 
            // lblGoiYLo
            // 
            this.lblGoiYLo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGoiYLo.ForeColor = System.Drawing.Color.Gray;
            this.lblGoiYLo.Location = new System.Drawing.Point(115, 529);
            this.lblGoiYLo.Size = new System.Drawing.Size(240, 32);
            this.lblGoiYLo.Text = "Mỗi lần nhập = 1 lô (bán theo FEFO: lô hết hạn trước bán trước)";
            this.lblGoiYLo.Name = "lblGoiYLo";
            this.lblGoiYLo.TabIndex = 24;
            // 
            // btnThemNhap
            // 
            this.btnThemNhap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnThemNhap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemNhap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemNhap.ForeColor = System.Drawing.Color.White;
            this.btnThemNhap.Location = new System.Drawing.Point(15, 574);
            this.btnThemNhap.Size = new System.Drawing.Size(330, 42);
            this.btnThemNhap.Text = "Xác Nhận Nhập (Tạo lô, tăng tồn kho)";
            this.btnThemNhap.UseVisualStyleBackColor = false;
            this.btnThemNhap.Name = "btnThemNhap";
            this.btnThemNhap.TabIndex = 25;
            this.btnThemNhap.Click += new System.EventHandler(this.BtnThemNhap_Click);
            // 
            // lblExcel
            // 
            this.lblExcel.AutoSize = true;
            this.lblExcel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExcel.Location = new System.Drawing.Point(15, 646);
            this.lblExcel.Text = "Nhập Hàng Từ File Excel";
            this.lblExcel.Name = "lblExcel";
            this.lblExcel.TabIndex = 26;
            // 
            // lblExcelMoTa
            // 
            this.lblExcelMoTa.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExcelMoTa.ForeColor = System.Drawing.Color.DimGray;
            this.lblExcelMoTa.Location = new System.Drawing.Point(15, 674);
            this.lblExcelMoTa.Size = new System.Drawing.Size(330, 36);
            this.lblExcelMoTa.Text = "Nhập nhiều phiếu, cho phép thuốc mới (tự thêm thuốc, mã vạch, công dụng); mỗi dòng tạo 1 lô (SoLo, NgaySX, HanSD).";
            this.lblExcelMoTa.Name = "lblExcelMoTa";
            this.lblExcelMoTa.TabIndex = 27;
            // 
            // btnImportExcel
            // 
            this.btnImportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(115)))), ((int)(((byte)(70)))));
            this.btnImportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportExcel.ForeColor = System.Drawing.Color.White;
            this.btnImportExcel.Location = new System.Drawing.Point(15, 714);
            this.btnImportExcel.Size = new System.Drawing.Size(330, 42);
            this.btnImportExcel.Text = "Import Phiếu Nhập (Excel)";
            this.btnImportExcel.UseVisualStyleBackColor = false;
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.TabIndex = 28;
            this.btnImportExcel.Click += new System.EventHandler(this.BtnImportExcel_Click);
            // 
            // btnTaiFileMau
            // 
            this.btnTaiFileMau.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnTaiFileMau.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTaiFileMau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaiFileMau.ForeColor = System.Drawing.Color.White;
            this.btnTaiFileMau.Location = new System.Drawing.Point(15, 764);
            this.btnTaiFileMau.Size = new System.Drawing.Size(330, 34);
            this.btnTaiFileMau.Text = "Tải file Excel mẫu";
            this.btnTaiFileMau.UseVisualStyleBackColor = false;
            this.btnTaiFileMau.Name = "btnTaiFileMau";
            this.btnTaiFileMau.TabIndex = 29;
            this.btnTaiFileMau.Click += new System.EventHandler(this.BtnTaiFileMau_Click);
            // 
            // dgvKetQua
            // 
            this.dgvKetQua.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvKetQua.BackgroundColor = System.Drawing.Color.White;
            this.dgvKetQua.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKetQua.Location = new System.Drawing.Point(15, 90);
            this.dgvKetQua.ReadOnly = true;
            this.dgvKetQua.Size = new System.Drawing.Size(590, 535);
            this.dgvKetQua.Name = "dgvKetQua";
            this.dgvKetQua.TabIndex = 30;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.lblTraCuu);
            this.pnlSearch.Controls.Add(this.txtTuKhoaSearch);
            this.pnlSearch.Controls.Add(this.btnTimKiemHDN);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(15, 15);
            this.pnlSearch.Size = new System.Drawing.Size(590, 75);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.TabIndex = 31;
            // 
            // lblTraCuu
            // 
            this.lblTraCuu.AutoSize = true;
            this.lblTraCuu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTraCuu.Location = new System.Drawing.Point(0, 0);
            this.lblTraCuu.Text = "Tra cứu Hóa Đơn Nhập (Theo Mã/Nhân viên/NCC):";
            this.lblTraCuu.Name = "lblTraCuu";
            this.lblTraCuu.TabIndex = 32;
            // 
            // txtTuKhoaSearch
            // 
            this.txtTuKhoaSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTuKhoaSearch.Location = new System.Drawing.Point(0, 32);
            this.txtTuKhoaSearch.Size = new System.Drawing.Size(320, 25);
            this.txtTuKhoaSearch.Name = "txtTuKhoaSearch";
            this.txtTuKhoaSearch.TabIndex = 33;
            // 
            // btnTimKiemHDN
            // 
            this.btnTimKiemHDN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnTimKiemHDN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiemHDN.ForeColor = System.Drawing.Color.White;
            this.btnTimKiemHDN.Location = new System.Drawing.Point(330, 30);
            this.btnTimKiemHDN.Size = new System.Drawing.Size(120, 30);
            this.btnTimKiemHDN.Text = "Tìm kiếm HĐN";
            this.btnTimKiemHDN.UseVisualStyleBackColor = false;
            this.btnTimKiemHDN.Name = "btnTimKiemHDN";
            this.btnTimKiemHDN.TabIndex = 34;
            this.btnTimKiemHDN.Click += new System.EventHandler(this.BtnTimKiemHDN_Click);
            // 
            // frmNhapHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Text = "Quản lý Nhập Hàng & Kho";
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Name = "frmNhapHang";
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKetQua)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label lblLapPhieu;
        private System.Windows.Forms.Label lblMaHDN;
        private System.Windows.Forms.TextBox txtMaHDN;
        private System.Windows.Forms.Label lblMaNV;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.Label lblTenNV;
        private System.Windows.Forms.Label lblMaNCC;
        private System.Windows.Forms.TextBox txtMaNCC;
        private System.Windows.Forms.Label lblTenNCC;
        private System.Windows.Forms.Label lblMaThuoc;
        private System.Windows.Forms.TextBox txtMaThuoc;
        private System.Windows.Forms.Label lblTenThuoc;
        private System.Windows.Forms.Label lblSLNhap;
        private System.Windows.Forms.TextBox txtSLNhap;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.Label lblSoLo;
        private System.Windows.Forms.TextBox txtSoLo;
        private System.Windows.Forms.Label lblNgaySX;
        private System.Windows.Forms.DateTimePicker dtpNgaySX;
        private System.Windows.Forms.Label lblHanSD;
        private System.Windows.Forms.DateTimePicker dtpHanSD;
        private System.Windows.Forms.Label lblGoiYLo;
        private System.Windows.Forms.Button btnThemNhap;
        private System.Windows.Forms.Label lblExcel;
        private System.Windows.Forms.Label lblExcelMoTa;
        private System.Windows.Forms.Button btnImportExcel;
        private System.Windows.Forms.Button btnTaiFileMau;
        private System.Windows.Forms.DataGridView dgvKetQua;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Label lblTraCuu;
        private System.Windows.Forms.TextBox txtTuKhoaSearch;
        private System.Windows.Forms.Button btnTimKiemHDN;
    }
}
