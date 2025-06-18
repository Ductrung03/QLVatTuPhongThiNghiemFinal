using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using System.Data;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class DanhGiaCapDoRepository : IDanhGiaCapDoRepository
    {
        private readonly AppDbContext _context;

        public DanhGiaCapDoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> DanhGiaCapDoAsync(DanhGiaCapDoViewModel model)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_DanhGiaCapDo @MaTTB, @CapDo, @NguoiDanhGia, @GhiChu, @KetQua OUTPUT",
                new SqlParameter("@MaTTB", model.MaTTB),
                new SqlParameter("@CapDo", model.CapDo),
                new SqlParameter("@NguoiDanhGia", model.NguoiDanhGia),
                new SqlParameter("@GhiChu", model.GhiChu ?? (object)DBNull.Value),
                ketQuaParam
            );

            return (int)ketQuaParam.Value;
        }

        public async Task<DanhGiaCapDoViewModel> GetCapDoThietBiAsync(int maTTB)
        {
            var result = await _context.DanhGiaCapDoViewModel
                .FromSqlRaw("EXEC SP_GetCapDoThietBi @MaTTB",
                    new SqlParameter("@MaTTB", maTTB))
                .FirstOrDefaultAsync();

            return result;
        }

        // PHƯƠNG THỨC MỚI: Lấy chi tiết đánh giá theo ID
        public async Task<DanhGiaCapDoViewModel> GetByIdAsync(int maDanhGia)
        {
            var query = @"
                SELECT d.MaDanhGia, d.MaTTB, d.CapDo, d.NgayDanhGia, d.NguoiDanhGia, d.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDanhGia
                FROM DanhGiaCapDo d
                INNER JOIN TrangTB t ON d.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON d.NguoiDanhGia = n.MaNguoiDung
                WHERE d.MaDanhGia = @MaDanhGia";

            return await _context.DanhGiaCapDoViewModel
                .FromSqlRaw(query, new SqlParameter("@MaDanhGia", maDanhGia))
                .FirstOrDefaultAsync();
        }

        // PHƯƠNG THỨC MỚI: Cập nhật đánh giá
        public async Task<(int KetQua, string ThongBao)> CapNhatDanhGiaAsync(DanhGiaCapDoViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.DanhGiaCapDo.FindAsync(model.MaDanhGia);
                if (existing == null)
                {
                    return (0, "Không tìm thấy đánh giá");
                }

                // Validate cấp độ
                if (model.CapDo < 1 || model.CapDo > 5)
                {
                    return (0, "Cấp độ phải từ 1 đến 5");
                }

                // Cập nhật thông tin
                existing.CapDo = model.CapDo;
                existing.GhiChu = model.GhiChu;
                existing.NgayDanhGia = DateTime.Now; // Cập nhật ngày đánh giá

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Cập nhật đánh giá thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Xóa đánh giá
        public async Task<(int KetQua, string ThongBao)> XoaDanhGiaAsync(int maDanhGia)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.DanhGiaCapDo.FindAsync(maDanhGia);
                if (existing == null)
                {
                    return (0, "Không tìm thấy đánh giá");
                }

                // Kiểm tra xem có phải là đánh giá mới nhất không
                var latestEvaluation = await _context.DanhGiaCapDo
                    .Where(d => d.MaTTB == existing.MaTTB)
                    .OrderByDescending(d => d.NgayDanhGia)
                    .FirstOrDefaultAsync();

                if (latestEvaluation?.MaDanhGia != maDanhGia)
                {
                    return (0, "Chỉ có thể xóa đánh giá mới nhất của thiết bị");
                }

                _context.DanhGiaCapDo.Remove(existing);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Xóa đánh giá thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetAllAsync()
        {
            var query = @"
                SELECT d.MaDanhGia, d.MaTTB, d.CapDo, d.NgayDanhGia, d.NguoiDanhGia, d.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDanhGia
                FROM DanhGiaCapDo d
                INNER JOIN TrangTB t ON d.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON d.NguoiDanhGia = n.MaNguoiDung
                ORDER BY d.NgayDanhGia DESC";

            return await _context.DanhGiaCapDoViewModel.FromSqlRaw(query).ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy đánh giá theo thiết bị
        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetByThietBiAsync(int maTTB)
        {
            var query = @"
                SELECT d.MaDanhGia, d.MaTTB, d.CapDo, d.NgayDanhGia, d.NguoiDanhGia, d.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDanhGia
                FROM DanhGiaCapDo d
                INNER JOIN TrangTB t ON d.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON d.NguoiDanhGia = n.MaNguoiDung
                WHERE d.MaTTB = @MaTTB
                ORDER BY d.NgayDanhGia DESC";

            return await _context.DanhGiaCapDoViewModel
                .FromSqlRaw(query, new SqlParameter("@MaTTB", maTTB))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy đánh giá theo người đánh giá
        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetByNguoiDanhGiaAsync(int nguoiDanhGia)
        {
            var query = @"
                SELECT d.MaDanhGia, d.MaTTB, d.CapDo, d.NgayDanhGia, d.NguoiDanhGia, d.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDanhGia
                FROM DanhGiaCapDo d
                INNER JOIN TrangTB t ON d.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON d.NguoiDanhGia = n.MaNguoiDung
                WHERE d.NguoiDanhGia = @NguoiDanhGia
                ORDER BY d.NgayDanhGia DESC";

            return await _context.DanhGiaCapDoViewModel
                .FromSqlRaw(query, new SqlParameter("@NguoiDanhGia", nguoiDanhGia))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy đánh giá theo cấp độ
        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetByCapDoAsync(int capDo)
        {
            var query = @"
                SELECT d.MaDanhGia, d.MaTTB, d.CapDo, d.NgayDanhGia, d.NguoiDanhGia, d.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDanhGia
                FROM DanhGiaCapDo d
                INNER JOIN TrangTB t ON d.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON d.NguoiDanhGia = n.MaNguoiDung
                WHERE d.CapDo = @CapDo
                ORDER BY d.NgayDanhGia DESC";

            return await _context.DanhGiaCapDoViewModel
                .FromSqlRaw(query, new SqlParameter("@CapDo", capDo))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy đánh giá theo khoảng thời gian
        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay)
        {
            var query = @"
                SELECT d.MaDanhGia, d.MaTTB, d.CapDo, d.NgayDanhGia, d.NguoiDanhGia, d.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDanhGia
                FROM DanhGiaCapDo d
                INNER JOIN TrangTB t ON d.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON d.NguoiDanhGia = n.MaNguoiDung
                WHERE d.NgayDanhGia >= @TuNgay AND d.NgayDanhGia <= @DenNgay
                ORDER BY d.NgayDanhGia DESC";

            return await _context.DanhGiaCapDoViewModel
                .FromSqlRaw(query,
                    new SqlParameter("@TuNgay", tuNgay.Date),
                    new SqlParameter("@DenNgay", denNgay.Date.AddDays(1).AddTicks(-1)))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Thống kê đánh giá
        public async Task<Dictionary<string, object>> ThongKeDanhGiaAsync(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            var query = @"
                SELECT 
                    COUNT(*) as TongSoDanhGia,
                    AVG(CAST(CapDo as FLOAT)) as CapDoTrungBinh,
                    SUM(CASE WHEN CapDo = 1 THEN 1 ELSE 0 END) as CapDo1,
                    SUM(CASE WHEN CapDo = 2 THEN 1 ELSE 0 END) as CapDo2,
                    SUM(CASE WHEN CapDo = 3 THEN 1 ELSE 0 END) as CapDo3,
                    SUM(CASE WHEN CapDo = 4 THEN 1 ELSE 0 END) as CapDo4,
                    SUM(CASE WHEN CapDo = 5 THEN 1 ELSE 0 END) as CapDo5,
                    COUNT(DISTINCT MaTTB) as SoThietBiDuocDanhGia,
                    COUNT(DISTINCT NguoiDanhGia) as SoNguoiThamGia
                FROM DanhGiaCapDo
                WHERE (@TuNgay IS NULL OR NgayDanhGia >= @TuNgay)
                AND (@DenNgay IS NULL OR NgayDanhGia <= @DenNgay)";

            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@TuNgay", tuNgay ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DenNgay", denNgay ?? (object)DBNull.Value));

            var result = new Dictionary<string, object>();

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                result["TongSoDanhGia"] = reader.GetInt32("TongSoDanhGia");
                result["CapDoTrungBinh"] = reader.IsDBNull("CapDoTrungBinh") ? 0.0 : reader.GetDouble("CapDoTrungBinh");
                result["CapDo1"] = reader.GetInt32("CapDo1");
                result["CapDo2"] = reader.GetInt32("CapDo2");
                result["CapDo3"] = reader.GetInt32("CapDo3");
                result["CapDo4"] = reader.GetInt32("CapDo4");
                result["CapDo5"] = reader.GetInt32("CapDo5");
                result["SoThietBiDuocDanhGia"] = reader.GetInt32("SoThietBiDuocDanhGia");
                result["SoNguoiThamGia"] = reader.GetInt32("SoNguoiThamGia");
            }

            return result;
        }
    }
}