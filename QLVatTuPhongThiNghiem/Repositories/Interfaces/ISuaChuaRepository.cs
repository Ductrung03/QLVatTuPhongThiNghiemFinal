using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface ISuaChuaRepository
    {
        // Các phương thức đã có
        Task<(int KetQua, int MaSuaChua)> TaoPhieuSuaChuaAsync(SuaChuaViewModel model);
        Task<int> HoanThanhSuaChuaAsync(int maSuaChua, decimal chiPhi, string tinhTrangMoi);
        Task<IEnumerable<SuaChuaViewModel>> GetAllAsync();
        Task<IEnumerable<SuaChuaViewModel>> GetByTrangThaiAsync(string trangThai);

        // Các phương thức bổ sung
        Task<SuaChuaViewModel> GetByIdAsync(int maSuaChua);
        Task<(int KetQua, string ThongBao)> CapNhatPhieuSuaChuaAsync(SuaChuaViewModel model);
        Task<(int KetQua, string ThongBao)> XoaPhieuSuaChuaAsync(int maSuaChua);
        Task<IEnumerable<SuaChuaViewModel>> GetByThietBiAsync(int maTTB);
        Task<IEnumerable<SuaChuaViewModel>> GetByNguoiThucHienAsync(int nguoiThucHien);
        Task<IEnumerable<SuaChuaViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay);
        Task<(int KetQua, string ThongBao)> HuyPhieuSuaChuaAsync(int maSuaChua, string lyDo);
    }
}
