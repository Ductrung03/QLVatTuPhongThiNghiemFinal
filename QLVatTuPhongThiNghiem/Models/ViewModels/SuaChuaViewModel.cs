using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.ViewModels
{
    public class SuaChuaViewModel
    {
        public int MaSuaChua { get; set; }

        [Required(ErrorMessage = "Thiết bị là bắt buộc")]
        [Display(Name = "Thiết bị")]
        public int MaTTB { get; set; }

        [Required(ErrorMessage = "Loại sửa chữa là bắt buộc")]
        [Display(Name = "Loại sửa chữa")]
        public string LoaiSuaChua { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [Display(Name = "Mô tả")]
        [StringLength(500)]
        public string MoTa { get; set; }

        [Display(Name = "Chi phí")]
        [Range(0, double.MaxValue, ErrorMessage = "Chi phí phải lớn hơn hoặc bằng 0")]
        public decimal? ChiPhi { get; set; }

        [Display(Name = "Tình trạng mới")]
        [StringLength(100)]
        public string TinhTrangMoi { get; set; }

        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string TrangThai { get; set; }
        public int NguoiThucHien { get; set; }

        // Display properties
        public string TenThietBi { get; set; }
        public string TenNguoiThucHien { get; set; }
    }
}
