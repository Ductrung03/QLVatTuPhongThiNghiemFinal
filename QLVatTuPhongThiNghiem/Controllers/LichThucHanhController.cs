// File: QLVatTuPhongThiNghiem/Controllers/LichThucHanhController.cs
// Vị trí: QLVatTuPhongThiNghiem/Controllers/LichThucHanhController.cs
// Thay thế file cũ - Bổ sung thêm các action CRUD đầy đủ

using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class LichThucHanhController : Controller
    {
        private readonly ILichThucHanhService _lichThucHanhService;
        private readonly INguoiDungService _nguoiDungService;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public LichThucHanhController(
            ILichThucHanhService lichThucHanhService,
            INguoiDungService nguoiDungService,
            ILichSuHoatDongService lichSuHoatDongService)
        {
            _lichThucHanhService = lichThucHanhService;
            _nguoiDungService = nguoiDungService;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        // READ: Danh sách tất cả lịch thực hành
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var lichList = await _lichThucHanhService.GetAllAsync();
            return View(lichList);
        }

        // CREATE: Hiển thị form đăng ký lịch - GET
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
                ThoiGianKT = DateTime.Now.AddHours(3),
                TrangThai = "Chờ duyệt"
            };

            return View(model);
        }

        // CREATE: Xử lý đăng ký lịch - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            model.TrangThai = "Chờ duyệt";

            var (success, maLich, message) = await _lichThucHanhService.DangKyLichAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Details", new { id = maLich });
            }
            else
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
        }

        // READ: Xem chi tiết lịch thực hành
        public async Task<IActionResult> Details(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await _lichThucHanhService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy lịch thực hành";
                return RedirectToAction("Index");
            }

            // Kiểm tra quyền xem (chủ sở hữu hoặc admin)
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "LichThucHanh_Read");
            if (model.MaNguoiDung != maNguoiDung.Value && !isAdmin)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền xem lịch này";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // UPDATE: Hiển thị form sửa lịch - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await _lichThucHanhService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy lịch thực hành";
                return RedirectToAction("Index");
            }

            // Kiểm tra quyền sửa
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "LichThucHanh_Update");
            var canEdit = await _lichThucHanhService.KiemTraQuyenChinhSuaAsync(id, maNguoiDung.Value, isAdmin);

            if (!canEdit)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền sửa lịch này hoặc lịch đã hoàn thành";
                return RedirectToAction("Details", new { id });
            }

            return View(model);
        }

        // UPDATE: Xử lý sửa lịch - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LichThucHanhViewModel model)
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

            // Kiểm tra quyền sửa
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "LichThucHanh_Update");
            var canEdit = await _lichThucHanhService.KiemTraQuyenChinhSuaAsync(model.MaLich, maNguoiDung.Value, isAdmin);

            if (!canEdit)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền sửa lịch này";
                return RedirectToAction("Details", new { id = model.MaLich });
            }

            var (success, message) = await _lichThucHanhService.CapNhatLichAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Details", new { id = model.MaLich });
            }
            else
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
        }

        // DELETE: Xóa lịch thực hành - POST
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            // Kiểm tra quyền xóa
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "LichThucHanh_Delete");
            var canEdit = await _lichThucHanhService.KiemTraQuyenChinhSuaAsync(id, maNguoiDung.Value, isAdmin);

            if (!canEdit)
            {
                return Json(new { success = false, message = "Bạn không có quyền xóa lịch này" });
            }

            var (success, message) = await _lichThucHanhService.XoaLichAsync(id, maNguoiDung.Value);
            return Json(new { success, message });
        }

        // UPDATE: Cập nhật trạng thái - POST
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            // Kiểm tra quyền cập nhật trạng thái
            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "LichThucHanh_Update"))
            {
                return Json(new { success = false, message = "Bạn không có quyền cập nhật trạng thái" });
            }

            var (success, message) = await _lichThucHanhService.CapNhatTrangThaiAsync(id, status);
            return Json(new { success, message });
        }

        // READ: Lịch của người dùng hiện tại
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

        // READ: Lịch theo ngày - AJAX
        [HttpGet]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            try
            {
                var lichList = await _lichThucHanhService.GetByNgayAsync(date);
                var result = lichList.Select(l => new
                {
                    id = l.MaLich,
                    title = $"{l.TenNguoiDung} - {l.TrangThai}",
                    start = l.ThoiGianBD.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = l.ThoiGianKT.ToString("yyyy-MM-ddTHH:mm:ss"),
                    status = l.TrangThai,
                    user = l.TenNguoiDung,
                    canEdit = l.MaNguoiDung == HttpContext.Session.GetInt32("MaNguoiDung")
                });

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // READ: Lịch theo trạng thái
        public async Task<IActionResult> ByStatus(string status)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (string.IsNullOrEmpty(status))
            {
                return RedirectToAction("Index");
            }

            var lichList = await _lichThucHanhService.GetByTrangThaiAsync(status);
            ViewBag.Status = status;
            ViewBag.StatusDisplayName = GetStatusDisplayName(status);

            return View("Index", lichList);
        }

        // READ: Calendar view
        public async Task<IActionResult> Calendar()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        // READ: Thống kê lịch thực hành
        public async Task<IActionResult> Statistics(DateTime? fromDate, DateTime? toDate)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Kiểm tra quyền xem báo cáo
            var canViewAll = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "BaoCao_Read");

            var stats = await _lichThucHanhService.ThongKeLichThucHanhAsync(
                canViewAll ? null : maNguoiDung.Value,
                fromDate,
                toDate);

            ViewBag.FromDate = fromDate ?? DateTime.Now.AddMonths(-1);
            ViewBag.ToDate = toDate ?? DateTime.Now;
            ViewBag.CanViewAll = canViewAll;

            return View(stats);
        }

        #region Helper Methods
        private string GetStatusDisplayName(string status)
        {
            return status switch
            {
                "Chờ duyệt" => "Chờ duyệt",
                "Đã duyệt" => "Đã duyệt",
                "Đang thực hiện" => "Đang thực hiện",
                "Hoàn thành" => "Hoàn thành",
                "Đã hủy" => "Đã hủy",
                _ => status
            };
        }
        #endregion
    }
}