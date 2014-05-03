using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class XuatNhapTonController : Controller
    {
        private readonly IXuatNhapTonService _xuatNhapTonService;
        private readonly ITrangTBService _trangTBService;
        private readonly IMasterDataService _masterDataService;

        public XuatNhapTonController(IXuatNhapTonService xuatNhapTonService, ITrangTBService trangTBService, IMasterDataService masterDataService)
        {
            _xuatNhapTonService = xuatNhapTonService;
            _trangTBService = trangTBService;
            _masterDataService = masterDataService;
        }

        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var phieuList = await _xuatNhapTonService.GetAllAsync();
            return View(phieuList);
        }

        [HttpGet]
        public async Task<IActionResult> Nhap()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await LoadThietBiList();
            return View(new XuatNhapTonViewModel { LoaiPhieu = "NHAP" });
        }

        [HttpPost]
        public async Task<IActionResult> Nhap(XuatNhapTonViewModel model)
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

            model.NguoiTao = maNguoiDung.Value;
            var (success, message) = await _xuatNhapTonService.NhapThietBiAsync(model);

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
        public async Task<IActionResult> Xuat()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await LoadThietBiList();
            return View(new XuatNhapTonViewModel { LoaiPhieu = "XUAT" });
        }

        [HttpPost]
        public async Task<IActionResult> Xuat(XuatNhapTonViewModel model)
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

            model.NguoiTao = maNguoiDung.Value;
            var (success, message) = await _xuatNhapTonService.XuatThietBiAsync(model);

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

        public async Task<IActionResult> BaoCaoTonKho(int? maPhongMay)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var baoCao = await _xuatNhapTonService.BaoCaoTonKhoAsync(maPhongMay);
            ViewBag.PhongMayList = await _masterDataService.GetPhongMayAsync();
            ViewBag.SelectedPhongMay = maPhongMay;

            return View(baoCao);
        }

        private async Task LoadThietBiList()
        {
            var thietBiList = await _trangTBService.GetAllAsync();
            ViewBag.ThietBiList = thietBiList;
        }
    }
}

