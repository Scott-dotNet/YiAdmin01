using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YiAdmin01.Common.Utils;

namespace YiAdmin01.Entity.InfoManage
{
    /// <summary>
    /// 供应商实体
    /// </summary>
    [Table("SysSupplier")]
    public class SupplierEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 供应商代码
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? SupplierCode { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        /// <returns></returns>
        public string Abbreviation { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public string SupplierName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        /// <returns></returns>
        public string CnAddress { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
        public string ContactPerson { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
        public string? Tel { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        /// <returns></returns>
        public string? Fax { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        /// <returns></returns>
        public string Email { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 状态  1启用；0禁用
        /// </summary>
        /// <returns></returns>
        public int? SupplierStatus { get; set; }
    }
}
