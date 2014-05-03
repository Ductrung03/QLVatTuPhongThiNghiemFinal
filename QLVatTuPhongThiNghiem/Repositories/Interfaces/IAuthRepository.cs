using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<(int KetQua, int MaNguoiDung)> DangNhapAsync(string tenDangNhap, string matKhau);
        Task DangXuatAsync(int maNguoiDung);
    }
}