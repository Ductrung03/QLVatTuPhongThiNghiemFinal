using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.ViewModels
{
    public class LichThucHanhViewModel
    {
        public int MaLich { get; set; }

        [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc")]
        [Display(Name = "Thời gian bắt đầu")]
        public DateTime ThoiGianBD { get; set; }

        [Required(ErrorMessage = "Thời gian kết thúc là bắt buộc")]
        [Display(Name = "Thời gian kết thúc")]
        public DateTime ThoiGianKT { get; set; }

        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; }

        public int? MaNguoiDung { get; set; }
        public string TenNguoiDung { get; set; }
    }
}
