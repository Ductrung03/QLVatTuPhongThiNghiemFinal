using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.Entities
{
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [StringLength(50)]
        public string MatKhau { get; set; } // Sẽ được thay thế bằng MatKhauHash

        [StringLength(255)]
        public string MatKhauHash { get; set; }

        [StringLength(255)]
        public string Salt { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        public string HoTen { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public bool TrangThaiTaiKhoan { get; set; } = true; // true: Hoạt động, false: Khóa

        public DateTime? LanDangNhapCuoi { get; set; }
        public int SoLanDangNhapSai { get; set; } = 0;
        public DateTime? NgayKhoaTaiKhoan { get; set; }

        [StringLength(255)]
        public string TokenDoiMatKhau { get; set; }
        public DateTime? NgayHetHanToken { get; set; }

        // Navigation properties
        public virtual ICollection<NguoiDungVaiTro> NguoiDungVaiTros { get; set; }
        public virtual ICollection<LichSuHoatDong> LichSuHoatDongs { get; set; }
        public virtual ICollection<ThongBao> ThongBaosDaGui { get; set; }
        public virtual ICollection<ThongBao> ThongBaosDaNhan { get; set; }
    }
}