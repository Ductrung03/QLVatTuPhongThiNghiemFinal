using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class LichSuSuaChua
    {
        [Key]
        public int MaSuaChua { get; set; }

        [Required]
        public int MaTTB { get; set; }

        [Required]
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }

        [Required]
        [StringLength(50)]
        public string LoaiSuaChua { get; set; }

        [StringLength(500)]
        public string MoTa { get; set; }
        public decimal? ChiPhi { get; set; }

        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; } = "Đang sửa chữa";

        [Required]
        public int NguoiThucHien { get; set; }

        // Navigation properties
        public virtual TrangTB TrangTB { get; set; }
        public virtual NguoiDung NguoiThucHien_User { get; set; }
    }
}
