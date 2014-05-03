using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface IBaoCaoService
    {
        Task<IEnumerable<ThongKeTheoPhongViewModel>> ThongKeThietBiTheoPhongAsync();
        Task<IEnumerable<ThongKeSuDungTheoThangViewModel>> ThongKeSuDungTheoThangAsync(int nam);
        Task<IEnumerable<dynamic>> BaoCaoChiPhiSuaChuaAsync(DateTime tuNgay, DateTime denNgay);
        Task<IEnumerable<dynamic>> ThongKeDanhGiaCapDoAsync();
    }
}