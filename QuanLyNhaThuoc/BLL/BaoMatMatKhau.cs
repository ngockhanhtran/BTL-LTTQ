using System;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyNhaThuoc.BLL
{
    /// <summary>
    /// [10/11/2026 lần 5] Băm mật khẩu SHA-256 có salt.
    ///   Hash = SHA256( UTF-16LE(Salt + MatKhau) ), ghi dạng hex chữ HOA (64 ký tự)
    ///   Salt = 16 byte ngẫu nhiên (RNGCryptoServiceProvider), dạng hex HOA (32 ký tự), mỗi NV một salt.
    /// Giống hệt cách SQL tính: CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', CAST(@Salt AS NVARCHAR(32)) + @MatKhau), 2)
    /// nên mật khẩu đặt trong script SQL và trong app đều đăng nhập được.
    /// CSDL chỉ lưu Hash + Salt -> không ai (kể cả quản lý) xem lại được mật khẩu, chỉ đặt lại.
    /// Salt giúp 2 NV trùng mật khẩu vẫn có Hash khác nhau, chống tra bảng băm sẵn (rainbow table).
    /// </summary>
    public static class BaoMatMatKhau
    {
        public static string TaoSalt()
        {
            byte[] b = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                rng.GetBytes(b);
            return ChuyenHex(b);
        }

        public static string Bam(string matKhau, string salt)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] duLieu = Encoding.Unicode.GetBytes((salt ?? "") + (matKhau ?? ""));
                return ChuyenHex(sha.ComputeHash(duLieu));
            }
        }

        /// <summary>So mật khẩu nhập với Hash đã lưu (so sánh thời gian cố định).</summary>
        public static bool KiemTra(string matKhau, string salt, string hashDaLuu)
        {
            if (string.IsNullOrEmpty(hashDaLuu) || string.IsNullOrEmpty(salt)) return false;
            string hash = Bam(matKhau, salt);
            string luu = hashDaLuu.Trim().ToUpperInvariant();
            if (hash.Length != luu.Length) return false;
            int khac = 0;
            for (int i = 0; i < hash.Length; i++) khac |= hash[i] ^ luu[i];
            return khac == 0;
        }

        private static string ChuyenHex(byte[] b)
        {
            StringBuilder sb = new StringBuilder(b.Length * 2);
            foreach (byte x in b) sb.Append(x.ToString("X2"));
            return sb.ToString();
        }
    }
}
