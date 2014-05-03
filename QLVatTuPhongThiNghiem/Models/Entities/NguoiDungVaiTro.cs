using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class NguoiDungVaiTro
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [Key]
        public int MaVaiTro { get; set; }

        public DateTime NgayCapQuyen { get; set; } = DateTime.Now;
        public DateTime? NgayHetHan { get; set; }
        public bool TrangThai { get; set; } = true;

        // Navigation properties
        public virtual NguoiDung NguoiDung { get; set; }
        public virtual VaiTro VaiTro { get; set; }
    }
}
