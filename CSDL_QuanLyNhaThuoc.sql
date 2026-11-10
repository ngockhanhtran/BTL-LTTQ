/* =====================================================================
   CSDL QUẢN LÝ CỬA HÀNG BÁN THUỐC TÂY (Đề 23 - BTL Lập trình trực quan)
   ---------------------------------------------------------------------
   CÁCH CHẠY: mở file trong SSMS -> bấm F5 (Execute) chạy CẢ FILE.

   * Chạy được trên máy MỚI (chưa có CSDL) lẫn CSDL ĐANG DÙNG:
       - Bảng       : chỉ tạo khi chưa có  -> KHÔNG mất dữ liệu cũ.
       - Proc/Trigger: xóa rồi tạo lại     -> luôn là bản mới nhất.
       - Dữ liệu mẫu: chỉ thêm khi chưa có -> không bị trùng khóa.
     => Chạy lại bao nhiêu lần cũng không lỗi.
   * Muốn XÓA SẠCH và tạo lại từ đầu: bỏ comment khối "PHẦN 0" bên dưới.
   * Yêu cầu: SQL Server 2012 trở lên (DATEFROMPARTS, FORMAT, HASHBYTES SHA2_256).
   * App kết nối: lần đầu chạy app tự dò .\SQLEXPRESS, ., (localdb)\MSSQLLocalDB;
     không được thì hiện form "Cấu hình kết nối" (không cần sửa code).
   * Muốn có nhiều dữ liệu để demo báo cáo: chạy thêm file DuLieuDemo.sql (1 lần).

   NỘI DUNG
     PHẦN 0. (Tùy chọn) Xóa CSDL cũ
     PHẦN 1. Tạo CSDL
     PHẦN 2. Tạo bảng
     PHẦN 2b. Bổ sung cấu trúc (bảng VaiTro, cột mới - chạy được trên CSDL cũ)
              + lần 5: mật khẩu băm, lô thuốc, kê đơn, trả hàng, nhật ký
     PHẦN 3. Trigger đồng bộ TongTien hóa đơn + nhật ký đổi giá
     PHẦN 4. Stored procedure (lô thuốc FEFO, trả hàng, 9 yêu cầu đề bài,
             cảnh báo, dashboard)
     PHẦN 5. Dữ liệu mẫu
       5.1 Danh mục, nhân viên, NCC, thuốc
       5.2 Công dụng + gán công dụng cho thuốc (yêu cầu 4)
       5.3 Vai trò, tài khoản đăng nhập, mã vạch, khách hàng mẫu
     PHẦN 6. Sửa dữ liệu cũ (TongTien bị sai ở các bản trước,
             tồn kho cũ chưa có lô -> lô TON-DAU)

   LỊCH SỬ CẬP NHẬT
     - first edition : tạo bảng + proc 9 yêu cầu.
     - 01/11/2026    : thêm dữ liệu NhaCungCap (trước đây bảng trống nên nhập
                       hàng lỗi khóa ngoại MaNCC), thêm NV03 thủ kho, sửa
                       sp_TimKiemHoaDonNhap (LEFT JOIN, trả thêm TenNV/TenNCC).
     - 03/11/2026    : trigger TongTien (lỗi Top 2 NV ra 0.00), proc 6-9 lọc
                       theo khoảng ngày + tô màu cảnh báo, sp_CanhBaoThuocSapHet,
                       bảng LichSuHuyThuoc, sp_DanhSachCanhBao, sp_HuyThuocHetHan.
     - 10/11/2026 (lần 1) - SẮP XẾP LẠI SCRIPT:
         * Gộp tất cả thành 1 script cài đặt, sắp đúng thứ tự (trước đây phần
           UPDATE nằm trên CREATE DATABASE nên máy mới chạy bị lỗi).
         * Chạy lại nhiều lần không lỗi: bảng IF OBJECT_ID ... IS NULL, proc /
           trigger DROP rồi CREATE (thay cho ALTER), dữ liệu mẫu IF NOT EXISTS.
         * Bỏ các proc 6-9 bản "first edition" (không khớp tham số code C# đang
           truyền), chỉ giữ bản 03/11.
         * Lưu UTF-8 BOM + CRLF để SSMS đọc đúng tiếng Việt.
         * Bảng NhanVien: bỏ dấu phẩy thừa sau cột DienThoai (không đổi cấu trúc).
         * sp_TimKiemHoaDonNhap: tìm thêm theo hd.MaNV và hd.MaNCC (trước chỉ
           theo tên NV / tên NCC); thêm SET NOCOUNT ON; đổi alias cthn -> ct.
         * sp_TimKiemThuoc: thêm SET NOCOUNT ON, @TuKhoa NULL -> chuỗi rỗng.
     - 10/11/2026 (lần 2) - YÊU CẦU 4: TÌM THUỐC THEO CÔNG DỤNG:
         * Lỗi cũ: bảng CongDung / Thuoc_CongDung trống, sp_TimKiemThuoc trả
           t.* (không có cột công dụng) -> ô "Công dụng" ở form Tra cứu luôn
           trống, tìm theo công dụng không ra kết quả.
         * sp_TimKiemThuoc viết lại:
             - Trả thêm cột CongDung (các tên công dụng nối bằng ", ").
             - Tìm theo công dụng bằng EXISTS (không cần DISTINCT nữa), tìm
               thêm trong Thuoc_CongDung.GhiChu (vd: "đau đầu", "cảm cúm").
             - @TuKhoa rỗng -> trả về TẤT CẢ thuốc (app dùng proc này để tải
               danh sách thay cho SELECT * FROM Thuoc), sắp xếp theo MaThuoc.
             - Dùng STUFF + FOR XML PATH để nối chuỗi (chạy được SQL 2012+,
               STRING_AGG cần SQL 2017).
         * Dữ liệu mẫu (Phần 5.1): thêm DVT03 'Chai', DC02 'Viên nang',
           DC03 'Siro', NSX02 'Đức'; thêm 6 thuốc mẫu TH010 - TH015 (giá bán
           = 110% giá nhập, SL >= 30, hạn SD xa -> không làm sai báo cáo cảnh báo).
           Không dùng mã TH002 / TH003 vì file MauPhieuNhap.xlsx dùng 2 mã này
           để demo import thuốc MỚI.
         * Dữ liệu mẫu (Phần 5.2): 10 công dụng CD01 - CD10 và bảng gán
           Thuoc_CongDung cho TH001, TH010 - TH015; TH002 (Efferalgan) và
           TH003 (Vitamin C) chỉ được gán nếu đã import và đúng tên thuốc.
     - 10/11/2026 (lần 3) - ĐĂNG NHẬP, PHÂN QUYỀN, MÃ VẠCH, TÍCH ĐIỂM, DASHBOARD:
         * PHẦN 2b (mới) - chỉ THÊM, không xóa/sửa cột cũ, dùng IF COL_LENGTH
           nên chạy được trên CSDL đang có dữ liệu:
             - Bảng mới VaiTro (MaVaiTro, TenVaiTro): QL Quản lý, BH Nhân viên
               bán hàng, TK Thủ kho (trước đây ghi chú "bảng Vai trò hiện chưa có").
             - NhanVien thêm: MatKhau, MaVaiTro (FK VaiTro), YeuCauDoiMK (1 = đã
               bấm "Quên mật khẩu", dòng tô vàng ở trang Quản lý nhân viên),
               NgayYeuCauDoiMK, NghiViec (1 = khóa đăng nhập; dùng khi không xóa
               được NV vì đã có hóa đơn).
             - Thuoc thêm MaVach (quét mã vạch khi bán) + UNIQUE INDEX lọc
               UX_Thuoc_MaVach (cho phép nhiều thuốc chưa có mã vạch = NULL).
             - KhachHang thêm DiemTichLuy (điểm tích lũy hiện có).
             - HoaDonBan thêm DiemSuDung, TienGiamDiem, DiemCong (lưu lịch sử
               đổi/cộng điểm của từng hóa đơn).
             - Tiền giảm do đổi điểm được PHÂN BỔ vào ChiTietHDB.ThanhTien nên
               trigger TongTien và các proc báo cáo 6-9 KHÔNG phải sửa.
         * PHẦN 4: thêm 5 proc cho Dashboard / biểu đồ so sánh:
           sp_Dashboard_TongQuan, sp_Dashboard_DoanhThuTheoThang,
           sp_Dashboard_TopThuocBanChay, sp_Dashboard_DoanhThuNhanVienTheoQuy,
           sp_Dashboard_HoaDonBanTheoNam. Không sửa proc cũ.
         * PHẦN 5.3 (mới):
             - VaiTro QL/BH/TK; TrinhDo TD02 Cao đẳng, TD03 Trung cấp;
               ChuyenMon CM03 Quản lý cửa hàng.
             - Thêm nhân viên TK001 (thủ kho), QL001 (quản lý).
             - Gán vai trò NV cũ (chỉ khi chưa có) - xem lần 4 (đã sửa quy tắc).
             - Mật khẩu mặc định (chỉ khi chưa có) = mã NV + '11'
               (vd QL001 -> QL00111, NV02 -> NV0211).
             - Mã vạch mẫu cho TH001, TH010 - TH015, TEST01, TEST04 (chỉ khi
               thuốc chưa có mã vạch).
             - Khách hàng mẫu KH001 (150 điểm để demo đổi điểm), KH002.
     - 10/11/2026 (lần 4) - SỬA QUY TẮC GÁN VAI TRÒ:
         * Lần 3 gán nhầm theo TÊN nhân viên (NV01 "Quản Lý" = QL, NV03 "Thủ Kho"
           = TK). Quy tắc đúng của nhóm: vai trò theo TIỀN TỐ MÃ NV
             QLxxx = Quản lý | TKxxx = Thủ kho | NVxx (và mã khác) = Bán hàng.
         * Phần 5.3: gán vai trò mặc định theo tiền tố mã (CASE ... LIKE).
         * Sửa dữ liệu đã chạy lần 3: NV01, NV03 đang là QL / TK -> BH.
     - 10/11/2026 (lần 5) - BĂM MẬT KHẨU, LÔ THUỐC (FEFO), KÊ ĐƠN, TRẢ HÀNG,
       NHẬT KÝ HOẠT ĐỘNG:
         * MẬT KHẨU (Phần 2b): bỏ cột NhanVien.MatKhau (chữ thường), thay bằng
             MatKhauHash VARCHAR(64) = SHA-256(Salt + mật khẩu), dạng hex HOA
             MatKhauSalt VARCHAR(32) = 16 byte ngẫu nhiên, dạng hex, mỗi NV khác nhau
             PhaiDoiMK   BIT         = 1 khi quản lý vừa đặt lại -> NV phải đổi khi đăng nhập
           Mật khẩu cũ được chuyển sang dạng băm TRƯỚC khi xóa cột (không ai bị
           mất tài khoản). App (C#) băm giống hệt: SHA256(UTF-16LE(Salt + MK)).
           Quản lý KHÔNG xem được mật khẩu nữa, chỉ đặt lại. Proc mới sp_DatMatKhau.
           Phần 5.3: TK001 / QL001 thêm không kèm mật khẩu, mật khẩu mặc định
           (mã NV + '11') được băm bằng HASHBYTES('SHA2_256', ...).
         * LÔ THUỐC (Phần 2b + 4): bảng LoThuoc (mỗi lần nhập = 1 lô, có số lô,
           NSX, HSD, SL nhập, SL còn) và ChiTietHDB_Lo (hóa đơn bán lấy từ lô nào).
             - Bán: sp_XuatKhoFEFO lấy lô HẾT HẠN TRƯỚC (First Expired First Out),
               bỏ qua lô đã hết hạn.
             - Nhập: sp_NhapLo tạo lô mới.
             - Thuoc.SLHienCo = tổng SL còn của các lô, Thuoc.HanSD = hạn gần nhất
               của lô còn hàng (sp_DongBoThuocTuLo) -> báo cáo Y/C 6 và cảnh báo
               KHÔNG phải sửa.
             - fn_SLBanDuoc: số lượng còn hạn bán được.
             - sp_HuyThuocHetHan viết lại: chỉ hủy LÔ đã hết hạn (lô còn hạn của
               cùng thuốc vẫn bán được); LichSuHuyThuoc thêm MaLo, SoLo.
             - Phần 6: tồn kho cũ chưa có lô -> tạo lô "TON-DAU" (sp_TaoLoTonDau).
             - sp_DanhSachLoThuoc: xem lô theo thuốc / tất cả lô.
         * KÊ ĐƠN: Thuoc.KeDon (1 = thuốc kê đơn), HoaDonBan.MaDonThuoc (bắt
           buộc nhập khi hóa đơn có thuốc kê đơn - kiểm tra ở C#). Phần 5.3:
           TH010 Amoxicillin là thuốc kê đơn.
         * TRẢ HÀNG / HỦY HÓA ĐƠN: bảng PhieuTraHang, ChiTietTraHang; HoaDonBan
           thêm DaHuy, NgayHuy, MaNVHuy, LyDoHuy. sp_TraHangDong trả 1 dòng:
           giảm SoLuong / ThanhTien của ChiTietHDB (trigger TongTien tự tính lại),
           trả hàng về đúng lô đã xuất. Các proc báo cáo 8, 9 và dashboard thêm
           điều kiện h.DaHuy = 0 (bỏ hóa đơn đã hủy).
         * NHẬT KÝ: bảng NhatKyHoatDong (ai, lúc nào, làm gì). App ghi đăng nhập,
           đăng xuất, sửa NV, thanh toán, trả hàng, nhập hàng, sao lưu...
           Trigger mới trg_Thuoc_NhatKyGia tự ghi mỗi lần đổi giá nhập / giá bán;
           người sửa lấy từ CONTEXT_INFO do app gán khi mở kết nối
           (fn_NguoiDungHienTai).
         * Kết nối: app không còn sửa chuỗi kết nối trong code, có form "Cấu hình
           kết nối" (tự dò .\SQLEXPRESS, ., (localdb)\MSSQLLocalDB).
         * Dữ liệu demo nhiều (50 thuốc, 12 tháng bán / nhập) để ở file riêng
           DuLieuDemo.sql - chạy SAU file này, chỉ chạy 1 lần.
   ===================================================================== */


