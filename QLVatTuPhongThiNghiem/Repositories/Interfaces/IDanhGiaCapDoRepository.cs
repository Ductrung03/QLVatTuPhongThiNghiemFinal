using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface IDanhGiaCapDoRepository
    {
        // Các phương thức đã có
        Task<int> DanhGiaCapDoAsync(DanhGiaCapDoViewModel model);
        Task<DanhGiaCapDoViewModel> GetCapDoThietBiAsync(int maTTB);
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetAllAsync();

        // Các phương thức bổ sung
        Task<DanhGiaCapDoViewModel> GetByIdAsync(int maDanhGia);
        Task<(int KetQua, string ThongBao)> CapNhatDanhGiaAsync(DanhGiaCapDoViewModel model);
        Task<(int KetQua, string ThongBao)> XoaDanhGiaAsync(int maDanhGia);
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetByThietBiAsync(int maTTB);
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetByNguoiDanhGiaAsync(int nguoiDanhGia);
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetByCapDoAsync(int capDo);
        Task<IEnumerable<DanhGiaCapDoViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay);
        Task<Dictionary<string, object>> ThongKeDanhGiaAsync(DateTime? tuNgay = null, DateTime? denNgay = null);
    }
}