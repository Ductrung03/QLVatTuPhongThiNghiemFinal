using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.ViewModels
{
    public class TrangTBViewModel
    {
        public int MaTTB { get; set; }

        [Required(ErrorMessage = "Phòng máy là bắt buộc")]
        [Display(Name = "Phòng máy")]
        public int MaPhongMay { get; set; }

        [Required(ErrorMessage = "Loại thiết bị là bắt buộc")]
        [Display(Name = "Loại thiết bị")]
        public int MaLoai { get; set; }

        [Display(Name = "Giá tiền")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá tiền phải lớn hơn 0")]
        public int? GiaTien { get; set; }

        [Display(Name = "Tình trạng")]
        [StringLength(100)]
        public string TinhTrang { get; set; }

        [Required(ErrorMessage = "Ngày nhập là bắt buộc")]
        [Display(Name = "Ngày nhập")]
        [DataType(DataType.Date)]
        public DateTime NgayNhap { get; set; }

        [Required(ErrorMessage = "Thương hiệu là bắt buộc")]
        [Display(Name = "Thương hiệu")]
        public int MaThuongHieu { get; set; }

        public int? SoLanSua { get; set; }

        // Display properties
        public string TenPhongMay { get; set; }
        public string TenLoai { get; set; }
        public string TenThuongHieu { get; set; }
    }
}