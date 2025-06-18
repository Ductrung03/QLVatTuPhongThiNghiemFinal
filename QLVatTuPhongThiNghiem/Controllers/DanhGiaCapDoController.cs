// File: QLVatTuPhongThiNghiem/Controllers/DanhGiaCapDoController.cs
// Vị trí: QLVatTuPhongThiNghiem/Controllers/DanhGiaCapDoController.cs
// Thay thế file cũ - Bổ sung thêm các action CRUD đầy đủ

using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class DanhGiaCapDoController : Controller
    {
        private readonly IDanhGiaCapDoService _danhGiaCapDoService;
        private readonly ITrangTBService _trangTBService;
        private readonly INguoiDungService _nguoiDungService;

        public DanhGiaCapDoController(
            IDanhGiaCapDoService danhGiaCapDoService,
            ITrangTBService trangTBService,
            INguoiDungService nguoiDungService)
        {
            _danhGiaCapDoService = danhGiaCapDoService;
            _trangTBService = trangTBService;
            _nguoiDungService = nguoiDungService;
        }

        // READ: Danh sách tất cả đánh giá
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var danhGiaList = await _danhGiaCapDoService.GetAllAsync();
            return View(danhGiaList);
        }

        // CREATE: Hiển thị form đánh giá - GET
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

        // CREATE: Xử lý đánh giá - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            model.NgayDanhGia = DateTime.Now;

            var (success, message) = await _danhGiaCapDoService.DanhGiaCapDoAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Details", new { id = model.MaDanhGia });
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadThietBiList();
                return View(model);
            }
        }

        // READ: Xem chi tiết đánh giá
        public async Task<IActionResult> Details(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await _danhGiaCapDoService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đánh giá";
                return RedirectToAction("Index");
            }

            // Lấy thêm thông tin về thiết bị
            var goiYHanhDong = await _danhGiaCapDoService.GetGoiYHanhDongAsync(model.MaTTB);
            var xuHuong = await _danhGiaCapDoService.GetXuHuongDanhGiaAsync(model.MaTTB);
            var capDoTrungBinh = await _danhGiaCapDoService.GetCapDoTrungBinhThietBiAsync(model.MaTTB);

            ViewBag.GoiYHanhDong = goiYHanhDong;
            ViewBag.XuHuong = xuHuong;
            ViewBag.CapDoTrungBinh = Math.Round(capDoTrungBinh, 2);

            return View(model);
        }

        // UPDATE: Hiển thị form sửa đánh giá - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await _danhGiaCapDoService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đánh giá";
                return RedirectToAction("Index");
            }

            // Kiểm tra quyền sửa
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "DanhGia_Update");
            var canEdit = await _danhGiaCapDoService.KiemTraQuyenChinhSuaAsync(id, maNguoiDung.Value, isAdmin);

            if (!canEdit)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền sửa đánh giá này hoặc đã quá thời hạn (24h)";
                return RedirectToAction("Details", new { id });
            }

            await LoadThietBiList();
            return View(model);
        }

        // UPDATE: Xử lý sửa đánh giá - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DanhGiaCapDoViewModel model)
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

            // Kiểm tra quyền sửa
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "DanhGia_Update");
            var canEdit = await _danhGiaCapDoService.KiemTraQuyenChinhSuaAsync(model.MaDanhGia, maNguoiDung.Value, isAdmin);

            if (!canEdit)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền sửa đánh giá này";
                return RedirectToAction("Details", new { id = model.MaDanhGia });
            }

            var (success, message) = await _danhGiaCapDoService.CapNhatDanhGiaAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Details", new { id = model.MaDanhGia });
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadThietBiList();
                return View(model);
            }
        }

        // DELETE: Xóa đánh giá - POST
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            // Kiểm tra quyền xóa
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "DanhGia_Delete");
            var canEdit = await _danhGiaCapDoService.KiemTraQuyenChinhSuaAsync(id, maNguoiDung.Value, isAdmin);

            if (!canEdit)
            {
                return Json(new { success = false, message = "Bạn không có quyền xóa đánh giá này" });
            }

            var (success, message) = await _danhGiaCapDoService.XoaDanhGiaAsync(id);
            return Json(new { success, message });
        }

        // READ: Lấy cấp độ thiết bị - AJAX
        [HttpGet]
        public async Task<IActionResult> GetCapDoThietBi(int maTTB)
        {
            try
            {
                var capDo = await _danhGiaCapDoService.GetCapDoThietBiAsync(maTTB);
                var capDoTrungBinh = await _danhGiaCapDoService.GetCapDoTrungBinhThietBiAsync(maTTB);
                var xuHuong = await _danhGiaCapDoService.GetXuHuongDanhGiaAsync(maTTB);

                return Json(new
                {
                    success = true,
                    capDo = capDo?.CapDo ?? 0,
                    capDoTrungBinh = Math.Round(capDoTrungBinh, 2),
                    xuHuong = xuHuong
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // READ: Kiểm tra có thể đánh giá không - AJAX
        [HttpGet]
        public async Task<IActionResult> CheckCanRate(int maTTB)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            try
            {
                var (canRate, message) = await _danhGiaCapDoService.KiemTraCoTheDanhGiaAsync(maTTB, maNguoiDung.Value);
                return Json(new { success = true, canRate, message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // READ: Đánh giá theo thiết bị
        public async Task<IActionResult> ByEquipment(int equipmentId)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var danhGiaList = await _danhGiaCapDoService.GetByThietBiAsync(equipmentId);
            ViewBag.Title = $"Lịch sử đánh giá thiết bị ID: {equipmentId}";
            ViewBag.EquipmentId = equipmentId;

            // Thông tin bổ sung
            var capDoTrungBinh = await _danhGiaCapDoService.GetCapDoTrungBinhThietBiAsync(equipmentId);
            var xuHuong = await _danhGiaCapDoService.GetXuHuongDanhGiaAsync(equipmentId);
            var goiYHanhDong = await _danhGiaCapDoService.GetGoiYHanhDongAsync(equipmentId);

            ViewBag.CapDoTrungBinh = Math.Round(capDoTrungBinh, 2);
            ViewBag.XuHuong = xuHuong;
            ViewBag.GoiYHanhDong = goiYHanhDong;

            return View("Index", danhGiaList);
        }

        // READ: Đánh giá của tôi
        public async Task<IActionResult> MyRatings()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var danhGiaList = await _danhGiaCapDoService.GetByNguoiDanhGiaAsync(maNguoiDung.Value);
            ViewBag.Title = "Đánh giá của tôi";
            return View("Index", danhGiaList);
        }

        // READ: Đánh giá theo cấp độ
        public async Task<IActionResult> ByLevel(int level)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (level < 1 || level > 5)
            {
                TempData["ErrorMessage"] = "Cấp độ không hợp lệ";
                return RedirectToAction("Index");
            }

            var danhGiaList = await _danhGiaCapDoService.GetByCapDoAsync(level);
            ViewBag.Title = $"Đánh giá cấp độ {level}";
            ViewBag.Level = level;

            return View("Index", danhGiaList);
        }

        // READ: Báo cáo thống kê đánh giá
        public async Task<IActionResult> Report(DateTime? fromDate, DateTime? toDate)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Kiểm tra quyền xem báo cáo
            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "BaoCao_Read"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền xem báo cáo";
                return RedirectToAction("Index");
            }

            var stats = await _danhGiaCapDoService.ThongKeDanhGiaAsync(fromDate, toDate);
            var topThietBiTot = await _danhGiaCapDoService.GetTopThietBiTheoCapDoAsync(true, 10);
            var topThietBiKem = await _danhGiaCapDoService.GetTopThietBiTheoCapDoAsync(false, 10);

            ViewBag.FromDate = fromDate ?? DateTime.Now.AddMonths(-1);
            ViewBag.ToDate = toDate ?? DateTime.Now;
            ViewBag.TopThietBiTot = topThietBiTot;
            ViewBag.TopThietBiKem = topThietBiKem;

            return View(stats);
        }

        // READ: Top thiết bị theo cấp độ
        public async Task<IActionResult> TopEquipments(bool isDescending = true)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var topThietBi = await _danhGiaCapDoService.GetTopThietBiTheoCapDoAsync(isDescending, 20);
            ViewBag.Title = isDescending ? "Top thiết bị chất lượng cao" : "Top thiết bị cần cải thiện";
            ViewBag.IsDescending = isDescending;

            return View(topThietBi);
        }

        // READ: Tìm kiếm đánh giá
        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int? level, DateTime? fromDate, DateTime? toDate)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var allRatings = await _danhGiaCapDoService.GetAllAsync();

            // Lọc theo từ khóa
            if (!string.IsNullOrEmpty(keyword))
            {
                allRatings = allRatings.Where(r =>
                    r.TenThietBi.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    r.TenNguoiDanhGia.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    (r.GhiChu != null && r.GhiChu.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
            }

            // Lọc theo cấp độ
            if (level.HasValue && level >= 1 && level <= 5)
            {
                allRatings = allRatings.Where(r => r.CapDo == level.Value);
            }

            // Lọc theo ngày
            if (fromDate.HasValue)
            {
                allRatings = allRatings.Where(r => r.NgayDanhGia.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                allRatings = allRatings.Where(r => r.NgayDanhGia.Date <= toDate.Value.Date);
            }

            ViewBag.Keyword = keyword;
            ViewBag.Level = level;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Title = "Kết quả tìm kiếm đánh giá";

            return View("Index", allRatings);
        }

        #region Private Methods
        private async Task LoadThietBiList()
        {
            var thietBiList = await _trangTBService.GetAllAsync();
            ViewBag.ThietBiList = thietBiList;

            // Danh sách cấp độ
            ViewBag.CapDoList = new[]
            {
                new { Value = 1, Text = "1 - Rất kém", Description = "Thiết bị hư hỏng nặng, không sử dụng được" },
                new { Value = 2, Text = "2 - Kém", Description = "Thiết bị hoạt động không ổn định, cần sửa chữa" },
                new { Value = 3, Text = "3 - Trung bình", Description = "Thiết bị hoạt động cơ bản, cần bảo trì" },
                new { Value = 4, Text = "4 - Tốt", Description = "Thiết bị hoạt động tốt, ít vấn đề" },
                new { Value = 5, Text = "5 - Rất tốt", Description = "Thiết bị hoạt động hoàn hảo, không có vấn đề" }
            };
        }
        #endregion
    }
}