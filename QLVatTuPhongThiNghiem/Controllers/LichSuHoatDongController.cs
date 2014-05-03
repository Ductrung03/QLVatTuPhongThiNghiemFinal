using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class LichSuHoatDongController : Controller
    {
        private readonly ILichSuHoatDongService _lichSuHoatDongService;
        private readonly INguoiDungService _nguoiDungService;

        public LichSuHoatDongController(ILichSuHoatDongService lichSuHoatDongService, INguoiDungService nguoiDungService)
        {
            _lichSuHoatDongService = lichSuHoatDongService;
            _nguoiDungService = nguoiDungService;
        }

        public async Task<IActionResult> Index(int? maNguoiDung, DateTime? tuNgay, DateTime? denNgay, int pageNumber = 1, int pageSize = 50)
        {
            var currentUserId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(currentUserId.Value, "BaoCao_Read"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền xem lịch sử hoạt động";
                return RedirectToAction("Index", "Dashboard");
            }

            if (!tuNgay.HasValue) tuNgay = DateTime.Now.AddDays(-30);
            if (!denNgay.HasValue) denNgay = DateTime.Now;

            var lichSu = await _lichSuHoatDongService.GetLichSuAsync(maNguoiDung, tuNgay, denNgay, pageNumber, pageSize);
            var tongSoBanGhi = await _lichSuHoatDongService.GetTongSoBanGhiAsync(maNguoiDung, tuNgay, denNgay);

            ViewBag.MaNguoiDung = maNguoiDung;
            ViewBag.TuNgay = tuNgay;
            ViewBag.DenNgay = denNgay;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TongSoBanGhi = tongSoBanGhi;
            ViewBag.TongSoTrang = (int)Math.Ceiling((double)tongSoBanGhi / pageSize);

            var nguoiDungList = await _nguoiDungService.GetAllAsync();
            ViewBag.NguoiDungList = nguoiDungList;

            return View(lichSu);
        }
    }
}