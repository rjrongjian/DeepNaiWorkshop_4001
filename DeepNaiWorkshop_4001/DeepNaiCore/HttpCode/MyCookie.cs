﻿using System;
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
    }
}