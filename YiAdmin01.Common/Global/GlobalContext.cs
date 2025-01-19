using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using YiAdmin01.Model.Configs;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace YiAdmin01.Common.Global
{
    /// <summary>
    /// 全局环境
    /// </summary>
    public class GlobalContext
    {
        /// <summary>
        /// 服务容器
        /// </summary>
        public static IServiceCollection Services { get; set; }

        /// <summary>
        /// 配置的服务提供商
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 主机环境
        /// </summary>
        public static IWebHostEnvironment HostingEnvironment { get; set; }

        public static SystemConfig SystemConfig { get; set; }

        /// <summary>
        /// 设置缓存的过期时间
        /// </summary>
        /// <param name="context"></param>
        public static void SetCacheControl(StaticFileResponseContext context)
        {
            int second = 365 * 24 * 60 * 60;
            context.Context.Response.Headers.Append("Cache-Control", new[] { "public,max-age=" + second });
            context.Context.Response.Headers.Append("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") });
            //context.Context.Response.Headers.Add("Cache-Control", new[] { "public,max-age=" + second });
            //context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") }); // Format RFC1123
        }
    }
}
