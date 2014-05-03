using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface IDanhGiaCapDoService
    {
        Task<(bool Success, string Message)> DanhGiaCapDoAsync(DanhGiaCapDoViewModel model);
        Task<DanhGiaCapDoViewModel> GetCapDoThietBiAsync(int maTTB);
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetAllAsync();
    }
}