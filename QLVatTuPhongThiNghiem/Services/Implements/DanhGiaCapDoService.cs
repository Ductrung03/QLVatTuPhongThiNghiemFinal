

using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class DanhGiaCapDoService : IDanhGiaCapDoService
    {
        private readonly IDanhGiaCapDoRepository _danhGiaCapDoRepository;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public DanhGiaCapDoService(IDanhGiaCapDoRepository danhGiaCapDoRepository, ILichSuHoatDongService lichSuHoatDongService)
        {
            _danhGiaCapDoRepository = danhGiaCapDoRepository;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        public async Task<(bool Success, string Message)> DanhGiaCapDoAsync(DanhGiaCapDoViewModel model)
        {
            try
            {
                // Business validation
                if (model.CapDo < 1 || model.CapDo > 5)
                {
                    return (false, "Cấp độ phải từ 1 đến 5");
                }

                if (model.MaTTB <= 0)
                {
                    return (false, "Thiết bị không hợp lệ");
                }

                if (model.NguoiDanhGia <= 0)
                {
                    return (false, "Người đánh giá không hợp lệ");
                }

                // Kiểm tra xem có thể đánh giá không
                var (canRate, message) = await KiemTraCoTheDanhGiaAsync(model.MaTTB, model.NguoiDanhGia);
                if (!canRate)
                {
                    return (false, message);
                }

                model.NgayDanhGia = DateTime.Now;

                var ketQua = await _danhGiaCapDoRepository.DanhGiaCapDoAsync(model);

                switch (ketQua)
                {
                    case 1:
                        await _lichSuHoatDongService.GhiLichSuAsync(model.NguoiDanhGia,
                            "Đánh giá cấp độ thiết bị", "Đánh giá",
                            $"Đánh giá thiết bị ID: {model.MaTTB} - Cấp độ: {model.CapDo}");
                        return (true, "Đánh giá cấp độ thiết bị thành công");
                    case 0:
                        return (false, "Cấp độ không hợp lệ hoặc thiết bị không tồn tại");
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

        // PHƯƠNG THỨC MỚI: Cập nhật đánh giá
        public async Task<(bool Success, string Message)> CapNhatDanhGiaAsync(DanhGiaCapDoViewModel model)
        {
            try
            {
                // Business validation
                if (model.CapDo < 1 || model.CapDo > 5)
                {
                    return (false, "Cấp độ phải từ 1 đến 5");
                }

                // Lấy thông tin hiện tại để kiểm tra quyền
                var currentDanhGia = await _danhGiaCapDoRepository.GetByIdAsync(model.MaDanhGia);
                if (currentDanhGia == null)
                {
                    return (false, "Không tìm thấy đánh giá");
                }

                // Kiểm tra thời gian (chỉ cho phép sửa trong 24h)
                var hoursDiff = (DateTime.Now - currentDanhGia.NgayDanhGia).TotalHours;
                if (hoursDiff > 24)
                {
                    return (false, "Chỉ có thể sửa đánh giá trong vòng 24 giờ");
                }

                var (ketQua, thongBao) = await _danhGiaCapDoRepository.CapNhatDanhGiaAsync(model);

                if (ketQua == 1)
                {
                    await _lichSuHoatDongService.GhiLichSuAsync(currentDanhGia.NguoiDanhGia,
                        "Cập nhật đánh giá", "Đánh giá",
                        $"Cập nhật đánh giá ID: {model.MaDanhGia} - Cấp độ mới: {model.CapDo}");
                }

                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Xóa đánh giá
        public async Task<(bool Success, string Message)> XoaDanhGiaAsync(int maDanhGia)
        {
            try
            {
                // Lấy thông tin đánh giá để ghi log
                var danhGiaInfo = await _danhGiaCapDoRepository.GetByIdAsync(maDanhGia);
                if (danhGiaInfo == null)
                {
                    return (false, "Không tìm thấy đánh giá");
                }

                // Kiểm tra thời gian (chỉ cho phép xóa trong 24h)
                var hoursDiff = (DateTime.Now - danhGiaInfo.NgayDanhGia).TotalHours;
                if (hoursDiff > 24)
                {
                    return (false, "Chỉ có thể xóa đánh giá trong vòng 24 giờ");
                }

                var (ketQua, thongBao) = await _danhGiaCapDoRepository.XoaDanhGiaAsync(maDanhGia);

                if (ketQua == 1)
                {
                    await _lichSuHoatDongService.GhiLichSuAsync(danhGiaInfo.NguoiDanhGia,
                        "Xóa đánh giá", "Đánh giá",
                        $"Xóa đánh giá ID: {maDanhGia} - Thiết bị: {danhGiaInfo.TenThietBi}");
                }

                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Kiểm tra có thể đánh giá không
        public async Task<(bool CanRate, string Message)> KiemTraCoTheDanhGiaAsync(int maTTB, int nguoiDanhGia)
        {
            try
            {
                // Lấy đánh giá gần nhất của người này cho thiết bị này
                var danhGiaGanNhat = (await _danhGiaCapDoRepository.GetByThietBiAsync(maTTB))
                    .Where(d => d.NguoiDanhGia == nguoiDanhGia)
                    .OrderByDescending(d => d.NgayDanhGia)
                    .FirstOrDefault();

                if (danhGiaGanNhat != null)
                {
                    // Kiểm tra thời gian (chỉ cho phép đánh giá lại sau 7 ngày)
                    var daysDiff = (DateTime.Now - danhGiaGanNhat.NgayDanhGia).TotalDays;
                    if (daysDiff < 7)
                    {
                        return (false, $"Bạn đã đánh giá thiết bị này. Có thể đánh giá lại sau {Math.Ceiling(7 - daysDiff)} ngày");
                    }
                }

                return (true, "Có thể đánh giá");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi kiểm tra: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Kiểm tra quyền chỉnh sửa đánh giá
        public async Task<bool> KiemTraQuyenChinhSuaAsync(int maDanhGia, int maNguoiDung, bool isAdmin = false)
        {
            var danhGia = await GetByIdAsync(maDanhGia);
            if (danhGia == null) return false;

            // Admin có thể sửa tất cả
            if (isAdmin) return true;

            // Người đánh giá có thể sửa trong 24h
            if (danhGia.NguoiDanhGia == maNguoiDung)
            {
                var hoursDiff = (DateTime.Now - danhGia.NgayDanhGia).TotalHours;
                return hoursDiff <= 24;
            }

            return false;
        }

        public async Task<DanhGiaCapDoViewModel> GetCapDoThietBiAsync(int maTTB)
        {
            return await _danhGiaCapDoRepository.GetCapDoThietBiAsync(maTTB);
        }

        // PHƯƠNG THỨC MỚI: Lấy chi tiết đánh giá
        public async Task<DanhGiaCapDoViewModel> GetByIdAsync(int maDanhGia)
        {
            return await _danhGiaCapDoRepository.GetByIdAsync(maDanhGia);
        }

        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetAllAsync()
        {
            return await _danhGiaCapDoRepository.GetAllAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy đánh giá theo thiết bị
        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetByThietBiAsync(int maTTB)
        {
            return await _danhGiaCapDoRepository.GetByThietBiAsync(maTTB);
        }

        // PHƯƠNG THỨC MỚI: Lấy đánh giá theo người đánh giá
        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetByNguoiDanhGiaAsync(int nguoiDanhGia)
        {
            return await _danhGiaCapDoRepository.GetByNguoiDanhGiaAsync(nguoiDanhGia);
        }

        // PHƯƠNG THỨC MỚI: Lấy đánh giá theo cấp độ
        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetByCapDoAsync(int capDo)
        {
            if (capDo < 1 || capDo > 5)
            {
                throw new ArgumentException("Cấp độ phải từ 1 đến 5");
            }

            return await _danhGiaCapDoRepository.GetByCapDoAsync(capDo);
        }

        // PHƯƠNG THỨC MỚI: Lấy đánh giá theo khoảng thời gian
        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay > denNgay)
            {
                throw new ArgumentException("Ngày bắt đầu không thể lớn hơn ngày kết thúc");
            }

            return await _danhGiaCapDoRepository.GetByDateRangeAsync(tuNgay, denNgay);
        }

        // PHƯƠNG THỨC MỚI: Thống kê đánh giá
        public async Task<Dictionary<string, object>> ThongKeDanhGiaAsync(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            var stats = await _danhGiaCapDoRepository.ThongKeDanhGiaAsync(tuNgay, denNgay);

            // Tính phần trăm cho mỗi cấp độ
            var tongSo = (int)stats["TongSoDanhGia"];
            if (tongSo > 0)
            {
                stats["PhanTramCapDo1"] = Math.Round((int)stats["CapDo1"] * 100.0 / tongSo, 2);
                stats["PhanTramCapDo2"] = Math.Round((int)stats["CapDo2"] * 100.0 / tongSo, 2);
                stats["PhanTramCapDo3"] = Math.Round((int)stats["CapDo3"] * 100.0 / tongSo, 2);
                stats["PhanTramCapDo4"] = Math.Round((int)stats["CapDo4"] * 100.0 / tongSo, 2);
                stats["PhanTramCapDo5"] = Math.Round((int)stats["CapDo5"] * 100.0 / tongSo, 2);
            }

            // Đánh giá chất lượng tổng thể
            var capDoTrungBinh = (double)stats["CapDoTrungBinh"];
            string danhGiaTongThe = capDoTrungBinh switch
            {
                >= 4.5 => "Xuất sắc",
                >= 4.0 => "Tốt",
                >= 3.5 => "Khá",
                >= 3.0 => "Trung bình",
                >= 2.0 => "Yếu",
                _ => "Kém"
            };
            stats["DanhGiaTongThe"] = danhGiaTongThe;

            return stats;
        }

        // PHƯƠNG THỨC MỚI: Lấy cấp độ trung bình của thiết bị
        public async Task<double> GetCapDoTrungBinhThietBiAsync(int maTTB)
        {
            var danhGiaList = await GetByThietBiAsync(maTTB);
            if (!danhGiaList.Any()) return 0;

            return danhGiaList.Average(d => d.CapDo);
        }

        // PHƯƠNG THỨC MỚI: Lấy xu hướng đánh giá của thiết bị
        public async Task<string> GetXuHuongDanhGiaAsync(int maTTB)
        {
            var danhGiaList = (await GetByThietBiAsync(maTTB))
                .OrderBy(d => d.NgayDanhGia)
                .Take(10) // Lấy 10 đánh giá gần nhất
                .ToList();

            if (danhGiaList.Count < 2) return "Chưa đủ dữ liệu";

            var danhGiaDau = danhGiaList.Take(5).Average(d => d.CapDo);
            var danhGiaCuoi = danhGiaList.Skip(Math.Max(0, danhGiaList.Count - 5)).Average(d => d.CapDo);

            var chenhLech = danhGiaCuoi - danhGiaDau;

            return chenhLech switch
            {
                > 0.5 => "Cải thiện đáng kể",
                > 0.2 => "Cải thiện nhẹ",
                < -0.5 => "Suy giảm đáng kể",
                < -0.2 => "Suy giảm nhẹ",
                _ => "Ổn định"
            };
        }

        // PHƯƠNG THỨC MỚI: Gợi ý hành động dựa trên đánh giá
        public async Task<List<string>> GetGoiYHanhDongAsync(int maTTB)
        {
            var capDoTrungBinh = await GetCapDoTrungBinhThietBiAsync(maTTB);
            var xuHuong = await GetXuHuongDanhGiaAsync(maTTB);
            var danhGiaGanNhat = (await GetByThietBiAsync(maTTB))
                .OrderByDescending(d => d.NgayDanhGia)
                .FirstOrDefault();

            var goiY = new List<string>();

            if (capDoTrungBinh <= 2)
            {
                goiY.Add("Cần kiểm tra và bảo trì khẩn cấp");
                goiY.Add("Xem xét thay thế thiết bị");
            }
            else if (capDoTrungBinh <= 3)
            {
                goiY.Add("Lên lịch bảo trì định kỳ");
                goiY.Add("Kiểm tra tình trạng sử dụng");
            }
            else if (capDoTrungBinh >= 4.5)
            {
                goiY.Add("Thiết bị hoạt động tốt");
                goiY.Add("Duy trì chế độ bảo trì hiện tại");
            }

            if (xuHuong == "Suy giảm đáng kể")
            {
                goiY.Add("Tăng cường kiểm tra và bảo trì");
                goiY.Add("Điều tra nguyên nhân suy giảm chất lượng");
            }

            if (danhGiaGanNhat != null && (DateTime.Now - danhGiaGanNhat.NgayDanhGia).TotalDays > 30)
            {
                goiY.Add("Cần đánh giá lại thiết bị (đã quá 30 ngày)");
            }

            return goiY;
        }

        // PHƯƠNG THỨC MỚI: Lấy top thiết bị theo cấp độ
        public async Task<IEnumerable<dynamic>> GetTopThietBiTheoCapDoAsync(bool isDescending = true, int top = 10)
        {
            var allDanhGia = await GetAllAsync();

            var thongKe = allDanhGia
                .GroupBy(d => new { d.MaTTB, d.TenThietBi })
                .Select(g => new
                {
                    MaTTB = g.Key.MaTTB,
                    TenThietBi = g.Key.TenThietBi,
                    CapDoTrungBinh = g.Average(d => d.CapDo),
                    SoDanhGia = g.Count(),
                    DanhGiaGanNhat = g.Max(d => d.NgayDanhGia)
                })
                .Where(x => x.SoDanhGia >= 3) // Chỉ lấy thiết bị có ít nhất 3 đánh giá
                .OrderBy(x => isDescending ? -x.CapDoTrungBinh : x.CapDoTrungBinh)
                .Take(top);

            return thongKe;
        }
    }
}