using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using System.Data;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class LichTrucRepository : ILichTrucRepository
    {
        private readonly AppDbContext _context;

        public LichTrucRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(int KetQua, string ThongBao)> TaoLichTrucAsync(LichTrucViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Kiểm tra trùng lịch cho cùng nhân viên trong cùng ngày
                var existingScheduleForEmployee = await _context.LichTruc
                    .Where(l => l.Ngay.Date == model.Ngay.Date &&
                               l.MaNV == model.MaNV &&
                               l.TrangThai != "Đã hủy")
                    .FirstOrDefaultAsync();

                if (existingScheduleForEmployee != null)
                {
                    return (0, "Nhân viên đã có lịch trực trong ngày này");
                }

                // Kiểm tra trùng lịch cho cùng phòng máy và ca làm
                var existingScheduleForRoom = await _context.LichTruc
                    .Where(l => l.Ngay.Date == model.Ngay.Date &&
                               l.MaPhongMay == model.MaPhongMay &&
                               l.CaLam == model.CaLam &&
                               l.TrangThai != "Đã hủy")
                    .FirstOrDefaultAsync();

                if (existingScheduleForRoom != null)
                {
                    return (0, "Đã có người trực phòng này trong ca này");
                }

                // Kiểm tra nhân viên có đang làm việc không
                var nhanVien = await _context.NhanVien.FindAsync(model.MaNV);
                if (nhanVien == null || !nhanVien.TrangThai)
                {
                    return (0, "Nhân viên không tồn tại hoặc không còn làm việc");
                }

                // Kiểm tra phòng máy có tồn tại không
                var phongMay = await _context.PhongMay.FindAsync(model.MaPhongMay);
                if (phongMay == null)
                {
                    return (0, "Phòng máy không tồn tại");
                }

                // Tạo mã lịch tự động
                var maxMaLich = await _context.LichTruc.MaxAsync(l => (int?)l.MaLich) ?? 0;
                var maLich = maxMaLich + 1;

                var lichTruc = new Models.Entities.LichTruc
                {
                    MaLich = maLich,
                    Ngay = model.Ngay.Date, // Chỉ lấy phần ngày
                    MaNV = model.MaNV,
                    MaPhongMay = model.MaPhongMay,
                    CaLam = model.CaLam,
                    GhiChu = model.GhiChu,
                    TrangThai = "Đã lên lịch"
                };

                _context.LichTruc.Add(lichTruc);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Tạo lịch trực thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> CapNhatLichTrucAsync(LichTrucViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var lichTruc = await _context.LichTruc.FindAsync(model.MaLich);
                if (lichTruc == null)
                {
                    return (0, "Không tìm thấy lịch trực");
                }

                // Kiểm tra trùng lịch (trừ chính nó)
                var existingSchedule = await _context.LichTruc
                    .Where(l => l.Ngay.Date == model.Ngay.Date &&
                               l.MaPhongMay == model.MaPhongMay &&
                               l.CaLam == model.CaLam &&
                               l.MaLich != model.MaLich &&
                               l.TrangThai != "Đã hủy")
                    .FirstOrDefaultAsync();

                if (existingSchedule != null)
                {
                    return (0, "Đã có người trực phòng này trong ca này");
                }

                // Kiểm tra nhân viên có trùng lịch không
                var existingScheduleForEmployee = await _context.LichTruc
                    .Where(l => l.Ngay.Date == model.Ngay.Date &&
                               l.MaNV == model.MaNV &&
                               l.MaLich != model.MaLich &&
                               l.TrangThai != "Đã hủy")
                    .FirstOrDefaultAsync();

                if (existingScheduleForEmployee != null)
                {
                    return (0, "Nhân viên đã có lịch trực khác trong ngày này");
                }

                // Cập nhật thông tin
                lichTruc.Ngay = model.Ngay.Date;
                lichTruc.MaNV = model.MaNV;
                lichTruc.MaPhongMay = model.MaPhongMay;
                lichTruc.CaLam = model.CaLam;
                lichTruc.GhiChu = model.GhiChu;

                // Chỉ cập nhật trạng thái nếu được cung cấp
                if (!string.IsNullOrEmpty(model.TrangThai))
                {
                    lichTruc.TrangThai = model.TrangThai;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Cập nhật lịch trực thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> XoaLichTrucAsync(int maLich)
        {
            try
            {
                var lichTruc = await _context.LichTruc.FindAsync(maLich);
                if (lichTruc == null)
                {
                    return (0, "Không tìm thấy lịch trực");
                }

                // Kiểm tra xem có thể hủy lịch không (chỉ hủy được nếu chưa bắt đầu)
                if (lichTruc.Ngay.Date < DateTime.Today)
                {
                    return (0, "Không thể hủy lịch trực đã qua");
                }

                if (lichTruc.TrangThai == "Hoàn thành")
                {
                    return (0, "Không thể hủy lịch trực đã hoàn thành");
                }

                // Soft delete - chỉ thay đổi trạng thái
                lichTruc.TrangThai = "Đã hủy";
                await _context.SaveChangesAsync();

                return (1, "Hủy lịch trực thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<LichTrucViewModel>> GetAllAsync()
        {
            var query = @"
                SELECT l.MaLich, l.Ngay, l.MaNV, l.MaPhongMay, l.CaLam, l.GhiChu, l.TrangThai,
                       nv.HoTen as TenNhanVien, p.TenPhongMay
                FROM LichTruc l
                INNER JOIN NhanVien nv ON l.MaNV = nv.MaNV
                INNER JOIN PhongMay p ON l.MaPhongMay = p.MaPhongMay
                WHERE l.TrangThai != N'Đã hủy'
                ORDER BY l.Ngay DESC, 
                         CASE l.CaLam 
                             WHEN N'Ca sáng' THEN 1
                             WHEN N'Ca chiều' THEN 2
                             WHEN N'Ca tối' THEN 3
                             ELSE 4
                         END";

            return await _context.LichTrucViewModel.FromSqlRaw(query).ToListAsync();
        }

        public async Task<IEnumerable<LichTrucViewModel>> GetByPhongMayAsync(int maPhongMay)
        {
            var query = @"
                SELECT l.MaLich, l.Ngay, l.MaNV, l.MaPhongMay, l.CaLam, l.GhiChu, l.TrangThai,
                       nv.HoTen as TenNhanVien, p.TenPhongMay
                FROM LichTruc l
                INNER JOIN NhanVien nv ON l.MaNV = nv.MaNV
                INNER JOIN PhongMay p ON l.MaPhongMay = p.MaPhongMay
                WHERE l.MaPhongMay = @MaPhongMay AND l.TrangThai != N'Đã hủy'
                ORDER BY l.Ngay DESC, 
                         CASE l.CaLam 
                             WHEN N'Ca sáng' THEN 1
                             WHEN N'Ca chiều' THEN 2
                             WHEN N'Ca tối' THEN 3
                             ELSE 4
                         END";

            return await _context.LichTrucViewModel.FromSqlRaw(query,
                new SqlParameter("@MaPhongMay", maPhongMay)
            ).ToListAsync();
        }

        public async Task<IEnumerable<LichTrucViewModel>> GetByNgayAsync(DateTime ngay)
        {
            var query = @"
                SELECT l.MaLich, l.Ngay, l.MaNV, l.MaPhongMay, l.CaLam, l.GhiChu, l.TrangThai,
                       nv.HoTen as TenNhanVien, p.TenPhongMay
                FROM LichTruc l
                INNER JOIN NhanVien nv ON l.MaNV = nv.MaNV
                INNER JOIN PhongMay p ON l.MaPhongMay = p.MaPhongMay
                WHERE CAST(l.Ngay as DATE) = @Ngay AND l.TrangThai != N'Đã hủy'
                ORDER BY CASE l.CaLam 
                             WHEN N'Ca sáng' THEN 1
                             WHEN N'Ca chiều' THEN 2
                             WHEN N'Ca tối' THEN 3
                             ELSE 4
                         END, p.TenPhongMay";

            return await _context.LichTrucViewModel.FromSqlRaw(query,
                new SqlParameter("@Ngay", ngay.Date)
            ).ToListAsync();
        }

        public async Task<LichTrucViewModel> GetByIdAsync(int maLich)
        {
            var query = @"
                SELECT l.MaLich, l.Ngay, l.MaNV, l.MaPhongMay, l.CaLam, l.GhiChu, l.TrangThai,
                       nv.HoTen as TenNhanVien, p.TenPhongMay
                FROM LichTruc l
                INNER JOIN NhanVien nv ON l.MaNV = nv.MaNV
                INNER JOIN PhongMay p ON l.MaPhongMay = p.MaPhongMay
                WHERE l.MaLich = @MaLich";

            return await _context.LichTrucViewModel.FromSqlRaw(query,
                new SqlParameter("@MaLich", maLich)
            ).FirstOrDefaultAsync();
        }

        // Lấy lịch trực trong khoảng thời gian
        public async Task<IEnumerable<LichTrucViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay)
        {
            var query = @"
                SELECT l.MaLich, l.Ngay, l.MaNV, l.MaPhongMay, l.CaLam, l.GhiChu, l.TrangThai,
                       nv.HoTen as TenNhanVien, p.TenPhongMay
                FROM LichTruc l
                INNER JOIN NhanVien nv ON l.MaNV = nv.MaNV
                INNER JOIN PhongMay p ON l.MaPhongMay = p.MaPhongMay
                WHERE l.Ngay >= @TuNgay AND l.Ngay <= @DenNgay AND l.TrangThai != N'Đã hủy'
                ORDER BY l.Ngay, 
                         CASE l.CaLam 
                             WHEN N'Ca sáng' THEN 1
                             WHEN N'Ca chiều' THEN 2
                             WHEN N'Ca tối' THEN 3
                             ELSE 4
                         END, p.TenPhongMay";

            return await _context.LichTrucViewModel.FromSqlRaw(query,
                new SqlParameter("@TuNgay", tuNgay.Date),
                new SqlParameter("@DenNgay", denNgay.Date)
            ).ToListAsync();
        }

        // Lấy lịch trực theo nhân viên
        public async Task<IEnumerable<LichTrucViewModel>> GetByNhanVienAsync(int maNV)
        {
            var query = @"
                SELECT l.MaLich, l.Ngay, l.MaNV, l.MaPhongMay, l.CaLam, l.GhiChu, l.TrangThai,
                       nv.HoTen as TenNhanVien, p.TenPhongMay
                FROM LichTruc l
                INNER JOIN NhanVien nv ON l.MaNV = nv.MaNV
                INNER JOIN PhongMay p ON l.MaPhongMay = p.MaPhongMay
                WHERE l.MaNV = @MaNV AND l.TrangThai != N'Đã hủy'
                ORDER BY l.Ngay DESC";

            return await _context.LichTrucViewModel.FromSqlRaw(query,
                new SqlParameter("@MaNV", maNV)
            ).ToListAsync();
        }

        // Kiểm tra xung đột lịch trực
        public async Task<bool> KiemTraXungDotLichTrucAsync(int maNV, DateTime ngay, string caLam, int? maLichLoaiTru = null)
        {
            var query = _context.LichTruc
                .Where(l => l.MaNV == maNV &&
                           l.Ngay.Date == ngay.Date &&
                           l.TrangThai != "Đã hủy");

            if (maLichLoaiTru.HasValue)
            {
                query = query.Where(l => l.MaLich != maLichLoaiTru.Value);
            }

            // Nếu không chỉ định ca làm cụ thể, kiểm tra xem có lịch nào trong ngày không
            if (string.IsNullOrEmpty(caLam))
            {
                return await query.AnyAsync();
            }
            else
            {
                return await query.Where(l => l.CaLam == caLam).AnyAsync();
            }
        }

        // Lấy thống kê lịch trực
        public async Task<Dictionary<string, object>> GetThongKeLichTrucAsync(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            if (!tuNgay.HasValue) tuNgay = DateTime.Today.AddMonths(-1);
            if (!denNgay.HasValue) denNgay = DateTime.Today;

            var query = @"
                SELECT 
                    COUNT(*) as TongLichTruc,
                    SUM(CASE WHEN TrangThai = N'Đã lên lịch' THEN 1 ELSE 0 END) as DaLenLich,
                    SUM(CASE WHEN TrangThai = N'Đang trực' THEN 1 ELSE 0 END) as DangTruc,
                    SUM(CASE WHEN TrangThai = N'Hoàn thành' THEN 1 ELSE 0 END) as HoanThanh,
                    SUM(CASE WHEN TrangThai = N'Đã hủy' THEN 1 ELSE 0 END) as DaHuy,
                    COUNT(DISTINCT MaNV) as SoNhanVienThamGia,
                    COUNT(DISTINCT MaPhongMay) as SoPhongDuocTruc
                FROM LichTruc
                WHERE Ngay >= @TuNgay AND Ngay <= @DenNgay";

            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@TuNgay", tuNgay.Value.Date));
            command.Parameters.Add(new SqlParameter("@DenNgay", denNgay.Value.Date));

            var result = new Dictionary<string, object>();

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                result["TongLichTruc"] = reader.GetInt32("TongLichTruc");
                result["DaLenLich"] = reader.GetInt32("DaLenLich");
                result["DangTruc"] = reader.GetInt32("DangTruc");
                result["HoanThanh"] = reader.GetInt32("HoanThanh");
                result["DaHuy"] = reader.GetInt32("DaHuy");
                result["SoNhanVienThamGia"] = reader.GetInt32("SoNhanVienThamGia");
                result["SoPhongDuocTruc"] = reader.GetInt32("SoPhongDuocTruc");
            }

            return result;
        }

        // Lấy lịch trực sắp tới
        public async Task<IEnumerable<LichTrucViewModel>> GetLichTrucSapToiAsync(int soNgay = 7)
        {
            var ngayBatDau = DateTime.Today;
            var ngayKetThuc = DateTime.Today.AddDays(soNgay);

            var query = @"
                SELECT l.MaLich, l.Ngay, l.MaNV, l.MaPhongMay, l.CaLam, l.GhiChu, l.TrangThai,
                       nv.HoTen as TenNhanVien, p.TenPhongMay
                FROM LichTruc l
                INNER JOIN NhanVien nv ON l.MaNV = nv.MaNV
                INNER JOIN PhongMay p ON l.MaPhongMay = p.MaPhongMay
                WHERE l.Ngay >= @NgayBatDau AND l.Ngay <= @NgayKetThuc 
                AND l.TrangThai IN (N'Đã lên lịch', N'Đang trực')
                ORDER BY l.Ngay, 
                         CASE l.CaLam 
                             WHEN N'Ca sáng' THEN 1
                             WHEN N'Ca chiều' THEN 2
                             WHEN N'Ca tối' THEN 3
                             ELSE 4
                         END";

            return await _context.LichTrucViewModel.FromSqlRaw(query,
                new SqlParameter("@NgayBatDau", ngayBatDau),
                new SqlParameter("@NgayKetThuc", ngayKetThuc)
            ).ToListAsync();
        }

        // Cập nhật trạng thái hàng loạt
        public async Task<(int KetQua, string ThongBao)> CapNhatTrangThaiHangLoatAsync(List<int> danhSachMaLich, string trangThaiMoi)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var validStates = new[] { "Đã lên lịch", "Đang trực", "Hoàn thành", "Đã hủy" };
                if (!validStates.Contains(trangThaiMoi))
                {
                    return (0, "Trạng thái không hợp lệ");
                }

                var lichTrucList = await _context.LichTruc
                    .Where(l => danhSachMaLich.Contains(l.MaLich))
                    .ToListAsync();

                if (!lichTrucList.Any())
                {
                    return (0, "Không tìm thấy lịch trực nào");
                }

                int updateCount = 0;
                foreach (var lichTruc in lichTrucList)
                {
                    // Kiểm tra logic business trước khi cập nhật
                    if (CanUpdateStatus(lichTruc.TrangThai, trangThaiMoi, lichTruc.Ngay))
                    {
                        lichTruc.TrangThai = trangThaiMoi;
                        updateCount++;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, $"Cập nhật thành công {updateCount}/{lichTrucList.Count} lịch trực");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        // Tìm kiếm lịch trực nâng cao
        public async Task<IEnumerable<LichTrucViewModel>> TimKiemNangCaoAsync(
            DateTime? tuNgay = null,
            DateTime? denNgay = null,
            int? maNV = null,
            int? maPhongMay = null,
            string caLam = null,
            string trangThai = null,
            int pageNumber = 1,
            int pageSize = 20)
        {
            var query = @"
                SELECT l.MaLich, l.Ngay, l.MaNV, l.MaPhongMay, l.CaLam, l.GhiChu, l.TrangThai,
                       nv.HoTen as TenNhanVien, p.TenPhongMay
                FROM LichTruc l
                INNER JOIN NhanVien nv ON l.MaNV = nv.MaNV
                INNER JOIN PhongMay p ON l.MaPhongMay = p.MaPhongMay
                WHERE 1=1";

            var parameters = new List<SqlParameter>();

            if (tuNgay.HasValue)
            {
                query += " AND l.Ngay >= @TuNgay";
                parameters.Add(new SqlParameter("@TuNgay", tuNgay.Value.Date));
            }

            if (denNgay.HasValue)
            {
                query += " AND l.Ngay <= @DenNgay";
                parameters.Add(new SqlParameter("@DenNgay", denNgay.Value.Date));
            }

            if (maNV.HasValue)
            {
                query += " AND l.MaNV = @MaNV";
                parameters.Add(new SqlParameter("@MaNV", maNV.Value));
            }

            if (maPhongMay.HasValue)
            {
                query += " AND l.MaPhongMay = @MaPhongMay";
                parameters.Add(new SqlParameter("@MaPhongMay", maPhongMay.Value));
            }

            if (!string.IsNullOrEmpty(caLam))
            {
                query += " AND l.CaLam = @CaLam";
                parameters.Add(new SqlParameter("@CaLam", caLam));
            }

            if (!string.IsNullOrEmpty(trangThai))
            {
                query += " AND l.TrangThai = @TrangThai";
                parameters.Add(new SqlParameter("@TrangThai", trangThai));
            }

            query += @" ORDER BY l.Ngay DESC, 
                               CASE l.CaLam 
                                   WHEN N'Ca sáng' THEN 1
                                   WHEN N'Ca chiều' THEN 2
                                   WHEN N'Ca tối' THEN 3
                                   ELSE 4
                               END
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var offset = (pageNumber - 1) * pageSize;
            parameters.Add(new SqlParameter("@Offset", offset));
            parameters.Add(new SqlParameter("@PageSize", pageSize));

            return await _context.LichTrucViewModel.FromSqlRaw(query, parameters.ToArray()).ToListAsync();
        }

        #region Private Helper Methods
        private bool CanUpdateStatus(string currentStatus, string newStatus, DateTime ngayTruc)
        {
            // Logic kiểm tra có thể chuyển trạng thái không
            switch (currentStatus)
            {
                case "Đã lên lịch":
                    return newStatus == "Đang trực" || newStatus == "Đã hủy";

                case "Đang trực":
                    return newStatus == "Hoàn thành" || newStatus == "Đã hủy";

                case "Hoàn thành":
                    return false; // Không thể thay đổi trạng thái đã hoàn thành

                case "Đã hủy":
                    // Chỉ có thể khôi phục nếu chưa quá ngày trực
                    return ngayTruc.Date >= DateTime.Today && newStatus == "Đã lên lịch";

                default:
                    return false;
            }
        }
        #endregion
    }
}
