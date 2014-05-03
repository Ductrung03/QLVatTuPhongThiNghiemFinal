namespace QLVatTuPhongThiNghiem.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TongThietBi { get; set; }
        public int ThietBiTot { get; set; }
        public int ThietBiHong { get; set; }
        public int ThietBiDangSua { get; set; }
        public int LichThucHanhHomNay { get; set; }
        public double TongGiaTriThietBi { get; set; }

        public List<ThongKeTheoPhongViewModel> ThongKeTheoPhong { get; set; } = new List<ThongKeTheoPhongViewModel>();
        public List<ThongKeSuDungTheoThangViewModel> ThongKeSuDungTheoThang { get; set; } = new List<ThongKeSuDungTheoThangViewModel>();
    }

    public class ThongKeTheoPhongViewModel
    {
        public int MaPhongMay { get; set; }
        public string TenPhongMay { get; set; }
        public int TongThietBi { get; set; }
        public int ThietBiTot { get; set; }
        public int ThietBiHong { get; set; }
        public int ThietBiDangSua { get; set; }
        public double GiaTriTrungBinh { get; set; }
    }

    public class ThongKeSuDungTheoThangViewModel
    {
        public int Thang { get; set; }
        public int SoLichThucHanh { get; set; }
        public int TongGioSuDung { get; set; }
    }
}