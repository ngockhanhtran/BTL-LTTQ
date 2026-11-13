namespace QuanLyNhaThuoc.GUI
{
    partial class frmThanhToan
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
            this.lblHoaDon = new System.Windows.Forms.Label();
            this.lblSoTien = new System.Windows.Forms.Label();
            this.picQR = new System.Windows.Forms.PictureBox();
            this.lblTrangThaiQR = new System.Windows.Forms.Label();
            this.lblTaiKhoan = new System.Windows.Forms.Label();
            this.lblNoiDung = new System.Windows.Forms.Label();
            this.btnDaThanhToan = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.ForeColor = System.Drawing.Color.Black;
            this.lblTieuDe.Location = new System.Drawing.Point(0, 15);
            this.lblTieuDe.Size = new System.Drawing.Size(420, 30);
            this.lblTieuDe.Text = "QUÉT MÃ QR ĐỂ THANH TOÁN";
            this.lblTieuDe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 0;
            // 
            // lblHoaDon
            // 
            this.lblHoaDon.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoaDon.ForeColor = System.Drawing.Color.DimGray;
            this.lblHoaDon.Location = new System.Drawing.Point(0, 48);
            this.lblHoaDon.Size = new System.Drawing.Size(420, 24);
            this.lblHoaDon.Text = "Hóa đơn: ";
            this.lblHoaDon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHoaDon.Name = "lblHoaDon";
            this.lblHoaDon.TabIndex = 1;
            // 
            // lblSoTien
            // 
            this.lblSoTien.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblSoTien.Location = new System.Drawing.Point(0, 74);
            this.lblSoTien.Size = new System.Drawing.Size(420, 38);
            this.lblSoTien.Text = "0 đ";
            this.lblSoTien.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSoTien.Name = "lblSoTien";
            this.lblSoTien.TabIndex = 2;
            // 
            // picQR
            // 
            this.picQR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picQR.Location = new System.Drawing.Point(50, 120);
            this.picQR.Size = new System.Drawing.Size(320, 320);
            this.picQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picQR.TabStop = false;
            this.picQR.Name = "picQR";
            this.picQR.TabIndex = 3;
            this.picQR.LoadCompleted += new System.ComponentModel.AsyncCompletedEventHandler(this.picQR_LoadCompleted);
            // 
            // lblTrangThaiQR
            // 
            this.lblTrangThaiQR.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrangThaiQR.ForeColor = System.Drawing.Color.Gray;
            this.lblTrangThaiQR.Location = new System.Drawing.Point(0, 445);
            this.lblTrangThaiQR.Size = new System.Drawing.Size(420, 22);
            this.lblTrangThaiQR.Text = "Đang tải mã QR...";
            this.lblTrangThaiQR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTrangThaiQR.Name = "lblTrangThaiQR";
            this.lblTrangThaiQR.TabIndex = 4;
            // 
            // lblTaiKhoan
            // 
            this.lblTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaiKhoan.ForeColor = System.Drawing.Color.Black;
            this.lblTaiKhoan.Location = new System.Drawing.Point(0, 468);
            this.lblTaiKhoan.Size = new System.Drawing.Size(420, 22);
            this.lblTaiKhoan.Text = "";
            this.lblTaiKhoan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTaiKhoan.Name = "lblTaiKhoan";
            this.lblTaiKhoan.TabIndex = 5;
            // 
            // lblNoiDung
            // 
            this.lblNoiDung.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoiDung.ForeColor = System.Drawing.Color.DimGray;
            this.lblNoiDung.Location = new System.Drawing.Point(0, 490);
            this.lblNoiDung.Size = new System.Drawing.Size(420, 22);
            this.lblNoiDung.Text = "Nội dung CK: Thanh toan ";
            this.lblNoiDung.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNoiDung.Name = "lblNoiDung";
            this.lblNoiDung.TabIndex = 6;
            // 
            // btnDaThanhToan
            // 
            this.btnDaThanhToan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnDaThanhToan.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDaThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDaThanhToan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDaThanhToan.ForeColor = System.Drawing.Color.White;
            this.btnDaThanhToan.Location = new System.Drawing.Point(30, 525);
            this.btnDaThanhToan.Size = new System.Drawing.Size(220, 42);
            this.btnDaThanhToan.Text = "Đã thanh toán";
            this.btnDaThanhToan.UseVisualStyleBackColor = false;
            this.btnDaThanhToan.Name = "btnDaThanhToan";
            this.btnDaThanhToan.TabIndex = 7;
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(260, 525);
            this.btnHuy.Size = new System.Drawing.Size(130, 42);
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.TabIndex = 8;
            // 
            // frmThanhToan
            // 
            this.AcceptButton = this.btnDaThanhToan;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(420, 585);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thanh toán chuyển khoản";
            this.Controls.Add(this.lblTieuDe);
            this.Controls.Add(this.lblHoaDon);
            this.Controls.Add(this.lblSoTien);
            this.Controls.Add(this.picQR);
            this.Controls.Add(this.lblTrangThaiQR);
            this.Controls.Add(this.lblTaiKhoan);
            this.Controls.Add(this.lblNoiDung);
            this.Controls.Add(this.btnDaThanhToan);
            this.Controls.Add(this.btnHuy);
            this.Name = "frmThanhToan";
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblHoaDon;
        private System.Windows.Forms.Label lblSoTien;
        private System.Windows.Forms.PictureBox picQR;
        private System.Windows.Forms.Label lblTrangThaiQR;
        private System.Windows.Forms.Label lblTaiKhoan;
        private System.Windows.Forms.Label lblNoiDung;
        private System.Windows.Forms.Button btnDaThanhToan;
        private System.Windows.Forms.Button btnHuy;
    }
}
