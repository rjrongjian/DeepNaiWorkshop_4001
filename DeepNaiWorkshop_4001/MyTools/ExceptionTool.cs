using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools
{
    public class ExceptionTool
    {

        public static void log(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
