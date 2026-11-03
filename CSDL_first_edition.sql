
-- HIỆN TẠI CSDL NÀY ĐÁP ỨNG ĐỦ 9 YÊU CẦU ĐỂ BÀI VUI LÒNG CẬP NHẬT LẠI VÀ NOTE NGAY DƯỚI NẾU CÓ CẬP NHẬT MỚI
-- UPDATE CSDL
-- =====================================================================
-- UPDATE CSDL 01/11/2026 - phục vụ form Nhập hàng (MaNCC, MaNV thủ kho, Import Excel)
-- * KHÔNG đổi cấu trúc bảng nào -> chạy được trên CSDL hiện tại.
-- * Bổ sung dữ liệu mẫu bảng NhaCungCap: trước đây bảng này TRỐNG nên mọi lệnh
--   nhập hàng (kể cả code cũ gán cứng 'NCC01') đều lỗi khóa ngoại MaNCC.
-- * Thêm 1 nhân viên thủ kho mẫu (NV03).
-- Sửa lại proc 5 ở dưới vì lỗi chưa tìm kiếm được bằng NV thực hiện và MaNCC.


-- =====================================================================
-- UPDATE CSDL 03/11/2026 - sửa form Báo cáo + thêm cảnh báo thuốc sắp hết
-- * KHÔNG đổi cấu trúc bảng cũ nào -> chạy thẳng trên CSDL hiện tại được,
--   chạy lại nhiều lần cũng không lỗi (proc/trigger DROP rồi CREATE lại,
--   bảng mới chỉ tạo khi chưa có).
-- * Lỗi Top 2 NV ra 0.00: HoaDonBan.TongTien không được cập nhật khi thêm
--   chi tiết (luôn = 0 mặc định). Sửa bằng 3 cách cùng lúc:
--     1) Các báo cáo tính tổng tiền từ bảng chi tiết (SUM ThanhTien),
--        không phụ thuộc cột TongTien nữa.
--     2) Thêm trigger tự cập nhật TongTien của HoaDonBan / HoaDonNhap mỗi khi
--        thêm / sửa / xóa chi tiết.
--     3) Cập nhật lại TongTien cho các hóa đơn cũ đang bị sai.
-- * Proc 6 (hạn sử dụng): thêm tham số @TrangThai = -1 (tất cả) và
--   @SoNgayCanhBao, sắp xếp theo hạn SD ngắn nhất, thêm cột SoNgayConLai,
--   TinhTrang, MucCanhBao (2 = đã hết hạn -> đỏ, 1 = sắp hết hạn -> vàng).
-- * Proc 7, 8: lọc theo khoảng ngày, trả thêm tên NV / NCC / khách hàng.
-- * Proc 9: tính theo quý chọn, thêm SoHoaDon, tham số @ChiTinhNVCoBan
--   (1 = bỏ qua NV không có hóa đơn nào trong quý, vd quản lý / thủ kho).
-- * Proc mới sp_CanhBaoThuocSapHet: thuốc có SLHienCo < @NguongSL (mặc định 30).
-- * (Bổ sung 03/11 lần 2) Thêm bảng LichSuHuyThuoc (bảng MỚI, không sửa bảng cũ),
--   proc sp_DanhSachCanhBao (trang chi tiết cảnh báo: hết hạn / cận hạn /
--   hết hàng / gần hết hàng) và sp_HuyThuocHetHan (nút "Hủy thuốc hết hạn":
--   đưa SLHienCo của thuốc hết hạn về 0 + ghi lịch sử hủy).
-- =====================================================================
USE QuanLyNhaThuoc;
GO

-- ---------------------------------------------------------------------
-- 1. Trigger đồng bộ TongTien = SUM(ThanhTien) của chi tiết
-- ---------------------------------------------------------------------
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

-- Sửa lại TongTien cho các hóa đơn đã lưu trước đây (chỉ hóa đơn có chi tiết)
UPDATE h SET TongTien = ct.Tong
FROM HoaDonBan h
JOIN (SELECT MaHDB, SUM(ThanhTien) AS Tong FROM ChiTietHDB GROUP BY MaHDB) ct ON ct.MaHDB = h.MaHDB
WHERE ISNULL(h.TongTien, 0) <> ct.Tong;

