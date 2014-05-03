using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class DanhGiaCapDoService : IDanhGiaCapDoService
    {
        private readonly IDanhGiaCapDoRepository _danhGiaCapDoRepository;

        public DanhGiaCapDoService(IDanhGiaCapDoRepository danhGiaCapDoRepository)
        {
            _danhGiaCapDoRepository = danhGiaCapDoRepository;
        }

        public async Task<(bool Success, string Message)> DanhGiaCapDoAsync(DanhGiaCapDoViewModel model)
        {
            try
            {
                var ketQua = await _danhGiaCapDoRepository.DanhGiaCapDoAsync(model);

                switch (ketQua)
                {
                    case 1:
                        return (true, "Đánh giá cấp độ thiết bị thành công");
                    case 0:
                        return (false, "Cấp độ không hợp lệ (1-5)");
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

        public async Task<DanhGiaCapDoViewModel> GetCapDoThietBiAsync(int maTTB)
        {
            return await _danhGiaCapDoRepository.GetCapDoThietBiAsync(maTTB);
        }

        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetAllAsync()
        {
            return await _danhGiaCapDoRepository.GetAllAsync();
        }
    }
}

