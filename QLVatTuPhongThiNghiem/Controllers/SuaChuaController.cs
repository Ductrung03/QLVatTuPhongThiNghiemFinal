// File: QLVatTuPhongThiNghiem/Controllers/SuaChuaController.cs
// Vị trí: QLVatTuPhongThiNghiem/Controllers/SuaChuaController.cs
// Thay thế file cũ - Bổ sung thêm các action CRUD đầy đủ

using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;



namespace QLVatTuPhongThiNghiem.Controllers
{
    public class SuaChuaController : Controller
    {
        private readonly ISuaChuaService _suaChuaService;
        private readonly ITrangTBService _trangTBService;
        private readonly INguoiDungService _nguoiDungService;

        public SuaChuaController(
            ISuaChuaService suaChuaService,
            ITrangTBService trangTBService,
            INguoiDungService nguoiDungService)
        {
            _suaChuaService = suaChuaService;
            _trangTBService = trangTBService;
            _nguoiDungService = nguoiDungService;
        }

        // READ: Danh sách tất cả phiếu sửa chữa
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var suaChuaList = await _suaChuaService.GetAllAsync();
            return View(suaChuaList);
        }

        // CREATE: Hiển thị form tạo phiếu sửa chữa - GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await LoadMasterData();
            return View(new SuaChuaViewModel());
        }

        // CREATE: Xử lý tạo phiếu sửa chữa - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SuaChuaViewModel model)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                await LoadMasterData();
                return View(model);
            }

            model.NguoiThucHien = maNguoiDung.Value;
            model.NgayBatDau = DateTime.Now;
            model.TrangThai = "Chờ xử lý";

            var (success, maSuaChua, message) = await _suaChuaService.TaoPhieuSuaChuaAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Details", new { id = maSuaChua });
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadMasterData();
                return View(model);
            }
        }

        // READ: Xem chi tiết phiếu sửa chữa
        public async Task<IActionResult> Details(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await _suaChuaService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu sửa chữa";
                return RedirectToAction("Index");
            }

            // Kiểm tra quyền xem (người tạo hoặc admin)
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "SuaChua_Read");
            if (model.NguoiThucHien != maNguoiDung.Value && !isAdmin)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền xem phiếu này";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // UPDATE: Hiển thị form sửa phiếu sửa chữa - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await _suaChuaService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu sửa chữa";
                return RedirectToAction("Index");
            }

            // Kiểm tra quyền sửa
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "SuaChua_Update");
            var canEdit = await _suaChuaService.KiemTraQuyenChinhSuaAsync(id, maNguoiDung.Value, isAdmin);

            if (!canEdit)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền sửa phiếu này hoặc phiếu đã hoàn thành";
                return RedirectToAction("Details", new { id });
            }

            await LoadMasterData();
            return View(model);
        }

        // UPDATE: Xử lý sửa phiếu sửa chữa - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SuaChuaViewModel model)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                await LoadMasterData();
                return View(model);
            }

            // Kiểm tra quyền sửa
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "SuaChua_Update");
            var canEdit = await _suaChuaService.KiemTraQuyenChinhSuaAsync(model.MaSuaChua, maNguoiDung.Value, isAdmin);

            if (!canEdit)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền sửa phiếu này";
                return RedirectToAction("Details", new { id = model.MaSuaChua });
            }

            var (success, message) = await _suaChuaService.CapNhatPhieuSuaChuaAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Details", new { id = model.MaSuaChua });
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadMasterData();
                return View(model);
            }
        }

        // DELETE: Xóa phiếu sửa chữa - POST
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            // Kiểm tra quyền xóa
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "SuaChua_Delete");
            var canEdit = await _suaChuaService.KiemTraQuyenChinhSuaAsync(id, maNguoiDung.Value, isAdmin);

            if (!canEdit)
            {
                return Json(new { success = false, message = "Bạn không có quyền xóa phiếu này" });
            }

            var (success, message) = await _suaChuaService.XoaPhieuSuaChuaAsync(id);
            return Json(new { success, message });
        }

        // UPDATE: Hoàn thành sửa chữa - POST
        [HttpPost]
        public async Task<IActionResult> Complete(int id, decimal chiPhi, string tinhTrangMoi)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            // Kiểm tra quyền hoàn thành
            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "SuaChua_Update"))
            {
                return Json(new { success = false, message = "Bạn không có quyền hoàn thành sửa chữa" });
            }

            var (success, message) = await _suaChuaService.HoanThanhSuaChuaAsync(id, chiPhi, tinhTrangMoi);
            return Json(new { success, message });
        }

        // UPDATE: Hủy phiếu sửa chữa - POST
        [HttpPost]
        public async Task<IActionResult> Cancel(int id, string lyDo)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            // Kiểm tra quyền hủy
            var isAdmin = await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "SuaChua_Update");
            var canEdit = await _suaChuaService.KiemTraQuyenChinhSuaAsync(id, maNguoiDung.Value, isAdmin);

            if (!canEdit)
            {
                return Json(new { success = false, message = "Bạn không có quyền hủy phiếu này" });
            }

            var (success, message) = await _suaChuaService.HuyPhieuSuaChuaAsync(id, lyDo);
            return Json(new { success, message });
        }

        // READ: Phiếu sửa chữa theo trạng thái
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

            var suaChuaList = await _suaChuaService.GetByTrangThaiAsync(status);
            ViewBag.Status = status;
            ViewBag.StatusDisplayName = GetStatusDisplayName(status);

            return View("Index", suaChuaList);
        }

        // READ: Phiếu sửa chữa đang thực hiện
        public async Task<IActionResult> InProgress()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var suaChuaList = await _suaChuaService.GetByTrangThaiAsync("Đang sửa chữa");
            ViewBag.Title = "Phiếu sửa chữa đang thực hiện";
            return View("Index", suaChuaList);
        }

        // READ: Phiếu sửa chữa của tôi
        public async Task<IActionResult> MyRepairs()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var suaChuaList = await _suaChuaService.GetByNguoiThucHienAsync(maNguoiDung.Value);
            ViewBag.Title = "Phiếu sửa chữa của tôi";
            return View("Index", suaChuaList);
        }

        // READ: Lịch sử sửa chữa theo thiết bị
        public async Task<IActionResult> ByEquipment(int equipmentId)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var suaChuaList = await _suaChuaService.GetByThietBiAsync(equipmentId);
            ViewBag.Title = $"Lịch sử sửa chữa thiết bị ID: {equipmentId}";
            return View("Index", suaChuaList);
        }

        // READ: Báo cáo sửa chữa
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

            var stats = await _suaChuaService.ThongKeSuaChuaAsync(fromDate, toDate);

            ViewBag.FromDate = fromDate ?? DateTime.Now.AddMonths(-1);
            ViewBag.ToDate = toDate ?? DateTime.Now;

            return View(stats);
        }

        // READ: Tìm kiếm phiếu sửa chữa
        [HttpGet]
        public async Task<IActionResult> Search(string keyword, string status, DateTime? fromDate, DateTime? toDate)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var allRepairs = await _suaChuaService.GetAllAsync();

            // Lọc theo từ khóa
            if (!string.IsNullOrEmpty(keyword))
            {
                allRepairs = allRepairs.Where(r =>
                    r.TenThietBi.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    r.MoTa.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    r.LoaiSuaChua.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(status))
            {
                allRepairs = allRepairs.Where(r => r.TrangThai == status);
            }

            // Lọc theo ngày
            if (fromDate.HasValue)
            {
                allRepairs = allRepairs.Where(r => r.NgayBatDau.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                allRepairs = allRepairs.Where(r => r.NgayBatDau.Date <= toDate.Value.Date);
            }

            ViewBag.Keyword = keyword;
            ViewBag.Status = status;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Title = "Kết quả tìm kiếm";

            return View("Index", allRepairs);
        }

        #region Private Methods
        private async Task LoadMasterData()
        {
            var thietBiList = await _trangTBService.GetAllAsync();
            ViewBag.ThietBiList = thietBiList.Where(t => t.TinhTrang != "Đang sửa chữa");

            // Danh sách loại sửa chữa
            ViewBag.LoaiSuaChuaList = new[]
            {
                "Bảo trì định kỳ",
                "Sửa chữa khẩn cấp",
                "Thay thế linh kiện",
                "Nâng cấp",
                "Bảo hành",
                "Sửa chữa chung"
            };

            // Danh sách tình trạng thiết bị
            ViewBag.TinhTrangList = new[]
            {
                "Tốt",
                "Khá tốt",
                "Trung bình",
                "Hỏng",
                "Hư hỏng nặng",
                "Cần thay thế"
            };

            // Danh sách trạng thái phiếu
            ViewBag.TrangThaiList = new[]
            {
                "Chờ xử lý",
                "Đang sửa chữa",
                "Hoàn thành",
                "Đã hủy"
            };
        }

        private string GetStatusDisplayName(string status)
        {
            return status switch
            {
                "Chờ xử lý" => "Chờ xử lý",
                "Đang sửa chữa" => "Đang sửa chữa",
                "Hoàn thành" => "Hoàn thành",
                "Đã hủy" => "Đã hủy",
                _ => status
            };
        }
        #endregion
    }
}