using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class BaoCaoController : Controller
    {
        private readonly IBaoCaoService _baoCaoService;
        private readonly IMasterDataService _masterDataService;

        public BaoCaoController(IBaoCaoService baoCaoService, IMasterDataService masterDataService)
        {
            _baoCaoService = baoCaoService;
            _masterDataService = masterDataService;
        }

        public async Task<IActionResult> ThongKeTheoPhong()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var thongKe = await _baoCaoService.ThongKeThietBiTheoPhongAsync();
            return View(thongKe);
        }

        public async Task<IActionResult> SuDungTheoThang(int nam = 0)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (nam == 0) nam = DateTime.Now.Year;

            var thongKe = await _baoCaoService.ThongKeSuDungTheoThangAsync(nam);
            ViewBag.Nam = nam;
            return View(thongKe);
        }

        public async Task<IActionResult> ChiPhiSuaChua(DateTime? tuNgay, DateTime? denNgay)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!tuNgay.HasValue) tuNgay = DateTime.Now.AddMonths(-1);
            if (!denNgay.HasValue) denNgay = DateTime.Now;

            var baoCao = await _baoCaoService.BaoCaoChiPhiSuaChuaAsync(tuNgay.Value, denNgay.Value);
            ViewBag.TuNgay = tuNgay.Value;
            ViewBag.DenNgay = denNgay.Value;

            return View(baoCao);
        }

        public async Task<IActionResult> DanhGiaCapDo()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var thongKe = await _baoCaoService.ThongKeDanhGiaCapDoAsync();
            return View(thongKe);
        }
    }
}

