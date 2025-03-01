using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiAdmin01.BLL.Services.InfoService;
using YiAdmin01.Common.Cache;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;

namespace YiAdmin01.BLL.Cache
{
    public class SupplierCache : BaseBusinessCache<SupplierEntity>
    {
        public override string CacheKey => "SupplierCache";
        private SupplierService supplierService = new SupplierService();

        public async Task<List<SupplierEntity>> GetList(SupplierListParam param)
        {
            String supplierCachekey = $"{CacheKey}_{param.SupplierName}";
            var cacheList = CacheFactory.Cache.GetCache<List<SupplierEntity>>(supplierCachekey);
            if (cacheList == null)
            {
                var list = await supplierService.GetList(param);
                CacheFactory.Cache.SetCache(supplierCachekey, list);
                return list;
            }
            else
            {
                return cacheList;
            }
        }
        public async Task<SupplierEntity> GetEntity(long id)
        {
            String supplierCachekey = $"{CacheKey}_{id}";
            var cacheList = CacheFactory.Cache.GetCache<SupplierEntity>(supplierCachekey);
            if (cacheList == null)
            {
                var list = await supplierService.GetEntity(id);
                CacheFactory.Cache.SetCache(supplierCachekey, list);
                return list;
            }
            else
            {
                return cacheList;
            }
        }
    }
}
