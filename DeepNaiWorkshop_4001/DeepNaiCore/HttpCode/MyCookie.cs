using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiCore.HttpCode
{
    class MyCookie
    {

        /// <summary>
        /// 获取Cookie的值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cc">Cookie集合对象</param>
        /// <returns>返回Cookie名称对应值</returns>
        public static string GetCookie(string cookieName, CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c1 in colCookies) lstCookies.Add(c1);
            }
            var model = lstCookies.Find(p => p.Name == cookieName);
            if (model != null)
            {
                return model.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookieHead">获取到的cookie字符串</param>
        /// <param name="defaultDomain">此域名下的cookie</param>
        /// <returns></returns>
        public static CookieContainer GetCookieCollectionByString(string cookieHead, string defaultDomain)
        {
            CookieContainer cookieContainer = new CookieContainer();
            CookieContainer result;
            if (cookieHead == null)
            {
                result = null;
            }
            else if (defaultDomain == null)
            {
                result = null;
            }
            else
            {
                string[] array = cookieHead.Split(new char[]
                    {
                        ';'
                    });
                for (int i = 0; i < array.Length; i++)
                {
                    Cookie cookieFromString = GetCookieFromString(array[i].Trim(), defaultDomain);
                    if (cookieFromString != null)
                    {
                        cookieContainer.Add(cookieFromString);
                    }
                }
                result = cookieContainer;
            }
            return result;

        }

        public static Cookie GetCookieFromString(string cookieString, string defaultDomain)
        {

            Cookie result;
            if (cookieString == null || defaultDomain == null)
            {
                result = null;
            }
            else
            {
                string[] array = cookieString.Split(new char[]
                    {
                        ','
                    });
                Hashtable hashtable = new Hashtable();
                for (int i = 0; i < array.Length; i++)
                {
                    string text = array[i].Trim();
                    int num = text.IndexOf("=", StringComparison.Ordinal);
                    if (num > 0)
                    {
                        hashtable.Add(text.Substring(0, num), text.Substring(num + 1));
                    }
                }
                Cookie cookie = new Cookie();
                foreach (object current in hashtable.Keys)
                {
                    if (current.ToString() == "path")
                    {
                        cookie.Path = hashtable[current].ToString();
                    }
                    else if (!(current.ToString() == "expires"))
                    {
                        if (current.ToString() == "domain")
                        {
                            cookie.Domain = hashtable[current].ToString();
                        }
                        else
                        {
                            cookie.Name = current.ToString();
                            cookie.Value = hashtable[current].ToString();
                        }
                    }
                }
                if (cookie.Name == "")
                {
                    result = null;
                }
                else
                {
                    if (cookie.Domain == "")
                    {
                        cookie.Domain = defaultDomain;
                    }
                    result = cookie;
                }
            }
            return result;
        }
    }
}
