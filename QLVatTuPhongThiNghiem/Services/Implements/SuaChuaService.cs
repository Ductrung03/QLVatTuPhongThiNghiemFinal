


using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class SuaChuaService : ISuaChuaService
    {
        private readonly ISuaChuaRepository _suaChuaRepository;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public SuaChuaService(ISuaChuaRepository suaChuaRepository, ILichSuHoatDongService lichSuHoatDongService)
        {
            _suaChuaRepository = suaChuaRepository;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        public async Task<(bool Success, int MaSuaChua, string Message)> TaoPhieuSuaChuaAsync(SuaChuaViewModel model)
        {
            try
            {
                // Business validation
                if (string.IsNullOrWhiteSpace(model.LoaiSuaChua))
                {
                    return (false, 0, "Loại sửa chữa là bắt buộc");
                }

                if (string.IsNullOrWhiteSpace(model.MoTa))
                {
                    return (false, 0, "Mô tả sự cố là bắt buộc");
                }

                if (model.MoTa.Length < 10)
                {
                    return (false, 0, "Mô tả sự cố phải có ít nhất 10 ký tự");
                }

                // Validate loại sửa chữa
                var validTypes = new[] { "Bảo trì định kỳ", "Sửa chữa khẩn cấp", "Thay thế linh kiện", "Nâng cấp", "Bảo hành" };
                if (!validTypes.Contains(model.LoaiSuaChua))
                {
                    return (false, 0, "Loại sửa chữa không hợp lệ");
                }

                var (ketQua, maSuaChua) = await _suaChuaRepository.TaoPhieuSuaChuaAsync(model);

                switch (ketQua)
                {
                    case 1:
                        await _lichSuHoatDongService.GhiLichSuAsync(model.NguoiThucHien,
                            "Tạo phiếu sửa chữa", "Sửa chữa",
                            $"Tạo phiếu sửa chữa {model.LoaiSuaChua} cho thiết bị ID: {model.MaTTB}");
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
                // Business validation
                if (chiPhi < 0)
                {
                    return (false, "Chi phí không thể âm");
                }

                if (chiPhi > 1000000000) // 1 tỷ
                {
                    return (false, "Chi phí quá lớn, vui lòng kiểm tra lại");
                }

                if (string.IsNullOrWhiteSpace(tinhTrangMoi))
                {
                    return (false, "Tình trạng mới của thiết bị là bắt buộc");
                }

                var validStatuses = new[] { "Tốt", "Khá tốt", "Trung bình", "Hỏng", "Hư hỏng nặng" };
                if (!validStatuses.Contains(tinhTrangMoi))
                {
                    return (false, "Tình trạng thiết bị không hợp lệ");
                }

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

        // PHƯƠNG THỨC MỚI: Cập nhật phiếu sửa chữa
        public async Task<(bool Success, string Message)> CapNhatPhieuSuaChuaAsync(SuaChuaViewModel model)
        {
            try
            {
                // Business validation
                if (string.IsNullOrWhiteSpace(model.LoaiSuaChua))
                {
                    return (false, "Loại sửa chữa là bắt buộc");
                }

                if (string.IsNullOrWhiteSpace(model.MoTa))
                {
                    return (false, "Mô tả sự cố là bắt buộc");
                }

                if (model.ChiPhi.HasValue && model.ChiPhi < 0)
                {
                    return (false, "Chi phí không thể âm");
                }

                // Lấy thông tin hiện tại để kiểm tra quyền
                var currentPhieu = await _suaChuaRepository.GetByIdAsync(model.MaSuaChua);
                if (currentPhieu == null)
                {
                    return (false, "Không tìm thấy phiếu sửa chữa");
                }

                var (ketQua, thongBao) = await _suaChuaRepository.CapNhatPhieuSuaChuaAsync(model);

                if (ketQua == 1)
                {
                    await _lichSuHoatDongService.GhiLichSuAsync(currentPhieu.NguoiThucHien,
                        "Cập nhật phiếu sửa chữa", "Sửa chữa",
                        $"Cập nhật phiếu sửa chữa ID: {model.MaSuaChua}");
                }

                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Xóa phiếu sửa chữa
        public async Task<(bool Success, string Message)> XoaPhieuSuaChuaAsync(int maSuaChua)
        {
            try
            {
                // Lấy thông tin phiếu để ghi log
                var phieuInfo = await _suaChuaRepository.GetByIdAsync(maSuaChua);
                if (phieuInfo == null)
                {
                    return (false, "Không tìm thấy phiếu sửa chữa");
                }

                var (ketQua, thongBao) = await _suaChuaRepository.XoaPhieuSuaChuaAsync(maSuaChua);

                if (ketQua == 1)
                {
                    await _lichSuHoatDongService.GhiLichSuAsync(phieuInfo.NguoiThucHien,
                        "Xóa phiếu sửa chữa", "Sửa chữa",
                        $"Xóa phiếu sửa chữa ID: {maSuaChua} - {phieuInfo.LoaiSuaChua}");
                }

                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Hủy phiếu sửa chữa với lý do
        public async Task<(bool Success, string Message)> HuyPhieuSuaChuaAsync(int maSuaChua, string lyDo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lyDo))
                {
                    return (false, "Lý do hủy là bắt buộc");
                }

                if (lyDo.Length < 5)
                {
                    return (false, "Lý do hủy phải có ít nhất 5 ký tự");
                }

                // Lấy thông tin phiếu để ghi log
                var phieuInfo = await _suaChuaRepository.GetByIdAsync(maSuaChua);
                if (phieuInfo == null)
                {
                    return (false, "Không tìm thấy phiếu sửa chữa");
                }

                var (ketQua, thongBao) = await _suaChuaRepository.HuyPhieuSuaChuaAsync(maSuaChua, lyDo);

                if (ketQua == 1)
                {
                    await _lichSuHoatDongService.GhiLichSuAsync(phieuInfo.NguoiThucHien,
                        "Hủy phiếu sửa chữa", "Sửa chữa",
                        $"Hủy phiếu sửa chữa ID: {maSuaChua} - Lý do: {lyDo}");
                }

                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Lấy chi tiết phiếu sửa chữa
        public async Task<SuaChuaViewModel> GetByIdAsync(int maSuaChua)
        {
            return await _suaChuaRepository.GetByIdAsync(maSuaChua);
        }

        public async Task<IEnumerable<SuaChuaViewModel>> GetAllAsync()
        {
            return await _suaChuaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<SuaChuaViewModel>> GetByTrangThaiAsync(string trangThai)
        {
            return await _suaChuaRepository.GetByTrangThaiAsync(trangThai);
        }

        // PHƯƠNG THỨC MỚI: Lấy phiếu sửa chữa theo thiết bị
        public async Task<IEnumerable<SuaChuaViewModel>> GetByThietBiAsync(int maTTB)
        {
            return await _suaChuaRepository.GetByThietBiAsync(maTTB);
        }

        // PHƯƠNG THỨC MỚI: Lấy phiếu sửa chữa theo người thực hiện
        public async Task<IEnumerable<SuaChuaViewModel>> GetByNguoiThucHienAsync(int nguoiThucHien)
        {
            return await _suaChuaRepository.GetByNguoiThucHienAsync(nguoiThucHien);
        }

        // PHƯƠNG THỨC MỚI: Lấy phiếu sửa chữa theo khoảng thời gian
        public async Task<IEnumerable<SuaChuaViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay > denNgay)
            {
                throw new ArgumentException("Ngày bắt đầu không thể lớn hơn ngày kết thúc");
            }

            return await _suaChuaRepository.GetByDateRangeAsync(tuNgay, denNgay);
        }

        // PHƯƠNG THỨC MỚI: Thống kê sửa chữa
        public async Task<Dictionary<string, object>> ThongKeSuaChuaAsync(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            var allPhieu = await GetAllAsync();

            if (tuNgay.HasValue)
                allPhieu = allPhieu.Where(p => p.NgayBatDau.Date >= tuNgay.Value.Date);

            if (denNgay.HasValue)
                allPhieu = allPhieu.Where(p => p.NgayBatDau.Date <= denNgay.Value.Date);

            var result = new Dictionary<string, object>
            {
                ["TongSoPhieu"] = allPhieu.Count(),
                ["ChoXuLy"] = allPhieu.Count(p => p.TrangThai == "Chờ xử lý"),
                ["DangSuaChua"] = allPhieu.Count(p => p.TrangThai == "Đang sửa chữa"),
                ["HoanThanh"] = allPhieu.Count(p => p.TrangThai == "Hoàn thành"),
                ["DaHuy"] = allPhieu.Count(p => p.TrangThai == "Đã hủy"),
                ["TongChiPhi"] = allPhieu.Where(p => p.ChiPhi.HasValue).Sum(p => p.ChiPhi.Value),
                ["ChiPhiTrungBinh"] = allPhieu.Where(p => p.ChiPhi.HasValue).Any() ?
                    allPhieu.Where(p => p.ChiPhi.HasValue).Average(p => p.ChiPhi.Value) : 0,
                ["SoThietBiDuocSua"] = allPhieu.Select(p => p.MaTTB).Distinct().Count(),
                ["PhieuHomNay"] = allPhieu.Count(p => p.NgayBatDau.Date == DateTime.Today),
                ["PhieuTuanNay"] = allPhieu.Count(p => p.NgayBatDau.Date >= DateTime.Today.AddDays(-7))
            };

            // Thống kê theo loại sửa chữa
            var thongKeTheoLoai = allPhieu
                .GroupBy(p => p.LoaiSuaChua)
                .Select(g => new
                {
                    LoaiSuaChua = g.Key,
                    SoLuong = g.Count(),
                    TongChiPhi = g.Where(p => p.ChiPhi.HasValue).Sum(p => p.ChiPhi.Value),
                    ChiPhiTrungBinh = g.Where(p => p.ChiPhi.HasValue).Any() ?
                        g.Where(p => p.ChiPhi.HasValue).Average(p => p.ChiPhi.Value) : 0
                })
                .OrderByDescending(x => x.SoLuong)
                .ToList();

            result["ThongKeTheoLoai"] = thongKeTheoLoai;

            return result;
        }

        // PHƯƠNG THỨC MỚI: Kiểm tra quyền chỉnh sửa phiếu sửa chữa
        public async Task<bool> KiemTraQuyenChinhSuaAsync(int maSuaChua, int maNguoiDung, bool isAdmin = false)
        {
            var phieu = await GetByIdAsync(maSuaChua);
            if (phieu == null) return false;

            // Admin có thể sửa tất cả
            if (isAdmin) return true;

            // Người tạo có thể sửa nếu chưa hoàn thành
            if (phieu.NguoiThucHien == maNguoiDung &&
                phieu.TrangThai != "Hoàn thành")
            {
                return true;
            }

            return false;
        }

        // PHƯƠNG THỨC MỚI: Lấy danh sách loại sửa chữa
        public List<string> GetDanhSachLoaiSuaChua()
        {
            return new List<string>
            {
                "Bảo trì định kỳ",
                "Sửa chữa khẩn cấp",
                "Thay thế linh kiện",
                "Nâng cấp",
                "Bảo hành",
                "Sửa chữa chung"
            };
        }

        // PHƯƠNG THỨC MỚI: Lấy danh sách trạng thái thiết bị
        public List<string> GetDanhSachTrangThaiThietBi()
        {
            return new List<string>
            {
                "Tốt",
                "Khá tốt",
                "Trung bình",
                "Hỏng",
                "Hư hỏng nặng",
                "Cần thay thế"
            };
        }
    }
}