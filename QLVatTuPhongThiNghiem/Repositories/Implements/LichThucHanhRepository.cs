using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class LichThucHanhRepository : ILichThucHanhRepository
    {
        private readonly AppDbContext _context;

        public LichThucHanhRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(int KetQua, int MaLich)> DangKyLichAsync(LichThucHanhViewModel model)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var maLichParam = new SqlParameter("@MaLich", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_DangKyLichThucHanh @ThoiGianBD, @ThoiGianKT, @MaNguoiDung, @MaLich OUTPUT, @KetQua OUTPUT",
                new SqlParameter("@ThoiGianBD", model.ThoiGianBD),
                new SqlParameter("@ThoiGianKT", model.ThoiGianKT),
                new SqlParameter("@MaNguoiDung", model.MaNguoiDung),
                maLichParam,
                ketQuaParam
            );

            return ((int)ketQuaParam.Value, (int)maLichParam.Value);
        }

        public async Task<int> CapNhatTrangThaiAsync(int maLich, string trangThai)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_CapNhatTrangThaiLich @MaLich, @TrangThai, @KetQua OUTPUT",
                new SqlParameter("@MaLich", maLich),
                new SqlParameter("@TrangThai", trangThai),
                ketQuaParam
            );

            return (int)ketQuaParam.Value;
        }

        // PHƯƠNG THỨC MỚI: Lấy chi tiết lịch thực hành theo ID
        public async Task<LichThucHanhViewModel> GetByIdAsync(int maLich)
        {
            var query = @"
                SELECT l.MaLich, l.TrangThai, l.ThoiGianBD, l.ThoiGianKT, l.MaNguoiDung, 
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDung
                FROM LichThucHanh l 
                LEFT JOIN NguoiDung n ON l.MaNguoiDung = n.MaNguoiDung
                WHERE l.MaLich = @MaLich";

            return await _context.LichThucHanhViewModel
                .FromSqlRaw(query, new SqlParameter("@MaLich", maLich))
                .FirstOrDefaultAsync();
        }

        // PHƯƠNG THỨC MỚI: Cập nhật thông tin lịch thực hành
        public async Task<(int KetQua, string ThongBao)> CapNhatLichAsync(LichThucHanhViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Kiểm tra lịch có tồn tại không
                var existing = await _context.LichThucHanh.FindAsync(model.MaLich);
                if (existing == null)
                {
                    return (0, "Không tìm thấy lịch thực hành");
                }

                // Kiểm tra thời gian hợp lệ
                if (model.ThoiGianBD >= model.ThoiGianKT)
                {
                    return (0, "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");
                }

                // Kiểm tra trùng lịch (loại trừ chính nó)
                var isConflict = await KiemTraTrungLichAsync(model.ThoiGianBD, model.ThoiGianKT, model.MaLich);
                if (isConflict)
                {
                    return (0, "Thời gian trùng với lịch thực hành khác");
                }

                // Cập nhật thông tin
                existing.ThoiGianBD = model.ThoiGianBD;
                existing.ThoiGianKT = model.ThoiGianKT;
                existing.TrangThai = model.TrangThai ?? existing.TrangThai;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Cập nhật lịch thực hành thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Xóa lịch thực hành 
        public async Task<(int KetQua, string ThongBao)> XoaLichAsync(int maLich, int maNguoiDung)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_LichThucHanh_Delete @MaLich, @MaNguoiDung, @KetQua OUTPUT",
                new SqlParameter("@MaLich", maLich),
                new SqlParameter("@MaNguoiDung", maNguoiDung),
                ketQuaParam
            );

            var ketQua = (int)ketQuaParam.Value;
            var thongBao = ketQua switch
            {
                1 => "Xóa lịch thực hành thành công",
                0 => "Không thể xóa lịch thực hành (không tìm thấy hoặc không có quyền)",
                -1 => "Lỗi hệ thống",
                _ => "Lỗi không xác định"
            };

            return (ketQua, thongBao);
        }

        public async Task<IEnumerable<LichThucHanhViewModel>> GetAllAsync()
        {
            var query = @"
                SELECT l.MaLich, l.TrangThai, l.ThoiGianBD, l.ThoiGianKT, l.MaNguoiDung, 
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDung
                FROM LichThucHanh l 
                LEFT JOIN NguoiDung n ON l.MaNguoiDung = n.MaNguoiDung
                ORDER BY l.ThoiGianBD DESC";

            return await _context.LichThucHanhViewModel.FromSqlRaw(query).ToListAsync();
        }

        public async Task<IEnumerable<LichThucHanhViewModel>> GetByUserAsync(int maNguoiDung)
        {
            var query = @"
                SELECT l.MaLich, l.TrangThai, l.ThoiGianBD, l.ThoiGianKT, l.MaNguoiDung, 
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDung
                FROM LichThucHanh l 
                LEFT JOIN NguoiDung n ON l.MaNguoiDung = n.MaNguoiDung
                WHERE l.MaNguoiDung = @MaNguoiDung
                ORDER BY l.ThoiGianBD DESC";

            return await _context.LichThucHanhViewModel
                .FromSqlRaw(query, new SqlParameter("@MaNguoiDung", maNguoiDung))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy lịch theo ngày
        public async Task<IEnumerable<LichThucHanhViewModel>> GetByNgayAsync(DateTime ngay)
        {
            var query = @"
                SELECT l.MaLich, l.TrangThai, l.ThoiGianBD, l.ThoiGianKT, l.MaNguoiDung, 
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDung
                FROM LichThucHanh l 
                LEFT JOIN NguoiDung n ON l.MaNguoiDung = n.MaNguoiDung
                WHERE CAST(l.ThoiGianBD as DATE) = @Ngay
                ORDER BY l.ThoiGianBD";

            return await _context.LichThucHanhViewModel
                .FromSqlRaw(query, new SqlParameter("@Ngay", ngay.Date))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy lịch theo trạng thái
        public async Task<IEnumerable<LichThucHanhViewModel>> GetByTrangThaiAsync(string trangThai)
        {
            var query = @"
                SELECT l.MaLich, l.TrangThai, l.ThoiGianBD, l.ThoiGianKT, l.MaNguoiDung, 
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDung
                FROM LichThucHanh l 
                LEFT JOIN NguoiDung n ON l.MaNguoiDung = n.MaNguoiDung
                WHERE l.TrangThai = @TrangThai
                ORDER BY l.ThoiGianBD DESC";

            return await _context.LichThucHanhViewModel
                .FromSqlRaw(query, new SqlParameter("@TrangThai", trangThai))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Kiểm tra trùng lịch
        public async Task<bool> KiemTraTrungLichAsync(DateTime thoiGianBD, DateTime thoiGianKT, int? maLichLoaiTru = null)
        {
            var query = _context.LichThucHanh
                .Where(l => l.TrangThai != "Đã hủy" && l.TrangThai != "Hoàn thành")
                .Where(l => (thoiGianBD < l.ThoiGianKT && thoiGianKT > l.ThoiGianBD));

            if (maLichLoaiTru.HasValue)
            {
                query = query.Where(l => l.MaLich != maLichLoaiTru.Value);
            }

            return await query.AnyAsync();
        }
    }
}