UPDATE h SET TongTien = ct.Tong
FROM HoaDonNhap h
JOIN (SELECT MaHDN, SUM(ThanhTien) AS Tong FROM ChiTietHDN GROUP BY MaHDN) ct ON ct.MaHDN = h.MaHDN
WHERE ISNULL(h.TongTien, 0) <> ct.Tong;
GO

-- ---------------------------------------------------------------------
-- 2. Yêu cầu 6: thuốc còn hạn / hết hạn, sắp xếp theo hạn SD ngắn nhất
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_BaoCaoHanSuDung', 'P') IS NOT NULL
    DROP PROCEDURE sp_BaoCaoHanSuDung;
GO
CREATE PROCEDURE sp_BaoCaoHanSuDung
    @TrangThai     INT = -1,  -- -1: tất cả, 1: còn hạn, 0: đã hết hạn
    @SoNgayCanhBao INT = 30   -- còn <= số ngày này thì tính là "sắp hết hạn"
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @HomNay DATE = CAST(GETDATE() AS DATE);
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
-- 3. Yêu cầu 7: danh sách HĐN và tổng tiền nhập theo tháng/năm chọn
--    Result set 1: danh sách | Result set 2: SoHoaDon, TongTienNhapThang
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
-- 4. Yêu cầu 8: 5 hóa đơn bán có tổng tiền ít nhất theo năm chọn
--    (bỏ qua hóa đơn rỗng: không có chi tiết và tổng tiền = 0)
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
        WHERE h.NgayBan >= @TuNgay AND h.NgayBan < @DenNgay
          AND (ct.MaHDB IS NOT NULL OR ISNULL(h.TongTien, 0) > 0)
    ) x
    ORDER BY STT;
END
GO

-- ---------------------------------------------------------------------
-- 5. Yêu cầu 9: 2 nhân viên bán được ít tiền nhất theo quý/năm chọn
-- ---------------------------------------------------------------------
IF OBJECT_ID('sp_BaoCaoTop2NhanVienBanItNhatTheoQuy', 'P') IS NOT NULL
    DROP PROCEDURE sp_BaoCaoTop2NhanVienBanItNhatTheoQuy;
GO
CREATE PROCEDURE sp_BaoCaoTop2NhanVienBanItNhatTheoQuy
    @Quy            INT,
    @Nam            INT,
    @ChiTinhNVCoBan BIT = 1  -- 1: chỉ xét NV có ít nhất 1 hóa đơn trong quý
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
        WHERE h.NgayBan >= @TuNgay AND h.NgayBan < @DenNgay
          AND (ct.MaHDB IS NOT NULL OR ISNULL(h.TongTien, 0) > 0)
    ),
    DT AS (
        SELECT nv.MaNV, nv.TenNV,
               COUNT(HD.MaHDB)              AS SoHoaDon,
               ISNULL(SUM(HD.TongTien), 0)  AS TongDoanhThu
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
-- 6. MỚI: cảnh báo thuốc sắp hết hàng (SLHienCo < @NguongSL)
--    MucCanhBao: 2 = hết hàng (SL <= 0) -> đỏ, 1 = sắp hết -> vàng
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
        JOIN HoaDonNhap h      ON h.MaHDN = c.MaHDN
        LEFT JOIN NhaCungCap ncc ON ncc.MaNCC = h.MaNCC
        WHERE c.MaThuoc = t.MaThuoc
        ORDER BY h.NgayNhap DESC
    ) gn
    WHERE t.SLHienCo < @NguongSL
    ORDER BY STT;
END
GO

-- ---------------------------------------------------------------------
-- 7. MỚI: bảng lịch sử hủy thuốc (lưu lại mỗi lần bấm "Hủy thuốc hết hạn")
--    Không xóa hẳn dòng trong bảng Thuoc vì thuốc đó còn nằm trong các
--    hóa đơn cũ (khóa ngoại) -> chỉ đưa SLHienCo về 0 và ghi lịch sử ở đây.
-- ---------------------------------------------------------------------
IF OBJECT_ID('LichSuHuyThuoc', 'U') IS NULL
BEGIN
    CREATE TABLE LichSuHuyThuoc (
        MaHuy       INT IDENTITY(1,1) PRIMARY KEY,
        MaThuoc     VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES Thuoc(MaThuoc),
        TenThuoc    NVARCHAR(100),
        SoLuongHuy  INT NOT NULL,
        HanSD       DATE,
        DonGiaNhap  DECIMAL(18,2) NOT NULL DEFAULT 0,
        GiaTriHuy   DECIMAL(18,2) NOT NULL DEFAULT 0,
        NgayHuy     DATETIME NOT NULL DEFAULT GETDATE(),
        MaNV        VARCHAR(10) NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
        LyDo        NVARCHAR(255)
    );
