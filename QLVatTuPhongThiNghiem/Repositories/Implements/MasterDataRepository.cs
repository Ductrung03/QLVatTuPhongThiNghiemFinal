using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.Entities;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class MasterDataRepository : IMasterDataRepository
    {
        private readonly AppDbContext _context;

        public MasterDataRepository(AppDbContext context)
        {
            _context = context;
        }

        #region Loai CRUD
        public async Task<IEnumerable<Loai>> GetAllLoaiAsync()
        {
            return await _context.Loai.OrderBy(l => l.TenLoai).ToListAsync();
        }

        public async Task<Loai> GetLoaiByIdAsync(int maLoai)
        {
            return await _context.Loai.FindAsync(maLoai);
        }

        public async Task<(int KetQua, string ThongBao)> CreateLoaiAsync(Loai loai)
        {
            try
            {
                // Kiểm tra trùng lặp
                if (await _context.Loai.AnyAsync(l => l.TenLoai == loai.TenLoai))
                {
                    return (0, "Tên loại thiết bị đã tồn tại");
                }

                // Tự động tạo MaLoai
                var maxId = await _context.Loai.MaxAsync(l => (int?)l.MaLoai) ?? 0;
                loai.MaLoai = maxId + 1;

                _context.Loai.Add(loai);
                await _context.SaveChangesAsync();
                return (1, "Thêm loại thiết bị thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> UpdateLoaiAsync(Loai loai)
        {
            try
            {
                var existing = await _context.Loai.FindAsync(loai.MaLoai);
                if (existing == null)
                {
                    return (0, "Không tìm thấy loại thiết bị");
                }

                // Kiểm tra trùng tên (trừ chính nó)
                if (await _context.Loai.AnyAsync(l => l.TenLoai == loai.TenLoai && l.MaLoai != loai.MaLoai))
                {
                    return (0, "Tên loại thiết bị đã tồn tại");
                }

                existing.TenLoai = loai.TenLoai;
                await _context.SaveChangesAsync();
                return (1, "Cập nhật loại thiết bị thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> DeleteLoaiAsync(int maLoai)
        {
            try
            {
                // Kiểm tra ràng buộc
                if (await _context.TrangTB.AnyAsync(t => t.MaLoai == maLoai))
                {
                    return (0, "Không thể xóa loại thiết bị đang được sử dụng");
                }

                var loai = await _context.Loai.FindAsync(maLoai);
                if (loai == null)
                {
                    return (0, "Không tìm thấy loại thiết bị");
                }

                _context.Loai.Remove(loai);
                await _context.SaveChangesAsync();
                return (1, "Xóa loại thiết bị thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }
        #endregion

        #region ThuongHieu CRUD
        public async Task<IEnumerable<ThuongHieu>> GetAllThuongHieuAsync()
        {
            return await _context.ThuongHieu.OrderBy(th => th.TenThuongHieu).ToListAsync();
        }

        public async Task<ThuongHieu> GetThuongHieuByIdAsync(int maThuongHieu)
        {
            return await _context.ThuongHieu.FindAsync(maThuongHieu);
        }

        public async Task<(int KetQua, string ThongBao)> CreateThuongHieuAsync(ThuongHieu thuongHieu)
        {
            try
            {
                // Kiểm tra trùng lặp
                if (await _context.ThuongHieu.AnyAsync(th => th.TenThuongHieu == thuongHieu.TenThuongHieu))
                {
                    return (0, "Tên thương hiệu đã tồn tại");
                }

                _context.ThuongHieu.Add(thuongHieu);
                await _context.SaveChangesAsync();
                return (1, "Thêm thương hiệu thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> UpdateThuongHieuAsync(ThuongHieu thuongHieu)
        {
            try
            {
                var existing = await _context.ThuongHieu.FindAsync(thuongHieu.MaThuongHieu);
                if (existing == null)
                {
                    return (0, "Không tìm thấy thương hiệu");
                }

                // Kiểm tra trùng tên (trừ chính nó)
                if (await _context.ThuongHieu.AnyAsync(th => th.TenThuongHieu == thuongHieu.TenThuongHieu && th.MaThuongHieu != thuongHieu.MaThuongHieu))
                {
                    return (0, "Tên thương hiệu đã tồn tại");
                }

                existing.TenThuongHieu = thuongHieu.TenThuongHieu;
                await _context.SaveChangesAsync();
                return (1, "Cập nhật thương hiệu thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> DeleteThuongHieuAsync(int maThuongHieu)
        {
            try
            {
                // Kiểm tra ràng buộc
                if (await _context.TrangTB.AnyAsync(t => t.MaThuongHieu == maThuongHieu))
                {
                    return (0, "Không thể xóa thương hiệu đang được sử dụng");
                }

                var thuongHieu = await _context.ThuongHieu.FindAsync(maThuongHieu);
                if (thuongHieu == null)
                {
                    return (0, "Không tìm thấy thương hiệu");
                }

                _context.ThuongHieu.Remove(thuongHieu);
                await _context.SaveChangesAsync();
                return (1, "Xóa thương hiệu thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }
        #endregion

        #region PhongMay CRUD
        public async Task<IEnumerable<PhongMay>> GetAllPhongMayAsync()
        {
            return await _context.PhongMay.OrderBy(pm => pm.TenPhongMay).ToListAsync();
        }

        public async Task<PhongMay> GetPhongMayByIdAsync(int maPhongMay)
        {
            return await _context.PhongMay.FindAsync(maPhongMay);
        }

        public async Task<(int KetQua, string ThongBao)> CreatePhongMayAsync(PhongMay phongMay)
        {
            try
            {
                // Kiểm tra trùng lặp
                if (await _context.PhongMay.AnyAsync(pm => pm.TenPhongMay == phongMay.TenPhongMay))
                {
                    return (0, "Tên phòng máy đã tồn tại");
                }

                _context.PhongMay.Add(phongMay);
                await _context.SaveChangesAsync();
                return (1, "Thêm phòng máy thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> UpdatePhongMayAsync(PhongMay phongMay)
        {
            try
            {
                var existing = await _context.PhongMay.FindAsync(phongMay.MaPhongMay);
                if (existing == null)
                {
                    return (0, "Không tìm thấy phòng máy");
                }

                // Kiểm tra trùng tên (trừ chính nó)
                if (await _context.PhongMay.AnyAsync(pm => pm.TenPhongMay == phongMay.TenPhongMay && pm.MaPhongMay != phongMay.MaPhongMay))
                {
                    return (0, "Tên phòng máy đã tồn tại");
                }

                existing.TenPhongMay = phongMay.TenPhongMay;
                await _context.SaveChangesAsync();
                return (1, "Cập nhật phòng máy thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> DeletePhongMayAsync(int maPhongMay)
        {
            try
            {
                // Kiểm tra ràng buộc
                if (await _context.TrangTB.AnyAsync(t => t.MaPhongMay == maPhongMay) ||
                    await _context.LichTruc.AnyAsync(l => l.MaPhongMay == maPhongMay))
                {
                    return (0, "Không thể xóa phòng máy đang được sử dụng");
                }

                var phongMay = await _context.PhongMay.FindAsync(maPhongMay);
                if (phongMay == null)
                {
                    return (0, "Không tìm thấy phòng máy");
                }

                _context.PhongMay.Remove(phongMay);
                await _context.SaveChangesAsync();
                return (1, "Xóa phòng máy thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }
        #endregion

        #region NhanVien CRUD
        public async Task<IEnumerable<NhanVien>> GetAllNhanVienAsync()
        {
            return await _context.NhanVien
                .Where(nv => nv.TrangThai)
                .OrderBy(nv => nv.HoTen)
                .ToListAsync();
        }

        public async Task<NhanVien> GetNhanVienByIdAsync(int maNV)
        {
            return await _context.NhanVien.FindAsync(maNV);
        }

        public async Task<(int KetQua, string ThongBao)> CreateNhanVienAsync(NhanVien nhanVien)
        {
            try
            {
                // Kiểm tra trùng email
                if (!string.IsNullOrEmpty(nhanVien.Email) &&
                    await _context.NhanVien.AnyAsync(nv => nv.Email == nhanVien.Email && nv.TrangThai))
                {
                    return (0, "Email đã được sử dụng");
                }

                // Tự động tạo MaNV
                var maxId = await _context.NhanVien.MaxAsync(nv => (int?)nv.MaNV) ?? 0;
                nhanVien.MaNV = maxId + 1;
                nhanVien.TrangThai = true;
                nhanVien.NgayVaoLam = nhanVien.NgayVaoLam ?? DateTime.Now;

                _context.NhanVien.Add(nhanVien);
                await _context.SaveChangesAsync();
                return (1, "Thêm nhân viên thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> UpdateNhanVienAsync(NhanVien nhanVien)
        {
            try
            {
                var existing = await _context.NhanVien.FindAsync(nhanVien.MaNV);
                if (existing == null)
                {
                    return (0, "Không tìm thấy nhân viên");
                }

                // Kiểm tra trùng email (trừ chính nó)
                if (!string.IsNullOrEmpty(nhanVien.Email) &&
                    await _context.NhanVien.AnyAsync(nv => nv.Email == nhanVien.Email && nv.MaNV != nhanVien.MaNV && nv.TrangThai))
                {
                    return (0, "Email đã được sử dụng");
                }

                existing.HoTen = nhanVien.HoTen;
                existing.SoDienThoai = nhanVien.SoDienThoai;
                existing.Email = nhanVien.Email;
                existing.DiaChi = nhanVien.DiaChi;
                existing.ChucVu = nhanVien.ChucVu;
                existing.NgayVaoLam = nhanVien.NgayVaoLam;
                existing.TrangThai = nhanVien.TrangThai;

                await _context.SaveChangesAsync();
                return (1, "Cập nhật nhân viên thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> DeleteNhanVienAsync(int maNV)
        {
            try
            {
                // Kiểm tra ràng buộc
                if (await _context.LichTruc.AnyAsync(l => l.MaNV == maNV))
                {
                    return (0, "Không thể xóa nhân viên đang có lịch trực");
                }

                var nhanVien = await _context.NhanVien.FindAsync(maNV);
                if (nhanVien == null)
                {
                    return (0, "Không tìm thấy nhân viên");
                }

                // Soft delete
                nhanVien.TrangThai = false;
                await _context.SaveChangesAsync();
                return (1, "Xóa nhân viên thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }
        #endregion
    }
}
