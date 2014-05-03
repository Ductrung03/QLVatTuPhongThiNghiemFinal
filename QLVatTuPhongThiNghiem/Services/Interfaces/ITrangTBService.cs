using QLVatTuPhongThiNghiem.Models.Entities;
using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface ITrangTBService
    {
        Task<(bool Success, int MaTTB, string Message)> CreateAsync(TrangTBViewModel model, int nguoiTao);
        Task<(bool Success, string Message)> UpdateAsync(TrangTBViewModel model, int nguoiCapNhat);
        Task<(bool Success, string Message)> DeleteAsync(int maTTB);
        Task<SearchTrangTBViewModel> SearchAsync(SearchTrangTBViewModel searchModel);
        Task<TrangTBViewModel> GetByIdAsync(int maTTB);
        Task<IEnumerable<TrangTB>> GetAllAsync();
    }
}