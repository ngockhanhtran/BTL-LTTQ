namespace QuanLyNhaThuoc.GUI
{
    partial class frmChonCongDung
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
            this.lblThuoc = new System.Windows.Forms.Label();
            this.lblLoc = new System.Windows.Forms.Label();
            this.txtLoc = new System.Windows.Forms.TextBox();
            this.clbCongDung = new System.Windows.Forms.CheckedListBox();
            this.lblDem = new System.Windows.Forms.Label();
            this.lblMoi = new System.Windows.Forms.Label();
            this.txtMoi = new System.Windows.Forms.TextBox();
            this.btnThemMoi = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(20, 15);
            this.lblTieuDe.Text = "Công dụng của thuốc";
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 0;
            // 
            // lblThuoc
            // 
            this.lblThuoc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblThuoc.Location = new System.Drawing.Point(20, 45);
            this.lblThuoc.Size = new System.Drawing.Size(400, 20);
            this.lblThuoc.Text = "";
            this.lblThuoc.AutoEllipsis = true;
            this.lblThuoc.Name = "lblThuoc";
            this.lblThuoc.TabIndex = 1;
            // 
            // lblLoc
            // 
            this.lblLoc.AutoSize = true;
            this.lblLoc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoc.Location = new System.Drawing.Point(20, 78);
            this.lblLoc.Text = "Lọc:";
            this.lblLoc.Name = "lblLoc";
            this.lblLoc.TabIndex = 2;
            // 
            // txtLoc
            // 
            this.txtLoc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoc.Location = new System.Drawing.Point(60, 75);
            this.txtLoc.Size = new System.Drawing.Size(360, 25);
            this.txtLoc.Name = "txtLoc";
            this.txtLoc.TabIndex = 3;
            this.txtLoc.TextChanged += new System.EventHandler(this.txtLoc_TextChanged);
            // 
            // clbCongDung
            // 
            this.clbCongDung.CheckOnClick = true;
            this.clbCongDung.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clbCongDung.FormattingEnabled = true;
            this.clbCongDung.IntegralHeight = false;
            this.clbCongDung.Location = new System.Drawing.Point(20, 108);
            this.clbCongDung.Size = new System.Drawing.Size(400, 300);
            this.clbCongDung.Name = "clbCongDung";
            this.clbCongDung.TabIndex = 4;
            this.clbCongDung.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbCongDung_ItemCheck);
            // 
            // lblDem
            // 
            this.lblDem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDem.ForeColor = System.Drawing.Color.Gray;
            this.lblDem.Location = new System.Drawing.Point(20, 412);
            this.lblDem.Size = new System.Drawing.Size(400, 20);
            this.lblDem.Text = "";
            this.lblDem.Name = "lblDem";
            this.lblDem.TabIndex = 5;
            // 
            // lblMoi
            // 
            this.lblMoi.AutoSize = true;
            this.lblMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoi.Location = new System.Drawing.Point(20, 440);
            this.lblMoi.Text = "Thêm công dụng mới:";
            this.lblMoi.Name = "lblMoi";
            this.lblMoi.TabIndex = 6;
            // 
            // txtMoi
            // 
            this.txtMoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMoi.Location = new System.Drawing.Point(155, 437);
            this.txtMoi.MaxLength = 100;
            this.txtMoi.Size = new System.Drawing.Size(175, 25);
            this.txtMoi.Name = "txtMoi";
            this.txtMoi.TabIndex = 7;
            this.txtMoi.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMoi_KeyDown);
            // 
            // btnThemMoi
            // 
            this.btnThemMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnThemMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemMoi.FlatAppearance.BorderSize = 0;
            this.btnThemMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemMoi.ForeColor = System.Drawing.Color.White;
            this.btnThemMoi.Location = new System.Drawing.Point(335, 436);
            this.btnThemMoi.Size = new System.Drawing.Size(85, 27);
            this.btnThemMoi.Text = "+ Thêm";
            this.btnThemMoi.UseVisualStyleBackColor = false;
            this.btnThemMoi.Name = "btnThemMoi";
            this.btnThemMoi.TabIndex = 8;
            this.btnThemMoi.Click += new System.EventHandler(this.btnThemMoi_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(20, 485);
            this.btnOK.Size = new System.Drawing.Size(150, 38);
            this.btnOK.Text = "Đồng ý";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 9;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(310, 485);
            this.btnHuy.Size = new System.Drawing.Size(110, 38);
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.TabIndex = 10;
            // 
            // frmChonCongDung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(440, 540);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chọn công dụng";
            this.Controls.Add(this.lblTieuDe);
            this.Controls.Add(this.lblThuoc);
            this.Controls.Add(this.lblLoc);
            this.Controls.Add(this.txtLoc);
            this.Controls.Add(this.clbCongDung);
            this.Controls.Add(this.lblDem);
            this.Controls.Add(this.lblMoi);
            this.Controls.Add(this.txtMoi);
            this.Controls.Add(this.btnThemMoi);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnHuy);
            this.Name = "frmChonCongDung";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblThuoc;
        private System.Windows.Forms.Label lblLoc;
        private System.Windows.Forms.TextBox txtLoc;
        private System.Windows.Forms.CheckedListBox clbCongDung;
        private System.Windows.Forms.Label lblDem;
        private System.Windows.Forms.Label lblMoi;
        private System.Windows.Forms.TextBox txtMoi;
        private System.Windows.Forms.Button btnThemMoi;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnHuy;
    }
}
