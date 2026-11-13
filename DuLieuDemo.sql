/* =====================================================================
   DỮ LIỆU DEMO - QUẢN LÝ CỬA HÀNG BÁN THUỐC TÂY (đề 23)
   ---------------------------------------------------------------------
   [10/11/2026 lần 5] Tạo bởi nhóm để buổi demo có số liệu "đầy" cho Dashboard,
   báo cáo Y/C 6-9, biểu đồ so sánh, lô thuốc FEFO, trả hàng, nhật ký.

   CÁCH CHẠY: chạy file CSDL_QuanLyNhaThuoc.sql (bản lần 5) TRƯỚC, rồi mở file
   này trong SSMS và bấm F5. Chỉ cần chạy 1 LẦN (chạy lại sẽ tự bỏ qua phần mô phỏng).

   NỘI DUNG
     A. Danh mục bổ sung: 3 ĐVT, 6 dạng điều chế, 9 nước SX, 5 NCC,
        12 công dụng, 4 nhân viên (NV04-NV06 bán hàng, TK002 thủ kho),
        40 khách hàng (KH003 - KH042).
     B. 50 thuốc mới TH101 - TH150 (có mã vạch, công dụng; 16 thuốc kê đơn),
        tồn kho = 0 (hàng vào kho qua phiếu nhập ở phần C).
     C. Mô phỏng 12 THÁNG gần nhất (tính đến ngày chạy script):
        - Ngày đầu: 1 phiếu nhập đủ mọi thuốc; ngày 5 và 20 hằng tháng: 1 phiếu
          nhập 8 thuốc (ưu tiên thuốc sắp hết) -> mỗi dòng = 1 LÔ (số lô, NSX, HSD).
          Một số lô nhập đầu kỳ hạn ngắn -> nay ĐÃ HẾT HẠN (demo "Hủy lô hết hạn");
          vài lô nhập gần đây sắp hết hạn (demo cảnh báo vàng).
        - Khoảng 200 hóa đơn bán (mùa lạnh T12-T3 bán nhiều hơn mùa hè), 1-4
          thuốc / hóa đơn, xuất kho bằng sp_XuatKhoFEFO, 7 nhân viên, 42 khách,
          tích điểm theo đúng quy định, thuốc kê đơn có mã đơn thuốc.
        - 1 lần tăng giá (tháng thứ 7), 2 phiếu trả hàng 1 phần + 1 hóa đơn bị hủy.
        - Nhật ký: đăng nhập, thanh toán, nhập hàng, đổi giá, trả hàng.
     Mật khẩu NV mới = mã NV + '11' (băm SHA-256 + salt), không bắt đổi khi demo.
   Mọi số ngẫu nhiên dùng NEWID() nên mỗi lần cài dữ liệu sẽ hơi khác nhau.
   ===================================================================== */
USE QuanLyNhaThuoc;
GO
SET NOCOUNT ON;
GO

IF OBJECT_ID('LoThuoc', 'U') IS NULL OR OBJECT_ID('sp_XuatKhoFEFO', 'P') IS NULL
BEGIN
    RAISERROR(N'Chưa có bảng LoThuoc / sp_XuatKhoFEFO. Hãy chạy file CSDL_QuanLyNhaThuoc.sql (bản lần 5) trước.', 16, 1);
    SET NOEXEC ON;
END
GO

/* =====================================================================
   A. DANH MỤC BỔ SUNG (chỉ thêm khi chưa có)
   ===================================================================== */
INSERT INTO DonViTinh (MaDVT, TenDVT)
SELECT v.MaDVT, v.TenDVT
FROM (VALUES
    ('DVT04', N'Tuýp'),
    ('DVT05', N'Gói'),
    ('DVT06', N'Lọ')
) AS v (MaDVT, TenDVT)
WHERE NOT EXISTS (SELECT 1 FROM DonViTinh x WHERE x.MaDVT = v.MaDVT);

INSERT INTO DangDieuChe (MaDangDC, TenDangDC)
SELECT v.MaDangDC, v.TenDangDC
FROM (VALUES
    ('DC04', N'Viên sủi'),
    ('DC05', N'Kem bôi'),
    ('DC06', N'Dung dịch'),
    ('DC07', N'Bột pha uống'),
    ('DC08', N'Thuốc nhỏ mắt'),
    ('DC09', N'Miếng dán')
) AS v (MaDangDC, TenDangDC)
WHERE NOT EXISTS (SELECT 1 FROM DangDieuChe x WHERE x.MaDangDC = v.MaDangDC);

INSERT INTO NuocSX (MaNuocSX, TenNuocSX)
SELECT v.MaNuocSX, v.TenNuocSX
FROM (VALUES
    ('NSX03', N'Pháp'),
    ('NSX04', N'Ấn Độ'),
    ('NSX05', N'Hàn Quốc'),
    ('NSX06', N'Mỹ'),
    ('NSX07', N'Anh'),
    ('NSX08', N'Hungary'),
    ('NSX09', N'Nhật Bản'),
    ('NSX10', N'Thụy Sĩ'),
    ('NSX11', N'Đan Mạch')
) AS v (MaNuocSX, TenNuocSX)
WHERE NOT EXISTS (SELECT 1 FROM NuocSX x WHERE x.MaNuocSX = v.MaNuocSX);

INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, DienThoai)
SELECT v.MaNCC, v.TenNCC, v.DiaChi, v.DienThoai
FROM (VALUES
    ('NCC03', N'Công ty CP Dược phẩm Imexpharm', N'Đồng Tháp', '02773851941'),
    ('NCC04', N'Công ty CP Pymepharco', N'Phú Yên', '02573829165'),
    ('NCC05', N'Công ty TNHH Sanofi-Aventis Việt Nam', N'TP. Hồ Chí Minh', '02838298526'),
    ('NCC06', N'Công ty CP Dược phẩm OPC', N'TP. Hồ Chí Minh', '02838777133'),
    ('NCC07', N'Công ty CP Dược phẩm Hà Tây', N'Hà Nội', '02433522525')
) AS v (MaNCC, TenNCC, DiaChi, DienThoai)
WHERE NOT EXISTS (SELECT 1 FROM NhaCungCap x WHERE x.MaNCC = v.MaNCC);

-- Công dụng mới: thêm theo TÊN (mã CD31 - CD42), tên đã có thì bỏ qua
INSERT INTO CongDung (MaCongDung, TenCongDung)
SELECT v.MaCongDung, v.TenCongDung
FROM (VALUES
    ('CD31', N'Trị đau dạ dày - trào ngược'),
    ('CD32', N'Hạ huyết áp'),
    ('CD33', N'Điều trị tiểu đường'),
    ('CD34', N'Kháng nấm'),
    ('CD35', N'Sát khuẩn - vết thương'),
    ('CD36', N'Bổ gan'),
    ('CD37', N'Men vi sinh - đường ruột'),
    ('CD38', N'Nhỏ mắt - khô mắt'),
    ('CD39', N'An thần - ngủ ngon'),
    ('CD40', N'Hạ mỡ máu'),
    ('CD41', N'Bù nước - điện giải'),
    ('CD42', N'Chống say tàu xe')
) AS v (MaCongDung, TenCongDung)
WHERE NOT EXISTS (SELECT 1 FROM CongDung x WHERE x.MaCongDung = v.MaCongDung OR x.TenCongDung = v.TenCongDung);

