using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.DAL.Enum
{
    /// <summary>
    /// 用户所属类别枚举类
    /// </summary>
    public enum UserBelongTypeEnum
    {
        [Description("职位")]
        Position = 1,

        [Description("角色")]
        Role = 2
    }
}
