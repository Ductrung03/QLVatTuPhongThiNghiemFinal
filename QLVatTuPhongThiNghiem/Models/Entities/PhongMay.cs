using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class PhongMay
    {
        [Key]
        public int MaPhongMay { get; set; }

        [StringLength(50)]
        public string TenPhongMay { get; set; }
    }
}