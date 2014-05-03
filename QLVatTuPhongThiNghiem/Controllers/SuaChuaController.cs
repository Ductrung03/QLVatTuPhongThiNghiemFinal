using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class SuaChuaController : Controller
    {
        private readonly ISuaChuaService _suaChuaService;
        private readonly ITrangTBService _trangTBService;

        public SuaChuaController(ISuaChuaService suaChuaService, ITrangTBService trangTBService)
        {
            _suaChuaService = suaChuaService;
            _trangTBService = trangTBService;
        }

        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var suaChuaList = await _suaChuaService.GetAllAsync();
            return View(suaChuaList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await LoadThietBiList();
            return View(new SuaChuaViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SuaChuaViewModel model)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                await LoadThietBiList();
                return View(model);
            }

            model.NguoiThucHien = maNguoiDung.Value;
            var (success, maSuaChua, message) = await _suaChuaService.TaoPhieuSuaChuaAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadThietBiList();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int id, decimal chiPhi, string tinhTrangMoi)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            var (success, message) = await _suaChuaService.HoanThanhSuaChuaAsync(id, chiPhi, tinhTrangMoi);
            return Json(new { success, message });
        }

        public async Task<IActionResult> InProgress()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var suaChuaList = await _suaChuaService.GetByTrangThaiAsync("Đang sửa chữa");
            return View(suaChuaList);
        }

        private async Task LoadThietBiList()
        {
            var thietBiList = await _trangTBService.GetAllAsync();
            ViewBag.ThietBiList = thietBiList;
        }
    }
}