END
GO

-- ---------------------------------------------------------------------
-- 8. MỚI: danh sách cảnh báo (trang mở ra khi bấm banner cảnh báo)
--    @Loai: 0 tất cả | 1 hết hạn | 2 cận hạn | 3 hết hàng | 4 gần hết hàng
--    * Hết hạn / cận hạn chỉ tính thuốc còn tồn (SLHienCo > 0); thuốc đã hủy
--      (SL = 0) sẽ chuyển sang nhóm "Hết hàng" để nhập lại.
--    * MucCanhBao: 2 = hết hạn hoặc hết hàng (đỏ), 1 = cận hạn / gần hết (vàng)
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
    DECLARE @HomNay DATE = CAST(GETDATE() AS DATE);
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
-- 9. MỚI: hủy toàn bộ thuốc đã hết hạn còn tồn kho
--    Đưa SLHienCo về 0 + ghi LichSuHuyThuoc, trong 1 transaction.
--    Trả về: SoThuocHuy, TongSLHuy, TongGiaTriHuy
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
        MaThuoc    VARCHAR(10),
        TenThuoc   NVARCHAR(100),
        SoLuong    INT,
        HanSD      DATE,
        DonGiaNhap DECIMAL(18,2)
    );

    IF @MaNV IS NOT NULL AND NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = @MaNV)
        SET @MaNV = NULL;

    BEGIN TRANSACTION;

        UPDATE Thuoc
        SET SLHienCo = 0
        OUTPUT deleted.MaThuoc, deleted.TenThuoc, deleted.SLHienCo, deleted.HanSD, deleted.DonGiaNhap
        INTO @DaHuy (MaThuoc, TenThuoc, SoLuong, HanSD, DonGiaNhap)
        WHERE SLHienCo > 0 AND HanSD < @HomNay;

        INSERT INTO LichSuHuyThuoc (MaThuoc, TenThuoc, SoLuongHuy, HanSD, DonGiaNhap, GiaTriHuy, NgayHuy, MaNV, LyDo)
        SELECT MaThuoc, TenThuoc, SoLuong, HanSD, DonGiaNhap, SoLuong * DonGiaNhap, GETDATE(), @MaNV, @LyDo
        FROM @DaHuy;

    COMMIT TRANSACTION;

    SELECT COUNT(*)                              AS SoThuocHuy,
           ISNULL(SUM(SoLuong), 0)               AS TongSLHuy,
           ISNULL(SUM(SoLuong * DonGiaNhap), 0)  AS TongGiaTriHuy
    FROM @DaHuy;
END
GO

-- =====================================================================
-- Dữ liệu mẫu để test màu cảnh báo
-- =====================================================================

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

-- =====================================================================
USE QuanLyNhaThuoc;
GO

IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE MaNCC = 'NCC01')
    INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, DienThoai)
    VALUES ('NCC01', N'Công ty CP Dược Hậu Giang', N'Cần Thơ', '02923891433');

IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE MaNCC = 'NCC02')
    INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, DienThoai)
    VALUES ('NCC02', N'Công ty CP Traphaco', N'Hà Nội', '02436830920');

IF NOT EXISTS (SELECT 1 FROM ChuyenMon WHERE MaChuyenMon = 'CM02')
    INSERT INTO ChuyenMon (MaChuyenMon, TenChuyenMon) VALUES ('CM02', N'Quản lý kho');

IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = 'NV03')
    INSERT INTO NhanVien (MaNV, TenNV, NgaySinh, GioiTinh, DiaChi, MaTrinhDo, MaChuyenMon, DienThoai)
    VALUES ('NV03', N'Lê Văn Thủ Kho', '1993-03-03', N'Nam', N'Hà Nội', 'TD01', 'CM02', '0966555666');
GO

-- Yêu cầu 5:
USE QuanLyNhaThuoc;
GO
ALTER PROCEDURE sp_TimKiemHoaDonNhap
    @TuKhoa NVARCHAR(100)
