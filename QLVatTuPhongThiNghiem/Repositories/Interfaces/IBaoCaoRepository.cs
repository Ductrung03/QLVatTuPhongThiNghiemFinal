using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface IBaoCaoRepository
    {
        Task<IEnumerable<ThongKeTheoPhongViewModel>> ThongKeThietBiTheoPhongAsync();
        Task<IEnumerable<ThongKeSuDungTheoThangViewModel>> ThongKeSuDungTheoThangAsync(int nam);
        Task<IEnumerable<dynamic>> BaoCaoChiPhiSuaChuaAsync(DateTime tuNgay, DateTime denNgay);
        Task<IEnumerable<dynamic>> ThongKeDanhGiaCapDoAsync();
    }
}