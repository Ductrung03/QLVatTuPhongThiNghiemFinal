using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class BaoCaoService : IBaoCaoService
    {
        private readonly IBaoCaoRepository _baoCaoRepository;

        public BaoCaoService(IBaoCaoRepository baoCaoRepository)
        {
            _baoCaoRepository = baoCaoRepository;
        }

        public async Task<IEnumerable<ThongKeTheoPhongViewModel>> ThongKeThietBiTheoPhongAsync()
        {
            return await _baoCaoRepository.ThongKeThietBiTheoPhongAsync();
        }

        public async Task<IEnumerable<ThongKeSuDungTheoThangViewModel>> ThongKeSuDungTheoThangAsync(int nam)
        {
            return await _baoCaoRepository.ThongKeSuDungTheoThangAsync(nam);
        }

        public async Task<IEnumerable<dynamic>> BaoCaoChiPhiSuaChuaAsync(DateTime tuNgay, DateTime denNgay)
        {
            return await _baoCaoRepository.BaoCaoChiPhiSuaChuaAsync(tuNgay, denNgay);
        }

        public async Task<IEnumerable<dynamic>> ThongKeDanhGiaCapDoAsync()
        {
            return await _baoCaoRepository.ThongKeDanhGiaCapDoAsync();
        }
    }
}