AS
BEGIN
    -- Xử lý khi từ khóa trống thì lấy toàn bộ danh sách
    IF @TuKhoa IS NULL SET @TuKhoa = '';

    SELECT DISTINCT 
        hd.MaHDN, 
        hd.NgayNhap, 
        hd.MaNV, 
        nv.TenNV, 
        hd.MaNCC, 
        ncc.TenNCC, 
        hd.TongTien 
    FROM HoaDonNhap hd
    LEFT JOIN NhanVien nv ON hd.MaNV = nv.MaNV
    LEFT JOIN NhaCungCap ncc ON hd.MaNCC = ncc.MaNCC
    LEFT JOIN ChiTietHDN cthn ON hd.MaHDN = cthn.MaHDN
    WHERE hd.MaHDN LIKE '%' + @TuKhoa + '%'
       OR nv.TenNV LIKE '%' + @TuKhoa + '%'
       OR ncc.TenNCC LIKE '%' + @TuKhoa + '%'
       OR cthn.MaThuoc LIKE '%' + @TuKhoa + '%'
    ORDER BY hd.NgayNhap DESC;
END
GO
-- UPDATE CSDL



CREATE DATABASE QuanLyNhaThuoc;
GO

USE QuanLyNhaThuoc;
GO

-- 1. Bảng Trình độ
CREATE TABLE TrinhDo (
    MaTrinhDo VARCHAR(10) PRIMARY KEY,
    TenTrinhDo NVARCHAR(50) NOT NULL
);

-- 2. Bảng Chuyên môn
CREATE TABLE ChuyenMon (
    MaChuyenMon VARCHAR(10) PRIMARY KEY,
    TenChuyenMon NVARCHAR(50) NOT NULL
);

-- 3. Bảng Vai trò (Phân quyền: Quản lý, Nhân viên bán hàng, Nhân viên kho...)
-- hiện chưa có 

-- 4. Bảng Đơn vị tính
CREATE TABLE DonViTinh (
    MaDVT VARCHAR(10) PRIMARY KEY,
    TenDVT NVARCHAR(50) NOT NULL
);

-- 5. Bảng Dạng điều chế
CREATE TABLE DangDieuChe (
    MaDangDC VARCHAR(10) PRIMARY KEY,
    TenDangDC NVARCHAR(50) NOT NULL
);

-- 6. Bảng Nước sản xuất
CREATE TABLE NuocSX (
    MaNuocSX VARCHAR(10) PRIMARY KEY,
    TenNuocSX NVARCHAR(50) NOT NULL
);

-- 7. Bảng Công dụng
CREATE TABLE CongDung (
    MaCongDung VARCHAR(10) PRIMARY KEY,
    TenCongDung NVARCHAR(100) NOT NULL
);

-- 8. Bảng Nhân viên
CREATE TABLE NhanVien (
    MaNV VARCHAR(10) PRIMARY KEY,
    TenNV NVARCHAR(50) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(255),
    MaTrinhDo VARCHAR(10) FOREIGN KEY REFERENCES TrinhDo(MaTrinhDo),
    MaChuyenMon VARCHAR(10) FOREIGN KEY REFERENCES ChuyenMon(MaChuyenMon),
    DienThoai VARCHAR(20),
);

-- 9. Bảng Khách hàng
CREATE TABLE KhachHang (
    MaKhach VARCHAR(10) PRIMARY KEY,
    TenKhach NVARCHAR(50) NOT NULL,
    DiaChi NVARCHAR(255),
    DienThoai VARCHAR(20)
);

-- 10. Bảng Nhà cung cấp
CREATE TABLE NhaCungCap (
    MaNCC VARCHAR(10) PRIMARY KEY,
    TenNCC NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(255),
    DienThoai VARCHAR(20)
);

-- 11. Bảng Thuốc (Danh mục thuốc)
CREATE TABLE Thuoc (
    MaThuoc VARCHAR(10) PRIMARY KEY,
    TenThuoc NVARCHAR(100) NOT NULL,
    MaDVT VARCHAR(10) FOREIGN KEY REFERENCES DonViTinh(MaDVT),
    MaDangDC VARCHAR(10) FOREIGN KEY REFERENCES DangDieuChe(MaDangDC),
    ThanhPhan NVARCHAR(255),
    DonGiaNhap DECIMAL(18,2) NOT NULL DEFAULT 0,
    GiaBan DECIMAL(18,2) NOT NULL DEFAULT 0, -- Tự động = 110% giá nhập
    SLHienCo INT NOT NULL DEFAULT 0,
    NgaySX DATE,
    MaNuocSX VARCHAR(10) FOREIGN KEY REFERENCES NuocSX(MaNuocSX),
    HanSD DATE,
    ChongChiDinh NVARCHAR(255)
);

