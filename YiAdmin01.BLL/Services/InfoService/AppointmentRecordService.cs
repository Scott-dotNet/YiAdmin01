using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YiAdmin01.Common.Configs;
using YiAdmin01.Common.Extension;
using YiAdmin01.Common.Utils;
using YiAdmin01.DAL.Repository;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;

namespace YiAdmin01.BLL.Services.InfoService
{
    public class AppointmentRecordService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<AppointmentRecordEntity>> GetList(AppointmentRecordListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AppointmentRecordEntity>> GetPageList(AppointmentRecordListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AppointmentRecordEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AppointmentRecordEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(AppointmentRecordEntity entity)
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
            await this.BaseRepository().Delete<AppointmentRecordEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<AppointmentRecordEntity, bool>> ListFilter(AppointmentRecordListParam param)
        {
            var expression = LinqExtensions.True<AppointmentRecordEntity>();
            if (param != null)
            {
                if (param.ApprovedStatus > -1)
                {
                    expression = expression.And(t => t.ApprovedStatus == param.ApprovedStatus);
                }
            }
            return expression;
        }
        #endregion
    }
}
