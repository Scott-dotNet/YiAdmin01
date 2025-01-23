
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using YiAdmin01.Common.Global;

namespace YiAdmin01.Common.Utils
{
    public class SessionHelper
    {
        /// <summary>
        /// 获取Session对象
        /// </summary>
        /// <returns></returns>
        public static ISession? GetObj()
        {
            IHttpContextAccessor? httpContextAccessor = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            return httpContextAccessor?.HttpContext?.Session;
        }

        #region session 读写删除
        /// <summary>
        /// 写 Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Set<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key)) return false;
            ISession? session = SessionHelper.GetObj();
            session.SetString(key, JsonSerializer.Serialize(value));
            return true;
        }

        /// <summary>
        /// 写入 Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        /// <returns>状态</returns>
        public static bool Set(string key, string value)
        {
            return Set<string>(key, value);
        }

        /// <summary>
        /// 读 session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T? Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) return default;
            ISession? session = SessionHelper.GetObj();
            string? str = session.GetString(key);
            if (string.IsNullOrEmpty(str)) return default;
            return JsonSerializer.Deserialize<T>(str);
        }

        /// <summary>
        /// 读取 Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <returns>值</returns>
        public static string Get(string key)
        {
            return Get<string>(key) ?? "";
        }

        /// <summary>
        /// 删除 Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <returns>状态</returns>
        public static bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) { return false; }
            SessionHelper.GetObj().Remove(key);
            return true;
        }

        /// <summary>
        /// 清空 Session
        /// </summary>
        /// <returns>状态</returns>
        public static bool Clear()
        {
            SessionHelper.GetObj().Clear();
            return true;
        }
        #endregion
    }
}
