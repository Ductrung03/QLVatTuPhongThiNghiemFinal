using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class LichThucHanhController : Controller
    {
        private readonly ILichThucHanhService _lichThucHanhService;

        public LichThucHanhController(ILichThucHanhService lichThucHanhService)
        {
            _lichThucHanhService = lichThucHanhService;
        }

        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var lichList = await _lichThucHanhService.GetAllAsync();
            return View(lichList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new LichThucHanhViewModel
            {
                ThoiGianBD = DateTime.Now.AddHours(1),
                ThoiGianKT = DateTime.Now.AddHours(3)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LichThucHanhViewModel model)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.MaNguoiDung = maNguoiDung.Value;
            var (success, maLich, message) = await _lichThucHanhService.DangKyLichAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            var (success, message) = await _lichThucHanhService.CapNhatTrangThaiAsync(id, status);
            return Json(new { success, message });
        }

        public async Task<IActionResult> MySchedule()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var lichList = await _lichThucHanhService.GetByUserAsync(maNguoiDung.Value);
            return View(lichList);
        }
    }
}
