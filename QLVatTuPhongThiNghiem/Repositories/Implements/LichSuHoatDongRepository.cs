using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class LichSuHoatDongRepository : ILichSuHoatDongRepository
    {
        private readonly AppDbContext _context;

        public LichSuHoatDongRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task GhiLichSuAsync(int maNguoiDung, string hanhDong, string module, string chiTiet, string diaChiIP = null, string userAgent = null)
        {
            try
            {
                var lichSu = new Models.Entities.LichSuHoatDong
                {
                    MaNguoiDung = maNguoiDung,
                    HanhDong = hanhDong,
                    Module = module,
                    ChiTiet = chiTiet,
                    ThoiGian = DateTime.Now,
                    DiaChi_IP = diaChiIP,
                    UserAgent = userAgent
                };

                _context.LichSuHoatDong.Add(lichSu);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Log error nhưng không throw để không ảnh hưởng đến luồng chính
            }
        }

        public async Task<IEnumerable<LichSuHoatDongViewModel>> GetLichSuAsync(int? maNguoiDung = null, DateTime? tuNgay = null, DateTime? denNgay = null, int pageNumber = 1, int pageSize = 50)
        {
            var query = @"
                SELECT l.MaLichSu, n.TenDangNhap + ' (' + ISNULL(n.HoTen, n.TenDangNhap) + ')' as TenNguoiDung,
                       l.HanhDong, l.Module, l.ChiTiet, l.ThoiGian, l.DiaChi_IP
                FROM LichSuHoatDong l
                LEFT JOIN NguoiDung n ON l.MaNguoiDung = n.MaNguoiDung
                WHERE (@MaNguoiDung IS NULL OR l.MaNguoiDung = @MaNguoiDung)
                AND (@TuNgay IS NULL OR l.ThoiGian >= @TuNgay)
                AND (@DenNgay IS NULL OR l.ThoiGian <= @DenNgay)
                ORDER BY l.ThoiGian DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var offset = (pageNumber - 1) * pageSize;

            return await _context.LichSuHoatDongViewModel.FromSqlRaw(query,
                new SqlParameter("@MaNguoiDung", maNguoiDung ?? (object)DBNull.Value),
                new SqlParameter("@TuNgay", tuNgay ?? (object)DBNull.Value),
                new SqlParameter("@DenNgay", denNgay ?? (object)DBNull.Value),
                new SqlParameter("@Offset", offset),
                new SqlParameter("@PageSize", pageSize)
            ).ToListAsync();
        }

        public async Task<int> GetTongSoBanGhiAsync(int? maNguoiDung = null, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            var query = @"
                SELECT COUNT(*)
                FROM LichSuHoatDong l
                WHERE (@MaNguoiDung IS NULL OR l.MaNguoiDung = @MaNguoiDung)
                AND (@TuNgay IS NULL OR l.ThoiGian >= @TuNgay)
                AND (@DenNgay IS NULL OR l.ThoiGian <= @DenNgay)";

            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@MaNguoiDung", maNguoiDung ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TuNgay", tuNgay ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DenNgay", denNgay ?? (object)DBNull.Value));

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
    }
}

