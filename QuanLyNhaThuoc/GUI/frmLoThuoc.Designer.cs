namespace QuanLyNhaThuoc.GUI
{
    partial class frmLoThuoc
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
            System.Windows.Forms.DataGridViewCellStyle sLoXen = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle sLoTieuDe = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle sLoO = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTren = new System.Windows.Forms.Panel();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblTomTat = new System.Windows.Forms.Label();
            this.chkConHang = new System.Windows.Forms.CheckBox();
            this.dgvLo = new System.Windows.Forms.DataGridView();
            this.lblChuThich = new System.Windows.Forms.Label();
            this.pnlTren.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTren
            // 
            sLoXen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            sLoTieuDe.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            sLoTieuDe.BackColor = System.Drawing.SystemColors.Control;
            sLoTieuDe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sLoTieuDe.ForeColor = System.Drawing.SystemColors.WindowText;
            sLoTieuDe.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            sLoTieuDe.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            sLoTieuDe.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            sLoO.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            sLoO.BackColor = System.Drawing.SystemColors.Window;
            sLoO.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sLoO.ForeColor = System.Drawing.SystemColors.ControlText;
            sLoO.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            sLoO.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            sLoO.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.pnlTren.Controls.Add(this.lblTieuDe);
            this.pnlTren.Controls.Add(this.lblTomTat);
            this.pnlTren.Controls.Add(this.chkConHang);
            this.pnlTren.BackColor = System.Drawing.Color.White;
            this.pnlTren.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTren.Location = new System.Drawing.Point(0, 0);
            this.pnlTren.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.pnlTren.Size = new System.Drawing.Size(860, 62);
            this.pnlTren.Name = "pnlTren";
            this.pnlTren.TabIndex = 0;
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTieuDe.Location = new System.Drawing.Point(12, 8);
            this.lblTieuDe.Text = "Các lô thuốc";
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 1;
            // 
            // lblTomTat
            // 
            this.lblTomTat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTomTat.ForeColor = System.Drawing.Color.DimGray;
            this.lblTomTat.Location = new System.Drawing.Point(12, 36);
            this.lblTomTat.Size = new System.Drawing.Size(640, 20);
            this.lblTomTat.Text = "";
            this.lblTomTat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTomTat.Name = "lblTomTat";
            this.lblTomTat.TabIndex = 2;
            // 
            // chkConHang
            // 
            this.chkConHang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkConHang.AutoSize = true;
            this.chkConHang.Checked = true;
            this.chkConHang.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConHang.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkConHang.Location = new System.Drawing.Point(700, 10);
            this.chkConHang.Text = "Chỉ lô còn hàng";
            this.chkConHang.UseVisualStyleBackColor = true;
            this.chkConHang.Name = "chkConHang";
            this.chkConHang.TabIndex = 3;
            this.chkConHang.CheckedChanged += new System.EventHandler(this.chkConHang_CheckedChanged);
            // 
            // dgvLo
            // 
            this.dgvLo.AllowUserToAddRows = false;
            this.dgvLo.AllowUserToDeleteRows = false;
            this.dgvLo.AllowUserToResizeRows = false;
            this.dgvLo.AlternatingRowsDefaultCellStyle = sLoXen;
            this.dgvLo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLo.BackgroundColor = System.Drawing.Color.White;
            this.dgvLo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLo.ColumnHeadersDefaultCellStyle = sLoTieuDe;
            this.dgvLo.DefaultCellStyle = sLoO;
            this.dgvLo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLo.Location = new System.Drawing.Point(0, 62);
            this.dgvLo.MultiSelect = false;
            this.dgvLo.ReadOnly = true;
            this.dgvLo.RowHeadersVisible = false;
            this.dgvLo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLo.Size = new System.Drawing.Size(860, 378);
            this.dgvLo.Name = "dgvLo";
            this.dgvLo.TabIndex = 4;
            this.dgvLo.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvLo_CellFormatting);
            this.dgvLo.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvLo_DataBindingComplete);
            // 
            // lblChuThich
            // 
            this.lblChuThich.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChuThich.ForeColor = System.Drawing.Color.DimGray;
            this.lblChuThich.Location = new System.Drawing.Point(0, 440);
            this.lblChuThich.Size = new System.Drawing.Size(860, 40);
            this.lblChuThich.Text = "Nền đỏ: lô đã hết hạn (không bán, chờ hủy)   •   Nền vàng: sắp hết hạn (≤ 30 ngày)   •   Cột \"Thứ tự bán\": 1 = lô sẽ được bán tiếp theo (FEFO - hết hạn trước bán trước)";
            this.lblChuThich.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblChuThich.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.lblChuThich.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblChuThich.Name = "lblChuThich";
            this.lblChuThich.TabIndex = 5;
            // 
            // frmLoThuoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(860, 480);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 360);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Lô thuốc";
            this.Controls.Add(this.dgvLo);
            this.Controls.Add(this.lblChuThich);
            this.Controls.Add(this.pnlTren);
            this.Name = "frmLoThuoc";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLo)).EndInit();
            this.pnlTren.ResumeLayout(false);
            this.pnlTren.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTren;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblTomTat;
        private System.Windows.Forms.CheckBox chkConHang;
        private System.Windows.Forms.DataGridView dgvLo;
        private System.Windows.Forms.Label lblChuThich;
    }
}
