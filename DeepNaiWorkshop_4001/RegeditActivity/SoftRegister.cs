using Microsoft.Win32;
using RegeditActivity.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace RegeditActivity
{
    public abstract class SoftRegister
    {
        
        // 取得设备硬盘的卷标号  
        public static string GetDiskVolumeSerialNumber()
        {
            //使用此类需要添加引用System.Management
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid='C:'");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        //获得CPU的序列号  
        public static string getCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        //生成机器码  
        public static string getMNum()
        {
            string strNum = getCpu() + GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号  
            string strMNum = strNum.Substring(0, 24);//从生成的字符串中取出前24个字符做为机器码  
            return strMNum;
        }
        public static int[] intCode = new int[127];//存储密钥  
        public static int[] intNumber = new int[25];//存机器码的Ascii值  
        public static char[] Charcode = new char[25];//存储机器码字  
        public static void setIntCode()//给数组赋值小于10的数  
        {
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i % 9;
            }
        }

        /*
         * 获取指定注册表基项值
         * 
         */ 
        public static RegistryKey getFatherKey()
        {
            RegistryKey lm = Registry.CurrentUser;
            RegistryKey fatherKey = lm.OpenSubKey("SOFTWARE", true);
            RegistryKey myProjectFatherKey = fatherKey.OpenSubKey(ActivityConst.REGISTRY_LOCATION,true);
            if (myProjectFatherKey == null)
            {
                myProjectFatherKey = fatherKey.CreateSubKey(ActivityConst.REGISTRY_LOCATION, true);

            }
            return myProjectFatherKey;
        }

        /*
         * 生成注册码
         * 
         */
        public abstract String generateRegistCode(String macNum);


        /*
         * 校验输入的机器码是否正确 
         * @param registCode 用户输入或者注册表中存储的注册码
         * @return RespMessage 响应消息
         */
        public abstract RespMessage checkReg(String registCode);


    }
}
