using System.Linq.Expressions;
using YiAdmin01.Common.Configs;
using YiAdmin01.Common.Extension;
using YiAdmin01.Common.Utils;
using YiAdmin01.DAL.Repository;
using YiAdmin01.Entity;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;

namespace YiAdmin01.BLL.Services.InfoService
{
    /// <summary>
    /// 供应商服务类
    /// </summary>
    public class SupplierService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<SupplierEntity>> GetList(SupplierListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<SupplierEntity>> GetPageList(SupplierListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<SupplierEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<SupplierEntity>(id);
        }

        public async Task<SupplierEntity> GetEntity(string SupplierName)
        {
            return await BaseRepository().FindEntity<SupplierEntity>(p => p.CompanyCnName == SupplierName);
        }

        public bool ExistSupplierName(SupplierEntity entity)
        {
            var expression = LinqExtensions.True<SupplierEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            { //新增
                expression = expression.And(t => t.CompanyCnName == entity.CompanyCnName);
            }
            else
            {//编辑
                expression = expression.And(t => t.CompanyCnName == entity.CompanyCnName && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }

        #endregion

        #region 提交数据
        public async Task SaveForm(SupplierEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                await entity.Modify();
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<SupplierEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<SupplierEntity, bool>> ListFilter(SupplierListParam param)
        {
            //var expression = LinqExtensions.True<SupplierEntity>();
            if (param.SupplierStatus == -1)
            {
                param.SupplierStatus = null;
            }

            //****根据查询字段自动过滤条件****
            var expression = LinqExtensions.GetExpressionItems<SupplierEntity, SupplierListParam>(param);

            if (param != null)
            {
                //if (!string.IsNullOrEmpty(param.CompanyCnName))
                //{
                //    expression = expression.And(t => t.CompanyCnName.Contains(param.CompanyCnName));
                //}

                if (!string.IsNullOrEmpty(param.SupplierIds))
                {
                    long[] supplierIdList = TextHelper.SplitToArray<long>(param.SupplierIds, ',');
                    expression = expression.And(t => supplierIdList.Contains(t.Id.Value));
                }

                //if (param.SupplierStatus > -1)
                //{
                //    expression = expression.And(t => t.SupplierStatus == param.SupplierStatus);
                //}
                if (!string.IsNullOrEmpty(param.StartTime.ParseToString()))
                {
                    expression = expression.And(t => t.BaseModifyTime >= param.StartTime);
                }
                if (!string.IsNullOrEmpty(param.EndTime.ParseToString()))
                {
                    param.EndTime = param.EndTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                    expression = expression.And(t => t.BaseModifyTime <= param.EndTime);
                }
            }
            return expression;
        }
        #endregion
    }
}
