// File: Program.cs (Cập nhật)
// Vị trí: QLVatTuPhongThiNghiem/Program.cs
// Thay thế file cũ

using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Repositories.Implements;
using QLVatTuPhongThiNghiem.Services.Interfaces;
using QLVatTuPhongThiNghiem.Services.Implements;
using QLVatTuPhongThiNghiem.Extensions;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Session với cấu hình bảo mật
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Tăng thời gian session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict; // Bảo mật chống CSRF
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Add Memory Cache
builder.Services.AddMemoryCache();

// Add Logging
builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.AddDebug();
});

// Register Repositories
// Authentication & Security
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<INguoiDungRepository, NguoiDungRepository>();
builder.Services.AddScoped<IVaiTroRepository, VaiTroRepository>();
builder.Services.AddScoped<ILichSuHoatDongRepository, LichSuHoatDongRepository>();
builder.Services.AddScoped<IThongBaoRepository, ThongBaoRepository>();
builder.Services.AddScoped<ILichTrucRepository, LichTrucRepository>();

// Business Logic
builder.Services.AddScoped<ITrangTBRepository, TrangTBRepository>();
builder.Services.AddScoped<ILichThucHanhRepository, LichThucHanhRepository>();
builder.Services.AddScoped<ISuaChuaRepository, SuaChuaRepository>();
builder.Services.AddScoped<IDanhGiaCapDoRepository, DanhGiaCapDoRepository>();
builder.Services.AddScoped<IXuatNhapTonRepository, XuatNhapTonRepository>();
builder.Services.AddScoped<IBaoCaoRepository, BaoCaoRepository>();
builder.Services.AddScoped<IMasterDataRepository,MasterDataRepository>();

// Register Services
// Authentication & Security Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INguoiDungService, NguoiDungService>();
builder.Services.AddScoped<IVaiTroService, VaiTroService>();
builder.Services.AddScoped<ILichSuHoatDongService, LichSuHoatDongService>();
builder.Services.AddScoped<IThongBaoService, ThongBaoService>();
builder.Services.AddScoped<ILichTrucService, LichTrucService>();

// Business Logic Services
builder.Services.AddScoped<ITrangTBService, TrangTBService>();
builder.Services.AddScoped<IMasterDataService, MasterDataService>();
builder.Services.AddScoped<ILichThucHanhService, LichThucHanhService>();
builder.Services.AddScoped<ISuaChuaService, SuaChuaService>();
builder.Services.AddScoped<IXuatNhapTonService, XuatNhapTonService>();
builder.Services.AddScoped<IBaoCaoService, BaoCaoService>();
builder.Services.AddScoped<IDanhGiaCapDoService, DanhGiaCapDoService>();

// Add HTTP Context Accessor for IP tracking
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use Session before authentication/authorization
app.UseSession();

// Use Permission Middleware
app.UsePermissionMiddleware();

app.UseAuthentication();
app.UseAuthorization();

// Configure routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard",
    defaults: new { controller = "Dashboard", action = "Index" });

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Run any pending migrations
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        // Seed default data if needed
        await SeedDefaultDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Có lỗi xảy ra khi khởi tạo database");
    }
}

app.Run();

