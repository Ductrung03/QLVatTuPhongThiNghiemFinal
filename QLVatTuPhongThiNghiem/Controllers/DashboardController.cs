using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ITrangTBService _trangTBService;

        public DashboardController(ITrangTBService trangTBService)
        {
            _trangTBService = trangTBService;
        }

        public async Task<IActionResult> Index()
        {
            // Check if user is logged in
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
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
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải dữ liệu: {ex.Message}";
            }

            return View(viewModel);
        }
    }
}
