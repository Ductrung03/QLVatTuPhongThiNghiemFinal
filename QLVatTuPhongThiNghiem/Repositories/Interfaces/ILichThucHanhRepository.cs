using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface ILichThucHanhRepository
    {
        Task<(int KetQua, int MaLich)> DangKyLichAsync(LichThucHanhViewModel model);
        Task<int> CapNhatTrangThaiAsync(int maLich, string trangThai);
        Task<IEnumerable<LichThucHanhViewModel>> GetAllAsync();
        Task<IEnumerable<LichThucHanhViewModel>> GetByUserAsync(int maNguoiDung);
    }
}