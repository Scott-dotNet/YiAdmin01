using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Model.Param
{
    public class ConsigneeListParam : DateTimeParam
    {

        public string ConsigneeName { get; set; }

        public string Tel { get; set; }
        public int? ConsigneeStatus { get; set; }

        /// <summary>
        /// ConsigneeIds
        /// </summary>
        public string ConsigneeIds { get; set; }


    }
}
