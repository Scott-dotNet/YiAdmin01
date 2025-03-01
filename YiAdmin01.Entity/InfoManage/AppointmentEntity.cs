using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Entity.InfoManage
{
    /// <summary>
    /// 预约信息实体
    /// </summary>
    [Table("SysAppointment")]
    public class AppointmentEntity : BaseExtensionEntity
    {
   
        public string ConsigneeName { get; set; }
       
        public string SupplierName { get; set; }
        
        public string CargoName { get; set; }

     
        [Description("类型：1:普通 2:危险品")]
        public int CargoType { get; set; }

        public int? CargoWeight { get; set; }

        [Description("重量单位：1:吨 2:千克 3:克")]
        public int WeightUnit { get; set; }

        public string Volume { get; set; }

        public string ArrivalDate { get; set; }

        [Description("到达时间：1:上午 2:下午 3:晚上")]
        public int ArrivalTime { get; set; }
       
        public string Remark { get; set; }
                
        public int? AppointmentStatus { get; set; }
    }
}
