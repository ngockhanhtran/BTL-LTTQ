namespace QuanLyNhaThuoc.GUI
{
    partial class frmCauHinhKetNoi
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
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblLoi = new System.Windows.Forms.Label();
            this.lblMayChu = new System.Windows.Forms.Label();
            this.cboMayChu = new System.Windows.Forms.ComboBox();
            this.lblGoiYMayChu = new System.Windows.Forms.Label();
            this.lblCSDL = new System.Windows.Forms.Label();
            this.txtCSDL = new System.Windows.Forms.TextBox();
            this.grpXacThuc = new System.Windows.Forms.GroupBox();
            this.rdoWindows = new System.Windows.Forms.RadioButton();
            this.rdoSql = new System.Windows.Forms.RadioButton();
            this.lblTaiKhoan = new System.Windows.Forms.Label();
            this.txtTaiKhoan = new System.Windows.Forms.TextBox();
            this.lblMatKhauSql = new System.Windows.Forms.Label();
            this.txtMatKhauSql = new System.Windows.Forms.TextBox();
            this.btnThu = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.lblKetQua = new System.Windows.Forms.Label();
            this.lblFile = new System.Windows.Forms.Label();
            this.grpXacThuc.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTieuDe.Location = new System.Drawing.Point(20, 15);
            this.lblTieuDe.Text = "Cấu hình kết nối SQL Server";
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 0;
            // 
            // lblLoi
            // 
            this.lblLoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblLoi.Location = new System.Drawing.Point(20, 50);
            this.lblLoi.Size = new System.Drawing.Size(440, 58);
            this.lblLoi.Text = "";
            this.lblLoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.lblLoi.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.lblLoi.Visible = false;
            this.lblLoi.Name = "lblLoi";
            this.lblLoi.TabIndex = 1;
            // 
            // lblMayChu
            // 
            this.lblMayChu.AutoSize = true;
            this.lblMayChu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMayChu.Location = new System.Drawing.Point(20, 116);
            this.lblMayChu.Text = "Máy chủ (Server name trong SSMS):";
            this.lblMayChu.Name = "lblMayChu";
            this.lblMayChu.TabIndex = 2;
            // 
            // cboMayChu
            // 
            this.cboMayChu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboMayChu.Location = new System.Drawing.Point(20, 139);
            this.cboMayChu.Size = new System.Drawing.Size(440, 25);
            this.cboMayChu.Name = "cboMayChu";
            this.cboMayChu.TabIndex = 3;
            // 
            // lblGoiYMayChu
            // 
            this.lblGoiYMayChu.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGoiYMayChu.ForeColor = System.Drawing.Color.Gray;
            this.lblGoiYMayChu.Location = new System.Drawing.Point(20, 167);
            this.lblGoiYMayChu.Size = new System.Drawing.Size(440, 18);
            this.lblGoiYMayChu.Text = "Ví dụ: .\\SQLEXPRESS  |  .  |  (localdb)\\MSSQLLocalDB  |  TEN-MAY\\SQLEXPRESS";
            this.lblGoiYMayChu.Name = "lblGoiYMayChu";
            this.lblGoiYMayChu.TabIndex = 4;
            // 
            // lblCSDL
            // 
            this.lblCSDL.AutoSize = true;
            this.lblCSDL.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCSDL.Location = new System.Drawing.Point(20, 194);
            this.lblCSDL.Text = "Tên CSDL:";
            this.lblCSDL.Name = "lblCSDL";
            this.lblCSDL.TabIndex = 5;
            // 
            // txtCSDL
            // 
            this.txtCSDL.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCSDL.Location = new System.Drawing.Point(20, 217);
            this.txtCSDL.MaxLength = 128;
            this.txtCSDL.Size = new System.Drawing.Size(220, 25);
            this.txtCSDL.Name = "txtCSDL";
            this.txtCSDL.TabIndex = 6;
            // 
            // grpXacThuc
            // 
            this.grpXacThuc.Controls.Add(this.rdoWindows);
            this.grpXacThuc.Controls.Add(this.rdoSql);
            this.grpXacThuc.Controls.Add(this.lblTaiKhoan);
            this.grpXacThuc.Controls.Add(this.txtTaiKhoan);
            this.grpXacThuc.Controls.Add(this.lblMatKhauSql);
            this.grpXacThuc.Controls.Add(this.txtMatKhauSql);
            this.grpXacThuc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpXacThuc.Location = new System.Drawing.Point(20, 252);
            this.grpXacThuc.Size = new System.Drawing.Size(440, 96);
            this.grpXacThuc.TabStop = false;
            this.grpXacThuc.Text = "Đăng nhập SQL Server";
            this.grpXacThuc.Name = "grpXacThuc";
            this.grpXacThuc.TabIndex = 7;
            // 
            // rdoWindows
            // 
            this.rdoWindows.AutoSize = true;
            this.rdoWindows.Checked = true;
            this.rdoWindows.Location = new System.Drawing.Point(15, 26);
            this.rdoWindows.Text = "Windows Authentication";
            this.rdoWindows.UseVisualStyleBackColor = true;
            this.rdoWindows.Name = "rdoWindows";
            this.rdoWindows.TabIndex = 8;
            this.rdoWindows.CheckedChanged += new System.EventHandler(this.rdoWindows_CheckedChanged);
            // 
            // rdoSql
            // 
            this.rdoSql.AutoSize = true;
            this.rdoSql.Location = new System.Drawing.Point(225, 26);
            this.rdoSql.Text = "SQL Server Authentication";
            this.rdoSql.UseVisualStyleBackColor = true;
            this.rdoSql.Name = "rdoSql";
            this.rdoSql.TabIndex = 9;
            // 
            // lblTaiKhoan
            // 
            this.lblTaiKhoan.AutoSize = true;
            this.lblTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaiKhoan.Location = new System.Drawing.Point(15, 61);
            this.lblTaiKhoan.Text = "Tài khoản:";
            this.lblTaiKhoan.Name = "lblTaiKhoan";
            this.lblTaiKhoan.TabIndex = 10;
            // 
            // txtTaiKhoan
            // 
            this.txtTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTaiKhoan.Location = new System.Drawing.Point(90, 58);
            this.txtTaiKhoan.MaxLength = 128;
            this.txtTaiKhoan.Size = new System.Drawing.Size(110, 25);
            this.txtTaiKhoan.Name = "txtTaiKhoan";
            this.txtTaiKhoan.TabIndex = 11;
            // 
            // lblMatKhauSql
            // 
            this.lblMatKhauSql.AutoSize = true;
            this.lblMatKhauSql.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatKhauSql.Location = new System.Drawing.Point(215, 61);
            this.lblMatKhauSql.Text = "Mật khẩu:";
            this.lblMatKhauSql.Name = "lblMatKhauSql";
            this.lblMatKhauSql.TabIndex = 12;
            // 
            // txtMatKhauSql
            // 
            this.txtMatKhauSql.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMatKhauSql.Location = new System.Drawing.Point(285, 58);
            this.txtMatKhauSql.MaxLength = 128;
            this.txtMatKhauSql.Size = new System.Drawing.Size(140, 25);
            this.txtMatKhauSql.UseSystemPasswordChar = true;
            this.txtMatKhauSql.Name = "txtMatKhauSql";
            this.txtMatKhauSql.TabIndex = 13;
            // 
            // btnThu
            // 
            this.btnThu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnThu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThu.FlatAppearance.BorderSize = 0;
            this.btnThu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThu.ForeColor = System.Drawing.Color.White;
            this.btnThu.Location = new System.Drawing.Point(20, 362);
            this.btnThu.Size = new System.Drawing.Size(150, 38);
            this.btnThu.Text = "🔌 Thử kết nối";
            this.btnThu.UseVisualStyleBackColor = false;
            this.btnThu.Name = "btnThu";
            this.btnThu.TabIndex = 14;
            this.btnThu.Click += new System.EventHandler(this.btnThu_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(230, 362);
            this.btnLuu.Size = new System.Drawing.Size(120, 38);
            this.btnLuu.Text = "💾 Lưu && dùng";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.TabIndex = 15;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(360, 362);
            this.btnHuy.Size = new System.Drawing.Size(100, 38);
            this.btnHuy.Text = "Đóng";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.TabIndex = 16;
            // 
            // lblKetQua
            // 
            this.lblKetQua.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKetQua.Location = new System.Drawing.Point(20, 408);
            this.lblKetQua.Size = new System.Drawing.Size(440, 22);
            this.lblKetQua.Text = "";
            this.lblKetQua.Name = "lblKetQua";
            this.lblKetQua.TabIndex = 17;
            // 
            // lblFile
            // 
            this.lblFile.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFile.ForeColor = System.Drawing.Color.Gray;
            this.lblFile.Location = new System.Drawing.Point(20, 432);
            this.lblFile.Size = new System.Drawing.Size(440, 32);
            this.lblFile.Text = "";
            this.lblFile.Name = "lblFile";
            this.lblFile.TabIndex = 18;
            // 
            // frmCauHinhKetNoi
            // 
            this.AcceptButton = this.btnLuu;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(480, 470);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cấu hình kết nối CSDL";
            this.Controls.Add(this.lblTieuDe);
            this.Controls.Add(this.lblLoi);
            this.Controls.Add(this.lblMayChu);
            this.Controls.Add(this.cboMayChu);
            this.Controls.Add(this.lblGoiYMayChu);
            this.Controls.Add(this.lblCSDL);
            this.Controls.Add(this.txtCSDL);
            this.Controls.Add(this.grpXacThuc);
            this.Controls.Add(this.btnThu);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.lblKetQua);
            this.Controls.Add(this.lblFile);
            this.Name = "frmCauHinhKetNoi";
            this.grpXacThuc.ResumeLayout(false);
            this.grpXacThuc.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblLoi;
        private System.Windows.Forms.Label lblMayChu;
        private System.Windows.Forms.ComboBox cboMayChu;
        private System.Windows.Forms.Label lblGoiYMayChu;
        private System.Windows.Forms.Label lblCSDL;
        private System.Windows.Forms.TextBox txtCSDL;
        private System.Windows.Forms.GroupBox grpXacThuc;
        private System.Windows.Forms.RadioButton rdoWindows;
        private System.Windows.Forms.RadioButton rdoSql;
        private System.Windows.Forms.Label lblTaiKhoan;
        private System.Windows.Forms.TextBox txtTaiKhoan;
        private System.Windows.Forms.Label lblMatKhauSql;
        private System.Windows.Forms.TextBox txtMatKhauSql;
        private System.Windows.Forms.Button btnThu;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Label lblKetQua;
        private System.Windows.Forms.Label lblFile;
    }
}
