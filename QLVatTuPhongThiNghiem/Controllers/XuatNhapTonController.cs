// File: Controllers/XuatNhapTonController.cs (Cập nhật)
// Vị trí: QLVatTuPhongThiNghiem/Controllers/XuatNhapTonController.cs
// Thay thế file cũ

using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class XuatNhapTonController : Controller
    {
        private readonly IXuatNhapTonService _xuatNhapTonService;
        private readonly ITrangTBService _trangTBService;
        private readonly IMasterDataService _masterDataService;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public XuatNhapTonController(
            IXuatNhapTonService xuatNhapTonService,
            ITrangTBService trangTBService,
            IMasterDataService masterDataService,
            ILichSuHoatDongService lichSuHoatDongService)
        {
            _xuatNhapTonService = xuatNhapTonService;
            _trangTBService = trangTBService;
            _masterDataService = masterDataService;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        // Danh sách phiếu xuất nhập tồn
        public async Task<IActionResult> Index(string loaiPhieu = "", DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            IEnumerable<XuatNhapTonViewModel> phieuList;

            if (!string.IsNullOrEmpty(loaiPhieu))
            {
                phieuList = await _xuatNhapTonService.GetByLoaiPhieuAsync(loaiPhieu);
            }
            else if (tuNgay.HasValue && denNgay.HasValue)
            {
                phieuList = await _xuatNhapTonService.GetByDateRangeAsync(tuNgay.Value, denNgay.Value);
            }
            else
            {
                phieuList = await _xuatNhapTonService.GetAllAsync();
            }

            ViewBag.LoaiPhieuFilter = loaiPhieu;
            ViewBag.TuNgayFilter = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgayFilter = denNgay?.ToString("yyyy-MM-dd");
            ViewBag.DanhSachLoaiPhieu = _xuatNhapTonService.GetDanhSachLoaiPhieu();

            return View(phieuList);
        }

        // Chi tiết phiếu
        public async Task<IActionResult> Details(int id)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var phieu = await _xuatNhapTonService.GetByIdAsync(id);
            if (phieu == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu";
                return RedirectToAction("Index");
            }

            // Lấy thông tin tồn kho chi tiết
            ViewBag.ThongTinTonKho = await _xuatNhapTonService.GetThongTinTonKhoChiTietAsync(phieu.MaTTB);

            return View(phieu);
        }

        // Trang nhập thiết bị
        [HttpGet]
        public async Task<IActionResult> Nhap()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await LoadThietBiList();
            return View(new XuatNhapTonViewModel { LoaiPhieu = "NHAP" });
        }

        [HttpPost]
        public async Task<IActionResult> Nhap(XuatNhapTonViewModel model)
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

            model.NguoiTao = maNguoiDung.Value;
            var (success, message) = await _xuatNhapTonService.NhapThietBiAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadThietBiList();
                return View(model);
            }
        }

        // Trang xuất thiết bị
        [HttpGet]
        public async Task<IActionResult> Xuat()
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await LoadThietBiList();
            return View(new XuatNhapTonViewModel { LoaiPhieu = "XUAT" });
        }

        [HttpPost]
        public async Task<IActionResult> Xuat(XuatNhapTonViewModel model)
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

            model.NguoiTao = maNguoiDung.Value;
            var (success, message) = await _xuatNhapTonService.XuatThietBiAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadThietBiList();
                return View(model);
            }
        }

        // Chỉnh sửa phiếu
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var phieu = await _xuatNhapTonService.GetByIdAsync(id);
            if (phieu == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu";
                return RedirectToAction("Index");
            }

            // Kiểm tra quyền chỉnh sửa
            var quyenHan = HttpContext.Session.GetString("QuyenHan")?.Split(',') ?? new string[0];
            var isAdmin = quyenHan.Contains("XuatNhapTon_Update");
            var coTheChinhSua = await _xuatNhapTonService.KiemTraQuyenChinhSuaAsync(id, maNguoiDung.Value, isAdmin);

            if (!coTheChinhSua)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền chỉnh sửa phiếu này hoặc phiếu đã quá thời hạn chỉnh sửa";
                return RedirectToAction("Index");
            }

            await LoadThietBiList();
            return View(phieu);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(XuatNhapTonViewModel model)
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

            var (success, message) = await _xuatNhapTonService.CapNhatPhieuAsync(model, maNguoiDung.Value);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", message);
                await LoadThietBiList();
                return View(model);
            }
        }

        // Xóa phiếu
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập đã hết hạn" });
            }

            // Kiểm tra có thể xóa không
            var (canDelete, canDeleteMessage) = await _xuatNhapTonService.KiemTraCoTheXoaAsync(id);
            if (!canDelete)
            {
                return Json(new { success = false, message = canDeleteMessage });
            }

            var (success, message) = await _xuatNhapTonService.XoaPhieuAsync(id, maNguoiDung.Value);

            return Json(new { success = success, message = message });
        }

        // Lấy tồn kho thiết bị (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetTonKho(int maTTB)
        {
            try
            {
                var tonKho = await _xuatNhapTonService.GetTonKhoThietBiAsync(maTTB);
                return Json(new { success = true, tonKho = tonKho });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Lấy thông tin chi tiết tồn kho (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetThongTinTonKho(int maTTB)
        {
            try
            {
                var thongTin = await _xuatNhapTonService.GetThongTinTonKhoChiTietAsync(maTTB);
                return Json(new { success = true, data = thongTin });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Lịch sử xuất nhập theo thiết bị
        public async Task<IActionResult> LichSuThietBi(int maTTB)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var lichSu = await _xuatNhapTonService.GetByThietBiAsync(maTTB);
            var thietBi = await _trangTBService.GetByIdAsync(maTTB);

            ViewBag.ThietBi = thietBi;
            ViewBag.TonKhoHienTai = await _xuatNhapTonService.GetTonKhoThietBiAsync(maTTB);

            return View(lichSu);
        }

        // Báo cáo tồn kho
        public async Task<IActionResult> BaoCaoTonKho(int? maPhongMay)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var baoCao = await _xuatNhapTonService.BaoCaoTonKhoAsync(maPhongMay);
            ViewBag.PhongMayList = await _masterDataService.GetPhongMayAsync();
            ViewBag.SelectedPhongMay = maPhongMay;

            return View(baoCao);
        }

        // Thống kê xuất nhập
        public async Task<IActionResult> ThongKe(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Mặc định là thống kê tháng hiện tại
            if (!tuNgay.HasValue)
                tuNgay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (!denNgay.HasValue)
                denNgay = tuNgay.Value.AddMonths(1).AddDays(-1);

            var thongKe = await _xuatNhapTonService.ThongKeXuatNhapAsync(tuNgay, denNgay);

            ViewBag.TuNgay = tuNgay.Value.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay.Value.ToString("yyyy-MM-dd");

            return View(thongKe);
        }

        // Xuất Excel báo cáo tồn kho
        public async Task<IActionResult> XuatExcelTonKho(int? maPhongMay)
        {
            if (!HttpContext.Session.GetInt32("MaNguoiDung").HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var baoCao = await _xuatNhapTonService.BaoCaoTonKhoAsync(maPhongMay);

            // Ghi lịch sử
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung").Value;
            await _lichSuHoatDongService.GhiLichSuAsync(
                maNguoiDung,
                "Xuất báo cáo",
                "Xuất nhập tồn",
                $"Xuất báo cáo tồn kho Excel - Phòng máy: {maPhongMay?.ToString() ?? "Tất cả"}");

            // Tạo file Excel và trả về
            // TODO: Implement Excel export logic
            TempData["InfoMessage"] = "Chức năng xuất Excel đang được phát triển";
            return RedirectToAction("BaoCaoTonKho", new { maPhongMay });
        }

        // API: Kiểm tra quyền chỉnh sửa
        [HttpGet]
        public async Task<IActionResult> CheckEditPermission(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                return Json(new { success = false, message = "Phiên đăng nhập đã hết hạn" });
            }

            var quyenHan = HttpContext.Session.GetString("QuyenHan")?.Split(',') ?? new string[0];
            var isAdmin = quyenHan.Contains("XuatNhapTon_Update");
            var coTheChinhSua = await _xuatNhapTonService.KiemTraQuyenChinhSuaAsync(id, maNguoiDung.Value, isAdmin);

            return Json(new { canEdit = coTheChinhSua });
        }

        // API: Kiểm tra có thể xóa
        [HttpGet]
        public async Task<IActionResult> CheckDeletePermission(int id)
        {
            var (canDelete, message) = await _xuatNhapTonService.KiemTraCoTheXoaAsync(id);
            return Json(new { canDelete = canDelete, message = message });
        }

        // Phương thức hỗ trợ
        private async Task LoadThietBiList()
        {
            var thietBiList = await _trangTBService.GetAllAsync();
            ViewBag.ThietBiList = thietBiList;
            ViewBag.DanhSachLoaiPhieu = _xuatNhapTonService.GetDanhSachLoaiPhieu();
        }
    }
}