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
using YiAdmin01.Entity;
using YiAdmin01.Model.Param;

namespace YiAdmin01.BLL.Services.SystemManage
{
    /// <summary>
    /// DataDictDetailService
    /// </summary>
    public class DataDictDetailService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<DataDictDetailEntity>> GetList(DataDictDetailListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.OrderBy(p => p.DictSort).ToList();
        }

        public async Task<List<DataDictDetailEntity>> GetPageList(DataDictDetailListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<DataDictDetailEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DataDictDetailEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(DictSort) FROM SysDataDictDetail");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

        public bool ExistDictKeyValue(DataDictDetailEntity entity)
        {
            var expression = LinqExtensions.True<DataDictDetailEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.DictType == entity.DictType && (t.DictKey == entity.DictKey || t.DictValue == entity.DictValue));
            }
            else
            {
                expression = expression.And(t => t.DictType == entity.DictType && (t.DictKey == entity.DictKey || t.DictValue == entity.DictValue) && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(DataDictDetailEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            { // 新增
                await entity.Create();
                await this.BaseRepository().Insert<DataDictDetailEntity>(entity);
            }
            else
            { // 修改
                await entity.Modify();
                await this.BaseRepository().Update<DataDictDetailEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<DataDictDetailEntity>(idArr);
        }
        #endregion

        #region 辅助方法
        private Expression<Func<DataDictDetailEntity, bool>> ListFilter(DataDictDetailListParam param)
        {
            var expression = LinqExtensions.True<DataDictDetailEntity>();
            if (param != null)
            {
                if (param.DictKey.ParseToInt() > 0)
                {
                    expression = expression.And(t => t.DictKey == param.DictKey);
                }

                if (!string.IsNullOrEmpty(param.DictValue))
                {
                    expression = expression.And(t => t.DictValue.Contains(param.DictValue));
                }

                if (!string.IsNullOrEmpty(param.DictType))
                {
                    expression = expression.And(t => t.DictType.Contains(param.DictType));
                }
            }
            return expression;
        }
        #endregion
    }
}
