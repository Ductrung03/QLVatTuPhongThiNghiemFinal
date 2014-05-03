using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface IThongBaoService
    {
        Task<(bool Success, string Message)> GuiThongBaoAsync(ThongBaoViewModel model);
        Task<IEnumerable<ThongBaoViewModel>> GetThongBaoByNguoiDungAsync(int maNguoiDung);
        Task<IEnumerable<ThongBaoViewModel>> GetAllThongBaoAsync();
        Task<(bool Success, string Message)> DanhDauDaDocAsync(int maThongBao, int maNguoiDung);
        Task<int> GetSoThongBaoChuaDocAsync(int maNguoiDung);
    }
}