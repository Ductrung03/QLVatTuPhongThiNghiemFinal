using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class NguoiDungService : INguoiDungService
    {
        private readonly INguoiDungRepository _nguoiDungRepository;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public NguoiDungService(INguoiDungRepository nguoiDungRepository, ILichSuHoatDongService lichSuHoatDongService)
        {
            _nguoiDungRepository = nguoiDungRepository;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        public async Task<(bool Success, int MaNguoiDung, string Message)> DangNhapBaoMatAsync(LoginViewModel model, string diaChiIP, string userAgent)
        {
            try
            {
                var (ketQua, maNguoiDung, thongBao) = await _nguoiDungRepository.DangNhapBaoMatAsync(model.TenDangNhap, model.MatKhau, diaChiIP, userAgent);

                if (ketQua == 1)
                {
                    return (true, maNguoiDung, thongBao);
                }
                else
                {
                    return (false, 0, thongBao);
                }
            }
            catch (Exception ex)
            {
                return (false, 0, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DoiMatKhauAsync(int maNguoiDung, DoiMatKhauViewModel model)
        {
            try
            {
                var (ketQua, thongBao) = await _nguoiDungRepository.DoiMatKhauAsync(maNguoiDung, model.MatKhauCu, model.MatKhauMoi);

                if (ketQua == 1)
                {
                    await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung, "Đổi mật khẩu", "Bảo mật", "Đổi mật khẩu thành công");
                    return (true, thongBao);
                }
                else
                {
                    return (false, thongBao);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<(bool Success, int MaNguoiDung, string Message)> TaoTaiKhoanAsync(TaoTaiKhoanViewModel model, int maNguoiTao)
        {
            try
            {
                var (ketQua, maNguoiDung, thongBao) = await _nguoiDungRepository.TaoTaiKhoanAsync(model, maNguoiTao);

                if (ketQua == 1)
                {
                    await _lichSuHoatDongService.GhiLichSuAsync(maNguoiTao, "Tạo tài khoản", "Quản lý người dùng",
                        $"Tạo tài khoản: {model.TenDangNhap} ({model.HoTen})");
                    return (true, maNguoiDung, thongBao);
                }
                else
                {
                    return (false, 0, thongBao);
                }
            }
            catch (Exception ex)
            {
                return (false, 0, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<IEnumerable<NguoiDungViewModel>> GetAllAsync()
        {
            return await _nguoiDungRepository.GetAllAsync();
        }

        public async Task<NguoiDungViewModel> GetByIdAsync(int maNguoiDung)
        {
            return await _nguoiDungRepository.GetByIdAsync(maNguoiDung);
        }

        public async Task<(bool Success, string Message)> CapNhatTrangThaiAsync(int maNguoiDung, bool trangThai)
        {
            try
            {
                var (ketQua, thongBao) = await _nguoiDungRepository.CapNhatTrangThaiAsync(maNguoiDung, trangThai);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> CapNhatThongTinAsync(NguoiDungViewModel model)
        {
            try
            {
                var (ketQua, thongBao) = await _nguoiDungRepository.CapNhatThongTinAsync(model);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<IEnumerable<string>> GetQuyenHanAsync(int maNguoiDung)
        {
            return await _nguoiDungRepository.GetQuyenHanAsync(maNguoiDung);
        }

        public async Task<(bool Success, string Message)> PhanQuyenAsync(int maNguoiDung, List<int> danhSachVaiTro)
        {
            try
            {
                var (ketQua, thongBao) = await _nguoiDungRepository.PhanQuyenAsync(maNguoiDung, danhSachVaiTro);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<bool> KiemTraQuyenAsync(int maNguoiDung, string tenQuyen)
        {
            var quyenHan = await GetQuyenHanAsync(maNguoiDung);
            return quyenHan.Contains(tenQuyen);
        }
    }
}

