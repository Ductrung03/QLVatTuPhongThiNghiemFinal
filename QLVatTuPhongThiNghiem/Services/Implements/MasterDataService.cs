using QLVatTuPhongThiNghiem.Models.Entities;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class MasterDataService : IMasterDataService
    {
        private readonly IMasterDataRepository _masterDataRepository;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public MasterDataService(IMasterDataRepository masterDataRepository, ILichSuHoatDongService lichSuHoatDongService)
        {
            _masterDataRepository = masterDataRepository;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        #region Loai Business Logic
        public async Task<IEnumerable<Loai>> GetLoaiAsync()
        {
            return await _masterDataRepository.GetAllLoaiAsync();
        }

        public async Task<Loai> GetLoaiByIdAsync(int maLoai)
        {
            return await _masterDataRepository.GetLoaiByIdAsync(maLoai);
        }

        public async Task<(bool Success, string Message)> CreateLoaiAsync(Loai loai)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(loai.TenLoai))
            {
                return (false, "Tên loại thiết bị không được để trống");
            }

            if (loai.TenLoai.Length > 100)
            {
                return (false, "Tên loại thiết bị không được quá 100 ký tự");
            }

            var (ketQua, thongBao) = await _masterDataRepository.CreateLoaiAsync(loai);
            return (ketQua == 1, thongBao);
        }

        public async Task<(bool Success, string Message)> UpdateLoaiAsync(Loai loai)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(loai.TenLoai))
            {
                return (false, "Tên loại thiết bị không được để trống");
            }

            if (loai.TenLoai.Length > 100)
            {
                return (false, "Tên loại thiết bị không được quá 100 ký tự");
            }

            var (ketQua, thongBao) = await _masterDataRepository.UpdateLoaiAsync(loai);
            return (ketQua == 1, thongBao);
        }

        public async Task<(bool Success, string Message)> DeleteLoaiAsync(int maLoai)
        {
            if (!await CanDeleteLoaiAsync(maLoai))
            {
                return (false, "Không thể xóa loại thiết bị đang được sử dụng");
            }

            var (ketQua, thongBao) = await _masterDataRepository.DeleteLoaiAsync(maLoai);
            return (ketQua == 1, thongBao);
        }

        public async Task<bool> CanDeleteLoaiAsync(int maLoai)
        {
            // Business rule: Không được xóa nếu có thiết bị đang sử dụng
            var loai = await _masterDataRepository.GetLoaiByIdAsync(maLoai);
            return loai != null; // Implement thêm logic kiểm tra ràng buộc
        }
        #endregion

        #region ThuongHieu Business Logic
        public async Task<IEnumerable<ThuongHieu>> GetThuongHieuAsync()
        {
            return await _masterDataRepository.GetAllThuongHieuAsync();
        }

        public async Task<ThuongHieu> GetThuongHieuByIdAsync(int maThuongHieu)
        {
            return await _masterDataRepository.GetThuongHieuByIdAsync(maThuongHieu);
        }

        public async Task<(bool Success, string Message)> CreateThuongHieuAsync(ThuongHieu thuongHieu)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(thuongHieu.TenThuongHieu))
            {
                return (false, "Tên thương hiệu không được để trống");
            }

            if (thuongHieu.TenThuongHieu.Length > 50)
            {
                return (false, "Tên thương hiệu không được quá 50 ký tự");
            }

            var (ketQua, thongBao) = await _masterDataRepository.CreateThuongHieuAsync(thuongHieu);
            return (ketQua == 1, thongBao);
        }

        public async Task<(bool Success, string Message)> UpdateThuongHieuAsync(ThuongHieu thuongHieu)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(thuongHieu.TenThuongHieu))
            {
                return (false, "Tên thương hiệu không được để trống");
            }

            if (thuongHieu.TenThuongHieu.Length > 50)
            {
                return (false, "Tên thương hiệu không được quá 50 ký tự");
            }

            var (ketQua, thongBao) = await _masterDataRepository.UpdateThuongHieuAsync(thuongHieu);
            return (ketQua == 1, thongBao);
        }

        public async Task<(bool Success, string Message)> DeleteThuongHieuAsync(int maThuongHieu)
        {
            if (!await CanDeleteThuongHieuAsync(maThuongHieu))
            {
                return (false, "Không thể xóa thương hiệu đang được sử dụng");
            }

            var (ketQua, thongBao) = await _masterDataRepository.DeleteThuongHieuAsync(maThuongHieu);
            return (ketQua == 1, thongBao);
        }

        public async Task<bool> CanDeleteThuongHieuAsync(int maThuongHieu)
        {
            var thuongHieu = await _masterDataRepository.GetThuongHieuByIdAsync(maThuongHieu);
            return thuongHieu != null; // Implement thêm logic kiểm tra ràng buộc
        }
        #endregion

        #region PhongMay Business Logic
        public async Task<IEnumerable<PhongMay>> GetPhongMayAsync()
        {
            return await _masterDataRepository.GetAllPhongMayAsync();
        }

        public async Task<PhongMay> GetPhongMayByIdAsync(int maPhongMay)
        {
            return await _masterDataRepository.GetPhongMayByIdAsync(maPhongMay);
        }

        public async Task<(bool Success, string Message)> CreatePhongMayAsync(PhongMay phongMay)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(phongMay.TenPhongMay))
            {
                return (false, "Tên phòng máy không được để trống");
            }

            if (phongMay.TenPhongMay.Length > 50)
            {
                return (false, "Tên phòng máy không được quá 50 ký tự");
            }

            var (ketQua, thongBao) = await _masterDataRepository.CreatePhongMayAsync(phongMay);
            return (ketQua == 1, thongBao);
        }

        public async Task<(bool Success, string Message)> UpdatePhongMayAsync(PhongMay phongMay)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(phongMay.TenPhongMay))
            {
                return (false, "Tên phòng máy không được để trống");
            }

            if (phongMay.TenPhongMay.Length > 50)
            {
                return (false, "Tên phòng máy không được quá 50 ký tự");
            }

            var (ketQua, thongBao) = await _masterDataRepository.UpdatePhongMayAsync(phongMay);
            return (ketQua == 1, thongBao);
        }

        public async Task<(bool Success, string Message)> DeletePhongMayAsync(int maPhongMay)
        {
            if (!await CanDeletePhongMayAsync(maPhongMay))
            {
                return (false, "Không thể xóa phòng máy đang được sử dụng");
            }

            var (ketQua, thongBao) = await _masterDataRepository.DeletePhongMayAsync(maPhongMay);
            return (ketQua == 1, thongBao);
        }

        public async Task<bool> CanDeletePhongMayAsync(int maPhongMay)
        {
            var phongMay = await _masterDataRepository.GetPhongMayByIdAsync(maPhongMay);
            return phongMay != null; // Implement thêm logic kiểm tra ràng buộc
        }
        #endregion

        #region NhanVien Business Logic
        public async Task<IEnumerable<NhanVien>> GetNhanVienAsync()
        {
            return await _masterDataRepository.GetAllNhanVienAsync();
        }

        public async Task<NhanVien> GetNhanVienByIdAsync(int maNV)
        {
            return await _masterDataRepository.GetNhanVienByIdAsync(maNV);
        }

        public async Task<(bool Success, string Message)> CreateNhanVienAsync(NhanVien nhanVien)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(nhanVien.HoTen))
            {
                return (false, "Họ tên nhân viên không được để trống");
            }

            if (nhanVien.HoTen.Length > 100)
            {
                return (false, "Họ tên không được quá 100 ký tự");
            }

            // Validate email format
            if (!string.IsNullOrEmpty(nhanVien.Email) && !IsValidEmail(nhanVien.Email))
            {
                return (false, "Email không đúng định dạng");
            }

            // Validate phone number
            if (!string.IsNullOrEmpty(nhanVien.SoDienThoai) && !IsValidPhoneNumber(nhanVien.SoDienThoai))
            {
                return (false, "Số điện thoại không đúng định dạng");
            }

            var (ketQua, thongBao) = await _masterDataRepository.CreateNhanVienAsync(nhanVien);
            return (ketQua == 1, thongBao);
        }

        public async Task<(bool Success, string Message)> UpdateNhanVienAsync(NhanVien nhanVien)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(nhanVien.HoTen))
            {
                return (false, "Họ tên nhân viên không được để trống");
            }

            if (nhanVien.HoTen.Length > 100)
            {
                return (false, "Họ tên không được quá 100 ký tự");
            }

            // Validate email format
            if (!string.IsNullOrEmpty(nhanVien.Email) && !IsValidEmail(nhanVien.Email))
            {
                return (false, "Email không đúng định dạng");
            }

            // Validate phone number
            if (!string.IsNullOrEmpty(nhanVien.SoDienThoai) && !IsValidPhoneNumber(nhanVien.SoDienThoai))
            {
                return (false, "Số điện thoại không đúng định dạng");
            }

            var (ketQua, thongBao) = await _masterDataRepository.UpdateNhanVienAsync(nhanVien);
            return (ketQua == 1, thongBao);
        }

        public async Task<(bool Success, string Message)> DeleteNhanVienAsync(int maNV)
        {
            if (!await CanDeleteNhanVienAsync(maNV))
            {
                return (false, "Không thể xóa nhân viên đang có lịch trực");
            }

            var (ketQua, thongBao) = await _masterDataRepository.DeleteNhanVienAsync(maNV);
            return (ketQua == 1, thongBao);
        }

        public async Task<bool> CanDeleteNhanVienAsync(int maNV)
        {
            var nhanVien = await _masterDataRepository.GetNhanVienByIdAsync(maNV);
            return nhanVien != null; // Implement thêm logic kiểm tra ràng buộc
        }
        #endregion

        #region Helper Methods
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Basic Vietnamese phone number validation
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^(\+84|0)[0-9]{9,10}$");
        }
        #endregion
    }
}