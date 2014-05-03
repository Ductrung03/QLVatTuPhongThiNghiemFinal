using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface ILichThucHanhService
    {
        Task<(bool Success, int MaLich, string Message)> DangKyLichAsync(LichThucHanhViewModel model);
        Task<(bool Success, string Message)> CapNhatTrangThaiAsync(int maLich, string trangThai);
        Task<IEnumerable<LichThucHanhViewModel>> GetAllAsync();
        Task<IEnumerable<LichThucHanhViewModel>> GetByUserAsync(int maNguoiDung);
    }
}