/* =====================================================================
   PHẦN 0. (TÙY CHỌN) XÓA CSDL CŨ ĐỂ CÀI LẠI TỪ ĐẦU
   !!! MẤT TOÀN BỘ DỮ LIỆU. Chỉ bỏ comment khi thật sự muốn làm sạch.
   ===================================================================== */
/*
USE master;
GO
IF DB_ID('QuanLyNhaThuoc') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyNhaThuoc SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyNhaThuoc;
END
GO
*/


/* =====================================================================
   PHẦN 1. TẠO CSDL
   ===================================================================== */
USE master;
GO
IF DB_ID('QuanLyNhaThuoc') IS NULL
    CREATE DATABASE QuanLyNhaThuoc;
GO
USE QuanLyNhaThuoc;
GO


/* =====================================================================
   PHẦN 2. TẠO BẢNG (thứ tự: bảng danh mục trước, bảng có khóa ngoại sau)
   ===================================================================== */

-- 1. Trình độ
IF OBJECT_ID('TrinhDo', 'U') IS NULL
CREATE TABLE TrinhDo (
    MaTrinhDo  VARCHAR(10)  PRIMARY KEY,
    TenTrinhDo NVARCHAR(50) NOT NULL
);

-- 2. Chuyên môn
IF OBJECT_ID('ChuyenMon', 'U') IS NULL
CREATE TABLE ChuyenMon (
    MaChuyenMon  VARCHAR(10)  PRIMARY KEY,
    TenChuyenMon NVARCHAR(50) NOT NULL
);

-- 3. Đơn vị tính
IF OBJECT_ID('DonViTinh', 'U') IS NULL
CREATE TABLE DonViTinh (
    MaDVT  VARCHAR(10)  PRIMARY KEY,
    TenDVT NVARCHAR(50) NOT NULL
);

-- 4. Dạng điều chế
IF OBJECT_ID('DangDieuChe', 'U') IS NULL
CREATE TABLE DangDieuChe (
    MaDangDC  VARCHAR(10)  PRIMARY KEY,
    TenDangDC NVARCHAR(50) NOT NULL
);

-- 5. Nước sản xuất
IF OBJECT_ID('NuocSX', 'U') IS NULL
CREATE TABLE NuocSX (
    MaNuocSX  VARCHAR(10)  PRIMARY KEY,
    TenNuocSX NVARCHAR(50) NOT NULL
);

-- 6. Công dụng
IF OBJECT_ID('CongDung', 'U') IS NULL
CREATE TABLE CongDung (
    MaCongDung  VARCHAR(10)   PRIMARY KEY,
    TenCongDung NVARCHAR(100) NOT NULL
);

-- 7. Nhân viên
IF OBJECT_ID('NhanVien', 'U') IS NULL
CREATE TABLE NhanVien (
    MaNV        VARCHAR(10)   PRIMARY KEY,
    TenNV       NVARCHAR(50)  NOT NULL,
    NgaySinh    DATE,
    GioiTinh    NVARCHAR(10),
    DiaChi      NVARCHAR(255),
    MaTrinhDo   VARCHAR(10)   FOREIGN KEY REFERENCES TrinhDo(MaTrinhDo),
    MaChuyenMon VARCHAR(10)   FOREIGN KEY REFERENCES ChuyenMon(MaChuyenMon),
    DienThoai   VARCHAR(20)
);

-- 8. Khách hàng
IF OBJECT_ID('KhachHang', 'U') IS NULL
CREATE TABLE KhachHang (
    MaKhach   VARCHAR(10)   PRIMARY KEY,
    TenKhach  NVARCHAR(50)  NOT NULL,
    DiaChi    NVARCHAR(255),
    DienThoai VARCHAR(20)
);

-- 9. Nhà cung cấp
IF OBJECT_ID('NhaCungCap', 'U') IS NULL
CREATE TABLE NhaCungCap (
    MaNCC     VARCHAR(10)   PRIMARY KEY,
    TenNCC    NVARCHAR(100) NOT NULL,
    DiaChi    NVARCHAR(255),
    DienThoai VARCHAR(20)
);

-- 10. Thuốc (danh mục thuốc)
--     SLHienCo  : tự cập nhật khi nhập / bán (yêu cầu 1)
--     DonGiaNhap: tự cập nhật khi nhập hàng (yêu cầu 2)
--     GiaBan    : = 110% DonGiaNhap         (yêu cầu 3)
IF OBJECT_ID('Thuoc', 'U') IS NULL
CREATE TABLE Thuoc (
    MaThuoc      VARCHAR(10)   PRIMARY KEY,
    TenThuoc     NVARCHAR(100) NOT NULL,
    MaDVT        VARCHAR(10)   FOREIGN KEY REFERENCES DonViTinh(MaDVT),
    MaDangDC     VARCHAR(10)   FOREIGN KEY REFERENCES DangDieuChe(MaDangDC),
    ThanhPhan    NVARCHAR(255),
    DonGiaNhap   DECIMAL(18,2) NOT NULL DEFAULT 0,
    GiaBan       DECIMAL(18,2) NOT NULL DEFAULT 0,
    SLHienCo     INT           NOT NULL DEFAULT 0,
    NgaySX       DATE,
    MaNuocSX     VARCHAR(10)   FOREIGN KEY REFERENCES NuocSX(MaNuocSX),
    HanSD        DATE,
    ChongChiDinh NVARCHAR(255)
);

-- 11. Thuốc - Công dụng (nhiều - nhiều)
IF OBJECT_ID('Thuoc_CongDung', 'U') IS NULL
CREATE TABLE Thuoc_CongDung (
    MaThuoc    VARCHAR(10)   FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    MaCongDung VARCHAR(10)   FOREIGN KEY REFERENCES CongDung(MaCongDung),
    GhiChu     NVARCHAR(255),
    PRIMARY KEY (MaThuoc, MaCongDung)
);

-- 12. Hóa đơn bán
IF OBJECT_ID('HoaDonBan', 'U') IS NULL
CREATE TABLE HoaDonBan (
    MaHDB    VARCHAR(10)   PRIMARY KEY,
    NgayBan  DATETIME      DEFAULT GETDATE(),
    MaNV     VARCHAR(10)   FOREIGN KEY REFERENCES NhanVien(MaNV),
    MaKhach  VARCHAR(10)   FOREIGN KEY REFERENCES KhachHang(MaKhach),
    TongTien DECIMAL(18,2) DEFAULT 0      -- tự đồng bộ bằng trigger (Phần 3)
);

-- 13. Chi tiết hóa đơn bán
IF OBJECT_ID('ChiTietHDB', 'U') IS NULL
CREATE TABLE ChiTietHDB (
    MaHDB     VARCHAR(10)   FOREIGN KEY REFERENCES HoaDonBan(MaHDB),
    MaThuoc   VARCHAR(10)   FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    SoLuong   INT           NOT NULL,
    GiamGia   DECIMAL(18,2) DEFAULT 0,    -- % giảm giá áp cho dòng
    ThanhTien DECIMAL(18,2) NOT NULL,     -- đã trừ giảm giá
    PRIMARY KEY (MaHDB, MaThuoc)
);

-- 14. Hóa đơn nhập
IF OBJECT_ID('HoaDonNhap', 'U') IS NULL
CREATE TABLE HoaDonNhap (
    MaHDN    VARCHAR(10)   PRIMARY KEY,
    NgayNhap DATETIME      DEFAULT GETDATE(),
    MaNV     VARCHAR(10)   FOREIGN KEY REFERENCES NhanVien(MaNV),   -- thủ kho
    MaNCC    VARCHAR(10)   FOREIGN KEY REFERENCES NhaCungCap(MaNCC),
    TongTien DECIMAL(18,2) DEFAULT 0      -- tự đồng bộ bằng trigger (Phần 3)
);

-- 15. Chi tiết hóa đơn nhập
IF OBJECT_ID('ChiTietHDN', 'U') IS NULL
CREATE TABLE ChiTietHDN (
    MaHDN     VARCHAR(10)   FOREIGN KEY REFERENCES HoaDonNhap(MaHDN),
    MaThuoc   VARCHAR(10)   FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    SLNhap    INT           NOT NULL,
    DonGia    DECIMAL(18,2) NOT NULL,
    KhuyenMai DECIMAL(18,2) DEFAULT 0,    -- số tiền được giảm
    ThanhTien DECIMAL(18,2) NOT NULL,     -- = SLNhap * DonGia - KhuyenMai
    PRIMARY KEY (MaHDN, MaThuoc)
);

-- 16. Lịch sử hủy thuốc (nút "Hủy thuốc hết hạn" ở trang Báo cáo)
--     Không xóa dòng trong Thuoc vì thuốc còn nằm trong hóa đơn cũ (khóa ngoại)
--     -> chỉ đưa SLHienCo về 0 và ghi lại lịch sử ở đây.
IF OBJECT_ID('LichSuHuyThuoc', 'U') IS NULL
CREATE TABLE LichSuHuyThuoc (
    MaHuy      INT IDENTITY(1,1) PRIMARY KEY,
    MaThuoc    VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    TenThuoc   NVARCHAR(100),
    SoLuongHuy INT           NOT NULL,
    HanSD      DATE,
    DonGiaNhap DECIMAL(18,2) NOT NULL DEFAULT 0,
    GiaTriHuy  DECIMAL(18,2) NOT NULL DEFAULT 0,
    NgayHuy    DATETIME      NOT NULL DEFAULT GETDATE(),
    MaNV       VARCHAR(10)   NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    LyDo       NVARCHAR(255)
);
GO


/* =====================================================================
   PHẦN 2b. [10/11/2026 lần 3] BỔ SUNG CẤU TRÚC
   Chỉ THÊM bảng / cột mới (IF ... IS NULL) -> chạy an toàn trên CSDL cũ,
   không mất dữ liệu. Xem LỊCH SỬ CẬP NHẬT ở đầu file.
   ===================================================================== */

-- 17. Vai trò (phân quyền): QL Quản lý | BH Nhân viên bán hàng | TK Thủ kho
IF OBJECT_ID('VaiTro', 'U') IS NULL
CREATE TABLE VaiTro (
    MaVaiTro  VARCHAR(10)  PRIMARY KEY,
    TenVaiTro NVARCHAR(50) NOT NULL
);
GO

-- NhanVien: tài khoản đăng nhập + phân quyền + quên mật khẩu + nghỉ việc
-- [10/11/2026 lần 5] bỏ cột MatKhau (chữ thường) -> MatKhauHash / MatKhauSalt ở cuối Phần 2b
IF COL_LENGTH('NhanVien', 'MaVaiTro') IS NULL
    ALTER TABLE NhanVien ADD MaVaiTro VARCHAR(10) NULL
        CONSTRAINT FK_NhanVien_VaiTro REFERENCES VaiTro(MaVaiTro);
IF COL_LENGTH('NhanVien', 'YeuCauDoiMK') IS NULL
    ALTER TABLE NhanVien ADD YeuCauDoiMK BIT NOT NULL
        CONSTRAINT DF_NhanVien_YeuCauDoiMK DEFAULT 0;
IF COL_LENGTH('NhanVien', 'NgayYeuCauDoiMK') IS NULL
    ALTER TABLE NhanVien ADD NgayYeuCauDoiMK DATETIME NULL;
IF COL_LENGTH('NhanVien', 'NghiViec') IS NULL
    ALTER TABLE NhanVien ADD NghiViec BIT NOT NULL
        CONSTRAINT DF_NhanVien_NghiViec DEFAULT 0;

-- Thuoc: mã vạch in trên hộp thuốc (quét khi bán)
IF COL_LENGTH('Thuoc', 'MaVach') IS NULL
    ALTER TABLE Thuoc ADD MaVach VARCHAR(50) NULL;

-- KhachHang: điểm tích lũy hiện có
IF COL_LENGTH('KhachHang', 'DiemTichLuy') IS NULL
    ALTER TABLE KhachHang ADD DiemTichLuy INT NOT NULL
        CONSTRAINT DF_KhachHang_DiemTichLuy DEFAULT 0;

-- HoaDonBan: lịch sử điểm của từng hóa đơn
--   DiemSuDung  : số điểm khách dùng để giảm tiền
--   TienGiamDiem: số tiền được giảm nhờ đổi điểm (đã phân bổ vào ChiTietHDB.ThanhTien)
--   DiemCong    : số điểm được cộng sau hóa đơn này
IF COL_LENGTH('HoaDonBan', 'DiemSuDung') IS NULL
    ALTER TABLE HoaDonBan ADD DiemSuDung INT NOT NULL
        CONSTRAINT DF_HoaDonBan_DiemSuDung DEFAULT 0;
IF COL_LENGTH('HoaDonBan', 'TienGiamDiem') IS NULL
    ALTER TABLE HoaDonBan ADD TienGiamDiem DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_HoaDonBan_TienGiamDiem DEFAULT 0;
IF COL_LENGTH('HoaDonBan', 'DiemCong') IS NULL
    ALTER TABLE HoaDonBan ADD DiemCong INT NOT NULL
        CONSTRAINT DF_HoaDonBan_DiemCong DEFAULT 0;
GO

-- Mã vạch không được trùng giữa 2 thuốc (thuốc chưa có mã vạch = NULL thì bỏ qua)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_Thuoc_MaVach' AND object_id = OBJECT_ID('Thuoc'))
    CREATE UNIQUE INDEX UX_Thuoc_MaVach ON Thuoc(MaVach) WHERE MaVach IS NOT NULL;
GO


/* ---------------------------------------------------------------------
   [10/11/2026 lần 5] BỔ SUNG CẤU TRÚC: băm mật khẩu, lô thuốc, kê đơn,
   trả hàng / hủy hóa đơn, nhật ký hoạt động. Xem LỊCH SỬ CẬP NHẬT.
   --------------------------------------------------------------------- */

-- NhanVien: mật khẩu băm SHA-256 có salt (thay cho cột MatKhau chữ thường)
IF COL_LENGTH('NhanVien', 'MatKhauHash') IS NULL
    ALTER TABLE NhanVien ADD MatKhauHash VARCHAR(64) NULL;
