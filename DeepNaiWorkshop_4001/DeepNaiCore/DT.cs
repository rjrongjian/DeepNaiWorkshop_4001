using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.Windows.Forms;

namespace DeepNaiCore
{
  public   class DT
  {
      public static string ConfigPath = Application.StartupPath + @"\Config.ini";
      public static string Cookie { get; set; }
      public static string AliLoginType = "快捷";
      public static string Token { get; set; }
  }
}
