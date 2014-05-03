using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface IVaiTroService
    {
        Task<IEnumerable<VaiTroViewModel>> GetAllAsync();
        Task<VaiTroViewModel> GetByIdAsync(int maVaiTro);
        Task<(bool Success, string Message)> CreateAsync(VaiTroViewModel model);
        Task<(bool Success, string Message)> UpdateAsync(VaiTroViewModel model);
        Task<(bool Success, string Message)> DeleteAsync(int maVaiTro);
        Task<IEnumerable<QuyenHanViewModel>> GetAllQuyenHanAsync();
        Task<IEnumerable<QuyenHanViewModel>> GetQuyenHanByVaiTroAsync(int maVaiTro);
        Task<(bool Success, string Message)> CapNhatQuyenHanAsync(int maVaiTro, List<int> danhSachQuyen);
    }
}
