using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class ThuongHieu
    {
        [Key]
        public int MaThuongHieu { get; set; }

        [Required]
        [StringLength(50)]
        public string TenThuongHieu { get; set; }
    }
}