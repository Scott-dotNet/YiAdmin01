using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YiAdmin01.Common.Extension;
using YiAdmin01.Common.Utils;
using YiAdmin01.DAL.Repository;
using YiAdmin01.Entity;
using YiAdmin01.Model.Param;

namespace YiAdmin01.BLL.Services.OrganizationManage
{
    public class MenuService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<MenuEntity>> GetList(MenuListParam param)
        {
            var expression = ListFilter(param);
            var list = await BaseRepository().FindList(expression);
            return list.OrderBy(p => p.MenuSort).ToList();
        }

        public async Task<MenuEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<MenuEntity>(id);
        }

        public async Task<int> GetMaxSort(long parentId)
        {
            string where = string.Empty;
            if (parentId > 0)
            {
                where += " AND ParentId = " + parentId;
            }
            object result = await BaseRepository().FindObject("SELECT MAX(MenuSort) FROM SysMenu where BaseIsDelete = 0 " + where);
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

        public bool ExistMenuName(MenuEntity entity)
        {
            var expression = LinqExtensions.True<MenuEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.MenuName == entity.MenuName && t.MenuType == entity.MenuType);
            }
            else
            {
                expression = expression.And(t => t.MenuName == entity.MenuName && t.MenuType == entity.MenuType && t.Id != entity.Id);
            }
            return BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(MenuEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await BaseRepository().Insert(entity);
            }
            else
            {
                await entity.Modify();
                await BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            var db = await BaseRepository().BeginTrans();
            try
            {
                long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
                await db.Delete<MenuEntity>(p => idArr.Contains(p.Id.Value) || idArr.Contains(p.ParentId.Value));
                await db.Delete<MenuAuthorizeEntity>(p => idArr.Contains(p.MenuId.Value));
                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
        }
        #endregion

        #region 私有方法
        private Expression<Func<MenuEntity, bool>> ListFilter(MenuListParam param)
        {
            var expression = LinqExtensions.True<MenuEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.MenuName))
                {
                    expression = expression.And(t => t.MenuName.Contains(param.MenuName));
                }
                if (param.MenuStatus > -1)
                {
                    expression = expression.And(t => t.MenuStatus == param.MenuStatus);
                }
            }
            return expression;
        }
        #endregion

    }
}
