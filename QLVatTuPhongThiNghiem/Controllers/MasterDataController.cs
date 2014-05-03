using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models.Entities;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class MasterDataController : Controller
    {
        private readonly IMasterDataService _masterDataService;
        private readonly INguoiDungService _nguoiDungService;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public MasterDataController(
            IMasterDataService masterDataService,
            INguoiDungService nguoiDungService,
            ILichSuHoatDongService lichSuHoatDongService)
        {
            _masterDataService = masterDataService;
            _nguoiDungService = nguoiDungService;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        #region Loại thiết bị
        public async Task<IActionResult> Loai()
        {
            if (!await CheckPermissionAsync("QuanLyTrangTB_Read"))
                return RedirectToAccessDenied();

            var loaiList = await _masterDataService.GetLoaiAsync();
            return View(loaiList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateLoai()
        {
            if (!await CheckPermissionAsync("QuanLyTrangTB_Create"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            return PartialView("_CreateLoaiModal", new Loai());
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoai(Loai model)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            if (!await CheckPermissionAsync("QuanLyTrangTB_Create"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            var (success, message) = await _masterDataService.CreateLoaiAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Tạo loại thiết bị", "Master Data",
                    $"Tạo loại thiết bị: {model.TenLoai}");
            }

            return Json(new { success, message });
        }

        [HttpGet]
        public async Task<IActionResult> EditLoai(int id)
        {
            if (!await CheckPermissionAsync("QuanLyTrangTB_Update"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            var loai = await _masterDataService.GetLoaiByIdAsync(id);
            if (loai == null)
                return Json(new { success = false, message = "Không tìm thấy loại thiết bị" });

            return PartialView("_EditLoaiModal", loai);
        }

        [HttpPost]
        public async Task<IActionResult> EditLoai(Loai model)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            if (!await CheckPermissionAsync("QuanLyTrangTB_Update"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            var (success, message) = await _masterDataService.UpdateLoaiAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Cập nhật loại thiết bị", "Master Data",
                    $"Cập nhật loại thiết bị: {model.TenLoai}");
            }

            return Json(new { success, message });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLoai(int id)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            if (!await CheckPermissionAsync("QuanLyTrangTB_Delete"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            var loai = await _masterDataService.GetLoaiByIdAsync(id);
            var (success, message) = await _masterDataService.DeleteLoaiAsync(id);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Xóa loại thiết bị", "Master Data",
                    $"Xóa loại thiết bị: {loai?.TenLoai}");
            }

            return Json(new { success, message });
        }
        #endregion

        #region Thương hiệu
        public async Task<IActionResult> ThuongHieu()
        {
            if (!await CheckPermissionAsync("QuanLyTrangTB_Read"))
                return RedirectToAccessDenied();

            var thuongHieuList = await _masterDataService.GetThuongHieuAsync();
            return View(thuongHieuList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateThuongHieu(ThuongHieu model)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            if (!await CheckPermissionAsync("QuanLyTrangTB_Create"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            var (success, message) = await _masterDataService.CreateThuongHieuAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Tạo thương hiệu", "Master Data",
                    $"Tạo thương hiệu: {model.TenThuongHieu}");
            }

            return Json(new { success, message });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateThuongHieu(ThuongHieu model)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            if (!await CheckPermissionAsync("QuanLyTrangTB_Update"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            var (success, message) = await _masterDataService.UpdateThuongHieuAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Cập nhật thương hiệu", "Master Data",
                    $"Cập nhật thương hiệu: {model.TenThuongHieu}");
            }

            return Json(new { success, message });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteThuongHieu(int id)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            if (!await CheckPermissionAsync("QuanLyTrangTB_Delete"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            var thuongHieu = await _masterDataService.GetThuongHieuByIdAsync(id);
            var (success, message) = await _masterDataService.DeleteThuongHieuAsync(id);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Xóa thương hiệu", "Master Data",
                    $"Xóa thương hiệu: {thuongHieu?.TenThuongHieu}");
            }

            return Json(new { success, message });
        }
        #endregion

        #region Phòng máy
        public async Task<IActionResult> PhongMay()
        {
            if (!await CheckPermissionAsync("QuanLyTrangTB_Read"))
                return RedirectToAccessDenied();

            var phongMayList = await _masterDataService.GetPhongMayAsync();
            return View(phongMayList);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePhongMay(PhongMay model)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            if (!await CheckPermissionAsync("QuanLyTrangTB_Create"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            var (success, message) = await _masterDataService.CreatePhongMayAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Tạo phòng máy", "Master Data",
                    $"Tạo phòng máy: {model.TenPhongMay}");
            }

            return Json(new { success, message });
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhongMay(PhongMay model)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            if (!await CheckPermissionAsync("QuanLyTrangTB_Update"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            var (success, message) = await _masterDataService.UpdatePhongMayAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Cập nhật phòng máy", "Master Data",
                    $"Cập nhật phòng máy: {model.TenPhongMay}");
            }

            return Json(new { success, message });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhongMay(int id)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            if (!await CheckPermissionAsync("QuanLyTrangTB_Delete"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            var phongMay = await _masterDataService.GetPhongMayByIdAsync(id);
            var (success, message) = await _masterDataService.DeletePhongMayAsync(id);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Xóa phòng máy", "Master Data",
                    $"Xóa phòng máy: {phongMay?.TenPhongMay}");
            }

            return Json(new { success, message });
        }
        #endregion

        #region Nhân viên
        public async Task<IActionResult> NhanVien()
        {
            if (!await CheckPermissionAsync("NguoiDung_Read"))
                return RedirectToAccessDenied();

            var nhanVienList = await _masterDataService.GetNhanVienAsync();
            return View(nhanVienList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateNhanVien()
        {
            if (!await CheckPermissionAsync("NguoiDung_Create"))
                return RedirectToAccessDenied();

            return View(new NhanVien());
        }

        [HttpPost]
        public async Task<IActionResult> CreateNhanVien(NhanVien model)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return RedirectToAction("Login", "Auth");

            if (!await CheckPermissionAsync("NguoiDung_Create"))
            {
                TempData["ErrorMessage"] = "Không có quyền thực hiện";
                return RedirectToAction("NhanVien");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _masterDataService.CreateNhanVienAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Tạo nhân viên", "Nhân viên",
                    $"Tạo nhân viên: {model.HoTen}");
                TempData["SuccessMessage"] = message;
                return RedirectToAction("NhanVien");
            }
            else
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditNhanVien(int id)
        {
            if (!await CheckPermissionAsync("NguoiDung_Update"))
                return RedirectToAccessDenied();

            var nhanVien = await _masterDataService.GetNhanVienByIdAsync(id);
            if (nhanVien == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy nhân viên";
                return RedirectToAction("NhanVien");
            }

            return View(nhanVien);
        }

        [HttpPost]
        public async Task<IActionResult> EditNhanVien(NhanVien model)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return RedirectToAction("Login", "Auth");

            if (!await CheckPermissionAsync("NguoiDung_Update"))
            {
                TempData["ErrorMessage"] = "Không có quyền thực hiện";
                return RedirectToAction("NhanVien");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _masterDataService.UpdateNhanVienAsync(model);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Cập nhật nhân viên", "Nhân viên",
                    $"Cập nhật nhân viên: {model.HoTen}");
                TempData["SuccessMessage"] = message;
                return RedirectToAction("NhanVien");
            }
            else
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNhanVien(int id)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue)
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            if (!await CheckPermissionAsync("NguoiDung_Delete"))
                return Json(new { success = false, message = "Không có quyền thực hiện" });

            var nhanVien = await _masterDataService.GetNhanVienByIdAsync(id);
            var (success, message) = await _masterDataService.DeleteNhanVienAsync(id);

            if (success)
            {
                await _lichSuHoatDongService.GhiLichSuAsync(maNguoiDung.Value, "Xóa nhân viên", "Nhân viên",
                    $"Xóa nhân viên: {nhanVien?.HoTen}");
            }

            return Json(new { success, message });
        }
        #endregion

        #region Helper Methods
        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("MaNguoiDung");
        }

        private async Task<bool> CheckPermissionAsync(string permission)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue) return false;

            return await _nguoiDungService.KiemTraQuyenAsync(maNguoiDung.Value, permission);
        }

        private IActionResult RedirectToAccessDenied()
        {
            TempData["ErrorMessage"] = "Bạn không có quyền truy cập chức năng này";
            return RedirectToAction("Index", "Dashboard");
        }
        #endregion
    }
}
