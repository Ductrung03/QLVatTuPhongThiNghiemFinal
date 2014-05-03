using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface ISuaChuaRepository
    {
        Task<(int KetQua, int MaSuaChua)> TaoPhieuSuaChuaAsync(SuaChuaViewModel model);
        Task<int> HoanThanhSuaChuaAsync(int maSuaChua, decimal chiPhi, string tinhTrangMoi);
        Task<IEnumerable<SuaChuaViewModel>> GetAllAsync();
        Task<IEnumerable<SuaChuaViewModel>> GetByTrangThaiAsync(string trangThai);
    }
}