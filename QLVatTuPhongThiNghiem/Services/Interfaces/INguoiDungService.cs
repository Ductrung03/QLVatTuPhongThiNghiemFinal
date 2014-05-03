using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface INguoiDungService
    {
        Task<(bool Success, int MaNguoiDung, string Message)> DangNhapBaoMatAsync(LoginViewModel model, string diaChiIP, string userAgent);
        Task<(bool Success, string Message)> DoiMatKhauAsync(int maNguoiDung, DoiMatKhauViewModel model);
        Task<(bool Success, int MaNguoiDung, string Message)> TaoTaiKhoanAsync(TaoTaiKhoanViewModel model, int maNguoiTao);
        Task<IEnumerable<NguoiDungViewModel>> GetAllAsync();
        Task<NguoiDungViewModel> GetByIdAsync(int maNguoiDung);
        Task<(bool Success, string Message)> CapNhatTrangThaiAsync(int maNguoiDung, bool trangThai);
        Task<(bool Success, string Message)> CapNhatThongTinAsync(NguoiDungViewModel model);
        Task<IEnumerable<string>> GetQuyenHanAsync(int maNguoiDung);
        Task<(bool Success, string Message)> PhanQuyenAsync(int maNguoiDung, List<int> danhSachVaiTro);
        Task<bool> KiemTraQuyenAsync(int maNguoiDung, string tenQuyen);
    }
}
