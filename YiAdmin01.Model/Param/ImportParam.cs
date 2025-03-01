using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Model.Param
{
    public class ImportParam
    {
        /// <summary>
        /// 导入文件上传服务器后的路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 是否更新已有的数据
        /// </summary>
        public int? IsOverride { get; set; }
    }
}
