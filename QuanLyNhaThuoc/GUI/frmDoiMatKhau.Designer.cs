namespace QuanLyNhaThuoc.GUI
{
    partial class frmDoiMatKhau
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
            this.lblBatBuoc = new System.Windows.Forms.Label();
            this.lblO0 = new System.Windows.Forms.Label();
            this.txtCu = new System.Windows.Forms.TextBox();
            this.lblO1 = new System.Windows.Forms.Label();
            this.txtMoi = new System.Windows.Forms.TextBox();
            this.lblO2 = new System.Windows.Forms.Label();
            this.txtNhapLai = new System.Windows.Forms.TextBox();
            this.lblLoi = new System.Windows.Forms.Label();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(25, 15);
            this.lblTieuDe.Text = "Đổi mật khẩu";
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 0;
            // 
            // lblBatBuoc
            // 
            this.lblBatBuoc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBatBuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(90)))), ((int)(((byte)(0)))));
            this.lblBatBuoc.Location = new System.Drawing.Point(25, 48);
            this.lblBatBuoc.Size = new System.Drawing.Size(350, 34);
            this.lblBatBuoc.Text = "";
            this.lblBatBuoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(243)))), ((int)(((byte)(205)))));
            this.lblBatBuoc.Visible = false;
            this.lblBatBuoc.Name = "lblBatBuoc";
            this.lblBatBuoc.TabIndex = 1;
            // 
            // lblO0
            // 
            this.lblO0.AutoSize = true;
            this.lblO0.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblO0.Location = new System.Drawing.Point(25, 88);
            this.lblO0.Text = "Mật khẩu hiện tại";
            this.lblO0.Name = "lblO0";
            this.lblO0.TabIndex = 2;
            // 
            // txtCu
            // 
            this.txtCu.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCu.Location = new System.Drawing.Point(25, 110);
            this.txtCu.MaxLength = 50;
            this.txtCu.Size = new System.Drawing.Size(350, 27);
            this.txtCu.UseSystemPasswordChar = true;
            this.txtCu.Name = "txtCu";
            this.txtCu.TabIndex = 3;
            this.txtCu.TextChanged += new System.EventHandler(this.oMatKhau_TextChanged);
            // 
            // lblO1
            // 
            this.lblO1.AutoSize = true;
            this.lblO1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblO1.Location = new System.Drawing.Point(25, 148);
            this.lblO1.Text = "Mật khẩu mới";
            this.lblO1.Name = "lblO1";
            this.lblO1.TabIndex = 4;
            // 
            // txtMoi
            // 
            this.txtMoi.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMoi.Location = new System.Drawing.Point(25, 170);
            this.txtMoi.MaxLength = 50;
            this.txtMoi.Size = new System.Drawing.Size(350, 27);
            this.txtMoi.UseSystemPasswordChar = true;
            this.txtMoi.Name = "txtMoi";
            this.txtMoi.TabIndex = 5;
            this.txtMoi.TextChanged += new System.EventHandler(this.oMatKhau_TextChanged);
            // 
            // lblO2
            // 
            this.lblO2.AutoSize = true;
            this.lblO2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblO2.Location = new System.Drawing.Point(25, 208);
            this.lblO2.Text = "Nhập lại mật khẩu mới";
            this.lblO2.Name = "lblO2";
            this.lblO2.TabIndex = 6;
            // 
            // txtNhapLai
            // 
            this.txtNhapLai.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNhapLai.Location = new System.Drawing.Point(25, 230);
            this.txtNhapLai.MaxLength = 50;
            this.txtNhapLai.Size = new System.Drawing.Size(350, 27);
            this.txtNhapLai.UseSystemPasswordChar = true;
            this.txtNhapLai.Name = "txtNhapLai";
            this.txtNhapLai.TabIndex = 7;
            this.txtNhapLai.TextChanged += new System.EventHandler(this.oMatKhau_TextChanged);
            // 
            // lblLoi
            // 
            this.lblLoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblLoi.Location = new System.Drawing.Point(25, 268);
            this.lblLoi.Size = new System.Drawing.Size(350, 20);
            this.lblLoi.Text = "";
            this.lblLoi.Name = "lblLoi";
            this.lblLoi.TabIndex = 8;
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(25, 294);
            this.btnLuu.Size = new System.Drawing.Size(170, 38);
            this.btnLuu.Text = "Lưu mật khẩu";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.TabIndex = 9;
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
            this.btnHuy.Location = new System.Drawing.Point(265, 294);
            this.btnHuy.Size = new System.Drawing.Size(110, 38);
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.TabIndex = 10;
            // 
            // frmDoiMatKhau
            // 
            this.AcceptButton = this.btnLuu;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đổi mật khẩu";
            this.Controls.Add(this.lblTieuDe);
            this.Controls.Add(this.lblBatBuoc);
            this.Controls.Add(this.lblO0);
            this.Controls.Add(this.txtCu);
            this.Controls.Add(this.lblO1);
            this.Controls.Add(this.txtMoi);
            this.Controls.Add(this.lblO2);
            this.Controls.Add(this.txtNhapLai);
            this.Controls.Add(this.lblLoi);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnHuy);
            this.Name = "frmDoiMatKhau";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblBatBuoc;
        private System.Windows.Forms.Label lblO0;
        private System.Windows.Forms.TextBox txtCu;
        private System.Windows.Forms.Label lblO1;
        private System.Windows.Forms.TextBox txtMoi;
        private System.Windows.Forms.Label lblO2;
        private System.Windows.Forms.TextBox txtNhapLai;
        private System.Windows.Forms.Label lblLoi;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}
