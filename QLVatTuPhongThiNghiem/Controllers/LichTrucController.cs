using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using QLVatTuPhongThiNghiem.Data;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class LichTrucController : Controller
    {
        private readonly ILichTrucService _lichTrucService;
        private readonly INguoiDungService _nguoiDungService;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;
        private readonly IMasterDataService _masterDataService;
        private readonly AppDbContext _context;

        public LichTrucController(
            ILichTrucService lichTrucService,
            INguoiDungService nguoiDungService,
            ILichSuHoatDongService lichSuHoatDongService,
            IMasterDataService masterDataService,
            AppDbContext context)
        {
            _lichTrucService = lichTrucService;
            _nguoiDungService = nguoiDungService;
            _lichSuHoatDongService = lichSuHoatDongService;
            _masterDataService = masterDataService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "LichThucHanh_Read"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền xem lịch trực";
                return RedirectToAction("Index", "Dashboard");
            }

            var lichTrucList = await _lichTrucService.GetAllAsync();
            return View(lichTrucList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "LichThucHanh_Create"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền tạo lịch trực";
                return RedirectToAction("Index");
            }

            await LoadMasterData();
            return View(new LichTrucViewModel { Ngay = DateTime.Today.AddDays(1) });
        }

        [HttpPost]
        public async Task<IActionResult> Create(LichTrucViewModel model)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "LichThucHanh_Create"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền tạo lịch trực";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                await LoadMasterData();
                return View(model);
            }

            var (success, message) = await _lichTrucService.TaoLichTrucAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Tạo lịch trực", "Lịch trực",
                    $"Tạo lịch trực ngày {model.Ngay:dd/MM/yyyy} - {model.CaLam}");
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadMasterData();
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

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "LichThucHanh_Update"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền cập nhật lịch trực";
                return RedirectToAction("Index");
            }

            var model = await _lichTrucService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy lịch trực";
                return RedirectToAction("Index");
            }

            await LoadMasterData();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LichTrucViewModel model)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "LichThucHanh_Update"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền cập nhật lịch trực";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                await LoadMasterData();
                return View(model);
            }

            var (success, message) = await _lichTrucService.CapNhatLichTrucAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Cập nhật lịch trực", "Lịch trực",
                    $"Cập nhật lịch trực ngày {model.Ngay:dd/MM/yyyy} - {model.CaLam}");
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadMasterData();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });
            }

            if (!await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, "LichThucHanh_Update"))
            {
                return Json(new { success = false, message = "Bạn không có quyền hủy lịch trực" });
            }

            var lichTruc = await _lichTrucService.GetByIdAsync(id);
            var (success, message) = await _lichTrucService.XoaLichTrucAsync(id);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Hủy lịch trực", "Lịch trực",
                    $"Hủy lịch trực ngày {lichTruc?.Ngay:dd/MM/yyyy} - {lichTruc?.CaLam}");
            }

            return Json(new { success, message });
        }

        public async Task<IActionResult> Calendar(DateTime? month)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!month.HasValue)
                month = DateTime.Today;

            var startDate = new DateTime(month.Value.Year, month.Value.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // Lấy tất cả lịch trực trong tháng
            var lichTrucList = await _lichTrucService.GetAllAsync();
            var lichTrucThang = lichTrucList.Where(l => l.Ngay >= startDate && l.Ngay <= endDate);

            ViewBag.Month = month.Value;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(lichTrucThang);
        }

        [HttpGet]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var lichTrucList = await _lichTrucService.GetByNgayAsync(date);
            return Json(lichTrucList);
        }

        private async Task LoadMasterData()
        {
            // Load danh sách nhân viên
            var nhanVienList = await _context.NhanVien
                .Where(nv => nv.TrangThai)
                .Select(nv => new { nv.MaNV, nv.HoTen })
                .OrderBy(nv => nv.HoTen)
                .ToListAsync();

            ViewBag.NhanVienList = nhanVienList;

            // Load danh sách phòng máy
            var phongMayList = await _masterDataService.GetPhongMayAsync();
            ViewBag.PhongMayList = phongMayList;

            // Danh sách ca làm
            ViewBag.CaLamList = new[]
            {
                new { Value = "Ca sáng", Text = "Ca sáng (7:00 - 11:30)" },
                new { Value = "Ca chiều", Text = "Ca chiều (13:30 - 17:00)" },
                new { Value = "Ca tối", Text = "Ca tối (18:00 - 21:00)" }
            };

            // Danh sách trạng thái
            ViewBag.TrangThaiList = new[]
            {
                new { Value = "Đã lên lịch", Text = "Đã lên lịch" },
                new { Value = "Đang trực", Text = "Đang trực" },
                new { Value = "Hoàn thành", Text = "Hoàn thành" },
                new { Value = "Đã hủy", Text = "Đã hủy" }
            };
        }
    }
}
