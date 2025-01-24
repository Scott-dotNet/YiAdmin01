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
    /// <summary>
    /// 数据字典缓存类
    /// </summary>
    public class DataDictCache : BaseBusinessCache<DataDictEntity>
    {
        private DataDictService dataDictService = new DataDictService();
        public override string CacheKey => this.GetType().Name;

        /// <summary>
        /// 查找数据字典实体列表
        /// </summary>
        /// <returns></returns>
        public override async Task<List<DataDictEntity>> GetList()
        {
            List<DataDictEntity>  cacheList = CacheFactory.Cache.GetCache<List<DataDictEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await dataDictService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);
                return list;
            }
            else
            {
                return cacheList;
            }



        }
    }
}

