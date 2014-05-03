using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface ISuaChuaService
    {
        Task<(bool Success, int MaSuaChua, string Message)> TaoPhieuSuaChuaAsync(SuaChuaViewModel model);
        Task<(bool Success, string Message)> HoanThanhSuaChuaAsync(int maSuaChua, decimal chiPhi, string tinhTrangMoi);
        Task<IEnumerable<SuaChuaViewModel>> GetAllAsync();
        Task<IEnumerable<SuaChuaViewModel>> GetByTrangThaiAsync(string trangThai);
    }
}
