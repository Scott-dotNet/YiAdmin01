using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiAdmin01.Common.Extension;

namespace YiAdmin01.Common.Utils
{
    /// <summary>
    /// Http连接操作帮助类 
    /// </summary>
    public  class HttpHelper
    {
        /// <summary>
        /// 是否是网站
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            url = url.ParseToString().ToLower();
            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
