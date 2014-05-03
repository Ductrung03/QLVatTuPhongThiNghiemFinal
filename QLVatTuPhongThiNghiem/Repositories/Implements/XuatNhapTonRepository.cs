using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class XuatNhapTonRepository : IXuatNhapTonRepository
    {
        private readonly AppDbContext _context;

        public XuatNhapTonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> XuatThietBiAsync(XuatNhapTonViewModel model)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_XuatThietBi @MaTTB, @SoLuong, @NguoiTao, @GhiChu, @KetQua OUTPUT",
                new SqlParameter("@MaTTB", model.MaTTB),
                new SqlParameter("@SoLuong", model.SoLuong),
                new SqlParameter("@NguoiTao", model.NguoiTao),
                new SqlParameter("@GhiChu", model.GhiChu ?? (object)DBNull.Value),
                ketQuaParam
            );

            return (int)ketQuaParam.Value;
        }

        public async Task<int> NhapThietBiAsync(XuatNhapTonViewModel model)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_NhapThietBi @MaTTB, @SoLuong, @NguoiTao, @GhiChu, @KetQua OUTPUT",
                new SqlParameter("@MaTTB", model.MaTTB),
                new SqlParameter("@SoLuong", model.SoLuong),
                new SqlParameter("@NguoiTao", model.NguoiTao),
                new SqlParameter("@GhiChu", model.GhiChu ?? (object)DBNull.Value),
                ketQuaParam
            );

            return (int)ketQuaParam.Value;
        }

        public async Task<IEnumerable<dynamic>> BaoCaoTonKhoAsync(int? maPhongMay = null)
        {
            // For dynamic results, we need to use a different approach
            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = "EXEC SP_BaoCaoTonKho @MaPhongMay";
            command.Parameters.Add(new SqlParameter("@MaPhongMay", maPhongMay ?? (object)DBNull.Value));

            var results = new List<dynamic>();

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var expando = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    expando[reader.GetName(i)] = reader[i];
                }
                results.Add(expando);
            }

            return results;
        }

        public async Task<IEnumerable<XuatNhapTonViewModel>> GetAllAsync()
        {
            var query = @"
                SELECT x.MaPhieu, x.LoaiPhieu, x.MaTTB, x.SoLuong, x.NgayTao, x.NguoiTao, x.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu) as TenThietBi,
                       n.TenDangNhap as TenNguoiTao
                FROM XuatNhapTon x
                INNER JOIN TrangTB t ON x.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                LEFT JOIN NguoiDung n ON x.NguoiTao = n.MaNguoiDung
                ORDER BY x.NgayTao DESC";

            return await _context.XuatNhapTonViewModel.FromSqlRaw(query).ToListAsync();
        }
    }
}