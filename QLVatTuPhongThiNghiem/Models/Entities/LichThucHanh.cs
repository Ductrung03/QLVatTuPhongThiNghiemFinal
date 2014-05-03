using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class LichThucHanh
    {
        [Key]
        public int MaLich { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }

        [Required]
        public DateTime ThoiGianBD { get; set; }

        [Required]
        public DateTime ThoiGianKT { get; set; }
        public int? MaNguoiDung { get; set; }

        // Navigation properties
        public virtual NguoiDung NguoiDung { get; set; }
    }
}