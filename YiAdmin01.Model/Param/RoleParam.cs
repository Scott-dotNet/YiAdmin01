using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Model.Param
{
    public class RoleListParam : DateTimeParam
    {
        public string RoleName { get; set; }

        public int? RoleStatus { get; set; }

        /// <summary>
        /// 多个角色Id
        /// </summary>
        public string RoleIds { get; set; }
    }
}
