using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class LichTrucService : ILichTrucService
    {
        private readonly ILichTrucRepository _lichTrucRepository;

        public LichTrucService(ILichTrucRepository lichTrucRepository)
        {
            _lichTrucRepository = lichTrucRepository;
        }

        public async Task<(bool Success, string Message)> TaoLichTrucAsync(LichTrucViewModel model)
        {
            try
            {
                // Validate business rules
                if (model.Ngay.Date <= DateTime.Today)
                {
                    return (false, "Không thể tạo lịch trực cho ngày trong quá khứ hoặc hôm nay");
                }

                if (string.IsNullOrEmpty(model.CaLam))
                {
                    return (false, "Vui lòng chọn ca làm việc");
                }

                var (ketQua, thongBao) = await _lichTrucRepository.TaoLichTrucAsync(model);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> CapNhatLichTrucAsync(LichTrucViewModel model)
        {
            try
            {
                var (ketQua, thongBao) = await _lichTrucRepository.CapNhatLichTrucAsync(model);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> XoaLichTrucAsync(int maLich)
        {
            try
            {
                var (ketQua, thongBao) = await _lichTrucRepository.XoaLichTrucAsync(maLich);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<IEnumerable<LichTrucViewModel>> GetAllAsync()
        {
            return await _lichTrucRepository.GetAllAsync();
        }

        public async Task<IEnumerable<LichTrucViewModel>> GetByPhongMayAsync(int maPhongMay)
        {
            return await _lichTrucRepository.GetByPhongMayAsync(maPhongMay);
        }

        public async Task<IEnumerable<LichTrucViewModel>> GetByNgayAsync(DateTime ngay)
        {
            return await _lichTrucRepository.GetByNgayAsync(ngay);
        }

        public async Task<LichTrucViewModel> GetByIdAsync(int maLich)
        {
            return await _lichTrucRepository.GetByIdAsync(maLich);
        }
    }
}
