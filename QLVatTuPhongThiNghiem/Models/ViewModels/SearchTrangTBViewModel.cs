namespace QLVatTuPhongThiNghiem.Models.ViewModels
{
    public class SearchTrangTBViewModel
    {
        public int? MaPhongMay { get; set; }
        public int? MaLoai { get; set; }
        public string TinhTrang { get; set; }
        public int? MaThuongHieu { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalRecords { get; set; }
        public List<TrangTBViewModel> Results { get; set; } = new List<TrangTBViewModel>();
    }
}