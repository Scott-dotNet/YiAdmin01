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
    public class AppointmentInstanceService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<AppointmentInstanceEntity>> GetList(AppointmentInstanceListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AppointmentInstanceEntity>> GetPageList(AppointmentInstanceListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AppointmentInstanceEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AppointmentInstanceEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(AppointmentInstanceEntity entity)
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

        public async Task<AppointmentInstanceEntity> SaveExsitForm(AppointmentInstanceEntity entity)
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
            return entity;
        }

        public async Task<long> SaveNewForm(AppointmentInstanceEntity entity)
        {
            await entity.Create();
            await this.BaseRepository().Insert(entity);
            return entity.Id.Value;
        }
        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<AppointmentInstanceEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<AppointmentInstanceEntity, bool>> ListFilter(AppointmentInstanceListParam param)
        {
            var expression = LinqExtensions.True<AppointmentInstanceEntity>();
            if (param != null)
            {
                if (param.AppointmentId != 0)
                {
                    expression = expression.And(t => t.AppointmentId == param.AppointmentId);
                }
            }
            return expression;
        }
        #endregion
    }
}
