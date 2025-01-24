using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Model.Param
{
    public class AreaListParam
    {
        public string AreaName { get; set; }
    }

    public class BaseAreaParam : BaseApiToken
    {
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? CountyId { get; set; }
        /// <summary>
        /// 逗号分隔的Id
        /// </summary>
        public string AreaIds { get; set; }
    }
}
