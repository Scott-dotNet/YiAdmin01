using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using YiAdmin01.Common.Extension;

namespace YiAdmin01.DAL.Data
{
    public class SqlServerDatabase : IDatabase
    {
        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        public DbContext dbContext { get; set; }
        /// <summary>
        /// 事务对象
        /// </summary>
        public IDbContextTransaction dbContextTransaction { get; set; }

        #region 构造函数

        public SqlServerDatabase(string connString)
        { 
            dbContext  = new SqlServerDbContext(connString);
        }
        #endregion


        #region 事务提交
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IDatabase> BeginTrans()
        {
            DbConnection dbConn = dbContext.Database.GetDbConnection();
            if (dbConn.State == ConnectionState.Closed) 
            { 
                await dbConn.OpenAsync();
            }
            dbContextTransaction = await dbContext.Database.BeginTransactionAsync();
            return this;
        }
        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> CommitTrans()
        {
            try
            {
                DbContextExtension.SetEntityDefaultValue(dbContext);

                int returnValue = await dbContext.SaveChangesAsync();
                if (dbContextTransaction != null)
                {
                    await dbContextTransaction.CommitAsync();
                    await this.Close();
                }
                else
                {
                    await this.Close();
                }
                return returnValue;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dbContextTransaction == null) { await this.Close();}
            }
        }
        /// <summary>
        /// 事务回滚
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task RollBackTrans()
        {
            await this.dbContextTransaction.RollbackAsync();
            await this.dbContextTransaction.DisposeAsync();
            await this.Close();
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Close()
        {
            await dbContext.DisposeAsync();
        }
        #endregion

        #region 执行 SQL 语句
        public async Task<int> ExecuteBySql(string strSql)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(strSql);
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(strSql);
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }

        public async Task<int> ExecuteBySql(string strSql, params DbParameter[] dbParameter)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(strSql, dbParameter);
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(strSql, dbParameter);
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }

        #endregion


        #region 添加、修改、删除
        public async Task<int> Insert<T>(T entity) where T : class
        {
            dbContext.Entry<T>(entity).State = EntityState.Added;
            return dbContextTransaction == null ? await this.CommitTrans() : 0;   
        }

        public async Task<int> Insert<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Entry<T>(entity).State = EntityState.Added;
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        public async Task<int> Update<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            Hashtable props = DatabasesExtension.GetPropertyInfo<T>(entity);
            foreach (string item in props.Keys)
            {
                if (item == "Id")
                {
                    continue;
                }
                object value = dbContext.Entry(entity).Property(item).CurrentValue;
                if (value != null)
                {
                    dbContext.Entry(entity).Property(item).IsModified = true;
                }
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        public async Task<int> Update<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Entry<T>(entity).State = EntityState.Modified;
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> Delete<T>() where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName));
            }
            return -1;
        }

        public async Task<int> Delete<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Set<T>().Remove(entity);
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        public async Task<int> Delete<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Set<T>().Attach(entity);
                dbContext.Set<T>().Remove(entity);
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        public async Task<int> Delete<T>(long keyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                string keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }

        public async Task<int> Delete<T>(long[] keyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                string keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }

        #endregion

        #region 对象实体 查询
        public async Task<T> FindEntity<T>(object KeyValue) where T : class
        {
            return await dbContext.Set<T>().FindAsync(KeyValue);
        }

        public async Task<IEnumerable<T>> FindList<T>() where T : class, new()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindList<T>(string strSql) where T : class
        {
            return await FindList<T>(strSql, null);
        }

        public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter) where T : class
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                var reader = await new DbHelper(dbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
                return DatabasesExtension.IDataReaderToList<T>(reader);
            }
        }


        public async Task<(int total, IEnumerable<T> list)> FindList<T>(string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new()
        {
            var tempData = dbContext.Set<T>().AsQueryable();
            return await FindList<T>(tempData, sort, isAsc, pageSize, pageIndex);
        }

        public async Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, string sort, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            return await FindList<T>(strSql, null, sort, isAsc, pageSize, pageIndex);

        }

        public async Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                DbHelper dbHelper = new DbHelper(dbContext, dbConnection);
                StringBuilder sb = new StringBuilder();
                sb.Append(DatabasePageExtension.SqlServerPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex));
                object tempTotal = await dbHelper.ExecuteScalarAsync(CommandType.Text, "SELECT COUNT(1) FROM (" + strSql + ") T", dbParameter);
                int total = tempTotal.ParseToInt();
                if (total > 0)
                {
                    var reader = await dbHelper.ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                    return (total, DatabasesExtension.IDataReaderToList<T>(reader));
                }
                else
                {
                    return (total, new List<T>());
                }
            }
        }
        #endregion

        #region 数据源查询
        public async Task<DataTable> FindTable(string strSql)
        {
            return await FindTable(strSql, null);
        }
        public async Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                var reader = await new DbHelper(dbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
                return DatabasesExtension.IDataReaderToDataTable(reader);
            }
        }
        public async Task<(int total, DataTable)> FindTable(string strSql, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            return await FindTable(strSql, null, sort, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, DataTable)> FindTable(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                DbHelper dbHelper = new DbHelper(dbContext, dbConnection);
                StringBuilder sb = new StringBuilder();
                sb.Append(DatabasePageExtension.SqlServerPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex));
                object tempTotal = await dbHelper.ExecuteScalarAsync(CommandType.Text, "SELECT COUNT(1) FROM (" + strSql + ") T", dbParameter);
                int total = tempTotal.ParseToInt();
                if (total > 0)
                {
                    var reader = await dbHelper.ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                    DataTable resultTable = DatabasesExtension.IDataReaderToDataTable(reader);
                    return (total, resultTable);
                }
                else
                {
                    return (total, new DataTable());
                }
            }
        }

        public async Task<object> FindObject(string strSql)
        {
            return await FindObject(strSql, null);
        }
        public async Task<object> FindObject(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                return await new DbHelper(dbContext, dbConnection).ExecuteScalarAsync(CommandType.Text, strSql, dbParameter);
            }
        }
        public async Task<T> FindObject<T>(string strSql) where T : class
        {
            var list = await dbContext.SqlQuery<T>(strSql);
            return list.FirstOrDefault();
        }
        #endregion

        private async Task<(int total, IEnumerable<T> list)> FindList<T>(IQueryable<T> tempData, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            tempData = DatabasesExtension.AppendSort<T>(tempData, sort, isAsc);
            var total = tempData.Count();
            if (total > 0)
            {
                tempData = tempData.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize).AsQueryable();
                var list = await tempData.ToListAsync();
                return (total, list);
            }
            else
            {
                return (total, new List<T>());
            }
        }
    }
}