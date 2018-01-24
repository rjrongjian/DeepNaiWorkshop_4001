using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegeditActivity.Bean
{
    class ActivityConst
    {
        public static String VERSION = "V1.1";//版本号
        public static String VALUE_NAME_FOR_VALIDATE_IN_REGISTRY = "VALIDATE";//注册表中使用的识别码
        public static String SOFT_FLAG_FOR_REGISTER = "TBKORDER";//软件本身的标记，防止开发的不同软件的注册码都能使用
        public static String PROJECT_ID = "DeepNai 4001";//项目识别标记
        //在HKEY_LOCAL_MACHINE/SOFTWARE下开始创建
        public static String REGISTRY_LOCATION = PROJECT_ID;//注册表基项位置
    }
}
