using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace YiAdmin01.DAL.Data
{
    public interface IDatabase
    {
        #region 属性
        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        public DbContext dbContext { get; set; }

        /// <summary>
        /// 事务对象
        /// </summary>
        public IDbContextTransaction dbContextTransaction { get; set; }
        #endregion

        #region 事务方法
        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        Task<IDatabase> BeginTrans();
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        Task<int> CommitTrans();

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns></returns>
        Task RollBackTrans();

        /// <summary>
        /// 关闭
        /// </summary>
        /// <returns></returns>
        Task Close();
        #endregion



        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        Task<int> ExecuteBySql(string strSql);
        /// <summary>
        /// 根据sql+参数执行
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        Task<int> ExecuteBySql(string strSql, params DbParameter[] dbParameter);


        /// <summary>
        /// 添加数据实体
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        Task<int> Insert<T>(T entity) where T : class;
        /// <summary>
        /// 添加多个数据实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> Insert<T>(IEnumerable<T> entities) where T : class;


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<int> Delete<T>() where T : class;
        /// <summary>
        /// 根据实体删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> Delete<T>(T entity) where T : class;
        /// <summary>
        /// 根据实体列表删除多个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> Delete<T>(IEnumerable<T> entities) where T : class;
        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> Delete<T>(long id) where T : class;
        /// <summary>
        /// 根据ID列表删除多个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> Delete<T>(long[] id) where T : class;


        /// <summary>
        /// 根据数据实体更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> Update<T>(T entity) where T : class;
        /// <summary>
        /// 根据数据实体列表更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> Update<T>(IEnumerable<T> entities) where T : class;
        
        
        /// <summary>
        /// 根据KeyValue查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        Task<T> FindEntity<T>(object KeyValue) where T : class;
        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<T>> FindList<T>() where T : class, new();
        /// <summary>
        /// 根据SQL语句查找数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindList<T>(string strSql) where T : class;
        /// <summary>
        /// 分页和排序查询数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sort">指定排序的字段名称</param>
        /// <param name="isAsc">true升序</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">当前页的索引，从1开始</param>
        /// <returns></returns>
        
        
        Task<(int total, IEnumerable<T> list)> FindList<T>(string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new();
        /// <summary>
        /// 根据提供的 SQL 语句、排序字段、排序方式、每页数据条数和当前页索引来查询数据列表，并返回一个包含总记录数和数据列表的元组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"> SQL 语句</param>
        /// <param name="sort">排序字段</param>
        /// <param name="isAsc">排序方式</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <returns></returns>
        Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, string sort, bool isAsc, int pageSize, int pageIndex) where T : class;
        Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex) where T : class;


        /// <summary>
        /// 执行传入的 SQL 查询语句，并将查询结果以 DataTable 的形式返回。
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        Task<DataTable> FindTable(string strSql);
        Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter);
        Task<(int total, DataTable)> FindTable(string strSql, string sort, bool isAsc, int pageSize, int pageIndex);
        Task<(int total, DataTable)> FindTable(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex);


        Task<object> FindObject(string strSql);
        Task<object> FindObject(string strSql, DbParameter[] dbParameter);
        Task<T> FindObject<T>(string strSql) where T : class;
    }
}
