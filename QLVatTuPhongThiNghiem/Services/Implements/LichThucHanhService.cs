using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class LichThucHanhService : ILichThucHanhService
    {
        private readonly ILichThucHanhRepository _lichThucHanhRepository;

        public LichThucHanhService(ILichThucHanhRepository lichThucHanhRepository)
        {
            _lichThucHanhRepository = lichThucHanhRepository;
        }

        public async Task<(bool Success, int MaLich, string Message)> DangKyLichAsync(LichThucHanhViewModel model)
        {
            try
            {
                var (ketQua, maLich) = await _lichThucHanhRepository.DangKyLichAsync(model);

                switch (ketQua)
                {
                    case 1:
                        return (true, maLich, "Đăng ký lịch thực hành thành công");
                    case 0:
                        return (false, 0, "Thời gian không hợp lệ hoặc trùng lịch");
                    case -1:
                        return (false, 0, "Lỗi hệ thống");
                    default:
                        return (false, 0, "Lỗi không xác định");
                }
            }
            catch (Exception ex)
            {
                return (false, 0, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> CapNhatTrangThaiAsync(int maLich, string trangThai)
        {
            try
            {
                var ketQua = await _lichThucHanhRepository.CapNhatTrangThaiAsync(maLich, trangThai);

                switch (ketQua)
                {
                    case 1:
                        return (true, "Cập nhật trạng thái thành công");
                    case 0:
                        return (false, "Không tìm thấy lịch thực hành");
                    case -1:
                        return (false, "Lỗi hệ thống");
                    default:
                        return (false, "Lỗi không xác định");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<LichThucHanhViewModel>> GetAllAsync()
        {
            return await _lichThucHanhRepository.GetAllAsync();
        }

        public async Task<IEnumerable<LichThucHanhViewModel>> GetByUserAsync(int maNguoiDung)
        {
            return await _lichThucHanhRepository.GetByUserAsync(maNguoiDung);
        }
    }
}

