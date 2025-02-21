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

        public string CompanyCnName { get; set; }

        public string Tel { get; set; }
        public int? ConsigneeStatus { get; set; }

        /// <summary>
        /// 多个供应商Id
        /// </summary>
        //public string SupplierIds { get; set; }


    }
}
