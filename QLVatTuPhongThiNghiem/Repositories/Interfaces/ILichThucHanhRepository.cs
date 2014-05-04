

using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface ILichThucHanhRepository
    {
        // Các phương thức đã có
        Task<(int KetQua, int MaLich)> DangKyLichAsync(LichThucHanhViewModel model);
        Task<int> CapNhatTrangThaiAsync(int maLich, string trangThai);
        Task<IEnumerable<LichThucHanhViewModel>> GetAllAsync();
        Task<IEnumerable<LichThucHanhViewModel>> GetByUserAsync(int maNguoiDung);

        // Các phương thức bổ sung
        Task<LichThucHanhViewModel> GetByIdAsync(int maLich);
        Task<(int KetQua, string ThongBao)> CapNhatLichAsync(LichThucHanhViewModel model);
        Task<(int KetQua, string ThongBao)> XoaLichAsync(int maLich, int maNguoiDung);
        Task<IEnumerable<LichThucHanhViewModel>> GetByNgayAsync(DateTime ngay);
        Task<IEnumerable<LichThucHanhViewModel>> GetByTrangThaiAsync(string trangThai);
        Task<bool> KiemTraTrungLichAsync(DateTime thoiGianBD, DateTime thoiGianKT, int? maLichLoaiTru = null);
    }
}