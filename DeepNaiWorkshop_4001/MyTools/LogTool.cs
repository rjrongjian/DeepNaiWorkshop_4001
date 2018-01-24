using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MyTools
{
    public class LogTool
    {
        /*
         *  向label输入内容
         */ 
        public static void log(String content,Label label)
        {
            label.Text = content;
        }

        public static void logInFile(string errorInfo)
        {
            Console.WriteLine(errorInfo);
        }
    }
}
