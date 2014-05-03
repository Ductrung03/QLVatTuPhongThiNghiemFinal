using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface IDanhGiaCapDoRepository
    {
        Task<int> DanhGiaCapDoAsync(DanhGiaCapDoViewModel model);
        Task<DanhGiaCapDoViewModel> GetCapDoThietBiAsync(int maTTB);
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetAllAsync();
    }
}