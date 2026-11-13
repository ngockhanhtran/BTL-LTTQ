namespace QuanLyNhaThuoc.GUI
{
    partial class frmSaoLuu
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
            System.Windows.Forms.DataGridViewCellStyle sLSXen = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle sLSTieuDe = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle sLSO = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTren = new System.Windows.Forms.Panel();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblCSDL = new System.Windows.Forms.Label();
            this.grpSaoLuu = new System.Windows.Forms.GroupBox();
            this.lblThuMuc = new System.Windows.Forms.Label();
            this.txtThuMuc = new System.Windows.Forms.TextBox();
            this.btnChonThuMuc = new System.Windows.Forms.Button();
            this.lblTenFile = new System.Windows.Forms.Label();
            this.txtTenFile = new System.Windows.Forms.TextBox();
            this.btnSaoLuu = new System.Windows.Forms.Button();
            this.grpKhoiPhuc = new System.Windows.Forms.GroupBox();
            this.lblFileKP = new System.Windows.Forms.Label();
            this.txtFileKP = new System.Windows.Forms.TextBox();
            this.btnChonFile = new System.Windows.Forms.Button();
            this.lblCanhBao = new System.Windows.Forms.Label();
            this.btnKhoiPhuc = new System.Windows.Forms.Button();
            this.pnlLuoi = new System.Windows.Forms.Panel();
            this.dgvLichSu = new System.Windows.Forms.DataGridView();
            this.lblLichSu = new System.Windows.Forms.Label();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.pnlTren.SuspendLayout();
            this.grpSaoLuu.SuspendLayout();
            this.grpKhoiPhuc.SuspendLayout();
            this.pnlLuoi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichSu)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTren
            // 
            sLSXen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            sLSTieuDe.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            sLSTieuDe.BackColor = System.Drawing.SystemColors.Control;
            sLSTieuDe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sLSTieuDe.ForeColor = System.Drawing.SystemColors.WindowText;
            sLSTieuDe.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            sLSTieuDe.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            sLSTieuDe.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            sLSO.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            sLSO.BackColor = System.Drawing.SystemColors.Window;
            sLSO.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sLSO.ForeColor = System.Drawing.SystemColors.ControlText;
            sLSO.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            sLSO.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            sLSO.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.pnlTren.Controls.Add(this.lblTieuDe);
            this.pnlTren.Controls.Add(this.lblCSDL);
            this.pnlTren.Controls.Add(this.grpSaoLuu);
            this.pnlTren.Controls.Add(this.grpKhoiPhuc);
            this.pnlTren.BackColor = System.Drawing.Color.White;
            this.pnlTren.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTren.Location = new System.Drawing.Point(0, 0);
            this.pnlTren.Size = new System.Drawing.Size(1000, 236);
            this.pnlTren.Name = "pnlTren";
            this.pnlTren.TabIndex = 0;
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTieuDe.Location = new System.Drawing.Point(20, 14);
            this.lblTieuDe.Text = "Sao lưu & khôi phục cơ sở dữ liệu";
            this.lblTieuDe.UseMnemonic = false;
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 1;
            // 
            // lblCSDL
            // 
            this.lblCSDL.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCSDL.ForeColor = System.Drawing.Color.DimGray;
            this.lblCSDL.Location = new System.Drawing.Point(20, 46);
            this.lblCSDL.Size = new System.Drawing.Size(950, 22);
            this.lblCSDL.Text = "";
            this.lblCSDL.Name = "lblCSDL";
            this.lblCSDL.TabIndex = 2;
            // 
            // grpSaoLuu
            // 
            this.grpSaoLuu.Controls.Add(this.lblThuMuc);
            this.grpSaoLuu.Controls.Add(this.txtThuMuc);
            this.grpSaoLuu.Controls.Add(this.btnChonThuMuc);
            this.grpSaoLuu.Controls.Add(this.lblTenFile);
            this.grpSaoLuu.Controls.Add(this.txtTenFile);
            this.grpSaoLuu.Controls.Add(this.btnSaoLuu);
            this.grpSaoLuu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSaoLuu.Location = new System.Drawing.Point(20, 76);
            this.grpSaoLuu.Size = new System.Drawing.Size(470, 148);
            this.grpSaoLuu.Text = "① Sao lưu (BACKUP)";
            this.grpSaoLuu.Name = "grpSaoLuu";
            this.grpSaoLuu.TabIndex = 3;
            // 
            // lblThuMuc
            // 
            this.lblThuMuc.AutoSize = true;
            this.lblThuMuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThuMuc.Location = new System.Drawing.Point(15, 28);
            this.lblThuMuc.Text = "Thư mục trên máy chạy SQL Server:";
            this.lblThuMuc.Name = "lblThuMuc";
            this.lblThuMuc.TabIndex = 4;
            // 
            // txtThuMuc
            // 
            this.txtThuMuc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThuMuc.Location = new System.Drawing.Point(15, 50);
            this.txtThuMuc.Size = new System.Drawing.Size(350, 25);
            this.txtThuMuc.Name = "txtThuMuc";
            this.txtThuMuc.TabIndex = 5;
            // 
            // btnChonThuMuc
            // 
            this.btnChonThuMuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChonThuMuc.Location = new System.Drawing.Point(371, 49);
            this.btnChonThuMuc.Size = new System.Drawing.Size(84, 27);
            this.btnChonThuMuc.Text = "Chọn...";
            this.btnChonThuMuc.UseVisualStyleBackColor = true;
            this.btnChonThuMuc.Name = "btnChonThuMuc";
            this.btnChonThuMuc.TabIndex = 6;
            this.btnChonThuMuc.Click += new System.EventHandler(this.btnChonThuMuc_Click);
            // 
            // lblTenFile
            // 
            this.lblTenFile.AutoSize = true;
            this.lblTenFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenFile.Location = new System.Drawing.Point(15, 92);
            this.lblTenFile.Text = "Tên file:";
            this.lblTenFile.Name = "lblTenFile";
            this.lblTenFile.TabIndex = 7;
            // 
            // txtTenFile
            // 
            this.txtTenFile.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTenFile.Location = new System.Drawing.Point(78, 89);
            this.txtTenFile.Size = new System.Drawing.Size(222, 25);
            this.txtTenFile.Name = "txtTenFile";
            this.txtTenFile.TabIndex = 8;
            // 
            // btnSaoLuu
            // 
            this.btnSaoLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnSaoLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaoLuu.FlatAppearance.BorderSize = 0;
            this.btnSaoLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaoLuu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaoLuu.ForeColor = System.Drawing.Color.White;
            this.btnSaoLuu.Location = new System.Drawing.Point(310, 85);
            this.btnSaoLuu.Size = new System.Drawing.Size(145, 34);
            this.btnSaoLuu.Text = "💾 Sao lưu ngay";
            this.btnSaoLuu.UseVisualStyleBackColor = false;
            this.btnSaoLuu.Name = "btnSaoLuu";
            this.btnSaoLuu.TabIndex = 9;
            this.btnSaoLuu.Click += new System.EventHandler(this.btnSaoLuu_Click);
            // 
            // grpKhoiPhuc
            // 
            this.grpKhoiPhuc.Controls.Add(this.lblFileKP);
            this.grpKhoiPhuc.Controls.Add(this.txtFileKP);
            this.grpKhoiPhuc.Controls.Add(this.btnChonFile);
            this.grpKhoiPhuc.Controls.Add(this.lblCanhBao);
            this.grpKhoiPhuc.Controls.Add(this.btnKhoiPhuc);
            this.grpKhoiPhuc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpKhoiPhuc.Location = new System.Drawing.Point(505, 76);
            this.grpKhoiPhuc.Size = new System.Drawing.Size(470, 148);
            this.grpKhoiPhuc.Text = "② Khôi phục (RESTORE)";
            this.grpKhoiPhuc.Name = "grpKhoiPhuc";
            this.grpKhoiPhuc.TabIndex = 10;
            // 
            // lblFileKP
            // 
            this.lblFileKP.AutoSize = true;
            this.lblFileKP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileKP.Location = new System.Drawing.Point(15, 28);
            this.lblFileKP.Text = "File sao lưu (.bak):";
            this.lblFileKP.Name = "lblFileKP";
            this.lblFileKP.TabIndex = 11;
            // 
            // txtFileKP
            // 
            this.txtFileKP.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileKP.Location = new System.Drawing.Point(15, 50);
            this.txtFileKP.Size = new System.Drawing.Size(350, 25);
            this.txtFileKP.Name = "txtFileKP";
            this.txtFileKP.TabIndex = 12;
            // 
            // btnChonFile
            // 
            this.btnChonFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChonFile.Location = new System.Drawing.Point(371, 49);
            this.btnChonFile.Size = new System.Drawing.Size(84, 27);
            this.btnChonFile.Text = "Chọn...";
            this.btnChonFile.UseVisualStyleBackColor = true;
            this.btnChonFile.Name = "btnChonFile";
            this.btnChonFile.TabIndex = 13;
            this.btnChonFile.Click += new System.EventHandler(this.btnChonFile_Click);
            // 
            // lblCanhBao
            // 
            this.lblCanhBao.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCanhBao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblCanhBao.Location = new System.Drawing.Point(15, 84);
            this.lblCanhBao.Size = new System.Drawing.Size(290, 52);
            this.lblCanhBao.Text = "⚠ Khôi phục sẽ GHI ĐÈ toàn bộ dữ liệu hiện tại và tự đăng xuất mọi người.";
            this.lblCanhBao.Name = "lblCanhBao";
            this.lblCanhBao.TabIndex = 14;
            // 
            // btnKhoiPhuc
            // 
            this.btnKhoiPhuc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnKhoiPhuc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKhoiPhuc.FlatAppearance.BorderSize = 0;
            this.btnKhoiPhuc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKhoiPhuc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKhoiPhuc.ForeColor = System.Drawing.Color.White;
            this.btnKhoiPhuc.Location = new System.Drawing.Point(320, 85);
            this.btnKhoiPhuc.Size = new System.Drawing.Size(135, 34);
            this.btnKhoiPhuc.Text = "⟲ Khôi phục";
            this.btnKhoiPhuc.UseVisualStyleBackColor = false;
            this.btnKhoiPhuc.Name = "btnKhoiPhuc";
            this.btnKhoiPhuc.TabIndex = 15;
            this.btnKhoiPhuc.Click += new System.EventHandler(this.btnKhoiPhuc_Click);
            // 
            // pnlLuoi
            // 
            this.pnlLuoi.Controls.Add(this.dgvLichSu);
            this.pnlLuoi.Controls.Add(this.lblLichSu);
            this.pnlLuoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLuoi.Location = new System.Drawing.Point(0, 236);
            this.pnlLuoi.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlLuoi.Size = new System.Drawing.Size(1000, 364);
            this.pnlLuoi.Name = "pnlLuoi";
            this.pnlLuoi.TabIndex = 16;
            // 
            // dgvLichSu
            // 
            this.dgvLichSu.AllowUserToAddRows = false;
            this.dgvLichSu.AllowUserToDeleteRows = false;
            this.dgvLichSu.AllowUserToResizeRows = false;
            this.dgvLichSu.AlternatingRowsDefaultCellStyle = sLSXen;
            this.dgvLichSu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLichSu.BackgroundColor = System.Drawing.Color.White;
            this.dgvLichSu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLichSu.ColumnHeadersDefaultCellStyle = sLSTieuDe;
            this.dgvLichSu.DefaultCellStyle = sLSO;
            this.dgvLichSu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLichSu.Location = new System.Drawing.Point(0, 40);
            this.dgvLichSu.MultiSelect = false;
            this.dgvLichSu.ReadOnly = true;
            this.dgvLichSu.RowHeadersVisible = false;
            this.dgvLichSu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLichSu.Size = new System.Drawing.Size(1000, 324);
            this.dgvLichSu.Name = "dgvLichSu";
            this.dgvLichSu.TabIndex = 17;
            this.dgvLichSu.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLichSu_CellDoubleClick);
            this.dgvLichSu.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvLichSu_DataBindingComplete);
            // 
            // lblLichSu
            // 
            this.lblLichSu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLichSu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblLichSu.Location = new System.Drawing.Point(0, 10);
            this.lblLichSu.Size = new System.Drawing.Size(1000, 30);
            this.lblLichSu.Text = "Lịch sử sao lưu (nhấp đúp 1 dòng để chọn làm file khôi phục):";
            this.lblLichSu.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLichSu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLichSu.Name = "lblLichSu";
            this.lblLichSu.TabIndex = 18;
            // 
            // lblGhiChu
            // 
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGhiChu.ForeColor = System.Drawing.Color.DimGray;
            this.lblGhiChu.Location = new System.Drawing.Point(0, 600);
            this.lblGhiChu.Size = new System.Drawing.Size(1000, 40);
            this.lblGhiChu.Text = "";
            this.lblGhiChu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblGhiChu.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.TabIndex = 19;
            // 
            // frmSaoLuu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Text = "Sao lưu / Khôi phục CSDL";
            this.Controls.Add(this.pnlLuoi);
            this.Controls.Add(this.lblGhiChu);
            this.Controls.Add(this.pnlTren);
            this.Name = "frmSaoLuu";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichSu)).EndInit();
            this.pnlLuoi.ResumeLayout(false);
            this.pnlLuoi.PerformLayout();
            this.grpKhoiPhuc.ResumeLayout(false);
            this.grpKhoiPhuc.PerformLayout();
            this.grpSaoLuu.ResumeLayout(false);
            this.grpSaoLuu.PerformLayout();
            this.pnlTren.ResumeLayout(false);
            this.pnlTren.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTren;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblCSDL;
        private System.Windows.Forms.GroupBox grpSaoLuu;
        private System.Windows.Forms.Label lblThuMuc;
        private System.Windows.Forms.TextBox txtThuMuc;
        private System.Windows.Forms.Button btnChonThuMuc;
        private System.Windows.Forms.Label lblTenFile;
        private System.Windows.Forms.TextBox txtTenFile;
        private System.Windows.Forms.Button btnSaoLuu;
        private System.Windows.Forms.GroupBox grpKhoiPhuc;
        private System.Windows.Forms.Label lblFileKP;
        private System.Windows.Forms.TextBox txtFileKP;
        private System.Windows.Forms.Button btnChonFile;
        private System.Windows.Forms.Label lblCanhBao;
        private System.Windows.Forms.Button btnKhoiPhuc;
        private System.Windows.Forms.Panel pnlLuoi;
        private System.Windows.Forms.DataGridView dgvLichSu;
        private System.Windows.Forms.Label lblLichSu;
        private System.Windows.Forms.Label lblGhiChu;
    }
}
