using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(int KetQua, int MaNguoiDung)> DangNhapAsync(string tenDangNhap, string matKhau)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var maNguoiDungParam = new SqlParameter("@MaNguoiDung", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_DangNhap @TenDangNhap, @MatKhau, @KetQua OUTPUT, @MaNguoiDung OUTPUT",
                new SqlParameter("@TenDangNhap", tenDangNhap),
                new SqlParameter("@MatKhau", matKhau),
                ketQuaParam,
                maNguoiDungParam
            );

            return ((int)ketQuaParam.Value, (int)maNguoiDungParam.Value);
        }

        public async Task DangXuatAsync(int maNguoiDung)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_DangXuat @MaNguoiDung",
                new SqlParameter("@MaNguoiDung", maNguoiDung)
            );
        }
    }
}

