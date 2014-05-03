using QLVatTuPhongThiNghiem.Models.Entities;
using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Services.Interfaces
{
    public interface IMasterDataService
    {
        // Loai
        Task<IEnumerable<Loai>> GetLoaiAsync();
        Task<Loai> GetLoaiByIdAsync(int maLoai);
        Task<(bool Success, string Message)> CreateLoaiAsync(Loai loai);
        Task<(bool Success, string Message)> UpdateLoaiAsync(Loai loai);
        Task<(bool Success, string Message)> DeleteLoaiAsync(int maLoai);

        // ThuongHieu
        Task<IEnumerable<ThuongHieu>> GetThuongHieuAsync();
        Task<ThuongHieu> GetThuongHieuByIdAsync(int maThuongHieu);
        Task<(bool Success, string Message)> CreateThuongHieuAsync(ThuongHieu thuongHieu);
        Task<(bool Success, string Message)> UpdateThuongHieuAsync(ThuongHieu thuongHieu);
        Task<(bool Success, string Message)> DeleteThuongHieuAsync(int maThuongHieu);

        // PhongMay
        Task<IEnumerable<PhongMay>> GetPhongMayAsync();
        Task<PhongMay> GetPhongMayByIdAsync(int maPhongMay);
        Task<(bool Success, string Message)> CreatePhongMayAsync(PhongMay phongMay);
        Task<(bool Success, string Message)> UpdatePhongMayAsync(PhongMay phongMay);
        Task<(bool Success, string Message)> DeletePhongMayAsync(int maPhongMay);

        // NhanVien
        Task<IEnumerable<NhanVien>> GetNhanVienAsync();
        Task<NhanVien> GetNhanVienByIdAsync(int maNV);
        Task<(bool Success, string Message)> CreateNhanVienAsync(NhanVien nhanVien);
        Task<(bool Success, string Message)> UpdateNhanVienAsync(NhanVien nhanVien);
        Task<(bool Success, string Message)> DeleteNhanVienAsync(int maNV);

        // Business logic validation
        Task<bool> CanDeleteLoaiAsync(int maLoai);
        Task<bool> CanDeleteThuongHieuAsync(int maThuongHieu);
        Task<bool> CanDeletePhongMayAsync(int maPhongMay);
        Task<bool> CanDeleteNhanVienAsync(int maNV);
    }
}

