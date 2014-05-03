using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface ILichTrucService
    {
        Task<(bool Success, string Message)> TaoLichTrucAsync(LichTrucViewModel model);
        Task<(bool Success, string Message)> CapNhatLichTrucAsync(LichTrucViewModel model);
        Task<(bool Success, string Message)> XoaLichTrucAsync(int maLich);
        Task<IEnumerable<LichTrucViewModel>> GetAllAsync();
        Task<IEnumerable<LichTrucViewModel>> GetByPhongMayAsync(int maPhongMay);
        Task<IEnumerable<LichTrucViewModel>> GetByNgayAsync(DateTime ngay);
        Task<LichTrucViewModel> GetByIdAsync(int maLich);
    }
}
