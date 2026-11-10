namespace QuanLyNhaThuoc.GUI
{
    partial class frmNhatKy
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
            System.Windows.Forms.DataGridViewCellStyle sNKXen = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle sNKTieuDe = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle sNKO = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTren = new System.Windows.Forms.Panel();
            this.lblTu = new System.Windows.Forms.Label();
            this.dtpTu = new System.Windows.Forms.DateTimePicker();
            this.lblDen = new System.Windows.Forms.Label();
            this.dtpDen = new System.Windows.Forms.DateTimePicker();
            this.lblNV = new System.Windows.Forms.Label();
            this.cboNhanVien = new System.Windows.Forms.ComboBox();
            this.lblHanhDong = new System.Windows.Forms.Label();
            this.cboHanhDong = new System.Windows.Forms.ComboBox();
            this.lblTuKhoa = new System.Windows.Forms.Label();
            this.txtTuKhoa = new System.Windows.Forms.TextBox();
            this.btnLoc = new System.Windows.Forms.Button();
            this.btnHomNay = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.pnlLuoi = new System.Windows.Forms.Panel();
            this.dgvNhatKy = new System.Windows.Forms.DataGridView();
            this.lblTong = new System.Windows.Forms.Label();
            this.pnlTren.SuspendLayout();
            this.pnlLuoi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhatKy)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTren
            // 
            sNKXen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            sNKTieuDe.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            sNKTieuDe.BackColor = System.Drawing.SystemColors.Control;
            sNKTieuDe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sNKTieuDe.ForeColor = System.Drawing.SystemColors.WindowText;
            sNKTieuDe.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            sNKTieuDe.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            sNKTieuDe.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            sNKO.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            sNKO.BackColor = System.Drawing.SystemColors.Window;
            sNKO.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sNKO.ForeColor = System.Drawing.SystemColors.ControlText;
            sNKO.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            sNKO.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            sNKO.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.pnlTren.Controls.Add(this.lblTu);
            this.pnlTren.Controls.Add(this.dtpTu);
            this.pnlTren.Controls.Add(this.lblDen);
            this.pnlTren.Controls.Add(this.dtpDen);
            this.pnlTren.Controls.Add(this.lblNV);
            this.pnlTren.Controls.Add(this.cboNhanVien);
            this.pnlTren.Controls.Add(this.lblHanhDong);
            this.pnlTren.Controls.Add(this.cboHanhDong);
            this.pnlTren.Controls.Add(this.lblTuKhoa);
            this.pnlTren.Controls.Add(this.txtTuKhoa);
            this.pnlTren.Controls.Add(this.btnLoc);
            this.pnlTren.Controls.Add(this.btnHomNay);
            this.pnlTren.Controls.Add(this.btnXuatExcel);
            this.pnlTren.BackColor = System.Drawing.Color.White;
            this.pnlTren.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTren.Location = new System.Drawing.Point(0, 0);
            this.pnlTren.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.pnlTren.Size = new System.Drawing.Size(1000, 92);
            this.pnlTren.Name = "pnlTren";
            this.pnlTren.TabIndex = 0;
            // 
            // lblTu
            // 
            this.lblTu.AutoSize = true;
            this.lblTu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTu.Location = new System.Drawing.Point(15, 16);
            this.lblTu.Text = "Từ ngày:";
            this.lblTu.Name = "lblTu";
            this.lblTu.TabIndex = 1;
            // 
            // dtpTu
            // 
            this.dtpTu.CustomFormat = "dd/MM/yyyy";
            this.dtpTu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTu.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTu.Location = new System.Drawing.Point(80, 13);
            this.dtpTu.Size = new System.Drawing.Size(115, 25);
            this.dtpTu.Name = "dtpTu";
            this.dtpTu.TabIndex = 2;
            // 
            // lblDen
            // 
            this.lblDen.AutoSize = true;
            this.lblDen.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDen.Location = new System.Drawing.Point(205, 16);
            this.lblDen.Text = "đến:";
            this.lblDen.Name = "lblDen";
            this.lblDen.TabIndex = 3;
            // 
            // dtpDen
            // 
            this.dtpDen.CustomFormat = "dd/MM/yyyy";
            this.dtpDen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDen.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDen.Location = new System.Drawing.Point(240, 13);
            this.dtpDen.Size = new System.Drawing.Size(115, 25);
            this.dtpDen.Name = "dtpDen";
            this.dtpDen.TabIndex = 4;
            // 
            // lblNV
            // 
            this.lblNV.AutoSize = true;
            this.lblNV.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNV.Location = new System.Drawing.Point(375, 16);
            this.lblNV.Text = "Nhân viên:";
            this.lblNV.Name = "lblNV";
            this.lblNV.TabIndex = 5;
            // 
            // cboNhanVien
            // 
            this.cboNhanVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboNhanVien.Location = new System.Drawing.Point(450, 13);
            this.cboNhanVien.Size = new System.Drawing.Size(220, 25);
            this.cboNhanVien.Name = "cboNhanVien";
            this.cboNhanVien.TabIndex = 6;
            // 
            // lblHanhDong
            // 
            this.lblHanhDong.AutoSize = true;
            this.lblHanhDong.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHanhDong.Location = new System.Drawing.Point(15, 54);
            this.lblHanhDong.Text = "Hành động:";
            this.lblHanhDong.Name = "lblHanhDong";
            this.lblHanhDong.TabIndex = 7;
            // 
            // cboHanhDong
            // 
            this.cboHanhDong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHanhDong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboHanhDong.Location = new System.Drawing.Point(95, 51);
            this.cboHanhDong.Size = new System.Drawing.Size(200, 25);
            this.cboHanhDong.Name = "cboHanhDong";
            this.cboHanhDong.TabIndex = 8;
            // 
            // lblTuKhoa
            // 
            this.lblTuKhoa.AutoSize = true;
            this.lblTuKhoa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTuKhoa.Location = new System.Drawing.Point(310, 54);
            this.lblTuKhoa.Text = "Từ khóa:";
            this.lblTuKhoa.Name = "lblTuKhoa";
            this.lblTuKhoa.TabIndex = 9;
            // 
            // txtTuKhoa
            // 
            this.txtTuKhoa.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTuKhoa.Location = new System.Drawing.Point(375, 51);
            this.txtTuKhoa.Size = new System.Drawing.Size(200, 25);
            this.txtTuKhoa.Name = "txtTuKhoa";
            this.txtTuKhoa.TabIndex = 10;
            this.txtTuKhoa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTuKhoa_KeyDown);
            // 
            // btnLoc
            // 
            this.btnLoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnLoc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoc.FlatAppearance.BorderSize = 0;
            this.btnLoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoc.ForeColor = System.Drawing.Color.White;
            this.btnLoc.Location = new System.Drawing.Point(590, 49);
            this.btnLoc.Size = new System.Drawing.Size(90, 30);
            this.btnLoc.Text = "🔍 Lọc";
            this.btnLoc.UseVisualStyleBackColor = false;
            this.btnLoc.Name = "btnLoc";
            this.btnLoc.TabIndex = 11;
            this.btnLoc.Click += new System.EventHandler(this.btnLoc_Click);
            // 
            // btnHomNay
            // 
            this.btnHomNay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnHomNay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHomNay.FlatAppearance.BorderSize = 0;
            this.btnHomNay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHomNay.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHomNay.ForeColor = System.Drawing.Color.White;
            this.btnHomNay.Location = new System.Drawing.Point(690, 49);
            this.btnHomNay.Size = new System.Drawing.Size(90, 30);
            this.btnHomNay.Text = "Hôm nay";
            this.btnHomNay.UseVisualStyleBackColor = false;
            this.btnHomNay.Name = "btnHomNay";
            this.btnHomNay.TabIndex = 12;
            this.btnHomNay.Click += new System.EventHandler(this.btnHomNay_Click);
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnXuatExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXuatExcel.FlatAppearance.BorderSize = 0;
            this.btnXuatExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatExcel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatExcel.ForeColor = System.Drawing.Color.White;
            this.btnXuatExcel.Location = new System.Drawing.Point(790, 49);
            this.btnXuatExcel.Size = new System.Drawing.Size(110, 30);
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = false;
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.TabIndex = 13;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // pnlLuoi
            // 
            this.pnlLuoi.Controls.Add(this.dgvNhatKy);
            this.pnlLuoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLuoi.Location = new System.Drawing.Point(0, 92);
            this.pnlLuoi.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlLuoi.Size = new System.Drawing.Size(1000, 518);
            this.pnlLuoi.Name = "pnlLuoi";
            this.pnlLuoi.TabIndex = 14;
            // 
            // dgvNhatKy
            // 
            this.dgvNhatKy.AllowUserToAddRows = false;
            this.dgvNhatKy.AllowUserToDeleteRows = false;
            this.dgvNhatKy.AllowUserToResizeRows = false;
            this.dgvNhatKy.AlternatingRowsDefaultCellStyle = sNKXen;
            this.dgvNhatKy.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNhatKy.BackgroundColor = System.Drawing.Color.White;
            this.dgvNhatKy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvNhatKy.ColumnHeadersDefaultCellStyle = sNKTieuDe;
            this.dgvNhatKy.DefaultCellStyle = sNKO;
            this.dgvNhatKy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNhatKy.Location = new System.Drawing.Point(0, 10);
            this.dgvNhatKy.MultiSelect = false;
            this.dgvNhatKy.ReadOnly = true;
            this.dgvNhatKy.RowHeadersVisible = false;
            this.dgvNhatKy.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNhatKy.Size = new System.Drawing.Size(1000, 508);
            this.dgvNhatKy.Name = "dgvNhatKy";
            this.dgvNhatKy.TabIndex = 15;
            this.dgvNhatKy.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvNhatKy_CellFormatting);
            this.dgvNhatKy.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvNhatKy_DataBindingComplete);
            // 
            // lblTong
            // 
            this.lblTong.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTong.ForeColor = System.Drawing.Color.DimGray;
            this.lblTong.Location = new System.Drawing.Point(0, 610);
            this.lblTong.Size = new System.Drawing.Size(1000, 30);
            this.lblTong.Text = "";
            this.lblTong.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTong.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblTong.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTong.Name = "lblTong";
            this.lblTong.TabIndex = 16;
            // 
            // frmNhatKy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Text = "Nhật ký hoạt động";
            this.Controls.Add(this.pnlLuoi);
            this.Controls.Add(this.lblTong);
            this.Controls.Add(this.pnlTren);
            this.Name = "frmNhatKy";
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhatKy)).EndInit();
            this.pnlLuoi.ResumeLayout(false);
            this.pnlLuoi.PerformLayout();
            this.pnlTren.ResumeLayout(false);
            this.pnlTren.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTren;
        private System.Windows.Forms.Label lblTu;
        private System.Windows.Forms.DateTimePicker dtpTu;
        private System.Windows.Forms.Label lblDen;
        private System.Windows.Forms.DateTimePicker dtpDen;
        private System.Windows.Forms.Label lblNV;
        private System.Windows.Forms.ComboBox cboNhanVien;
        private System.Windows.Forms.Label lblHanhDong;
        private System.Windows.Forms.ComboBox cboHanhDong;
        private System.Windows.Forms.Label lblTuKhoa;
        private System.Windows.Forms.TextBox txtTuKhoa;
        private System.Windows.Forms.Button btnLoc;
        private System.Windows.Forms.Button btnHomNay;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Panel pnlLuoi;
        private System.Windows.Forms.DataGridView dgvNhatKy;
        private System.Windows.Forms.Label lblTong;
    }
}
