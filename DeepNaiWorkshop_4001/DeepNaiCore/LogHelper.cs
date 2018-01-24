using System.IO;
using System.Windows.Forms;

namespace DeepNaiCore
{
    public class LogHelper
    {
        /// <summary>
        /// txt写出
        /// </summary>
        /// <param name="txt">字符串</param>
        /// <param name="file">路径</param>
        public static void WriteTxt(string txt, string file)
        {
            FileStream fs = new FileStream(Application.StartupPath + "\\" + file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(txt);
            sw.Close();
            fs.Close();

        }
        public static void WriteTxtAllpath(string txt, string file)
        {
            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(txt);
            sw.Close();
            fs.Close();
        }
        public static void AppendTxtAllpath(string txt, string file)
        {
            FileStream fs = new FileStream(file, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(txt);
            sw.Close();
            fs.Close();
        }

        public static string ReadTxt(string file)
        {
            FileStream fs = new FileStream(Application.StartupPath + "\\" + file, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            string txt = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            return txt;
        }
        public static string ReadTxtAllpath(string file)
        {
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            string txt = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            return txt;
        }
    }
}
