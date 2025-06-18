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
            _logger.LogInformation("GET Login called");

            // Nếu đã đăng nhập thì chuyển về Dashboard
            if (HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            _logger.LogInformation("POST Login called with username: {Username}", model.TenDangNhap);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid");
                return View(model);
            }

            try
            {
                // Lấy thông tin IP và User Agent
                var diaChiIP = GetClientIPAddress();
                var userAgent = Request.Headers["User-Agent"].ToString();

                _logger.LogInformation("Attempting login for user: {Username}", model.TenDangNhap);

                var (success, maNguoiDung, message) = await _nguoiDungService.DangNhapBaoMatAsync(model, diaChiIP, userAgent);

                if (success)
                {
                    _logger.LogInformation("Login successful for user: {Username}", model.TenDangNhap);

                    // Lưu thông tin vào Session
                    HttpContext.Session.SetInt32("MaNguoiDung", maNguoiDung);
                    HttpContext.Session.SetString("TenDangNhap", model.TenDangNhap);

                    // Lưu quyền hạn vào Session
                    var quyenHan = await _nguoiDungService.GetQuyenHanAsync(maNguoiDung);
                    HttpContext.Session.SetString("QuyenHan", string.Join(",", quyenHan));

                    // Lấy thông tin người dùng
                    var nguoiDung = await _nguoiDungService.GetByIdAsync(maNguoiDung);
                    if (nguoiDung != null)
                    {
                        HttpContext.Session.SetString("HoTen", nguoiDung.HoTen ?? model.TenDangNhap);
                    }

                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    _logger.LogWarning("Login failed for user: {Username}, Reason: {Message}", model.TenDangNhap, message);

                    ModelState.AddModelError("", message);

                    // Ghi log đăng nhập thất bại
                    await _lichSuHoatDongService.GhiLichSuAsync(0, "Đăng nhập thất bại", "Bảo mật",
                        $"Tên đăng nhập: {model.TenDangNhap} - Lỗi: {message}", diaChiIP, userAgent);

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Username}", model.TenDangNhap);

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