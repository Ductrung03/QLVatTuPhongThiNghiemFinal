using System.ComponentModel.DataAnnotations;

namespace QLVatTuPhongThiNghiem.Models.ViewModels
{
    public class NguoiDungViewModel
    {
        public int MaNguoiDung { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được quá 50 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự")]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được quá 100 ký tự")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Trạng thái tài khoản")]
        public bool TrangThaiTaiKhoan { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime NgayTao { get; set; }

        [Display(Name = "Lần đăng nhập cuối")]
        public DateTime? LanDangNhapCuoi { get; set; }

        public List<int> DanhSachVaiTro { get; set; } = new List<int>();
        public string TenVaiTro { get; set; }
    }

    public class TaoTaiKhoanViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được quá 50 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 50 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string XacNhanMatKhau { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự")]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được quá 100 ký tự")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        [Display(Name = "Vai trò")]
        public int MaVaiTro { get; set; }
    }

    public class DoiMatKhauViewModel
    {
        [Required(ErrorMessage = "Mật khẩu cũ là bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string MatKhauCu { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 50 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string MatKhauMoi { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu mới là bắt buộc")]
        [DataType(DataType.Password)]
        [Compare("MatKhauMoi", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        [Display(Name = "Xác nhận mật khẩu mới")]
        public string XacNhanMatKhauMoi { get; set; }
    }

    public class VaiTroViewModel
    {
        public int MaVaiTro { get; set; }

        [Required(ErrorMessage = "Tên vai trò là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên vai trò không được quá 50 ký tự")]
        [Display(Name = "Tên vai trò")]
        public string TenVaiTro { get; set; }

        [StringLength(200, ErrorMessage = "Mô tả không được quá 200 ký tự")]
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; } = true;

        public List<int> DanhSachQuyen { get; set; } = new List<int>();
    }

    public class QuyenHanViewModel
    {
        public int MaQuyen { get; set; }
        public string TenQuyen { get; set; }
        public string MoTa { get; set; }
        public string Module { get; set; }
        public string HanhDong { get; set; }
        public bool DuocChon { get; set; } = false;
    }

    public class LichSuHoatDongViewModel
    {
        public int MaLichSu { get; set; }
        public string TenNguoiDung { get; set; }
        public string HanhDong { get; set; }
        public string Module { get; set; }
        public string ChiTiet { get; set; }
        public DateTime ThoiGian { get; set; }
        public string DiaChi_IP { get; set; }
    }

    public class ThongBaoViewModel
    {
        public int MaThongBao { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được quá 200 ký tự")]
        [Display(Name = "Tiêu đề")]
        public string TieuDe { get; set; }

        [Required(ErrorMessage = "Nội dung là bắt buộc")]
        [Display(Name = "Nội dung")]
        public string NoiDung { get; set; }

        [Required(ErrorMessage = "Loại thông báo là bắt buộc")]
        [Display(Name = "Loại thông báo")]
        public string LoaiThongBao { get; set; }

        [Display(Name = "Người nhận")]
        public int? MaNguoiNhan { get; set; } // NULL = gửi cho tất cả

        public DateTime NgayTao { get; set; }
        public bool DaDoc { get; set; }
        public string TenNguoiGui { get; set; }
        public string TenNguoiNhan { get; set; }
    }

    public class LichTrucViewModel
    {
        public int MaLich { get; set; }

        [Required(ErrorMessage = "Ngày trực là bắt buộc")]
        [Display(Name = "Ngày trực")]
        [DataType(DataType.Date)]
        public DateTime Ngay { get; set; }

        [Required(ErrorMessage = "Nhân viên trực là bắt buộc")]
        [Display(Name = "Nhân viên trực")]
        public int MaNV { get; set; }

        [Required(ErrorMessage = "Phòng máy là bắt buộc")]
        [Display(Name = "Phòng máy")]
        public int MaPhongMay { get; set; }

        [Display(Name = "Ca làm")]
        public string CaLam { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string GhiChu { get; set; }

        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Đã lên lịch";

        // Display properties
        public string TenNhanVien { get; set; }
        public string TenPhongMay { get; set; }
    }
}