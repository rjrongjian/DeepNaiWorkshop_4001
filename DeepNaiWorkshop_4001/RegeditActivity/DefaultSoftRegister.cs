
using MyTools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegeditActivity.Bean;

namespace RegeditActivity
{
    /*
     * 一次注册终身使用
     * 
     */
    public class DefaultSoftRegister : SoftRegister
    {
        public int[] intCode = new int[127];//存储密钥  
        public int[] intNumber = new int[25];//存机器码的Ascii值  
        public char[] Charcode = new char[25];//存储机器码字  
        public void setIntCode()//给数组赋值小于10的数  
        {
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i % 9;
            }
        }
        public override string generateRegistCode(string macNum)
        {
            setIntCode();//初始化127位数组  
            for (int i = 1; i < Charcode.Length; i++)//把机器码存入数组中  
            {
                Charcode[i] = Convert.ToChar(macNum.Substring(i - 1, 1));
            }
            for (int j = 1; j < intNumber.Length; j++)//把字符的ASCII值存入一个整数组中。  
            {
                intNumber[j] = intCode[Convert.ToInt32(Charcode[j])] + Convert.ToInt32(Charcode[j]);
            }
            string strAsciiName = "";//用于存储注册码  
            for (int j = 1; j < intNumber.Length; j++)
            {
                if (intNumber[j] >= 48 && intNumber[j] <= 57)//判断字符ASCII值是否0－9之间  
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 65 && intNumber[j] <= 90)//判断字符ASCII值是否A－Z之间  
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 97 && intNumber[j] <= 122)//判断字符ASCII值是否a－z之间  
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else//判断字符ASCII值不在以上范围内  
                {
                    if (intNumber[j] > 122)//判断字符ASCII值是否大于z  
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 10).ToString();
                    }
                    else
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 9).ToString();
                    }
                }
            }
            //获取的机器码，打上软件标识再进行一次MD5
            strAsciiName = MD5Tool.md5(strAsciiName + ActivityConst.SOFT_FLAG_FOR_REGISTER).ToUpper();
            return strAsciiName;
        }

        public override RespMessage checkReg(String registCode)
        {
            //相等且注册表没有存此值时，就存入
            RegistryKey fatherKey = SoftRegister.getFatherKey();
            String localRegistCode = this.generateRegistCode(SoftRegister.getMNum());
            string str = fatherKey.GetValue(ActivityConst.VALUE_NAME_FOR_VALIDATE_IN_REGISTRY, "").ToString();
            if (localRegistCode.Equals(registCode))
            {
                
                
                if ("".Equals(str))
                {
                    fatherKey.SetValue(ActivityConst.VALUE_NAME_FOR_VALIDATE_IN_REGISTRY, registCode);
                }
                return new RespMessage(1,"OK");
            }
            else
            {
                //当注册码不想等时，且当注册表此注册信息存在，也会被清除
                //string str = Registry.GetValue(Const.REGISTRY_LOCATION, Const.VALUE_NAME_FOR_VALIDATE_IN_REGISTRY, "").ToString();
                if (!"".Equals(str))
                {
                    //fatherKey.SetValue( Const.VALUE_NAME_FOR_VALIDATE_IN_REGISTRY, "");
                    fatherKey.DeleteValue(ActivityConst.VALUE_NAME_FOR_VALIDATE_IN_REGISTRY);
                    
                    return new RespMessage(2, "请重新输入注册码，以便继续使用应用");
                }
                return new RespMessage(3, "请输入正确的注册码！");

            }
            
        }
    }
}
