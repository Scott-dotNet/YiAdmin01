using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Model.Param
{
    public class AppointmentListParam : DateTimeParam
    {
        public long? AppointmentId { get; set; }
        public int? AppointmentStatus { get; set; }
    }
}
