using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly INguoiDungService _nguoiDungService;
        private readonly IVaiTroService _vaiTroService;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public NguoiDungController(INguoiDungService nguoiDungService, IVaiTroService vaiTroService, ILichSuHoatDongService lichSuHoatDongService)
        {
            _nguoiDungService = nguoiDungService;
            _vaiTroService = vaiTroService;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        public async Task<IActionResult> Index()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Kiểm tra quyền
            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Read"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập chức năng này";
                return RedirectToAction("Index", "Dashboard");
            }

            var nguoiDungList = await _nguoiDungService.GetAllAsync();
            return View(nguoiDungList);
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
                TempData["ErrorMessage"] = "Bạn không có quyền tạo tài khoản";
                return RedirectToAction("Index");
            }

            await LoadVaiTroList();
            return View(new TaoTaiKhoanViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoTaiKhoanViewModel model)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Create"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền tạo tài khoản";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                await LoadVaiTroList();
                return View(model);
            }

            var (success, newMaNguoiDung, message) = await _nguoiDungService.TaoTaiKhoanAsync(model, maNguoiDung.Value);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadVaiTroList();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Update"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền cập nhật thông tin người dùng";
                return RedirectToAction("Index");
            }

            var model = await _nguoiDungService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NguoiDungViewModel model)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Update"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền cập nhật thông tin người dùng";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _nguoiDungService.CapNhatThongTinAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Cập nhật người dùng", "Quản lý người dùng",
                    $"Cập nhật thông tin người dùng: {model.TenDangNhap}");
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
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Update"))
            {
                return Json(new { success = false, message = "Bạn không có quyền thực hiện hành động này" });
            }

            var user = await _nguoiDungService.GetByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            var (success, message) = await _nguoiDungService.CapNhatTrangThaiAsync(id, !user.TrangThaiTaiKhoan);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Thay đổi trạng thái", "Quản lý người dùng",
                    $"Thay đổi trạng thái tài khoản: {user.TenDangNhap} - {(!user.TrangThaiTaiKhoan ? "Kích hoạt" : "Khóa")}");
            }

            return Json(new { success, message });
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(new DoiMatKhauViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(DoiMatKhauViewModel model)
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

            var (success, message) = await _nguoiDungService.DoiMatKhauAsync(maNguoiDung.Value, model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("", message);
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
                TempData["ErrorMessage"] = "Bạn không có quyền phân quyền";
                return RedirectToAction("Index");
            }

            var user = await _nguoiDungService.GetByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng";
                return RedirectToAction("Index");
            }

            var vaiTroList = await _vaiTroService.GetAllAsync();
            ViewBag.VaiTroList = vaiTroList;
            ViewBag.User = user;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoles(int userId, List<int> roleIds)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Update"))
            {
                return Json(new { success = false, message = "Bạn không có quyền phân quyền" });
            }

            var (success, message) = await _nguoiDungService.PhanQuyenAsync(userId, roleIds ?? new List<int>());

            if (success)
            {
                var user = await _nguoiDungService.GetByIdAsync(userId);
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Phân quyền", "Quản lý người dùng",
                    $"Phân quyền cho người dùng: {user?.TenDangNhap}");
            }

            return Json(new { success, message });
        }

        private async Task LoadVaiTroList()
        {
            var vaiTroList = await _vaiTroService.GetAllAsync();
            ViewBag.VaiTroList = vaiTroList;
        }
    }
}
