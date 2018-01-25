using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepNaiCore
{
    /// <summary>
    /// 将J_AliLoginForm获取的数据传递到MainForm中
    /// </summary>
    public class DataPassWithinForm
    {
        public static IList<TbUnionAccountBean> InfoList = new List<TbUnionAccountBean>();
        public static ComboBox ComboBoxInMainForm;
    }
}
