using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class NhanVien
    {
        [Key]
        public int MaNV { get; set; }

        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(20)]
        public string SoDienThoai { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string DiaChi { get; set; }

        [StringLength(50)]
        public string ChucVu { get; set; }

        public DateTime? NgayVaoLam { get; set; }

        public bool TrangThai { get; set; } = true; // true: Đang làm việc, false: Đã nghỉ

        // Navigation properties
        public virtual ICollection<LichTruc> LichTrucs { get; set; }
    }
}