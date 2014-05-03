using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.ViewModels
{
    public class DanhGiaCapDoViewModel
    {
        public int MaDanhGia { get; set; }

        [Required(ErrorMessage = "Thiết bị là bắt buộc")]
        [Display(Name = "Thiết bị")]
        public int MaTTB { get; set; }

        [Required(ErrorMessage = "Cấp độ là bắt buộc")]
        [Display(Name = "Cấp độ")]
        [Range(1, 5, ErrorMessage = "Cấp độ phải từ 1 đến 5")]
        public int CapDo { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string GhiChu { get; set; }

        public DateTime NgayDanhGia { get; set; }
        public int NguoiDanhGia { get; set; }

        // Display properties
        public string TenThietBi { get; set; }
        public string TenNguoiDanhGia { get; set; }
    }
}