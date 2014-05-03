using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class TrangTB
    {
        [Key]
        public int MaTTB { get; set; }
        public int? MaPhongMay { get; set; }
        public int? MaLoai { get; set; }
        public int? GiaTien { get; set; }

        [StringLength(100)]
        public string TinhTrang { get; set; }

        [Required]
        public DateTime NgayNhap { get; set; }
        public int? SoLanSua { get; set; }
        public int? MaThuongHieu { get; set; }
        public int? MaLich { get; set; }

        // Navigation properties
        public virtual PhongMay PhongMay { get; set; }
        public virtual Loai Loai { get; set; }
        public virtual ThuongHieu ThuongHieu { get; set; }
    }
}