namespace QuanLyNhaThuoc.GUI
{
    partial class frmTuongTacThuoc
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
            this.dgvTuongTac = new System.Windows.Forms.DataGridView();
            this.pnlTim = new System.Windows.Forms.Panel();
            this.lblTim = new System.Windows.Forms.Label();
            this.txtTim = new System.Windows.Forms.TextBox();
            this.btnNapMau = new System.Windows.Forms.Button();
            this.lblSoCap = new System.Windows.Forms.Label();
            this.pnlNhap = new System.Windows.Forms.Panel();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblThuoc1 = new System.Windows.Forms.Label();
            this.cboThuoc1 = new System.Windows.Forms.ComboBox();
            this.lblThuoc2 = new System.Windows.Forms.Label();
            this.cboThuoc2 = new System.Windows.Forms.ComboBox();
            this.lblMucDo = new System.Windows.Forms.Label();
            this.cboMucDo = new System.Windows.Forms.ComboBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.lblCanhBao = new System.Windows.Forms.Label();
            this.tmrTim = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTuongTac)).BeginInit();
            this.pnlTim.SuspendLayout();
            this.pnlNhap.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTuongTac
            // 
            this.dgvTuongTac.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.dgvTuongTac.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTuongTac.AllowUserToAddRows = false;
            this.dgvTuongTac.AllowUserToDeleteRows = false;
            this.dgvTuongTac.AllowUserToResizeRows = false;
            this.dgvTuongTac.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTuongTac.BackgroundColor = System.Drawing.Color.White;
            this.dgvTuongTac.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTuongTac.ColumnHeadersHeight = 32;
            this.dgvTuongTac.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTuongTac.EnableHeadersVisualStyles = false;
            this.dgvTuongTac.MultiSelect = false;
            this.dgvTuongTac.RowHeadersVisible = false;
            this.dgvTuongTac.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTuongTac.RowTemplate.Height = 28;
            this.dgvTuongTac.ReadOnly = true;
            this.dgvTuongTac.Name = "dgvTuongTac";
            this.dgvTuongTac.TabIndex = 3;
            this.dgvTuongTac.SelectionChanged += new System.EventHandler(this.dgvTuongTac_SelectionChanged);
            this.dgvTuongTac.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTuongTac_DataBindingComplete);
            // 
            // pnlTim
            // 
            this.pnlTim.Controls.Add(this.lblTim);
            this.pnlTim.Controls.Add(this.txtTim);
            this.pnlTim.Controls.Add(this.btnNapMau);
            this.pnlTim.Controls.Add(this.lblSoCap);
            this.pnlTim.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.pnlTim.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTim.Location = new System.Drawing.Point(0, 184);
            this.pnlTim.Size = new System.Drawing.Size(1000, 46);
            this.pnlTim.Name = "pnlTim";
            this.pnlTim.TabIndex = 2;
            // 
            // lblTim
            // 
            this.lblTim.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTim.Location = new System.Drawing.Point(15, 13);
            this.lblTim.Text = "Tìm:";
            this.lblTim.AutoSize = true;
            this.lblTim.Name = "lblTim";
            this.lblTim.TabIndex = 0;
            // 
            // txtTim
            // 
            this.txtTim.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTim.Location = new System.Drawing.Point(55, 10);
            this.txtTim.Size = new System.Drawing.Size(300, 25);
            this.txtTim.Name = "txtTim";
            this.txtTim.TabIndex = 1;
            this.txtTim.TextChanged += new System.EventHandler(this.txtTim_TextChanged);
            // 
            // btnNapMau
            // 
            this.btnNapMau.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnNapMau.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNapMau.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNapMau.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNapMau.ForeColor = System.Drawing.Color.White;
            this.btnNapMau.Location = new System.Drawing.Point(370, 6);
            this.btnNapMau.Size = new System.Drawing.Size(190, 32);
            this.btnNapMau.Text = "📥 Nạp dữ liệu mẫu";
            this.btnNapMau.UseVisualStyleBackColor = false;
            this.btnNapMau.FlatAppearance.BorderSize = 0;
            this.btnNapMau.Name = "btnNapMau";
            this.btnNapMau.TabIndex = 2;
            this.btnNapMau.Click += new System.EventHandler(this.btnNapMau_Click);
            // 
            // lblSoCap
            // 
            this.lblSoCap.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoCap.Location = new System.Drawing.Point(580, 13);
            this.lblSoCap.Text = "";
            this.lblSoCap.AutoSize = true;
            this.lblSoCap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(98)))), ((int)(((byte)(104)))));
            this.lblSoCap.Name = "lblSoCap";
            this.lblSoCap.TabIndex = 3;
            // 
            // pnlNhap
            // 
            this.pnlNhap.Controls.Add(this.lblTieuDe);
            this.pnlNhap.Controls.Add(this.lblThuoc1);
            this.pnlNhap.Controls.Add(this.cboThuoc1);
            this.pnlNhap.Controls.Add(this.lblThuoc2);
            this.pnlNhap.Controls.Add(this.cboThuoc2);
            this.pnlNhap.Controls.Add(this.lblMucDo);
            this.pnlNhap.Controls.Add(this.cboMucDo);
            this.pnlNhap.Controls.Add(this.lblMoTa);
            this.pnlNhap.Controls.Add(this.txtMoTa);
            this.pnlNhap.Controls.Add(this.btnThem);
            this.pnlNhap.Controls.Add(this.btnSua);
            this.pnlNhap.Controls.Add(this.btnXoa);
            this.pnlNhap.Controls.Add(this.btnLamMoi);
            this.pnlNhap.BackColor = System.Drawing.Color.White;
            this.pnlNhap.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNhap.Location = new System.Drawing.Point(0, 34);
            this.pnlNhap.Size = new System.Drawing.Size(1000, 150);
            this.pnlNhap.Name = "pnlNhap";
            this.pnlNhap.TabIndex = 1;
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(15, 8);
            this.lblTieuDe.Text = "⚠ Cảnh báo tương tác thuốc khi bán";
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 0;
            // 
            // lblThuoc1
            // 
            this.lblThuoc1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThuoc1.Location = new System.Drawing.Point(17, 48);
            this.lblThuoc1.Text = "Thuốc 1:";
            this.lblThuoc1.AutoSize = true;
            this.lblThuoc1.Name = "lblThuoc1";
            this.lblThuoc1.TabIndex = 1;
            // 
            // cboThuoc1
            // 
            this.cboThuoc1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboThuoc1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboThuoc1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboThuoc1.FormattingEnabled = true;
            this.cboThuoc1.Location = new System.Drawing.Point(85, 45);
            this.cboThuoc1.Size = new System.Drawing.Size(300, 25);
            this.cboThuoc1.Name = "cboThuoc1";
            this.cboThuoc1.TabIndex = 2;
            // 
            // lblThuoc2
            // 
            this.lblThuoc2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThuoc2.Location = new System.Drawing.Point(400, 48);
            this.lblThuoc2.Text = "Thuốc 2:";
            this.lblThuoc2.AutoSize = true;
            this.lblThuoc2.Name = "lblThuoc2";
            this.lblThuoc2.TabIndex = 3;
            // 
            // cboThuoc2
            // 
            this.cboThuoc2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboThuoc2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboThuoc2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboThuoc2.FormattingEnabled = true;
            this.cboThuoc2.Location = new System.Drawing.Point(468, 45);
            this.cboThuoc2.Size = new System.Drawing.Size(300, 25);
            this.cboThuoc2.Name = "cboThuoc2";
            this.cboThuoc2.TabIndex = 4;
            // 
            // lblMucDo
            // 
            this.lblMucDo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMucDo.Location = new System.Drawing.Point(782, 48);
            this.lblMucDo.Text = "Mức độ:";
            this.lblMucDo.AutoSize = true;
            this.lblMucDo.Name = "lblMucDo";
            this.lblMucDo.TabIndex = 5;
            // 
            // cboMucDo
            // 
            this.cboMucDo.Items.AddRange(new object[] {
                "1 - Lưu ý",
                "2 - Thận trọng",
                "3 - Nghiêm trọng"});
            this.cboMucDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMucDo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboMucDo.FormattingEnabled = true;
            this.cboMucDo.Location = new System.Drawing.Point(840, 45);
            this.cboMucDo.Size = new System.Drawing.Size(145, 25);
            this.cboMucDo.Name = "cboMucDo";
            this.cboMucDo.TabIndex = 6;
            // 
            // lblMoTa
            // 
            this.lblMoTa.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoTa.Location = new System.Drawing.Point(17, 84);
            this.lblMoTa.Text = "Mô tả:";
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.TabIndex = 7;
            // 
            // txtMoTa
            // 
            this.txtMoTa.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMoTa.Location = new System.Drawing.Point(85, 81);
            this.txtMoTa.MaxLength = 500;
            this.txtMoTa.Multiline = true;
            this.txtMoTa.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMoTa.Size = new System.Drawing.Size(683, 60);
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.TabIndex = 8;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(782, 78);
            this.btnThem.Size = new System.Drawing.Size(100, 32);
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.Name = "btnThem";
            this.btnThem.TabIndex = 9;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSua.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(888, 78);
            this.btnSua.Size = new System.Drawing.Size(97, 32);
            this.btnSua.Text = "Lưu sửa";
            this.btnSua.UseVisualStyleBackColor = false;
            this.btnSua.FlatAppearance.BorderSize = 0;
            this.btnSua.Name = "btnSua";
            this.btnSua.TabIndex = 10;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnXoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(782, 114);
            this.btnXoa.Size = new System.Drawing.Size(100, 30);
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.FlatAppearance.BorderSize = 0;
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.TabIndex = 11;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(888, 114);
            this.btnLamMoi.Size = new System.Drawing.Size(97, 30);
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.FlatAppearance.BorderSize = 0;
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.TabIndex = 12;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // lblCanhBao
            // 
            this.lblCanhBao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(243)))), ((int)(((byte)(205)))));
            this.lblCanhBao.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCanhBao.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCanhBao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(100)))), ((int)(((byte)(4)))));
            this.lblCanhBao.Location = new System.Drawing.Point(0, 0);
            this.lblCanhBao.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblCanhBao.Size = new System.Drawing.Size(1000, 34);
            this.lblCanhBao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCanhBao.Text = "⚠ Dữ liệu tương tác là DỮ LIỆU MẪU để demo, KHÔNG phải cơ sở dữ liệu y khoa - không thay thế tư vấn của dược sĩ / bác sĩ.";
            this.lblCanhBao.Name = "lblCanhBao";
            this.lblCanhBao.TabIndex = 0;
            // 
            // tmrTim
            // 
            this.tmrTim.Interval = 300;
            this.tmrTim.Tick += new System.EventHandler(this.tmrTim_Tick);
            // 
            // frmTuongTacThuoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text = "Tương tác thuốc";
            this.Load += new System.EventHandler(this.frmTuongTacThuoc_Load);
            this.Controls.Add(this.dgvTuongTac);
            this.Controls.Add(this.pnlTim);
            this.Controls.Add(this.pnlNhap);
            this.Controls.Add(this.lblCanhBao);
            this.Name = "frmTuongTacThuoc";
            this.pnlNhap.ResumeLayout(false);
            this.pnlNhap.PerformLayout();
            this.pnlTim.ResumeLayout(false);
            this.pnlTim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTuongTac)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTuongTac;
        private System.Windows.Forms.Panel pnlTim;
        private System.Windows.Forms.Label lblTim;
        private System.Windows.Forms.TextBox txtTim;
        private System.Windows.Forms.Button btnNapMau;
        private System.Windows.Forms.Label lblSoCap;
        private System.Windows.Forms.Panel pnlNhap;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblThuoc1;
        private System.Windows.Forms.ComboBox cboThuoc1;
        private System.Windows.Forms.Label lblThuoc2;
        private System.Windows.Forms.ComboBox cboThuoc2;
        private System.Windows.Forms.Label lblMucDo;
        private System.Windows.Forms.ComboBox cboMucDo;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Label lblCanhBao;
        private System.Windows.Forms.Timer tmrTim;
    }
}
