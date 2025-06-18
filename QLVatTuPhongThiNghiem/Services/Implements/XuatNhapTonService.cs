// File: Services/Implements/XuatNhapTonService.cs (Cập nhật)
// Vị trí: QLVatTuPhongThiNghiem/Services/Implements/XuatNhapTonService.cs
// Thay thế file cũ

using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class XuatNhapTonService : IXuatNhapTonService
    {
        private readonly IXuatNhapTonRepository _xuatNhapTonRepository;
        private readonly ILichSuHoatDongRepository _lichSuHoatDongRepository;

        public XuatNhapTonService(
            IXuatNhapTonRepository xuatNhapTonRepository,
            ILichSuHoatDongRepository lichSuHoatDongRepository)
        {
            _xuatNhapTonRepository = xuatNhapTonRepository;
            _lichSuHoatDongRepository = lichSuHoatDongRepository;
        }

        public async Task<(bool Success, string Message)> XuatThietBiAsync(XuatNhapTonViewModel model)
        {
            try
            {
                // Kiểm tra tồn kho trước khi xuất
                var tonKho = await _xuatNhapTonRepository.GetTonKhoThietBiAsync(model.MaTTB);
                if (tonKho < model.SoLuong)
                {
                    return (false, $"Không đủ tồn kho. Hiện tại: {tonKho}, yêu cầu xuất: {model.SoLuong}");
                }

                var ketQua = await _xuatNhapTonRepository.XuatThietBiAsync(model);

                switch (ketQua)
                {
                    case 1:
                        // Ghi lịch sử hoạt động
                        await _lichSuHoatDongRepository.GhiLichSuAsync(
                            model.NguoiTao,
                            "Xuất thiết bị",
                            "Xuất nhập tồn",
                            $"Xuất {model.SoLuong} thiết bị ID: {model.MaTTB}. Ghi chú: {model.GhiChu}");

                        return (true, "Xuất thiết bị thành công");
                    case 0:
                        return (false, "Số lượng không hợp lệ hoặc thiết bị không tồn tại");
                    case -1:
                        return (false, "Lỗi hệ thống");
                    default:
                        return (false, "Lỗi không xác định");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> NhapThietBiAsync(XuatNhapTonViewModel model)
        {
            try
            {
                var ketQua = await _xuatNhapTonRepository.NhapThietBiAsync(model);

                switch (ketQua)
                {
                    case 1:
                        // Ghi lịch sử hoạt động
                        await _lichSuHoatDongRepository.GhiLichSuAsync(
                            model.NguoiTao,
                            "Nhập thiết bị",
                            "Xuất nhập tồn",
                            $"Nhập {model.SoLuong} thiết bị ID: {model.MaTTB}. Ghi chú: {model.GhiChu}");

                        return (true, "Nhập thiết bị thành công");
                    case 0:
                        return (false, "Số lượng không hợp lệ");
                    case -1:
                        return (false, "Lỗi hệ thống");
                    default:
                        return (false, "Lỗi không xác định");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<XuatNhapTonViewModel> GetByIdAsync(int maPhieu)
        {
            try
            {
                return await _xuatNhapTonRepository.GetByIdAsync(maPhieu);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(bool Success, string Message)> CapNhatPhieuAsync(XuatNhapTonViewModel model, int nguoiCapNhat)
        {
            try
            {
                var (ketQua, thongBao) = await _xuatNhapTonRepository.CapNhatPhieuAsync(model);

                if (ketQua == 1)
                {
                    // Ghi lịch sử hoạt động
                    await _lichSuHoatDongRepository.GhiLichSuAsync(
                        nguoiCapNhat,
                        "Cập nhật phiếu",
                        "Xuất nhập tồn",
                        $"Cập nhật phiếu {model.LoaiPhieu} ID: {model.MaPhieu}");

                    return (true, thongBao);
                }

                return (false, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> XoaPhieuAsync(int maPhieu, int nguoiXoa)
        {
            try
            {
                var phieu = await _xuatNhapTonRepository.GetByIdAsync(maPhieu);
                if (phieu == null)
                {
                    return (false, "Không tìm thấy phiếu");
                }

                var (ketQua, thongBao) = await _xuatNhapTonRepository.XoaPhieuAsync(maPhieu);

                if (ketQua == 1)
                {
                    // Ghi lịch sử hoạt động
                    await _lichSuHoatDongRepository.GhiLichSuAsync(
                        nguoiXoa,
                        "Xóa phiếu",
                        "Xuất nhập tồn",
                        $"Xóa phiếu {phieu.LoaiPhieu} ID: {maPhieu}");

                    return (true, thongBao);
                }

                return (false, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<dynamic>> BaoCaoTonKhoAsync(int? maPhongMay = null)
        {
            return await _xuatNhapTonRepository.BaoCaoTonKhoAsync(maPhongMay);
        }

        public async Task<IEnumerable<XuatNhapTonViewModel>> GetAllAsync()
        {
            return await _xuatNhapTonRepository.GetAllAsync();
        }

        public async Task<IEnumerable<XuatNhapTonViewModel>> GetByThietBiAsync(int maTTB)
        {
            return await _xuatNhapTonRepository.GetByThietBiAsync(maTTB);
        }

        public async Task<IEnumerable<XuatNhapTonViewModel>> GetByNguoiTaoAsync(int nguoiTao)
        {
            return await _xuatNhapTonRepository.GetByNguoiTaoAsync(nguoiTao);
        }

        public async Task<IEnumerable<XuatNhapTonViewModel>> GetByLoaiPhieuAsync(string loaiPhieu)
        {
            if (string.IsNullOrEmpty(loaiPhieu) || !new[] { "NHAP", "XUAT" }.Contains(loaiPhieu.ToUpper()))
            {
                return new List<XuatNhapTonViewModel>();
            }

            return await _xuatNhapTonRepository.GetByLoaiPhieuAsync(loaiPhieu.ToUpper());
        }

        public async Task<IEnumerable<XuatNhapTonViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay > denNgay)
            {
                return new List<XuatNhapTonViewModel>();
            }

            return await _xuatNhapTonRepository.GetByDateRangeAsync(tuNgay, denNgay);
        }

        public async Task<Dictionary<string, object>> ThongKeXuatNhapAsync(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            return await _xuatNhapTonRepository.ThongKeXuatNhapAsync(tuNgay, denNgay);
        }

        public async Task<int> GetTonKhoThietBiAsync(int maTTB)
        {
            return await _xuatNhapTonRepository.GetTonKhoThietBiAsync(maTTB);
        }

        public async Task<bool> KiemTraQuyenChinhSuaAsync(int maPhieu, int maNguoiDung, bool isAdmin = false)
        {
            try
            {
                var phieu = await _xuatNhapTonRepository.GetByIdAsync(maPhieu);
                if (phieu == null) return false;

                // Admin có thể sửa tất cả
                if (isAdmin) return true;

                // Người tạo có thể sửa trong ngày
                if (phieu.NguoiTao == maNguoiDung && phieu.NgayTao.Date == DateTime.Today)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<string> GetDanhSachLoaiPhieu()
        {
            return new List<string> { "NHAP", "XUAT" };
        }

        public async Task<Dictionary<string, object>> GetThongTinTonKhoChiTietAsync(int maTTB)
        {
            try
            {
                var lichSuPhieu = await _xuatNhapTonRepository.GetByThietBiAsync(maTTB);
                var tonKhoHienTai = await _xuatNhapTonRepository.GetTonKhoThietBiAsync(maTTB);

                var thongKe = new Dictionary<string, object>
                {
                    ["TonKhoHienTai"] = tonKhoHienTai,
                    ["TongSoPhieu"] = lichSuPhieu.Count(),
                    ["SoPhieuNhap"] = lichSuPhieu.Count(p => p.LoaiPhieu == "NHAP"),
                    ["SoPhieuXuat"] = lichSuPhieu.Count(p => p.LoaiPhieu == "XUAT"),
                    ["TongSoLuongNhap"] = lichSuPhieu.Where(p => p.LoaiPhieu == "NHAP").Sum(p => p.SoLuong),
                    ["TongSoLuongXuat"] = lichSuPhieu.Where(p => p.LoaiPhieu == "XUAT").Sum(p => p.SoLuong),
                    ["PhieuGanNhat"] = lichSuPhieu.OrderByDescending(p => p.NgayTao).FirstOrDefault()
                };

                return thongKe;
            }
            catch
            {
                return new Dictionary<string, object>();
            }
        }

        public async Task<(bool CanDelete, string Message)> KiemTraCoTheXoaAsync(int maPhieu)
        {
            try
            {
                var phieu = await _xuatNhapTonRepository.GetByIdAsync(maPhieu);
                if (phieu == null)
                {
                    return (false, "Không tìm thấy phiếu");
                }

                // Chỉ cho phép xóa phiếu trong ngày tạo
                if (phieu.NgayTao.Date != DateTime.Today)
                {
                    return (false, "Chỉ có thể xóa phiếu trong ngày tạo");
                }

                // Kiểm tra ảnh hưởng đến tồn kho
                if (phieu.LoaiPhieu == "NHAP")
                {
                    var tonKhoHienTai = await _xuatNhapTonRepository.GetTonKhoThietBiAsync(phieu.MaTTB);
                    if (tonKhoHienTai < phieu.SoLuong)
                    {
                        return (false, $"Không thể xóa phiếu nhập vì sẽ làm tồn kho âm. Tồn kho hiện tại: {tonKhoHienTai}");
                    }
                }

                return (true, "Có thể xóa phiếu");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }
    }
}