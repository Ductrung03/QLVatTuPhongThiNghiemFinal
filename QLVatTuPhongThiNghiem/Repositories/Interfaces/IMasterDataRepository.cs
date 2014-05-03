using QLVatTuPhongThiNghiem.Models.Entities;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface IMasterDataRepository
    {
        // Loai CRUD
        Task<IEnumerable<Loai>> GetAllLoaiAsync();
        Task<Loai> GetLoaiByIdAsync(int maLoai);
        Task<(int KetQua, string ThongBao)> CreateLoaiAsync(Loai loai);
        Task<(int KetQua, string ThongBao)> UpdateLoaiAsync(Loai loai);
        Task<(int KetQua, string ThongBao)> DeleteLoaiAsync(int maLoai);

        // ThuongHieu CRUD
        Task<IEnumerable<ThuongHieu>> GetAllThuongHieuAsync();
        Task<ThuongHieu> GetThuongHieuByIdAsync(int maThuongHieu);
        Task<(int KetQua, string ThongBao)> CreateThuongHieuAsync(ThuongHieu thuongHieu);
        Task<(int KetQua, string ThongBao)> UpdateThuongHieuAsync(ThuongHieu thuongHieu);
        Task<(int KetQua, string ThongBao)> DeleteThuongHieuAsync(int maThuongHieu);

        // PhongMay CRUD
        Task<IEnumerable<PhongMay>> GetAllPhongMayAsync();
        Task<PhongMay> GetPhongMayByIdAsync(int maPhongMay);
        Task<(int KetQua, string ThongBao)> CreatePhongMayAsync(PhongMay phongMay);
        Task<(int KetQua, string ThongBao)> UpdatePhongMayAsync(PhongMay phongMay);
        Task<(int KetQua, string ThongBao)> DeletePhongMayAsync(int maPhongMay);

        // NhanVien CRUD
        Task<IEnumerable<NhanVien>> GetAllNhanVienAsync();
        Task<NhanVien> GetNhanVienByIdAsync(int maNV);
        Task<(int KetQua, string ThongBao)> CreateNhanVienAsync(NhanVien nhanVien);
        Task<(int KetQua, string ThongBao)> UpdateNhanVienAsync(NhanVien nhanVien);
        Task<(int KetQua, string ThongBao)> DeleteNhanVienAsync(int maNV);
    }
}
