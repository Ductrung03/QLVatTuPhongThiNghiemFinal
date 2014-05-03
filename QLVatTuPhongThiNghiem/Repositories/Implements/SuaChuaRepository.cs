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

        public async Task<IEnumerable<SuaChuaViewModel>> GetAllAsync()
        {
            var query = @"
                SELECT s.MaSuaChua, s.MaTTB, s.NgayBatDau, s.NgayKetThuc, s.LoaiSuaChua, s.MoTa, 
                       s.ChiPhi, s.TrangThai, s.NguoiThucHien,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu) as TenThietBi,
                       n.TenDangNhap as TenNguoiThucHien
                FROM LichSuSuaChua s
                INNER JOIN TrangTB t ON s.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                LEFT JOIN NguoiDung n ON s.NguoiThucHien = n.MaNguoiDung
                ORDER BY s.NgayBatDau DESC";

            return await _context.SuaChuaViewModel.FromSqlRaw(query).ToListAsync();
        }

        public async Task<IEnumerable<SuaChuaViewModel>> GetByTrangThaiAsync(string trangThai)
        {
            var query = @"
                SELECT s.MaSuaChua, s.MaTTB, s.NgayBatDau, s.NgayKetThuc, s.LoaiSuaChua, s.MoTa, 
                       s.ChiPhi, s.TrangThai, s.NguoiThucHien,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu) as TenThietBi,
                       n.TenDangNhap as TenNguoiThucHien
                FROM LichSuSuaChua s
                INNER JOIN TrangTB t ON s.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                LEFT JOIN NguoiDung n ON s.NguoiThucHien = n.MaNguoiDung
                WHERE s.TrangThai = @TrangThai
                ORDER BY s.NgayBatDau DESC";

            return await _context.SuaChuaViewModel
                .FromSqlRaw(query, new SqlParameter("@TrangThai", trangThai))
                .ToListAsync();
        }
    }
}