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
    /// 数据字典服务类
    /// </summary>
    public class DataDictService : RepositoryFactory
    {
        #region 获取数据
        /// <summary>
        /// 获取 DataDictEntity 列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<DataDictEntity>> GetList(DataDictListParam param)
        {
            Expression<Func<DataDictEntity, bool>> expression = ListFilter(param);
            IEnumerable<DataDictEntity> list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<DataDictEntity>> GetPageList(DataDictListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            IEnumerable<DataDictEntity> list = await BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }
        /// <summary>
        /// 根据ID查找数据字典实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DataDictEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DataDictEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(DictSort) FROM SysDataDict");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

        /// <summary>
        /// 是否存在字典类型
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExistDictType(DataDictEntity entity)
        {
            var expression = LinqExtensions.True<DataDictEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.DictType == entity.DictType);
            }
            else
            {
                expression = expression.And(t => t.DictType == entity.DictType && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0;
        }

        /// <summary>
        /// 是否存在字典值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExistDictDetail(string dictType)
        {
            var expression = LinqExtensions.True<DataDictDetailEntity>();
            expression = expression.And(t => t.DictType == dictType);
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增或修改 表单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task SaveForm(DataDictEntity entity)
        {
            var db = await this.BaseRepository().BeginTrans();
            try
            {
                if (!entity.Id.IsNullOrZero())
                { // 修改
                    //获得旧数据字典实体
                    DataDictEntity dataDictEntity = await db.FindEntity<DataDictEntity>(entity.Id.Value);
                    if (dataDictEntity.DictType != entity.DictType)
                    {
                        //更新子表的DictType，因为2个表用DictType进行关联
                        var detailList = await db.FindList<DataDictDetailEntity>(p => p.DictType == dataDictEntity.DictType);
                        foreach (var detailEntity in detailList) 
                        { 
                            detailEntity.DictType = entity.DictType;
                            await detailEntity.Modify();
                        }
                    }
                    dataDictEntity.DictType = entity.DictType;
                    dataDictEntity.Remark = entity.Remark;
                    dataDictEntity.DictSort = entity.DictSort;
                    await dataDictEntity.Modify();
                    await db.Update<DataDictEntity>(dataDictEntity);
                }
                else
                {// 新建
                    await entity.Create();
                    await db.Insert(entity);
                }
                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }

        }

        /// <summary>
        /// 删除表单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteForm(string ids)
        {
            long[] idArray = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete(idArray);
        }


        #endregion

        #region 辅助方法
        private Expression<Func<DataDictEntity, bool>> ListFilter(DataDictListParam param)
        {
            Expression<Func<DataDictEntity, bool>> expression = LinqExtensions.True<DataDictEntity>();
            if (param != null) 
            {
                if (!param.DictType.IsEmpty())
                {
                    expression = expression.And(t => t.DictType.Contains(param.DictType));
                }
                if (!param.Remark.IsEmpty())
                {
                    expression = expression.And(t => t.Remark.Contains(param.Remark));
                }
            }
            return expression;
        }
        #endregion
    }
}