-- 12. Bảng trung gian Thuốc - Công dụng
CREATE TABLE Thuoc_CongDung (
    MaThuoc VARCHAR(10) FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    MaCongDung VARCHAR(10) FOREIGN KEY REFERENCES CongDung(MaCongDung),
    GhiChu NVARCHAR(255),
    PRIMARY KEY (MaThuoc, MaCongDung)
);

-- 13. Bảng Hóa đơn bán
CREATE TABLE HoaDonBan ( 
    MaHDB VARCHAR(10) PRIMARY KEY,
    NgayBan DATETIME DEFAULT GETDATE(),
    MaNV VARCHAR(10) FOREIGN KEY REFERENCES NhanVien(MaNV),
    MaKhach VARCHAR(10) FOREIGN KEY REFERENCES KhachHang(MaKhach),
    TongTien DECIMAL(18,2) DEFAULT 0
);

-- 14. Bảng Chi tiết HDB
CREATE TABLE ChiTietHDB (
    MaHDB VARCHAR(10) FOREIGN KEY REFERENCES HoaDonBan(MaHDB),
    MaThuoc VARCHAR(10) FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    SoLuong INT NOT NULL,
    GiamGia DECIMAL(18,2) DEFAULT 0,
    ThanhTien DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaHDB, MaThuoc)
);

-- 15. Bảng Hóa đơn nhập
CREATE TABLE HoaDonNhap (
    MaHDN VARCHAR(10) PRIMARY KEY,
    NgayNhap DATETIME DEFAULT GETDATE(),
    MaNV VARCHAR(10) FOREIGN KEY REFERENCES NhanVien(MaNV),
    MaNCC VARCHAR(10) FOREIGN KEY REFERENCES NhaCungCap(MaNCC),
    TongTien DECIMAL(18,2) DEFAULT 0
);

-- 16. Bảng Chi tiết HDN
CREATE TABLE ChiTietHDN  (
    MaHDN VARCHAR(10) FOREIGN KEY REFERENCES HoaDonNhap(MaHDN),
    MaThuoc VARCHAR(10) FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    SLNhap INT NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    KhuyenMai DECIMAL(18,2) DEFAULT 0,
    ThanhTien DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaHDN, MaThuoc)
);
GO



-- Thêm Trình độ & Chuyên môn
INSERT INTO TrinhDo (MaTrinhDo, TenTrinhDo) VALUES ('TD01', N'Đại học');
INSERT INTO ChuyenMon (MaChuyenMon, TenChuyenMon) VALUES ('CM01', N'Dược sĩ');

-- Thêm Nhân viên mẫu
INSERT INTO NhanVien (MaNV, TenNV, NgaySinh, GioiTinh, DiaChi, MaTrinhDo, MaChuyenMon, DienThoai) VALUES 
('NV01', N'Nguyễn Văn Quản Lý', '1990-01-01', N'Nam', N'Hà Nội', 'TD01', 'CM01', '0988111222'),
('NV02', N'Trần Thị Bán Hàng', '1995-05-05', N'Nữ', N'Hà Nội', 'TD01', 'CM01', '0977333444');

-- Thêm Đơn vị tính, Dạng điều chế, Nước SX, Danh mục
INSERT INTO DonViTinh (MaDVT, TenDVT) VALUES ('DVT01', N'Hộp'), ('DVT02', N'Vỉ');
INSERT INTO DangDieuChe (MaDangDC, TenDangDC) VALUES ('DC01', N'Viên nén');
INSERT INTO NuocSX (MaNuocSX, TenNuocSX) VALUES ('NSX01', N'Việt Nam');

-- Thêm Thuốc mẫu
INSERT INTO Thuoc (MaThuoc, TenThuoc, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan, SLHienCo, NgaySX, MaNuocSX, HanSD, ChongChiDinh) VALUES 
('TH001', N'Panadol Extra', 'DVT02', 'DC01', N'Paracetamol', 15000, 16500, 100, '2025-01-01', 'NSX01', '2028-01-01', N'Mẫn cảm với thuốc');
GO

