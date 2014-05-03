using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface IXuatNhapTonRepository
    {
        Task<int> XuatThietBiAsync(XuatNhapTonViewModel model);
        Task<int> NhapThietBiAsync(XuatNhapTonViewModel model);
        Task<IEnumerable<dynamic>> BaoCaoTonKhoAsync(int? maPhongMay = null);
        Task<IEnumerable<XuatNhapTonViewModel>> GetAllAsync();
    }
}