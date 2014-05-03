using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Repositories.Interfaces
{
    public interface INguoiDungRepository
    {
        Task<(int KetQua, int MaNguoiDung, string ThongBao)> DangNhapBaoMatAsync(string tenDangNhap, string matKhau, string diaChiIP, string userAgent);
        Task<(int KetQua, string ThongBao)> DoiMatKhauAsync(int maNguoiDung, string matKhauCu, string matKhauMoi);
        Task<(int KetQua, int MaNguoiDung, string ThongBao)> TaoTaiKhoanAsync(TaoTaiKhoanViewModel model, int maNguoiTao);
        Task<IEnumerable<NguoiDungViewModel>> GetAllAsync();
        Task<NguoiDungViewModel> GetByIdAsync(int maNguoiDung);
        Task<(int KetQua, string ThongBao)> CapNhatTrangThaiAsync(int maNguoiDung, bool trangThai);
        Task<(int KetQua, string ThongBao)> CapNhatThongTinAsync(NguoiDungViewModel model);
        Task<IEnumerable<string>> GetQuyenHanAsync(int maNguoiDung);
        Task<(int KetQua, string ThongBao)> PhanQuyenAsync(int maNguoiDung, List<int> danhSachVaiTro);
    }
}
