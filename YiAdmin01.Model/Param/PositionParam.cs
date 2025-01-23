using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Model.Param
{
    public class PositionListParam
    {
        public string PositionName { get; set; }
        /// <summary>
        /// 多个职位Id
        /// </summary>
        public string PositionIds { get; set; }
    }
}
