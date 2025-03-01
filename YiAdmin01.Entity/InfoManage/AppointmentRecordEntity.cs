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
    [Table("SysAppointmentRecord")]
    public class AppointmentRecordEntity : BaseExtensionEntity
    {
       
        public int? CurrentNodeCode { get; set; }
     
        public string CurrentNodeName { get; set; }
      
        [JsonConverter(typeof(StringJsonConverter))]
        public long? OperatorId { get; set; }
       
        public string OperatorName { get; set; }
        /// <summary>
        /// 1:同意，2：拒绝
        /// </summary>
        public int ResultStatus { get; set; }
        
        /// <summary>
        /// 1：已审批，2：未审批
        /// </summary>
        public int? ApprovedStatus { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public long? InstanceId { get; set; }
    }
}
