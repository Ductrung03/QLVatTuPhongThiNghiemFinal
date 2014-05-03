using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class VaiTroService : IVaiTroService
    {
        private readonly IVaiTroRepository _vaiTroRepository;

        public VaiTroService(IVaiTroRepository vaiTroRepository)
        {
            _vaiTroRepository = vaiTroRepository;
        }

        public async Task<IEnumerable<VaiTroViewModel>> GetAllAsync()
        {
            return await _vaiTroRepository.GetAllAsync();
        }

        public async Task<VaiTroViewModel> GetByIdAsync(int maVaiTro)
        {
            return await _vaiTroRepository.GetByIdAsync(maVaiTro);
        }

        public async Task<(bool Success, string Message)> CreateAsync(VaiTroViewModel model)
        {
            try
            {
                var (ketQua, thongBao) = await _vaiTroRepository.CreateAsync(model);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(VaiTroViewModel model)
        {
            try
            {
                var (ketQua, thongBao) = await _vaiTroRepository.UpdateAsync(model);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int maVaiTro)
        {
            try
            {
                var (ketQua, thongBao) = await _vaiTroRepository.DeleteAsync(maVaiTro);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<IEnumerable<QuyenHanViewModel>> GetAllQuyenHanAsync()
        {
            return await _vaiTroRepository.GetAllQuyenHanAsync();
        }

        public async Task<IEnumerable<QuyenHanViewModel>> GetQuyenHanByVaiTroAsync(int maVaiTro)
        {
            return await _vaiTroRepository.GetQuyenHanByVaiTroAsync(maVaiTro);
        }

        public async Task<(bool Success, string Message)> CapNhatQuyenHanAsync(int maVaiTro, List<int> danhSachQuyen)
        {
            try
            {
                var (ketQua, thongBao) = await _vaiTroRepository.CapNhatQuyenHanAsync(maVaiTro, danhSachQuyen);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }
    }
}
