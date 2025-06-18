using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface ISuaChuaService
    {
        Task<(bool Success, string Message)> CapNhatPhieuSuaChuaAsync(SuaChuaViewModel model);
        Task<IEnumerable<SuaChuaViewModel>> GetAllAsync();
        Task<IEnumerable<SuaChuaViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay);
        Task<SuaChuaViewModel> GetByIdAsync(int maSuaChua);
        Task<IEnumerable<SuaChuaViewModel>> GetByNguoiThucHienAsync(int nguoiThucHien);
        Task<IEnumerable<SuaChuaViewModel>> GetByThietBiAsync(int maTTB);
        Task<IEnumerable<SuaChuaViewModel>> GetByTrangThaiAsync(string trangThai);
        List<string> GetDanhSachLoaiSuaChua();
        List<string> GetDanhSachTrangThaiThietBi();
        Task<(bool Success, string Message)> HoanThanhSuaChuaAsync(int maSuaChua, decimal chiPhi, string tinhTrangMoi);
        Task<(bool Success, string Message)> HuyPhieuSuaChuaAsync(int maSuaChua, string lyDo);
        Task<bool> KiemTraQuyenChinhSuaAsync(int maSuaChua, int maNguoiDung, bool isAdmin = false);
        Task<(bool Success, int MaSuaChua, string Message)> TaoPhieuSuaChuaAsync(SuaChuaViewModel model);
        Task<Dictionary<string, object>> ThongKeSuaChuaAsync(DateTime? tuNgay = null, DateTime? denNgay = null);
        Task<(bool Success, string Message)> XoaPhieuSuaChuaAsync(int maSuaChua);
    }
}