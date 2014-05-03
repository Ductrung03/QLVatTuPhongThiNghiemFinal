using QLVatTuPhongThiNghiem.Models.Entities;
using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface ITrangTBRepository : IBaseRepository<TrangTB>
    {
        Task<(int KetQua, int MaTTB)> InsertAsync(TrangTBViewModel model, int nguoiCapNhat);
        Task<int> UpdateAsync(TrangTBViewModel model, int nguoiCapNhat);
        Task<int> DeleteAsync(int maTTB);
        Task<SearchTrangTBViewModel> SearchAsync(SearchTrangTBViewModel searchModel);
    }
}