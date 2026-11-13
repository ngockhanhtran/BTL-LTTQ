using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLyNhaThuoc.BLL;
using QuanLyNhaThuoc.DAL;
using QuanLyNhaThuoc.DTO;
using QuanLyNhaThuoc.GUI;

namespace QuanLyNhaThuoc.Tests
{
    /// <summary>Tìm kiếm không dấu (giống dbo.fn_BoDau trong CSDL).</summary>
    [TestClass]
    public class ChuoiTiengVietTests
    {
        [TestMethod]
        public void BoDau_VaChuThuong()
        {
            Assert.AreEqual("giam dau", ChuoiTiengViet.BoDau("Giảm Đau"));
            Assert.AreEqual("thuoc ho thao duoc", ChuoiTiengViet.BoDau("THUỐC HO THẢO DƯỢC"));
            Assert.AreEqual("uong o", ChuoiTiengViet.BoDau("ưỡng ờ"));
        }

        [TestMethod]
        public void BoDau_KieuGoToHop()
        {
            // "Giảm" gõ kiểu tổ hợp: chữ a + dấu hỏi rời (U+0309)
            Assert.AreEqual("giam", ChuoiTiengViet.BoDau("Gia\u0309m"));
        }

        [TestMethod]
        public void TimKhongDau()
        {
            Assert.IsTrue(ChuoiTiengViet.ChuaKhongDau("Giảm đau, hạ sốt", "giam dau"));
            Assert.IsTrue(ChuoiTiengViet.ChuaKhongDau("Paracetamol 500mg", "PARA"));
            Assert.IsTrue(ChuoiTiengViet.ChuaKhongDau("Đau đầu", "dau dau"));
            Assert.IsFalse(ChuoiTiengViet.ChuaKhongDau("Vitamin C", "para"));
        }

        [TestMethod]
        public void Null_TraVeNull()
        {
            Assert.IsNull(ChuoiTiengViet.BoDau(null));
        }
    }

    /// <summary>Mã vạch EAN-13 (in tem cho thuốc chưa có mã vạch).</summary>
    [TestClass]
    public class MaVachTests
    {
        [TestMethod]
        public void ChuSoKiemTra_VDChuan()
        {
            Assert.AreEqual(1, MaVachBLL.ChuSoKiemTraEan13("400638133393"));   // 4006381333931
        }

        [TestMethod]
        public void HopLe_VaKhongHopLe()
        {
            Assert.IsTrue(MaVachBLL.LaEan13HopLe("4006381333931"));
            Assert.IsTrue(MaVachBLL.LaEan13HopLe("8934588012112"));             // mã mẫu của TH001 trong CSDL
            Assert.IsFalse(MaVachBLL.LaEan13HopLe("4006381333932"));            // sai số kiểm tra
            Assert.IsFalse(MaVachBLL.LaEan13HopLe("TH001"));
            Assert.IsFalse(MaVachBLL.LaEan13HopLe(null));
        }

        [TestMethod]
        public void MaNoiBo_DauTien_VaTiepTheo()
        {
            Assert.AreEqual("2000000000015", MaVachBLL.MaNoiBoTiepTheo(new string[0]));
            Assert.AreEqual("2000000000022", MaVachBLL.MaNoiBoTiepTheo(new[] { "2000000000015", "8934588012112", "rac" }));
        }

        [TestMethod]
        public void MaNoiBo_LuonHopLe()
        {
            List<string> da = new List<string>();
            for (int i = 0; i < 50; i++)
            {
                string m = MaVachBLL.MaNoiBoTiepTheo(da);
                Assert.IsTrue(MaVachBLL.LaEan13HopLe(m));
                Assert.IsTrue(m.StartsWith("200"));
                Assert.IsFalse(da.Contains(m));
                da.Add(m);
            }
        }

