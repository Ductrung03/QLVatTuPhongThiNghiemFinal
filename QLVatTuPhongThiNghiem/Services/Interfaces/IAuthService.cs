using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, int MaNguoiDung, string Message)> LoginAsync(LoginViewModel model);
        Task LogoutAsync(int maNguoiDung);
    }
}