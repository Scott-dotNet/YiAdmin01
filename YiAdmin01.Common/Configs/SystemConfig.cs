using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Common.Configs
{
    /// <summary>
    ///  系统配置
    /// </summary>
    public class SystemConfig
    {
        public SystemConfig(int dBSlowSqlLogTime)
        {
            DBSlowSqlLogTime = 5;
        }


        /// <summary>
        /// 是否是调试模式
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// 数据库供应商
        /// </summary>
        public string DBProvider { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DBConnectionString { get; set; }

        /// <summary>
        ///  数据库超时间（秒）
        /// </summary>
        public int DBCommandTimeout { get; set; }

        /// <summary>
        /// 慢查询记录Sql(秒),保存到文件以便分析
        /// </summary>
        public int DBSlowSqlLogTime { get; set; }

        /// <summary>
        /// 网站虚拟目录
        /// </summary>
        public string VirtualDirectory { get; set; }

        /// <summary>
        /// 登录信息保存方式
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public int SnowFlakeWorkerId { get; set; }
        /// <summary>
        /// 是否允许一个账户在多处登录
        /// </summary>
        public bool LoginMultiple { get; set; }

        /// <summary>
        /// api地址
        /// </summary>
        public string ApiSite { get; set; }
    }
}