-- Nhân viên mới (mật khẩu băm ở dưới)
INSERT INTO NhanVien (MaNV, TenNV, NgaySinh, GioiTinh, DiaChi, MaTrinhDo, MaChuyenMon, DienThoai, MaVaiTro)
SELECT v.MaNV, v.TenNV, v.NgaySinh, v.GioiTinh, v.DiaChi, v.MaTrinhDo, v.MaChuyenMon, v.DienThoai, v.MaVaiTro
FROM (VALUES
    ('NV04', N'Phạm Thị Hồng', '1998-02-14', N'Nữ', N'Cầu Giấy, Hà Nội', 'TD02', 'CM01', '0904111222', 'BH'),
    ('NV05', N'Đỗ Văn Nam', '1996-11-03', N'Nam', N'Đống Đa, Hà Nội', 'TD03', 'CM01', '0904333444', 'BH'),
    ('NV06', N'Vũ Thị Mai', '2000-06-25', N'Nữ', N'Hoàng Mai, Hà Nội', 'TD02', 'CM01', '0904555666', 'BH'),
    ('TK002', N'Bùi Văn Kiên', '1991-09-09', N'Nam', N'Thanh Xuân, Hà Nội', 'TD02', 'CM02', '0912000003', 'TK')
) AS v (MaNV, TenNV, NgaySinh, GioiTinh, DiaChi, MaTrinhDo, MaChuyenMon, DienThoai, MaVaiTro)
WHERE NOT EXISTS (SELECT 1 FROM NhanVien x WHERE x.MaNV = v.MaNV);

UPDATE NhanVien
SET MatKhauSalt = CONVERT(VARCHAR(32), CAST(NEWID() AS BINARY(16)), 2)
WHERE MatKhauHash IS NULL;
UPDATE NhanVien
SET MatKhauHash = CONVERT(VARCHAR(64),
        HASHBYTES('SHA2_256', CAST(MatKhauSalt AS NVARCHAR(32)) + CAST(RTRIM(MaNV) AS NVARCHAR(10)) + N'11'), 2),
    PhaiDoiMK = 0
WHERE MatKhauHash IS NULL;

-- 40 khách hàng
INSERT INTO KhachHang (MaKhach, TenKhach, DiaChi, DienThoai)
SELECT v.MaKhach, v.TenKhach, v.DiaChi, v.DienThoai
FROM (VALUES
    ('KH003', N'Nguyễn Thị Hương', N'Ba Đình, Hà Nội', '0981039867'),
    ('KH004', N'Phạm Đức Huy', N'Hoàn Kiếm, Hà Nội', '0981053156'),
    ('KH005', N'Lê Ngọc Vân', N'Đống Đa, Hà Nội', '0981066445'),
    ('KH006', N'Đỗ Quang Bình', N'Cầu Giấy, Hà Nội', '0981079734'),
    ('KH007', N'Hoàng Phương Yến', N'Thanh Xuân, Hà Nội', '0981093023'),
    ('KH008', N'Trần Thành Hải', N'Hai Bà Trưng, Hà Nội', '0981106312'),
    ('KH009', N'Võ Thị Thảo', N'Hoàng Mai, Hà Nội', '0981119601'),
    ('KH010', N'Đặng Đức Dũng', N'Long Biên, Hà Nội', '0981132890'),
    ('KH011', N'Bùi Ngọc Linh', N'Tây Hồ, Hà Nội', '0981146179'),
    ('KH012', N'Lý Quang Long', N'Hà Đông, Hà Nội', '0981159468'),
    ('KH013', N'Hồ Phương Hà', N'Nam Từ Liêm, Hà Nội', '0981172757'),
    ('KH014', N'Vũ Thành Sơn', N'Bắc Từ Liêm, Hà Nội', '0981186046'),
    ('KH015', N'Dương Thị Hương', N'Ba Đình, Hà Nội', '0981199335'),
    ('KH016', N'Ngô Đức Huy', N'Hoàn Kiếm, Hà Nội', '0981212624'),
    ('KH017', N'Nguyễn Ngọc Vân', N'Đống Đa, Hà Nội', '0981225913'),
    ('KH018', N'Phạm Quang Bình', N'Cầu Giấy, Hà Nội', '0981239202'),
    ('KH019', N'Lê Phương Yến', N'Thanh Xuân, Hà Nội', '0981252491'),
    ('KH020', N'Đỗ Thành Hải', N'Hai Bà Trưng, Hà Nội', '0981265780'),
    ('KH021', N'Hoàng Thị Thảo', N'Hoàng Mai, Hà Nội', '0981279069'),
    ('KH022', N'Trần Đức Dũng', N'Long Biên, Hà Nội', '0981292358'),
    ('KH023', N'Võ Ngọc Linh', N'Tây Hồ, Hà Nội', '0981305647'),
    ('KH024', N'Đặng Quang Long', N'Hà Đông, Hà Nội', '0981318936'),
    ('KH025', N'Bùi Phương Hà', N'Nam Từ Liêm, Hà Nội', '0981332225'),
    ('KH026', N'Lý Thành Sơn', N'Bắc Từ Liêm, Hà Nội', '0981345514'),
    ('KH027', N'Hồ Thị Hương', N'Ba Đình, Hà Nội', '0981358803'),
    ('KH028', N'Vũ Đức Huy', N'Hoàn Kiếm, Hà Nội', '0981372092'),
    ('KH029', N'Dương Ngọc Vân', N'Đống Đa, Hà Nội', '0981385381'),
    ('KH030', N'Ngô Quang Bình', N'Cầu Giấy, Hà Nội', '0981398670'),
    ('KH031', N'Nguyễn Phương Yến', N'Thanh Xuân, Hà Nội', '0981411959'),
    ('KH032', N'Phạm Thành Hải', N'Hai Bà Trưng, Hà Nội', '0981425248'),
    ('KH033', N'Lê Thị Thảo', N'Hoàng Mai, Hà Nội', '0981438537'),
    ('KH034', N'Đỗ Đức Dũng', N'Long Biên, Hà Nội', '0981451826'),
    ('KH035', N'Hoàng Ngọc Linh', N'Tây Hồ, Hà Nội', '0981465115'),
    ('KH036', N'Trần Quang Long', N'Hà Đông, Hà Nội', '0981478404'),
    ('KH037', N'Võ Phương Hà', N'Nam Từ Liêm, Hà Nội', '0981491693'),
    ('KH038', N'Đặng Thành Sơn', N'Bắc Từ Liêm, Hà Nội', '0981504982'),
    ('KH039', N'Bùi Thị Hương', N'Ba Đình, Hà Nội', '0981518271'),
    ('KH040', N'Lý Đức Huy', N'Hoàn Kiếm, Hà Nội', '0981531560'),
    ('KH041', N'Hồ Ngọc Vân', N'Đống Đa, Hà Nội', '0981544849'),
    ('KH042', N'Vũ Quang Bình', N'Cầu Giấy, Hà Nội', '0981558138')
) AS v (MaKhach, TenKhach, DiaChi, DienThoai)
WHERE NOT EXISTS (SELECT 1 FROM KhachHang x WHERE x.MaKhach = v.MaKhach);

GO

/* =====================================================================
   B. 50 THUỐC MỚI TH101 - TH150 (tồn kho 0, giá bán = 110% giá nhập)
   ===================================================================== */
INSERT INTO Thuoc (MaThuoc, TenThuoc, MaVach, MaDVT, MaDangDC, ThanhPhan, DonGiaNhap, GiaBan, SLHienCo,
                   NgaySX, MaNuocSX, HanSD, ChongChiDinh, KeDon)
SELECT v.MaThuoc, v.TenThuoc, v.MaVach, v.MaDVT, v.MaDangDC, v.ThanhPhan, v.Gia, ROUND(v.Gia * 1.1, 2), 0,
       NULL, v.MaNuocSX, NULL, v.ChongChiDinh, v.KeDon
