using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.Entities;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class TrangTBRepository : BaseRepository<TrangTB>, ITrangTBRepository
    {
        public TrangTBRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<(int KetQua, int MaTTB)> InsertAsync(TrangTBViewModel model, int nguoiCapNhat)
        {
            
                var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                var maTTBParam = new SqlParameter("@MaTTB", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_TrangTB_Insert @MaPhongMay, @MaLoai, @GiaTien, @TinhTrang, @NgayNhap, @MaThuongHieu, @MaTTB OUTPUT, @KetQua OUTPUT",
                    new SqlParameter("@MaPhongMay", model.MaPhongMay),
                    new SqlParameter("@MaLoai", model.MaLoai),
                    new SqlParameter("@GiaTien", model.GiaTien),
                    new SqlParameter("@TinhTrang", model.TinhTrang),
                    new SqlParameter("@NgayNhap", model.NgayNhap),
                    new SqlParameter("@MaThuongHieu", model.MaThuongHieu),
                    maTTBParam,
                    ketQuaParam
                );

                return ((int)ketQuaParam.Value, (int)maTTBParam.Value);
           
        }

        public async Task<int> UpdateAsync(TrangTBViewModel model, int nguoiCapNhat)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_TrangTB_Update @MaTTB, @MaPhongMay, @MaLoai, @GiaTien, @TinhTrang, @MaThuongHieu, @NguoiCapNhat, @KetQua OUTPUT",
                new SqlParameter("@MaTTB", model.MaTTB),
                new SqlParameter("@MaPhongMay", model.MaPhongMay),
                new SqlParameter("@MaLoai", model.MaLoai),
                new SqlParameter("@GiaTien", model.GiaTien ?? (object)DBNull.Value),
                new SqlParameter("@TinhTrang", model.TinhTrang ?? (object)DBNull.Value),
                new SqlParameter("@MaThuongHieu", model.MaThuongHieu),
                new SqlParameter("@NguoiCapNhat", nguoiCapNhat),
                ketQuaParam
            );

            return (int)ketQuaParam.Value;
        }

        public async Task<int> DeleteAsync(int maTTB)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_TrangTB_Delete @MaTTB, @KetQua OUTPUT",
                new SqlParameter("@MaTTB", maTTB),
                ketQuaParam
            );

            return (int)ketQuaParam.Value;
        }

        public async Task<SearchTrangTBViewModel> SearchAsync(SearchTrangTBViewModel searchModel)
        {
            var results = await _context.TrangTBViewModel.FromSqlRaw(
                @"EXEC SP_TrangTB_Search @MaPhongMay, @MaLoai, @TinhTrang, @MaThuongHieu, @PageNumber, @PageSize",
                new SqlParameter("@MaPhongMay", searchModel.MaPhongMay ?? (object)DBNull.Value),
                new SqlParameter("@MaLoai", searchModel.MaLoai ?? (object)DBNull.Value),
                new SqlParameter("@TinhTrang", searchModel.TinhTrang ?? (object)DBNull.Value),
                new SqlParameter("@MaThuongHieu", searchModel.MaThuongHieu ?? (object)DBNull.Value),
                new SqlParameter("@PageNumber", searchModel.PageNumber),
                new SqlParameter("@PageSize", searchModel.PageSize)
            ).ToListAsync();

            searchModel.Results = results;
            return searchModel;
        }

        public override async Task<IEnumerable<TrangTB>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.PhongMay)
                .Include(t => t.Loai)
                .Include(t => t.ThuongHieu)
                .ToListAsync();
        }
    }
}