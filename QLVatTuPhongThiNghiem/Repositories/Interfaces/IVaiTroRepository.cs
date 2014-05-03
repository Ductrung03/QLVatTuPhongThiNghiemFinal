using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface IVaiTroRepository
    {
        Task<IEnumerable<VaiTroViewModel>> GetAllAsync();
        Task<VaiTroViewModel> GetByIdAsync(int maVaiTro);
        Task<(int KetQua, string ThongBao)> CreateAsync(VaiTroViewModel model);
        Task<(int KetQua, string ThongBao)> UpdateAsync(VaiTroViewModel model);
        Task<(int KetQua, string ThongBao)> DeleteAsync(int maVaiTro);
        Task<IEnumerable<QuyenHanViewModel>> GetAllQuyenHanAsync();
        Task<IEnumerable<QuyenHanViewModel>> GetQuyenHanByVaiTroAsync(int maVaiTro);
        Task<(int KetQua, string ThongBao)> CapNhatQuyenHanAsync(int maVaiTro, List<int> danhSachQuyen);
    }
}