FROM (VALUES
    ('TH101', N'Hapacol 650', '8938888000101', 'DVT01', 'DC01', N'Paracetamol 650mg', 42000, 'NSX01', 0, N'Suy gan nặng'),
    ('TH102', N'Efferalgan 500mg sủi', '8938888000102', 'DVT01', 'DC04', N'Paracetamol 500mg', 52000, 'NSX03', 0, N'Suy gan, phenylceton niệu'),
    ('TH103', N'Decolgen Forte', '8938888000103', 'DVT02', 'DC01', N'Paracetamol, Phenylephrin, Clorpheniramin', 9000, 'NSX01', 0, N'Tăng huyết áp nặng, bệnh mạch vành'),
    ('TH104', N'Tiffy Dey', '8938888000104', 'DVT02', 'DC01', N'Paracetamol, Clorpheniramin, Phenylephrin', 7000, 'NSX01', 0, N'Glaucom góc đóng'),
    ('TH105', N'Augmentin 625mg', '8938888000105', 'DVT01', 'DC01', N'Amoxicillin 500mg, Acid clavulanic 125mg', 210000, 'NSX03', 1, N'Dị ứng penicillin, tiền sử vàng da'),
    ('TH106', N'Cefuroxim 500mg', '8938888000106', 'DVT01', 'DC01', N'Cefuroxim axetil 500mg', 95000, 'NSX01', 1, N'Dị ứng cephalosporin'),
    ('TH107', N'Azithromycin 250mg', '8938888000107', 'DVT01', 'DC02', N'Azithromycin 250mg', 68000, 'NSX01', 1, N'Suy gan nặng'),
    ('TH108', N'Ciprofloxacin 500mg', '8938888000108', 'DVT01', 'DC01', N'Ciprofloxacin 500mg', 45000, 'NSX04', 1, N'Người dưới 18 tuổi, phụ nữ có thai'),
    ('TH109', N'Cephalexin 500mg', '8938888000109', 'DVT01', 'DC02', N'Cephalexin 500mg', 55000, 'NSX01', 1, N'Dị ứng cephalosporin'),
    ('TH110', N'Metronidazol 250mg', '8938888000110', 'DVT01', 'DC01', N'Metronidazol 250mg', 18000, 'NSX01', 1, N'3 tháng đầu thai kỳ'),
    ('TH111', N'Omeprazol 20mg', '8938888000111', 'DVT01', 'DC02', N'Omeprazol 20mg', 35000, 'NSX01', 0, N'Mẫn cảm với omeprazol'),
    ('TH112', N'Gaviscon', '8938888000112', 'DVT05', 'DC06', N'Natri alginat, Natri bicarbonat, Calci carbonat', 115000, 'NSX07', 0, N'Suy thận nặng'),
    ('TH113', N'Phosphalugel', '8938888000113', 'DVT05', 'DC06', N'Nhôm phosphat 20%', 98000, 'NSX03', 0, N'Suy thận nặng'),
    ('TH114', N'Smecta', '8938888000114', 'DVT01', 'DC07', N'Diosmectit 3g', 82000, 'NSX03', 0, N'Tắc ruột'),
    ('TH115', N'Oresol 245', '8938888000115', 'DVT05', 'DC07', N'Natri clorid, Kali clorid, Natri citrat, Glucose', 2500, 'NSX01', 0, N'Suy thận vô niệu'),
    ('TH116', N'Enterogermina', '8938888000116', 'DVT01', 'DC06', N'Bacillus clausii 2 tỷ bào tử', 125000, 'NSX03', 0, N'Mẫn cảm với thành phần thuốc'),
    ('TH117', N'Bio-acimin', '8938888000117', 'DVT01', 'DC07', N'Lactobacillus acidophilus, kẽm, vitamin', 78000, 'NSX01', 0, N'Mẫn cảm với thành phần thuốc'),
    ('TH118', N'Amlodipin 5mg', '8938888000118', 'DVT01', 'DC01', N'Amlodipin besilat 5mg', 30000, 'NSX01', 1, N'Sốc tim, hẹp động mạch chủ nặng'),
    ('TH119', N'Losartan 50mg', '8938888000119', 'DVT01', 'DC01', N'Losartan kali 50mg', 48000, 'NSX04', 1, N'Phụ nữ có thai'),
    ('TH120', N'Concor 5mg', '8938888000120', 'DVT01', 'DC01', N'Bisoprolol fumarat 5mg', 120000, 'NSX02', 1, N'Suy tim mất bù, nhịp tim chậm'),
    ('TH121', N'Metformin 500mg', '8938888000121', 'DVT01', 'DC01', N'Metformin HCl 500mg', 32000, 'NSX01', 1, N'Suy thận, nhiễm toan chuyển hóa'),
    ('TH122', N'Diamicron MR 30mg', '8938888000122', 'DVT01', 'DC01', N'Gliclazid 30mg', 145000, 'NSX03', 1, N'Tiểu đường type 1'),
    ('TH123', N'Atorvastatin 20mg', '8938888000123', 'DVT01', 'DC01', N'Atorvastatin 20mg', 65000, 'NSX04', 1, N'Bệnh gan tiến triển'),
    ('TH124', N'Rosuvastatin 10mg', '8938888000124', 'DVT01', 'DC01', N'Rosuvastatin 10mg', 85000, 'NSX05', 1, N'Phụ nữ có thai, cho con bú'),
    ('TH125', N'Cetirizin 10mg', '8938888000125', 'DVT01', 'DC01', N'Cetirizin dihydroclorid 10mg', 22000, 'NSX01', 0, N'Suy thận nặng'),
    ('TH126', N'Telfast 180mg', '8938888000126', 'DVT01', 'DC01', N'Fexofenadin HCl 180mg', 135000, 'NSX03', 0, N'Mẫn cảm với fexofenadin'),
    ('TH127', N'Clorpheniramin 4mg', '8938888000127', 'DVT06', 'DC01', N'Clorpheniramin maleat 4mg', 15000, 'NSX01', 0, N'Glaucom góc hẹp'),
    ('TH128', N'Prospan siro', '8938888000128', 'DVT03', 'DC03', N'Cao khô lá thường xuân', 88000, 'NSX02', 0, N'Mẫn cảm với thành phần thuốc'),
    ('TH129', N'Bổ phế Nam Hà', '8938888000129', 'DVT03', 'DC03', N'Cát cánh, Tỳ bà diệp, Cam thảo, Bách bộ', 38000, 'NSX01', 0, N'Người tiểu đường (siro có đường)'),
    ('TH130', N'Acetylcystein 200mg', '8938888000130', 'DVT01', 'DC07', N'Acetylcystein 200mg', 45000, 'NSX01', 0, N'Hen phế quản'),
    ('TH131', N'Strepsils', '8938888000131', 'DVT01', 'DC01', N'Amylmetacresol, Dichlorobenzyl alcohol', 42000, 'NSX07', 0, N'Trẻ dưới 6 tuổi'),
    ('TH132', N'Betadine 10%', '8938888000132', 'DVT03', 'DC06', N'Povidon iod 10%', 35000, 'NSX08', 0, N'Cường giáp, trẻ sơ sinh'),
    ('TH133', N'Oxy già 3%', '8938888000133', 'DVT03', 'DC06', N'Hydrogen peroxid 3%', 6000, 'NSX01', 0, N'Không bôi vào khoang kín'),
    ('TH134', N'Nước muối sinh lý 0,9%', '8938888000134', 'DVT03', 'DC06', N'Natri clorid 0,9%', 5000, 'NSX01', 0, N'Không có'),
    ('TH135', N'V.Rohto', '8938888000135', 'DVT06', 'DC08', N'Tetrahydrozolin, Vitamin B6, Kali L-aspartat', 52000, 'NSX01', 0, N'Glaucom'),
    ('TH136', N'Systane Ultra', '8938888000136', 'DVT06', 'DC08', N'Polyethylen glycol 400, Propylen glycol', 145000, 'NSX06', 0, N'Mẫn cảm với thành phần thuốc'),
    ('TH137', N'Tobrex', '8938888000137', 'DVT06', 'DC08', N'Tobramycin 0,3%', 55000, 'NSX06', 1, N'Mẫn cảm aminoglycosid'),
    ('TH138', N'Clotrimazol 1%', '8938888000138', 'DVT04', 'DC05', N'Clotrimazol 1%', 18000, 'NSX01', 0, N'Mẫn cảm với clotrimazol'),
    ('TH139', N'Fucidin 2%', '8938888000139', 'DVT04', 'DC05', N'Acid fusidic 2%', 98000, 'NSX11', 0, N'Mẫn cảm với acid fusidic'),
    ('TH140', N'Fluconazol 150mg', '8938888000140', 'DVT01', 'DC02', N'Fluconazol 150mg', 25000, 'NSX01', 1, N'Phụ nữ có thai'),
    ('TH141', N'Salonpas', '8938888000141', 'DVT01', 'DC09', N'Methyl salicylat, Menthol, Camphor', 35000, 'NSX09', 0, N'Vết thương hở, chàm'),
    ('TH142', N'Voltaren Emulgel 1%', '8938888000142', 'DVT04', 'DC05', N'Diclofenac diethylamin 1,16%', 115000, 'NSX10', 0, N'Mẫn cảm với NSAID'),
    ('TH143', N'Meloxicam 7,5mg', '8938888000143', 'DVT01', 'DC01', N'Meloxicam 7,5mg', 28000, 'NSX01', 1, N'Loét dạ dày, suy thận nặng'),
    ('TH144', N'Alpha Choay', '8938888000144', 'DVT01', 'DC01', N'Alphachymotrypsin 4200 IU', 48000, 'NSX01', 0, N'Mẫn cảm với thành phần thuốc'),
    ('TH145', N'Essentiale Forte', '8938888000145', 'DVT01', 'DC02', N'Phospholipid đậu nành 300mg', 210000, 'NSX02', 0, N'Mẫn cảm với thành phần thuốc'),
    ('TH146', N'Boganic', '8938888000146', 'DVT01', 'DC01', N'Cao Actiso, Rau đắng đất, Bìm bìm', 65000, 'NSX01', 0, N'Phụ nữ có thai'),
    ('TH147', N'Rotunda 30mg', '8938888000147', 'DVT01', 'DC01', N'Rotundin 30mg', 25000, 'NSX01', 0, N'Trầm cảm, phụ nữ có thai'),
    ('TH148', N'Dramamine', '8938888000148', 'DVT02', 'DC01', N'Dimenhydrinat 50mg', 12000, 'NSX01', 0, N'Glaucom, phì đại tuyến tiền liệt'),
    ('TH149', N'Berocca', '8938888000149', 'DVT04', 'DC04', N'Vitamin nhóm B, Vitamin C, Calci, Magie, Kẽm', 95000, 'NSX02', 0, N'Tăng calci máu'),
    ('TH150', N'Calci D Hasan', '8938888000150', 'DVT01', 'DC04', N'Calci carbonat 1250mg, Vitamin D3 440 IU', 55000, 'NSX01', 0, N'Tăng calci máu, sỏi thận')
) AS v (MaThuoc, TenThuoc, MaVach, MaDVT, MaDangDC, ThanhPhan, Gia, MaNuocSX, KeDon, ChongChiDinh)
WHERE NOT EXISTS (SELECT 1 FROM Thuoc x WHERE x.MaThuoc = v.MaThuoc)
  AND NOT EXISTS (SELECT 1 FROM Thuoc y WHERE y.MaVach = v.MaVach);

