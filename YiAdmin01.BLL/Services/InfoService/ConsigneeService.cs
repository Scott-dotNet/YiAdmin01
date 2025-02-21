using System.Linq.Expressions;
using YiAdmin01.Common.Configs;
using YiAdmin01.Common.Extension;
using YiAdmin01.Common.Utils;
using YiAdmin01.DAL.Repository;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;

namespace YiAdmin01.BLL.Services.InfoService
{
    /// <summary>
    /// 货主/收货人服务类
    /// </summary>
    public class ConsigneeService : RepositoryFactory
    {
        #region 获取数据
        /// <summary>
        /// 获取货主/收货人列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ConsigneeEntity>> GetList(ConsigneeListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        /// <summary>
        /// 获取货主/收货人分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<List<ConsigneeEntity>> GetPageList(ConsigneeListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        /// <summary>
        /// 获取货主/收货人实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ConsigneeEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<ConsigneeEntity>(id);
        }

        /// <summary>
        /// 判断货主/收货人名称是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExistConsigneeName(ConsigneeEntity entity)
        {
            var expression = LinqExtensions.True<ConsigneeEntity>();
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
        /// <summary>
        /// 保存（新增或修改）货主/收货人实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task SaveForm(ConsigneeEntity entity)
        {//新增
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {//修改
                await entity.Modify();
                await this.BaseRepository().Update(entity);
            }
        }

        /// <summary>
        /// 删除货主/收货人实体
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<ConsigneeEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<ConsigneeEntity, bool>> ListFilter(ConsigneeListParam param)
        {
            var expression = LinqExtensions.True<ConsigneeEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.CompanyCnName))
                {
                    expression = expression.And(t => t.CompanyCnName.Contains(param.CompanyCnName));
                }

                if (!string.IsNullOrEmpty(param.Tel))
                {
                    expression = expression.And(t => t.Tel.Contains(param.Tel));
                }

                if (param.ConsigneeStatus > -1)
                {
                    expression = expression.And(t => t.ConsigneeStatus == param.ConsigneeStatus);
                }
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
