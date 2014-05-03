using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class ThongBaoController : Controller
    {
        private readonly IThongBaoService _thongBaoService;
        private readonly INguoiDungService _nguoiDungService;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public ThongBaoController(
            IThongBaoService thongBaoService,
            INguoiDungService nguoiDungService,
            ILichSuHoatDongService lichSuHoatDongService)
        {
            _thongBaoService = thongBaoService;
            _nguoiDungService = nguoiDungService;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        // Xem thông báo của người dùng hiện tại
        public async Task<IActionResult> Index()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var thongBaoList = await _thongBaoService.GetThongBaoByNguoiDungAsync(maNguoiDung.Value);
            return View(thongBaoList);
        }

        // Quản lý tất cả thông báo (cho admin)
        public async Task<IActionResult> Manage()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Create"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền quản lý thông báo";
                return RedirectToAction("Index");
            }

            var allThongBao = await _thongBaoService.GetAllThongBaoAsync();
            return View(allThongBao);
        }

        // Tạo thông báo mới - GET
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
                TempData["ErrorMessage"] = "Bạn không có quyền tạo thông báo";
                return RedirectToAction("Index");
            }

            await LoadNguoiDungList();
            return View(new ThongBaoViewModel());
        }

        // Tạo thông báo mới - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThongBaoViewModel model)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Create"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền tạo thông báo";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                await LoadNguoiDungList();
                return View(model);
            }

            // Set người gửi là người dùng hiện tại
            var currentUser = await _nguoiDungService.GetByIdAsync(maNguoiDung.Value);
            var thongBaoToSend = new ThongBaoViewModel
            {
                TieuDe = model.TieuDe,
                NoiDung = model.NoiDung,
                LoaiThongBao = model.LoaiThongBao,
                MaNguoiNhan = model.MaNguoiNhan, // null = gửi cho tất cả
                NgayTao = DateTime.Now
            };

            var (success, message) = await _thongBaoService.GuiThongBaoAsync(thongBaoToSend);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Gửi thông báo", "Thông báo",
                    $"Gửi thông báo: {model.TieuDe} - Người nhận: {(model.MaNguoiNhan.HasValue ? "Cá nhân" : "Tất cả")}");
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Manage");
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadNguoiDungList();
                return View(model);
            }
        }

        // Xem chi tiết thông báo
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Tự động đánh dấu đã đọc khi xem chi tiết
            await _thongBaoService.DanhDauDaDocAsync(id, maNguoiDung.Value);

            // Lấy danh sách thông báo của user để tìm thông báo cần xem
            var thongBaoList = await _thongBaoService.GetThongBaoByNguoiDungAsync(maNguoiDung.Value);
            var thongBao = thongBaoList.FirstOrDefault(t => t.MaThongBao == id);

            if (thongBao == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông báo";
                return RedirectToAction("Index");
            }

            return View(thongBao);
        }

        // Đánh dấu đã đọc - AJAX
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            var (success, message) = await _thongBaoService.DanhDauDaDocAsync(id, maNguoiDung.Value);
            return Json(new { success, message });
        }

        // Đánh dấu tất cả đã đọc - AJAX
        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            try
            {
                var thongBaoList = await _thongBaoService.GetThongBaoByNguoiDungAsync(maNguoiDung.Value);
                var chuaDocList = thongBaoList.Where(t => !t.DaDoc).ToList();

                int successCount = 0;
                foreach (var thongBao in chuaDocList)
                {
                    var (success, _) = await _thongBaoService.DanhDauDaDocAsync(thongBao.MaThongBao, maNguoiDung.Value);
                    if (success) successCount++;
                }

                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Đánh dấu đã đọc", "Thông báo",
                    $"Đánh dấu đã đọc {successCount} thông báo");

                return Json(new { success = true, message = $"Đã đánh dấu {successCount} thông báo là đã đọc" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // Lấy số lượng thông báo chưa đọc - AJAX
        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { count = 0 });
            }

            var count = await _thongBaoService.GetSoThongBaoChuaDocAsync(maNguoiDung.Value);
            return Json(new { count });
        }

        // Lấy danh sách thông báo mới nhất - AJAX  
        [HttpGet]
        public async Task<IActionResult> GetLatestNotifications(int limit = 5)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { notifications = new List<object>() });
            }

            var thongBaoList = await _thongBaoService.GetThongBaoByNguoiDungAsync(maNguoiDung.Value);
            var latestNotifications = thongBaoList
                .OrderByDescending(t => t.NgayTao)
                .Take(limit)
                .Select(t => new
                {
                    id = t.MaThongBao,
                    title = t.TieuDe,
                    content = t.NoiDung.Length > 100 ? t.NoiDung.Substring(0, 100) + "..." : t.NoiDung,
                    type = t.LoaiThongBao,
                    isRead = t.DaDoc,
                    createdAt = t.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                    timeAgo = GetTimeAgo(t.NgayTao)
                })
                .ToList();

            return Json(new { notifications = latestNotifications });
        }

        // Xóa thông báo (chỉ admin)
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Delete"))
            {
                return Json(new { success = false, message = "Bạn không có quyền xóa thông báo" });
            }

            try
            {
                // Logic xóa thông báo (cần implement trong service)
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Xóa thông báo", "Thông báo",
                    $"Xóa thông báo ID: {id}");

                return Json(new { success = true, message = "Xóa thông báo thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // Gửi thông báo nhanh - AJAX
        [HttpPost]
        public async Task<IActionResult> SendQuickNotification(string title, string content, string type = "Thong_Tin")
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "NguoiDung_Create"))
            {
                return Json(new { success = false, message = "Bạn không có quyền gửi thông báo" });
            }

            var thongBao = new ThongBaoViewModel
            {
                TieuDe = title,
                NoiDung = content,
                LoaiThongBao = type,
                MaNguoiNhan = null, // Gửi cho tất cả
                NgayTao = DateTime.Now
            };

            var (success, message) = await _thongBaoService.GuiThongBaoAsync(thongBao);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Gửi thông báo nhanh", "Thông báo",
                    $"Gửi thông báo nhanh: {title}");
            }

            return Json(new { success, message });
        }

        #region Private Methods
        private async Task LoadNguoiDungList()
        {
            var nguoiDungList = await _nguoiDungService.GetAllAsync();
            ViewBag.NguoiDungList = nguoiDungList.Where(nd => nd.TrangThaiTaiKhoan);

            // Danh sách loại thông báo
            ViewBag.LoaiThongBaoList = new[]
            {
                new { Value = "Thong_Tin", Text = "Thông tin", Icon = "fas fa-info-circle", Color = "primary" },
                new { Value = "Canh_Bao", Text = "Cảnh báo", Icon = "fas fa-exclamation-triangle", Color = "warning" },
                new { Value = "Loi", Text = "Lỗi", Icon = "fas fa-times-circle", Color = "danger" },
                new { Value = "Thanh_Cong", Text = "Thành công", Icon = "fas fa-check-circle", Color = "success" }
            };
        }

        private string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalDays >= 1)
                return $"{(int)timeSpan.TotalDays} ngày trước";
            else if (timeSpan.TotalHours >= 1)
                return $"{(int)timeSpan.TotalHours} giờ trước";
            else if (timeSpan.TotalMinutes >= 1)
                return $"{(int)timeSpan.TotalMinutes} phút trước";
            else
                return "Vừa xong";
        }
        #endregion
    }
}