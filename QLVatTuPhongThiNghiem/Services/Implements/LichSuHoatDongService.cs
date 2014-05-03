using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class LichSuHoatDongService : ILichSuHoatDongService
    {
        private readonly ILichSuHoatDongRepository _lichSuHoatDongRepository;

        public LichSuHoatDongService(ILichSuHoatDongRepository lichSuHoatDongRepository)
        {
            _lichSuHoatDongRepository = lichSuHoatDongRepository;
        }

        public async Task GhiLichSuAsync(int maNguoiDung, string hanhDong, string module, string chiTiet, string diaChiIP = null, string userAgent = null)
        {
            await _lichSuHoatDongRepository.GhiLichSuAsync(maNguoiDung, hanhDong, module, chiTiet, diaChiIP, userAgent);
        }

        public async Task<IEnumerable<LichSuHoatDongViewModel>> GetLichSuAsync(int? maNguoiDung = null, DateTime? tuNgay = null, DateTime? denNgay = null, int pageNumber = 1, int pageSize = 50)
        {
            return await _lichSuHoatDongRepository.GetLichSuAsync(maNguoiDung, tuNgay, denNgay, pageNumber, pageSize);
        }

        public async Task<int> GetTongSoBanGhiAsync(int? maNguoiDung = null, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            return await _lichSuHoatDongRepository.GetTongSoBanGhiAsync(maNguoiDung, tuNgay, denNgay);
        }
    }
}