using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class XuatNhapTonService : IXuatNhapTonService
    {
        private readonly IXuatNhapTonRepository _xuatNhapTonRepository;

        public XuatNhapTonService(IXuatNhapTonRepository xuatNhapTonRepository)
        {
            _xuatNhapTonRepository = xuatNhapTonRepository;
        }

        public async Task<(bool Success, string Message)> XuatThietBiAsync(XuatNhapTonViewModel model)
        {
            try
            {
                var ketQua = await _xuatNhapTonRepository.XuatThietBiAsync(model);

                switch (ketQua)
                {
                    case 1:
                        return (true, "Xuất thiết bị thành công");
                    case 0:
                        return (false, "Số lượng không hợp lệ hoặc thiết bị không tồn tại");
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

        public async Task<(bool Success, string Message)> NhapThietBiAsync(XuatNhapTonViewModel model)
        {
            try
            {
                var ketQua = await _xuatNhapTonRepository.NhapThietBiAsync(model);

                switch (ketQua)
                {
                    case 1:
                        return (true, "Nhập thiết bị thành công");
                    case 0:
                        return (false, "Số lượng không hợp lệ");
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

        public async Task<IEnumerable<dynamic>> BaoCaoTonKhoAsync(int? maPhongMay = null)
        {
            return await _xuatNhapTonRepository.BaoCaoTonKhoAsync(maPhongMay);
        }

        public async Task<IEnumerable<XuatNhapTonViewModel>> GetAllAsync()
        {
            return await _xuatNhapTonRepository.GetAllAsync();
        }
    }
}