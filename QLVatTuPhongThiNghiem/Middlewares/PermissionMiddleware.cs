using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Middlewares
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PermissionMiddleware> _logger;

        public PermissionMiddleware(RequestDelegate next, ILogger<PermissionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Bỏ qua kiểm tra cho các route không cần xác thực
            var path = context.Request.Path.Value?.ToLower();
            var method = context.Request.Method.ToUpper();

            if (IsPublicRoute(path, method))
            {
                await _next(context);
                return;
            }

            // Kiểm tra đăng nhập
            var maNguoiDung = context.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }

            // Kiểm tra quyền hạn cho các action cụ thể
            var controller = context.Request.RouteValues["controller"]?.ToString();
            var action = context.Request.RouteValues["action"]?.ToString();

            if (!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
            {
                var requiredPermission = GetRequiredPermission(controller, action, method);

                if (!string.IsNullOrEmpty(requiredPermission))
                {
                    var quyenHan = context.Session.GetString("QuyenHan")?.Split(',') ?? new string[0];

                    if (!quyenHan.Contains(requiredPermission))
                    {
                        _logger.LogWarning($"User {maNguoiDung} không có quyền {requiredPermission} cho {controller}/{action}");

                        if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            // AJAX request
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync("{\"success\": false, \"message\": \"Bạn không có quyền thực hiện hành động này\"}");
                            return;
                        }
                        else
                        {
                            // Normal request
                            context.Response.Redirect("/Auth/AccessDenied");
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }

        private bool IsPublicRoute(string path, string method)
        {
            // Routes không cần authentication
            var publicRoutes = new[]
            {
                "/auth/login",           // Cả GET và POST
                "/auth/logout",          // POST logout  
                "/auth/accessdenied",    // GET
                "/home/error",           // GET
                "/favicon.ico",
                "/css/",
                "/js/",
                "/lib/",
                "/images/",
                "/assets/"
            };

            // ✅ Kiểm tra đặc biệt cho Auth controller
            if (path?.StartsWith("/auth") == true)
            {
                // Cho phép tất cả các action trong Auth controller
                return true;
            }

            return publicRoutes.Any(route => path?.StartsWith(route) == true);
        }

        private string GetRequiredPermission(string controller, string action, string method)
        {
            // ✅ Bỏ qua kiểm tra quyền cho Auth controller
            if (controller?.ToLower() == "auth")
            {
                return null;
            }

            // Mapping controller/action với quyền hạn
            var permissionMap = new Dictionary<string, Dictionary<string, string>>
            {
                ["TrangTB"] = new Dictionary<string, string>
                {
                    ["Index"] = "QuanLyTrangTB_Read",
                    ["Create"] = "QuanLyTrangTB_Create",
                    ["Edit"] = "QuanLyTrangTB_Update",
                    ["Delete"] = "QuanLyTrangTB_Delete"
                },
                ["LichThucHanh"] = new Dictionary<string, string>
                {
                    ["Index"] = "LichThucHanh_Read",
                    ["Create"] = "LichThucHanh_Create",
                    ["Edit"] = "LichThucHanh_Update",
                    ["UpdateStatus"] = "LichThucHanh_Update"
                },
                ["SuaChua"] = new Dictionary<string, string>
                {
                    ["Index"] = "SuaChua_Read",
                    ["Create"] = "SuaChua_Create",
                    ["Complete"] = "SuaChua_Update"
                },
                ["NguoiDung"] = new Dictionary<string, string>
                {
                    ["Index"] = "NguoiDung_Read",
                    ["Create"] = "NguoiDung_Create",
                    ["Edit"] = "NguoiDung_Update",
                    ["Delete"] = "NguoiDung_Delete",
                    ["ToggleStatus"] = "NguoiDung_Update",
                    ["Permissions"] = "NguoiDung_Update",
                    ["AssignRoles"] = "NguoiDung_Update"
                },
                ["BaoCao"] = new Dictionary<string, string>
                {
                    ["ThongKeTheoPhong"] = "BaoCao_Read",
                    ["SuDungTheoThang"] = "BaoCao_Read",
                    ["ChiPhiSuaChua"] = "BaoCao_Read",
                    ["DanhGiaCapDo"] = "BaoCao_Read"
                },
                ["LichSuHoatDong"] = new Dictionary<string, string>
                {
                    ["Index"] = "BaoCao_Read"
                }
            };

            if (permissionMap.ContainsKey(controller) && permissionMap[controller].ContainsKey(action))
            {
                return permissionMap[controller][action];
            }

            return null; // Không yêu cầu quyền đặc biệt
        }
    }
}