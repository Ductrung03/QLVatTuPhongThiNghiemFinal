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

        public async Task<IEnumerable<LichThucHanhViewModel>> GetAllAsync()
        {
            var query = @"
                SELECT l.MaLich, l.TrangThai, l.ThoiGianBD, l.ThoiGianKT, l.MaNguoiDung, n.TenDangNhap as TenNguoiDung
                FROM LichThucHanh l 
                LEFT JOIN NguoiDung n ON l.MaNguoiDung = n.MaNguoiDung
                ORDER BY l.ThoiGianBD DESC";

            return await _context.LichThucHanhViewModel.FromSqlRaw(query).ToListAsync();
        }

        public async Task<IEnumerable<LichThucHanhViewModel>> GetByUserAsync(int maNguoiDung)
        {
            var query = @"
                SELECT l.MaLich, l.TrangThai, l.ThoiGianBD, l.ThoiGianKT, l.MaNguoiDung, n.TenDangNhap as TenNguoiDung
                FROM LichThucHanh l 
                LEFT JOIN NguoiDung n ON l.MaNguoiDung = n.MaNguoiDung
                WHERE l.MaNguoiDung = @MaNguoiDung
                ORDER BY l.ThoiGianBD DESC";

            return await _context.LichThucHanhViewModel
                .FromSqlRaw(query, new SqlParameter("@MaNguoiDung", maNguoiDung))
                .ToListAsync();
        }
    }
}