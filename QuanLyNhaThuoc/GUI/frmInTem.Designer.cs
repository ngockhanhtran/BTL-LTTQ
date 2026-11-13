namespace QuanLyNhaThuoc.GUI
{
    partial class frmInTem
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
            this.dgvThuoc = new System.Windows.Forms.DataGridView();
            this.colIn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMaThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaVach = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoaiMa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiaBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlPhai = new System.Windows.Forms.Panel();
            this.picXemTruoc = new System.Windows.Forms.PictureBox();
            this.lblXemTruoc = new System.Windows.Forms.Label();
            this.pnlDuoi = new System.Windows.Forms.Panel();
            this.btnCapMa = new System.Windows.Forms.Button();
            this.btnChonTatCa = new System.Windows.Forms.Button();
            this.lblTongTem = new System.Windows.Forms.Label();
            this.btnIn = new System.Windows.Forms.Button();
            this.pnlTren = new System.Windows.Forms.Panel();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblTim = new System.Windows.Forms.Label();
            this.txtTim = new System.Windows.Forms.TextBox();
            this.chkChuaCoMa = new System.Windows.Forms.CheckBox();
            this.lblLoai = new System.Windows.Forms.Label();
            this.rdoMaVach = new System.Windows.Forms.RadioButton();
            this.rdoQR = new System.Windows.Forms.RadioButton();
            this.lblSoTem = new System.Windows.Forms.Label();
            this.nudSoTem = new System.Windows.Forms.NumericUpDown();
            this.lblHuongDan = new System.Windows.Forms.Label();
            this.tmrTim = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThuoc)).BeginInit();
            this.pnlPhai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picXemTruoc)).BeginInit();
            this.pnlDuoi.SuspendLayout();
            this.pnlTren.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoTem)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvThuoc
            // 
            this.dgvThuoc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colIn,
            this.colMaThuoc,
            this.colTenThuoc,
            this.colMaVach,
            this.colLoaiMa,
            this.colGiaBan});
            this.dgvThuoc.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.dgvThuoc.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvThuoc.AllowUserToAddRows = false;
            this.dgvThuoc.AllowUserToDeleteRows = false;
            this.dgvThuoc.AllowUserToResizeRows = false;
            this.dgvThuoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvThuoc.BackgroundColor = System.Drawing.Color.White;
            this.dgvThuoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvThuoc.ColumnHeadersHeight = 32;
            this.dgvThuoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThuoc.EnableHeadersVisualStyles = false;
            this.dgvThuoc.MultiSelect = false;
            this.dgvThuoc.RowHeadersVisible = false;
            this.dgvThuoc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThuoc.RowTemplate.Height = 28;
            this.dgvThuoc.ReadOnly = false;
            this.dgvThuoc.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvThuoc.Name = "dgvThuoc";
            this.dgvThuoc.TabIndex = 3;
            this.dgvThuoc.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvThuoc_CurrentCellDirtyStateChanged);
            this.dgvThuoc.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThuoc_CellValueChanged);
            this.dgvThuoc.SelectionChanged += new System.EventHandler(this.dgvThuoc_SelectionChanged);
            // 
            // colIn
            // 
            this.colIn.HeaderText = "In";
            this.colIn.FillWeight = 30F;
            this.colIn.Name = "colIn";
            // 
            // colMaThuoc
            // 
            this.colMaThuoc.HeaderText = "Mã thuốc";
            this.colMaThuoc.FillWeight = 55F;
            this.colMaThuoc.ReadOnly = true;
            this.colMaThuoc.Name = "colMaThuoc";
            // 
            // colTenThuoc
            // 
            this.colTenThuoc.HeaderText = "Tên thuốc";
            this.colTenThuoc.FillWeight = 150F;
            this.colTenThuoc.ReadOnly = true;
            this.colTenThuoc.Name = "colTenThuoc";
            // 
            // colMaVach
            // 
            this.colMaVach.HeaderText = "Mã vạch";
            this.colMaVach.FillWeight = 95F;
            this.colMaVach.ReadOnly = true;
            this.colMaVach.Name = "colMaVach";
            // 
            // colLoaiMa
            // 
            this.colLoaiMa.HeaderText = "Loại mã";
            this.colLoaiMa.FillWeight = 60F;
            this.colLoaiMa.ReadOnly = true;
            this.colLoaiMa.Name = "colLoaiMa";
            // 
            // colGiaBan
            // 
            this.colGiaBan.HeaderText = "Giá bán";
            this.colGiaBan.FillWeight = 60F;
            this.colGiaBan.ReadOnly = true;
            this.colGiaBan.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colGiaBan.Name = "colGiaBan";
            // 
            // pnlPhai
            // 
            this.pnlPhai.Controls.Add(this.picXemTruoc);
            this.pnlPhai.Controls.Add(this.lblXemTruoc);
            this.pnlPhai.BackColor = System.Drawing.Color.White;
            this.pnlPhai.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPhai.Location = new System.Drawing.Point(690, 96);
            this.pnlPhai.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.pnlPhai.Size = new System.Drawing.Size(310, 484);
            this.pnlPhai.Name = "pnlPhai";
            this.pnlPhai.TabIndex = 4;
            // 
            // picXemTruoc
            // 
            this.picXemTruoc.BackColor = System.Drawing.Color.White;
            this.picXemTruoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picXemTruoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.picXemTruoc.Location = new System.Drawing.Point(10, 40);
            this.picXemTruoc.Size = new System.Drawing.Size(290, 170);
            this.picXemTruoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picXemTruoc.TabStop = false;
            this.picXemTruoc.Name = "picXemTruoc";
            this.picXemTruoc.TabIndex = 1;
            // 
            // lblXemTruoc
            // 
            this.lblXemTruoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblXemTruoc.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXemTruoc.Location = new System.Drawing.Point(10, 10);
            this.lblXemTruoc.Size = new System.Drawing.Size(290, 30);
            this.lblXemTruoc.Text = "Xem trước 1 tem";
            this.lblXemTruoc.Name = "lblXemTruoc";
            this.lblXemTruoc.TabIndex = 0;
            // 
            // pnlDuoi
            // 
            this.pnlDuoi.Controls.Add(this.btnCapMa);
            this.pnlDuoi.Controls.Add(this.btnChonTatCa);
            this.pnlDuoi.Controls.Add(this.lblTongTem);
            this.pnlDuoi.Controls.Add(this.btnIn);
            this.pnlDuoi.BackColor = System.Drawing.Color.White;
            this.pnlDuoi.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDuoi.Location = new System.Drawing.Point(0, 580);
            this.pnlDuoi.Size = new System.Drawing.Size(1000, 60);
            this.pnlDuoi.Name = "pnlDuoi";
            this.pnlDuoi.TabIndex = 2;
            // 
            // btnCapMa
            // 
            this.btnCapMa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnCapMa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCapMa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCapMa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCapMa.ForeColor = System.Drawing.Color.White;
            this.btnCapMa.Location = new System.Drawing.Point(15, 11);
            this.btnCapMa.Size = new System.Drawing.Size(260, 38);
            this.btnCapMa.Text = "🔢 Cấp mã vạch cho thuốc đã chọn";
            this.btnCapMa.UseVisualStyleBackColor = false;
            this.btnCapMa.FlatAppearance.BorderSize = 0;
            this.btnCapMa.Name = "btnCapMa";
            this.btnCapMa.TabIndex = 0;
            this.btnCapMa.Click += new System.EventHandler(this.btnCapMa_Click);
            // 
            // btnChonTatCa
            // 
            this.btnChonTatCa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnChonTatCa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChonTatCa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChonTatCa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChonTatCa.ForeColor = System.Drawing.Color.White;
            this.btnChonTatCa.Location = new System.Drawing.Point(283, 11);
            this.btnChonTatCa.Size = new System.Drawing.Size(140, 38);
            this.btnChonTatCa.Text = "Chọn / bỏ tất cả";
            this.btnChonTatCa.UseVisualStyleBackColor = false;
            this.btnChonTatCa.FlatAppearance.BorderSize = 0;
            this.btnChonTatCa.Name = "btnChonTatCa";
            this.btnChonTatCa.TabIndex = 1;
            this.btnChonTatCa.Click += new System.EventHandler(this.btnChonTatCa_Click);
            // 
            // lblTongTem
            // 
            this.lblTongTem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTem.Location = new System.Drawing.Point(435, 21);
            this.lblTongTem.Text = "";
            this.lblTongTem.AutoSize = true;
            this.lblTongTem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(98)))), ((int)(((byte)(104)))));
            this.lblTongTem.Name = "lblTongTem";
            this.lblTongTem.TabIndex = 2;
            // 
            // btnIn
            // 
            this.btnIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIn.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIn.ForeColor = System.Drawing.Color.White;
            this.btnIn.Location = new System.Drawing.Point(765, 11);
            this.btnIn.Size = new System.Drawing.Size(220, 38);
            this.btnIn.Text = "🖨 Xem trước && In tem";
            this.btnIn.UseVisualStyleBackColor = false;
            this.btnIn.FlatAppearance.BorderSize = 0;
            this.btnIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Right));
            this.btnIn.Name = "btnIn";
            this.btnIn.TabIndex = 3;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // pnlTren
            // 
            this.pnlTren.Controls.Add(this.lblTieuDe);
            this.pnlTren.Controls.Add(this.lblTim);
            this.pnlTren.Controls.Add(this.txtTim);
            this.pnlTren.Controls.Add(this.chkChuaCoMa);
            this.pnlTren.Controls.Add(this.lblLoai);
            this.pnlTren.Controls.Add(this.rdoMaVach);
            this.pnlTren.Controls.Add(this.rdoQR);
            this.pnlTren.Controls.Add(this.lblSoTem);
            this.pnlTren.Controls.Add(this.nudSoTem);
            this.pnlTren.Controls.Add(this.lblHuongDan);
            this.pnlTren.BackColor = System.Drawing.Color.White;
            this.pnlTren.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTren.Location = new System.Drawing.Point(0, 0);
            this.pnlTren.Size = new System.Drawing.Size(1000, 96);
            this.pnlTren.Name = "pnlTren";
            this.pnlTren.TabIndex = 0;
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(15, 8);
            this.lblTieuDe.Text = "🏷 In tem mã vạch / QR cho thuốc";
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 0;
            // 
            // lblTim
            // 
            this.lblTim.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTim.Location = new System.Drawing.Point(17, 48);
            this.lblTim.Text = "Tìm thuốc:";
            this.lblTim.AutoSize = true;
            this.lblTim.Name = "lblTim";
            this.lblTim.TabIndex = 1;
            // 
            // txtTim
            // 
            this.txtTim.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTim.Location = new System.Drawing.Point(95, 45);
            this.txtTim.Size = new System.Drawing.Size(240, 25);
            this.txtTim.Name = "txtTim";
            this.txtTim.TabIndex = 2;
            this.txtTim.TextChanged += new System.EventHandler(this.txtTim_TextChanged);
            // 
            // chkChuaCoMa
            // 
            this.chkChuaCoMa.AutoSize = true;
            this.chkChuaCoMa.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkChuaCoMa.Location = new System.Drawing.Point(350, 47);
            this.chkChuaCoMa.Text = "Chỉ thuốc chưa có mã vạch";
            this.chkChuaCoMa.Name = "chkChuaCoMa";
            this.chkChuaCoMa.TabIndex = 3;
            this.chkChuaCoMa.CheckedChanged += new System.EventHandler(this.chkChuaCoMa_CheckedChanged);
            // 
            // lblLoai
            // 
            this.lblLoai.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoai.Location = new System.Drawing.Point(560, 48);
            this.lblLoai.Text = "Loại tem:";
            this.lblLoai.AutoSize = true;
            this.lblLoai.Name = "lblLoai";
            this.lblLoai.TabIndex = 4;
            // 
            // rdoMaVach
            // 
            this.rdoMaVach.AutoSize = true;
            this.rdoMaVach.Checked = true;
            this.rdoMaVach.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoMaVach.Location = new System.Drawing.Point(625, 46);
            this.rdoMaVach.TabStop = true;
            this.rdoMaVach.Text = "Mã vạch";
            this.rdoMaVach.Name = "rdoMaVach";
            this.rdoMaVach.TabIndex = 5;
            this.rdoMaVach.CheckedChanged += new System.EventHandler(this.rdoLoai_CheckedChanged);
            // 
            // rdoQR
            // 
            this.rdoQR.AutoSize = true;
            this.rdoQR.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoQR.Location = new System.Drawing.Point(705, 46);
            this.rdoQR.Text = "Mã QR";
            this.rdoQR.Name = "rdoQR";
            this.rdoQR.TabIndex = 6;
            this.rdoQR.CheckedChanged += new System.EventHandler(this.rdoLoai_CheckedChanged);
            // 
            // lblSoTem
            // 
            this.lblSoTem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoTem.Location = new System.Drawing.Point(785, 48);
            this.lblSoTem.Text = "Số tem / thuốc:";
            this.lblSoTem.AutoSize = true;
            this.lblSoTem.Name = "lblSoTem";
            this.lblSoTem.TabIndex = 7;
            // 
            // nudSoTem
            // 
            this.nudSoTem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSoTem.Location = new System.Drawing.Point(890, 45);
            this.nudSoTem.Maximum = new decimal(new int[] { 100, 0, 0, 0});
            this.nudSoTem.Minimum = new decimal(new int[] { 1, 0, 0, 0});
            this.nudSoTem.Size = new System.Drawing.Size(60, 25);
            this.nudSoTem.Value = new decimal(new int[] { 3, 0, 0, 0});
            this.nudSoTem.Name = "nudSoTem";
            this.nudSoTem.TabIndex = 8;
            this.nudSoTem.ValueChanged += new System.EventHandler(this.nudSoTem_ValueChanged);
            // 
            // lblHuongDan
            // 
            this.lblHuongDan.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHuongDan.Location = new System.Drawing.Point(17, 74);
            this.lblHuongDan.Text = "Tích cột \"In\" rồi bấm Xem trước & In (khổ A4, 24 tem / trang). Thuốc chưa có mã vạch: bấm \"Cấp mã vạch\" (mã EAN-13 nội bộ đầu số 200), in tem dán lên hộp là quét được khi bán.";
            this.lblHuongDan.Size = new System.Drawing.Size(970, 20);
            this.lblHuongDan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblHuongDan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right));
            this.lblHuongDan.Name = "lblHuongDan";
            this.lblHuongDan.TabIndex = 9;
            // 
            // tmrTim
            // 
            this.tmrTim.Interval = 300;
            this.tmrTim.Tick += new System.EventHandler(this.tmrTim_Tick);
            // 
            // frmInTem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text = "In tem mã vạch";
            this.Load += new System.EventHandler(this.frmInTem_Load);
            this.Controls.Add(this.dgvThuoc);
            this.Controls.Add(this.pnlPhai);
            this.Controls.Add(this.pnlDuoi);
            this.Controls.Add(this.pnlTren);
            this.Name = "frmInTem";
            ((System.ComponentModel.ISupportInitialize)(this.nudSoTem)).EndInit();
            this.pnlTren.ResumeLayout(false);
            this.pnlTren.PerformLayout();
            this.pnlDuoi.ResumeLayout(false);
            this.pnlDuoi.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picXemTruoc)).EndInit();
            this.pnlPhai.ResumeLayout(false);
            this.pnlPhai.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThuoc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvThuoc;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaVach;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoaiMa;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaBan;
        private System.Windows.Forms.Panel pnlPhai;
        private System.Windows.Forms.PictureBox picXemTruoc;
        private System.Windows.Forms.Label lblXemTruoc;
        private System.Windows.Forms.Panel pnlDuoi;
        private System.Windows.Forms.Button btnCapMa;
        private System.Windows.Forms.Button btnChonTatCa;
        private System.Windows.Forms.Label lblTongTem;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Panel pnlTren;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblTim;
        private System.Windows.Forms.TextBox txtTim;
        private System.Windows.Forms.CheckBox chkChuaCoMa;
        private System.Windows.Forms.Label lblLoai;
        private System.Windows.Forms.RadioButton rdoMaVach;
        private System.Windows.Forms.RadioButton rdoQR;
        private System.Windows.Forms.Label lblSoTem;
        private System.Windows.Forms.NumericUpDown nudSoTem;
        private System.Windows.Forms.Label lblHuongDan;
        private System.Windows.Forms.Timer tmrTim;
    }
}
