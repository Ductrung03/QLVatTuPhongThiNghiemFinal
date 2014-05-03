using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class XuatNhapTon
    {
        [Key]
        public int MaPhieu { get; set; }

        [Required]
        [StringLength(10)]
        public string LoaiPhieu { get; set; } // NHAP, XUAT

        [Required]
        public int MaTTB { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [Required]
        public int NguoiTao { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        // Navigation properties
        public virtual TrangTB TrangTB { get; set; }
        public virtual NguoiDung NguoiTao_User { get; set; }
    }
}