IF COL_LENGTH('NhanVien', 'MatKhauSalt') IS NULL
    ALTER TABLE NhanVien ADD MatKhauSalt VARCHAR(32) NULL;
IF COL_LENGTH('NhanVien', 'PhaiDoiMK') IS NULL
    ALTER TABLE NhanVien ADD PhaiDoiMK BIT NOT NULL
        CONSTRAINT DF_NhanVien_PhaiDoiMK DEFAULT 0;
GO

-- CSDL đã chạy lần 3-4 (còn cột MatKhau chữ thường): băm mật khẩu đang dùng rồi xóa cột.
-- Dùng sp_executesql vì cột MatKhau có thể không còn (SQL Server kiểm tra tên cột khi biên dịch).
IF COL_LENGTH('NhanVien', 'MatKhau') IS NOT NULL
BEGIN
    EXEC sp_executesql N'
        UPDATE NhanVien
        SET MatKhauSalt = CONVERT(VARCHAR(32), CAST(NEWID() AS BINARY(16)), 2)
        WHERE MatKhauHash IS NULL AND LTRIM(RTRIM(ISNULL(MatKhau, N''''))) <> N'''';

        UPDATE NhanVien
        SET MatKhauHash = CONVERT(VARCHAR(64), HASHBYTES(''SHA2_256'', CAST(MatKhauSalt AS NVARCHAR(32)) + MatKhau), 2)
        WHERE MatKhauHash IS NULL AND MatKhauSalt IS NOT NULL AND LTRIM(RTRIM(ISNULL(MatKhau, N''''))) <> N'''';';

    ALTER TABLE NhanVien DROP COLUMN MatKhau;
END
GO

-- Thuoc: thuốc kê đơn (bán phải nhập mã đơn thuốc)
IF COL_LENGTH('Thuoc', 'KeDon') IS NULL
    ALTER TABLE Thuoc ADD KeDon BIT NOT NULL
        CONSTRAINT DF_Thuoc_KeDon DEFAULT 0;

-- HoaDonBan: mã đơn thuốc (khi có thuốc kê đơn) + hủy hóa đơn
IF COL_LENGTH('HoaDonBan', 'MaDonThuoc') IS NULL
    ALTER TABLE HoaDonBan ADD MaDonThuoc NVARCHAR(30) NULL;
IF COL_LENGTH('HoaDonBan', 'DaHuy') IS NULL
    ALTER TABLE HoaDonBan ADD DaHuy BIT NOT NULL
        CONSTRAINT DF_HoaDonBan_DaHuy DEFAULT 0;
IF COL_LENGTH('HoaDonBan', 'NgayHuy') IS NULL
    ALTER TABLE HoaDonBan ADD NgayHuy DATETIME NULL;
IF COL_LENGTH('HoaDonBan', 'MaNVHuy') IS NULL
    ALTER TABLE HoaDonBan ADD MaNVHuy VARCHAR(10) NULL
        CONSTRAINT FK_HoaDonBan_NVHuy REFERENCES NhanVien(MaNV);
IF COL_LENGTH('HoaDonBan', 'LyDoHuy') IS NULL
    ALTER TABLE HoaDonBan ADD LyDoHuy NVARCHAR(255) NULL;

-- LichSuHuyThuoc: hủy theo lô
IF COL_LENGTH('LichSuHuyThuoc', 'MaLo') IS NULL
    ALTER TABLE LichSuHuyThuoc ADD MaLo INT NULL;
IF COL_LENGTH('LichSuHuyThuoc', 'SoLo') IS NULL
    ALTER TABLE LichSuHuyThuoc ADD SoLo NVARCHAR(30) NULL;
GO

-- 18. Lô thuốc: mỗi lần nhập 1 thuốc = 1 lô (cùng thuốc có nhiều lô, nhiều hạn dùng)
--     SLCon giảm khi bán (FEFO), tăng khi khách trả hàng, về 0 khi hủy lô hết hạn.
IF OBJECT_ID('LoThuoc', 'U') IS NULL
CREATE TABLE LoThuoc (
    MaLo       INT IDENTITY(1,1) PRIMARY KEY,
    MaThuoc    VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    SoLo       NVARCHAR(30)  NOT NULL,              -- số lô in trên hộp (không có thì tự đặt)
    NgaySX     DATE          NULL,
    HanSD      DATE          NOT NULL,
    SLNhap     INT           NOT NULL,
    SLCon      INT           NOT NULL,
    DonGiaNhap DECIMAL(18,2) NOT NULL DEFAULT 0,
    MaHDN      VARCHAR(10)   NULL FOREIGN KEY REFERENCES HoaDonNhap(MaHDN),   -- NULL = tồn đầu kỳ
    NgayNhap   DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT CK_LoThuoc_SL CHECK (SLNhap >= 0 AND SLCon >= 0)
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LoThuoc_FEFO' AND object_id = OBJECT_ID('LoThuoc'))
    CREATE INDEX IX_LoThuoc_FEFO ON LoThuoc(MaThuoc, HanSD) INCLUDE (SLCon);
GO

-- 19. Hóa đơn bán lấy hàng từ lô nào (để trả hàng về đúng lô)
IF OBJECT_ID('ChiTietHDB_Lo', 'U') IS NULL
CREATE TABLE ChiTietHDB_Lo (
    MaHDB   VARCHAR(10) NOT NULL,
    MaThuoc VARCHAR(10) NOT NULL,
    MaLo    INT         NOT NULL FOREIGN KEY REFERENCES LoThuoc(MaLo),
    SoLuong INT         NOT NULL,
    PRIMARY KEY (MaHDB, MaThuoc, MaLo),
    CONSTRAINT FK_ChiTietHDB_Lo_ChiTietHDB FOREIGN KEY (MaHDB, MaThuoc) REFERENCES ChiTietHDB(MaHDB, MaThuoc)
);

-- 20. Phiếu trả hàng (trả 1 phần) / hủy hóa đơn (trả toàn bộ, LaHuyHoaDon = 1)
IF OBJECT_ID('PhieuTraHang', 'U') IS NULL
CREATE TABLE PhieuTraHang (
    MaPTH       VARCHAR(10)   PRIMARY KEY,
    MaHDB       VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES HoaDonBan(MaHDB),
    NgayTra     DATETIME      NOT NULL DEFAULT GETDATE(),
    MaNV        VARCHAR(10)   NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    LyDo        NVARCHAR(255) NULL,
    TienHoan    DECIMAL(18,2) NOT NULL DEFAULT 0,
    DiemTru     INT           NOT NULL DEFAULT 0,   -- điểm đã cộng bị trừ lại
    DiemHoan    INT           NOT NULL DEFAULT 0,   -- điểm đã dùng được trả lại (hủy cả hóa đơn)
    LaHuyHoaDon BIT           NOT NULL DEFAULT 0
);

IF OBJECT_ID('ChiTietTraHang', 'U') IS NULL
CREATE TABLE ChiTietTraHang (
    MaPTH    VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES PhieuTraHang(MaPTH),
    MaThuoc  VARCHAR(10)   NOT NULL FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    SoLuong  INT           NOT NULL,
    TienHoan DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaPTH, MaThuoc)
);

-- 21. Nhật ký hoạt động (không khóa ngoại MaNV để xóa NV không bị vướng nhật ký)
IF OBJECT_ID('NhatKyHoatDong', 'U') IS NULL
CREATE TABLE NhatKyHoatDong (
    MaNK     BIGINT IDENTITY(1,1) PRIMARY KEY,
    ThoiGian DATETIME      NOT NULL DEFAULT GETDATE(),
    MaNV     VARCHAR(10)   NULL,
    TenNV    NVARCHAR(50)  NULL,
    HanhDong NVARCHAR(50)  NOT NULL,     -- Đăng nhập, Sửa giá, Thanh toán, Trả hàng...
    DoiTuong NVARCHAR(50)  NULL,         -- mã thuốc / mã hóa đơn / mã NV...
    ChiTiet  NVARCHAR(500) NULL,
    MayTinh  NVARCHAR(100) NULL
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_NhatKy_ThoiGian' AND object_id = OBJECT_ID('NhatKyHoatDong'))
    CREATE INDEX IX_NhatKy_ThoiGian ON NhatKyHoatDong(ThoiGian DESC);
GO


/* =====================================================================
   PHẦN 3. TRIGGER: TongTien = SUM(ThanhTien) của chi tiết
   + [10/11/2026 lần 5] trigger nhật ký đổi giá thuốc
   (sửa lỗi báo cáo Top 2 NV ra 0.00 vì TongTien không được cập nhật)
   ===================================================================== */
IF OBJECT_ID('trg_ChiTietHDB_CapNhatTongTien', 'TR') IS NOT NULL
    DROP TRIGGER trg_ChiTietHDB_CapNhatTongTien;
GO
CREATE TRIGGER trg_ChiTietHDB_CapNhatTongTien
ON ChiTietHDB
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE h
    SET TongTien = ISNULL((SELECT SUM(c.ThanhTien) FROM ChiTietHDB c WHERE c.MaHDB = h.MaHDB), 0)
    FROM HoaDonBan h
    WHERE h.MaHDB IN (SELECT MaHDB FROM inserted UNION SELECT MaHDB FROM deleted);
END
GO

IF OBJECT_ID('trg_ChiTietHDN_CapNhatTongTien', 'TR') IS NOT NULL
    DROP TRIGGER trg_ChiTietHDN_CapNhatTongTien;
GO
CREATE TRIGGER trg_ChiTietHDN_CapNhatTongTien
ON ChiTietHDN
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE h
    SET TongTien = ISNULL((SELECT SUM(c.ThanhTien) FROM ChiTietHDN c WHERE c.MaHDN = h.MaHDN), 0)
    FROM HoaDonNhap h
    WHERE h.MaHDN IN (SELECT MaHDN FROM inserted UNION SELECT MaHDN FROM deleted);
END
GO

-- ---------------------------------------------------------------------
-- [10/11/2026 lần 5] Người dùng đang thao tác: app gán mã NV vào CONTEXT_INFO
-- mỗi khi mở kết nối (DAL/DBConnection.cs). Chạy từ SSMS thì trả về NULL.
-- ---------------------------------------------------------------------
IF OBJECT_ID('fn_NguoiDungHienTai', 'FN') IS NOT NULL
    DROP FUNCTION fn_NguoiDungHienTai;
GO
CREATE FUNCTION fn_NguoiDungHienTai()
RETURNS VARCHAR(10)
AS
BEGIN
    DECLARE @Ma VARCHAR(128) = CAST(CONTEXT_INFO() AS VARCHAR(128));
    SET @Ma = LTRIM(RTRIM(REPLACE(ISNULL(@Ma, ''), CHAR(0), '')));
    RETURN CASE WHEN @Ma = '' THEN NULL ELSE LEFT(@Ma, 10) END;
END
GO

-- ---------------------------------------------------------------------
-- [10/11/2026 lần 5] Nhật ký đổi giá: mỗi lần DonGiaNhap / GiaBan thay đổi
-- (sửa ở Danh mục thuốc, nhập hàng, import Excel...) đều được ghi lại.
-- ---------------------------------------------------------------------
IF OBJECT_ID('trg_Thuoc_NhatKyGia', 'TR') IS NOT NULL
    DROP TRIGGER trg_Thuoc_NhatKyGia;
GO
CREATE TRIGGER trg_Thuoc_NhatKyGia
ON Thuoc
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT (UPDATE(GiaBan) OR UPDATE(DonGiaNhap)) RETURN;

    DECLARE @MaNV VARCHAR(10) = dbo.fn_NguoiDungHienTai();

    INSERT INTO NhatKyHoatDong (ThoiGian, MaNV, TenNV, HanhDong, DoiTuong, ChiTiet, MayTinh)
    SELECT GETDATE(), @MaNV, nv.TenNV, N'Đổi giá thuốc', i.MaThuoc,
           i.TenThuoc + N': giá nhập ' + FORMAT(d.DonGiaNhap, 'N0') + N' -> ' + FORMAT(i.DonGiaNhap, 'N0')
                      + N', giá bán ' + FORMAT(d.GiaBan, 'N0') + N' -> ' + FORMAT(i.GiaBan, 'N0'),
           HOST_NAME()
    FROM inserted i
    JOIN deleted d ON d.MaThuoc = i.MaThuoc
    LEFT JOIN NhanVien nv ON nv.MaNV = @MaNV
    WHERE i.GiaBan <> d.GiaBan OR i.DonGiaNhap <> d.DonGiaNhap;
END
GO


/* =====================================================================
   PHẦN 4. STORED PROCEDURE
   Yêu cầu 1, 2, 3 xử lý trong C# (DAL: HoaDonBanDAO, HoaDonNhapDAO).
   ===================================================================== */

-- =====================================================================
-- [10/11/2026 lần 5] LÔ THUỐC - FEFO (First Expired, First Out)
--   Thuoc.SLHienCo = SUM(LoThuoc.SLCon)
--   Thuoc.HanSD    = hạn gần nhất của lô còn hàng (hết hàng: hạn của lô mới nhất)
-- =====================================================================

-- Số lượng CÒN HẠN bán được của 1 thuốc tại ngày @Ngay
IF OBJECT_ID('fn_SLBanDuoc', 'FN') IS NOT NULL
    DROP FUNCTION fn_SLBanDuoc;
GO
CREATE FUNCTION fn_SLBanDuoc (@MaThuoc VARCHAR(10), @Ngay DATE)
RETURNS INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM LoThuoc WHERE MaThuoc = @MaThuoc)
        RETURN (SELECT ISNULL(SUM(SLCon), 0) FROM LoThuoc
                WHERE MaThuoc = @MaThuoc AND SLCon > 0 AND HanSD >= @Ngay);
    -- thuốc chưa có lô (CSDL cũ chưa chạy Phần 6): dùng số liệu ở bảng Thuoc
    RETURN ISNULL((SELECT CASE WHEN HanSD IS NULL OR HanSD >= @Ngay THEN SLHienCo ELSE 0 END
                   FROM Thuoc WHERE MaThuoc = @MaThuoc), 0);
END
GO

-- Cập nhật SLHienCo + HanSD của thuốc theo các lô (@MaThuoc NULL = tất cả thuốc có lô)
IF OBJECT_ID('sp_DongBoThuocTuLo', 'P') IS NOT NULL
    DROP PROCEDURE sp_DongBoThuocTuLo;
GO
CREATE PROCEDURE sp_DongBoThuocTuLo
    @MaThuoc VARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE t
    SET SLHienCo = l.TongCon,
        HanSD    = ISNULL(l.HanGanNhat, l.HanMoiNhat)
    FROM Thuoc t
    JOIN (SELECT MaThuoc,
                 SUM(SLCon)                              AS TongCon,
                 MIN(CASE WHEN SLCon > 0 THEN HanSD END) AS HanGanNhat,
                 MAX(HanSD)                              AS HanMoiNhat
          FROM LoThuoc
          WHERE @MaThuoc IS NULL OR MaThuoc = @MaThuoc
          GROUP BY MaThuoc) l ON l.MaThuoc = t.MaThuoc
    WHERE t.SLHienCo <> l.TongCon
       OR ISNULL(t.HanSD, '19000101') <> ISNULL(l.HanGanNhat, l.HanMoiNhat);
END
GO

-- Tồn kho có từ trước khi quản lý theo lô -> gom thành 1 lô "TON-DAU"
-- (@MaThuoc NULL = tất cả thuốc). Chạy lại nhiều lần không bị nhân đôi.
IF OBJECT_ID('sp_TaoLoTonDau', 'P') IS NOT NULL
    DROP PROCEDURE sp_TaoLoTonDau;
GO
CREATE PROCEDURE sp_TaoLoTonDau
    @MaThuoc VARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO LoThuoc (MaThuoc, SoLo, NgaySX, HanSD, SLNhap, SLCon, DonGiaNhap, MaHDN, NgayNhap)
    SELECT t.MaThuoc, N'TON-DAU', t.NgaySX,
           ISNULL(t.HanSD, DATEADD(YEAR, 2, CAST(GETDATE() AS DATE))),
           t.SLHienCo - ISNULL(l.TongCon, 0), t.SLHienCo - ISNULL(l.TongCon, 0),
           t.DonGiaNhap, NULL, GETDATE()
    FROM Thuoc t
    LEFT JOIN (SELECT MaThuoc, SUM(SLCon) AS TongCon FROM LoThuoc GROUP BY MaThuoc) l ON l.MaThuoc = t.MaThuoc
    WHERE (@MaThuoc IS NULL OR t.MaThuoc = @MaThuoc)
      AND t.SLHienCo > ISNULL(l.TongCon, 0);
END
GO

-- Nhập 1 lô (gọi từ C# trong transaction nhập hàng / import Excel)
IF OBJECT_ID('sp_NhapLo', 'P') IS NOT NULL
    DROP PROCEDURE sp_NhapLo;
GO
CREATE PROCEDURE sp_NhapLo
    @MaThuoc    VARCHAR(10),
    @SoLo       NVARCHAR(30)  = NULL,
    @NgaySX     DATE          = NULL,
    @HanSD      DATE,
    @SoLuong    INT,
    @DonGiaNhap DECIMAL(18,2) = 0,
    @MaHDN      VARCHAR(10)   = NULL,
    @NgayNhap   DATETIME      = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @SoLuong <= 0
    BEGIN
        RAISERROR(N'Số lượng nhập phải lớn hơn 0.', 16, 1);
        RETURN;
    END
    IF @HanSD IS NULL
    BEGIN
        RAISERROR(N'Lô thuốc %s chưa có hạn sử dụng.', 16, 1, @MaThuoc);
        RETURN;
    END
    IF @NgaySX IS NOT NULL AND @HanSD <= @NgaySX
    BEGIN
        RAISERROR(N'Hạn sử dụng của lô thuốc %s phải sau ngày sản xuất.', 16, 1, @MaThuoc);
        RETURN;
    END

    -- tồn kho cũ chưa có lô thì gom thành lô TON-DAU trước, tránh lệch số lượng
    EXEC sp_TaoLoTonDau @MaThuoc;

    SET @SoLo = NULLIF(LTRIM(RTRIM(@SoLo)), N'');
    IF @SoLo IS NULL
        SET @SoLo = ISNULL(@MaHDN, 'LO') + N'-' + CONVERT(NVARCHAR(8), @HanSD, 112);

    INSERT INTO LoThuoc (MaThuoc, SoLo, NgaySX, HanSD, SLNhap, SLCon, DonGiaNhap, MaHDN, NgayNhap)
    VALUES (@MaThuoc, @SoLo, @NgaySX, @HanSD, @SoLuong, @SoLuong, ISNULL(@DonGiaNhap, 0), @MaHDN, ISNULL(@NgayNhap, GETDATE()));

    EXEC sp_DongBoThuocTuLo @MaThuoc;
END
GO

-- Xuất kho cho 1 dòng hóa đơn bán theo FEFO: lô có hạn gần nhất (còn hạn) xuất trước.
-- Dòng ChiTietHDB phải được thêm TRƯỚC khi gọi proc này (khóa ngoại ChiTietHDB_Lo).
IF OBJECT_ID('sp_XuatKhoFEFO', 'P') IS NOT NULL
    DROP PROCEDURE sp_XuatKhoFEFO;
GO
CREATE PROCEDURE sp_XuatKhoFEFO
    @MaHDB   VARCHAR(10),
    @MaThuoc VARCHAR(10),
    @SoLuong INT,
    @NgayBan DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Ngay DATE = CAST(ISNULL(@NgayBan, GETDATE()) AS DATE);
    DECLARE @ConLai INT = @SoLuong, @MaLo INT, @Co INT, @Lay INT, @CoTheBan INT;

    EXEC sp_TaoLoTonDau @MaThuoc;

    SELECT @CoTheBan = ISNULL(SUM(SLCon), 0)
    FROM LoThuoc WITH (UPDLOCK, HOLDLOCK)
    WHERE MaThuoc = @MaThuoc AND SLCon > 0 AND HanSD >= @Ngay;

    IF @CoTheBan < @SoLuong
    BEGIN
        RAISERROR(N'Thuốc %s chỉ còn %d (còn hạn) - không đủ để bán %d.', 16, 1, @MaThuoc, @CoTheBan, @SoLuong);
        RETURN;
    END

    WHILE @ConLai > 0
    BEGIN
        SET @MaLo = NULL;
        SELECT TOP 1 @MaLo = MaLo, @Co = SLCon
        FROM LoThuoc WITH (UPDLOCK)
        WHERE MaThuoc = @MaThuoc AND SLCon > 0 AND HanSD >= @Ngay
        ORDER BY HanSD, MaLo;

        IF @MaLo IS NULL BREAK;

        SET @Lay = CASE WHEN @Co < @ConLai THEN @Co ELSE @ConLai END;

        UPDATE LoThuoc SET SLCon = SLCon - @Lay WHERE MaLo = @MaLo;

        IF EXISTS (SELECT 1 FROM ChiTietHDB_Lo WHERE MaHDB = @MaHDB AND MaThuoc = @MaThuoc AND MaLo = @MaLo)
            UPDATE ChiTietHDB_Lo SET SoLuong = SoLuong + @Lay
            WHERE MaHDB = @MaHDB AND MaThuoc = @MaThuoc AND MaLo = @MaLo;
        ELSE
            INSERT INTO ChiTietHDB_Lo (MaHDB, MaThuoc, MaLo, SoLuong) VALUES (@MaHDB, @MaThuoc, @MaLo, @Lay);

        SET @ConLai = @ConLai - @Lay;
    END

    EXEC sp_DongBoThuocTuLo @MaThuoc;
END
GO

-- Trả 1 dòng hàng: giảm SL / thành tiền của chi tiết hóa đơn, trả hàng về đúng lô đã xuất
-- (lô hạn xa nhất trước), ghi ChiTietTraHang. @TienHoan trả về số tiền hoàn cho dòng này.
-- Phiếu PhieuTraHang (@MaPTH) phải được tạo trước (C# làm trong cùng transaction).
IF OBJECT_ID('sp_TraHangDong', 'P') IS NOT NULL
    DROP PROCEDURE sp_TraHangDong;
GO
CREATE PROCEDURE sp_TraHangDong
    @MaPTH    VARCHAR(10),
    @MaHDB    VARCHAR(10),
    @MaThuoc  VARCHAR(10),
    @SoLuong  INT,
    @TienHoan DECIMAL(18,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @SLBan INT, @ThanhTien DECIMAL(18,2), @ConLai INT, @MaLo INT, @Co INT, @Lay INT;

    SELECT @SLBan = SoLuong, @ThanhTien = ThanhTien
    FROM ChiTietHDB WITH (UPDLOCK)
    WHERE MaHDB = @MaHDB AND MaThuoc = @MaThuoc;

    IF @SLBan IS NULL
    BEGIN
        RAISERROR(N'Hóa đơn %s không có thuốc %s.', 16, 1, @MaHDB, @MaThuoc);
        RETURN;
    END
    IF @SoLuong <= 0 OR @SoLuong > @SLBan
    BEGIN
        RAISERROR(N'Số lượng trả của thuốc %s không hợp lệ (còn %d trên hóa đơn).', 16, 1, @MaThuoc, @SLBan);
        RETURN;
    END

    -- tiền hoàn theo đơn giá THỰC đã trả (đã trừ giảm giá %, đổi điểm)
    SET @TienHoan = CASE WHEN @SoLuong = @SLBan THEN @ThanhTien
                         ELSE ROUND(@ThanhTien * @SoLuong / @SLBan, 0) END;

    UPDATE ChiTietHDB
    SET SoLuong = SoLuong - @SoLuong, ThanhTien = ThanhTien - @TienHoan
    WHERE MaHDB = @MaHDB AND MaThuoc = @MaThuoc;

    -- trả về các lô đã xuất
    SET @ConLai = @SoLuong;
    WHILE @ConLai > 0
    BEGIN
        SET @MaLo = NULL;
        SELECT TOP 1 @MaLo = x.MaLo, @Co = x.SoLuong
        FROM ChiTietHDB_Lo x
        JOIN LoThuoc l ON l.MaLo = x.MaLo
        WHERE x.MaHDB = @MaHDB AND x.MaThuoc = @MaThuoc AND x.SoLuong > 0
        ORDER BY l.HanSD DESC, x.MaLo DESC;

        IF @MaLo IS NULL BREAK;

        SET @Lay = CASE WHEN @Co < @ConLai THEN @Co ELSE @ConLai END;
        UPDATE LoThuoc SET SLCon = SLCon + @Lay WHERE MaLo = @MaLo;
        UPDATE ChiTietHDB_Lo SET SoLuong = SoLuong - @Lay
        WHERE MaHDB = @MaHDB AND MaThuoc = @MaThuoc AND MaLo = @MaLo;
        SET @ConLai = @ConLai - @Lay;
    END
    DELETE FROM ChiTietHDB_Lo WHERE MaHDB = @MaHDB AND MaThuoc = @MaThuoc AND SoLuong <= 0;

    -- hóa đơn bán trước khi có lô (không biết lô nào): nhập lại thành lô riêng
    IF @ConLai > 0
        INSERT INTO LoThuoc (MaThuoc, SoLo, NgaySX, HanSD, SLNhap, SLCon, DonGiaNhap, MaHDN, NgayNhap)
        SELECT MaThuoc, N'TRA-' + @MaPTH, NgaySX,
               ISNULL(HanSD, DATEADD(YEAR, 1, CAST(GETDATE() AS DATE))),
               @ConLai, @ConLai, DonGiaNhap, NULL, GETDATE()
        FROM Thuoc WHERE MaThuoc = @MaThuoc;

    INSERT INTO ChiTietTraHang (MaPTH, MaThuoc, SoLuong, TienHoan)
    VALUES (@MaPTH, @MaThuoc, @SoLuong, @TienHoan);

    EXEC sp_DongBoThuocTuLo @MaThuoc;
END
GO

-- Danh sách lô: theo 1 thuốc (@MaThuoc) hoặc tất cả; @ChiConHang = 1 bỏ lô đã hết.
--   ThuTuFEFO: thứ tự sẽ bán (1 = lô bán tiếp theo); lô hết hạn / hết hàng = NULL
--   MucCanhBao: 2 hết hạn (đỏ) | 1 sắp hết hạn (vàng) | 0 bình thường
IF OBJECT_ID('sp_DanhSachLoThuoc', 'P') IS NOT NULL
    DROP PROCEDURE sp_DanhSachLoThuoc;
GO
CREATE PROCEDURE sp_DanhSachLoThuoc
    @MaThuoc       VARCHAR(10) = NULL,
    @ChiConHang    BIT = 0,
    @SoNgayCanhBao INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @HomNay     DATE = CAST(GETDATE() AS DATE);
    DECLARE @MocCanhBao DATE = DATEADD(DAY, @SoNgayCanhBao, @HomNay);

    SELECT
        l.MaLo,
        l.MaThuoc,
        t.TenThuoc,
        l.SoLo,
        l.NgaySX,
        l.HanSD,
        DATEDIFF(DAY, @HomNay, l.HanSD) AS SoNgayConLai,
        l.SLNhap,
        l.SLCon,
        l.DonGiaNhap,
        l.SLCon * l.DonGiaNhap AS GiaTriTon,
        l.MaHDN,
        l.NgayNhap,
        CASE WHEN l.SLCon > 0 AND l.HanSD >= @HomNay
             THEN ROW_NUMBER() OVER (PARTITION BY l.MaThuoc,
                                     CASE WHEN l.SLCon > 0 AND l.HanSD >= @HomNay THEN 1 ELSE 0 END
                                     ORDER BY l.HanSD, l.MaLo)
        END AS ThuTuFEFO,
        CASE WHEN l.SLCon = 0              THEN N'Đã hết'
             WHEN l.HanSD < @HomNay        THEN N'Hết hạn'
             WHEN l.HanSD <= @MocCanhBao   THEN N'Sắp hết hạn'
             ELSE N'Còn hạn' END AS TinhTrang,
        CASE WHEN l.SLCon > 0 AND l.HanSD < @HomNay      THEN 2
             WHEN l.SLCon > 0 AND l.HanSD <= @MocCanhBao THEN 1
             ELSE 0 END AS MucCanhBao
    FROM LoThuoc l
    JOIN Thuoc t ON t.MaThuoc = l.MaThuoc
    WHERE (@MaThuoc IS NULL OR l.MaThuoc = @MaThuoc)
      AND (@ChiConHang = 0 OR l.SLCon > 0)
    ORDER BY l.MaThuoc, CASE WHEN l.SLCon > 0 THEN 0 ELSE 1 END, l.HanSD, l.MaLo;
END
GO

-- ---------------------------------------------------------------------
-- [10/11/2026 lần 5] Đặt mật khẩu (băm SHA-256 + salt ngẫu nhiên).
-- App C# tự băm (BLL/BaoMatMatKhau.cs); proc này dùng khi cần đặt lại
-- mật khẩu trực tiếp trong SSMS, vd: EXEC sp_DatMatKhau 'QL001', N'QL00111';
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_DatMatKhau', 'P') IS NOT NULL
    DROP PROCEDURE sp_DatMatKhau;
GO
CREATE PROCEDURE sp_DatMatKhau
    @MaNV    VARCHAR(10),
    @MatKhau NVARCHAR(50),
    @PhaiDoi BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Salt VARCHAR(32) = CONVERT(VARCHAR(32), CRYPT_GEN_RANDOM(16), 2);
    UPDATE NhanVien
    SET MatKhauSalt = @Salt,
        MatKhauHash = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', CAST(@Salt AS NVARCHAR(32)) + @MatKhau), 2),
        PhaiDoiMK   = @PhaiDoi,
        YeuCauDoiMK = 0,
        NgayYeuCauDoiMK = NULL
    WHERE MaNV = @MaNV;
END
GO

-- ---------------------------------------------------------------------
-- Yêu cầu 4: tìm thuốc theo tên thuốc, thành phần hoặc công dụng
--   * @TuKhoa rỗng / NULL -> trả về TẤT CẢ thuốc (form Tra cứu dùng proc này
--     để tải danh sách, nhờ vậy luôn có cột CongDung).
--   * Cột CongDung: tên các công dụng của thuốc, nối bằng ", ".
--   * Tìm trong: TenThuoc, ThanhPhan, CongDung.TenCongDung, Thuoc_CongDung.GhiChu
--   [10/11/2026 lần 2] viết lại - xem LỊCH SỬ CẬP NHẬT ở đầu file.
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_TimKiemThuoc', 'P') IS NOT NULL
    DROP PROCEDURE sp_TimKiemThuoc;
GO
CREATE PROCEDURE sp_TimKiemThuoc
    @TuKhoa NVARCHAR(100) = N''
AS
BEGIN
    SET NOCOUNT ON;
    SET @TuKhoa = LTRIM(RTRIM(ISNULL(@TuKhoa, N'')));

    SELECT
        t.*,
        dbo.fn_SLBanDuoc(t.MaThuoc, CAST(GETDATE() AS DATE)) AS SLBanDuoc,   -- [10/11/2026 lần 5]
        STUFF((
            SELECT N', ' + cd.TenCongDung
            FROM Thuoc_CongDung tcd
            JOIN CongDung cd ON cd.MaCongDung = tcd.MaCongDung
            WHERE tcd.MaThuoc = t.MaThuoc
            ORDER BY cd.MaCongDung
            FOR XML PATH(''), TYPE
        ).value('.', 'NVARCHAR(MAX)'), 1, 2, N'') AS CongDung
    FROM Thuoc t
    WHERE @TuKhoa = N''
       OR t.TenThuoc  LIKE N'%' + @TuKhoa + N'%'
       OR t.ThanhPhan LIKE N'%' + @TuKhoa + N'%'
       OR EXISTS (
            SELECT 1
            FROM Thuoc_CongDung tcd
            JOIN CongDung cd ON cd.MaCongDung = tcd.MaCongDung
            WHERE tcd.MaThuoc = t.MaThuoc
              AND (   cd.TenCongDung LIKE N'%' + @TuKhoa + N'%'
                   OR tcd.GhiChu     LIKE N'%' + @TuKhoa + N'%')
          )
    ORDER BY t.MaThuoc;
END
GO

-- ---------------------------------------------------------------------
-- Yêu cầu 5: tìm hóa đơn nhập theo mã hàng, NV thực hiện, nhà cung cấp
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_TimKiemHoaDonNhap', 'P') IS NOT NULL
    DROP PROCEDURE sp_TimKiemHoaDonNhap;
GO
CREATE PROCEDURE sp_TimKiemHoaDonNhap
    @TuKhoa NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF @TuKhoa IS NULL SET @TuKhoa = N'';   -- từ khóa trống -> lấy tất cả

    SELECT DISTINCT
        hd.MaHDN,
        hd.NgayNhap,
        hd.MaNV,
        nv.TenNV,
        hd.MaNCC,
        ncc.TenNCC,
        hd.TongTien
    FROM HoaDonNhap hd
    LEFT JOIN NhanVien nv    ON hd.MaNV  = nv.MaNV
    LEFT JOIN NhaCungCap ncc ON hd.MaNCC = ncc.MaNCC
    LEFT JOIN ChiTietHDN ct  ON hd.MaHDN = ct.MaHDN
    WHERE hd.MaHDN   LIKE '%' + @TuKhoa + '%'
       OR hd.MaNV    LIKE '%' + @TuKhoa + '%'
       OR nv.TenNV   LIKE N'%' + @TuKhoa + N'%'
       OR hd.MaNCC   LIKE '%' + @TuKhoa + '%'
       OR ncc.TenNCC LIKE N'%' + @TuKhoa + N'%'
       OR ct.MaThuoc LIKE '%' + @TuKhoa + '%'
    ORDER BY hd.NgayNhap DESC;
END
GO

-- ---------------------------------------------------------------------
-- Yêu cầu 6: thuốc còn hạn / hết hạn, sắp xếp theo hạn SD ngắn nhất
--   @TrangThai : -1 tất cả | 1 còn hạn | 0 đã hết hạn
--   MucCanhBao : 2 = đã hết hạn (tô đỏ) | 1 = sắp hết hạn (tô vàng) | 0 = bình thường
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_BaoCaoHanSuDung', 'P') IS NOT NULL
    DROP PROCEDURE sp_BaoCaoHanSuDung;
GO
CREATE PROCEDURE sp_BaoCaoHanSuDung
    @TrangThai     INT = -1,
    @SoNgayCanhBao INT = 30     -- còn <= số ngày này thì tính là "sắp hết hạn"
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @HomNay     DATE = CAST(GETDATE() AS DATE);
    DECLARE @MocCanhBao DATE = DATEADD(DAY, @SoNgayCanhBao, @HomNay);

    SELECT
        ROW_NUMBER() OVER (ORDER BY CASE WHEN t.HanSD IS NULL THEN 1 ELSE 0 END, t.HanSD, t.TenThuoc) AS STT,
        t.MaThuoc,
        t.TenThuoc,
        dvt.TenDVT,
        t.SLHienCo,
        t.GiaBan,
        t.NgaySX,
        t.HanSD,
        DATEDIFF(DAY, @HomNay, t.HanSD) AS SoNgayConLai,
        CASE
            WHEN t.HanSD IS NULL        THEN N'Chưa có hạn SD'
            WHEN t.HanSD <  @HomNay     THEN N'Đã hết hạn'
            WHEN t.HanSD <= @MocCanhBao THEN N'Sắp hết hạn'
            ELSE N'Còn hạn'
        END AS TinhTrang,
        CASE
            WHEN t.HanSD IS NULL        THEN 0
            WHEN t.HanSD <  @HomNay     THEN 2
            WHEN t.HanSD <= @MocCanhBao THEN 1
            ELSE 0
        END AS MucCanhBao
    FROM Thuoc t
    LEFT JOIN DonViTinh dvt ON t.MaDVT = dvt.MaDVT
    WHERE @TrangThai = -1
       OR (@TrangThai = 1 AND t.HanSD >= @HomNay)
       OR (@TrangThai = 0 AND t.HanSD <  @HomNay)
    ORDER BY STT;
END
GO

-- ---------------------------------------------------------------------
-- Yêu cầu 7: danh sách HĐ nhập và tổng tiền nhập theo tháng/năm chọn
--   Result set 1: danh sách | Result set 2: SoHoaDon, TongTienNhapThang
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_BaoCaoNhapHangTheoThang', 'P') IS NOT NULL
    DROP PROCEDURE sp_BaoCaoNhapHangTheoThang;
GO
CREATE PROCEDURE sp_BaoCaoNhapHangTheoThang
    @Thang INT,
    @Nam   INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @Thang NOT BETWEEN 1 AND 12 OR @Nam NOT BETWEEN 1900 AND 9999
    BEGIN
        RAISERROR(N'Tháng hoặc năm không hợp lệ.', 16, 1);
        RETURN;
    END

    DECLARE @TuNgay  DATETIME = DATEFROMPARTS(@Nam, @Thang, 1);
    DECLARE @DenNgay DATETIME = DATEADD(MONTH, 1, @TuNgay);

    SELECT
        ROW_NUMBER() OVER (ORDER BY h.NgayNhap, h.MaHDN) AS STT,
        h.MaHDN,
        h.NgayNhap,
        h.MaNV,
        nv.TenNV,
        h.MaNCC,
        ncc.TenNCC,
        ISNULL(ct.SoMatHang, 0) AS SoMatHang,
        ISNULL(ct.TongSL, 0)    AS TongSLNhap,
        ISNULL(ct.TongCT, ISNULL(h.TongTien, 0)) AS TongTien
    INTO #DS
    FROM HoaDonNhap h
    LEFT JOIN NhanVien nv    ON h.MaNV  = nv.MaNV
    LEFT JOIN NhaCungCap ncc ON h.MaNCC = ncc.MaNCC
    LEFT JOIN (SELECT MaHDN, COUNT(*) AS SoMatHang, SUM(SLNhap) AS TongSL, SUM(ThanhTien) AS TongCT
               FROM ChiTietHDN GROUP BY MaHDN) ct ON ct.MaHDN = h.MaHDN
    WHERE h.NgayNhap >= @TuNgay AND h.NgayNhap < @DenNgay;

    SELECT * FROM #DS ORDER BY STT;

    SELECT COUNT(*) AS SoHoaDon, ISNULL(SUM(TongTien), 0) AS TongTienNhapThang FROM #DS;
END
GO

-- ---------------------------------------------------------------------
-- Yêu cầu 8: 5 hóa đơn bán có tổng tiền ít nhất theo năm chọn
--   (bỏ qua hóa đơn rỗng: không có chi tiết và tổng tiền = 0)
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_BaoCaoTop5HoaDonBanItNhatTheoNam', 'P') IS NOT NULL
    DROP PROCEDURE sp_BaoCaoTop5HoaDonBanItNhatTheoNam;
GO
CREATE PROCEDURE sp_BaoCaoTop5HoaDonBanItNhatTheoNam
    @Nam INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TuNgay  DATETIME = DATEFROMPARTS(@Nam, 1, 1);
    DECLARE @DenNgay DATETIME = DATEADD(YEAR, 1, @TuNgay);

    SELECT TOP 5
        ROW_NUMBER() OVER (ORDER BY x.TongTien, x.NgayBan, x.MaHDB) AS STT,
        x.*
    FROM (
        SELECT
            h.MaHDB,
            h.NgayBan,
            h.MaNV,
            nv.TenNV,
            h.MaKhach,
            kh.TenKhach,
            ISNULL(ct.TongSL, 0) AS TongSLBan,
            ISNULL(ct.TongCT, ISNULL(h.TongTien, 0)) AS TongTien
        FROM HoaDonBan h
        LEFT JOIN NhanVien nv  ON h.MaNV    = nv.MaNV
        LEFT JOIN KhachHang kh ON h.MaKhach = kh.MaKhach
        LEFT JOIN (SELECT MaHDB, SUM(SoLuong) AS TongSL, SUM(ThanhTien) AS TongCT
                   FROM ChiTietHDB GROUP BY MaHDB) ct ON ct.MaHDB = h.MaHDB
        WHERE h.NgayBan >= @TuNgay AND h.NgayBan < @DenNgay AND h.DaHuy = 0
          AND (ct.MaHDB IS NOT NULL OR ISNULL(h.TongTien, 0) > 0)
    ) x
    ORDER BY STT;
END
GO

-- ---------------------------------------------------------------------
-- Yêu cầu 9: họ tên, tổng tiền của 2 NV bán được ít tiền nhất theo quý/năm
--   @ChiTinhNVCoBan = 1: chỉ xét NV có ít nhất 1 hóa đơn trong quý
--   (bỏ qua quản lý / thủ kho không bán hàng)
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_BaoCaoTop2NhanVienBanItNhatTheoQuy', 'P') IS NOT NULL
    DROP PROCEDURE sp_BaoCaoTop2NhanVienBanItNhatTheoQuy;
GO
CREATE PROCEDURE sp_BaoCaoTop2NhanVienBanItNhatTheoQuy
    @Quy            INT,
    @Nam            INT,
    @ChiTinhNVCoBan BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    IF @Quy NOT BETWEEN 1 AND 4
    BEGIN
        RAISERROR(N'Quý không hợp lệ (1-4).', 16, 1);
        RETURN;
    END

    DECLARE @TuNgay  DATETIME = DATEFROMPARTS(@Nam, (@Quy - 1) * 3 + 1, 1);
    DECLARE @DenNgay DATETIME = DATEADD(MONTH, 3, @TuNgay);

    ;WITH HD AS (
        SELECT h.MaHDB, h.MaNV,
               ISNULL(ct.TongCT, ISNULL(h.TongTien, 0)) AS TongTien
        FROM HoaDonBan h
        LEFT JOIN (SELECT MaHDB, SUM(ThanhTien) AS TongCT
                   FROM ChiTietHDB GROUP BY MaHDB) ct ON ct.MaHDB = h.MaHDB
        WHERE h.NgayBan >= @TuNgay AND h.NgayBan < @DenNgay AND h.DaHuy = 0
          AND (ct.MaHDB IS NOT NULL OR ISNULL(h.TongTien, 0) > 0)
    ),
    DT AS (
        SELECT nv.MaNV, nv.TenNV,
               COUNT(HD.MaHDB)             AS SoHoaDon,
               ISNULL(SUM(HD.TongTien), 0) AS TongDoanhThu
        FROM NhanVien nv
        LEFT JOIN HD ON HD.MaNV = nv.MaNV
        GROUP BY nv.MaNV, nv.TenNV
    )
    SELECT TOP 2
        ROW_NUMBER() OVER (ORDER BY TongDoanhThu, MaNV) AS STT,
        MaNV, TenNV, SoHoaDon, TongDoanhThu
    FROM DT
    WHERE @ChiTinhNVCoBan = 0 OR SoHoaDon > 0
    ORDER BY STT;
END
GO

-- ---------------------------------------------------------------------
-- Bổ sung: cảnh báo thuốc sắp hết hàng (SLHienCo < @NguongSL, mặc định 30)
--   MucCanhBao: 2 = hết hàng (SL <= 0, đỏ) | 1 = sắp hết (vàng)
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_CanhBaoThuocSapHet', 'P') IS NOT NULL
    DROP PROCEDURE sp_CanhBaoThuocSapHet;
GO
CREATE PROCEDURE sp_CanhBaoThuocSapHet
    @NguongSL INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        ROW_NUMBER() OVER (ORDER BY t.SLHienCo, t.TenThuoc) AS STT,
        t.MaThuoc,
        t.TenThuoc,
        dvt.TenDVT,
        t.SLHienCo,
        @NguongSL - t.SLHienCo AS SLCanNhapThem,
        t.DonGiaNhap,
        t.HanSD,
        gn.TenNCC AS NCCGanNhat,
        CASE WHEN t.SLHienCo <= 0 THEN N'Hết hàng' ELSE N'Sắp hết' END AS TinhTrang,
        CASE WHEN t.SLHienCo <= 0 THEN 2 ELSE 1 END AS MucCanhBao
    FROM Thuoc t
    LEFT JOIN DonViTinh dvt ON t.MaDVT = dvt.MaDVT
    OUTER APPLY (
        SELECT TOP 1 ncc.TenNCC
        FROM ChiTietHDN c
        JOIN HoaDonNhap h        ON h.MaHDN = c.MaHDN
        LEFT JOIN NhaCungCap ncc ON ncc.MaNCC = h.MaNCC
        WHERE c.MaThuoc = t.MaThuoc
        ORDER BY h.NgayNhap DESC
    ) gn
    WHERE t.SLHienCo < @NguongSL
    ORDER BY STT;
END
GO

-- ---------------------------------------------------------------------
-- Bổ sung: danh sách cảnh báo (trang mở ra khi bấm banner cảnh báo)
--   @Loai: 0 tất cả | 1 hết hạn | 2 cận hạn | 3 hết hàng | 4 gần hết hàng
--   * Hết hạn / cận hạn chỉ tính thuốc còn tồn (SLHienCo > 0); thuốc đã hủy
--     (SL = 0) chuyển sang nhóm "Hết hàng" để nhập lại.
--   * MucCanhBao: 2 = hết hạn hoặc hết hàng (đỏ) | 1 = cận hạn / gần hết (vàng)
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_DanhSachCanhBao', 'P') IS NOT NULL
    DROP PROCEDURE sp_DanhSachCanhBao;
GO
CREATE PROCEDURE sp_DanhSachCanhBao
    @Loai          INT = 0,
    @SoNgayCanhBao INT = 30,
    @NguongSL      INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @HomNay     DATE = CAST(GETDATE() AS DATE);
    DECLARE @MocCanhBao DATE = DATEADD(DAY, @SoNgayCanhBao, @HomNay);

    ;WITH CO AS (
        SELECT t.MaThuoc, t.TenThuoc, dvt.TenDVT, t.SLHienCo, t.DonGiaNhap, t.HanSD,
            CASE WHEN t.SLHienCo > 0 AND t.HanSD < @HomNay THEN 1 ELSE 0 END AS LaHetHan,
            CASE WHEN t.SLHienCo > 0 AND t.HanSD >= @HomNay AND t.HanSD <= @MocCanhBao THEN 1 ELSE 0 END AS LaCanHan,
            CASE WHEN t.SLHienCo <= 0 THEN 1 ELSE 0 END AS LaHetHang,
            CASE WHEN t.SLHienCo > 0 AND t.SLHienCo < @NguongSL THEN 1 ELSE 0 END AS LaGanHetHang
        FROM Thuoc t
        LEFT JOIN DonViTinh dvt ON t.MaDVT = dvt.MaDVT
    ),
    CB AS (
        SELECT CO.*,
            CASE WHEN LaHetHan = 1 OR LaHetHang = 1 THEN 2 ELSE 1 END AS MucCanhBao
        FROM CO
        WHERE (@Loai = 0 AND LaHetHan + LaCanHan + LaHetHang + LaGanHetHang > 0)
           OR (@Loai = 1 AND LaHetHan = 1)
           OR (@Loai = 2 AND LaCanHan = 1)
           OR (@Loai = 3 AND LaHetHang = 1)
           OR (@Loai = 4 AND LaGanHetHang = 1)
    )
    SELECT
        ROW_NUMBER() OVER (ORDER BY CB.MucCanhBao DESC,
                                    CASE WHEN CB.HanSD IS NULL THEN 1 ELSE 0 END, CB.HanSD,
                                    CB.SLHienCo, CB.TenThuoc) AS STT,
        CB.MaThuoc,
        CB.TenThuoc,
        CB.TenDVT,
        STUFF(  CASE WHEN CB.LaHetHan     = 1 THEN N', Hết hạn'      ELSE N'' END
              + CASE WHEN CB.LaCanHan     = 1 THEN N', Cận hạn'      ELSE N'' END
              + CASE WHEN CB.LaHetHang    = 1 THEN N', Hết hàng'     ELSE N'' END
              + CASE WHEN CB.LaGanHetHang = 1 THEN N', Gần hết hàng' ELSE N'' END, 1, 2, N'') AS LoaiCanhBao,
        CB.SLHienCo,
        CB.HanSD,
        DATEDIFF(DAY, @HomNay, CB.HanSD) AS SoNgayConLai,
        CASE WHEN CB.SLHienCo > 0 THEN CB.SLHienCo * CB.DonGiaNhap ELSE 0 END AS GiaTriTon,
        CASE WHEN CB.SLHienCo < @NguongSL THEN @NguongSL - CB.SLHienCo END AS SLCanNhapThem,
        gn.TenNCC AS NCCGanNhat,
        CB.MucCanhBao,
        CB.LaHetHan, CB.LaCanHan, CB.LaHetHang, CB.LaGanHetHang
    FROM CB
    OUTER APPLY (
        SELECT TOP 1 ncc.TenNCC
        FROM ChiTietHDN c
        JOIN HoaDonNhap h        ON h.MaHDN = c.MaHDN
        LEFT JOIN NhaCungCap ncc ON ncc.MaNCC = h.MaNCC
        WHERE c.MaThuoc = CB.MaThuoc
        ORDER BY h.NgayNhap DESC
    ) gn
    ORDER BY STT;
END
GO

-- ---------------------------------------------------------------------
-- Bổ sung: hủy thuốc đã hết hạn còn tồn kho
--   [10/11/2026 lần 5] HỦY THEO LÔ: chỉ đưa SLCon của các LÔ đã hết hạn về 0
--   (lô còn hạn của cùng thuốc vẫn bán bình thường), ghi LichSuHuyThuoc
--   từng lô (MaLo, SoLo), rồi đồng bộ lại Thuoc. Tất cả trong 1 transaction.
--   Trả về: SoThuocHuy, TongSLHuy, TongGiaTriHuy (giống bản cũ -> C# không đổi)
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_HuyThuocHetHan', 'P') IS NOT NULL
    DROP PROCEDURE sp_HuyThuocHetHan;
GO
CREATE PROCEDURE sp_HuyThuocHetHan
    @MaNV VARCHAR(10)   = NULL,
    @LyDo NVARCHAR(255) = N'Hết hạn sử dụng'
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @HomNay DATE = CAST(GETDATE() AS DATE);
    DECLARE @DaHuy TABLE (
        MaLo       INT,
        MaThuoc    VARCHAR(10),
        SoLo       NVARCHAR(30),
        SoLuong    INT,
        HanSD      DATE,
        DonGiaNhap DECIMAL(18,2)
    );

    IF @MaNV IS NOT NULL AND NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = @MaNV)
        SET @MaNV = NULL;

    BEGIN TRANSACTION;

        EXEC sp_TaoLoTonDau NULL;      -- thuốc cũ chưa có lô

        UPDATE LoThuoc
        SET SLCon = 0
        OUTPUT deleted.MaLo, deleted.MaThuoc, deleted.SoLo, deleted.SLCon, deleted.HanSD, deleted.DonGiaNhap
        INTO @DaHuy (MaLo, MaThuoc, SoLo, SoLuong, HanSD, DonGiaNhap)
        WHERE SLCon > 0 AND HanSD < @HomNay;

        INSERT INTO LichSuHuyThuoc (MaThuoc, TenThuoc, SoLuongHuy, HanSD, DonGiaNhap, GiaTriHuy, NgayHuy, MaNV, LyDo, MaLo, SoLo)
        SELECT d.MaThuoc, t.TenThuoc, d.SoLuong, d.HanSD, d.DonGiaNhap, d.SoLuong * d.DonGiaNhap,
               GETDATE(), @MaNV, @LyDo, d.MaLo, d.SoLo
        FROM @DaHuy d
        JOIN Thuoc t ON t.MaThuoc = d.MaThuoc;

        EXEC sp_DongBoThuocTuLo NULL;

    COMMIT TRANSACTION;

    SELECT COUNT(DISTINCT MaThuoc)              AS SoThuocHuy,
           ISNULL(SUM(SoLuong), 0)              AS TongSLHuy,
           ISNULL(SUM(SoLuong * DonGiaNhap), 0) AS TongGiaTriHuy
    FROM @DaHuy;
END
GO


-- =====================================================================
-- [10/11/2026 lần 3] CÁC PROC CHO DASHBOARD / BIỂU ĐỒ SO SÁNH
-- Doanh thu luôn tính = SUM(ChiTietHDB.ThanhTien) giống các báo cáo 6-9.
-- [10/11/2026 lần 5] mọi proc tính doanh thu bỏ hóa đơn đã hủy (h.DaHuy = 0).
-- =====================================================================

-- ---------------------------------------------------------------------
-- Dashboard: các con số tổng quan (trả về 1 dòng)
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_Dashboard_TongQuan', 'P') IS NOT NULL
    DROP PROCEDURE sp_Dashboard_TongQuan;
GO
CREATE PROCEDURE sp_Dashboard_TongQuan
    @NguongSL      INT = 30,
    @SoNgayCanhBao INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @HomNay      DATE = CAST(GETDATE() AS DATE);
    DECLARE @NgayMai     DATE = DATEADD(DAY, 1, @HomNay);
    DECLARE @DauThang    DATE = DATEFROMPARTS(YEAR(@HomNay), MONTH(@HomNay), 1);
    DECLARE @DauThangSau DATE = DATEADD(MONTH, 1, @DauThang);
    DECLARE @MocCanhBao  DATE = DATEADD(DAY, @SoNgayCanhBao, @HomNay);

    SELECT
        (SELECT ISNULL(SUM(c.ThanhTien), 0)
           FROM ChiTietHDB c JOIN HoaDonBan h ON h.MaHDB = c.MaHDB
          WHERE h.NgayBan >= @HomNay AND h.NgayBan < @NgayMai AND h.DaHuy = 0)            AS DoanhThuHomNay,
        (SELECT COUNT(DISTINCT c.MaHDB)
           FROM ChiTietHDB c JOIN HoaDonBan h ON h.MaHDB = c.MaHDB
          WHERE h.NgayBan >= @HomNay AND h.NgayBan < @NgayMai AND h.DaHuy = 0)            AS SoHoaDonHomNay,
        (SELECT ISNULL(SUM(c.ThanhTien), 0)
           FROM ChiTietHDB c JOIN HoaDonBan h ON h.MaHDB = c.MaHDB
          WHERE h.NgayBan >= @DauThang AND h.NgayBan < @DauThangSau AND h.DaHuy = 0)      AS DoanhThuThangNay,
        (SELECT COUNT(DISTINCT c.MaHDB)
           FROM ChiTietHDB c JOIN HoaDonBan h ON h.MaHDB = c.MaHDB
          WHERE h.NgayBan >= @DauThang AND h.NgayBan < @DauThangSau AND h.DaHuy = 0)      AS SoHoaDonThangNay,
        (SELECT ISNULL(SUM(c.ThanhTien), 0)
           FROM ChiTietHDN c JOIN HoaDonNhap h ON h.MaHDN = c.MaHDN
          WHERE h.NgayNhap >= @DauThang AND h.NgayNhap < @DauThangSau)    AS TienNhapThangNay,
        (SELECT COUNT(*) FROM KhachHang)                                  AS SoKhachHang,
        (SELECT COUNT(*) FROM Thuoc
          WHERE SLHienCo < @NguongSL
             OR (SLHienCo > 0 AND HanSD <= @MocCanhBao))                  AS SoThuocCanhBao;
END
GO

-- ---------------------------------------------------------------------
-- Dashboard: doanh thu bán + tiền nhập theo từng tháng của 1 năm (12 dòng)
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_Dashboard_DoanhThuTheoThang', 'P') IS NOT NULL
    DROP PROCEDURE sp_Dashboard_DoanhThuTheoThang;
GO
CREATE PROCEDURE sp_Dashboard_DoanhThuTheoThang
    @Nam INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TuNgay  DATETIME = DATEFROMPARTS(@Nam, 1, 1);
    DECLARE @DenNgay DATETIME = DATEADD(YEAR, 1, @TuNgay);

    ;WITH Thang AS (
        SELECT v.Thang FROM (VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS v (Thang)
    ),
    Ban AS (
        SELECT MONTH(h.NgayBan) AS Thang, SUM(c.ThanhTien) AS DoanhThu, COUNT(DISTINCT h.MaHDB) AS SoHoaDon
        FROM HoaDonBan h JOIN ChiTietHDB c ON c.MaHDB = h.MaHDB
        WHERE h.NgayBan >= @TuNgay AND h.NgayBan < @DenNgay AND h.DaHuy = 0
        GROUP BY MONTH(h.NgayBan)
    ),
    Nhap AS (
        SELECT MONTH(h.NgayNhap) AS Thang, SUM(c.ThanhTien) AS TienNhap
        FROM HoaDonNhap h JOIN ChiTietHDN c ON c.MaHDN = h.MaHDN
        WHERE h.NgayNhap >= @TuNgay AND h.NgayNhap < @DenNgay
        GROUP BY MONTH(h.NgayNhap)
    )
    SELECT t.Thang,
           ISNULL(b.DoanhThu, 0) AS DoanhThu,
           ISNULL(b.SoHoaDon, 0) AS SoHoaDon,
           ISNULL(n.TienNhap, 0) AS TienNhap
    FROM Thang t
    LEFT JOIN Ban  b ON b.Thang = t.Thang
    LEFT JOIN Nhap n ON n.Thang = t.Thang
    ORDER BY t.Thang;
END
GO

-- ---------------------------------------------------------------------
-- Dashboard: top thuốc bán chạy (theo số lượng) trong năm
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_Dashboard_TopThuocBanChay', 'P') IS NOT NULL
    DROP PROCEDURE sp_Dashboard_TopThuocBanChay;
GO
CREATE PROCEDURE sp_Dashboard_TopThuocBanChay
    @Nam     INT,
    @SoLuong INT = 5
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TuNgay  DATETIME = DATEFROMPARTS(@Nam, 1, 1);
    DECLARE @DenNgay DATETIME = DATEADD(YEAR, 1, @TuNgay);

    SELECT TOP (@SoLuong)
        t.MaThuoc,
        t.TenThuoc,
        SUM(c.SoLuong)   AS SLBan,
        SUM(c.ThanhTien) AS DoanhThu
    FROM ChiTietHDB c
    JOIN HoaDonBan h ON h.MaHDB = c.MaHDB
    JOIN Thuoc t     ON t.MaThuoc = c.MaThuoc
    WHERE h.NgayBan >= @TuNgay AND h.NgayBan < @DenNgay AND h.DaHuy = 0
    GROUP BY t.MaThuoc, t.TenThuoc
    ORDER BY SUM(c.SoLuong) DESC, SUM(c.ThanhTien) DESC;
END
GO

-- ---------------------------------------------------------------------
-- Biểu đồ so sánh Y/C 9: doanh thu của TẤT CẢ nhân viên trong quý
--   (báo cáo Y/C 9 chỉ lấy 2 NV thấp nhất, biểu đồ cần cả phần còn lại)
--   @ChiTinhNVCoBan = 1: chỉ NV có hóa đơn trong quý (giống báo cáo Y/C 9)
--   @ChiTinhNVCoBan = 0: thêm NV bán hàng / quản lý đang làm nhưng chưa bán
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_Dashboard_DoanhThuNhanVienTheoQuy', 'P') IS NOT NULL
    DROP PROCEDURE sp_Dashboard_DoanhThuNhanVienTheoQuy;
GO
CREATE PROCEDURE sp_Dashboard_DoanhThuNhanVienTheoQuy
    @Quy            INT,
    @Nam            INT,
    @ChiTinhNVCoBan BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    IF @Quy NOT BETWEEN 1 AND 4
    BEGIN
        RAISERROR(N'Quý không hợp lệ (1-4).', 16, 1);
        RETURN;
    END

    DECLARE @TuNgay  DATETIME = DATEFROMPARTS(@Nam, (@Quy - 1) * 3 + 1, 1);
    DECLARE @DenNgay DATETIME = DATEADD(MONTH, 3, @TuNgay);

    ;WITH HD AS (
        SELECT h.MaHDB, h.MaNV,
               ISNULL(ct.TongCT, ISNULL(h.TongTien, 0)) AS TongTien
        FROM HoaDonBan h
        LEFT JOIN (SELECT MaHDB, SUM(ThanhTien) AS TongCT
                   FROM ChiTietHDB GROUP BY MaHDB) ct ON ct.MaHDB = h.MaHDB
        WHERE h.NgayBan >= @TuNgay AND h.NgayBan < @DenNgay AND h.DaHuy = 0
          AND (ct.MaHDB IS NOT NULL OR ISNULL(h.TongTien, 0) > 0)
    ),
    DT AS (
        SELECT MaNV, COUNT(*) AS SoHoaDon, SUM(TongTien) AS DoanhThu
        FROM HD GROUP BY MaNV
    )
    SELECT nv.MaNV,
           nv.TenNV,
           ISNULL(dt.SoHoaDon, 0) AS SoHoaDon,
           ISNULL(dt.DoanhThu, 0) AS DoanhThu
    FROM NhanVien nv
    LEFT JOIN DT dt ON dt.MaNV = nv.MaNV
    WHERE dt.MaNV IS NOT NULL
       OR (@ChiTinhNVCoBan = 0 AND nv.NghiViec = 0 AND ISNULL(nv.MaVaiTro, 'BH') <> 'TK')
    ORDER BY ISNULL(dt.DoanhThu, 0), nv.MaNV;
END
GO

-- ---------------------------------------------------------------------
-- Biểu đồ so sánh Y/C 8: tổng tiền TẤT CẢ hóa đơn bán trong năm (tăng dần)
--   cùng điều kiện với sp_BaoCaoTop5HoaDonBanItNhatTheoNam
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_Dashboard_HoaDonBanTheoNam', 'P') IS NOT NULL
    DROP PROCEDURE sp_Dashboard_HoaDonBanTheoNam;
GO
CREATE PROCEDURE sp_Dashboard_HoaDonBanTheoNam
    @Nam INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TuNgay  DATETIME = DATEFROMPARTS(@Nam, 1, 1);
    DECLARE @DenNgay DATETIME = DATEADD(YEAR, 1, @TuNgay);

    SELECT h.MaHDB,
           h.NgayBan,
           ISNULL(ct.TongCT, ISNULL(h.TongTien, 0)) AS TongTien
    FROM HoaDonBan h
    LEFT JOIN (SELECT MaHDB, SUM(ThanhTien) AS TongCT
               FROM ChiTietHDB GROUP BY MaHDB) ct ON ct.MaHDB = h.MaHDB
    WHERE h.NgayBan >= @TuNgay AND h.NgayBan < @DenNgay AND h.DaHuy = 0
      AND (ct.MaHDB IS NOT NULL OR ISNULL(h.TongTien, 0) > 0)
    ORDER BY TongTien, h.NgayBan, h.MaHDB;
END
GO


/* =====================================================================
   PHẦN 5. DỮ LIỆU MẪU (chỉ thêm khi chưa có)
   ===================================================================== */

-- ---------------------------------------------------------------------
-- 5.1 Danh mục, nhân viên, nhà cung cấp, thuốc
-- ---------------------------------------------------------------------

-- Trình độ, chuyên môn
IF NOT EXISTS (SELECT 1 FROM TrinhDo WHERE MaTrinhDo = 'TD01')
    INSERT INTO TrinhDo (MaTrinhDo, TenTrinhDo) VALUES ('TD01', N'Đại học');

IF NOT EXISTS (SELECT 1 FROM ChuyenMon WHERE MaChuyenMon = 'CM01')
    INSERT INTO ChuyenMon (MaChuyenMon, TenChuyenMon) VALUES ('CM01', N'Dược sĩ');
IF NOT EXISTS (SELECT 1 FROM ChuyenMon WHERE MaChuyenMon = 'CM02')
    INSERT INTO ChuyenMon (MaChuyenMon, TenChuyenMon) VALUES ('CM02', N'Quản lý kho');

-- Nhân viên
IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = 'NV01')
    INSERT INTO NhanVien (MaNV, TenNV, NgaySinh, GioiTinh, DiaChi, MaTrinhDo, MaChuyenMon, DienThoai)
    VALUES ('NV01', N'Nguyễn Văn Quản Lý', '1990-01-01', N'Nam', N'Hà Nội', 'TD01', 'CM01', '0988111222');
IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = 'NV02')
    INSERT INTO NhanVien (MaNV, TenNV, NgaySinh, GioiTinh, DiaChi, MaTrinhDo, MaChuyenMon, DienThoai)
    VALUES ('NV02', N'Trần Thị Bán Hàng', '1995-05-05', N'Nữ', N'Hà Nội', 'TD01', 'CM01', '0977333444');
IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = 'NV03')
    INSERT INTO NhanVien (MaNV, TenNV, NgaySinh, GioiTinh, DiaChi, MaTrinhDo, MaChuyenMon, DienThoai)
    VALUES ('NV03', N'Lê Văn Thủ Kho', '1993-03-03', N'Nam', N'Hà Nội', 'TD01', 'CM02', '0966555666');

-- Đơn vị tính, dạng điều chế, nước sản xuất
IF NOT EXISTS (SELECT 1 FROM DonViTinh WHERE MaDVT = 'DVT01')
    INSERT INTO DonViTinh (MaDVT, TenDVT) VALUES ('DVT01', N'Hộp');
IF NOT EXISTS (SELECT 1 FROM DonViTinh WHERE MaDVT = 'DVT02')
    INSERT INTO DonViTinh (MaDVT, TenDVT) VALUES ('DVT02', N'Vỉ');
IF NOT EXISTS (SELECT 1 FROM DonViTinh WHERE MaDVT = 'DVT03')                -- [10/11/2026 lần 2]
    INSERT INTO DonViTinh (MaDVT, TenDVT) VALUES ('DVT03', N'Chai');

IF NOT EXISTS (SELECT 1 FROM DangDieuChe WHERE MaDangDC = 'DC01')
    INSERT INTO DangDieuChe (MaDangDC, TenDangDC) VALUES ('DC01', N'Viên nén');
IF NOT EXISTS (SELECT 1 FROM DangDieuChe WHERE MaDangDC = 'DC02')            -- [10/11/2026 lần 2]
    INSERT INTO DangDieuChe (MaDangDC, TenDangDC) VALUES ('DC02', N'Viên nang');
IF NOT EXISTS (SELECT 1 FROM DangDieuChe WHERE MaDangDC = 'DC03')            -- [10/11/2026 lần 2]
    INSERT INTO DangDieuChe (MaDangDC, TenDangDC) VALUES ('DC03', N'Siro');

IF NOT EXISTS (SELECT 1 FROM NuocSX WHERE MaNuocSX = 'NSX01')
    INSERT INTO NuocSX (MaNuocSX, TenNuocSX) VALUES ('NSX01', N'Việt Nam');
IF NOT EXISTS (SELECT 1 FROM NuocSX WHERE MaNuocSX = 'NSX02')                -- [10/11/2026 lần 2]
    INSERT INTO NuocSX (MaNuocSX, TenNuocSX) VALUES ('NSX02', N'Đức');

-- Nhà cung cấp (bắt buộc phải có để nhập hàng không lỗi khóa ngoại MaNCC)
IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE MaNCC = 'NCC01')
    INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, DienThoai)
    VALUES ('NCC01', N'Công ty CP Dược Hậu Giang', N'Cần Thơ', '02923891433');
IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE MaNCC = 'NCC02')
    INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, DienThoai)
    VALUES ('NCC02', N'Công ty CP Traphaco', N'Hà Nội', '02436830920');

-- Thuốc
IF NOT EXISTS (SELECT 1 FROM Thuoc WHERE MaThuoc = 'TH001')
    INSERT INTO Thuoc (MaThuoc, TenThuoc, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan, SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh)
    VALUES ('TH001', N'Panadol Extra', 'DVT02', 'DC01', N'Paracetamol', 15000, 16500, 100, '2025-01-01', 'NSX01', '2028-01-01', N'Mẫn cảm với thuốc');

-- [10/11/2026 lần 2] Thêm 6 thuốc mẫu để demo tìm theo công dụng (yêu cầu 4).
--   Giá bán = 110% giá nhập (yêu cầu 3), SL >= 30 và hạn SD xa nên không xuất
--   hiện trong báo cáo cảnh báo. Bỏ qua mã TH002, TH003 (dành cho demo import
--   thuốc mới bằng MauPhieuNhap.xlsx).
INSERT INTO Thuoc (MaThuoc, TenThuoc, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan, SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh)
SELECT v.MaThuoc, v.TenThuoc, v.MaDVT, v.MaDangDC, v.ThanhPhan, v.DonGiaNhap, v.DonGiaNhap * 1.1, v.SLHienCo, v.NgaySX, v.MaNuocSX, v.HanSD, v.ChongChiDinh
FROM (VALUES
    ('TH010', N'Amoxicillin 500mg',        'DVT02', 'DC02', N'Amoxicillin 500mg',                         25000, 120, '2025-06-01', 'NSX01', '2028-06-30', N'Dị ứng với kháng sinh nhóm penicillin'),
    ('TH011', N'Berberin 100mg',           'DVT01', 'DC01', N'Berberin clorid 100mg',                      8000, 200, '2025-03-01', 'NSX01', '2028-03-31', N'Phụ nữ có thai'),
    ('TH012', N'Loratadin 10mg',           'DVT02', 'DC01', N'Loratadin 10mg',                            12000,  90, '2025-01-01', 'NSX01', '2027-12-31', N'Mẫn cảm với loratadin'),
    ('TH013', N'Siro ho thảo dược',        'DVT03', 'DC03', N'Cao khô lá thường xuân',                    65000,  60, '2025-09-01', 'NSX02', '2028-09-30', N'Mẫn cảm với thành phần của thuốc'),
    ('TH014', N'Vitamin 3B',               'DVT01', 'DC02', N'Vitamin B1, Vitamin B6, Vitamin B12',      30000, 150, '2025-02-01', 'NSX01', '2028-01-31', N'Mẫn cảm với thành phần của thuốc'),
    ('TH015', N'Ibuprofen 400mg',          'DVT02', 'DC01', N'Ibuprofen 400mg',                           10000, 100, '2025-04-01', 'NSX01', '2027-10-31', N'Loét dạ dày tá tràng, suy gan, suy thận nặng')
) AS v (MaThuoc, TenThuoc, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh)
WHERE NOT EXISTS (SELECT 1 FROM Thuoc t WHERE t.MaThuoc = v.MaThuoc);

-- Thuốc test màu cảnh báo (đỏ / vàng) ở trang Báo cáo.
-- Hạn SD tính theo ngày chạy script nên luôn đúng trạng thái.
IF NOT EXISTS (SELECT 1 FROM Thuoc WHERE MaThuoc = 'TEST01')
    INSERT INTO Thuoc (MaThuoc, TenThuoc, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan, SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh)
    VALUES ('TEST01', N'[Test] Đã hết hạn', 'DVT01', 'DC01', N'Test', 10000, 11000, 50, '2024-01-01', 'NSX01', DATEADD(DAY, -10, GETDATE()), NULL);
IF NOT EXISTS (SELECT 1 FROM Thuoc WHERE MaThuoc = 'TEST02')
    INSERT INTO Thuoc (MaThuoc, TenThuoc, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan, SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh)
    VALUES ('TEST02', N'[Test] Sắp hết hạn', 'DVT01', 'DC01', N'Test', 10000, 11000, 80, '2025-01-01', 'NSX01', DATEADD(DAY, 15, GETDATE()), NULL);
IF NOT EXISTS (SELECT 1 FROM Thuoc WHERE MaThuoc = 'TEST03')
    INSERT INTO Thuoc (MaThuoc, TenThuoc, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan, SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh)
    VALUES ('TEST03', N'[Test] Sắp hết hàng', 'DVT01', 'DC01', N'Test', 10000, 11000, 12, '2025-01-01', 'NSX01', '2028-12-31', NULL);
IF NOT EXISTS (SELECT 1 FROM Thuoc WHERE MaThuoc = 'TEST04')
    INSERT INTO Thuoc (MaThuoc, TenThuoc, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan, SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh)
    VALUES ('TEST04', N'[Test] Hết hàng', 'DVT01', 'DC01', N'Test', 10000, 11000, 0, '2025-01-01', 'NSX01', '2028-12-31', NULL);
GO

-- ---------------------------------------------------------------------
-- 5.2 [10/11/2026 lần 2] CÔNG DỤNG + GÁN CÔNG DỤNG CHO THUỐC (yêu cầu 4)
--   Thử trên form Tra cứu: gõ "giảm đau", "hạ sốt", "dị ứng", "ho",
--   "tiêu chảy", "vitamin", "nhiễm khuẩn", "đau đầu"...
-- ---------------------------------------------------------------------
INSERT INTO CongDung (MaCongDung, TenCongDung)
SELECT v.MaCongDung, v.TenCongDung
FROM (VALUES
    ('CD01', N'Giảm đau'),
    ('CD02', N'Hạ sốt'),
    ('CD03', N'Kháng viêm'),
    ('CD04', N'Kháng sinh - trị nhiễm khuẩn'),
    ('CD05', N'Chống dị ứng'),
    ('CD06', N'Giảm ho - long đờm'),
    ('CD07', N'Trị tiêu chảy - rối loạn tiêu hóa'),
    ('CD08', N'Bổ sung vitamin'),
    ('CD09', N'Tăng sức đề kháng'),
    ('CD10', N'Trị viêm mũi dị ứng')
) AS v (MaCongDung, TenCongDung)
WHERE NOT EXISTS (SELECT 1 FROM CongDung c WHERE c.MaCongDung = v.MaCongDung);

-- Gán công dụng. Cột MauTen: chỉ gán khi tên thuốc trong CSDL khớp mẫu này,
-- tránh gán nhầm nếu máy bạn đã có mã thuốc trùng nhưng là thuốc khác
-- (TH002, TH003 chỉ có sau khi import MauPhieuNhap.xlsx).
INSERT INTO Thuoc_CongDung (MaThuoc, MaCongDung, GhiChu)
SELECT v.MaThuoc, v.MaCongDung, v.GhiChu
FROM (VALUES
    ('TH001', 'CD01', N'Đau đầu, đau răng, đau cơ, đau bụng kinh', N'%Panadol%'),
    ('TH001', 'CD02', N'Sốt do cảm cúm',                          N'%Panadol%'),
    ('TH002', 'CD01', N'Đau đầu, đau nhức xương khớp',            N'%Efferalgan%'),
    ('TH002', 'CD02', N'Sốt, cảm cúm',                            N'%Efferalgan%'),
    ('TH003', 'CD08', N'Bổ sung vitamin C',                       N'%Vitamin C%'),
    ('TH003', 'CD09', N'Tăng đề kháng, mau lành vết thương',      N'%Vitamin C%'),
    ('TH010', 'CD04', N'Viêm họng, viêm phế quản, nhiễm khuẩn tiết niệu', N'%Amoxicillin%'),
    ('TH011', 'CD07', N'Tiêu chảy, lỵ trực khuẩn',                N'%Berberin%'),
    ('TH012', 'CD05', N'Mày đay, ngứa do dị ứng',                 N'%Loratadin%'),
    ('TH012', 'CD10', N'Hắt hơi, sổ mũi, ngứa mũi',               N'%Loratadin%'),
    ('TH013', 'CD06', N'Ho có đờm, viêm đường hô hấp',            N'%Siro ho%'),
    ('TH014', 'CD08', N'Bổ sung vitamin nhóm B',                  N'%Vitamin 3B%'),
    ('TH014', 'CD09', N'Suy nhược cơ thể, mệt mỏi',               N'%Vitamin 3B%'),
    ('TH015', 'CD01', N'Đau đầu, đau răng, đau khớp',             N'%Ibuprofen%'),
    ('TH015', 'CD02', N'Hạ sốt',                                  N'%Ibuprofen%'),
    ('TH015', 'CD03', N'Viêm khớp, sưng đau',                     N'%Ibuprofen%')
) AS v (MaThuoc, MaCongDung, GhiChu, MauTen)
JOIN Thuoc t ON t.MaThuoc = v.MaThuoc AND t.TenThuoc LIKE v.MauTen
WHERE NOT EXISTS (SELECT 1 FROM Thuoc_CongDung x
                  WHERE x.MaThuoc = v.MaThuoc AND x.MaCongDung = v.MaCongDung);
GO

-- ---------------------------------------------------------------------
-- 5.3 [10/11/2026 lần 3] VAI TRÒ, TÀI KHOẢN ĐĂNG NHẬP, MÃ VẠCH, KHÁCH HÀNG
-- ---------------------------------------------------------------------

-- Vai trò
INSERT INTO VaiTro (MaVaiTro, TenVaiTro)
SELECT v.MaVaiTro, v.TenVaiTro
FROM (VALUES
    ('QL', N'Quản lý'),
    ('BH', N'Nhân viên bán hàng'),
    ('TK', N'Thủ kho')
) AS v (MaVaiTro, TenVaiTro)
WHERE NOT EXISTS (SELECT 1 FROM VaiTro x WHERE x.MaVaiTro = v.MaVaiTro);

-- Trình độ, chuyên môn bổ sung (dùng ở form Quản lý nhân viên)
IF NOT EXISTS (SELECT 1 FROM TrinhDo WHERE MaTrinhDo = 'TD02')
    INSERT INTO TrinhDo (MaTrinhDo, TenTrinhDo) VALUES ('TD02', N'Cao đẳng');
IF NOT EXISTS (SELECT 1 FROM TrinhDo WHERE MaTrinhDo = 'TD03')
    INSERT INTO TrinhDo (MaTrinhDo, TenTrinhDo) VALUES ('TD03', N'Trung cấp');
IF NOT EXISTS (SELECT 1 FROM ChuyenMon WHERE MaChuyenMon = 'CM03')
    INSERT INTO ChuyenMon (MaChuyenMon, TenChuyenMon) VALUES ('CM03', N'Quản lý cửa hàng');

-- Nhân viên mới: 1 thủ kho, 1 quản lý (mật khẩu = mã NV + '11', băm ở dưới)
-- [10/11/2026 lần 5] bỏ cột MatKhau khỏi câu INSERT (mật khẩu không còn lưu chữ thường)
IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = 'TK001')
    INSERT INTO NhanVien (MaNV, TenNV, NgaySinh, GioiTinh, DiaChi, MaTrinhDo, MaChuyenMon, DienThoai, MaVaiTro)
    VALUES ('TK001', N'Phạm Văn Kho', '1992-07-12', N'Nam', N'Hà Nội', 'TD02', 'CM02', '0912000001', 'TK');
IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = 'QL001')
    INSERT INTO NhanVien (MaNV, TenNV, NgaySinh, GioiTinh, DiaChi, MaTrinhDo, MaChuyenMon, DienThoai, MaVaiTro)
    VALUES ('QL001', N'Hoàng Thị Quản Lý', '1988-09-20', N'Nữ', N'Hà Nội', 'TD01', 'CM03', '0912000002', 'QL');

-- [10/11/2026 lần 4] Gán vai trò cho NV chưa có vai trò THEO TIỀN TỐ MÃ NV
--   QLxxx = Quản lý | TKxxx = Thủ kho | NVxx và mã khác = Bán hàng
UPDATE NhanVien
SET MaVaiTro = CASE WHEN MaNV LIKE 'QL%' THEN 'QL'
                    WHEN MaNV LIKE 'TK%' THEN 'TK'
                    ELSE 'BH' END
WHERE MaVaiTro IS NULL;

-- [10/11/2026 lần 4] Sửa dữ liệu của lần 3 (đã gán nhầm NV01 = QL, NV03 = TK theo tên):
-- mã NV... là nhân viên bán hàng
UPDATE NhanVien SET MaVaiTro = 'BH' WHERE MaNV IN ('NV01', 'NV03') AND MaVaiTro IN ('QL', 'TK');

-- Mật khẩu mặc định cho NV chưa có mật khẩu: mã NV + '11'
-- [10/11/2026 lần 5] lưu dạng băm: Hash = SHA-256(Salt + mật khẩu), Salt ngẫu nhiên từng NV
--   (NEWID() sinh khác nhau trên từng dòng; app C# băm giống hệt để so khi đăng nhập)
UPDATE NhanVien
SET MatKhauSalt = CONVERT(VARCHAR(32), CAST(NEWID() AS BINARY(16)), 2)
WHERE MatKhauHash IS NULL;

UPDATE NhanVien
SET MatKhauHash = CONVERT(VARCHAR(64),
        HASHBYTES('SHA2_256', CAST(MatKhauSalt AS NVARCHAR(32)) + CAST(RTRIM(MaNV) AS NVARCHAR(10)) + N'11'), 2)
WHERE MatKhauHash IS NULL;

-- [10/11/2026 lần 5] Thuốc kê đơn mẫu: kháng sinh Amoxicillin
UPDATE Thuoc SET KeDon = 1 WHERE MaThuoc = 'TH010' AND TenThuoc LIKE N'%Amoxicillin%' AND KeDon = 0;

-- Mã vạch mẫu (chỉ gán khi thuốc chưa có mã vạch và mã vạch chưa bị dùng)
UPDATE t
SET MaVach = v.MaVach
FROM Thuoc t
JOIN (VALUES
    ('TH001',  '8934588012112'),
    ('TH010',  '8936100100107'),
    ('TH011',  '8936100100114'),
    ('TH012',  '8936100100121'),
    ('TH013',  '8936100100138'),
    ('TH014',  '8936100100145'),
    ('TH015',  '8936100100152'),
    ('TEST01', '8930000000017'),   -- thuốc đã hết hạn: quét thử sẽ bị chặn
    ('TEST04', '8930000000048')    -- thuốc hết hàng: quét thử sẽ báo không đủ tồn
) AS v (MaThuoc, MaVach) ON v.MaThuoc = t.MaThuoc
WHERE t.MaVach IS NULL
  AND NOT EXISTS (SELECT 1 FROM Thuoc x WHERE x.MaVach = v.MaVach);

-- Khách hàng mẫu (KH001 có sẵn 150 điểm để demo đổi điểm)
IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKhach = 'KH001')
    INSERT INTO KhachHang (MaKhach, TenKhach, DiaChi, DienThoai, DiemTichLuy)
    VALUES ('KH001', N'Nguyễn Thị Lan', N'Hà Nội', '0901234567', 150);
IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKhach = 'KH002')
    INSERT INTO KhachHang (MaKhach, TenKhach, DiaChi, DienThoai, DiemTichLuy)
    VALUES ('KH002', N'Trần Văn Minh', N'Hà Nội', '0912345678', 0);