// Method to seed default data
static async Task SeedDefaultDataAsync(AppDbContext context)
{
    try
    {
        // Seed default admin user if not exists
        if (!await context.NguoiDung.AnyAsync())
        {
            var salt = Guid.NewGuid().ToString();
            var password = "Admin@123";
            var passwordHash = System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes("admin123" + salt));
            var hashString = Convert.ToBase64String(passwordHash);

            var adminUser = new QLVatTuPhongThiNghiem.Models.Entities.NguoiDung
            {
                MaNguoiDung = 1,
                TenDangNhap = "admin",
                MatKhau = password,
                MatKhauHash = hashString,
                Salt = salt,
                Email = "admin@lab.com",
                HoTen = "Quản trị viên",
                TrangThaiTaiKhoan = true,
                NgayTao = DateTime.Now
            };

            context.NguoiDung.Add(adminUser);
        }

        // Seed VaiTro (lưu trước để lấy ID sinh ra)
        if (!await context.VaiTro.AnyAsync())
        {
            var roles = new[]
            {
                new QLVatTuPhongThiNghiem.Models.Entities.VaiTro { TenVaiTro = "Admin", MoTa = "Quản trị viên hệ thống" },
                new QLVatTuPhongThiNghiem.Models.Entities.VaiTro { TenVaiTro = "QuanLy", MoTa = "Quản lý phòng thí nghiệm" },
                new QLVatTuPhongThiNghiem.Models.Entities.VaiTro { TenVaiTro = "NhanVien", MoTa = "Nhân viên sử dụng thiết bị" },
                new QLVatTuPhongThiNghiem.Models.Entities.VaiTro { TenVaiTro = "SinhVien", MoTa = "Sinh viên thực hành" }
            };

            context.VaiTro.AddRange(roles);
            await context.SaveChangesAsync(); // 👈 Quan trọng: lưu để có MaVaiTro
        }

        // Assign admin role to admin user
        if (!await context.NguoiDungVaiTro.AnyAsync())
        {
            // Lấy ID của vai trò Admin
            var adminRoleId = await context.VaiTro
                .Where(v => v.TenVaiTro == "Admin")
                .Select(v => v.MaVaiTro)
                .FirstOrDefaultAsync();

            context.NguoiDungVaiTro.Add(new QLVatTuPhongThiNghiem.Models.Entities.NguoiDungVaiTro
            {
                MaNguoiDung = 1,
                MaVaiTro = adminRoleId,
                NgayCapQuyen = DateTime.Now,
                TrangThai = true
            });
        }

        // Phòng máy mẫu
        if (!await context.PhongMay.AnyAsync())
        {
            var phongMayList = new[]
            {
                new QLVatTuPhongThiNghiem.Models.Entities.PhongMay { TenPhongMay = "Phòng máy 1" },
                new QLVatTuPhongThiNghiem.Models.Entities.PhongMay { TenPhongMay = "Phòng máy 2" },
                new QLVatTuPhongThiNghiem.Models.Entities.PhongMay { TenPhongMay = "Phòng thí nghiệm A" },
                new QLVatTuPhongThiNghiem.Models.Entities.PhongMay { TenPhongMay = "Phòng thí nghiệm B" }
            };
            context.PhongMay.AddRange(phongMayList);
        }

        // Loại thiết bị mẫu
        if (!await context.Loai.AnyAsync())
        {
            var loaiList = new[]
            {
                new QLVatTuPhongThiNghiem.Models.Entities.Loai { MaLoai = 1, TenLoai = "Máy tính" },
                new QLVatTuPhongThiNghiem.Models.Entities.Loai { MaLoai = 2, TenLoai = "Máy chiếu" },
                new QLVatTuPhongThiNghiem.Models.Entities.Loai { MaLoai = 3, TenLoai = "Máy in" },
                new QLVatTuPhongThiNghiem.Models.Entities.Loai { MaLoai = 4, TenLoai = "Thiết bị mạng" },
                new QLVatTuPhongThiNghiem.Models.Entities.Loai { MaLoai = 5, TenLoai = "Thiết bị thí nghiệm" }
            };
            context.Loai.AddRange(loaiList);
        }

        // Thương hiệu mẫu
        if (!await context.ThuongHieu.AnyAsync())
        {
            var thuongHieuList = new[]
            {
                new QLVatTuPhongThiNghiem.Models.Entities.ThuongHieu { TenThuongHieu = "Dell" },
                new QLVatTuPhongThiNghiem.Models.Entities.ThuongHieu { TenThuongHieu = "HP" },
                new QLVatTuPhongThiNghiem.Models.Entities.ThuongHieu { TenThuongHieu = "Asus" },
                new QLVatTuPhongThiNghiem.Models.Entities.ThuongHieu { TenThuongHieu = "Acer" },
                new QLVatTuPhongThiNghiem.Models.Entities.ThuongHieu { TenThuongHieu = "Canon" },
                new QLVatTuPhongThiNghiem.Models.Entities.ThuongHieu { TenThuongHieu = "Epson" }
            };
            context.ThuongHieu.AddRange(thuongHieuList);
        }

        // Nhân viên mẫu
        if (!await context.NhanVien.AnyAsync())
        {
            var nhanVienList = new[]
            {
                new QLVatTuPhongThiNghiem.Models.Entities.NhanVien
                {
                    MaNV = 1,
                    HoTen = "Nguyễn Văn An",
                    SoDienThoai = "0987654321",
                    Email = "an.nguyen@lab.com",
                    ChucVu = "Kỹ thuật viên",
                    NgayVaoLam = DateTime.Now.AddYears(-2)
                },
                new QLVatTuPhongThiNghiem.Models.Entities.NhanVien
                {
                    MaNV = 2,
                    HoTen = "Trần Thị Bình",
                    SoDienThoai = "0976543210",
                    Email = "binh.tran@lab.com",
                    ChucVu = "Trưởng phòng",
                    NgayVaoLam = DateTime.Now.AddYears(-3)
                }
            };
            context.NhanVien.AddRange(nhanVienList);
        }

        // Cuối cùng, lưu tất cả dữ liệu
        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding data: {ex.Message}");
    }
}
