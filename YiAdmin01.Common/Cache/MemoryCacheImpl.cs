
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using YiAdmin01.Common.Global;

namespace YiAdmin01.Common.Cache
{
    /// <summary>
    /// MemoryCache 实现类
    /// </summary>
    public class MemoryCacheImpl : ICache
    {
        private readonly IMemoryCache _cache = GlobalContext.ServiceProvider.GetService<IMemoryCache>() ;

        /// <summary>
        /// 获取Cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetCache<T>(string key)
        {
            var value = _cache.Get<T>(key);
            return value;
        }
        /// <summary>
        /// 移除Cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveCache(string key)
        {
            _cache.Remove(key);
            return true;
        }

        /// <summary>
        /// 设置Cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public bool SetCache<T>(string key, T value, DateTime? expireTime = null)
        {
            try
            {
                if (expireTime == null) 
                {
                    return _cache.Set(key, value) != null;
                }
                else
                {
                    return _cache.Set(key, value, expireTime.Value-DateTime.Now) != null;
                }
            }
            catch (Exception ex)
            {
                //日志TBD
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
    }
}
