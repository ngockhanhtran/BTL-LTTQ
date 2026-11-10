namespace QuanLyNhaThuoc.GUI
{
    partial class frmQuenMatKhau
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
            this.lblIcon = new System.Windows.Forms.Label();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.lblMa = new System.Windows.Forms.Label();
            this.txtMaNV = new System.Windows.Forms.TextBox();
            this.lblSdt = new System.Windows.Forms.Label();
            this.txtDienThoai = new System.Windows.Forms.TextBox();
            this.lblLoi = new System.Windows.Forms.Label();
            this.btnGui = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblIcon
            // 
            this.lblIcon.AutoSize = true;
            this.lblIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIcon.Location = new System.Drawing.Point(25, 18);
            this.lblIcon.Text = "🔑";
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.TabIndex = 0;
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTieuDe.Location = new System.Drawing.Point(75, 24);
            this.lblTieuDe.Text = "Quên mật khẩu";
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 1;
            // 
            // lblMoTa
            // 
            this.lblMoTa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoTa.ForeColor = System.Drawing.Color.DimGray;
            this.lblMoTa.Location = new System.Drawing.Point(27, 72);
            this.lblMoTa.Size = new System.Drawing.Size(390, 42);
            this.lblMoTa.Text = "Nhập mã nhân viên và số điện thoại đã đăng ký. Yêu cầu sẽ được gửi tới quản lý; quản lý sẽ đặt lại mật khẩu mới cho bạn.";
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.TabIndex = 2;
            // 
            // lblMa
            // 
            this.lblMa.AutoSize = true;
            this.lblMa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMa.Location = new System.Drawing.Point(27, 125);
            this.lblMa.Text = "Mã nhân viên";
            this.lblMa.Name = "lblMa";
            this.lblMa.TabIndex = 3;
            // 
            // txtMaNV
            // 
            this.txtMaNV.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaNV.Location = new System.Drawing.Point(27, 148);
            this.txtMaNV.MaxLength = 10;
            this.txtMaNV.Size = new System.Drawing.Size(385, 27);
            this.txtMaNV.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMaNV.Name = "txtMaNV";
            this.txtMaNV.TabIndex = 4;
            // 
            // lblSdt
            // 
            this.lblSdt.AutoSize = true;
            this.lblSdt.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSdt.Location = new System.Drawing.Point(27, 185);
            this.lblSdt.Text = "Số điện thoại đã đăng ký";
            this.lblSdt.Name = "lblSdt";
            this.lblSdt.TabIndex = 5;
            // 
            // txtDienThoai
            // 
            this.txtDienThoai.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDienThoai.Location = new System.Drawing.Point(27, 208);
            this.txtDienThoai.MaxLength = 20;
            this.txtDienThoai.Size = new System.Drawing.Size(385, 27);
            this.txtDienThoai.Name = "txtDienThoai";
            this.txtDienThoai.TabIndex = 6;
            // 
            // lblLoi
            // 
            this.lblLoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblLoi.Location = new System.Drawing.Point(27, 242);
            this.lblLoi.Size = new System.Drawing.Size(390, 20);
            this.lblLoi.Text = "";
            this.lblLoi.Name = "lblLoi";
            this.lblLoi.TabIndex = 7;
            // 
            // btnGui
            // 
            this.btnGui.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnGui.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGui.FlatAppearance.BorderSize = 0;
            this.btnGui.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGui.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGui.ForeColor = System.Drawing.Color.White;
            this.btnGui.Location = new System.Drawing.Point(27, 272);
            this.btnGui.Size = new System.Drawing.Size(180, 40);
            this.btnGui.Text = "Gửi yêu cầu";
            this.btnGui.UseVisualStyleBackColor = false;
            this.btnGui.Name = "btnGui";
            this.btnGui.TabIndex = 8;
            this.btnGui.Click += new System.EventHandler(this.btnGui_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(292, 272);
            this.btnHuy.Size = new System.Drawing.Size(120, 40);
            this.btnHuy.Text = "Đóng";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.TabIndex = 9;
            // 
            // frmQuenMatKhau
            // 
            this.AcceptButton = this.btnGui;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(440, 330);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quên mật khẩu";
            this.Controls.Add(this.lblIcon);
            this.Controls.Add(this.lblTieuDe);
            this.Controls.Add(this.lblMoTa);
            this.Controls.Add(this.lblMa);
            this.Controls.Add(this.txtMaNV);
            this.Controls.Add(this.lblSdt);
            this.Controls.Add(this.txtDienThoai);
            this.Controls.Add(this.lblLoi);
            this.Controls.Add(this.btnGui);
            this.Controls.Add(this.btnHuy);
            this.Name = "frmQuenMatKhau";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.Label lblMa;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.Label lblSdt;
        private System.Windows.Forms.TextBox txtDienThoai;
        private System.Windows.Forms.Label lblLoi;
        private System.Windows.Forms.Button btnGui;
        private System.Windows.Forms.Button btnHuy;
    }
}
