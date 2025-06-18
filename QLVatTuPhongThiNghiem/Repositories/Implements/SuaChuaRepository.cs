using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class SuaChuaRepository : ISuaChuaRepository
    {
        private readonly AppDbContext _context;

        public SuaChuaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(int KetQua, int MaSuaChua)> TaoPhieuSuaChuaAsync(SuaChuaViewModel model)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var maSuaChuaParam = new SqlParameter("@MaSuaChua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_TaoPhieuSuaChua @MaTTB, @LoaiSuaChua, @MoTa, @NguoiThucHien, @MaSuaChua OUTPUT, @KetQua OUTPUT",
                new SqlParameter("@MaTTB", model.MaTTB),
                new SqlParameter("@LoaiSuaChua", model.LoaiSuaChua),
                new SqlParameter("@MoTa", model.MoTa),
                new SqlParameter("@NguoiThucHien", model.NguoiThucHien),
                maSuaChuaParam,
                ketQuaParam
            );

            return ((int)ketQuaParam.Value, (int)maSuaChuaParam.Value);
        }

        public async Task<int> HoanThanhSuaChuaAsync(int maSuaChua, decimal chiPhi, string tinhTrangMoi)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_HoanThanhSuaChua @MaSuaChua, @ChiPhi, @TinhTrangMoi, @KetQua OUTPUT",
                new SqlParameter("@MaSuaChua", maSuaChua),
                new SqlParameter("@ChiPhi", chiPhi),
                new SqlParameter("@TinhTrangMoi", tinhTrangMoi),
                ketQuaParam
            );

            return (int)ketQuaParam.Value;
        }

        // PHƯƠNG THỨC MỚI: Lấy chi tiết phiếu sửa chữa theo ID
        public async Task<SuaChuaViewModel> GetByIdAsync(int maSuaChua)
        {
            var query = @"
                SELECT s.MaSuaChua, s.MaTTB, s.NgayBatDau, s.NgayKetThuc, s.LoaiSuaChua, s.MoTa, 
                       s.ChiPhi, s.TrangThai, s.NguoiThucHien,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiThucHien
                FROM LichSuSuaChua s
                INNER JOIN TrangTB t ON s.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON s.NguoiThucHien = n.MaNguoiDung
                WHERE s.MaSuaChua = @MaSuaChua";

            return await _context.SuaChuaViewModel
                .FromSqlRaw(query, new SqlParameter("@MaSuaChua", maSuaChua))
                .FirstOrDefaultAsync();
        }

        // PHƯƠNG THỨC MỚI: Cập nhật phiếu sửa chữa
        public async Task<(int KetQua, string ThongBao)> CapNhatPhieuSuaChuaAsync(SuaChuaViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.LichSuSuaChua.FindAsync(model.MaSuaChua);
                if (existing == null)
                {
                    return (0, "Không tìm thấy phiếu sửa chữa");
                }

                // Chỉ cho phép sửa nếu chưa hoàn thành
                if (existing.TrangThai == "Hoàn thành")
                {
                    return (0, "Không thể sửa phiếu đã hoàn thành");
                }

                // Cập nhật thông tin
                existing.LoaiSuaChua = model.LoaiSuaChua;
                existing.MoTa = model.MoTa;
                existing.ChiPhi = model.ChiPhi;

                // Nếu có tình trạng mới, cập nhật cả trạng thái thiết bị
                if (!string.IsNullOrEmpty(model.TinhTrangMoi))
                {
                    var thietBi = await _context.TrangTB.FindAsync(existing.MaTTB);
                    if (thietBi != null)
                    {
                        thietBi.TinhTrang = model.TinhTrangMoi;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Cập nhật phiếu sửa chữa thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Xóa phiếu sửa chữa
        public async Task<(int KetQua, string ThongBao)> XoaPhieuSuaChuaAsync(int maSuaChua)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.LichSuSuaChua.FindAsync(maSuaChua);
                if (existing == null)
                {
                    return (0, "Không tìm thấy phiếu sửa chữa");
                }

                // Chỉ cho phép xóa nếu chưa bắt đầu hoặc đã hủy
                if (existing.TrangThai == "Đang sửa chữa" || existing.TrangThai == "Hoàn thành")
                {
                    return (0, "Không thể xóa phiếu đang thực hiện hoặc đã hoàn thành");
                }

                // Soft delete
                existing.TrangThai = "Đã hủy";

                // Cập nhật lại tình trạng thiết bị nếu cần
                var thietBi = await _context.TrangTB.FindAsync(existing.MaTTB);
                if (thietBi != null && thietBi.TinhTrang == "Đang sửa chữa")
                {
                    thietBi.TinhTrang = "Hỏng"; // Trả về trạng thái trước đó
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Hủy phiếu sửa chữa thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Hủy phiếu sửa chữa với lý do
        public async Task<(int KetQua, string ThongBao)> HuyPhieuSuaChuaAsync(int maSuaChua, string lyDo)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.LichSuSuaChua.FindAsync(maSuaChua);
                if (existing == null)
                {
                    return (0, "Không tìm thấy phiếu sửa chữa");
                }

                if (existing.TrangThai == "Hoàn thành")
                {
                    return (0, "Không thể hủy phiếu đã hoàn thành");
                }

                // Cập nhật trạng thái và lý do hủy
                existing.TrangThai = "Đã hủy";
                existing.MoTa = existing.MoTa + $"\n[Lý do hủy: {lyDo}]";

                // Cập nhật lại tình trạng thiết bị
                var thietBi = await _context.TrangTB.FindAsync(existing.MaTTB);
                if (thietBi != null && thietBi.TinhTrang == "Đang sửa chữa")
                {
                    thietBi.TinhTrang = "Hỏng";
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Hủy phiếu sửa chữa thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<SuaChuaViewModel>> GetAllAsync()
        {
            var query = @"
                SELECT s.MaSuaChua, s.MaTTB, s.NgayBatDau, s.NgayKetThuc, s.LoaiSuaChua, s.MoTa, 
                       s.ChiPhi, s.TrangThai, s.NguoiThucHien, '' as TinhTrangMoi,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiThucHien
                FROM LichSuSuaChua s
                INNER JOIN TrangTB t ON s.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON s.NguoiThucHien = n.MaNguoiDung
                ORDER BY s.NgayBatDau DESC";

            return await _context.SuaChuaViewModel.FromSqlRaw(query).ToListAsync();
        }

        public async Task<IEnumerable<SuaChuaViewModel>> GetByTrangThaiAsync(string trangThai)
        {
            var query = @"
                SELECT s.MaSuaChua, s.MaTTB, s.NgayBatDau, s.NgayKetThuc, s.LoaiSuaChua, s.MoTa, 
                       s.ChiPhi, s.TrangThai, s.NguoiThucHien, '' as TinhTrangMoi,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiThucHien
                FROM LichSuSuaChua s
                INNER JOIN TrangTB t ON s.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON s.NguoiThucHien = n.MaNguoiDung
                WHERE s.TrangThai = @TrangThai
                ORDER BY s.NgayBatDau DESC";

            return await _context.SuaChuaViewModel
                .FromSqlRaw(query, new SqlParameter("@TrangThai", trangThai))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy phiếu sửa chữa theo thiết bị
        public async Task<IEnumerable<SuaChuaViewModel>> GetByThietBiAsync(int maTTB)
        {
            var query = @"
                SELECT s.MaSuaChua, s.MaTTB, s.NgayBatDau, s.NgayKetThuc, s.LoaiSuaChua, s.MoTa, 
                       s.ChiPhi, s.TrangThai, s.NguoiThucHien, '' as TinhTrangMoi,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiThucHien
                FROM LichSuSuaChua s
                INNER JOIN TrangTB t ON s.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON s.NguoiThucHien = n.MaNguoiDung
                WHERE s.MaTTB = @MaTTB
                ORDER BY s.NgayBatDau DESC";

            return await _context.SuaChuaViewModel
                .FromSqlRaw(query, new SqlParameter("@MaTTB", maTTB))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy phiếu sửa chữa theo người thực hiện
        public async Task<IEnumerable<SuaChuaViewModel>> GetByNguoiThucHienAsync(int nguoiThucHien)
        {
            var query = @"
                SELECT s.MaSuaChua, s.MaTTB, s.NgayBatDau, s.NgayKetThuc, s.LoaiSuaChua, s.MoTa, 
                       s.ChiPhi, s.TrangThai, s.NguoiThucHien, '' as TinhTrangMoi,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiThucHien
                FROM LichSuSuaChua s
                INNER JOIN TrangTB t ON s.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON s.NguoiThucHien = n.MaNguoiDung
                WHERE s.NguoiThucHien = @NguoiThucHien
                ORDER BY s.NgayBatDau DESC";

            return await _context.SuaChuaViewModel
                .FromSqlRaw(query, new SqlParameter("@NguoiThucHien", nguoiThucHien))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy phiếu sửa chữa theo khoảng thời gian
        public async Task<IEnumerable<SuaChuaViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay)
        {
            var query = @"
                SELECT s.MaSuaChua, s.MaTTB, s.NgayBatDau, s.NgayKetThuc, s.LoaiSuaChua, s.MoTa, 
                       s.ChiPhi, s.TrangThai, s.NguoiThucHien, '' as TinhTrangMoi,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiThucHien
                FROM LichSuSuaChua s
                INNER JOIN TrangTB t ON s.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON s.NguoiThucHien = n.MaNguoiDung
                WHERE s.NgayBatDau >= @TuNgay AND s.NgayBatDau <= @DenNgay
                ORDER BY s.NgayBatDau DESC";

            return await _context.SuaChuaViewModel
                .FromSqlRaw(query,
                    new SqlParameter("@TuNgay", tuNgay.Date),
                    new SqlParameter("@DenNgay", denNgay.Date.AddDays(1).AddTicks(-1)))
                .ToListAsync();
        }
    }
}