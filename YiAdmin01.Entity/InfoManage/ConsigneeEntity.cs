
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using YiAdmin01.Common.Utils;


namespace YiAdmin01.Entity.InfoManage
{
    /// <summary>
    /// 货主/收货人实体
    /// </summary>
    /// 
    [Table("SysConsignee")]
    public class ConsigneeEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 货主代码
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? ConsigneeCode { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        /// <returns></returns>
        public string Abbreviation { get; set; }
     
        /// <summary>
        ///  中文名
        /// </summary>
        /// <returns></returns>
        public string CompanyCnName { get; set; }
        /// <summary>
        /// 中文地址
        /// </summary>
        /// <returns></returns>
        public string CnAddress { get; set; }
     
        /// <summary>
        ///  联系人
        /// </summary>
        /// <returns></returns>
        public string ContactPerson { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
        public string Tel { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        /// <returns></returns>
        public string Fax { get; set; }
        /// <summary>
        ///  Email
        /// </summary>
        /// <returns></returns>
        public string Email { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        /// <returns></returns>
        public string City { get; set; }
        /// <summary>
        /// 所在大区
        /// </summary>
        /// <returns></returns>
        public string Area { get; set; }
        /// <summary>
        /// 备用代码
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long RefCode { get; set; }
        /// <summary>
        /// 备用名称
        /// </summary>
        /// <returns></returns>
        public string RefName { get; set; }
        /// <summary>
        /// 母公司编码
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long ParentCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }

        /// <summary>
        /// 状态  1启用；0禁用
        /// </summary>
        /// <returns></returns>
        public int? ConsigneeStatus { get; set; }
    }
}
