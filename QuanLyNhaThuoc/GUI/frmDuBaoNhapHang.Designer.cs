namespace QuanLyNhaThuoc.GUI
{
    partial class frmDuBaoNhapHang
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
            this.dgvDuBao = new System.Windows.Forms.DataGridView();
            this.colChon = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMaThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDVT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTocDo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConDu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeXuat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMucDo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoiKhuyen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlDuoi = new System.Windows.Forms.Panel();
            this.lblTomTat = new System.Windows.Forms.Label();
            this.btnXuatPhieu = new System.Windows.Forms.Button();
            this.btnMoNhapHang = new System.Windows.Forms.Button();
            this.pnlTren = new System.Windows.Forms.Panel();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblPhanTich = new System.Windows.Forms.Label();
            this.nudPhanTich = new System.Windows.Forms.NumericUpDown();
            this.lblNgay1 = new System.Windows.Forms.Label();
            this.nudDuTru = new System.Windows.Forms.NumericUpDown();
            this.lblNgay2 = new System.Windows.Forms.Label();
            this.chkChiCanNhap = new System.Windows.Forms.CheckBox();
            this.btnTinh = new System.Windows.Forms.Button();
            this.lblCongThuc = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDuBao)).BeginInit();
            this.pnlDuoi.SuspendLayout();
            this.pnlTren.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPhanTich)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuTru)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDuBao
            // 
            this.dgvDuBao.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colChon,
            this.colMaThuoc,
            this.colTenThuoc,
            this.colDVT,
            this.colTon,
            this.colDaBan,
            this.colTocDo,
            this.colConDu,
            this.colDeXuat,
            this.colMucDo,
            this.colLoiKhuyen});
            this.dgvDuBao.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.dgvDuBao.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDuBao.AllowUserToAddRows = false;
            this.dgvDuBao.AllowUserToDeleteRows = false;
            this.dgvDuBao.AllowUserToResizeRows = false;
            this.dgvDuBao.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDuBao.BackgroundColor = System.Drawing.Color.White;
            this.dgvDuBao.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDuBao.ColumnHeadersHeight = 32;
            this.dgvDuBao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDuBao.EnableHeadersVisualStyles = false;
            this.dgvDuBao.MultiSelect = false;
            this.dgvDuBao.RowHeadersVisible = false;
            this.dgvDuBao.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDuBao.RowTemplate.Height = 28;
            this.dgvDuBao.ReadOnly = false;
            this.dgvDuBao.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvDuBao.Name = "dgvDuBao";
            this.dgvDuBao.TabIndex = 3;
            this.dgvDuBao.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvDuBao_CurrentCellDirtyStateChanged);
            this.dgvDuBao.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDuBao_CellValueChanged);
            // 
            // colChon
            // 
            this.colChon.HeaderText = "Chọn";
            this.colChon.FillWeight = 35F;
            this.colChon.Name = "colChon";
            // 
            // colMaThuoc
            // 
            this.colMaThuoc.HeaderText = "Mã";
            this.colMaThuoc.FillWeight = 50F;
            this.colMaThuoc.ReadOnly = true;
            this.colMaThuoc.Name = "colMaThuoc";
            // 
            // colTenThuoc
            // 
            this.colTenThuoc.HeaderText = "Tên thuốc";
            this.colTenThuoc.FillWeight = 120F;
            this.colTenThuoc.ReadOnly = true;
            this.colTenThuoc.Name = "colTenThuoc";
            // 
            // colDVT
            // 
            this.colDVT.HeaderText = "ĐVT";
            this.colDVT.FillWeight = 45F;
            this.colDVT.ReadOnly = true;
            this.colDVT.Name = "colDVT";
            // 
            // colTon
            // 
            this.colTon.HeaderText = "Tồn bán được";
            this.colTon.FillWeight = 60F;
            this.colTon.ReadOnly = true;
            this.colTon.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTon.Name = "colTon";
            // 
            // colDaBan
            // 
            this.colDaBan.HeaderText = "Đã bán";
            this.colDaBan.FillWeight = 55F;
            this.colDaBan.ReadOnly = true;
            this.colDaBan.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colDaBan.Name = "colDaBan";
            // 
            // colTocDo
            // 
            this.colTocDo.HeaderText = "Bán / ngày";
            this.colTocDo.FillWeight = 55F;
            this.colTocDo.ReadOnly = true;
            this.colTocDo.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTocDo.Name = "colTocDo";
            // 
            // colConDu
            // 
            this.colConDu.HeaderText = "Đủ bán (ngày)";
            this.colConDu.FillWeight = 60F;
            this.colConDu.ReadOnly = true;
            this.colConDu.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colConDu.Name = "colConDu";
            // 
            // colDeXuat
            // 
            this.colDeXuat.HeaderText = "Nên nhập";
            this.colDeXuat.FillWeight = 55F;
            this.colDeXuat.ReadOnly = true;
            this.colDeXuat.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colDeXuat.Name = "colDeXuat";
            // 
            // colMucDo
            // 
            this.colMucDo.HeaderText = "Mức";
            this.colMucDo.FillWeight = 75F;
            this.colMucDo.ReadOnly = true;
            this.colMucDo.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colMucDo.Name = "colMucDo";
            // 
            // colLoiKhuyen
            // 
            this.colLoiKhuyen.HeaderText = "Lời khuyên";
            this.colLoiKhuyen.FillWeight = 230F;
            this.colLoiKhuyen.ReadOnly = true;
            this.colLoiKhuyen.Name = "colLoiKhuyen";
            // 
            // pnlDuoi
            // 
            this.pnlDuoi.Controls.Add(this.lblTomTat);
            this.pnlDuoi.Controls.Add(this.btnXuatPhieu);
            this.pnlDuoi.Controls.Add(this.btnMoNhapHang);
            this.pnlDuoi.BackColor = System.Drawing.Color.White;
            this.pnlDuoi.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDuoi.Location = new System.Drawing.Point(0, 580);
            this.pnlDuoi.Size = new System.Drawing.Size(1000, 60);
            this.pnlDuoi.Name = "pnlDuoi";
            this.pnlDuoi.TabIndex = 2;
            // 
            // lblTomTat
            // 
            this.lblTomTat.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTomTat.Location = new System.Drawing.Point(15, 8);
            this.lblTomTat.Text = "";
            this.lblTomTat.Size = new System.Drawing.Size(560, 44);
            this.lblTomTat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTomTat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right));
            this.lblTomTat.AutoEllipsis = true;
            this.lblTomTat.Name = "lblTomTat";
            this.lblTomTat.TabIndex = 0;
            // 
            // btnXuatPhieu
            // 
            this.btnXuatPhieu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnXuatPhieu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXuatPhieu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatPhieu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatPhieu.ForeColor = System.Drawing.Color.White;
            this.btnXuatPhieu.Location = new System.Drawing.Point(598, 11);
            this.btnXuatPhieu.Size = new System.Drawing.Size(230, 38);
            this.btnXuatPhieu.Text = "📄 Lập phiếu nhập Excel";
            this.btnXuatPhieu.UseVisualStyleBackColor = false;
            this.btnXuatPhieu.FlatAppearance.BorderSize = 0;
            this.btnXuatPhieu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Right));
            this.btnXuatPhieu.Name = "btnXuatPhieu";
            this.btnXuatPhieu.TabIndex = 1;
            this.btnXuatPhieu.Click += new System.EventHandler(this.btnXuatPhieu_Click);
            // 
            // btnMoNhapHang
            // 
            this.btnMoNhapHang.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnMoNhapHang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMoNhapHang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoNhapHang.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoNhapHang.ForeColor = System.Drawing.Color.White;
            this.btnMoNhapHang.Location = new System.Drawing.Point(838, 11);
            this.btnMoNhapHang.Size = new System.Drawing.Size(150, 38);
            this.btnMoNhapHang.Text = "➡ Trang Nhập hàng";
            this.btnMoNhapHang.UseVisualStyleBackColor = false;
            this.btnMoNhapHang.FlatAppearance.BorderSize = 0;
            this.btnMoNhapHang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Right));
            this.btnMoNhapHang.Name = "btnMoNhapHang";
            this.btnMoNhapHang.TabIndex = 2;
            this.btnMoNhapHang.Click += new System.EventHandler(this.btnMoNhapHang_Click);
            // 
            // pnlTren
            // 
            this.pnlTren.Controls.Add(this.lblTieuDe);
            this.pnlTren.Controls.Add(this.lblPhanTich);
            this.pnlTren.Controls.Add(this.nudPhanTich);
            this.pnlTren.Controls.Add(this.lblNgay1);
            this.pnlTren.Controls.Add(this.nudDuTru);
            this.pnlTren.Controls.Add(this.lblNgay2);
            this.pnlTren.Controls.Add(this.chkChiCanNhap);
            this.pnlTren.Controls.Add(this.btnTinh);
            this.pnlTren.Controls.Add(this.lblCongThuc);
            this.pnlTren.BackColor = System.Drawing.Color.White;
            this.pnlTren.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTren.Location = new System.Drawing.Point(0, 0);
            this.pnlTren.Size = new System.Drawing.Size(1000, 106);
            this.pnlTren.Name = "pnlTren";
            this.pnlTren.TabIndex = 0;
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(15, 10);
            this.lblTieuDe.Text = "📈 Dự báo nhập hàng theo tốc độ bán";
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 0;
            // 
            // lblPhanTich
            // 
            this.lblPhanTich.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhanTich.Location = new System.Drawing.Point(17, 52);
            this.lblPhanTich.Text = "Phân tích bán trong";
            this.lblPhanTich.AutoSize = true;
            this.lblPhanTich.Name = "lblPhanTich";
            this.lblPhanTich.TabIndex = 1;
            // 
            // nudPhanTich
            // 
            this.nudPhanTich.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudPhanTich.Location = new System.Drawing.Point(150, 49);
            this.nudPhanTich.Maximum = new decimal(new int[] { 365, 0, 0, 0});
            this.nudPhanTich.Minimum = new decimal(new int[] { 7, 0, 0, 0});
            this.nudPhanTich.Size = new System.Drawing.Size(65, 25);
            this.nudPhanTich.Value = new decimal(new int[] { 30, 0, 0, 0});
            this.nudPhanTich.Name = "nudPhanTich";
            this.nudPhanTich.TabIndex = 2;
            // 
            // lblNgay1
            // 
            this.lblNgay1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgay1.Location = new System.Drawing.Point(220, 52);
            this.lblNgay1.Text = "ngày gần nhất,  dự trữ đủ bán";
            this.lblNgay1.AutoSize = true;
            this.lblNgay1.Name = "lblNgay1";
            this.lblNgay1.TabIndex = 3;
            // 
            // nudDuTru
            // 
            this.nudDuTru.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDuTru.Location = new System.Drawing.Point(415, 49);
            this.nudDuTru.Maximum = new decimal(new int[] { 180, 0, 0, 0});
            this.nudDuTru.Minimum = new decimal(new int[] { 7, 0, 0, 0});
            this.nudDuTru.Size = new System.Drawing.Size(65, 25);
            this.nudDuTru.Value = new decimal(new int[] { 30, 0, 0, 0});
            this.nudDuTru.Name = "nudDuTru";
            this.nudDuTru.TabIndex = 4;
            // 
            // lblNgay2
            // 
            this.lblNgay2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgay2.Location = new System.Drawing.Point(485, 52);
            this.lblNgay2.Text = "ngày";
            this.lblNgay2.AutoSize = true;
            this.lblNgay2.Name = "lblNgay2";
            this.lblNgay2.TabIndex = 5;
            // 
            // chkChiCanNhap
            // 
            this.chkChiCanNhap.AutoSize = true;
            this.chkChiCanNhap.Checked = true;
            this.chkChiCanNhap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChiCanNhap.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkChiCanNhap.Location = new System.Drawing.Point(545, 51);
            this.chkChiCanNhap.Text = "Chỉ hiện thuốc cần nhập";
            this.chkChiCanNhap.Name = "chkChiCanNhap";
            this.chkChiCanNhap.TabIndex = 6;
            this.chkChiCanNhap.CheckedChanged += new System.EventHandler(this.chkChiCanNhap_CheckedChanged);
            // 
            // btnTinh
            // 
            this.btnTinh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnTinh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTinh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTinh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTinh.ForeColor = System.Drawing.Color.White;
            this.btnTinh.Location = new System.Drawing.Point(760, 44);
            this.btnTinh.Size = new System.Drawing.Size(140, 34);
            this.btnTinh.Text = "🔄 Tính dự báo";
            this.btnTinh.UseVisualStyleBackColor = false;
            this.btnTinh.FlatAppearance.BorderSize = 0;
            this.btnTinh.Name = "btnTinh";
            this.btnTinh.TabIndex = 7;
            this.btnTinh.Click += new System.EventHandler(this.btnTinh_Click);
            // 
            // lblCongThuc
            // 
            this.lblCongThuc.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCongThuc.Location = new System.Drawing.Point(17, 82);
            this.lblCongThuc.Text = "Bán/ngày = đã bán ÷ số ngày phân tích   •   Đủ bán = tồn còn hạn ÷ bán/ngày   •   Nên nhập = bán/ngày × ngày dự trữ − tồn   •   Tích cột \"Chọn\" rồi bấm \"Lập phiếu nhập Excel\".";
            this.lblCongThuc.Size = new System.Drawing.Size(970, 20);
            this.lblCongThuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblCongThuc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top) | System.Windows.Forms.AnchorStyles.Left))) | System.Windows.Forms.AnchorStyles.Right));
            this.lblCongThuc.Name = "lblCongThuc";
            this.lblCongThuc.TabIndex = 8;
            // 
            // frmDuBaoNhapHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text = "Dự báo nhập hàng";
            this.Load += new System.EventHandler(this.frmDuBaoNhapHang_Load);
            this.Controls.Add(this.dgvDuBao);
            this.Controls.Add(this.pnlDuoi);
            this.Controls.Add(this.pnlTren);
            this.Name = "frmDuBaoNhapHang";
            ((System.ComponentModel.ISupportInitialize)(this.nudDuTru)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPhanTich)).EndInit();
            this.pnlTren.ResumeLayout(false);
            this.pnlTren.PerformLayout();
            this.pnlDuoi.ResumeLayout(false);
            this.pnlDuoi.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDuBao)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDuBao;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDVT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTocDo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConDu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeXuat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMucDo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoiKhuyen;
        private System.Windows.Forms.Panel pnlDuoi;
        private System.Windows.Forms.Label lblTomTat;
        private System.Windows.Forms.Button btnXuatPhieu;
        private System.Windows.Forms.Button btnMoNhapHang;
        private System.Windows.Forms.Panel pnlTren;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblPhanTich;
        private System.Windows.Forms.NumericUpDown nudPhanTich;
        private System.Windows.Forms.Label lblNgay1;
        private System.Windows.Forms.NumericUpDown nudDuTru;
        private System.Windows.Forms.Label lblNgay2;
        private System.Windows.Forms.CheckBox chkChiCanNhap;
        private System.Windows.Forms.Button btnTinh;
        private System.Windows.Forms.Label lblCongThuc;
    }
}
