using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools
{
    public class DateTool
    {

        public static string getCurrentTimeStr()
        {
            return DateTime.Now.ToString();
        }

        /// <summary>  
        /// 获取当前时间的时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetCurrentTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
    }
}
