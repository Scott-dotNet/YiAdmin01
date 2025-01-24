using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Model.Param
{
    public class NewsListParam : BaseAreaParam
    {
        public string NewsTitle { get; set; }
        public int? NewsType { get; set; }
        public string NewsTag { get; set; }
    }
}
