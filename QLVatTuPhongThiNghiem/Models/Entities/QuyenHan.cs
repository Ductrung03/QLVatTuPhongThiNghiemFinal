using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class QuyenHan
    {
        [Key]
        public int MaQuyen { get; set; }

        [Required]
        [StringLength(50)]
        public string TenQuyen { get; set; }

        [StringLength(200)]
        public string MoTa { get; set; }

        [Required]
        [StringLength(50)]
        public string Module { get; set; }

        [Required]
        [StringLength(50)]
        public string HanhDong { get; set; }

        // Navigation properties
        public virtual ICollection<VaiTroQuyen> VaiTroQuyens { get; set; }
    }
}
