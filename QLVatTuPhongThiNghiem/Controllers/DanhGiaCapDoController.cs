using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class DanhGiaCapDoController : Controller
    {
        private readonly IDanhGiaCapDoService _danhGiaCapDoService;
        private readonly ITrangTBService _trangTBService;

        public DanhGiaCapDoController(IDanhGiaCapDoService danhGiaCapDoService, ITrangTBService trangTBService)
        {
            _danhGiaCapDoService = danhGiaCapDoService;
            _trangTBService = trangTBService;
        }

        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var danhGiaList = await _danhGiaCapDoService.GetAllAsync();
            return View(danhGiaList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await LoadThietBiList();
            return View(new DanhGiaCapDoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(DanhGiaCapDoViewModel model)
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

            model.NguoiDanhGia = maNguoiDung.Value;
            var (success, message) = await _danhGiaCapDoService.DanhGiaCapDoAsync(model);

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

        [HttpGet]
        public async Task<IActionResult> GetCapDoThietBi(int maTTB)
        {
            var capDo = await _danhGiaCapDoService.GetCapDoThietBiAsync(maTTB);
            return Json(capDo);
        }

        private async Task LoadThietBiList()
        {
            var thietBiList = await _trangTBService.GetAllAsync();
            ViewBag.ThietBiList = thietBiList;
        }
    }
}