-- Gán công dụng theo tên
INSERT INTO Thuoc_CongDung (MaThuoc, MaCongDung)
SELECT DISTINCT v.MaThuoc, cd.MaCongDung
FROM (VALUES
    ('TH101', N'Giảm đau'),
    ('TH101', N'Hạ sốt'),
    ('TH102', N'Giảm đau'),
    ('TH102', N'Hạ sốt'),
    ('TH103', N'Hạ sốt'),
    ('TH103', N'Trị viêm mũi dị ứng'),
    ('TH104', N'Hạ sốt'),
    ('TH104', N'Chống dị ứng'),
    ('TH105', N'Kháng sinh - trị nhiễm khuẩn'),
    ('TH106', N'Kháng sinh - trị nhiễm khuẩn'),
    ('TH107', N'Kháng sinh - trị nhiễm khuẩn'),
    ('TH108', N'Kháng sinh - trị nhiễm khuẩn'),
    ('TH109', N'Kháng sinh - trị nhiễm khuẩn'),
    ('TH110', N'Kháng sinh - trị nhiễm khuẩn'),
    ('TH111', N'Trị đau dạ dày - trào ngược'),
    ('TH112', N'Trị đau dạ dày - trào ngược'),
    ('TH113', N'Trị đau dạ dày - trào ngược'),
    ('TH114', N'Trị tiêu chảy - rối loạn tiêu hóa'),
    ('TH115', N'Bù nước - điện giải'),
    ('TH115', N'Trị tiêu chảy - rối loạn tiêu hóa'),
    ('TH116', N'Men vi sinh - đường ruột'),
    ('TH116', N'Trị tiêu chảy - rối loạn tiêu hóa'),
    ('TH117', N'Men vi sinh - đường ruột'),
    ('TH118', N'Hạ huyết áp'),
    ('TH119', N'Hạ huyết áp'),
    ('TH120', N'Hạ huyết áp'),
    ('TH121', N'Điều trị tiểu đường'),
    ('TH122', N'Điều trị tiểu đường'),
    ('TH123', N'Hạ mỡ máu'),
    ('TH124', N'Hạ mỡ máu'),
    ('TH125', N'Chống dị ứng'),
    ('TH125', N'Trị viêm mũi dị ứng'),
    ('TH126', N'Chống dị ứng'),
    ('TH126', N'Trị viêm mũi dị ứng'),
    ('TH127', N'Chống dị ứng'),
    ('TH128', N'Giảm ho - long đờm'),
    ('TH129', N'Giảm ho - long đờm'),
    ('TH130', N'Giảm ho - long đờm'),
    ('TH131', N'Sát khuẩn - vết thương'),
    ('TH132', N'Sát khuẩn - vết thương'),
    ('TH133', N'Sát khuẩn - vết thương'),
    ('TH134', N'Sát khuẩn - vết thương'),
    ('TH134', N'Nhỏ mắt - khô mắt'),
    ('TH135', N'Nhỏ mắt - khô mắt'),
    ('TH136', N'Nhỏ mắt - khô mắt'),
    ('TH137', N'Kháng sinh - trị nhiễm khuẩn'),
    ('TH137', N'Nhỏ mắt - khô mắt'),
    ('TH138', N'Kháng nấm'),
    ('TH139', N'Sát khuẩn - vết thương'),
    ('TH140', N'Kháng nấm'),
    ('TH141', N'Giảm đau'),
    ('TH141', N'Kháng viêm'),
    ('TH142', N'Giảm đau'),
    ('TH142', N'Kháng viêm'),
    ('TH143', N'Giảm đau'),
    ('TH143', N'Kháng viêm'),
    ('TH144', N'Kháng viêm'),
    ('TH145', N'Bổ gan'),
    ('TH146', N'Bổ gan'),
    ('TH147', N'An thần - ngủ ngon'),
    ('TH148', N'Chống say tàu xe'),
    ('TH149', N'Bổ sung vitamin'),
    ('TH149', N'Tăng sức đề kháng'),
    ('TH150', N'Bổ sung vitamin')
) AS v (MaThuoc, TenCongDung)
JOIN Thuoc t     ON t.MaThuoc = v.MaThuoc
JOIN CongDung cd ON cd.TenCongDung = v.TenCongDung
WHERE NOT EXISTS (SELECT 1 FROM Thuoc_CongDung x WHERE x.MaThuoc = v.MaThuoc AND x.MaCongDung = cd.MaCongDung);
GO

