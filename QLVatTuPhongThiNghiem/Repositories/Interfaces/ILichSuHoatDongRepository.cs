using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface ILichSuHoatDongRepository
    {
        Task GhiLichSuAsync(int maNguoiDung, string hanhDong, string module, string chiTiet, string diaChiIP = null, string userAgent = null);
        Task<IEnumerable<LichSuHoatDongViewModel>> GetLichSuAsync(int? maNguoiDung = null, DateTime? tuNgay = null, DateTime? denNgay = null, int pageNumber = 1, int pageSize = 50);
        Task<int> GetTongSoBanGhiAsync(int? maNguoiDung = null, DateTime? tuNgay = null, DateTime? denNgay = null);
    }
}
