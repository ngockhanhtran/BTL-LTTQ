namespace QuanLyNhaThuoc.GUI
{
    partial class frmDangNhap
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
            this.pnlTrai = new System.Windows.Forms.Panel();
            this.lblIcon = new System.Windows.Forms.Label();
            this.lblTenCH = new System.Windows.Forms.Label();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.lblTinhNang = new System.Windows.Forms.Label();
            this.lblChanTrang = new System.Windows.Forms.Label();
            this.pnlPhai = new System.Windows.Forms.Panel();
            this.btnDong = new System.Windows.Forms.Button();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblPhu = new System.Windows.Forms.Label();
            this.lblMa = new System.Windows.Forms.Label();
            this.pnlMa = new System.Windows.Forms.Panel();
            this.lblMk = new System.Windows.Forms.Label();
            this.pnlMk = new System.Windows.Forms.Panel();
            this.lblLoi = new System.Windows.Forms.Label();
            this.lnkQuenMK = new System.Windows.Forms.LinkLabel();
            this.lnkCauHinh = new System.Windows.Forms.LinkLabel();
            this.btnDangNhap = new System.Windows.Forms.Button();
            this.lblGoiY = new System.Windows.Forms.Label();
            this.lblIconMa = new System.Windows.Forms.Label();
            this.txtMaNV = new System.Windows.Forms.TextBox();
            this.lblIconMk = new System.Windows.Forms.Label();
            this.txtMatKhau = new System.Windows.Forms.TextBox();
            this.btnHienMK = new System.Windows.Forms.Button();
            this.pnlTrai.SuspendLayout();
            this.pnlPhai.SuspendLayout();
            this.pnlMa.SuspendLayout();
            this.pnlMk.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTrai
            // 
            this.pnlTrai.Controls.Add(this.lblIcon);
            this.pnlTrai.Controls.Add(this.lblTenCH);
            this.pnlTrai.Controls.Add(this.lblMoTa);
            this.pnlTrai.Controls.Add(this.lblTinhNang);
            this.pnlTrai.Controls.Add(this.lblChanTrang);
            this.pnlTrai.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTrai.Location = new System.Drawing.Point(0, 0);
            this.pnlTrai.Size = new System.Drawing.Size(360, 480);
            this.pnlTrai.Name = "pnlTrai";
            this.pnlTrai.TabIndex = 0;
            this.pnlTrai.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTrai_Paint);
            this.pnlTrai.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeoForm);
            // 
            // lblIcon
            // 
            this.lblIcon.AutoSize = true;
            this.lblIcon.BackColor = System.Drawing.Color.Transparent;
            this.lblIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIcon.ForeColor = System.Drawing.Color.White;
            this.lblIcon.Location = new System.Drawing.Point(40, 70);
            this.lblIcon.Text = "💊";
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.TabIndex = 1;
            this.lblIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeoForm);
            // 
            // lblTenCH
            // 
            this.lblTenCH.AutoSize = true;
            this.lblTenCH.BackColor = System.Drawing.Color.Transparent;
            this.lblTenCH.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenCH.ForeColor = System.Drawing.Color.White;
            this.lblTenCH.Location = new System.Drawing.Point(40, 160);
            this.lblTenCH.Text = "NHÀ THUỐC 1";
            this.lblTenCH.Name = "lblTenCH";
            this.lblTenCH.TabIndex = 2;
            this.lblTenCH.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeoForm);
            // 
            // lblMoTa
            // 
            this.lblMoTa.BackColor = System.Drawing.Color.Transparent;
            this.lblMoTa.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoTa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.lblMoTa.Location = new System.Drawing.Point(42, 205);
            this.lblMoTa.Size = new System.Drawing.Size(290, 50);
            this.lblMoTa.Text = "Hệ thống quản lý cửa hàng bán thuốc tây";
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.TabIndex = 3;
            this.lblMoTa.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeoForm);
            // 
            // lblTinhNang
            // 
            this.lblTinhNang.BackColor = System.Drawing.Color.Transparent;
            this.lblTinhNang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTinhNang.ForeColor = System.Drawing.Color.White;
            this.lblTinhNang.Location = new System.Drawing.Point(42, 275);
            this.lblTinhNang.Size = new System.Drawing.Size(300, 80);
            this.lblTinhNang.Text = "✓  Tra cứu & bán thuốc, quét mã vạch\r\n✓  Tích điểm khách hàng, in hóa đơn PDF\r\n✓  Quản lý kho, báo cáo & dashboard";
            this.lblTinhNang.Name = "lblTinhNang";
            this.lblTinhNang.TabIndex = 4;
            this.lblTinhNang.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeoForm);
            // 
            // lblChanTrang
            // 
            this.lblChanTrang.AutoSize = true;
            this.lblChanTrang.BackColor = System.Drawing.Color.Transparent;
            this.lblChanTrang.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChanTrang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.lblChanTrang.Location = new System.Drawing.Point(42, 440);
            this.lblChanTrang.Text = "© 2026  •  BTL Lập trình trực quan";
            this.lblChanTrang.Name = "lblChanTrang";
            this.lblChanTrang.TabIndex = 5;
            this.lblChanTrang.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeoForm);
            // 
            // pnlPhai
            // 
            this.pnlPhai.Controls.Add(this.btnDong);
            this.pnlPhai.Controls.Add(this.lblTieuDe);
            this.pnlPhai.Controls.Add(this.lblPhu);
            this.pnlPhai.Controls.Add(this.lblMa);
            this.pnlPhai.Controls.Add(this.pnlMa);
            this.pnlPhai.Controls.Add(this.lblMk);
            this.pnlPhai.Controls.Add(this.pnlMk);
            this.pnlPhai.Controls.Add(this.lblLoi);
            this.pnlPhai.Controls.Add(this.lnkQuenMK);
            this.pnlPhai.Controls.Add(this.lnkCauHinh);
            this.pnlPhai.Controls.Add(this.btnDangNhap);
            this.pnlPhai.Controls.Add(this.lblGoiY);
            this.pnlPhai.BackColor = System.Drawing.Color.White;
            this.pnlPhai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPhai.Location = new System.Drawing.Point(360, 0);
            this.pnlPhai.Size = new System.Drawing.Size(460, 480);
            this.pnlPhai.Name = "pnlPhai";
            this.pnlPhai.TabIndex = 6;
            this.pnlPhai.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeoForm);
            // 
            // btnDong
            // 
            this.btnDong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDong.BackColor = System.Drawing.Color.White;
            this.btnDong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDong.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDong.FlatAppearance.BorderSize = 0;
            this.btnDong.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(17)))), ((int)(((byte)(35)))));
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDong.ForeColor = System.Drawing.Color.Gray;
            this.btnDong.Location = new System.Drawing.Point(410, 6);
            this.btnDong.Size = new System.Drawing.Size(44, 34);
            this.btnDong.TabStop = false;
            this.btnDong.Text = "✕";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Name = "btnDong";
            this.btnDong.TabIndex = 7;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            this.btnDong.MouseEnter += new System.EventHandler(this.btnDong_MouseEnter);
            this.btnDong.MouseLeave += new System.EventHandler(this.btnDong_MouseLeave);
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblTieuDe.Location = new System.Drawing.Point(46, 52);
            this.lblTieuDe.Text = "Đăng nhập";
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.TabIndex = 8;
            // 
            // lblPhu
            // 
            this.lblPhu.AutoSize = true;
            this.lblPhu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhu.ForeColor = System.Drawing.Color.Gray;
            this.lblPhu.Location = new System.Drawing.Point(50, 100);
            this.lblPhu.Text = "Chào mừng trở lại! Vui lòng đăng nhập để tiếp tục.";
            this.lblPhu.Name = "lblPhu";
            this.lblPhu.TabIndex = 9;
            // 
            // lblMa
            // 
            this.lblMa.AutoSize = true;
            this.lblMa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.lblMa.Location = new System.Drawing.Point(50, 145);
            this.lblMa.Text = "Mã nhân viên";
            this.lblMa.Name = "lblMa";
            this.lblMa.TabIndex = 10;
            // 
            // pnlMa
            // 
            this.pnlMa.Controls.Add(this.lblIconMa);
            this.pnlMa.Controls.Add(this.txtMaNV);
            this.pnlMa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.pnlMa.Location = new System.Drawing.Point(50, 170);
            this.pnlMa.Size = new System.Drawing.Size(360, 44);
            this.pnlMa.TabIndex = 1;
            this.pnlMa.Name = "pnlMa";
            this.pnlMa.Click += new System.EventHandler(this.pnlONhap_Click);
            this.pnlMa.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlONhap_Paint);
            // 
            // lblMk
            // 
            this.lblMk.AutoSize = true;
            this.lblMk.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.lblMk.Location = new System.Drawing.Point(50, 230);
            this.lblMk.Text = "Mật khẩu";
            this.lblMk.Name = "lblMk";
            this.lblMk.TabIndex = 11;
            // 
            // pnlMk
            // 
            this.pnlMk.Controls.Add(this.lblIconMk);
            this.pnlMk.Controls.Add(this.txtMatKhau);
            this.pnlMk.Controls.Add(this.btnHienMK);
            this.pnlMk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.pnlMk.Location = new System.Drawing.Point(50, 255);
            this.pnlMk.Size = new System.Drawing.Size(360, 44);
            this.pnlMk.TabIndex = 2;
            this.pnlMk.Name = "pnlMk";
            this.pnlMk.Click += new System.EventHandler(this.pnlONhap_Click);
            this.pnlMk.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlONhap_Paint);
            // 
            // lblLoi
            // 
            this.lblLoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblLoi.Location = new System.Drawing.Point(50, 306);
            this.lblLoi.Size = new System.Drawing.Size(360, 22);
            this.lblLoi.Name = "lblLoi";
            this.lblLoi.TabIndex = 12;
            // 
            // lnkQuenMK
            // 
            this.lnkQuenMK.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(160)))));
            this.lnkQuenMK.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkQuenMK.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkQuenMK.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lnkQuenMK.Location = new System.Drawing.Point(270, 330);
            this.lnkQuenMK.Size = new System.Drawing.Size(140, 22);
            this.lnkQuenMK.TabStop = false;
            this.lnkQuenMK.Text = "Quên mật khẩu?";
            this.lnkQuenMK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lnkQuenMK.Name = "lnkQuenMK";
            this.lnkQuenMK.TabIndex = 13;
            this.lnkQuenMK.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkQuenMK_LinkClicked);
            // 
            // lnkCauHinh
            // 
            this.lnkCauHinh.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(160)))));
            this.lnkCauHinh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCauHinh.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkCauHinh.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lnkCauHinh.Location = new System.Drawing.Point(50, 330);
            this.lnkCauHinh.Size = new System.Drawing.Size(190, 22);
            this.lnkCauHinh.TabStop = false;
            this.lnkCauHinh.Text = "⚙ Cấu hình kết nối CSDL";
            this.lnkCauHinh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkCauHinh.Name = "lnkCauHinh";
            this.lnkCauHinh.TabIndex = 14;
            this.lnkCauHinh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCauHinh_LinkClicked);
            // 
            // btnDangNhap
            // 
            this.btnDangNhap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnDangNhap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDangNhap.FlatAppearance.BorderSize = 0;
            this.btnDangNhap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(160)))));
            this.btnDangNhap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangNhap.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDangNhap.ForeColor = System.Drawing.Color.White;
            this.btnDangNhap.Location = new System.Drawing.Point(50, 362);
            this.btnDangNhap.Size = new System.Drawing.Size(360, 46);
            this.btnDangNhap.TabIndex = 3;
            this.btnDangNhap.Text = "ĐĂNG NHẬP";
            this.btnDangNhap.UseVisualStyleBackColor = false;
            this.btnDangNhap.Name = "btnDangNhap";
            this.btnDangNhap.Click += new System.EventHandler(this.btnDangNhap_Click);
            // 
            // lblGoiY
            // 
            this.lblGoiY.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGoiY.ForeColor = System.Drawing.Color.DarkGray;
            this.lblGoiY.Location = new System.Drawing.Point(50, 425);
            this.lblGoiY.Size = new System.Drawing.Size(360, 20);
            this.lblGoiY.Text = "Lần đầu: mã NV + 11 (vd QL001 → QL00111), sau đó đổi mật khẩu riêng";
            this.lblGoiY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGoiY.Name = "lblGoiY";
            this.lblGoiY.TabIndex = 15;
            // 
            // lblIconMa
            // 
            this.lblIconMa.BackColor = System.Drawing.Color.Transparent;
            this.lblIconMa.Font = new System.Drawing.Font("Segoe UI Emoji", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIconMa.ForeColor = System.Drawing.Color.Gray;
            this.lblIconMa.Location = new System.Drawing.Point(10, 10);
            this.lblIconMa.Size = new System.Drawing.Size(28, 24);
            this.lblIconMa.Text = "👤";
            this.lblIconMa.TabIndex = 1;
            this.lblIconMa.Name = "lblIconMa";
            this.lblIconMa.Click += new System.EventHandler(this.pnlONhap_Click);
            // 
            // txtMaNV
            // 
            this.txtMaNV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtMaNV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMaNV.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMaNV.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaNV.Location = new System.Drawing.Point(44, 12);
            this.txtMaNV.MaxLength = 10;
            this.txtMaNV.Size = new System.Drawing.Size(300, 21);
            this.txtMaNV.TabIndex = 0;
            this.txtMaNV.Name = "txtMaNV";
            this.txtMaNV.Enter += new System.EventHandler(this.oNhap_Enter);
            this.txtMaNV.Leave += new System.EventHandler(this.oNhap_Leave);
            this.txtMaNV.TextChanged += new System.EventHandler(this.oNhap_TextChanged);
            // 
            // lblIconMk
            // 
            this.lblIconMk.BackColor = System.Drawing.Color.Transparent;
            this.lblIconMk.Font = new System.Drawing.Font("Segoe UI Emoji", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIconMk.ForeColor = System.Drawing.Color.Gray;
            this.lblIconMk.Location = new System.Drawing.Point(10, 10);
            this.lblIconMk.Size = new System.Drawing.Size(28, 24);
            this.lblIconMk.Text = "🔒";
            this.lblIconMk.TabIndex = 1;
            this.lblIconMk.Name = "lblIconMk";
            this.lblIconMk.Click += new System.EventHandler(this.pnlONhap_Click);
            // 
            // txtMatKhau
            // 
            this.txtMatKhau.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtMatKhau.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMatKhau.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMatKhau.Location = new System.Drawing.Point(44, 12);
            this.txtMatKhau.MaxLength = 50;
            this.txtMatKhau.Size = new System.Drawing.Size(262, 21);
            this.txtMatKhau.TabIndex = 0;
            this.txtMatKhau.UseSystemPasswordChar = true;
            this.txtMatKhau.Name = "txtMatKhau";
            this.txtMatKhau.Enter += new System.EventHandler(this.oNhap_Enter);
            this.txtMatKhau.Leave += new System.EventHandler(this.oNhap_Leave);
            this.txtMatKhau.TextChanged += new System.EventHandler(this.oNhap_TextChanged);
            // 
            // btnHienMK
            // 
            this.btnHienMK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.btnHienMK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHienMK.FlatAppearance.BorderSize = 0;
            this.btnHienMK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHienMK.Font = new System.Drawing.Font("Segoe UI Emoji", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHienMK.ForeColor = System.Drawing.Color.Gray;
            this.btnHienMK.Location = new System.Drawing.Point(315, 5);
            this.btnHienMK.Size = new System.Drawing.Size(40, 34);
            this.btnHienMK.TabIndex = 2;
            this.btnHienMK.TabStop = false;
            this.btnHienMK.Text = "👁";
            this.btnHienMK.UseVisualStyleBackColor = false;
            this.btnHienMK.Name = "btnHienMK";
            this.btnHienMK.Click += new System.EventHandler(this.btnHienMK_Click);
            // 
            // frmDangNhap
            // 
            this.AcceptButton = this.btnDangNhap;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnDong;
            this.ClientSize = new System.Drawing.Size(820, 480);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập - Nhà thuốc 1";
            this.Controls.Add(this.pnlPhai);
            this.Controls.Add(this.pnlTrai);
            this.Name = "frmDangNhap";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmDangNhap_Paint);
            this.pnlMk.ResumeLayout(false);
            this.pnlMk.PerformLayout();
            this.pnlMa.ResumeLayout(false);
            this.pnlMa.PerformLayout();
            this.pnlPhai.ResumeLayout(false);
            this.pnlPhai.PerformLayout();
            this.pnlTrai.ResumeLayout(false);
            this.pnlTrai.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTrai;
        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.Label lblTenCH;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.Label lblTinhNang;
        private System.Windows.Forms.Label lblChanTrang;
        private System.Windows.Forms.Panel pnlPhai;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblPhu;
        private System.Windows.Forms.Label lblMa;
        private System.Windows.Forms.Panel pnlMa;
        private System.Windows.Forms.Label lblMk;
        private System.Windows.Forms.Panel pnlMk;
        private System.Windows.Forms.Label lblLoi;
        private System.Windows.Forms.LinkLabel lnkQuenMK;
        private System.Windows.Forms.LinkLabel lnkCauHinh;
        private System.Windows.Forms.Button btnDangNhap;
        private System.Windows.Forms.Label lblGoiY;
        private System.Windows.Forms.Label lblIconMa;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.Label lblIconMk;
        private System.Windows.Forms.TextBox txtMatKhau;
        private System.Windows.Forms.Button btnHienMK;
    }
}
