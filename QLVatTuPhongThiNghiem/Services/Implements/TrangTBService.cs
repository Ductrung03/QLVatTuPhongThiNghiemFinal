using QLVatTuPhongThiNghiem.Models.Entities;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class TrangTBService : ITrangTBService
    {
        private readonly ITrangTBRepository _trangTBRepository;

        public TrangTBService(ITrangTBRepository trangTBRepository)
        {
            _trangTBRepository = trangTBRepository;
        }

        public async Task<(bool Success, int MaTTB, string Message)> CreateAsync(TrangTBViewModel model, int nguoiTao)
        {
            try
            {
                var (ketQua, maTTB) = await _trangTBRepository.InsertAsync(model, nguoiTao);

                switch (ketQua)
                {
                    case 1:
                        return (true, maTTB, "Thêm thiết bị thành công");
                    case 0:
                        return (false, 0, "Dữ liệu không hợp lệ");
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

        public async Task<(bool Success, string Message)> UpdateAsync(TrangTBViewModel model, int nguoiCapNhat)
        {
            try
            {
                var ketQua = await _trangTBRepository.UpdateAsync(model, nguoiCapNhat);

                switch (ketQua)
                {
                    case 1:
                        return (true, "Cập nhật thiết bị thành công");
                    case 0:
                        return (false, "Không tìm thấy thiết bị");
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

        public async Task<(bool Success, string Message)> DeleteAsync(int maTTB)
        {
            try
            {
                var ketQua = await _trangTBRepository.DeleteAsync(maTTB);

                switch (ketQua)
                {
                    case 1:
                        return (true, "Xóa thiết bị thành công");
                    case 0:
                        return (false, "Không thể xóa thiết bị do ràng buộc dữ liệu");
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

        public async Task<SearchTrangTBViewModel> SearchAsync(SearchTrangTBViewModel searchModel)
        {
            return await _trangTBRepository.SearchAsync(searchModel);
        }

        public async Task<TrangTBViewModel> GetByIdAsync(int maTTB)
        {
            var entity = await _trangTBRepository.GetByIdAsync(maTTB);
            if (entity == null) return null;

            return new TrangTBViewModel
            {
                MaTTB = entity.MaTTB,
                MaPhongMay = entity.MaPhongMay ?? 0,
                MaLoai = entity.MaLoai ?? 0,
                GiaTien = entity.GiaTien,
                TinhTrang = entity.TinhTrang,
                NgayNhap = entity.NgayNhap,
                MaThuongHieu = entity.MaThuongHieu ?? 0,
                SoLanSua = entity.SoLanSua
            };
        }

        public async Task<IEnumerable<TrangTB>> GetAllAsync()
        {
            return await _trangTBRepository.GetAllAsync();
        }
    }
}
