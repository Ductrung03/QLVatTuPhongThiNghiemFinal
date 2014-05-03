using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface ILichTrucRepository
    {
        Task<(int KetQua, string ThongBao)> TaoLichTrucAsync(LichTrucViewModel model);
        Task<(int KetQua, string ThongBao)> CapNhatLichTrucAsync(LichTrucViewModel model);
        Task<(int KetQua, string ThongBao)> XoaLichTrucAsync(int maLich);
        Task<IEnumerable<LichTrucViewModel>> GetAllAsync();
        Task<IEnumerable<LichTrucViewModel>> GetByPhongMayAsync(int maPhongMay);
        Task<IEnumerable<LichTrucViewModel>> GetByNgayAsync(DateTime ngay);
        Task<LichTrucViewModel> GetByIdAsync(int maLich);
    }
}
