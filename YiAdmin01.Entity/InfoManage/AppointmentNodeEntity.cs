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
    [Table("SysAppointmentNode")]
    public class AppointmentNodeEntity : BaseExtensionEntity
    {
       
        public int? CurrentNodeCode { get; set; }
        
        public string CurrentNodeName { get; set; }
        
        [JsonConverter(typeof(StringJsonConverter))]
        public long? OperatorId { get; set; }
       
        public string OperatorName { get; set; }
       
        public int? PrevNodeCode { get; set; }
        
        public string PrevNodeName { get; set; }

        public int? NextNodeCode { get; set; }

        public string NextNodeName { get; set; }
        public string Remark { get; set; }
        
        public int? NodeStatus { get; set; }
    }
}
