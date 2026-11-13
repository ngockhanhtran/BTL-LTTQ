/* =====================================================================
   THỬ RÀNG BUỘC CHECK (11/11/2026 lần 6) - DÙNG KHI DEMO
   ---------------------------------------------------------------------
   Chạy SAU file CSDL_QuanLyNhaThuoc.sql. Mở SSMS -> F5.
   Mỗi dòng thử 1 câu sửa dữ liệu SAI (giống ai đó sửa thẳng trong SSMS),
   ghi lại SQL Server có chặn không. Cuối file ROLLBACK -> KHÔNG đổi dữ liệu,
   chạy bao nhiêu lần cũng được.
   Kết quả: bảng ở tab Results, cột KetQua phải trùng cột KyVong.
   ===================================================================== */
USE QuanLyNhaThuoc;
GO
SET NOCOUNT ON;
SET XACT_ABORT OFF;      -- lỗi CHECK chỉ hủy 1 câu, transaction vẫn chạy tiếp

DECLARE @Thu TABLE (STT INT IDENTITY(1,1), ThuNghiem NVARCHAR(100), KyVong NVARCHAR(10), Lenh NVARCHAR(1000));
INSERT INTO @Thu (ThuNghiem, KyVong, Lenh) VALUES
(N'Tồn kho âm',                       N'Chặn',    N'UPDATE Thuoc SET SLHienCo = -5 WHERE MaThuoc = ''TH001'''),
(N'Giá bán âm',                       N'Chặn',    N'UPDATE Thuoc SET GiaBan = -1000 WHERE MaThuoc = ''TH001'''),
(N'Tên thuốc toàn dấu cách',          N'Chặn',    N'UPDATE Thuoc SET TenThuoc = N''   '' WHERE MaThuoc = ''TH001'''),
(N'Điểm tích lũy âm',                 N'Chặn',    N'UPDATE KhachHang SET DiemTichLuy = -10 WHERE MaKhach = ''KH001'''),
(N'Giới tính lạ',                     N'Chặn',    N'UPDATE NhanVien SET GioiTinh = N''abc'' WHERE MaNV = ''NV02'''),
(N'Ngày sinh ở tương lai',            N'Chặn',    N'UPDATE NhanVien SET NgaySinh = ''20990101'' WHERE MaNV = ''NV02'''),
(N'Lô còn nhiều hơn số đã nhập',      N'Chặn',    N'UPDATE TOP (1) LoThuoc SET SLCon = SLNhap + 1'),
(N'Lô có hạn SD trước ngày SX',       N'Chặn',    N'UPDATE TOP (1) LoThuoc SET HanSD = DATEADD(DAY, -1, NgaySX) WHERE NgaySX IS NOT NULL'),
(N'Giảm giá 150%',                    N'Chặn',    N'UPDATE TOP (1) ChiTietHDB SET GiamGia = 150'),
(N'Nhập 0 hộp',                       N'Chặn',    N'UPDATE TOP (1) ChiTietHDN SET SLNhap = 0'),
(N'Hủy hóa đơn mà không có ngày hủy', N'Chặn',    N'UPDATE TOP (1) HoaDonBan SET DaHuy = 1, NgayHuy = NULL'),
(N'Sửa giá bán hợp lệ (đối chứng)',   N'Cho qua', N'UPDATE Thuoc SET GiaBan = GiaBan + 0 WHERE MaThuoc = ''TH001''');

DECLARE @KetQua TABLE (STT INT, ThuNghiem NVARCHAR(100), KyVong NVARCHAR(10), KetQua NVARCHAR(30), ThongBaoLoi NVARCHAR(400));
DECLARE @i INT = 1, @n INT, @Ten NVARCHAR(100), @KyVong NVARCHAR(10), @Lenh NVARCHAR(1100), @SoDong INT;
SELECT @n = COUNT(*) FROM @Thu;

BEGIN TRANSACTION;
WHILE @i <= @n
BEGIN
    SELECT @Ten = ThuNghiem, @KyVong = KyVong, @Lenh = Lenh + N'; SET @SoDong = @@ROWCOUNT;' FROM @Thu WHERE STT = @i;
    SET @SoDong = NULL;
    BEGIN TRY
        EXEC sp_executesql @Lenh, N'@SoDong INT OUTPUT', @SoDong = @SoDong OUTPUT;
        INSERT INTO @KetQua VALUES (@i, @Ten, @KyVong,
            CASE WHEN @SoDong = 0 THEN N'(không có dòng để thử)' ELSE N'Cho qua' END, NULL);
    END TRY
    BEGIN CATCH
        INSERT INTO @KetQua VALUES (@i, @Ten, @KyVong,
            CASE WHEN ERROR_NUMBER() = 547 THEN N'Chặn' ELSE N'Lỗi khác' END, ERROR_MESSAGE());
    END CATCH
    SET @i = @i + 1;
END
ROLLBACK TRANSACTION;    -- trả lại dữ liệu như cũ (biến bảng @KetQua không bị rollback)

SELECT STT, ThuNghiem, KyVong, KetQua,
       CASE WHEN KetQua = KyVong THEN N'Đúng' WHEN KetQua LIKE N'(%' THEN N'-' ELSE N'SAI' END AS DanhGia,
       ThongBaoLoi
FROM @KetQua ORDER BY STT;
GO
