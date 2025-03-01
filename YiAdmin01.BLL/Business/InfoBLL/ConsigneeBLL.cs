using YiAdmin01.BLL.Services.InfoService;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;
using YiAdmin01.Model;
using YiAdmin01.Common.Extension;
using YiAdmin01.BLL.Cache;
using YiAdmin01.BLL.Services.OrganizationManage;
using YiAdmin01.Entity;

namespace YiAdmin01.BLL.Business.InfoBLL
{
    public class ConsigneeBLL
    {
        private ConsigneeService consigneeService = new ConsigneeService();
        private ConsigneeCache consigneeCache = new ConsigneeCache();

        #region 获取数据
        public async Task<TData<List<ConsigneeEntity>>> GetList(ConsigneeListParam param)
        {
            TData<List<ConsigneeEntity>> obj = new TData<List<ConsigneeEntity>>();
            
            //obj.Data = await consigneeCache.GetList(param);
            obj.Data = await consigneeService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ConsigneeEntity>>> GetPageList(ConsigneeListParam param, Pagination pagination)
        {
            TData<List<ConsigneeEntity>> obj = new TData<List<ConsigneeEntity>>();
            obj.Data = await consigneeService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<ConsigneeEntity>> GetEntity(long id)
        {
            TData<ConsigneeEntity> obj = new TData<ConsigneeEntity>();
            obj.Data = await consigneeService.GetEntity(id);
            //obj.Data = await consigneeCache.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(ConsigneeEntity entity)
        {
            TData<string> obj = new TData<string>();
            //Cache 移除
            await consigneeService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            //Cache 移除
            await consigneeService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        /// 导入货主/收货人
        /// </summary>
        /// <param name="param"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<TData> ImportConsignee(ImportParam param, List<ConsigneeEntity> list)
        {
            TData obj = new TData();
            if (list.Count != 0)
            {
                foreach (ConsigneeEntity entity in list)
                {
                    ConsigneeEntity dbEntity = await consigneeService.GetEntity(entity.ConsigneeName);
                    if (dbEntity != null)
                    {
                        entity.Id = dbEntity.Id;
                        if (param.IsOverride == 1)
                        {
                            await consigneeService.SaveForm(entity);
                            //await RemoveCacheById(entity.Id.Value);
                        }
                    }
                    else
                    {
                        await consigneeService.SaveForm(entity);
                        //await RemoveCacheById(entity.Id.Value);
                    }
                }
                obj.Tag = 1;
            }
            else
            {
                obj.Message = " 未找到导入的数据";
            }
            return obj;
        }
        #endregion
    }
}
