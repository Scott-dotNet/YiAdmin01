
namespace YiAdmin01.Common.Cache
{
    /// <summary>
    /// 缓存工厂
    /// </summary>
    public class CacheFactory
    {
        private static ICache cache = null;
        private static readonly object lockHelper = new object();

        public static ICache Cache
        {
            get
            {//双重检查锁定机制
                if (cache == null)
                {
                    lock (lockHelper)
                    {
                        if (cache == null)
                        {
                            cache = new MemoryCacheImpl();
                        }
                    }
                }
                return cache;
            }
        }
    }
}
