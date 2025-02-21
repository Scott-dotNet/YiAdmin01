using YiAdmin01.BLL.Services.InfoService;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;
using YiAdmin01.Model;
using YiAdmin01.Common.Extension;
using YiAdmin01.BLL.Cache;
using YiAdmin01.BLL.Services.OrganizationManage;
using YiAdmin01.Common.Utils;

namespace YiAdmin01.BLL.Business.InfoBLL
{
    /// <summary>
    /// 供应商业务类
    /// </summary>
    public class SupplierBLL
    {
        private SupplierService supplierService = new SupplierService();
        private SupplierCache supplierCache = new SupplierCache();

        #region 获取数据
        public async Task<TData<List<SupplierEntity>>> GetList(SupplierListParam param)
        {
            TData<List<SupplierEntity>> obj = new TData<List<SupplierEntity>>();
            //obj.Data = await supplierCache.GetList(param);
            obj.Data = await supplierService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<SupplierEntity>>> GetPageList(SupplierListParam param, Pagination pagination)
        {
            TData<List<SupplierEntity>> obj = new TData<List<SupplierEntity>>();
            obj.Data = await supplierService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<SupplierEntity>> GetEntity(long id)
        {
            TData<SupplierEntity> obj = new TData<SupplierEntity>();
            obj.Data = await supplierService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        public async Task<TData<SupplierEntity>> GetEntity(string supplierName)
        {
            TData<SupplierEntity> obj = new TData<SupplierEntity>();
            obj.Data = await supplierService.GetEntity(supplierName);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData<SupplierEntity>> GetSupplierList(string supplierName)
        {
            TData<SupplierEntity> obj = new TData<SupplierEntity>();
            obj.Data = await supplierService.GetEntity(supplierName);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(SupplierEntity entity)
        {
            TData<string> obj = new TData<string>();

            if (supplierService.ExistSupplierName(entity))
            {
                obj.Message = "供应商名称已经存在！";
                return obj;
            }

            await supplierService.SaveForm(entity);

            // 清除缓存里面的数据
            //string key = supplierCache.CacheKey + entity.CompanyCnName.ParseToString();
            //supplierCache.Remove(key);

            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;

            return obj;           
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            if (string.IsNullOrEmpty(ids))
            {
                obj.Message = "参数不能为空";
                return obj;
            }
            await supplierService.DeleteForm(ids);

            // 清除缓存里面的数据
            //foreach (var id in TextHelper.SplitToArray<long>(ids, ','))
            //{
            //    string key = supplierCache.CacheKey + id.ParseToString();
            //    supplierCache.Remove(key);
            //}

            obj.Tag = 1;
            return obj;
        }
        #endregion

    }
}
