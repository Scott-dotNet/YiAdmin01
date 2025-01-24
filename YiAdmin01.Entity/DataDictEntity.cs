using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Entity
{
    [Table("SysDataDict")]
    public class DataDictEntity : BaseExtensionEntity
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
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
    }
}
