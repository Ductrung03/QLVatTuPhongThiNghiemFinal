using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly INguoiDungService _nguoiDungService;

        public AuthService(IAuthRepository authRepository, INguoiDungService nguoiDungService)
        {
            _authRepository = authRepository;
            _nguoiDungService = nguoiDungService;
        }

        public async Task<(bool Success, int MaNguoiDung, string Message)> LoginAsync(LoginViewModel model)
        {
            try
            {
                // Sử dụng phương thức đăng nhập cũ cho compatibility
                var (ketQua, maNguoiDung) = await _authRepository.DangNhapAsync(model.TenDangNhap, model.MatKhau);

                switch (ketQua)
                {
                    case 1:
                        return (true, maNguoiDung, "Đăng nhập thành công");
                    case 0:
                        return (false, 0, "Sai tên đăng nhập hoặc mật khẩu");
                    case -1:
                        return (false, 0, "Lỗi hệ thống, vui lòng thử lại");
                    default:
                        return (false, 0, "Lỗi không xác định");
                }
            }
            catch (Exception ex)
            {
                return (false, 0, $"Lỗi: {ex.Message}");
            }
        }

        public async Task LogoutAsync(int maNguoiDung)
        {
            await _authRepository.DangXuatAsync(maNguoiDung);
        }
    }
}