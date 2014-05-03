using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface IThongBaoRepository
    {
        Task<(int KetQua, string ThongBao)> GuiThongBaoAsync(ThongBaoViewModel model);
        Task<IEnumerable<ThongBaoViewModel>> GetThongBaoByNguoiDungAsync(int maNguoiDung);
        Task<IEnumerable<ThongBaoViewModel>> GetAllThongBaoAsync();
        Task<(int KetQua, string ThongBao)> DanhDauDaDocAsync(int maThongBao, int maNguoiDung);
        Task<int> GetSoThongBaoChuaDocAsync(int maNguoiDung);
    }
}
