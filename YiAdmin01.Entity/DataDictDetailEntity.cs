using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Entity
{
    [Table("SysDataDictDetail")]
    public  class DataDictDetailEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 类型
        /// </summary>
        /// <returns></returns>
        public string DictType { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        /// <returns></returns>
        public int? DictSort { get; set; }

        /// <summary>
        /// 字典键
        /// </summary>
        /// <returns></returns>
        public int? DictKey { get; set; }

        /// <summary>
        /// 字典值
        /// </summary>
        /// <returns></returns>
        public string DictValue { get; set; }

        public string ListClass { get; set; }
        public int? DictStatus { get; set; }
        public int? IsDefault { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
    }
}
