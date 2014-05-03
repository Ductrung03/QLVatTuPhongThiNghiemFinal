using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class ThongBao
    {
        [Key]
        public int MaThongBao { get; set; }

        [Required]
        [StringLength(200)]
        public string TieuDe { get; set; }

        [Required]
        public string NoiDung { get; set; }

        [Required]
        [StringLength(50)]
        public string LoaiThongBao { get; set; } // Thong_Tin, Canh_Bao, Loi

        public int? MaNguoiGui { get; set; }
        public int? MaNguoiNhan { get; set; } // NULL = gửi cho tất cả

        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool DaDoc { get; set; } = false;
        public bool TrangThai { get; set; } = true;

        // Navigation properties
        public virtual NguoiDung NguoiGui { get; set; }
        public virtual NguoiDung NguoiNhan { get; set; }
    }
}