-- Các proc phục vụ 9 yêu cầu 
USE QuanLyNhaThuoc;
GO
-- Yêu cầu 1,2,3: xử lý trong C#

-- Yêu cầu 4: Tìm kiếm thuốc theo Tên thuốc, Công dụng hoặc Thành phần
CREATE PROC sp_TimKiemThuoc
    @TuKhoa NVARCHAR(100)
AS
BEGIN
    SELECT DISTINCT t.* 
    FROM Thuoc t
    LEFT JOIN Thuoc_CongDung tcd ON t.MaThuoc = tcd.MaThuoc
    LEFT JOIN CongDung cd ON tcd.MaCongDung = cd.MaCongDung
    WHERE t.TenThuoc LIKE '%' + @TuKhoa + '%'
       OR t.ThanhPhan LIKE '%' + @TuKhoa + '%'
       OR cd.TenCongDung LIKE '%' + @TuKhoa + '%';
END
GO

-- Yêu cầu 5: Tìm kiếm Hóa đơn nhập theo Mã hàng, Nhân viên, Nhà cung cấp
CREATE PROC sp_TimKiemHoaDonNhap
    @TuKhoa NVARCHAR(100)
AS
BEGIN
    SELECT DISTINCT h.* 
    FROM HoaDonNhap h
    JOIN NhanVien nv ON h.MaNV = nv.MaNV
    JOIN NhaCungCap ncc ON h.MaNCC = ncc.MaNCC
    LEFT JOIN ChiTietHDN cthn ON h.MaHDN = cthn.MaHDN
    WHERE h.MaHDN LIKE '%' + @TuKhoa + '%'
       OR nv.TenNV LIKE '%' + @TuKhoa + '%'
       OR ncc.TenNCC LIKE '%' + @TuKhoa + '%'
       OR cthn.MaThuoc LIKE '%' + @TuKhoa + '%';
END
GO

-- Yêu cầu 6: Báo cáo danh sách các thuốc còn hạn sử dụng và hết hạn sử dụng
CREATE PROC sp_BaoCaoHanSuDung
    @TrangThai INT -- 1: Còn hạn, 0: Hết hạn
AS
BEGIN
    IF @TrangThai = 1
        SELECT * FROM Thuoc WHERE HanSD >= GETDATE();
    ELSE
        SELECT * FROM Thuoc WHERE HanSD < GETDATE();
END
GO

-- Yêu cầu 7: Báo cáo danh sách hóa đơn và tổng tiền nhập hàng theo tháng/năm
CREATE PROC sp_BaoCaoNhapHangTheoThang
    @Thang INT,
    @Nam INT
AS
BEGIN
    SELECT * FROM HoaDonNhap 
    WHERE MONTH(NgayNhap) = @Thang AND YEAR(NgayNhap) = @Nam;

    SELECT ISNULL(SUM(TongTien), 0) AS TongTienNhapThang 
    FROM HoaDonNhap 
    WHERE MONTH(NgayNhap) = @Thang AND YEAR(NgayNhap) = @Nam;
END
GO

-- Yêu cầu 8: Báo cáo danh sách 5 hóa đơn bán hàng có tổng tiền ít nhất theo năm
CREATE PROC sp_BaoCaoTop5HoaDonBanItNhatTheoNam
    @Nam INT
AS
BEGIN
    SELECT TOP 5 * FROM HoaDonBan 
    WHERE YEAR(NgayBan) = @Nam 
    ORDER BY TongTien ASC;
END
GO

-- Yêu cầu 9: Báo cáo 2 nhân viên bán được ít tiền nhất theo quý và năm
CREATE PROC sp_BaoCaoTop2NhanVienBanItNhatTheoQuy
    @Quy INT,
    @Nam INT
AS
BEGIN
    SELECT TOP 2 nv.MaNV, nv.TenNV, ISNULL(SUM(h.TongTien), 0) AS TongDoanhThu
    FROM NhanVien nv
    LEFT JOIN HoaDonBan h ON nv.MaNV = h.MaNV AND YEAR(h.NgayBan) = @Nam AND DATEPART(QUARTER, h.NgayBan) = @Quy
    GROUP BY nv.MaNV, nv.TenNV
    ORDER BY TongDoanhThu ASC;
END
GO