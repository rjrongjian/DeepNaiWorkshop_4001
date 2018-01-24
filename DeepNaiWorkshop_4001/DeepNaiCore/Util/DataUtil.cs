using System;
using System.Collections.Generic;
using System.Text;

namespace DeepNaiCore.Util
{
    class DataUtil
    {
        /// <summary>
        /// 去除浮点数后面的无效0，例如：2.50
        /// </summary>
        /// <param name="floatStr"></param>
        /// <returns></returns>
        public static String DeleteFloatZero(String floatStr)
        {
            decimal dec = decimal.Parse(floatStr);
            string s = dec.ToString().TrimEnd('0');
            if (s.EndsWith(".")) s = s.Substring(0, s.Length - 1);

            if (String.IsNullOrEmpty(s))
            {
                return "0";
            }
            else
            {
                return s;
            }
            
        }

    }
}
