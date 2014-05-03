using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;

namespace QLVatTuPhongThiNghiem.Repositories.Implements
{
    public class VaiTroRepository : IVaiTroRepository
    {
        private readonly AppDbContext _context;

        public VaiTroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VaiTroViewModel>> GetAllAsync()
        {
            return await _context.VaiTro
                .Where(v => v.TrangThai)
                .Select(v => new VaiTroViewModel
                {
                    MaVaiTro = v.MaVaiTro,
                    TenVaiTro = v.TenVaiTro,
                    MoTa = v.MoTa,
                    TrangThai = v.TrangThai
                })
                .OrderBy(v => v.TenVaiTro)
                .ToListAsync();
        }

        public async Task<VaiTroViewModel> GetByIdAsync(int maVaiTro)
        {
            return await _context.VaiTro
                .Where(v => v.MaVaiTro == maVaiTro)
                .Select(v => new VaiTroViewModel
                {
                    MaVaiTro = v.MaVaiTro,
                    TenVaiTro = v.TenVaiTro,
                    MoTa = v.MoTa,
                    TrangThai = v.TrangThai
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(int KetQua, string ThongBao)> CreateAsync(VaiTroViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Kiểm tra trùng tên
                if (await _context.VaiTro.AnyAsync(v => v.TenVaiTro == model.TenVaiTro && v.TrangThai))
                {
                    return (0, "Tên vai trò đã tồn tại");
                }

                var vaiTro = new Models.Entities.VaiTro
                {
                    TenVaiTro = model.TenVaiTro,
                    MoTa = model.MoTa,
                    TrangThai = true
                };

                _context.VaiTro.Add(vaiTro);
                await _context.SaveChangesAsync();

                // Thêm quyền hạn nếu có
                if (model.DanhSachQuyen?.Any() == true)
                {
                    foreach (var maQuyen in model.DanhSachQuyen)
                    {
                        _context.VaiTroQuyen.Add(new Models.Entities.VaiTroQuyen
                        {
                            MaVaiTro = vaiTro.MaVaiTro,
                            MaQuyen = maQuyen
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return (1, "Tạo vai trò thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> UpdateAsync(VaiTroViewModel model)
        {
            try
            {
                var vaiTro = await _context.VaiTro.FindAsync(model.MaVaiTro);
                if (vaiTro == null)
                {
                    return (0, "Không tìm thấy vai trò");
                }

                // Kiểm tra trùng tên (trừ chính nó)
                if (await _context.VaiTro.AnyAsync(v => v.TenVaiTro == model.TenVaiTro && v.MaVaiTro != model.MaVaiTro && v.TrangThai))
                {
                    return (0, "Tên vai trò đã tồn tại");
                }

                vaiTro.TenVaiTro = model.TenVaiTro;
                vaiTro.MoTa = model.MoTa;
                vaiTro.TrangThai = model.TrangThai;

                await _context.SaveChangesAsync();
                return (1, "Cập nhật vai trò thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(int KetQua, string ThongBao)> DeleteAsync(int maVaiTro)
        {
            try
            {
                // Kiểm tra có người dùng nào đang sử dụng vai trò này không
                if (await _context.NguoiDungVaiTro.AnyAsync(nv => nv.MaVaiTro == maVaiTro && nv.TrangThai))
                {
                    return (0, "Không thể xóa vai trò đang được sử dụng");
                }

                var vaiTro = await _context.VaiTro.FindAsync(maVaiTro);
                if (vaiTro == null)
                {
                    return (0, "Không tìm thấy vai trò");
                }

                vaiTro.TrangThai = false; // Soft delete
                await _context.SaveChangesAsync();

                return (1, "Xóa vai trò thành công");
            }
            catch (Exception ex)
            {
                return (-1, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<IEnumerable<QuyenHanViewModel>> GetAllQuyenHanAsync()
        {
            return await _context.QuyenHan
                .Select(q => new QuyenHanViewModel
                {
                    MaQuyen = q.MaQuyen,
                    TenQuyen = q.TenQuyen,
                    MoTa = q.MoTa,
                    Module = q.Module,
                    HanhDong = q.HanhDong
                })
                .OrderBy(q => q.Module)
                .ThenBy(q => q.HanhDong)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuyenHanViewModel>> GetQuyenHanByVaiTroAsync(int maVaiTro)
        {
            return await _context.VaiTroQuyen
                .Where(vq => vq.MaVaiTro == maVaiTro)
                .Select(vq => new QuyenHanViewModel
                {
                    MaQuyen = vq.QuyenHan.MaQuyen,
                    TenQuyen = vq.QuyenHan.TenQuyen,
                    MoTa = vq.QuyenHan.MoTa,
                    Module = vq.QuyenHan.Module,
                    HanhDong = vq.QuyenHan.HanhDong
                })
                .ToListAsync();
        }

        public async Task<(int KetQua, string ThongBao)> CapNhatQuyenHanAsync(int maVaiTro, List<int> danhSachQuyen)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Xóa quyền hạn hiện tại
                var existingPermissions = await _context.VaiTroQuyen
                    .Where(vq => vq.MaVaiTro == maVaiTro)
                    .ToListAsync();

                _context.VaiTroQuyen.RemoveRange(existingPermissions);

                // Thêm quyền hạn mới
                foreach (var maQuyen in danhSachQuyen)
                {
                    _context.VaiTroQuyen.Add(new Models.Entities.VaiTroQuyen
                    {
                        MaVaiTro = maVaiTro,
                        MaQuyen = maQuyen
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (1, "Cập nhật quyền hạn thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (-1, $"Lỗi: {ex.Message}");
            }
        }
    }
}
