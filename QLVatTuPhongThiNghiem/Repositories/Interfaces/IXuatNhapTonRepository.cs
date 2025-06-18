using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface IXuatNhapTonRepository
    {
        // Các phương thức đã có
        Task<int> XuatThietBiAsync(XuatNhapTonViewModel model);
        Task<int> NhapThietBiAsync(XuatNhapTonViewModel model);
        Task<IEnumerable<dynamic>> BaoCaoTonKhoAsync(int? maPhongMay = null);
        Task<IEnumerable<XuatNhapTonViewModel>> GetAllAsync();

        // Các phương thức bổ sung
        Task<XuatNhapTonViewModel> GetByIdAsync(int maPhieu);
        Task<(int KetQua, string ThongBao)> CapNhatPhieuAsync(XuatNhapTonViewModel model);
        Task<(int KetQua, string ThongBao)> XoaPhieuAsync(int maPhieu);
        Task<IEnumerable<XuatNhapTonViewModel>> GetByThietBiAsync(int maTTB);
        Task<IEnumerable<XuatNhapTonViewModel>> GetByNguoiTaoAsync(int nguoiTao);
        Task<IEnumerable<XuatNhapTonViewModel>> GetByLoaiPhieuAsync(string loaiPhieu);
        Task<IEnumerable<XuatNhapTonViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay);
        Task<Dictionary<string, object>> ThongKeXuatNhapAsync(DateTime? tuNgay = null, DateTime? denNgay = null);
        Task<int> GetTonKhoThietBiAsync(int maTTB);
    }
}