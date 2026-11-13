namespace QuanLyNhaThuoc.GUI
{
    partial class frmHieuNang
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            chartArea1.Name = "caChinh";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "lgChinh";
            this.dgvKetQua = new System.Windows.Forms.DataGridView();
            this.colTruyVan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoDong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTruoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSau = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNhanh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMoTa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chartKetQua = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblGiaiThich = new System.Windows.Forms.Label();
            this.pnlTren = new System.Windows.Forms.Panel();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblThongTin = new System.Windows.Forms.Label();
            this.lblSoHoaDon = new System.Windows.Forms.Label();
            this.nudSoHoaDon = new System.Windows.Forms.NumericUpDown();
            this.btnSinh = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnDo = new System.Windows.Forms.Button();
            this.lblTrangThai = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKetQua)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartKetQua)).BeginInit();
            this.pnlTren.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoHoaDon)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvKetQua
            // 
            this.dgvKetQua.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colTruyVan,
            this.colSoDong,
            this.colTruoc,
            this.colSau,
            this.colNhanh,
            this.colMoTa});
            this.dgvKetQua.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.dgvKetQua.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvKetQua.AllowUserToAddRows = false;
            this.dgvKetQua.AllowUserToDeleteRows = false;
            this.dgvKetQua.AllowUserToResizeRows = false;
            this.dgvKetQua.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvKetQua.BackgroundColor = System.Drawing.Color.White;
            this.dgvKetQua.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvKetQua.ColumnHeadersHeight = 32;
            this.dgvKetQua.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKetQua.EnableHeadersVisualStyles = false;
            this.dgvKetQua.MultiSelect = false;
            this.dgvKetQua.RowHeadersVisible = false;
            this.dgvKetQua.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvKetQua.RowTemplate.Height = 28;
            this.dgvKetQua.Name = "dgvKetQua";
            this.dgvKetQua.TabIndex = 5;
            // 
            // colTruyVan
            // 
            this.colTruyVan.HeaderText = "Truy vấn";
            this.colTruyVan.FillWeight = 120F;
            this.colTruyVan.ReadOnly = true;
            this.colTruyVan.Name = "colTruyVan";
            // 
            // colSoDong
            // 
            this.colSoDong.HeaderText = "Số dòng";
            this.colSoDong.FillWeight = 55F;
            this.colSoDong.ReadOnly = true;
            this.colSoDong.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSoDong.Name = "colSoDong";
            // 
            // colTruoc
            // 
            this.colTruoc.HeaderText = "TRƯỚC - chưa có index (ms)";
            this.colTruoc.FillWeight = 85F;
            this.colTruoc.ReadOnly = true;
            this.colTruoc.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTruoc.Name = "colTruoc";
            // 
            // colSau
            // 
            this.colSau.HeaderText = "SAU - có index (ms)";
            this.colSau.FillWeight = 85F;
            this.colSau.ReadOnly = true;
            this.colSau.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSau.Name = "colSau";
            // 
            // colNhanh
            // 
            this.colNhanh.HeaderText = "Nhanh hơn";
            this.colNhanh.FillWeight = 65F;
            this.colNhanh.ReadOnly = true;
            this.colNhanh.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colNhanh.Name = "colNhanh";
            // 
            // colMoTa
            // 
            this.colMoTa.HeaderText = "Điều kiện tìm / index được dùng";
            this.colMoTa.FillWeight = 210F;
            this.colMoTa.ReadOnly = true;
            this.colMoTa.Name = "colMoTa";
            // 
            // chartKetQua
            // 
            this.chartKetQua.ChartAreas.Add(chartArea1);
            this.chartKetQua.Legends.Add(legend1);
            this.chartKetQua.BackColor = System.Drawing.Color.White;
            this.chartKetQua.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chartKetQua.Location = new System.Drawing.Point(0, 380);
            this.chartKetQua.Size = new System.Drawing.Size(1000, 200);
            this.chartKetQua.Name = "chartKetQua";
            this.chartKetQua.TabIndex = 6;
            // 
            // lblGiaiThich
            // 
            this.lblGiaiThich.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblGiaiThich.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGiaiThich.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(98)))), ((int)(((byte)(104)))));
            this.lblGiaiThich.Location = new System.Drawing.Point(0, 580);
            this.lblGiaiThich.Padding = new System.Windows.Forms.Padding(15, 4, 15, 4);
            this.lblGiaiThich.Size = new System.Drawing.Size(1000, 60);
            this.lblGiaiThich.Text = "Index giống mục lục của cuốn sách: không có index, SQL Server phải đọc TOÀN BỘ bảng hóa đơn (Clustered Index Scan) rồi mới lọc; có index theo NgayBan / MaThuoc thì nhảy thẳng tới đúng đoạn cần tìm (Index Seek). Bảng càng lớn, chênh lệch càng rõ. Thời gian = trung vị của 5 lần chạy (đo bằng Stopwatch), sau 1 lần chạy làm nóng.";
            this.lblGiaiThich.Name = "lblGiaiThich";
            this.lblGiaiThich.TabIndex = 7;
            // 
            // pnlTren
            // 
            this.pnlTren.Controls.Add(this.lblTieuDe);
            this.pnlTren.Controls.Add(this.lblThongTin);
            this.pnlTren.Controls.Add(this.lblSoHoaDon);
            this.pnlTren.Controls.Add(this.nudSoHoaDon);
            this.pnlTren.Controls.Add(this.btnSinh);
            this.pnlTren.Controls.Add(this.btnXoa);
            this.pnlTren.Controls.Add(this.btnDo);
            this.pnlTren.Controls.Add(this.lblTrangThai);
            this.pnlTren.BackColor = System.Drawing.Color.White;
            this.pnlTren.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTren.Location = new System.Drawing.Point(0, 0);
            this.pnlTren.Size = new System.Drawing.Size(1000, 150);
            this.pnlTren.Name = "pnlTren";
            this.pnlTren.TabIndex = 0;
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(15, 10);
            this.lblTieuDe.Text = "⚡ Đo tốc độ truy vấn TRƯỚC / SAU khi thêm index";
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 0;
            // 
            // lblThongTin
            // 
            this.lblThongTin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThongTin.Location = new System.Drawing.Point(17, 42);
            this.lblThongTin.Text = "";
            this.lblThongTin.Size = new System.Drawing.Size(950, 24);
            this.lblThongTin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(98)))), ((int)(((byte)(104)))));
            this.lblThongTin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right));
            this.lblThongTin.Name = "lblThongTin";
            this.lblThongTin.TabIndex = 1;
            // 
            // lblSoHoaDon
            // 
            this.lblSoHoaDon.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoHoaDon.Location = new System.Drawing.Point(17, 77);
            this.lblSoHoaDon.Text = "Số hóa đơn giả:";
            this.lblSoHoaDon.AutoSize = true;
            this.lblSoHoaDon.Name = "lblSoHoaDon";
            this.lblSoHoaDon.TabIndex = 2;
            // 
            // nudSoHoaDon
            // 
            this.nudSoHoaDon.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSoHoaDon.Increment = new decimal(new int[] { 10000, 0, 0, 0});
            this.nudSoHoaDon.Location = new System.Drawing.Point(125, 74);
            this.nudSoHoaDon.Maximum = new decimal(new int[] { 500000, 0, 0, 0});
            this.nudSoHoaDon.Minimum = new decimal(new int[] { 1000, 0, 0, 0});
            this.nudSoHoaDon.Size = new System.Drawing.Size(110, 25);
            this.nudSoHoaDon.ThousandsSeparator = true;
            this.nudSoHoaDon.Value = new decimal(new int[] { 100000, 0, 0, 0});
            this.nudSoHoaDon.Name = "nudSoHoaDon";
            this.nudSoHoaDon.TabIndex = 3;
            // 
            // btnSinh
            // 
            this.btnSinh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSinh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSinh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSinh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSinh.ForeColor = System.Drawing.Color.White;
            this.btnSinh.Location = new System.Drawing.Point(245, 70);
            this.btnSinh.Size = new System.Drawing.Size(170, 32);
            this.btnSinh.Text = "➕ Sinh dữ liệu giả";
            this.btnSinh.UseVisualStyleBackColor = false;
            this.btnSinh.FlatAppearance.BorderSize = 0;
            this.btnSinh.Name = "btnSinh";
            this.btnSinh.TabIndex = 4;
            this.btnSinh.Click += new System.EventHandler(this.btnSinh_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnXoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(423, 70);
            this.btnXoa.Size = new System.Drawing.Size(165, 32);
            this.btnXoa.Text = "🗑 Xóa dữ liệu giả";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.FlatAppearance.BorderSize = 0;
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.TabIndex = 5;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnDo
            // 
            this.btnDo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnDo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDo.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDo.ForeColor = System.Drawing.Color.White;
            this.btnDo.Location = new System.Drawing.Point(605, 66);
            this.btnDo.Size = new System.Drawing.Size(230, 40);
            this.btnDo.Text = "▶  ĐO TRƯỚC / SAU";
            this.btnDo.UseVisualStyleBackColor = false;
            this.btnDo.FlatAppearance.BorderSize = 0;
            this.btnDo.Name = "btnDo";
            this.btnDo.TabIndex = 6;
            this.btnDo.Click += new System.EventHandler(this.btnDo_Click);
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrangThai.Location = new System.Drawing.Point(17, 116);
            this.lblTrangThai.Text = "";
            this.lblTrangThai.Size = new System.Drawing.Size(950, 26);
            this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblTrangThai.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right));
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.TabIndex = 7;
            // 
            // frmHieuNang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text = "Hiệu năng truy vấn";
            this.Load += new System.EventHandler(this.frmHieuNang_Load);
            this.Controls.Add(this.dgvKetQua);
            this.Controls.Add(this.chartKetQua);
            this.Controls.Add(this.lblGiaiThich);
            this.Controls.Add(this.pnlTren);
            this.Name = "frmHieuNang";
            ((System.ComponentModel.ISupportInitialize)(this.nudSoHoaDon)).EndInit();
            this.pnlTren.ResumeLayout(false);
            this.pnlTren.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartKetQua)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKetQua)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvKetQua;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTruyVan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoDong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTruoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSau;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNhanh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMoTa;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartKetQua;
        private System.Windows.Forms.Label lblGiaiThich;
        private System.Windows.Forms.Panel pnlTren;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblThongTin;
        private System.Windows.Forms.Label lblSoHoaDon;
        private System.Windows.Forms.NumericUpDown nudSoHoaDon;
        private System.Windows.Forms.Button btnSinh;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnDo;
        private System.Windows.Forms.Label lblTrangThai;
    }
}