/* =====================================================================
   C. MÔ PHỎNG 12 THÁNG NHẬP / BÁN (chỉ chạy khi chưa có phiếu nhập demo "PN...")
   ===================================================================== */
IF EXISTS (SELECT 1 FROM HoaDonNhap WHERE MaHDN LIKE 'PN[0-9][0-9][0-9][0-9][0-9][0-9]')
BEGIN
    PRINT N'Đã có dữ liệu mô phỏng (phiếu nhập PNyymmdd) -> bỏ qua phần C.';
    SET NOEXEC ON;
END
GO

-- Tắt trigger ghi nhật ký đổi giá trong lúc mô phỏng (nhật ký đổi giá được ghi đúng ngày ở dưới)
DISABLE TRIGGER trg_Thuoc_NhatKyGia ON Thuoc;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @HomNay   DATE     = CAST(GETDATE() AS DATE);
    DECLARE @BayGio   DATETIME = GETDATE();
    DECLARE @Dau      DATE     = DATEADD(MONTH, -11, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1));
    DECLARE @NgayTangGia DATE  = DATEADD(DAY, 4, DATEADD(MONTH, 6, @Dau));
    DECLARE @Ngay     DATE     = @Dau;

    DECLARE @SoHDB INT = ISNULL((SELECT MAX(TRY_CAST(SUBSTRING(MaHDB, 4, 10) AS INT)) FROM HoaDonBan WHERE MaHDB LIKE 'HDB%'), 0);
    DECLARE @MaHDB VARCHAR(10), @MaHDN VARCHAR(10), @MaNCC VARCHAR(10), @MaTK VARCHAR(10), @MaNV VARCHAR(10), @MaKhach VARCHAR(10);
    DECLARE @MaThuoc VARCHAR(10), @SoLo NVARCHAR(30);
    DECLARE @NgayNhap DATETIME, @NgayBan DATETIME, @NSX DATE, @HSD DATE;
    DECLARE @i INT, @n INT, @SL INT, @CoThe INT, @SoHD INT, @h INT, @SoDong INT, @d INT, @PhutMax INT, @Phut INT;
    DECLARE @Gia DECIMAL(18,2), @GiaBan DECIMAL(18,2), @GG DECIMAL(18,2), @Tong DECIMAL(18,2);
    DECLARE @SoLan INT, @Diem INT, @HeSo DECIMAL(5,2);
    DECLARE @r INT;      -- số ngẫu nhiên phải tính TRƯỚC rồi mới dùng trong WHERE (NEWID() trong WHERE bị tính lại cho từng dòng)

    -- Thuốc tham gia mô phỏng: 50 thuốc mới + thuốc mẫu của script chính (không dùng thuốc [Test])
    DECLARE @DSThuoc TABLE (MaThuoc VARCHAR(10) PRIMARY KEY, Gia DECIMAL(18,2), KeDon BIT, PhoBien INT);
    INSERT INTO @DSThuoc (MaThuoc, Gia, KeDon, PhoBien)
    SELECT t.MaThuoc, t.DonGiaNhap, t.KeDon, v.PhoBien
    FROM (VALUES
    ('TH101', 40),
    ('TH102', 30),
    ('TH103', 40),
    ('TH104', 35),
    ('TH105', 15),
    ('TH106', 15),
    ('TH107', 12),
    ('TH108', 8),
    ('TH109', 10),
    ('TH110', 6),
    ('TH111', 30),
    ('TH112', 20),
    ('TH113', 20),
    ('TH114', 30),
    ('TH115', 35),
    ('TH116', 18),
    ('TH117', 18),
    ('TH118', 20),
    ('TH119', 15),
    ('TH120', 12),
    ('TH121', 18),
    ('TH122', 12),
    ('TH123', 12),
    ('TH124', 10),
    ('TH125', 30),
    ('TH126', 15),
    ('TH127', 15),
    ('TH128', 35),
    ('TH129', 30),
    ('TH130', 20),
    ('TH131', 25),
    ('TH132', 18),
    ('TH133', 15),
    ('TH134', 40),
    ('TH135', 25),
    ('TH136', 12),
    ('TH137', 8),
    ('TH138', 12),
    ('TH139', 10),
    ('TH140', 8),
    ('TH141', 25),
    ('TH142', 15),
    ('TH143', 10),
    ('TH144', 25),
    ('TH145', 12),
    ('TH146', 20),
    ('TH147', 12),
    ('TH148', 12),
    ('TH149', 20),
    ('TH150', 18),
    ('TH001', 40),
    ('TH010', 12),
    ('TH011', 15),
    ('TH012', 20),
    ('TH013', 25),
    ('TH014', 20),
    ('TH015', 20)
    ) AS v (MaThuoc, PhoBien)
    JOIN Thuoc t ON t.MaThuoc = v.MaThuoc
    WHERE t.DonGiaNhap > 0;

    -- Nhân viên bán (lặp tên = bán nhiều hơn) và khách (KH001-KH010 là khách quen)
    DECLARE @NV TABLE (STT INT IDENTITY(1,1), MaNV VARCHAR(10));
    INSERT INTO @NV (MaNV)
    SELECT v.MaNV FROM (VALUES ('NV01'),('NV01'),('NV01'),('NV02'),('NV02'),('NV02'),('NV02'),('NV02'),('NV02'),
                               ('NV03'),('NV03'),('NV04'),('NV04'),('NV04'),('NV04'),('NV04'),('NV05'),('NV05'),('NV05'),
                               ('NV06'),('NV06'),('QL001')) v (MaNV)
    WHERE EXISTS (SELECT 1 FROM NhanVien x WHERE x.MaNV = v.MaNV);

    DECLARE @KH TABLE (STT INT IDENTITY(1,1), MaKhach VARCHAR(10));
    INSERT INTO @KH (MaKhach) SELECT MaKhach FROM KhachHang;
    INSERT INTO @KH (MaKhach) SELECT MaKhach FROM KhachHang WHERE MaKhach BETWEEN 'KH001' AND 'KH010';
    INSERT INTO @KH (MaKhach) SELECT MaKhach FROM KhachHang WHERE MaKhach BETWEEN 'KH001' AND 'KH010';

    DECLARE @SoNV INT = (SELECT COUNT(*) FROM @NV), @SoKH INT = (SELECT COUNT(*) FROM @KH);
    DECLARE @Nhap TABLE (STT INT IDENTITY(1,1), MaThuoc VARCHAR(10));

    WHILE @Ngay <= @HomNay
    BEGIN
        -- ================= 1. NHẬP HÀNG =================
        IF @Ngay = @Dau OR DAY(@Ngay) IN (5, 20)
        BEGIN
            -- tháng thứ 7: nhà cung cấp tăng giá 3 thuốc (ghi nhật ký đổi giá đúng ngày)
            IF @Ngay = @NgayTangGia
            BEGIN
                INSERT INTO NhatKyHoatDong (ThoiGian, MaNV, TenNV, HanhDong, DoiTuong, ChiTiet, MayTinh)
                SELECT DATEADD(MINUTE, 515, CAST(@Ngay AS DATETIME)), 'TK001', nv.TenNV, N'Đổi giá thuốc', d.MaThuoc,
                       t.TenThuoc + N': giá nhập ' + FORMAT(d.Gia, 'N0') + N' -> ' + FORMAT(ROUND(d.Gia * 1.08, -2), 'N0')
                                  + N', giá bán ' + FORMAT(ROUND(d.Gia * 1.1, 2), 'N0') + N' -> ' + FORMAT(ROUND(ROUND(d.Gia * 1.08, -2) * 1.1, 2), 'N0'),
                       N'QUAY-THUOC'
                FROM @DSThuoc d
                JOIN Thuoc t ON t.MaThuoc = d.MaThuoc
                LEFT JOIN NhanVien nv ON nv.MaNV = 'TK001'
                WHERE d.MaThuoc IN ('TH105', 'TH120', 'TH145');
                UPDATE @DSThuoc SET Gia = ROUND(Gia * 1.08, -2) WHERE MaThuoc IN ('TH105', 'TH120', 'TH145');
            END

            SET @MaHDN = 'PN' + CONVERT(VARCHAR(6), @Ngay, 12);                 -- PNyymmdd
            SET @NgayNhap = DATEADD(MINUTE, 435, CAST(@Ngay AS DATETIME));      -- 07:15 (trước giờ bán)
            SET @MaNCC = (SELECT TOP 1 MaNCC FROM NhaCungCap ORDER BY NEWID());
            SET @MaTK = CASE WHEN ABS(CHECKSUM(NEWID())) % 3 = 0 AND EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = 'TK002')
                             THEN 'TK002' ELSE 'TK001' END;

            INSERT INTO HoaDonNhap (MaHDN, NgayNhap, MaNV, MaNCC, TongTien) VALUES (@MaHDN, @NgayNhap, @MaTK, @MaNCC, 0);

            DELETE FROM @Nhap;
            IF @Ngay = @Dau
                INSERT INTO @Nhap (MaThuoc) SELECT MaThuoc FROM @DSThuoc ORDER BY MaThuoc;
            ELSE
                INSERT INTO @Nhap (MaThuoc)
                SELECT TOP 8 d.MaThuoc
                FROM @DSThuoc d
                ORDER BY (SELECT ISNULL(SUM(l.SLCon), 0) FROM LoThuoc l WHERE l.MaThuoc = d.MaThuoc AND l.HanSD > @Ngay)
                         + ABS(CHECKSUM(NEWID())) % 40;                          -- ưu tiên thuốc sắp hết

            SELECT @i = MIN(STT), @n = MAX(STT) FROM @Nhap;
            WHILE @i <= @n
            BEGIN
                SELECT @MaThuoc = n.MaThuoc, @Gia = d.Gia
                FROM @Nhap n JOIN @DSThuoc d ON d.MaThuoc = n.MaThuoc
                WHERE n.STT = @i;

                SET @SL  = CASE WHEN @Ngay = @Dau THEN 40 + ABS(CHECKSUM(NEWID())) % 50 ELSE 20 + ABS(CHECKSUM(NEWID())) % 40 END;
                SET @NSX = DATEADD(DAY, -(20 + ABS(CHECKSUM(NEWID())) % 150), @Ngay);
                SET @HSD = DATEADD(MONTH, 12 + ABS(CHECKSUM(NEWID())) % 25, @Ngay);
                -- lô hạn ngắn ở 2 tháng đầu -> nay đã hết hạn (một phần đã bán theo FEFO)
                IF @Ngay < DATEADD(MONTH, 2, @Dau) AND ABS(CHECKSUM(NEWID())) % 5 = 0
                    SET @HSD = DATEADD(DAY, 150 + ABS(CHECKSUM(NEWID())) % 90, @Ngay);
                -- vài lô nhập gần đây sắp hết hạn (cảnh báo vàng)
                IF @Ngay >= DATEADD(MONTH, -2, @HomNay) AND ABS(CHECKSUM(NEWID())) % 8 = 0
                    SET @HSD = DATEADD(DAY, 10 + ABS(CHECKSUM(NEWID())) % 18, @HomNay);
                IF @HSD <= @Ngay SET @HSD = DATEADD(MONTH, 12, @Ngay);

                SET @SoLo = N'L' + CONVERT(NVARCHAR(6), @Ngay, 12) + RIGHT(@MaThuoc, 3);

                INSERT INTO ChiTietHDN (MaHDN, MaThuoc, SLNhap, DonGia, KhuyenMai, ThanhTien)
                VALUES (@MaHDN, @MaThuoc, @SL, @Gia, 0, @SL * @Gia);
                UPDATE Thuoc SET DonGiaNhap = @Gia, GiaBan = ROUND(@Gia * 1.1, 2) WHERE MaThuoc = @MaThuoc;
                EXEC sp_NhapLo @MaThuoc = @MaThuoc, @SoLo = @SoLo, @NgaySX = @NSX, @HanSD = @HSD, @SoLuong = @SL,
                               @DonGiaNhap = @Gia, @MaHDN = @MaHDN, @NgayNhap = @NgayNhap;
                SET @i = @i + 1;
            END

            INSERT INTO NhatKyHoatDong (ThoiGian, MaNV, TenNV, HanhDong, DoiTuong, ChiTiet, MayTinh)
            SELECT DATEADD(MINUTE, 5, @NgayNhap), @MaTK, nv.TenNV, N'Nhập hàng', @MaHDN,
                   N'Phiếu ' + @MaHDN + N': ' + CAST((SELECT COUNT(*) FROM @Nhap) AS NVARCHAR(10)) + N' lô, NCC ' + @MaNCC + N', tổng '
                   + FORMAT((SELECT SUM(ThanhTien) FROM ChiTietHDN WHERE MaHDN = @MaHDN), 'N0') + N' đ',
                   N'QUAY-KHO'
            FROM NhanVien nv WHERE nv.MaNV = @MaTK;
        END

        -- ================= 2. BÁN HÀNG =================
        -- mùa lạnh (T12 - T3) bán nhiều, mùa hè (T6 - T8) bán ít
        SET @SoHD = CASE WHEN MONTH(@Ngay) IN (12, 1, 2, 3) THEN (ABS(CHECKSUM(NEWID())) % 10) / 4
                         WHEN MONTH(@Ngay) IN (6, 7, 8)     THEN CASE WHEN ABS(CHECKSUM(NEWID())) % 10 < 3 THEN 1 ELSE 0 END
                         ELSE CASE WHEN ABS(CHECKSUM(NEWID())) % 10 < 5 THEN 1 ELSE 0 END END;
        IF @Ngay = @HomNay SET @SoHD = CASE WHEN @SoHD = 0 THEN 1 ELSE @SoHD END;   -- hôm nay luôn có doanh thu

        -- giờ bán: 08:00 - 21:00 (hôm nay: không quá giờ hiện tại)
        SET @PhutMax = CASE WHEN @Ngay = @HomNay THEN DATEDIFF(MINUTE, CAST(@HomNay AS DATETIME), @BayGio) - 5 ELSE 1260 END;
        IF @PhutMax > 1260 SET @PhutMax = 1260;
        IF @PhutMax <= 480 SET @SoHD = 0;

        SET @h = 1;
        WHILE @h <= @SoHD
        BEGIN
            SET @SoHDB = @SoHDB + 1;
            SET @MaHDB = 'HDB' + CASE WHEN @SoHDB < 1000 THEN RIGHT('00' + CAST(@SoHDB AS VARCHAR(10)), 3)
                                      ELSE CAST(@SoHDB AS VARCHAR(10)) END;
            SET @Phut = 480 + ABS(CHECKSUM(NEWID())) % (@PhutMax - 480);
            SET @NgayBan = DATEADD(MINUTE, @Phut, CAST(@Ngay AS DATETIME));
            SET @r = 1 + ABS(CHECKSUM(NEWID())) % @SoNV;
            SET @MaNV = (SELECT MaNV FROM @NV WHERE STT = @r);
            SET @r = 1 + ABS(CHECKSUM(NEWID())) % @SoKH;
            SET @MaKhach = (SELECT MaKhach FROM @KH WHERE STT = @r);
            SET @GG = CASE WHEN ABS(CHECKSUM(NEWID())) % 10 = 0 THEN 5 ELSE 0 END;

            INSERT INTO HoaDonBan (MaHDB, NgayBan, MaNV, MaKhach, TongTien, DiemSuDung, TienGiamDiem, DiemCong)
            VALUES (@MaHDB, @NgayBan, @MaNV, @MaKhach, 0, 0, 0, 0);

            SET @SoDong = 1 + (ABS(CHECKSUM(NEWID())) % 10) / 3;               -- 1..4 thuốc
            SET @d = 1;
            WHILE @d <= @SoDong
            BEGIN
                SET @MaThuoc = NULL;
                SELECT TOP 1 @MaThuoc = d.MaThuoc, @CoThe = x.SL
                FROM @DSThuoc d
                CROSS APPLY (SELECT ISNULL(SUM(l.SLCon), 0) AS SL FROM LoThuoc l
                             WHERE l.MaThuoc = d.MaThuoc AND l.SLCon > 0 AND l.HanSD >= @Ngay) x
                WHERE x.SL > 0
                  AND NOT EXISTS (SELECT 1 FROM ChiTietHDB c WHERE c.MaHDB = @MaHDB AND c.MaThuoc = d.MaThuoc)
                ORDER BY ABS(CHECKSUM(NEWID())) % 100 - d.PhoBien;              -- thuốc phổ biến bán nhiều hơn
                IF @MaThuoc IS NULL BREAK;

                SET @SL = 1 + ABS(CHECKSUM(NEWID())) % 3;
                IF @SL > @CoThe SET @SL = @CoThe;
                SET @GiaBan = (SELECT GiaBan FROM Thuoc WHERE MaThuoc = @MaThuoc);

                INSERT INTO ChiTietHDB (MaHDB, MaThuoc, SoLuong, GiamGia, ThanhTien)
                VALUES (@MaHDB, @MaThuoc, @SL, @GG, ROUND(@SL * @GiaBan * (100 - @GG) / 100, 0));
                EXEC sp_XuatKhoFEFO @MaHDB = @MaHDB, @MaThuoc = @MaThuoc, @SoLuong = @SL, @NgayBan = @NgayBan;
                SET @d = @d + 1;
            END

            IF NOT EXISTS (SELECT 1 FROM ChiTietHDB WHERE MaHDB = @MaHDB)
            BEGIN
                DELETE FROM HoaDonBan WHERE MaHDB = @MaHDB;                    -- hết hàng: bỏ hóa đơn rỗng
                SET @SoHDB = @SoHDB - 1;
            END
            ELSE
            BEGIN
                -- thuốc kê đơn -> có mã đơn thuốc
                IF EXISTS (SELECT 1 FROM ChiTietHDB c JOIN Thuoc t ON t.MaThuoc = c.MaThuoc WHERE c.MaHDB = @MaHDB AND t.KeDon = 1)
                    UPDATE HoaDonBan
                    SET MaDonThuoc = N'DT' + CONVERT(NVARCHAR(6), @Ngay, 12) + RIGHT(N'00' + CAST(ABS(CHECKSUM(NEWID())) % 100 AS NVARCHAR(3)), 2)
                    WHERE MaHDB = @MaHDB;

                -- tích điểm: 10.000đ = 1 điểm x hệ số hạng (Bạc >= 5 lần: 1.2, Vàng >= 10 lần: 1.5), +20 ở lần mua thứ 5, 10...
                SET @Tong  = (SELECT SUM(ThanhTien) FROM ChiTietHDB WHERE MaHDB = @MaHDB);
                SET @SoLan = (SELECT COUNT(*) FROM HoaDonBan WHERE MaKhach = @MaKhach AND MaHDB <> @MaHDB AND DaHuy = 0);
                SET @HeSo  = CASE WHEN @SoLan >= 10 THEN 1.5 WHEN @SoLan >= 5 THEN 1.2 ELSE 1.0 END;
                SET @Diem  = FLOOR(FLOOR(@Tong / 10000) * @HeSo) + CASE WHEN (@SoLan + 1) % 5 = 0 THEN 20 ELSE 0 END;
                UPDATE HoaDonBan SET DiemCong = @Diem WHERE MaHDB = @MaHDB;
                UPDATE KhachHang SET DiemTichLuy = DiemTichLuy + @Diem WHERE MaKhach = @MaKhach;

                INSERT INTO NhatKyHoatDong (ThoiGian, MaNV, TenNV, HanhDong, DoiTuong, ChiTiet, MayTinh)
                SELECT @NgayBan, @MaNV, nv.TenNV, N'Thanh toán', @MaHDB,
                       N'Khách ' + @MaKhach + N', ' + CAST((SELECT COUNT(*) FROM ChiTietHDB WHERE MaHDB = @MaHDB) AS NVARCHAR(5))
                       + N' loại thuốc, tổng ' + FORMAT(@Tong, 'N0') + N' đ', N'QUAY-1'
                FROM NhanVien nv WHERE nv.MaNV = @MaNV;
            END
            SET @h = @h + 1;
        END

        SET @Ngay = DATEADD(DAY, 1, @Ngay);
    END

    -- ================= 3. ĐĂNG NHẬP (mỗi NV bán hàng trong ngày đăng nhập 1 lần) =================
    INSERT INTO NhatKyHoatDong (ThoiGian, MaNV, TenNV, HanhDong, DoiTuong, ChiTiet, MayTinh)
    SELECT DATEADD(MINUTE, 450 + ABS(CHECKSUM(NEWID())) % 25, CAST(x.Ngay AS DATETIME)), x.MaNV, nv.TenNV, N'Đăng nhập', x.MaNV,
           N'Vai trò: ' + ISNULL(vt.TenVaiTro, N''), N'QUAY-1'
    FROM (SELECT DISTINCT CAST(h.NgayBan AS DATE) AS Ngay, h.MaNV
          FROM HoaDonBan h WHERE h.NgayBan >= @Dau
            AND EXISTS (SELECT 1 FROM NhatKyHoatDong k WHERE k.DoiTuong = h.MaHDB AND k.HanhDong = N'Thanh toán')) x
    JOIN NhanVien nv ON nv.MaNV = x.MaNV
    LEFT JOIN VaiTro vt ON vt.MaVaiTro = nv.MaVaiTro;

    -- vài lần đăng nhập sai (demo tô đỏ ở trang Nhật ký)
    INSERT INTO NhatKyHoatDong (ThoiGian, MaNV, TenNV, HanhDong, DoiTuong, ChiTiet, MayTinh)
    SELECT DATEADD(MINUTE, v.Phut, CAST(DATEADD(DAY, -v.TruocNgay, @HomNay) AS DATETIME)), v.MaNV, nv.TenNV,
           N'Đăng nhập sai', v.MaNV, N'Sai mật khẩu', N'QUAY-1'
    FROM (VALUES ('NV04', 3, 440), ('NV04', 3, 441), ('NV02', 12, 455), ('QL001', 20, 1290)) v (MaNV, TruocNgay, Phut)
    JOIN NhanVien nv ON nv.MaNV = v.MaNV;

    -- ================= 4. TRẢ HÀNG / HỦY HÓA ĐƠN =================
    DECLARE @MaPTH VARCHAR(10), @SoPTH INT, @Tien DECIMAL(18,2), @TienHoan DECIMAL(18,2), @DiemCong INT, @DiemTru INT, @DiemDung INT;
    SET @SoPTH = ISNULL((SELECT MAX(TRY_CAST(SUBSTRING(MaPTH, 4, 10) AS INT)) FROM PhieuTraHang WHERE MaPTH LIKE 'PTH%'), 0);

    -- 4a. 2 phiếu trả 1 phần: hóa đơn có >= 2 thuốc trong 25 ngày gần đây, trả 1 hộp của 1 thuốc
    DECLARE @Tra TABLE (STT INT IDENTITY(1,1), MaHDB VARCHAR(10));
    INSERT INTO @Tra (MaHDB)
    SELECT TOP 2 h.MaHDB FROM HoaDonBan h
    WHERE h.NgayBan >= DATEADD(DAY, -25, @HomNay) AND h.NgayBan < @HomNay AND h.DaHuy = 0
      AND (SELECT COUNT(*) FROM ChiTietHDB c WHERE c.MaHDB = h.MaHDB) >= 2
    ORDER BY NEWID();

    SET @i = 1;
    WHILE @i <= (SELECT COUNT(*) FROM @Tra)
    BEGIN
        SELECT @MaHDB = MaHDB FROM @Tra WHERE STT = @i;
        SELECT @MaKhach = MaKhach, @Tien = TongTien, @DiemCong = DiemCong, @NgayBan = NgayBan FROM HoaDonBan WHERE MaHDB = @MaHDB;
        SET @SoPTH = @SoPTH + 1;
        SET @MaPTH = 'PTH' + RIGHT('00' + CAST(@SoPTH AS VARCHAR(10)), 3);
        SET @MaThuoc = (SELECT TOP 1 MaThuoc FROM ChiTietHDB WHERE MaHDB = @MaHDB ORDER BY ThanhTien);

        INSERT INTO PhieuTraHang (MaPTH, MaHDB, NgayTra, MaNV, LyDo, TienHoan, DiemTru, DiemHoan, LaHuyHoaDon)
        VALUES (@MaPTH, @MaHDB, DATEADD(DAY, 1, @NgayBan), 'NV02', N'Khách mua dư, hộp còn nguyên tem', 0, 0, 0, 0);
        EXEC sp_TraHangDong @MaPTH = @MaPTH, @MaHDB = @MaHDB, @MaThuoc = @MaThuoc, @SoLuong = 1, @TienHoan = @TienHoan OUTPUT;

        SET @DiemTru = CASE WHEN @Tien > 0 THEN FLOOR(@DiemCong * @TienHoan / @Tien) ELSE 0 END;
        UPDATE PhieuTraHang SET TienHoan = @TienHoan, DiemTru = @DiemTru WHERE MaPTH = @MaPTH;
        UPDATE KhachHang SET DiemTichLuy = CASE WHEN DiemTichLuy > @DiemTru THEN DiemTichLuy - @DiemTru ELSE 0 END WHERE MaKhach = @MaKhach;

        INSERT INTO NhatKyHoatDong (ThoiGian, MaNV, TenNV, HanhDong, DoiTuong, ChiTiet, MayTinh)
        SELECT DATEADD(DAY, 1, @NgayBan), 'NV02', nv.TenNV, N'Trả hàng', @MaHDB,
               @MaPTH + N': ' + @MaThuoc + N' x1, hoàn ' + FORMAT(@TienHoan, 'N0') + N' đ. Lý do: Khách mua dư', N'QUAY-1'
        FROM NhanVien nv WHERE nv.MaNV = 'NV02';
        SET @i = @i + 1;
    END

    -- 4b. 1 hóa đơn bị quản lý hủy (nhập nhầm khách) trong 10 ngày gần đây
    SET @MaHDB = (SELECT TOP 1 h.MaHDB FROM HoaDonBan h
                  WHERE h.NgayBan >= DATEADD(DAY, -10, @HomNay) AND h.NgayBan < @HomNay AND h.DaHuy = 0
                    AND NOT EXISTS (SELECT 1 FROM PhieuTraHang p WHERE p.MaHDB = h.MaHDB)
                  ORDER BY NEWID());
    IF @MaHDB IS NOT NULL
    BEGIN
        SELECT @MaKhach = MaKhach, @DiemCong = DiemCong, @DiemDung = DiemSuDung, @NgayBan = NgayBan FROM HoaDonBan WHERE MaHDB = @MaHDB;
        SET @SoPTH = @SoPTH + 1;
        SET @MaPTH = 'PTH' + RIGHT('00' + CAST(@SoPTH AS VARCHAR(10)), 3);
        INSERT INTO PhieuTraHang (MaPTH, MaHDB, NgayTra, MaNV, LyDo, TienHoan, DiemTru, DiemHoan, LaHuyHoaDon)
        VALUES (@MaPTH, @MaHDB, DATEADD(HOUR, 2, @NgayBan), 'QL001', N'Nhập nhầm hóa đơn, khách chưa nhận hàng', 0, 0, 0, 1);

        DECLARE @Dong TABLE (STT INT IDENTITY(1,1), MaThuoc VARCHAR(10), SoLuong INT);
        INSERT INTO @Dong (MaThuoc, SoLuong) SELECT MaThuoc, SoLuong FROM ChiTietHDB WHERE MaHDB = @MaHDB AND SoLuong > 0;
        SET @Tien = 0;
        SET @i = 1;
        WHILE @i <= (SELECT COUNT(*) FROM @Dong)
        BEGIN
            SELECT @MaThuoc = MaThuoc, @SL = SoLuong FROM @Dong WHERE STT = @i;
            EXEC sp_TraHangDong @MaPTH = @MaPTH, @MaHDB = @MaHDB, @MaThuoc = @MaThuoc, @SoLuong = @SL, @TienHoan = @TienHoan OUTPUT;
            SET @Tien = @Tien + @TienHoan;
            SET @i = @i + 1;
        END
        UPDATE PhieuTraHang SET TienHoan = @Tien, DiemTru = @DiemCong, DiemHoan = @DiemDung WHERE MaPTH = @MaPTH;
        UPDATE HoaDonBan SET DaHuy = 1, NgayHuy = DATEADD(HOUR, 2, @NgayBan), MaNVHuy = 'QL001',
                             LyDoHuy = N'Nhập nhầm hóa đơn, khách chưa nhận hàng'
        WHERE MaHDB = @MaHDB;
        UPDATE KhachHang SET DiemTichLuy = CASE WHEN DiemTichLuy - @DiemCong + @DiemDung > 0
                                                THEN DiemTichLuy - @DiemCong + @DiemDung ELSE 0 END
        WHERE MaKhach = @MaKhach;

        INSERT INTO NhatKyHoatDong (ThoiGian, MaNV, TenNV, HanhDong, DoiTuong, ChiTiet, MayTinh)
        SELECT DATEADD(HOUR, 2, @NgayBan), 'QL001', nv.TenNV, N'Hủy hóa đơn', @MaHDB,
               @MaPTH + N': hủy cả hóa đơn, hoàn ' + FORMAT(@Tien, 'N0') + N' đ. Lý do: Nhập nhầm hóa đơn', N'QUAY-QL'
        FROM NhanVien nv WHERE nv.MaNV = 'QL001';
    END

    EXEC sp_DongBoThuocTuLo NULL;
    COMMIT TRANSACTION;

    PRINT N'=== Đã tạo dữ liệu demo ===';
    SELECT (SELECT COUNT(*) FROM HoaDonBan WHERE NgayBan >= DATEADD(MONTH, -12, GETDATE())) AS SoHoaDonBan_12Thang,
           (SELECT COUNT(*) FROM HoaDonNhap WHERE MaHDN LIKE 'PN%')                         AS SoPhieuNhapDemo,
           (SELECT COUNT(*) FROM LoThuoc)                                                     AS SoLoThuoc,
           (SELECT COUNT(*) FROM LoThuoc WHERE SLCon > 0 AND HanSD < CAST(GETDATE() AS DATE)) AS SoLoDaHetHan,
           (SELECT COUNT(*) FROM Thuoc)                                                       AS SoThuoc,
           (SELECT COUNT(*) FROM KhachHang)                                                   AS SoKhachHang;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    DECLARE @Loi NVARCHAR(2000) = ERROR_MESSAGE();
    RAISERROR(N'Tạo dữ liệu demo thất bại, đã hủy toàn bộ phần C: %s', 16, 1, @Loi);
END CATCH
GO

ENABLE TRIGGER trg_Thuoc_NhatKyGia ON Thuoc;
GO
SET NOEXEC OFF;
GO