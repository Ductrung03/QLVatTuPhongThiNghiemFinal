using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class VaiTroQuyen
    {
        [Key]
        public int MaVaiTro { get; set; }

        [Key]
        public int MaQuyen { get; set; }

        public DateTime NgayCapQuyen { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual VaiTro VaiTro { get; set; }
        public virtual QuyenHan QuyenHan { get; set; }
    }
}
