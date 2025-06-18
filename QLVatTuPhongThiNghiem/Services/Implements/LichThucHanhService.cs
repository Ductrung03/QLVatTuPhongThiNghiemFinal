// File: QLVatTuPhongThiNghiem/Services/Implements/LichThucHanhService.cs
// Vị trí: QLVatTuPhongThiNghiem/Services/Implements/LichThucHanhService.cs
// Thay thế file cũ - Bổ sung thêm business logic cho CRUD đầy đủ

using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class LichThucHanhService : ILichThucHanhService
    {
        private readonly ILichThucHanhRepository _lichThucHanhRepository;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public LichThucHanhService(ILichThucHanhRepository lichThucHanhRepository, ILichSuHoatDongService lichSuHoatDongService)
        {
            _lichThucHanhRepository = lichThucHanhRepository;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        public async Task<(bool Success, int MaLich, string Message)> DangKyLichAsync(LichThucHanhViewModel model)
        {
            try
            {
                // Business validation
                if (model.ThoiGianBD >= model.ThoiGianKT)
                {
                    return (false, 0, "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");
                }

                if (model.ThoiGianBD <= DateTime.Now)
                {
                    return (false, 0, "Thời gian bắt đầu phải lớn hơn thời gian hiện tại");
                }

                var timeSpan = model.ThoiGianKT - model.ThoiGianBD;
                if (timeSpan.TotalMinutes < 30)
                {
                    return (false, 0, "Thời gian thực hành tối thiểu 30 phút");
                }

                if (timeSpan.TotalHours > 8)
                {
                    return (false, 0, "Thời gian thực hành tối đa 8 giờ");
                }

                // Kiểm tra trùng lịch
                var hasConflict = await _lichThucHanhRepository.KiemTraTrungLichAsync(model.ThoiGianBD, model.ThoiGianKT);
                if (hasConflict)
                {
                    return (false, 0, "Thời gian đã có lịch thực hành khác");
                }

                var (ketQua, maLich) = await _lichThucHanhRepository.DangKyLichAsync(model);

                switch (ketQua)
                {
                    case 1:
                        await _lichSuHoatDongService.GhiLichSuAsync(model.MaNguoiDung.Value,
                            "Đăng ký lịch thực hành", "Lịch thực hành",
                            $"Đăng ký lịch từ {model.ThoiGianBD:dd/MM/yyyy HH:mm} đến {model.ThoiGianKT:dd/MM/yyyy HH:mm}");
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
                // Business validation cho trạng thái
                var validStatuses = new[] { "Chờ duyệt", "Đã duyệt", "Đang thực hiện", "Hoàn thành", "Đã hủy" };
                if (!validStatuses.Contains(trangThai))
                {
                    return (false, "Trạng thái không hợp lệ");
                }

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

        // PHƯƠNG THỨC MỚI: Cập nhật thông tin lịch thực hành
        public async Task<(bool Success, string Message)> CapNhatLichAsync(LichThucHanhViewModel model)
        {
            try
            {
                // Business validation
                if (model.ThoiGianBD >= model.ThoiGianKT)
                {
                    return (false, "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");
                }

                var timeSpan = model.ThoiGianKT - model.ThoiGianBD;
                if (timeSpan.TotalMinutes < 30)
                {
                    return (false, "Thời gian thực hành tối thiểu 30 phút");
                }

                if (timeSpan.TotalHours > 8)
                {
                    return (false, "Thời gian thực hành tối đa 8 giờ");
                }

                // Lấy thông tin hiện tại để kiểm tra quyền
                var currentLich = await _lichThucHanhRepository.GetByIdAsync(model.MaLich);
                if (currentLich == null)
                {
                    return (false, "Không tìm thấy lịch thực hành");
                }

                // Kiểm tra trạng thái có thể sửa
                if (currentLich.TrangThai == "Hoàn thành" || currentLich.TrangThai == "Đang thực hiện")
                {
                    return (false, "Không thể sửa lịch đã hoàn thành hoặc đang thực hiện");
                }

                var (ketQua, thongBao) = await _lichThucHanhRepository.CapNhatLichAsync(model);

                if (ketQua == 1)
                {
                    await _lichSuHoatDongService.GhiLichSuAsync(currentLich.MaNguoiDung.Value,
                        "Cập nhật lịch thực hành", "Lịch thực hành",
                        $"Cập nhật lịch từ {model.ThoiGianBD:dd/MM/yyyy HH:mm} đến {model.ThoiGianKT:dd/MM/yyyy HH:mm}");
                }

                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Xóa lịch thực hành
        public async Task<(bool Success, string Message)> XoaLichAsync(int maLich, int maNguoiDung)
        {
            try
            {
                // Lấy thông tin lịch để ghi log
                var lichInfo = await _lichThucHanhRepository.GetByIdAsync(maLich);
                if (lichInfo == null)
                {
                    return (false, "Không tìm thấy lịch thực hành");
                }

                var (ketQua, thongBao) = await _lichThucHanhRepository.XoaLichAsync(maLich, maNguoiDung);

                if (ketQua == 1)
                {
                    await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung,
                        "Xóa lịch thực hành", "Lịch thực hành",
                        $"Xóa lịch từ {lichInfo.ThoiGianBD:dd/MM/yyyy HH:mm} đến {lichInfo.ThoiGianKT:dd/MM/yyyy HH:mm}");
                }

                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Lấy chi tiết lịch theo ID
        public async Task<LichThucHanhViewModel> GetByIdAsync(int maLich)
        {
            return await _lichThucHanhRepository.GetByIdAsync(maLich);
        }

        public async Task<IEnumerable<LichThucHanhViewModel>> GetAllAsync()
        {
            return await _lichThucHanhRepository.GetAllAsync();
        }

        public async Task<IEnumerable<LichThucHanhViewModel>> GetByUserAsync(int maNguoiDung)
        {
            return await _lichThucHanhRepository.GetByUserAsync(maNguoiDung);
        }

        // PHƯƠNG THỨC MỚI: Lấy lịch theo ngày
        public async Task<IEnumerable<LichThucHanhViewModel>> GetByNgayAsync(DateTime ngay)
        {
            return await _lichThucHanhRepository.GetByNgayAsync(ngay);
        }

        // PHƯƠNG THỨC MỚI: Lấy lịch theo trạng thái
        public async Task<IEnumerable<LichThucHanhViewModel>> GetByTrangThaiAsync(string trangThai)
        {
            return await _lichThucHanhRepository.GetByTrangThaiAsync(trangThai);
        }

        // PHƯƠNG THỨC MỚI: Kiểm tra quyền sửa/xóa lịch
        public async Task<bool> KiemTraQuyenChinhSuaAsync(int maLich, int maNguoiDung, bool isAdmin = false)
        {
            var lich = await _lichThucHanhRepository.GetByIdAsync(maLich);
            if (lich == null) return false;

            // Admin có thể sửa tất cả
            if (isAdmin) return true;

            // Chủ sở hữu có thể sửa nếu chưa hoàn thành
            if (lich.MaNguoiDung == maNguoiDung &&
                lich.TrangThai != "Hoàn thành" &&
                lich.TrangThai != "Đang thực hiện")
            {
                return true;
            }

            return false;
        }

        // PHƯƠNG THỨC MỚI: Thống kê lịch thực hành
        public async Task<Dictionary<string, object>> ThongKeLichThucHanhAsync(int? maNguoiDung = null, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            var allLich = maNguoiDung.HasValue
                ? await GetByUserAsync(maNguoiDung.Value)
                : await GetAllAsync();

            if (tuNgay.HasValue)
                allLich = allLich.Where(l => l.ThoiGianBD.Date >= tuNgay.Value.Date);

            if (denNgay.HasValue)
                allLich = allLich.Where(l => l.ThoiGianBD.Date <= denNgay.Value.Date);

            var result = new Dictionary<string, object>
            {
                ["TongSoLich"] = allLich.Count(),
                ["ChoDuyet"] = allLich.Count(l => l.TrangThai == "Chờ duyệt"),
                ["DaDuyet"] = allLich.Count(l => l.TrangThai == "Đã duyệt"),
                ["DangThucHien"] = allLich.Count(l => l.TrangThai == "Đang thực hiện"),
                ["HoanThanh"] = allLich.Count(l => l.TrangThai == "Hoàn thành"),
                ["DaHuy"] = allLich.Count(l => l.TrangThai == "Đã hủy"),
                ["TongGioThucHanh"] = allLich.Where(l => l.TrangThai == "Hoàn thành")
                    .Sum(l => (l.ThoiGianKT - l.ThoiGianBD).TotalHours),
                ["LichHomNay"] = allLich.Count(l => l.ThoiGianBD.Date == DateTime.Today),
                ["LichTuanNay"] = allLich.Count(l => l.ThoiGianBD.Date >= DateTime.Today.AddDays(-7))
            };

            return result;
        }
    }
}