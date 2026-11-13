# Tính năng nâng cao – hướng dẫn cài đặt và kịch bản demo

Cập nhật 11/11/2026 (code: "Sửa lần 8", SQL: "lần 7"). Mọi chỗ sửa trong code có ghi chú `[11/11/2026 lần 7]`.

## 1. Cài đặt (làm 1 lần trên mỗi máy)

1. Visual Studio: chuột phải Solution → **Restore NuGet Packages** (thêm `ZXing.Net 0.16.9`, `AForge.Video.DirectShow 2.2.5`) → **Rebuild**.
   Nếu báo thiếu `zxing` hoặc `AForge`: Tools → NuGet Package Manager → Package Manager Console, chạy
   `Update-Package -reinstall ZXing.Net` và `Update-Package -reinstall AForge.Video.DirectShow`.
2. SSMS: chạy lại `CSDL_QuanLyNhaThuoc.sql` (F5 cả file). Chạy lại bao nhiêu lần cũng được, không mất dữ liệu.
3. Đã chạy `DuLieuDemo.sql` trước đó thì chạy lại file CSDL thêm 1 lần (hoặc bấm "Nạp dữ liệu mẫu" ở trang Tương tác thuốc) để có đủ cặp tương tác mẫu cho thuốc TH101–TH150.
4. (Tùy chọn) `ThuRangBuoc.sql`: thử 11 câu sửa dữ liệu sai, cột DanhGia phải toàn "Đúng".

## 2. Kịch bản demo gợi ý (khoảng 10 phút)

| # | Tính năng | Ở đâu | Làm gì khi demo |
|---|-----------|-------|-----------------|
| 1 | Tìm không dấu | Tra cứu thuốc | Gõ `giam dau` → ra thuốc "Giảm đau"; gõ `para` → Paracetamol |
| 2 | Gợi ý khi gõ | Bán hàng, ô quét mã | Gõ `hapa` → danh sách hiện ra, mũi tên xuống + Enter là thêm vào đơn |
| 3 | Index + đo tốc độ | Menu NÂNG CAO → Hiệu năng truy vấn | "Sinh dữ liệu giả" 100.000 hóa đơn → "Đo trước / sau" → bảng + biểu đồ "Trước x ms → Sau y ms". Xong bấm "Xóa dữ liệu giả" |
| 4 | Không đơ (async) | Báo cáo, Dashboard | Khi đang có 100.000 hóa đơn, mở Dashboard: vòng xoay "Đang tải…", vẫn bấm được menu |
| 5 | Dự báo nhập hàng | NÂNG CAO → Dự báo nhập hàng | Đọc dòng đỏ "… còn đủ bán 6 ngày, nên nhập 96 hộp" → tích chọn → "Lập phiếu nhập Excel" → trang Nhập hàng → Import file đó |
| 6 | Gợi ý mua kèm | Bán hàng | Thêm 1 thuốc bán chạy → dải xanh "💡 Khách hay mua kèm: …" → bấm để thêm |
| 7 | Tương tác thuốc | Bán hàng | Thêm Hapacol 650 (TH101) + Decolgen Forte (TH103) → dải đỏ; bấm Thanh toán → hộp xác nhận |
| 8 | Quét webcam | Bán hàng → "📷 Camera" | Đưa hộp thuốc / tem in ra trước camera laptop → thuốc nhảy vào hóa đơn |
| 9 | In tem mã vạch / QR | NÂNG CAO → In tem mã vạch | Tích thuốc chưa có mã → "Cấp mã vạch" → "Xem trước & In"; quét tem vừa in bằng webcam (khép kín) |
| 10 | Unit test | Test → Test Explorer → Run All | 42 test xanh |
| 11 | GitHub Actions | Tab Actions trên GitHub | Mỗi lần push có dấu ✓ xanh build + test |

## 3. Giải thích ngắn khi giảng viên hỏi

- **Vì sao không dùng collation `Vietnamese_CI_AI`?** Collation này coi "đ" là chữ riêng, khác "d", nên gõ `giam dau` sẽ không ra "Giảm đau". Nhóm viết hàm `fn_BoDau`: chữ thường, bỏ dấu, đổi đ thành d, xử lý cả kiểu gõ Unicode tổ hợp.
- **Index giúp gì?** Không có index theo `NgayBan`, SQL Server phải đọc toàn bộ bảng hóa đơn (Clustered Index Scan). Có index thì nhảy thẳng tới đúng ngày (Index Seek). `INCLUDE (TongTien, …)` giúp khỏi quay lại bảng. Thời gian đo là trung vị của 5 lần chạy, sau 1 lần chạy làm nóng.
- **Gợi ý mua kèm tính thế nào?** Đếm số hóa đơn (1 năm gần đây, bỏ hóa đơn hủy) có cả thuốc A và thuốc B. Tỷ lệ = số hóa đơn có cả A và B ÷ số hóa đơn có A. Đây là độ tin cậy của luật kết hợp A → B trong khai phá dữ liệu.
- **Dự báo nhập hàng tính thế nào?**
  - Bán/ngày = đã bán N ngày ÷ N.
  - Đủ bán = tồn còn hạn ÷ bán/ngày.
  - Nên nhập = ⌈bán/ngày × số ngày dự trữ⌉ − tồn.
  - Công thức có unit test.
- **Mã vạch nội bộ?** Dùng EAN-13 đầu số 200–299. GS1 dành dải này cho dùng nội bộ cửa hàng nên không trùng mã thật của nhà sản xuất. Chữ số cuối là số kiểm tra.
- **Dữ liệu tương tác thuốc** là dữ liệu mẫu để demo, không phải tra cứu y khoa (có ghi rõ trên giao diện).

## 4. Lưu ý

- Hóa đơn giả `PF…` nằm ở năm 2018–2022, không trừ kho. Dự báo nhập hàng và gợi ý mua kèm chỉ xét dữ liệu gần đây nên không bị ảnh hưởng. Nhớ bấm "Xóa dữ liệu giả" sau khi demo.
- Webcam cần quyền camera của Windows (Settings → Privacy → Camera → cho phép ứng dụng desktop).
