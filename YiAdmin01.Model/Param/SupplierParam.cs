using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Model.Param
{
    public class SupplierListParam : DateTimeParam
    {
        public string SupplierName { get; set; }
        public string Tel { get; set; }
        public int? SupplierStatus { get; set; }
        /// <summary>
        /// SupplierIds
        /// </summary>
        public string SupplierIds { get; set; }
    }
}
