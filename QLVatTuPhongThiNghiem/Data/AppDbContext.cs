// File: Data/AppDbContext.cs (Cập nhật)
// Vị trí: QLVatTuPhongThiNghiem/Data/AppDbContext.cs
// Thay thế file cũ

using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Models.Entities;
using QLVatTuPhongThiNghiem.Models.ViewModels;

namespace QLVatTuPhongThiNghiem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Entities chính
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<VaiTro> VaiTro { get; set; }
        public DbSet<QuyenHan> QuyenHan { get; set; }
        public DbSet<VaiTroQuyen> VaiTroQuyen { get; set; }
        public DbSet<NguoiDungVaiTro> NguoiDungVaiTro { get; set; }
        public DbSet<LichSuHoatDong> LichSuHoatDong { get; set; }
        public DbSet<ThongBao> ThongBao { get; set; }
        public DbSet<Loai> Loai { get; set; }
        public DbSet<ThuongHieu> ThuongHieu { get; set; }
        public DbSet<PhongMay> PhongMay { get; set; }
        public DbSet<TrangTB> TrangTB { get; set; }
        public DbSet<LichThucHanh> LichThucHanh { get; set; }
        public DbSet<LichSuSuaChua> LichSuSuaChua { get; set; }
        public DbSet<DanhGiaCapDo> DanhGiaCapDo { get; set; }
        public DbSet<XuatNhapTon> XuatNhapTon { get; set; }
        public DbSet<LichTruc> LichTruc { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }

        // ViewModels cho SqlQueryRaw
        public DbSet<ThongKeTheoPhongViewModel> ThongKeTheoPhongViewModel { get; set; }
        public DbSet<ThongKeSuDungTheoThangViewModel> ThongKeSuDungTheoThangViewModel { get; set; }
        public DbSet<DanhGiaCapDoViewModel> DanhGiaCapDoViewModel { get; set; }
        public DbSet<SuaChuaViewModel> SuaChuaViewModel { get; set; }
        public DbSet<XuatNhapTonViewModel> XuatNhapTonViewModel { get; set; }
        public DbSet<LichThucHanhViewModel> LichThucHanhViewModel { get; set; }
        public DbSet<TrangTBViewModel> TrangTBViewModel { get; set; }
        public DbSet<NguoiDungViewModel> NguoiDungViewModel { get; set; }
        public DbSet<LichSuHoatDongViewModel> LichSuHoatDongViewModel { get; set; }
        public DbSet<ThongBaoViewModel> ThongBaoViewModel { get; set; }
        public DbSet<LichTrucViewModel> LichTrucViewModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure table names
            modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");
            modelBuilder.Entity<VaiTro>().ToTable("VaiTro");
            modelBuilder.Entity<QuyenHan>().ToTable("QuyenHan");
            modelBuilder.Entity<VaiTroQuyen>().ToTable("VaiTroQuyen");
            modelBuilder.Entity<NguoiDungVaiTro>().ToTable("NguoiDungVaiTro");
            modelBuilder.Entity<LichSuHoatDong>().ToTable("LichSuHoatDong");
            modelBuilder.Entity<ThongBao>().ToTable("ThongBao");
            modelBuilder.Entity<Loai>().ToTable("Loai");
            modelBuilder.Entity<ThuongHieu>().ToTable("ThuongHieu");
            modelBuilder.Entity<PhongMay>().ToTable("PhongMay");
            modelBuilder.Entity<TrangTB>().ToTable("TrangTB");
            modelBuilder.Entity<LichThucHanh>().ToTable("LichThucHanh");
            modelBuilder.Entity<LichSuSuaChua>().ToTable("LichSuSuaChua");
            modelBuilder.Entity<DanhGiaCapDo>().ToTable("DanhGiaCapDo");
            modelBuilder.Entity<XuatNhapTon>().ToTable("XuatNhapTon");
            modelBuilder.Entity<LichTruc>().ToTable("LichTruc");
            modelBuilder.Entity<NhanVien>().ToTable("NhanVien");

            // Configure ViewModels as keyless entities
            modelBuilder.Entity<ThongKeTheoPhongViewModel>().HasNoKey();
            modelBuilder.Entity<ThongKeSuDungTheoThangViewModel>().HasNoKey();
            modelBuilder.Entity<DanhGiaCapDoViewModel>().HasNoKey();
            modelBuilder.Entity<SuaChuaViewModel>().HasNoKey();
            modelBuilder.Entity<XuatNhapTonViewModel>().HasNoKey();
            modelBuilder.Entity<LichThucHanhViewModel>().HasNoKey();
            modelBuilder.Entity<TrangTBViewModel>().HasNoKey();
            modelBuilder.Entity<NguoiDungViewModel>().HasNoKey();
            modelBuilder.Entity<LichSuHoatDongViewModel>().HasNoKey();
            modelBuilder.Entity<ThongBaoViewModel>().HasNoKey();
            modelBuilder.Entity<LichTrucViewModel>().HasNoKey();

            // Configure composite keys for many-to-many relationships
            modelBuilder.Entity<VaiTroQuyen>()
                .HasKey(vq => new { vq.MaVaiTro, vq.MaQuyen });

            modelBuilder.Entity<NguoiDungVaiTro>()
                .HasKey(nv => new { nv.MaNguoiDung, nv.MaVaiTro });

            // Configure decimal precision
            modelBuilder.Entity<LichSuSuaChua>()
                .Property(e => e.ChiPhi)
                .HasPrecision(18, 2);

            // Configure relationships for security entities
            modelBuilder.Entity<VaiTroQuyen>()
                .HasOne(vq => vq.VaiTro)
                .WithMany(v => v.VaiTroQuyens)
                .HasForeignKey(vq => vq.MaVaiTro)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VaiTroQuyen>()
                .HasOne(vq => vq.QuyenHan)
                .WithMany(q => q.VaiTroQuyens)
                .HasForeignKey(vq => vq.MaQuyen)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NguoiDungVaiTro>()
                .HasOne(nv => nv.NguoiDung)
                .WithMany(n => n.NguoiDungVaiTros)
                .HasForeignKey(nv => nv.MaNguoiDung)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NguoiDungVaiTro>()
                .HasOne(nv => nv.VaiTro)
                .WithMany(v => v.NguoiDungVaiTros)
                .HasForeignKey(nv => nv.MaVaiTro)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichSuHoatDong>()
                .HasOne(l => l.NguoiDung)
                .WithMany(n => n.LichSuHoatDongs)
                .HasForeignKey(l => l.MaNguoiDung)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ThongBao relationships
            modelBuilder.Entity<ThongBao>()
                .HasOne(t => t.NguoiGui)
                .WithMany(n => n.ThongBaosDaGui)
                .HasForeignKey(t => t.MaNguoiGui)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ThongBao>()
                .HasOne(t => t.NguoiNhan)
                .WithMany(n => n.ThongBaosDaNhan)
                .HasForeignKey(t => t.MaNguoiNhan)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure existing relationships
            modelBuilder.Entity<TrangTB>()
                .HasOne(t => t.PhongMay)
                .WithMany()
                .HasForeignKey(t => t.MaPhongMay)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrangTB>()
                .HasOne(t => t.Loai)
                .WithMany()
                .HasForeignKey(t => t.MaLoai)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrangTB>()
                .HasOne(t => t.ThuongHieu)
                .WithMany()
                .HasForeignKey(t => t.MaThuongHieu)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichSuSuaChua>()
                .HasOne(l => l.TrangTB)
                .WithMany()
                .HasForeignKey(l => l.MaTTB)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichSuSuaChua>()
                .HasOne(l => l.NguoiThucHien_User)
                .WithMany()
                .HasForeignKey(l => l.NguoiThucHien)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DanhGiaCapDo>()
                .HasOne(d => d.TrangTB)
                .WithMany()
                .HasForeignKey(d => d.MaTTB)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DanhGiaCapDo>()
                .HasOne(d => d.NguoiDanhGia_User)
                .WithMany()
                .HasForeignKey(d => d.NguoiDanhGia)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<XuatNhapTon>()
                .HasOne(x => x.TrangTB)
                .WithMany()
                .HasForeignKey(x => x.MaTTB)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<XuatNhapTon>()
                .HasOne(x => x.NguoiTao_User)
                .WithMany()
                .HasForeignKey(x => x.NguoiTao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichThucHanh>()
                .HasOne(l => l.NguoiDung)
                .WithMany()
                .HasForeignKey(l => l.MaNguoiDung)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichTruc>()
                .HasOne<NhanVien>()
                .WithMany()
                .HasForeignKey(l => l.MaNV)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichTruc>()
                .HasOne<PhongMay>()
                .WithMany()
                .HasForeignKey(l => l.MaPhongMay)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}