. Bạn của bạn phải chuyển sang nhánh Tu. Clone hay pull về thì mặc định là nhánh main, mà main chưa có code mới (PR chưa merge). Các bước cho bạn của bạn:

Nếu đã có repo trên máy thì mở Visual Studio, vào Git → Fetch. Chưa có thì clone repo về.
Ở thanh dưới cùng, bấm tên nhánh, vào tab Remotes, chọn origin/Tu rồi Checkout.
Chuột phải vào Solution, chọn Restore NuGet Packages, rồi Rebuild.
Mở SSMS, chạy CSDL_QuanLyNhaThuoc.sql bằng F5. Nếu có gạch đỏ thì nhấn Ctrl+Shift+R.
Chạy app, đăng nhập bằng QL001 / QL00111.
