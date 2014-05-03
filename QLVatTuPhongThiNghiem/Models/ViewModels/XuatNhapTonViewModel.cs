using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.ViewModels
{
    public class XuatNhapTonViewModel
    {
        public int MaPhieu { get; set; }

        [Required(ErrorMessage = "Loại phiếu là bắt buộc")]
        [Display(Name = "Loại phiếu")]
        public string LoaiPhieu { get; set; }

        [Required(ErrorMessage = "Thiết bị là bắt buộc")]
        [Display(Name = "Thiết bị")]
        public int MaTTB { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Display(Name = "Số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string GhiChu { get; set; }

        public DateTime NgayTao { get; set; }
        public int NguoiTao { get; set; }

        // Display properties
        public string TenThietBi { get; set; }
        public string TenNguoiTao { get; set; }
    }
}
