using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools
{
    public class ColorTool
    {
        /// <summary>
        /// 转化16进制颜色值 
        /// </summary>
        /// <param name="hexValue">形如："#FF0000"</param>
        /// <returns></returns>
        public static Color getColorFromHtml(String hexValue)
        {
            return ColorTranslator.FromHtml(hexValue);
        }

    }
}
