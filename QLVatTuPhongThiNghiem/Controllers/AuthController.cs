using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class AuthController : Controller
    {
        private readonly INguoiDungService _nguoiDungService;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            INguoiDungService nguoiDungService,
            ILichSuHoatDongService lichSuHoatDongService,
            ILogger<AuthController> logger)
        {
            _nguoiDungService = nguoiDungService;
            _lichSuHoatDongService = lichSuHoatDongService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("🔍 GET Login called - Checking session...");

            // Nếu đã đăng nhập thì chuyển về Dashboard
            var currentUser = HttpContext.Session.GetInt32("MaNguoiDung");
            _logger.LogInformation($"🔍 Current session user: {currentUser}");

            if (currentUser.HasValue)
            {
                _logger.LogInformation("✅ User already logged in, redirecting to Dashboard");
                return RedirectToAction("Index", "Dashboard");
            }

            _logger.LogInformation("📝 Showing login form");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            _logger.LogInformation("🚀 POST Login called!");
            _logger.LogInformation($"🔍 Username: {model?.TenDangNhap}");
            _logger.LogInformation($"🔍 Password length: {model?.MatKhau?.Length}");
            _logger.LogInformation($"🔍 ModelState valid: {ModelState.IsValid}");
            _logger.LogInformation($"🔍 Request method: {Request.Method}");
            _logger.LogInformation($"🔍 Request path: {Request.Path}");

            // Debug ModelState errors
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("❌ ModelState is invalid:");
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        _logger.LogWarning($"   - {error.Key}: {err.ErrorMessage}");
                    }
                }
                return View(model);
            }

            try
            {
                // Lấy thông tin IP và User Agent
                var diaChiIP = GetClientIPAddress();
                var userAgent = Request.Headers["User-Agent"].ToString();

                _logger.LogInformation($"🔍 IP: {diaChiIP}, UserAgent: {userAgent}");
                _logger.LogInformation("🔄 Attempting login...");

                var (success, maNguoiDung, message) = await _nguoiDungService.DangNhapBaoMatAsync(model, diaChiIP, userAgent);

                _logger.LogInformation($"🔍 Login result: Success={success}, UserID={maNguoiDung}, Message={message}");

                if (success)
                {
                    _logger.LogInformation("✅ Login successful, setting session...");

                    // Lưu thông tin vào Session
                    HttpContext.Session.SetInt32("MaNguoiDung", maNguoiDung);
                    HttpContext.Session.SetString("TenDangNhap", model.TenDangNhap);

                    _logger.LogInformation($"✅ Session set: MaNguoiDung={maNguoiDung}, TenDangNhap={model.TenDangNhap}");

                    // Lưu quyền hạn vào Session
                    var quyenHan = await _nguoiDungService.GetQuyenHanAsync(maNguoiDung);
                    HttpContext.Session.SetString("QuyenHan", string.Join(",", quyenHan));

                    _logger.LogInformation($"✅ Permissions set: {string.Join(",", quyenHan)}");

                    // Lấy thông tin người dùng
                    var nguoiDung = await _nguoiDungService.GetByIdAsync(maNguoiDung);
                    if (nguoiDung != null)
                    {
                        HttpContext.Session.SetString("HoTen", nguoiDung.HoTen ?? model.TenDangNhap);
                        _logger.LogInformation($"✅ Full name set: {nguoiDung.HoTen}");
                    }

                    TempData["SuccessMessage"] = message;
                    _logger.LogInformation("🏠 Redirecting to Dashboard...");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    _logger.LogWarning($"❌ Login failed: {message}");

                    ModelState.AddModelError("", message);

                    // Ghi log đăng nhập thất bại
                    await _lichSuHoatDongService.GhiLichSuAsync(0, "Đăng nhập thất bại", "Bảo mật",
                        $"Tên đăng nhập: {model.TenDangNhap} - Lỗi: {message}", diaChiIP, userAgent);

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error during login for user: {Username}", model.TenDangNhap);

                ModelState.AddModelError("", "Có lỗi xảy ra trong quá trình đăng nhập. Vui lòng thử lại.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung.HasValue)
            {
                // Ghi lịch sử đăng xuất
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Đăng xuất", "Hệ thống", "Đăng xuất thành công");
            }

            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Đăng xuất thành công";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private string GetClientIPAddress()
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Kiểm tra header X-Forwarded-For (trong trường hợp có proxy)
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
            }

            // Kiểm tra header X-Real-IP
            if (string.IsNullOrEmpty(ipAddress) && Request.Headers.ContainsKey("X-Real-IP"))
            {
                ipAddress = Request.Headers["X-Real-IP"].FirstOrDefault();
            }

            return ipAddress ?? "Unknown";
        }
    }
}