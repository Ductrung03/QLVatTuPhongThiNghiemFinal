

using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface ILichThucHanhService
    {
        // Các phương thức đã có
        Task<(bool Success, int MaLich, string Message)> DangKyLichAsync(LichThucHanhViewModel model);
        Task<(bool Success, string Message)> CapNhatTrangThaiAsync(int maLich, string trangThai);
        Task<IEnumerable<LichThucHanhViewModel>> GetAllAsync();
        Task<IEnumerable<LichThucHanhViewModel>> GetByUserAsync(int maNguoiDung);

        // Các phương thức bổ sung
        Task<LichThucHanhViewModel> GetByIdAsync(int maLich);
        Task<(bool Success, string Message)> CapNhatLichAsync(LichThucHanhViewModel model);
        Task<(bool Success, string Message)> XoaLichAsync(int maLich, int maNguoiDung);
        Task<IEnumerable<LichThucHanhViewModel>> GetByNgayAsync(DateTime ngay);
        Task<IEnumerable<LichThucHanhViewModel>> GetByTrangThaiAsync(string trangThai);
        Task<bool> KiemTraQuyenChinhSuaAsync(int maLich, int maNguoiDung, bool isAdmin = false);
        Task<Dictionary<string, object>> ThongKeLichThucHanhAsync(int? maNguoiDung = null, DateTime? tuNgay = null, DateTime? denNgay = null);
    }
}