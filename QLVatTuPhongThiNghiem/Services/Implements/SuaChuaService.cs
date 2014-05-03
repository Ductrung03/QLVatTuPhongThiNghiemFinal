using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class SuaChuaService : ISuaChuaService
    {
        private readonly ISuaChuaRepository _suaChuaRepository;

        public SuaChuaService(ISuaChuaRepository suaChuaRepository)
        {
            _suaChuaRepository = suaChuaRepository;
        }

        public async Task<(bool Success, int MaSuaChua, string Message)> TaoPhieuSuaChuaAsync(SuaChuaViewModel model)
        {
            try
            {
                var (ketQua, maSuaChua) = await _suaChuaRepository.TaoPhieuSuaChuaAsync(model);

                switch (ketQua)
                {
                    case 1:
                        return (true, maSuaChua, "Tạo phiếu sửa chữa thành công");
                    case 0:
                        return (false, 0, "Thiết bị đang được sửa chữa hoặc dữ liệu không hợp lệ");
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

        public async Task<(bool Success, string Message)> HoanThanhSuaChuaAsync(int maSuaChua, decimal chiPhi, string tinhTrangMoi)
        {
            try
            {
                var ketQua = await _suaChuaRepository.HoanThanhSuaChuaAsync(maSuaChua, chiPhi, tinhTrangMoi);

                switch (ketQua)
                {
                    case 1:
                        return (true, "Hoàn thành sửa chữa thành công");
                    case 0:
                        return (false, "Không tìm thấy phiếu sửa chữa hoặc đã hoàn thành");
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

        public async Task<IEnumerable<SuaChuaViewModel>> GetAllAsync()
        {
            return await _suaChuaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<SuaChuaViewModel>> GetByTrangThaiAsync(string trangThai)
        {
            return await _suaChuaRepository.GetByTrangThaiAsync(trangThai);
        }
    }
}
