namespace QLVatTuPhongThiNghiem.Models.ViewModels
{
    public class ChiPhiSuaChuaViewModel
    {
        public string TenLoai { get; set; }
        public string TenThuongHieu { get; set; }
        public int SoLanSuaChua { get; set; }
        public decimal TongChiPhi { get; set; }
        public decimal ChiPhiTrungBinh { get; set; }
    }

    public class ThongKeDanhGiaViewModel
    {
        public int CapDo { get; set; }
        public int SoLuongDanhGia { get; set; }
        public decimal TyLe { get; set; }
    }

    public class TonKhoViewModel
    {
        public int MaTTB { get; set; }
        public int MaPhongMay { get; set; }
        public string TenPhongMay { get; set; }
        public string TenLoai { get; set; }
        public string TenThuongHieu { get; set; }
        public int TongNhap { get; set; }
        public int TongXuat { get; set; }
        public int TonKho { get; set; }
        public string TinhTrang { get; set; }
    }
}