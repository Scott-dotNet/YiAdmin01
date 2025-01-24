using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiAdmin01.BLL.Services.OrganizationManage;
using YiAdmin01.Common.Cache;
using YiAdmin01.Entity;

namespace YiAdmin01.BLL.Cache
{
    public class AreaCache : BaseBusinessCache<AreaEntity>
    {
        private AreaService areaService = new AreaService();

        public override string CacheKey => this.GetType().Name;

        public override async Task<List<AreaEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<AreaEntity>>(CacheKey);
            if (cacheList == null)
            {
                var result = await areaService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, result);
                return result;
            }
            else
            {
                return cacheList;
            }
        }
    }
}
