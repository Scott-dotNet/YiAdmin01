using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Entity
{
    [Table("SysArea")]
    public class AreaEntity : BaseExtensionEntity
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string AreaCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string ParentAreaCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string AreaName { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string ZipCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int? AreaLevel { get; set; }
    }

    /// <summary>
    /// 此类给其他需要省市县的业务表继承
    /// </summary>
    public class BaseAreaEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 省份ID
        /// </summary>
        /// <returns></returns>
        public long? ProvinceId { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        /// <returns></returns>
        public long? CityId { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        /// <returns></returns>
        public long? CountyId { get; set; }

        [NotMapped]
        public string ProvinceName { get; set; }

        [NotMapped]
        public string CityName { get; set; }

        [NotMapped]
        public string CountryName { get; set; }

        [NotMapped]
        public string AreaId { get; set; }
    }
}
