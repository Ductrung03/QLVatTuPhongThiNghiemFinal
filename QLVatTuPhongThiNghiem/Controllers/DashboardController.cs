using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ITrangTBService _trangTBService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ITrangTBService trangTBService, ILogger<DashboardController> logger)
        {
            _trangTBService = trangTBService;
            _logger = logger;
        }

        // ✅ Ensure Index action exists and is public
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("🏠 Dashboard Index called");
            _logger.LogInformation($"🔍 Request path: {Request.Path}");
            _logger.LogInformation($"🔍 Route values: {string.Join(", ", Request.RouteValues?.Select(kv => $"{kv.Key}={kv.Value}") ?? new string[0])}");

            // Check if user is logged in
            var currentUser = HttpContext.Session.GetInt32("MaNguoiDung");
            _logger.LogInformation($"🔍 Current user: {currentUser}");

            if (!currentUser.HasValue)
            {
                _logger.LogWarning("❌ User not logged in, redirecting to login");
                return RedirectToAction("Login", "Auth");
            }

            var viewModel = new DashboardViewModel();

            try
            {
                var allEquipments = await _trangTBService.GetAllAsync();

                viewModel.TongThietBi = allEquipments.Count();
                viewModel.ThietBiTot = allEquipments.Count(x => x.TinhTrang == "Tốt");
                viewModel.ThietBiHong = allEquipments.Count(x => x.TinhTrang == "Hỏng");
                viewModel.ThietBiDangSua = allEquipments.Count(x => x.TinhTrang == "Đang sửa chữa");
                viewModel.TongGiaTriThietBi = allEquipments.Sum(x => x.GiaTien ?? 0);

                _logger.LogInformation($"✅ Dashboard data loaded: {viewModel.TongThietBi} devices");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error loading dashboard data");
                TempData["ErrorMessage"] = $"Lỗi khi tải dữ liệu: {ex.Message}";
            }

            return View(viewModel);
        }

        // ✅ Add a test action to verify routing
        [HttpGet]
        public IActionResult Test()
        {
            _logger.LogInformation("🧪 Dashboard Test action called");
            return Json(new { message = "Dashboard controller is working", timestamp = DateTime.Now });
        }
    }
}