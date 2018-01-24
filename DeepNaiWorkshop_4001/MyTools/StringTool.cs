using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools
{
    public class StringTool
    {
        public static String replaceStartWith(String str,String startWith,String replaceWithStr)
        {
            if (str.StartsWith(startWith))
            {
                return replaceWithStr + str.Substring(str.IndexOf(startWith) + startWith.Length);
            }
            else
            {
                return str;
            }
        }
        /*
        /// <summary>
        /// 绘制文本自动换行（超出截断），并用画笔画出
        /// </summary>
        /// <param name="graphic">绘图图面</param>
        /// <param name="font">字体</param>
        /// <param name="text">文本</param>
        /// <param name="recangle">绘制范围</param>
        public static void DrawStringWrap(Graphics graphic, Font font, string text, Rectangle recangle)
        {
            List<string> textRows = GetStringRows(graphic, font, text, recangle.Width);
            int rowHeight = (int)(Math.Ceiling(graphic.MeasureString(text, font).Height));


            int maxRowCount = recangle.Height / rowHeight;
            int drawRowCount = (maxRowCount < textRows.Count) ? maxRowCount : textRows.Count;

            int top = (recangle.Height - rowHeight * drawRowCount) / 2;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            for (int i = 0; i < drawRowCount; i++)
            {
                Rectangle fontRectanle = new Rectangle(recangle.Left, top + rowHeight * i, recangle.Width, rowHeight);
                graphic.DrawString(textRows, font, new SolidBrush(Color.Black), fontRectanle, sf);
            }

        }

        /// <summary>
        /// 将文本分行，并用画笔画出
        /// </summary>
        /// <param name=\"graphic\">绘图图面</param>
        /// <param name=\"font\">字体</param>
        /// <param name=\"text\">文本</param>
        /// <param name=\"width\">行宽</param>
        /// <returns></returns>
        private static List<string> GetStringRows(Graphics graphic, Font font, string text, int width)
        {
            int RowBeginIndex = 0;
            int rowEndIndex = 0;
            int textLength = text.Length;
            List<string> textRows = new List<string>();

            for (int index = 0; index < textLength; index++)
            {
                rowEndIndex = index;

                if (index == textLength - 1)
                {
                    textRows.Add(text.Substring(RowBeginIndex));
                }
                else if (rowEndIndex + 1 < text.Length && text.Substring(rowEndIndex, 2) == "\\r\\n")
                {
                    textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                    rowEndIndex = index += 2;
                    RowBeginIndex = rowEndIndex;
                }
                else if (graphic.MeasureString(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex + 1), font).Width > width)
                {
                    textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                    RowBeginIndex = rowEndIndex;
                }
            }

            return textRows;
        }
        */
    }
}
