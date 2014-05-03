using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class TrangTBController : Controller
    {
        private readonly ITrangTBService _trangTBService;
        private readonly IMasterDataService _masterDataService;

        public TrangTBController(ITrangTBService trangTBService, IMasterDataService masterDataService)
        {
            _trangTBService = trangTBService;
            _masterDataService = masterDataService;
        }

        public async Task<IActionResult> Index(SearchTrangTBViewModel model)
        {
            // Check if user is logged in
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await LoadMasterData();

            var result = await _trangTBService.SearchAsync(model);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Check if user is logged in
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await LoadMasterData();

            var model = new TrangTBViewModel
            {
                NgayNhap = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrangTBViewModel model)
        {
            // Check if user is logged in
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                await LoadMasterData();
                return View(model);
            }

            var (success, maTTB, message) = await _trangTBService.CreateAsync(model, maNguoiDung.Value);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadMasterData();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Check if user is logged in
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await _trangTBService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thiết bị";
                return RedirectToAction("Index");
            }

            await LoadMasterData();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TrangTBViewModel model)
        {
            // Check if user is logged in
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                await LoadMasterData();
                return View(model);
            }

            var (success, message) = await _trangTBService.UpdateAsync(model, maNguoiDung.Value);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadMasterData();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Check if user is logged in
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            var (success, message) = await _trangTBService.DeleteAsync(id);
            return Json(new { success, message });
        }

        private async Task LoadMasterData()
        {
            ViewBag.PhongMayList = await _masterDataService.GetPhongMayAsync();
            ViewBag.LoaiList = await _masterDataService.GetLoaiAsync();
            ViewBag.ThuongHieuList = await _masterDataService.GetThuongHieuAsync();
        }
    }
}

