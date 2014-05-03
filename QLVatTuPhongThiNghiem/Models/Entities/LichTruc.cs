using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class LichTruc
    {
        [Key]
        public int MaLich { get; set; }

        [Required]
        public DateTime Ngay { get; set; }

        [Required]
        public int MaNV { get; set; }

        [Required]
        public int MaPhongMay { get; set; }

        [StringLength(50)]
        public string CaLam { get; set; } // Ca sáng, Ca chiều, Ca tối

        [StringLength(500)]
        public string GhiChu { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; } = "Đã lên lịch"; // Đã lên lịch, Đang trực, Hoàn thành, Đã hủy

        // Navigation properties
        public virtual NhanVien NhanVien { get; set; }
        public virtual PhongMay PhongMay { get; set; }
    }
}
