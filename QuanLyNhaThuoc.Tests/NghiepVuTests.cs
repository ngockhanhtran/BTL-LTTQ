using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DTO;

namespace QuanLyNhaThuoc.Tests
{
    /// <summary>Tích điểm (DTO/TichDiemDTO.cs: 10.000đ = 1 điểm, Bạc x1.2, Vàng x1.5, thưởng 20 điểm mỗi lần mua thứ 5).</summary>
    [TestClass]
    public class TichDiemTests
    {
        [TestMethod]
        public void KhachMoi_100k_Duoc10Diem()
        {
            Assert.AreEqual(10, TichDiemBLL.TinhDiemCong(100000m, 0));
        }

        [TestMethod]
        public void LanMuaThu5_DuocThuong20Diem()
        {
            // đã mua 4 lần -> lần này là lần thứ 5: hạng Thành viên x1.0 + 20 thưởng
            Assert.AreEqual(30, TichDiemBLL.TinhDiemCong(100000m, 4));
        }

        [TestMethod]
        public void HangBac_NhanHeSo1_2()
        {
            Assert.AreEqual("Bạc", TichDiemBLL.TenHang(5));
            Assert.AreEqual(12, TichDiemBLL.TinhDiemCong(100000m, 5));
        }

        [TestMethod]
        public void HangVang_TuLanMua10()
        {
            Assert.AreEqual("Vàng", TichDiemBLL.TenHang(10));
            Assert.AreEqual(15, TichDiemBLL.TinhDiemCong(100000m, 10));
        }

        [TestMethod]
        public void DoiDiem_DuoiToiThieu_KhongDoiDuoc()
        {
            Assert.AreEqual(0, TichDiemBLL.DiemDoiToiDa(19, 1000000m));
        }

        [TestMethod]
        public void DoiDiem_ToiDa50PhanTramHoaDon()
        {
            // 100.000đ x 50% / 500đ = 100 điểm dù khách có 1.000 điểm
            Assert.AreEqual(100, TichDiemBLL.DiemDoiToiDa(1000, 100000m));
            Assert.AreEqual(30, TichDiemBLL.DiemDoiToiDa(30, 100000m));
        }

        [TestMethod]
        public void TienAm_KhongCongDiem()
        {
            Assert.AreEqual(0, TichDiemBLL.TinhDiemCong(-5000m, 0));
        }
    }

    /// <summary>Phân bổ giảm giá + đổi điểm về từng dòng hóa đơn (DTO/HoaDonTam.PhanBoThanhTien).</summary>
    [TestClass]
    public class PhanBoGiamGiaTests
    {
        private static HoaDonTam TaoHoaDon()
        {
            HoaDonTam hd = new HoaDonTam("HDB999");
            hd.ThemThuoc("TH001", "Panadol", 3, 16500m);
            hd.ThemThuoc("TH010", "Amoxicillin", 1, 27500m);
            hd.ThemThuoc("TH015", "Ibuprofen", 7, 11000m);
            return hd;
        }

        [TestMethod]
        public void KhongGiam_GiuNguyenThanhTien()
        {
            HoaDonTam hd = TaoHoaDon();
            CollectionAssert.AreEqual(hd.Dong.Select(d => d.ThanhTien).ToList(), hd.PhanBoThanhTien());
        }

        [TestMethod]
        public void GiamPhanTramVaDoiDiem_TongBangDungTongThanhToan()
        {
            HoaDonTam hd = TaoHoaDon();
            hd.GiamGiaPhanTram = 7.5m;
            hd.DiemSuDung = 37;
            List<decimal> pb = hd.PhanBoThanhTien();
            Assert.AreEqual(hd.TongThanhToan, pb.Sum(), "Tổng các dòng phải bằng đúng số khách trả");
            Assert.IsTrue(pb.All(x => x >= 0), "Không dòng nào âm");
            Assert.IsTrue(pb.All(x => x == Math.Floor(x)), "Thành tiền là số nguyên (đồng)");
        }

        [TestMethod]
        public void GiamGia_BiChanTrongKhoang0Den100()
        {
            HoaDonTam hd = TaoHoaDon();
            hd.GiamGiaPhanTram = 150;
            Assert.AreEqual(100m, hd.GiamGiaPhanTram);
            hd.GiamGiaPhanTram = -5;
            Assert.AreEqual(0m, hd.GiamGiaPhanTram);
        }

        [TestMethod]
        public void DoiDiemVuotTienHang_KhongAm()
        {
            HoaDonTam hd = TaoHoaDon();
            hd.DiemSuDung = 1000000;
            Assert.AreEqual(0m, hd.TongThanhToan);
            Assert.IsTrue(hd.PhanBoThanhTien().All(x => x >= 0));
        }

        [TestMethod]
        public void ThemThuocTrung_CongDonSoLuong()
        {
            HoaDonTam hd = TaoHoaDon();
            hd.ThemThuoc("TH001", "Panadol", 2, 16500m);
            Assert.AreEqual(3, hd.Dong.Count);
            Assert.AreEqual(5, hd.SoLuongCua("TH001"));
        }
    }

    /// <summary>Băm mật khẩu SHA-256 + salt - phải khớp với HASHBYTES('SHA2_256', ...) của SQL Server.</summary>
    [TestClass]
    public class BaoMatMatKhauTests
    {
        [TestMethod]
        public void Bam_KhopVoiGiaTriTinhSan()
        {
            // SHA256(UTF-16LE("ABC" + "QL00111")) - tính bằng công cụ khác để đối chiếu
            Assert.AreEqual("C6B91397D6BB1694475A59FCAFE2834833AD6E56FC136557FB9943B7E473BFE7",
                            BaoMatMatKhau.Bam("QL00111", "ABC"));
        }

        [TestMethod]
        public void KiemTra_DungSai()
        {
            string salt = BaoMatMatKhau.TaoSalt();
            string hash = BaoMatMatKhau.Bam("matkhau", salt);
            Assert.IsTrue(BaoMatMatKhau.KiemTra("matkhau", salt, hash));
            Assert.IsTrue(BaoMatMatKhau.KiemTra("matkhau", salt, hash.ToLowerInvariant()));
            Assert.IsFalse(BaoMatMatKhau.KiemTra("MatKhau", salt, hash));
            Assert.IsFalse(BaoMatMatKhau.KiemTra("matkhau", salt, null));
        }

        [TestMethod]
        public void Salt_32KyTuHex_VaKhacNhau()
        {
            string a = BaoMatMatKhau.TaoSalt(), b = BaoMatMatKhau.TaoSalt();
            Assert.AreEqual(32, a.Length);
            Assert.AreNotEqual(a, b);
        }
    }
}
