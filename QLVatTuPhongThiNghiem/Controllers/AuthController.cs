using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class AuthController : Controller
    {
        private readonly INguoiDungService _nguoiDungService;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public AuthController(INguoiDungService nguoiDungService, ILichSuHoatDongService lichSuHoatDongService)
        {
            _nguoiDungService = nguoiDungService;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Nếu đã đăng nhập thì chuyển về Dashboard
            if (HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Lấy thông tin IP và User Agent
            var diaChiIP = GetClientIPAddress();
            var userAgent = Request.Headers["User-Agent"].ToString();

            var (success, maNguoiDung, message) = await _nguoiDungService.DangNhapBaoMatAsync(model, diaChiIP, userAgent);

            if (success)
            {
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
                ModelState.AddModelError("", message);

                // Ghi log đăng nhập thất bại
                await _lichSuHoatDongService.GhiLichSuAsync(0, "Đăng nhập thất bại", "Bảo mật",
                    $"Tên đăng nhập: {model.TenDangNhap} - Lỗi: {message}", diaChiIP, userAgent);

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

                // Gọi stored procedure đăng xuất (nếu có)
                // await _authService.LogoutAsync(maNguoiDung.Value);
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