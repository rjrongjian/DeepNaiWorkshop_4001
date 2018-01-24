using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyTools
{
    public class WebTool
    {
        public static String getHtmlContent(String url)
        {

            WebClient MyWebClient = new WebClient();


            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

            Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据

            string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            

            //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句

            //Console.WriteLine(pageHtml);//在控制台输入获取的内容

            return pageHtml;
        }


    }
}
