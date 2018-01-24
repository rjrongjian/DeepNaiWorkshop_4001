using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyTools
{
    public class ImageTool
    {
        public static Image getImageBy(String imgUrl)
        {
            System.Net.WebRequest webreq = System.Net.WebRequest.Create(imgUrl);
            System.Net.WebResponse webres = webreq.GetResponse();
            Stream stream = webres.GetResponseStream();
            Image image;
            image = Image.FromStream(stream);
            stream.Close();
            return image;
        }

        /// <summary>
        /// 读取本地图片
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static Image getLocalImageBy(String imgPath)
        {
            Image img = null;
            Stream s = File.Open(imgPath, FileMode.Open);
            img = Image.FromStream(s);
            s.Close();
            return img;
        }

        /// <summary>
        /// 判断当前图片路径是否有效，且是网络图片还是本地图片
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <returns>0 不合法 1 网络图片 2 本地图片</returns>
        public static int isLegalOfImgPath(string imgPath)
        {
            if (String.IsNullOrWhiteSpace(imgPath))
            {
                return 0;
            }
            //网络图片
            Regex reg = new Regex("[A-Za-z]:[\\s\\S]+[.jpg|.jpeg|.gif|.png|.bmp]");
            Match match = reg.Match(imgPath);

            if (match.Success)
            {
                return 2;
            }
            Regex reg2 = new Regex("[http://|https://][\\s\\S]+");
            Match match2 = reg.Match(imgPath);
            if (match2.Success)
            {
                return 1;
            }

            return 0;

        }
        /// <summary>
        /// 重新设置图片大小
        /// </summary>
        /// <param name="oriImg">原始图片</param>
        /// <param name="width">更改后的图宽</param>
        /// <param name="height">更改后的图高</param>
        /// <returns>返回一个调整大小后的克隆对象</returns>
        public static Image resetImgSize(Image oriImg,int width,int height)
        {
            Image img = (Image)oriImg.Clone();
            return new Bitmap(img, width, height);

        }

        
    }
}