GO


/* =====================================================================
   PHẦN 6. SỬA DỮ LIỆU CŨ
   TongTien của hóa đơn lưu trước khi có trigger có thể = 0 -> tính lại.
   (CSDL mới cài thì đoạn này không làm gì.)
   ===================================================================== */
UPDATE h SET TongTien = ct.Tong
FROM HoaDonBan h
JOIN (SELECT MaHDB, SUM(ThanhTien) AS Tong FROM ChiTietHDB GROUP BY MaHDB) ct ON ct.MaHDB = h.MaHDB
WHERE ISNULL(h.TongTien, 0) <> ct.Tong;

UPDATE h SET TongTien = ct.Tong
FROM HoaDonNhap h
JOIN (SELECT MaHDN, SUM(ThanhTien) AS Tong FROM ChiTietHDN GROUP BY MaHDN) ct ON ct.MaHDN = h.MaHDN
WHERE ISNULL(h.TongTien, 0) <> ct.Tong;
GO

-- [10/11/2026 lần 5] Tồn kho có từ trước (chưa có lô) -> lô "TON-DAU" với hạn SD cũ của thuốc,
-- rồi đồng bộ SLHienCo / HanSD theo lô. Chạy lại không bị nhân đôi.
EXEC sp_TaoLoTonDau NULL;
EXEC sp_DongBoThuocTuLo NULL;
GO

PRINT N'=== Cài đặt CSDL QuanLyNhaThuoc hoàn tất ===';
GO
