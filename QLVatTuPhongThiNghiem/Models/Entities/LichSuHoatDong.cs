using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class LichSuHoatDong
    {
        [Key]
        public int MaLichSu { get; set; }

        [Required]
        public int MaNguoiDung { get; set; }

        [Required]
        [StringLength(50)]
        public string HanhDong { get; set; }

        [Required]
        [StringLength(50)]
        public string Module { get; set; }

        public string ChiTiet { get; set; }

        public DateTime ThoiGian { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string DiaChi_IP { get; set; }

        [StringLength(500)]
        public string UserAgent { get; set; }

        // Navigation properties
        public virtual NguoiDung NguoiDung { get; set; }
    }
}
