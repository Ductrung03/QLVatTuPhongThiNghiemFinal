using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;

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

        public async Task<IEnumerable<DanhGiaCapDoViewModel>> GetAllAsync()
        {
            var query = @"
                SELECT d.MaDanhGia, d.MaTTB, d.CapDo, d.NgayDanhGia, d.NguoiDanhGia, d.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu) as TenThietBi,
                       n.TenDangNhap as TenNguoiDanhGia
                FROM DanhGiaCapDo d
                INNER JOIN TrangTB t ON d.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                LEFT JOIN NguoiDung n ON d.NguoiDanhGia = n.MaNguoiDung
                ORDER BY d.NgayDanhGia DESC";

            return await _context.DanhGiaCapDoViewModel.FromSqlRaw(query).ToListAsync();
        }
    }
}