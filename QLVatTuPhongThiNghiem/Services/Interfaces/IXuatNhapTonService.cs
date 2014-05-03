using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface IXuatNhapTonService
    {
        Task<(bool Success, string Message)> XuatThietBiAsync(XuatNhapTonViewModel model);
        Task<(bool Success, string Message)> NhapThietBiAsync(XuatNhapTonViewModel model);
        Task<IEnumerable<dynamic>> BaoCaoTonKhoAsync(int? maPhongMay = null);
        Task<IEnumerable<XuatNhapTonViewModel>> GetAllAsync();
    }
}