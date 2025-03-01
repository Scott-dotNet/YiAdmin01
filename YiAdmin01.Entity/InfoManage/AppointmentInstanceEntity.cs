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
    [Table("SysAppointmentInstance")]
    public class AppointmentInstanceEntity : BaseExtensionEntity
    {
        
        public int? CurrentNodeCode { get; set; }
       
        public string CurrentNodeName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        public int? AppointmentStatus { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public long? StarterId { get; set; }
        /// <summary>
        /// 发起者
        /// </summary>
        /// <returns></returns>
        public string Starter { get; set; }
        
        [JsonConverter(typeof(StringJsonConverter))]
        public long? OperatorId { get; set; }
        /// <summary>
        /// 当前审批人
        /// </summary>
        /// <returns></returns>
        public string Operator { get; set; }
        
        [JsonConverter(typeof(StringJsonConverter))]
        public long? NextOperatorId { get; set; }
        /// <summary>
        /// 下一个审批人
        /// </summary>
        /// <returns></returns>
        public string NextOperator { get; set; }
        /// <summary>
        /// 已审批人
        /// </summary>
        /// <returns></returns>
        public string OperatedNames { get; set; }
       
        public string Remark { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public long? AppointmentId { get; set; }
    }
}
