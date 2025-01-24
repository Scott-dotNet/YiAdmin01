using YiAdmin01.Common.Global;
using YiAdmin01.DAL.Data;
using YiAdmin01.DAL.Enum;

namespace YiAdmin01.DAL.Repository
{
    /// <summary>
    /// 仓储工厂
    /// </summary>
    public class RepositoryFactory
    {
        public MyRepository BaseRepository()
        {
            IMyDatabase database = null;
            string dbType = GlobalContext.SystemConfig.DBProvider;
            string dbConnectionString = GlobalContext.SystemConfig.DBConnectionString;

            switch (dbType)
            {
                case "SqlServer":
                    DbHelper.DbType = DatabaseType.SqlServer;
                    database = new SqlServerDatabase(dbConnectionString);
                    break;
                case "MySql":
                //TBD
                case "Oracle":
                //TBD
                default:
                    throw new Exception("未找到数据库配置");
            }
            return new MyRepository(database);
        }
    }
}

