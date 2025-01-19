using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace YiAdmin01.DAL.Data
{
    /// <summary>
    /// 主键约定，把属性Id当做数据库主键
    /// </summary>
    public class PrimaryKeyConvention
    {
        /// <summary>
        /// 将实体的 Id 属性设置为主键
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="entityName"></param>
        public static void SetPrimaryKey(ModelBuilder modelBuilder, string entityName)
        {
            modelBuilder.Entity(entityName).HasKey("Id");
        }
    }
}