        [TestMethod]
        public void LoaiMa()
        {
            Assert.AreEqual("EAN-13", MaVachBLL.LoaiMaVach("8934588012112"));
            Assert.AreEqual("Code 128", MaVachBLL.LoaiMaVach("TH001"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ChuSoKiemTra_SaiDoDai_BaoLoi()
        {
            MaVachBLL.ChuSoKiemTraEan13("123");
        }
    }

    /// <summary>Dự báo nhập hàng từ tốc độ bán.</summary>
    [TestClass]
    public class DuBaoNhapHangTests
    {
        private static SoLieuBanThuoc SL(int ton, int daBan)
        {
            return new SoLieuBanThuoc { MaThuoc = "TH001", TenThuoc = "Panadol", TenDVT = "Hộp", TonBanDuoc = ton, DaBan = daBan, DonGiaNhap = 15000 };
        }

        [TestMethod]
        public void ViDu_ConDu6Ngay_NenNhap96()
        {
            // 30 ngày bán 120 hộp = 4 hộp/ngày, còn 24 hộp -> đủ 6 ngày; dự trữ 30 ngày cần 120 -> nhập 96
            KetQuaDuBao k = DuBaoNhapHangBLL.Tinh(SL(24, 120), 30, 30);
            Assert.AreEqual(4m, k.TocDoBanNgay);
            Assert.AreEqual(6m, k.SoNgayConDu);
            Assert.AreEqual(96, k.DeXuatNhap);
            Assert.AreEqual(MucDoDuBao.CanNhapGap, k.MucDo);
            StringAssert.Contains(k.LoiKhuyen, "còn đủ bán 6 ngày");
            StringAssert.Contains(k.LoiKhuyen, "nên nhập 96 hộp");
        }

        [TestMethod]
        public void HetHang()
        {
            KetQuaDuBao k = DuBaoNhapHangBLL.Tinh(SL(0, 30), 30, 30);
            Assert.AreEqual(MucDoDuBao.HetHang, k.MucDo);
            Assert.AreEqual(30, k.DeXuatNhap);
        }

        [TestMethod]
        public void NenNhap_VaDuHang()
        {
            Assert.AreEqual(MucDoDuBao.NenNhap, DuBaoNhapHangBLL.Tinh(SL(20, 30), 30, 30).MucDo);   // 1/ngày, đủ 20 ngày < 30
            Assert.AreEqual(10, DuBaoNhapHangBLL.Tinh(SL(20, 30), 30, 30).DeXuatNhap);
            KetQuaDuBao du = DuBaoNhapHangBLL.Tinh(SL(200, 30), 30, 30);
            Assert.AreEqual(MucDoDuBao.DuHang, du.MucDo);
            Assert.AreEqual(0, du.DeXuatNhap);
        }

        [TestMethod]
        public void KhongBan_KhongDeXuat()
        {
            KetQuaDuBao k = DuBaoNhapHangBLL.Tinh(SL(5, 0), 30, 30);
            Assert.AreEqual(MucDoDuBao.KhongBan, k.MucDo);
            Assert.IsNull(k.SoNgayConDu);
            Assert.AreEqual(0, k.DeXuatNhap);
        }

        [TestMethod]
        public void LamTronLen()
        {
            // 10 hộp / 30 ngày x 7 ngày dự trữ = 2,33 -> cần 3, tồn 1 -> nhập 2
            Assert.AreEqual(2, DuBaoNhapHangBLL.Tinh(SL(1, 10), 30, 7).DeXuatNhap);
        }

        [TestMethod]
        public void TangMa()
        {
            Assert.AreEqual("HDN011", DuBaoNhapHangBLL.TangMa("HDN009", 2));
            Assert.AreEqual("HDN100", DuBaoNhapHangBLL.TangMa("HDN099", 1));
            Assert.AreEqual("HDN005", DuBaoNhapHangBLL.TangMa("HDN005", 0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThamSoSai_BaoLoi()
        {
            DuBaoNhapHangBLL.KiemTraThamSo(3, 30);
        }
    }

    /// <summary>Tương tác thuốc.</summary>
    [TestClass]
    public class TuongTacThuocTests
    {
        [TestMethod]
        public void ChuanHoaCap_MaNhoTruoc()
        {
            string a, b;
            TuongTacThuocBLL.ChuanHoaCap("TH101", " th001 ", out a, out b);
            Assert.AreEqual("TH001", a);
            Assert.AreEqual("TH101", b);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ChuanHoaCap_HaiThuocGiongNhau_BaoLoi()
        {
            string a, b;
            TuongTacThuocBLL.ChuanHoaCap("TH001", "th001", out a, out b);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ChuanHoaCap_ThieuThuoc_BaoLoi()
        {
            string a, b;
            TuongTacThuocBLL.ChuanHoaCap("TH001", "", out a, out b);
        }

        [TestMethod]
        public void MucDoCaoNhat_VaThongBao()
        {
            List<CanhBaoTuongTac> ds = new List<CanhBaoTuongTac>
            {
                new CanhBaoTuongTac { TenThuoc1 = "A", TenThuoc2 = "B", MucDo = 1, MoTa = "nhẹ" },
                new CanhBaoTuongTac { TenThuoc1 = "C", TenThuoc2 = "D", MucDo = 3, MoTa = "nặng" }
            };
            Assert.AreEqual(3, TuongTacThuocBLL.MucDoCaoNhat(ds));
            Assert.AreEqual(0, TuongTacThuocBLL.MucDoCaoNhat(new List<CanhBaoTuongTac>()));
            string tb = TuongTacThuocBLL.TaoThongBao(ds);
            Assert.IsTrue(tb.IndexOf("Nghiêm trọng") < tb.IndexOf("Lưu ý"), "Mức cao hiện trước");
        }

        [TestMethod]
        public void KiemTra_DuoiHaiThuoc_KhongGoiCSDL()
        {
            // 0 hoặc 1 thuốc thì không thể có cặp -> trả rỗng ngay (không cần kết nối CSDL)
            Assert.AreEqual(0, new TuongTacThuocBLL().KiemTra(new[] { "TH001" }).Count);
            Assert.AreEqual(0, new TuongTacThuocBLL().KiemTra(new[] { "TH001", " TH001" }).Count);
        }
    }

    [TestClass]
    public class HieuNangTests
    {
        [TestMethod]
        public void TrungVi()
        {
            Assert.AreEqual(3.0, HieuNangDAO.TrungVi(new List<double> { 5, 1, 3 }));
            Assert.AreEqual(2.5, HieuNangDAO.TrungVi(new List<double> { 4, 1, 3, 2 }));
            Assert.AreEqual(0.0, HieuNangDAO.TrungVi(new List<double>()));
        }

        [TestMethod]
        public void GhepKetQua_TinhHeSo()
        {
            var truoc = new List<KetQuaChay> { new KetQuaChay { Ten = "Q1", SoDong = 55, Ms = 85 } };
            var sau = new List<KetQuaChay> { new KetQuaChay { Ten = "Q1", SoDong = 55, Ms = 1.7 } };
            List<KetQuaDoTruyVan> kq = HieuNangBLL.GhepKetQua(truoc, sau, new DateTime(2020, 6, 15), "TH001");
            Assert.AreEqual(1, kq.Count);
            Assert.AreEqual(50.0, kq[0].HeSo, 0.001);
            StringAssert.Contains(HieuNangBLL.TomTat(kq), "85 ms -> 1.7 ms".Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
        }
    }

    /// <summary>Lỗi vi phạm ràng buộc CHECK -> câu tiếng Việt (GUI/GiaoDien.DichLoiRangBuoc).</summary>
    [TestClass]
    public class DichLoiRangBuocTests
    {
        [TestMethod]
        public void DichDungTenRangBuoc()
        {
            Exception ex = new Exception("The UPDATE statement conflicted with the CHECK constraint \"CK_Thuoc_SLHienCo\". The conflict occurred in database \"QuanLyNhaThuoc\", table \"dbo.Thuoc\", column 'SLHienCo'.");
            string s = GiaoDien.DichLoiRangBuoc(ex);
            Assert.IsNotNull(s);
            StringAssert.Contains(s, "tồn kho không được âm");
        }

        [TestMethod]
        public void LoiBocBenTrong_VanDichDuoc()
        {
            Exception ex = new InvalidOperationException("Lưu thất bại",
                new Exception("... CHECK constraint \"CK_ChiTietHDB_GiamGia\" ..."));
            StringAssert.Contains(GiaoDien.DichLoiRangBuoc(ex), "0% đến 100%");
        }

        [TestMethod]
        public void KhongPhaiLoiCheck_TraNull()
        {
            Assert.IsNull(GiaoDien.DichLoiRangBuoc(new Exception("Timeout expired")));
            Assert.AreEqual("Timeout expired", GiaoDien.DichLoi(new Exception("Timeout expired")));
        }
    }
}
