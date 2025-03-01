using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;
using YiAdmin01.Common.Configs;
using YiAdmin01.Common.Extension;
using YiAdmin01.Common.Utils;
using YiAdmin01.DAL.Repository;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;

namespace YiAdmin01.BLL.Services.InfoService
{
    public class AppointmentService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<AppointmentEntity>> GetList(AppointmentListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AppointmentEntity>> GetPageList(AppointmentListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AppointmentEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AppointmentEntity>(id);
        }

        #endregion

        #region 提交数据
        public async Task<long> SaveNewForm(AppointmentEntity entity)
        {
            await entity.Create();
            await this.BaseRepository().Insert(entity);
            return entity.Id.Value;
        }


        public async Task SaveForm(AppointmentEntity entity)
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
            await this.BaseRepository().Delete<AppointmentEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<AppointmentEntity, bool>> ListFilter(AppointmentListParam param)
        {
           

            //****根据查询字段自动过滤条件****
            var expression = LinqExtensions.GetExpressionItems<AppointmentEntity, AppointmentListParam>(param);

            if (param != null)
            {
                
                if (param.AppointmentStatus > -1)
                {
                    expression = expression.And(t => t.AppointmentStatus == param.AppointmentStatus);
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
