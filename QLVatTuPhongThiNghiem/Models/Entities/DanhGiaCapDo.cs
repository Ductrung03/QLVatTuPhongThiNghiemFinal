using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class DanhGiaCapDo
    {
        [Key]
        public int MaDanhGia { get; set; }

        [Required]
        public int MaTTB { get; set; }

        [Required]
        [Range(1, 5)]
        public int CapDo { get; set; }

        [Required]
        public DateTime NgayDanhGia { get; set; } = DateTime.Now;

        [Required]
        public int NguoiDanhGia { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        // Navigation properties
        public virtual TrangTB TrangTB { get; set; }
        public virtual NguoiDung NguoiDanhGia_User { get; set; }
    }
}