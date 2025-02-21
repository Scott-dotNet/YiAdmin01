using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiAdmin01.BLL.Services.InfoService;
using YiAdmin01.Common.Cache;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;

namespace YiAdmin01.BLL.Cache
{
    public class ConsigneeCache : BaseBusinessCache<ConsigneeEntity>
    {
        public override string CacheKey => "ConsigneeCache";
        private ConsigneeService consigneeService = new ConsigneeService();

        /// <summary>
        /// Key：公司名称；
        /// Value：List<ConsigneeEntity>；
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ConsigneeEntity>> GetList(ConsigneeListParam param)
        {
            string consigneeCacheKey = $"{CacheKey}_{param.CompanyCnName}";
            var cacheList = CacheFactory.Cache.GetCache<List<ConsigneeEntity>>(consigneeCacheKey);
            if (cacheList == null)
            { //没有缓存
                var list = await consigneeService.GetList(param);
                CacheFactory.Cache.SetCache(consigneeCacheKey, list);
                return list;
            }
            else
            { //有缓存
                return cacheList;
            }
        }

        public async Task<ConsigneeEntity> GetEntity(long id)
        {
            string consigneeCacheKey = $"{CacheKey}_{id}";
            var cacheList = CacheFactory.Cache.GetCache<ConsigneeEntity>(consigneeCacheKey);
            if (cacheList == null)
            { //没有缓存
                var list = await consigneeService.GetEntity(id);
                CacheFactory.Cache.SetCache(consigneeCacheKey, list);
                return list;
            }
            else
            { //有缓存
                return cacheList;
            }
        }

       
        
    }
    
}
