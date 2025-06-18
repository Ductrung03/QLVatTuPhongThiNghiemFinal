using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface IDanhGiaCapDoService
    {
        Task<(bool Success, string Message)> CapNhatDanhGiaAsync(DanhGiaCapDoViewModel model);
        Task<(bool Success, string Message)> DanhGiaCapDoAsync(DanhGiaCapDoViewModel model);
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetAllAsync();
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetByCapDoAsync(int capDo);
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay);
        Task<DanhGiaCapDoViewModel> GetByIdAsync(int maDanhGia);
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetByNguoiDanhGiaAsync(int nguoiDanhGia);
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetByThietBiAsync(int maTTB);
        Task<DanhGiaCapDoViewModel> GetCapDoThietBiAsync(int maTTB);
        Task<double> GetCapDoTrungBinhThietBiAsync(int maTTB);
        Task<List<string>> GetGoiYHanhDongAsync(int maTTB);
        Task<IEnumerable<dynamic>> GetTopThietBiTheoCapDoAsync(bool isDescending = true, int top = 10);
        Task<string> GetXuHuongDanhGiaAsync(int maTTB);
        Task<(bool CanRate, string Message)> KiemTraCoTheDanhGiaAsync(int maTTB, int nguoiDanhGia);
        Task<bool> KiemTraQuyenChinhSuaAsync(int maDanhGia, int maNguoiDung, bool isAdmin = false);
        Task<Dictionary<string, object>> ThongKeDanhGiaAsync(DateTime? tuNgay = null, DateTime? denNgay = null);
        Task<(bool Success, string Message)> XoaDanhGiaAsync(int maDanhGia);
    }
}