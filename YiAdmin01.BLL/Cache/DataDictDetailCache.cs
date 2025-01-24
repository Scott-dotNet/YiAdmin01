using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiAdmin01.BLL.Services.SystemManage;
using YiAdmin01.Common.Cache;
using YiAdmin01.Entity;

namespace YiAdmin01.BLL.Cache
{
    public class DataDictDetailCache : BaseBusinessCache<DataDictDetailEntity>
    {
        private DataDictDetailService service = new();

        public override string CacheKey => this.GetType().Name;

        public override async Task<List<DataDictDetailEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<DataDictDetailEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await service.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);
                return list;
            }
            return cacheList;
        }
    }
}
