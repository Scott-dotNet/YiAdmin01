using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using YiAdmin01.Common.Global;

namespace YiAdmin01.DAL.Data
{
    /// <summary>
    /// sqlserver数据库环境设置
    /// </summary>
    public class SqlServerDbContext : DbContext
    {
        //日志
        private static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        private string ConnectionString { get; set; }

        #region 构造函数
        public SqlServerDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }
        #endregion

        #region Override
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString, p=>p.CommandTimeout(GlobalContext.SystemConfig.DBCommandTimeout));
            // 此处SQL语句拦截器为空，可自定义 DbCommandCustomInterceptor : DbCommandInterceptor
            optionsBuilder.AddInterceptors();
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            // 这里需要注意，不能采用这种写法：optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            // 会导致内存泄露的问题
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Assembly entityAssembly = Assembly.Load("YiAdmin01.Entity");
            IEnumerable<Type> typesToRegister = entityAssembly.GetTypes().Where(p => !string.IsNullOrEmpty(p.Namespace))
                                                                    .Where(p => !string.IsNullOrEmpty(p.GetCustomAttribute<TableAttribute>()?.Name));

            foreach (Type type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Model.AddEntityType(type);
            }
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                PrimaryKeyConvention.SetPrimaryKey(modelBuilder, entity.Name);
                string currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetTableName();
                modelBuilder.Entity(entity.Name).ToTable(currentTableName);               
            }

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
