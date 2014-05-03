using QLVatTuPhongThiNghiem.Middlewares;

namespace QLVatTuPhongThiNghiem.Extensions
{
    public static class ServiceExtensions
    {
        public static IApplicationBuilder UsePermissionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PermissionMiddleware>();
        }
    }
}