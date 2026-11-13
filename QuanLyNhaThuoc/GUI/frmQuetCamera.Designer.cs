namespace QuanLyNhaThuoc.GUI
{
    partial class frmQuetCamera
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
            this.picCamera = new System.Windows.Forms.PictureBox();
            this.pnlDuoi = new System.Windows.Forms.Panel();
            this.lblKetQua = new System.Windows.Forms.Label();
            this.lblHuongDan = new System.Windows.Forms.Label();
            this.btnDong = new System.Windows.Forms.Button();
            this.pnlTren = new System.Windows.Forms.Panel();
            this.lblCamera = new System.Windows.Forms.Label();
            this.cboCamera = new System.Windows.Forms.ComboBox();
            this.chkLienTuc = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).BeginInit();
            this.pnlDuoi.SuspendLayout();
            this.pnlTren.SuspendLayout();
            this.SuspendLayout();
            // 
            // picCamera
            // 
            this.picCamera.BackColor = System.Drawing.Color.Black;
            this.picCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCamera.Location = new System.Drawing.Point(0, 50);
            this.picCamera.Size = new System.Drawing.Size(720, 440);
            this.picCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCamera.TabStop = false;
            this.picCamera.Name = "picCamera";
            this.picCamera.TabIndex = 2;
            // 
            // pnlDuoi
            // 
            this.pnlDuoi.Controls.Add(this.lblKetQua);
            this.pnlDuoi.Controls.Add(this.lblHuongDan);
            this.pnlDuoi.Controls.Add(this.btnDong);
            this.pnlDuoi.BackColor = System.Drawing.Color.White;
            this.pnlDuoi.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDuoi.Location = new System.Drawing.Point(0, 490);
            this.pnlDuoi.Size = new System.Drawing.Size(720, 80);
            this.pnlDuoi.Name = "pnlDuoi";
            this.pnlDuoi.TabIndex = 1;
            // 
            // lblKetQua
            // 
            this.lblKetQua.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKetQua.Location = new System.Drawing.Point(12, 8);
            this.lblKetQua.Text = "";
            this.lblKetQua.Size = new System.Drawing.Size(580, 26);
            this.lblKetQua.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblKetQua.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right));
            this.lblKetQua.AutoEllipsis = true;
            this.lblKetQua.Name = "lblKetQua";
            this.lblKetQua.TabIndex = 0;
            // 
            // lblHuongDan
            // 
            this.lblHuongDan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHuongDan.Location = new System.Drawing.Point(12, 40);
            this.lblHuongDan.Text = "Đưa mã vạch / mã QR trên hộp thuốc vào giữa khung hình, cách camera 10 - 20 cm, giữ yên 1 - 2 giây.";
            this.lblHuongDan.Size = new System.Drawing.Size(580, 34);
            this.lblHuongDan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblHuongDan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right));
            this.lblHuongDan.Name = "lblHuongDan";
            this.lblHuongDan.TabIndex = 1;
            // 
            // btnDong
            // 
            this.btnDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnDong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDong.ForeColor = System.Drawing.Color.White;
            this.btnDong.Location = new System.Drawing.Point(606, 20);
            this.btnDong.Size = new System.Drawing.Size(100, 38);
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.FlatAppearance.BorderSize = 0;
            this.btnDong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Right));
            this.btnDong.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDong.Name = "btnDong";
            this.btnDong.TabIndex = 2;
            // 
            // pnlTren
            // 
            this.pnlTren.Controls.Add(this.lblCamera);
            this.pnlTren.Controls.Add(this.cboCamera);
            this.pnlTren.Controls.Add(this.chkLienTuc);
            this.pnlTren.BackColor = System.Drawing.Color.White;
            this.pnlTren.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTren.Location = new System.Drawing.Point(0, 0);
            this.pnlTren.Size = new System.Drawing.Size(720, 50);
            this.pnlTren.Name = "pnlTren";
            this.pnlTren.TabIndex = 0;
            // 
            // lblCamera
            // 
            this.lblCamera.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCamera.Location = new System.Drawing.Point(12, 15);
            this.lblCamera.Text = "Camera:";
            this.lblCamera.AutoSize = true;
            this.lblCamera.Name = "lblCamera";
            this.lblCamera.TabIndex = 0;
            // 
            // cboCamera
            // 
            this.cboCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCamera.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCamera.FormattingEnabled = true;
            this.cboCamera.Location = new System.Drawing.Point(75, 12);
            this.cboCamera.Size = new System.Drawing.Size(320, 25);
            this.cboCamera.Name = "cboCamera";
            this.cboCamera.TabIndex = 1;
            this.cboCamera.SelectedIndexChanged += new System.EventHandler(this.cboCamera_SelectedIndexChanged);
            // 
            // chkLienTuc
            // 
            this.chkLienTuc.AutoSize = true;
            this.chkLienTuc.Checked = true;
            this.chkLienTuc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLienTuc.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLienTuc.Location = new System.Drawing.Point(410, 14);
            this.chkLienTuc.Text = "Quét liên tục (thêm nhiều thuốc)";
            this.chkLienTuc.Name = "chkLienTuc";
            this.chkLienTuc.TabIndex = 2;
            // 
            // frmQuetCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(720, 570);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text = "Quét mã vạch bằng webcam";
            this.CancelButton = this.btnDong;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MinimumSize = new System.Drawing.Size(560, 460);
            this.Load += new System.EventHandler(this.frmQuetCamera_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmQuetCamera_FormClosing);
            this.Controls.Add(this.picCamera);
            this.Controls.Add(this.pnlDuoi);
            this.Controls.Add(this.pnlTren);
            this.Name = "frmQuetCamera";
            this.pnlTren.ResumeLayout(false);
            this.pnlTren.PerformLayout();
            this.pnlDuoi.ResumeLayout(false);
            this.pnlDuoi.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picCamera;
        private System.Windows.Forms.Panel pnlDuoi;
        private System.Windows.Forms.Label lblKetQua;
        private System.Windows.Forms.Label lblHuongDan;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Panel pnlTren;
        private System.Windows.Forms.Label lblCamera;
        private System.Windows.Forms.ComboBox cboCamera;
        private System.Windows.Forms.CheckBox chkLienTuc;
    }
}
