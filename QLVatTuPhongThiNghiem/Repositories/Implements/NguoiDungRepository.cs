using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using System.Data;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class NguoiDungRepository : INguoiDungRepository
    {
        private readonly AppDbContext _context;

        public NguoiDungRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(int KetQua, int MaNguoiDung, string ThongBao)> DangNhapBaoMatAsync(string tenDangNhap, string matKhau, string diaChiIP, string userAgent)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var maNguoiDungParam = new SqlParameter("@MaNguoiDung", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var thongBaoParam = new SqlParameter("@ThongBao", System.Data.SqlDbType.NVarChar, 255)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_DangNhap_BaoMat @TenDangNhap, @MatKhau, @DiaChi_IP, @UserAgent, @KetQua OUTPUT, @MaNguoiDung OUTPUT, @ThongBao OUTPUT",
                new SqlParameter("@TenDangNhap", tenDangNhap),
                new SqlParameter("@MatKhau", matKhau),
                new SqlParameter("@DiaChi_IP", diaChiIP ?? (object)DBNull.Value),
                new SqlParameter("@UserAgent", userAgent ?? (object)DBNull.Value),
                ketQuaParam,
                maNguoiDungParam,
                thongBaoParam
            );

            return ((int)ketQuaParam.Value,
                   maNguoiDungParam.Value == DBNull.Value ? 0 : (int)maNguoiDungParam.Value,
                   thongBaoParam.Value?.ToString() ?? "");
        }

        public async Task<(int KetQua, string ThongBao)> DoiMatKhauAsync(int maNguoiDung, string matKhauCu, string matKhauMoi)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var thongBaoParam = new SqlParameter("@ThongBao", System.Data.SqlDbType.NVarChar, 255)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_DoiMatKhau @MaNguoiDung, @MatKhauCu, @MatKhauMoi, @KetQua OUTPUT, @ThongBao OUTPUT",
                new SqlParameter("@MaNguoiDung", maNguoiDung),
                new SqlParameter("@MatKhauCu", matKhauCu),
                new SqlParameter("@MatKhauMoi", matKhauMoi),
                ketQuaParam,
                thongBaoParam
            );

            return ((int)ketQuaParam.Value, thongBaoParam.Value?.ToString() ?? "");
        }

        public async Task<(int KetQua, int MaNguoiDung, string ThongBao)> TaoTaiKhoanAsync(TaoTaiKhoanViewModel model, int maNguoiTao)
        {
            var ketQuaParam = new SqlParameter("@KetQua", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var maNguoiDungParam = new SqlParameter("@MaNguoiDung", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var thongBaoParam = new SqlParameter("@ThongBao", System.Data.SqlDbType.NVarChar, 255)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_TaoTaiKhoan @TenDangNhap, @MatKhau, @Email, @HoTen, @MaVaiTro, @MaNguoiTao, @KetQua OUTPUT, @MaNguoiDung OUTPUT, @ThongBao OUTPUT",
                new SqlParameter("@TenDangNhap", model.TenDangNhap),
                new SqlParameter("@MatKhau", model.MatKhau),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@HoTen", model.HoTen),
                new SqlParameter("@MaVaiTro", model.MaVaiTro),
                new SqlParameter("@MaNguoiTao", maNguoiTao),
                ketQuaParam,
                maNguoiDungParam,
                thongBaoParam
            );

            return ((int)ketQuaParam.Value,
                   maNguoiDungParam.Value == DBNull.Value ? 0 : (int)maNguoiDungParam.Value,
                   thongBaoParam.Value?.ToString() ?? "");
        }

        public async Task<IEnumerable<NguoiDungViewModel>> GetAllAsync()
        {
            var query = @"
                SELECT n.MaNguoiDung, n.TenDangNhap, n.HoTen, n.Email, n.TrangThaiTaiKhoan, 
                       n.NgayTao, n.LanDangNhapCuoi,
                       STRING_AGG(v.TenVaiTro, ', ') as TenVaiTro
                FROM NguoiDung n
                LEFT JOIN NguoiDungVaiTro nv ON n.MaNguoiDung = nv.MaNguoiDung AND nv.TrangThai = 1
                LEFT JOIN VaiTro v ON nv.MaVaiTro = v.MaVaiTro AND v.TrangThai = 1
                GROUP BY n.MaNguoiDung, n.TenDangNhap, n.HoTen, n.Email, n.TrangThaiTaiKhoan, n.NgayTao, n.LanDangNhapCuoi
                ORDER BY n.NgayTao DESC";

            return await _context.NguoiDungViewModel.FromSqlRaw(query).ToListAsync();
        }

        public async Task<NguoiDungViewModel> GetByIdAsync(int maNguoiDung)
        {
            var query = @"
                SELECT n.MaNguoiDung, n.TenDangNhap, n.HoTen, n.Email, n.TrangThaiTaiKhoan, 
                       n.NgayTao, n.LanDangNhapCuoi,
                       STRING_AGG(v.TenVaiTro, ', ') as TenVaiTro
                FROM NguoiDung n
                LEFT JOIN NguoiDungVaiTro nv ON n.MaNguoiDung = nv.MaNguoiDung AND nv.TrangThai = 1
                LEFT JOIN VaiTro v ON nv.MaVaiTro = v.MaVaiTro AND v.TrangThai = 1
                WHERE n.MaNguoiDung = @MaNguoiDung
                GROUP BY n.MaNguoiDung, n.TenDangNhap, n.HoTen, n.Email, n.TrangThaiTaiKhoan, n.NgayTao, n.LanDangNhapCuoi";

            return await _context.NguoiDungViewModel
                .FromSqlRaw(query, new SqlParameter("@MaNguoiDung", maNguoiDung))
                .FirstOrDefaultAsync();
        }

        public async Task<(int KetQua, string ThongBao)> CapNhatTrangThaiAsync(int maNguoiDung, bool trangThai)
        {
            try
            {
                var user = await _context.NguoiDung.FindAsync(maNguoiDung);
                if (user == null)
                {
                    return (0, "Không tìm thấy người dùng");
                }

                user.TrangThaiTaiKhoan = trangThai;
                user.NgayCapNhat = DateTime.Now;

                if (!trangThai)
                {
                    user.NgayKhoaTaiKhoan = DateTime.Now;
                }
                else
                {
                    user.NgayKhoaTaiKhoan = null;
                    user.SoLanDangNhapSai = 0;
                }

                await _context.SaveChangesAsync();
                return (1, trangThai ? "Kích hoạt tài khoản thành công" : "Khóa tài khoản thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> CapNhatThongTinAsync(NguoiDungViewModel model)
        {
            try
            {
                var user = await _context.NguoiDung.FindAsync(model.MaNguoiDung);
                if (user == null)
                {
                    return (0, "Không tìm thấy người dùng");
                }

                user.HoTen = model.HoTen;
                user.Email = model.Email;
                user.NgayCapNhat = DateTime.Now;

                await _context.SaveChangesAsync();
                return (1, "Cập nhật thông tin thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<string>> GetQuyenHanAsync(int maNguoiDung)
        {
            var query = @"
                SELECT DISTINCT q.TenQuyen
                FROM NguoiDung n
                INNER JOIN NguoiDungVaiTro nv ON n.MaNguoiDung = nv.MaNguoiDung AND nv.TrangThai = 1
                INNER JOIN VaiTro v ON nv.MaVaiTro = v.MaVaiTro AND v.TrangThai = 1
                INNER JOIN VaiTroQuyen vq ON v.MaVaiTro = vq.MaVaiTro
                INNER JOIN QuyenHan q ON vq.MaQuyen = q.MaQuyen
                WHERE n.MaNguoiDung = @MaNguoiDung AND n.TrangThaiTaiKhoan = 1";

            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@MaNguoiDung", maNguoiDung));

            var permissions = new List<string>();

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                permissions.Add(reader.GetString("TenQuyen"));
            }

            return permissions;
        }

        public async Task<(int KetQua, string ThongBao)> PhanQuyenAsync(int maNguoiDung, List<int> danhSachVaiTro)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Xóa các vai trò hiện tại
                var existingRoles = await _context.NguoiDungVaiTro
                    .Where(x => x.MaNguoiDung == maNguoiDung)
                    .ToListAsync();

                _context.NguoiDungVaiTro.RemoveRange(existingRoles);

                // Thêm vai trò mới
                foreach (var maVaiTro in danhSachVaiTro)
                {
                    _context.NguoiDungVaiTro.Add(new Models.Entities.NguoiDungVaiTro
                    {
                        MaNguoiDung = maNguoiDung,
                        MaVaiTro = maVaiTro,
                        NgayCapQuyen = DateTime.Now,
                        TrangThai = true
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Phân quyền thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }
    }
}