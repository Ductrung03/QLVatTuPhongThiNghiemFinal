// File: Services/Interfaces/IXuatNhapTonService.cs (Cập nhật)
// Vị trí: QLVatTuPhongThiNghiem/Services/Interfaces/IXuatNhapTonService.cs
// Thay thế file cũ

using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface IXuatNhapTonService
    {
        // Các phương thức CRUD cơ bản
        Task<(bool Success, string Message)> XuatThietBiAsync(XuatNhapTonViewModel model);
        Task<(bool Success, string Message)> NhapThietBiAsync(XuatNhapTonViewModel model);
        Task<XuatNhapTonViewModel> GetByIdAsync(int maPhieu);
        Task<(bool Success, string Message)> CapNhatPhieuAsync(XuatNhapTonViewModel model, int nguoiCapNhat);
        Task<(bool Success, string Message)> XoaPhieuAsync(int maPhieu, int nguoiXoa);

        // Các phương thức truy vấn
        Task<IEnumerable<XuatNhapTonViewModel>> GetAllAsync();
        Task<IEnumerable<XuatNhapTonViewModel>> GetByThietBiAsync(int maTTB);
        Task<IEnumerable<XuatNhapTonViewModel>> GetByNguoiTaoAsync(int nguoiTao);
        Task<IEnumerable<XuatNhapTonViewModel>> GetByLoaiPhieuAsync(string loaiPhieu);
        Task<IEnumerable<XuatNhapTonViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay);

        // Các phương thức báo cáo và thống kê
        Task<IEnumerable<dynamic>> BaoCaoTonKhoAsync(int? maPhongMay = null);
        Task<Dictionary<string, object>> ThongKeXuatNhapAsync(DateTime? tuNgay = null, DateTime? denNgay = null);
        Task<int> GetTonKhoThietBiAsync(int maTTB);
        Task<Dictionary<string, object>> GetThongTinTonKhoChiTietAsync(int maTTB);

        // Các phương thức hỗ trợ
        Task<bool> KiemTraQuyenChinhSuaAsync(int maPhieu, int maNguoiDung, bool isAdmin = false);
        Task<(bool CanDelete, string Message)> KiemTraCoTheXoaAsync(int maPhieu);
        List<string> GetDanhSachLoaiPhieu();
    }
}