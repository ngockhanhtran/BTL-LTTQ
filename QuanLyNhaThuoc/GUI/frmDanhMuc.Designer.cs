namespace QuanLyNhaThuoc.GUI
{
    partial class frmDanhMuc
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
            System.Windows.Forms.DataGridViewCellStyle sDMXen = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle sDMTieuDe = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle sDMO = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTren = new System.Windows.Forms.Panel();
            this.lblTim = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnTim = new System.Windows.Forms.Button();
            this.btnTatCa = new System.Windows.Forms.Button();
            this.pnlGiua = new System.Windows.Forms.Panel();
            this.pnlLuoi = new System.Windows.Forms.Panel();
            this.pnlPhai = new System.Windows.Forms.Panel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.lblTieuDeKhung = new System.Windows.Forms.Label();
            this.lblCheDo = new System.Windows.Forms.Label();
            this.pnlONhap = new System.Windows.Forms.Panel();
            this.pnlNut = new System.Windows.Forms.Panel();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.pnlTren.SuspendLayout();
            this.pnlGiua.SuspendLayout();
            this.pnlLuoi.SuspendLayout();
            this.pnlPhai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.pnlONhap.SuspendLayout();
            this.pnlNut.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTren
            // 
            sDMXen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            sDMTieuDe.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            sDMTieuDe.BackColor = System.Drawing.SystemColors.Control;
            sDMTieuDe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sDMTieuDe.ForeColor = System.Drawing.SystemColors.WindowText;
            sDMTieuDe.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            sDMTieuDe.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            sDMTieuDe.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            sDMO.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            sDMO.BackColor = System.Drawing.SystemColors.Window;
            sDMO.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sDMO.ForeColor = System.Drawing.SystemColors.ControlText;
            sDMO.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            sDMO.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            sDMO.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.pnlTren.Controls.Add(this.lblTim);
            this.pnlTren.Controls.Add(this.txtTimKiem);
            this.pnlTren.Controls.Add(this.btnTim);
            this.pnlTren.Controls.Add(this.btnTatCa);
            this.pnlTren.BackColor = System.Drawing.Color.White;
            this.pnlTren.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTren.Location = new System.Drawing.Point(0, 0);
            this.pnlTren.Size = new System.Drawing.Size(1000, 56);
            this.pnlTren.Name = "pnlTren";
            this.pnlTren.TabIndex = 0;
            // 
            // lblTim
            // 
            this.lblTim.AutoSize = true;
            this.lblTim.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTim.Location = new System.Drawing.Point(15, 18);
            this.lblTim.Text = "Tìm kiếm:";
            this.lblTim.Name = "lblTim";
            this.lblTim.TabIndex = 1;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimKiem.Location = new System.Drawing.Point(90, 15);
            this.txtTimKiem.Size = new System.Drawing.Size(280, 25);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.TabIndex = 2;
            this.txtTimKiem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimKiem_KeyDown);
            // 
            // btnTim
            // 
            this.btnTim.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnTim.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTim.FlatAppearance.BorderSize = 0;
            this.btnTim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTim.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTim.ForeColor = System.Drawing.Color.White;
            this.btnTim.Location = new System.Drawing.Point(380, 13);
            this.btnTim.Size = new System.Drawing.Size(70, 30);
            this.btnTim.Text = "Tìm";
            this.btnTim.UseVisualStyleBackColor = false;
            this.btnTim.Name = "btnTim";
            this.btnTim.TabIndex = 3;
            this.btnTim.Click += new System.EventHandler(this.btnTim_Click);
            // 
            // btnTatCa
            // 
            this.btnTatCa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnTatCa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTatCa.FlatAppearance.BorderSize = 0;
            this.btnTatCa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTatCa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTatCa.ForeColor = System.Drawing.Color.White;
            this.btnTatCa.Location = new System.Drawing.Point(458, 13);
            this.btnTatCa.Size = new System.Drawing.Size(80, 30);
            this.btnTatCa.Text = "Tất cả";
            this.btnTatCa.UseVisualStyleBackColor = false;
            this.btnTatCa.Name = "btnTatCa";
            this.btnTatCa.TabIndex = 4;
            this.btnTatCa.Click += new System.EventHandler(this.btnTatCa_Click);
            // 
            // pnlGiua
            // 
            this.pnlGiua.Controls.Add(this.pnlLuoi);
            this.pnlGiua.Controls.Add(this.pnlPhai);
            this.pnlGiua.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGiua.Location = new System.Drawing.Point(0, 56);
            this.pnlGiua.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlGiua.Size = new System.Drawing.Size(1000, 584);
            this.pnlGiua.Name = "pnlGiua";
            this.pnlGiua.TabIndex = 5;
            // 
            // pnlLuoi
            // 
            this.pnlLuoi.Controls.Add(this.dgv);
            this.pnlLuoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLuoi.Location = new System.Drawing.Point(0, 10);
            this.pnlLuoi.Padding = new System.Windows.Forms.Padding(0, 10, 10, 0);
            this.pnlLuoi.Size = new System.Drawing.Size(620, 574);
            this.pnlLuoi.Name = "pnlLuoi";
            this.pnlLuoi.TabIndex = 6;
            // 
            // pnlPhai
            // 
            this.pnlPhai.Controls.Add(this.lblTieuDeKhung);
            this.pnlPhai.Controls.Add(this.lblCheDo);
            this.pnlPhai.Controls.Add(this.pnlONhap);
            this.pnlPhai.Controls.Add(this.pnlNut);
            this.pnlPhai.AutoScroll = true;
            this.pnlPhai.BackColor = System.Drawing.Color.White;
            this.pnlPhai.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPhai.Location = new System.Drawing.Point(620, 10);
            this.pnlPhai.Size = new System.Drawing.Size(380, 574);
            this.pnlPhai.Name = "pnlPhai";
            this.pnlPhai.TabIndex = 7;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.AlternatingRowsDefaultCellStyle = sDMXen;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv.ColumnHeadersDefaultCellStyle = sDMTieuDe;
            this.dgv.DefaultCellStyle = sDMO;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 10);
            this.dgv.MultiSelect = false;
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(610, 564);
            this.dgv.Name = "dgv";
            this.dgv.TabIndex = 8;
            this.dgv.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_DataBindingComplete);
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // lblTieuDeKhung
            // 
            this.lblTieuDeKhung.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDeKhung.Location = new System.Drawing.Point(18, 15);
            this.lblTieuDeKhung.Size = new System.Drawing.Size(345, 26);
            this.lblTieuDeKhung.Text = "Danh mục";
            this.lblTieuDeKhung.Name = "lblTieuDeKhung";
            this.lblTieuDeKhung.TabIndex = 9;
            // 
            // lblCheDo
            // 
            this.lblCheDo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheDo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblCheDo.Location = new System.Drawing.Point(18, 43);
            this.lblCheDo.Size = new System.Drawing.Size(345, 20);
            this.lblCheDo.Text = "";
            this.lblCheDo.Name = "lblCheDo";
            this.lblCheDo.TabIndex = 10;
            // 
            // pnlONhap
            // 
            this.pnlONhap.Location = new System.Drawing.Point(0, 75);
            this.pnlONhap.Size = new System.Drawing.Size(380, 200);
            this.pnlONhap.Name = "pnlONhap";
            this.pnlONhap.TabIndex = 11;
            // 
            // pnlNut
            // 
            this.pnlNut.Controls.Add(this.btnThem);
            this.pnlNut.Controls.Add(this.btnLuu);
            this.pnlNut.Controls.Add(this.btnXoa);
            this.pnlNut.Controls.Add(this.btnLamMoi);
            this.pnlNut.Location = new System.Drawing.Point(0, 285);
            this.pnlNut.Size = new System.Drawing.Size(380, 100);
            this.pnlNut.Name = "pnlNut";
            this.pnlNut.TabIndex = 12;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(18, 0);
            this.btnThem.Size = new System.Drawing.Size(165, 38);
            this.btnThem.Text = "➕ Thêm mới";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Name = "btnThem";
            this.btnThem.TabIndex = 13;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(195, 0);
            this.btnLuu.Size = new System.Drawing.Size(165, 38);
            this.btnLuu.Text = "💾 Lưu";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.TabIndex = 14;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnXoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoa.FlatAppearance.BorderSize = 0;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(18, 46);
            this.btnXoa.Size = new System.Drawing.Size(165, 38);
            this.btnXoa.Text = "🗑 Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.TabIndex = 15;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.FlatAppearance.BorderSize = 0;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(195, 46);
            this.btnLamMoi.Size = new System.Drawing.Size(165, 38);
            this.btnLamMoi.Text = "↻ Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.TabIndex = 16;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // frmDanhMuc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Text = "Danh mục";
            this.Controls.Add(this.pnlGiua);
            this.Controls.Add(this.pnlTren);
            this.Name = "frmDanhMuc";
            this.pnlNut.ResumeLayout(false);
            this.pnlNut.PerformLayout();
            this.pnlONhap.ResumeLayout(false);
            this.pnlONhap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.pnlPhai.ResumeLayout(false);
            this.pnlPhai.PerformLayout();
            this.pnlLuoi.ResumeLayout(false);
            this.pnlLuoi.PerformLayout();
            this.pnlGiua.ResumeLayout(false);
            this.pnlGiua.PerformLayout();
            this.pnlTren.ResumeLayout(false);
            this.pnlTren.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTren;
        private System.Windows.Forms.Label lblTim;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.Button btnTatCa;
        private System.Windows.Forms.Panel pnlGiua;
        private System.Windows.Forms.Panel pnlLuoi;
        private System.Windows.Forms.Panel pnlPhai;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lblTieuDeKhung;
        private System.Windows.Forms.Label lblCheDo;
        private System.Windows.Forms.Panel pnlONhap;
        private System.Windows.Forms.Panel pnlNut;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
    }
}
