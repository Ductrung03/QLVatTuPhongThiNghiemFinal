using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class VaiTro
    {
        [Key]
        public int MaVaiTro { get; set; }

        [Required]
        [StringLength(50)]
        public string TenVaiTro { get; set; }

        [StringLength(200)]
        public string MoTa { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool TrangThai { get; set; } = true;

        // Navigation properties
        public virtual ICollection<VaiTroQuyen> VaiTroQuyens { get; set; }
        public virtual ICollection<NguoiDungVaiTro> NguoiDungVaiTros { get; set; }
    }
}
