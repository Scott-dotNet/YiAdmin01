using YiAdmin01.Common.Extension;
using YiAdmin01;
using YiAdmin01.DAL.Repository;
using YiAdmin01.Entity;




namespace YiAdmin01.BLL.Services.OrganizationManage
{
    public class UserBelongService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<UserBelongEntity>> GetList(UserBelongEntity entity)
        {
            var expression = LinqExtensions.True<UserBelongEntity>();
            if (entity != null)
            {
                if (entity.BelongType != null)
                {
                    expression = expression.And(t => t.BelongType == entity.BelongType);
                }
                if (entity.UserId != null)
                {
                    expression = expression.And(t => t.UserId == entity.UserId);
                }
            }
            var list = await BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<UserBelongEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<UserBelongEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(UserBelongEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await BaseRepository().Insert(entity);
            }
            else
            {
                await BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(long id)
        {
            await BaseRepository().Delete<UserBelongEntity>(id);
        }
        #endregion
    }
}
