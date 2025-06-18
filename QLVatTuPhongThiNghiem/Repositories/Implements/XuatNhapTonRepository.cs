using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using System.Data;

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

        // PHƯƠNG THỨC MỚI: Lấy chi tiết phiếu theo ID
        public async Task<XuatNhapTonViewModel> GetByIdAsync(int maPhieu)
        {
            var query = @"
                SELECT x.MaPhieu, x.LoaiPhieu, x.MaTTB, x.SoLuong, x.NgayTao, x.NguoiTao, x.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiTao
                FROM XuatNhapTon x
                INNER JOIN TrangTB t ON x.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON x.NguoiTao = n.MaNguoiDung
                WHERE x.MaPhieu = @MaPhieu";

            return await _context.XuatNhapTonViewModel
                .FromSqlRaw(query, new SqlParameter("@MaPhieu", maPhieu))
                .FirstOrDefaultAsync();
        }

        // PHƯƠNG THỨC MỚI: Cập nhật phiếu xuất nhập
        public async Task<(int KetQua, string ThongBao)> CapNhatPhieuAsync(XuatNhapTonViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.XuatNhapTon.FindAsync(model.MaPhieu);
                if (existing == null)
                {
                    return (0, "Không tìm thấy phiếu");
                }

                // Chỉ cho phép sửa trong ngày tạo
                if (existing.NgayTao.Date != DateTime.Today)
                {
                    return (0, "Chỉ có thể sửa phiếu trong ngày tạo");
                }

                // Kiểm tra số lượng hợp lệ
                if (model.SoLuong <= 0)
                {
                    return (0, "Số lượng phải lớn hơn 0");
                }

                // Nếu là phiếu xuất, kiểm tra tồn kho
                if (existing.LoaiPhieu == "XUAT")
                {
                    var tonKhoHienTai = await GetTonKhoThietBiAsync(existing.MaTTB);
                    var chenhLech = model.SoLuong - existing.SoLuong;

                    if (tonKhoHienTai < chenhLech)
                    {
                        return (0, $"Không đủ tồn kho. Hiện tại: {tonKhoHienTai}, cần thêm: {chenhLech}");
                    }
                }

                // Cập nhật thông tin
                existing.SoLuong = model.SoLuong;
                existing.GhiChu = model.GhiChu;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Cập nhật phiếu thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        // PHƯƠNG THỨC MỚI: Xóa phiếu xuất nhập
        public async Task<(int KetQua, string ThongBao)> XoaPhieuAsync(int maPhieu)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.XuatNhapTon.FindAsync(maPhieu);
                if (existing == null)
                {
                    return (0, "Không tìm thấy phiếu");
                }

                // Chỉ cho phép xóa trong ngày tạo
                if (existing.NgayTao.Date != DateTime.Today)
                {
                    return (0, "Chỉ có thể xóa phiếu trong ngày tạo");
                }

                // Kiểm tra ảnh hưởng đến tồn kho nếu xóa phiếu xuất
                if (existing.LoaiPhieu == "XUAT")
                {
                    var tonKhoHienTai = await GetTonKhoThietBiAsync(existing.MaTTB);
                    // Khi xóa phiếu xuất, tồn kho sẽ tăng lên
                    // Không cần kiểm tra gì thêm
                }
                else if (existing.LoaiPhieu == "NHAP")
                {
                    var tonKhoHienTai = await GetTonKhoThietBiAsync(existing.MaTTB);
                    // Khi xóa phiếu nhập, tồn kho sẽ giảm
                    if (tonKhoHienTai < existing.SoLuong)
                    {
                        return (0, $"Không thể xóa phiếu nhập vì sẽ làm tồn kho âm. Tồn kho hiện tại: {tonKhoHienTai}");
                    }
                }

                _context.XuatNhapTon.Remove(existing);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Xóa phiếu thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<dynamic>> BaoCaoTonKhoAsync(int? maPhongMay = null)
        {
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
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiTao
                FROM XuatNhapTon x
                INNER JOIN TrangTB t ON x.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON x.NguoiTao = n.MaNguoiDung
                ORDER BY x.NgayTao DESC";

            return await _context.XuatNhapTonViewModel.FromSqlRaw(query).ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy phiếu theo thiết bị
        public async Task<IEnumerable<XuatNhapTonViewModel>> GetByThietBiAsync(int maTTB)
        {
            var query = @"
                SELECT x.MaPhieu, x.LoaiPhieu, x.MaTTB, x.SoLuong, x.NgayTao, x.NguoiTao, x.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiTao
                FROM XuatNhapTon x
                INNER JOIN TrangTB t ON x.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON x.NguoiTao = n.MaNguoiDung
                WHERE x.MaTTB = @MaTTB
                ORDER BY x.NgayTao DESC";

            return await _context.XuatNhapTonViewModel
                .FromSqlRaw(query, new SqlParameter("@MaTTB", maTTB))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy phiếu theo người tạo
        public async Task<IEnumerable<XuatNhapTonViewModel>> GetByNguoiTaoAsync(int nguoiTao)
        {
            var query = @"
                SELECT x.MaPhieu, x.LoaiPhieu, x.MaTTB, x.SoLuong, x.NgayTao, x.NguoiTao, x.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiTao
                FROM XuatNhapTon x
                INNER JOIN TrangTB t ON x.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON x.NguoiTao = n.MaNguoiDung
                WHERE x.NguoiTao = @NguoiTao
                ORDER BY x.NgayTao DESC";

            return await _context.XuatNhapTonViewModel
                .FromSqlRaw(query, new SqlParameter("@NguoiTao", nguoiTao))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy phiếu theo loại
        public async Task<IEnumerable<XuatNhapTonViewModel>> GetByLoaiPhieuAsync(string loaiPhieu)
        {
            var query = @"
                SELECT x.MaPhieu, x.LoaiPhieu, x.MaTTB, x.SoLuong, x.NgayTao, x.NguoiTao, x.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiTao
                FROM XuatNhapTon x
                INNER JOIN TrangTB t ON x.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON x.NguoiTao = n.MaNguoiDung
                WHERE x.LoaiPhieu = @LoaiPhieu
                ORDER BY x.NgayTao DESC";

            return await _context.XuatNhapTonViewModel
                .FromSqlRaw(query, new SqlParameter("@LoaiPhieu", loaiPhieu))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Lấy phiếu theo khoảng thời gian
        public async Task<IEnumerable<XuatNhapTonViewModel>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay)
        {
            var query = @"
                SELECT x.MaPhieu, x.LoaiPhieu, x.MaTTB, x.SoLuong, x.NgayTao, x.NguoiTao, x.GhiChu,
                       CONCAT(l.TenLoai, ' - ', th.TenThuongHieu, ' (', pm.TenPhongMay, ')') as TenThietBi,
                       ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiTao
                FROM XuatNhapTon x
                INNER JOIN TrangTB t ON x.MaTTB = t.MaTTB
                INNER JOIN Loai l ON t.MaLoai = l.MaLoai
                INNER JOIN ThuongHieu th ON t.MaThuongHieu = th.MaThuongHieu
                INNER JOIN PhongMay pm ON t.MaPhongMay = pm.MaPhongMay
                LEFT JOIN NguoiDung n ON x.NguoiTao = n.MaNguoiDung
                WHERE x.NgayTao >= @TuNgay AND x.NgayTao <= @DenNgay
                ORDER BY x.NgayTao DESC";

            return await _context.XuatNhapTonViewModel
                .FromSqlRaw(query,
                    new SqlParameter("@TuNgay", tuNgay.Date),
                    new SqlParameter("@DenNgay", denNgay.Date.AddDays(1).AddTicks(-1)))
                .ToListAsync();
        }

        // PHƯƠNG THỨC MỚI: Thống kê xuất nhập
        public async Task<Dictionary<string, object>> ThongKeXuatNhapAsync(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            var query = @"
                SELECT 
                    COUNT(*) as TongSoPhieu,
                    SUM(CASE WHEN LoaiPhieu = 'NHAP' THEN 1 ELSE 0 END) as SoPhieuNhap,
                    SUM(CASE WHEN LoaiPhieu = 'XUAT' THEN 1 ELSE 0 END) as SoPhieuXuat,
                    SUM(CASE WHEN LoaiPhieu = 'NHAP' THEN SoLuong ELSE 0 END) as TongSoLuongNhap,
                    SUM(CASE WHEN LoaiPhieu = 'XUAT' THEN SoLuong ELSE 0 END) as TongSoLuongXuat,
                    COUNT(DISTINCT MaTTB) as SoThietBiThamGia,
                    COUNT(DISTINCT NguoiTao) as SoNguoiThamGia
                FROM XuatNhapTon
                WHERE (@TuNgay IS NULL OR NgayTao >= @TuNgay)
                AND (@DenNgay IS NULL OR NgayTao <= @DenNgay)";

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
                result["TongSoPhieu"] = reader.GetInt32("TongSoPhieu");
                result["SoPhieuNhap"] = reader.GetInt32("SoPhieuNhap");
                result["SoPhieuXuat"] = reader.GetInt32("SoPhieuXuat");
                result["TongSoLuongNhap"] = reader.GetInt32("TongSoLuongNhap");
                result["TongSoLuongXuat"] = reader.GetInt32("TongSoLuongXuat");
                result["SoThietBiThamGia"] = reader.GetInt32("SoThietBiThamGia");
                result["SoNguoiThamGia"] = reader.GetInt32("SoNguoiThamGia");
            }

            return result;
        }

        // PHƯƠNG THỨC MỚI: Lấy tồn kho thiết bị
        public async Task<int> GetTonKhoThietBiAsync(int maTTB)
        {
            var query = @"
                SELECT ISNULL(SUM(CASE WHEN LoaiPhieu = 'NHAP' THEN SoLuong ELSE -SoLuong END), 0) as TonKho
                FROM XuatNhapTon
                WHERE MaTTB = @MaTTB";

            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@MaTTB", maTTB));

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result ?? 0);
        }
    }
}