using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class VaiTroController : Controller
    {
        private readonly IVaiTroService _vaiTroService;
        private readonly INguoiDungService _nguoiDungService;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public VaiTroController(IVaiTroService vaiTroService, INguoiDungService nguoiDungService, ILichSuHoatDongService lichSuHoatDongService)
        {
            _vaiTroService = vaiTroService;
            _nguoiDungService = nguoiDungService;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        public async Task<IActionResult> Index()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Read"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập chức năng này";
                return RedirectToAction("Index", "Dashboard");
            }

            var vaiTroList = await _vaiTroService.GetAllAsync();
            return View(vaiTroList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Create"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền tạo vai trò";
                return RedirectToAction("Index");
            }

            await LoadQuyenHanList();
            return View(new VaiTroViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(VaiTroViewModel model)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Create"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền tạo vai trò";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                await LoadQuyenHanList();
                return View(model);
            }

            var (success, message) = await _vaiTroService.CreateAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Tạo vai trò", "Quản lý vai trò",
                    $"Tạo vai trò: {model.TenVaiTro}");
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadQuyenHanList();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Permissions(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Update"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền cập nhật quyền hạn";
                return RedirectToAction("Index");
            }

            var vaiTro = await _vaiTroService.GetByIdAsync(id);
            if (vaiTro == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy vai trò";
                return RedirectToAction("Index");
            }

            var allPermissions = await _vaiTroService.GetAllQuyenHanAsync();
            var rolePermissions = await _vaiTroService.GetQuyenHanByVaiTroAsync(id);

            foreach (var permission in allPermissions)
            {
                permission.DuocChon = rolePermissions.Any(rp => rp.MaQuyen == permission.MaQuyen);
            }

            ViewBag.VaiTro = vaiTro;
            return View(allPermissions);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePermissions(int roleId, List<int> permissionIds)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Update"))
            {
                return Json(new { success = false, message = "Bạn không có quyền cập nhật quyền hạn" });
            }

            var (success, message) = await _vaiTroService.CapNhatQuyenHanAsync(roleId, permissionIds ?? new List<int>());

            if (success)
            {
                var vaiTro = await _vaiTroService.GetByIdAsync(roleId);
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Cập nhật quyền hạn", "Quản lý vai trò",
                    $"Cập nhật quyền hạn cho vai trò: {vaiTro?.TenVaiTro}");
            }

            return Json(new { success, message });
        }

        private async Task LoadQuyenHanList()
        {
            var quyenHanList = await _vaiTroService.GetAllQuyenHanAsync();
            ViewBag.QuyenHanList = quyenHanList;
        }
    }
}
