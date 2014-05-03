

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using System.Data;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class ThongBaoRepository : IThongBaoRepository
    {
        private readonly AppDbContext _context;

        public ThongBaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(int KetQua, string ThongBao)> GuiThongBaoAsync(ThongBaoViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(model.TieuDe))
                {
                    return (0, "Tiêu đề không được để trống");
                }

                if (string.IsNullOrWhiteSpace(model.NoiDung))
                {
                    return (0, "Nội dung không được để trống");
                }

                // Kiểm tra người nhận nếu không phải gửi cho tất cả
                if (model.MaNguoiNhan.HasValue)
                {
                    var nguoiNhan = await _context.NguoiDung.FindAsync(model.MaNguoiNhan.Value);
                    if (nguoiNhan == null || !nguoiNhan.TrangThaiTaiKhoan)
                    {
                        return (0, "Người nhận không tồn tại hoặc tài khoản đã bị khóa");
                    }
                }

                // Tạo entity thông báo
                var thongBao = new Models.Entities.ThongBao
                {
                    TieuDe = model.TieuDe.Trim(),
                    NoiDung = model.NoiDung.Trim(),
                    LoaiThongBao = model.LoaiThongBao,
                    MaNguoiGui = model.MaNguoiNhan, // Tạm thời, sẽ được set đúng từ controller
                    MaNguoiNhan = model.MaNguoiNhan,
                    NgayTao = DateTime.Now,
                    DaDoc = false,
                    TrangThai = true
                };

                _context.ThongBao.Add(thongBao);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Gửi thông báo thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<ThongBaoViewModel>> GetThongBaoByNguoiDungAsync(int maNguoiDung)
        {
            var query = @"
                SELECT t.MaThongBao, t.TieuDe, t.NoiDung, t.LoaiThongBao, t.NgayTao, t.DaDoc,
                       CASE 
                           WHEN t.MaNguoiGui IS NULL THEN N'Hệ thống'
                           ELSE ISNULL(ng.HoTen, ng.TenDangNhap)
                       END as TenNguoiGui,
                       CASE 
                           WHEN t.MaNguoiNhan IS NULL THEN N'Tất cả người dùng'
                           ELSE ISNULL(nn.HoTen, nn.TenDangNhap)
                       END as TenNguoiNhan
                FROM ThongBao t
                LEFT JOIN NguoiDung ng ON t.MaNguoiGui = ng.MaNguoiDung
                LEFT JOIN NguoiDung nn ON t.MaNguoiNhan = nn.MaNguoiDung
                WHERE (t.MaNguoiNhan = @MaNguoiDung OR t.MaNguoiNhan IS NULL) 
                AND t.TrangThai = 1
                ORDER BY t.DaDoc ASC, t.NgayTao DESC";

            return await _context.ThongBaoViewModel.FromSqlRaw(query,
                new SqlParameter("@MaNguoiDung", maNguoiDung)
            ).ToListAsync();
        }

        public async Task<IEnumerable<ThongBaoViewModel>> GetAllThongBaoAsync()
        {
            var query = @"
                SELECT t.MaThongBao, t.TieuDe, t.NoiDung, t.LoaiThongBao, t.NgayTao, t.DaDoc,
                       CASE 
                           WHEN t.MaNguoiGui IS NULL THEN N'Hệ thống'
                           ELSE ISNULL(ng.HoTen, ng.TenDangNhap)
                       END as TenNguoiGui,
                       CASE 
                           WHEN t.MaNguoiNhan IS NULL THEN N'Tất cả người dùng'
                           ELSE ISNULL(nn.HoTen, nn.TenDangNhap)
                       END as TenNguoiNhan
                FROM ThongBao t
                LEFT JOIN NguoiDung ng ON t.MaNguoiGui = ng.MaNguoiDung
                LEFT JOIN NguoiDung nn ON t.MaNguoiNhan = nn.MaNguoiDung
                WHERE t.TrangThai = 1
                ORDER BY t.NgayTao DESC";

            return await _context.ThongBaoViewModel.FromSqlRaw(query).ToListAsync();
        }

        public async Task<(int KetQua, string ThongBao)> DanhDauDaDocAsync(int maThongBao, int maNguoiDung)
        {
            try
            {
                var thongBao = await _context.ThongBao
                    .Where(t => t.MaThongBao == maThongBao &&
                               (t.MaNguoiNhan == maNguoiDung || t.MaNguoiNhan == null) &&
                               t.TrangThai == true)
                    .FirstOrDefaultAsync();

                if (thongBao == null)
                {
                    return (0, "Không tìm thấy thông báo hoặc bạn không có quyền truy cập");
                }

                if (thongBao.DaDoc)
                {
                    return (1, "Thông báo đã được đánh dấu đọc trước đó");
                }

                thongBao.DaDoc = true;
                await _context.SaveChangesAsync();

                return (1, "Đánh dấu đã đọc thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<int> GetSoThongBaoChuaDocAsync(int maNguoiDung)
        {
            try
            {
                return await _context.ThongBao
                    .Where(t => (t.MaNguoiNhan == maNguoiDung || t.MaNguoiNhan == null) &&
                               !t.DaDoc &&
                               t.TrangThai)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<ThongBaoViewModel> GetThongBaoByIdAsync(int maThongBao)
        {
            var query = @"
                SELECT t.MaThongBao, t.TieuDe, t.NoiDung, t.LoaiThongBao, t.NgayTao, t.DaDoc,
                       CASE 
                           WHEN t.MaNguoiGui IS NULL THEN N'Hệ thống'
                           ELSE ISNULL(ng.HoTen, ng.TenDangNhap)
                       END as TenNguoiGui,
                       CASE 
                           WHEN t.MaNguoiNhan IS NULL THEN N'Tất cả người dùng'
                           ELSE ISNULL(nn.HoTen, nn.TenDangNhap)
                       END as TenNguoiNhan
                FROM ThongBao t
                LEFT JOIN NguoiDung ng ON t.MaNguoiGui = ng.MaNguoiDung
                LEFT JOIN NguoiDung nn ON t.MaNguoiNhan = nn.MaNguoiDung
                WHERE t.MaThongBao = @MaThongBao AND t.TrangThai = 1";

            return await _context.ThongBaoViewModel.FromSqlRaw(query,
                new SqlParameter("@MaThongBao", maThongBao)
            ).FirstOrDefaultAsync();
        }

        public async Task<(int KetQua, string ThongBao)> XoaThongBaoAsync(int maThongBao)
        {
            try
            {
                var thongBao = await _context.ThongBao.FindAsync(maThongBao);
                if (thongBao == null)
                {
                    return (0, "Không tìm thấy thông báo");
                }

                // Soft delete
                thongBao.TrangThai = false;
                await _context.SaveChangesAsync();

                return (1, "Xóa thông báo thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<ThongBaoViewModel>> GetThongBaoMoiNhatAsync(int maNguoiDung, int soLuong = 5)
        {
            var query = @"
                SELECT TOP (@SoLuong)
                       t.MaThongBao, t.TieuDe, t.NoiDung, t.LoaiThongBao, t.NgayTao, t.DaDoc,
                       CASE 
                           WHEN t.MaNguoiGui IS NULL THEN N'Hệ thống'
                           ELSE ISNULL(ng.HoTen, ng.TenDangNhap)
                       END as TenNguoiGui,
                       CASE 
                           WHEN t.MaNguoiNhan IS NULL THEN N'Tất cả người dùng'
                           ELSE ISNULL(nn.HoTen, nn.TenDangNhap)
                       END as TenNguoiNhan
                FROM ThongBao t
                LEFT JOIN NguoiDung ng ON t.MaNguoiGui = ng.MaNguoiDung
                LEFT JOIN NguoiDung nn ON t.MaNguoiNhan = nn.MaNguoiDung
                WHERE (t.MaNguoiNhan = @MaNguoiDung OR t.MaNguoiNhan IS NULL) 
                AND t.TrangThai = 1
                ORDER BY t.NgayTao DESC";

            return await _context.ThongBaoViewModel.FromSqlRaw(query,
                new SqlParameter("@MaNguoiDung", maNguoiDung),
                new SqlParameter("@SoLuong", soLuong)
            ).ToListAsync();
        }

        public async Task<IEnumerable<ThongBaoViewModel>> GetThongBaoChuaDocAsync(int maNguoiDung)
        {
            var query = @"
                SELECT t.MaThongBao, t.TieuDe, t.NoiDung, t.LoaiThongBao, t.NgayTao, t.DaDoc,
                       CASE 
                           WHEN t.MaNguoiGui IS NULL THEN N'Hệ thống'
                           ELSE ISNULL(ng.HoTen, ng.TenDangNhap)
                       END as TenNguoiGui,
                       CASE 
                           WHEN t.MaNguoiNhan IS NULL THEN N'Tất cả người dùng'
                           ELSE ISNULL(nn.HoTen, nn.TenDangNhap)
                       END as TenNguoiNhan
                FROM ThongBao t
                LEFT JOIN NguoiDung ng ON t.MaNguoiGui = ng.MaNguoiDung
                LEFT JOIN NguoiDung nn ON t.MaNguoiNhan = nn.MaNguoiDung
                WHERE (t.MaNguoiNhan = @MaNguoiDung OR t.MaNguoiNhan IS NULL) 
                AND t.DaDoc = 0
                AND t.TrangThai = 1
                ORDER BY t.NgayTao DESC";

            return await _context.ThongBaoViewModel.FromSqlRaw(query,
                new SqlParameter("@MaNguoiDung", maNguoiDung)
            ).ToListAsync();
        }

        public async Task<(int KetQua, string ThongBao)> DanhDauTatCaDaDocAsync(int maNguoiDung)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var thongBaoList = await _context.ThongBao
                    .Where(t => (t.MaNguoiNhan == maNguoiDung || t.MaNguoiNhan == null) &&
                               !t.DaDoc &&
                               t.TrangThai)
                    .ToListAsync();

                if (!thongBaoList.Any())
                {
                    return (1, "Không có thông báo chưa đọc");
                }

                foreach (var thongBao in thongBaoList)
                {
                    thongBao.DaDoc = true;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, $"Đã đánh dấu {thongBaoList.Count} thông báo là đã đọc");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<ThongBaoViewModel>> TimKiemThongBaoAsync(
            int? maNguoiDung = null,
            string tuKhoa = null,
            string loaiThongBao = null,
            DateTime? tuNgay = null,
            DateTime? denNgay = null,
            bool? daDoc = null,
            int pageNumber = 1,
            int pageSize = 20)
        {
            var query = @"
                SELECT t.MaThongBao, t.TieuDe, t.NoiDung, t.LoaiThongBao, t.NgayTao, t.DaDoc,
                       CASE 
                           WHEN t.MaNguoiGui IS NULL THEN N'Hệ thống'
                           ELSE ISNULL(ng.HoTen, ng.TenDangNhap)
                       END as TenNguoiGui,
                       CASE 
                           WHEN t.MaNguoiNhan IS NULL THEN N'Tất cả người dùng'
                           ELSE ISNULL(nn.HoTen, nn.TenDangNhap)
                       END as TenNguoiNhan
                FROM ThongBao t
                LEFT JOIN NguoiDung ng ON t.MaNguoiGui = ng.MaNguoiDung
                LEFT JOIN NguoiDung nn ON t.MaNguoiNhan = nn.MaNguoiDung
                WHERE t.TrangThai = 1";

            var parameters = new List<SqlParameter>();

            if (maNguoiDung.HasValue)
            {
                query += " AND (t.MaNguoiNhan = @MaNguoiDung OR t.MaNguoiNhan IS NULL)";
                parameters.Add(new SqlParameter("@MaNguoiDung", maNguoiDung.Value));
            }

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query += " AND (t.TieuDe LIKE @TuKhoa OR t.NoiDung LIKE @TuKhoa)";
                parameters.Add(new SqlParameter("@TuKhoa", $"%{tuKhoa}%"));
            }

            if (!string.IsNullOrEmpty(loaiThongBao))
            {
                query += " AND t.LoaiThongBao = @LoaiThongBao";
                parameters.Add(new SqlParameter("@LoaiThongBao", loaiThongBao));
            }

            if (tuNgay.HasValue)
            {
                query += " AND t.NgayTao >= @TuNgay";
                parameters.Add(new SqlParameter("@TuNgay", tuNgay.Value.Date));
            }

            if (denNgay.HasValue)
            {
                query += " AND t.NgayTao <= @DenNgay";
                parameters.Add(new SqlParameter("@DenNgay", denNgay.Value.Date.AddDays(1).AddTicks(-1)));
            }

            if (daDoc.HasValue)
            {
                query += " AND t.DaDoc = @DaDoc";
                parameters.Add(new SqlParameter("@DaDoc", daDoc.Value));
            }

            query += " ORDER BY t.NgayTao DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var offset = (pageNumber - 1) * pageSize;
            parameters.Add(new SqlParameter("@Offset", offset));
            parameters.Add(new SqlParameter("@PageSize", pageSize));

            return await _context.ThongBaoViewModel.FromSqlRaw(query, parameters.ToArray()).ToListAsync();
        }

        public async Task<int> GetTongSoThongBaoAsync(int? maNguoiDung = null)
        {
            var query = _context.ThongBao.Where(t => t.TrangThai);

            if (maNguoiDung.HasValue)
            {
                query = query.Where(t => t.MaNguoiNhan == maNguoiDung.Value || t.MaNguoiNhan == null);
            }

            return await query.CountAsync();
        }

        public async Task<Dictionary<string, int>> GetThongKeThongBaoAsync(int? maNguoiDung = null)
        {
            var query = @"
                SELECT 
                    COUNT(*) as TongSo,
                    SUM(CASE WHEN DaDoc = 0 THEN 1 ELSE 0 END) as ChuaDoc,
                    SUM(CASE WHEN DaDoc = 1 THEN 1 ELSE 0 END) as DaDoc,
                    SUM(CASE WHEN LoaiThongBao = 'Thong_Tin' THEN 1 ELSE 0 END) as ThongTin,
                    SUM(CASE WHEN LoaiThongBao = 'Canh_Bao' THEN 1 ELSE 0 END) as CanhBao,
                    SUM(CASE WHEN LoaiThongBao = 'Loi' THEN 1 ELSE 0 END) as Loi,
                    SUM(CASE WHEN LoaiThongBao = 'Thanh_Cong' THEN 1 ELSE 0 END) as ThanhCong,
                    SUM(CASE WHEN CAST(NgayTao as DATE) = CAST(GETDATE() as DATE) THEN 1 ELSE 0 END) as HomNay,
                    SUM(CASE WHEN NgayTao >= DATEADD(day, -7, GETDATE()) THEN 1 ELSE 0 END) as TuanNay
                FROM ThongBao 
                WHERE TrangThai = 1";

            var parameters = new List<SqlParameter>();

            if (maNguoiDung.HasValue)
            {
                query += " AND (MaNguoiNhan = @MaNguoiDung OR MaNguoiNhan IS NULL)";
                parameters.Add(new SqlParameter("@MaNguoiDung", maNguoiDung.Value));
            }

            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddRange(parameters.ToArray());

            var result = new Dictionary<string, int>();

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                result["TongSo"] = reader.GetInt32("TongSo");
                result["ChuaDoc"] = reader.GetInt32("ChuaDoc");
                result["DaDoc"] = reader.GetInt32("DaDoc");
                result["ThongTin"] = reader.GetInt32("ThongTin");
                result["CanhBao"] = reader.GetInt32("CanhBao");
                result["Loi"] = reader.GetInt32("Loi");
                result["ThanhCong"] = reader.GetInt32("ThanhCong");
                result["HomNay"] = reader.GetInt32("HomNay");
                result["TuanNay"] = reader.GetInt32("TuanNay");
            }

            return result;
        }

        public async Task<(int KetQua, string ThongBao)> GuiThongBaoNhomAsync(
            string tieuDe,
            string noiDung,
            string loaiThongBao,
            List<int> danhSachNguoiNhan,
            int? maNguoiGui = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(tieuDe) || string.IsNullOrWhiteSpace(noiDung))
                {
                    return (0, "Tiêu đề và nội dung không được để trống");
                }

                if (danhSachNguoiNhan == null || !danhSachNguoiNhan.Any())
                {
                    return (0, "Danh sách người nhận không được để trống");
                }

                // Kiểm tra người nhận có tồn tại không
                var nguoiNhanHopLe = await _context.NguoiDung
                    .Where(n => danhSachNguoiNhan.Contains(n.MaNguoiDung) && n.TrangThaiTaiKhoan)
                    .Select(n => n.MaNguoiDung)
                    .ToListAsync();

                if (!nguoiNhanHopLe.Any())
                {
                    return (0, "Không có người nhận hợp lệ nào");
                }

                var thongBaoList = new List<Models.Entities.ThongBao>();

                foreach (var maNguoiNhan in nguoiNhanHopLe)
                {
                    thongBaoList.Add(new Models.Entities.ThongBao
                    {
                        TieuDe = tieuDe.Trim(),
                        NoiDung = noiDung.Trim(),
                        LoaiThongBao = loaiThongBao,
                        MaNguoiGui = maNguoiGui,
                        MaNguoiNhan = maNguoiNhan,
                        NgayTao = DateTime.Now,
                        DaDoc = false,
                        TrangThai = true
                    });
                }

                _context.ThongBao.AddRange(thongBaoList);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, $"Gửi thông báo thành công cho {nguoiNhanHopLe.Count} người dùng");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> CapNhatThongBaoAsync(ThongBaoViewModel model)
        {
            try
            {
                var thongBao = await _context.ThongBao.FindAsync(model.MaThongBao);
                if (thongBao == null)
                {
                    return (0, "Không tìm thấy thông báo");
                }

                if (string.IsNullOrWhiteSpace(model.TieuDe) || string.IsNullOrWhiteSpace(model.NoiDung))
                {
                    return (0, "Tiêu đề và nội dung không được để trống");
                }

                thongBao.TieuDe = model.TieuDe.Trim();
                thongBao.NoiDung = model.NoiDung.Trim();
                thongBao.LoaiThongBao = model.LoaiThongBao;

                await _context.SaveChangesAsync();

                return (1, "Cập nhật thông báo thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> XoaThongBaoHangLoatAsync(List<int> danhSachMaThongBao)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (danhSachMaThongBao == null || !danhSachMaThongBao.Any())
                {
                    return (0, "Danh sách thông báo không được để trống");
                }

                var thongBaoList = await _context.ThongBao
                    .Where(t => danhSachMaThongBao.Contains(t.MaThongBao))
                    .ToListAsync();

                if (!thongBaoList.Any())
                {
                    return (0, "Không tìm thấy thông báo nào");
                }

                // Soft delete
                foreach (var thongBao in thongBaoList)
                {
                    thongBao.TrangThai = false;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, $"Xóa thành công {thongBaoList.Count} thông báo");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<ThongBaoViewModel>> GetThongBaoTheoLoaiAsync(string loaiThongBao, int? maNguoiDung = null)
        {
            var query = @"
                SELECT t.MaThongBao, t.TieuDe, t.NoiDung, t.LoaiThongBao, t.NgayTao, t.DaDoc,
                       CASE 
                           WHEN t.MaNguoiGui IS NULL THEN N'Hệ thống'
                           ELSE ISNULL(ng.HoTen, ng.TenDangNhap)
                       END as TenNguoiGui,
                       CASE 
                           WHEN t.MaNguoiNhan IS NULL THEN N'Tất cả người dùng'
                           ELSE ISNULL(nn.HoTen, nn.TenDangNhap)
                       END as TenNguoiNhan
                FROM ThongBao t
                LEFT JOIN NguoiDung ng ON t.MaNguoiGui = ng.MaNguoiDung
                LEFT JOIN NguoiDung nn ON t.MaNguoiNhan = nn.MaNguoiDung
                WHERE t.LoaiThongBao = @LoaiThongBao AND t.TrangThai = 1";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@LoaiThongBao", loaiThongBao)
            };

            if (maNguoiDung.HasValue)
            {
                query += " AND (t.MaNguoiNhan = @MaNguoiDung OR t.MaNguoiNhan IS NULL)";
                parameters.Add(new SqlParameter("@MaNguoiDung", maNguoiDung.Value));
            }

            query += " ORDER BY t.NgayTao DESC";

            return await _context.ThongBaoViewModel.FromSqlRaw(query, parameters.ToArray()).ToListAsync();
        }
    }
}
