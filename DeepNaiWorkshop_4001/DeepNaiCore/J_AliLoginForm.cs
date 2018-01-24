
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HttpCodeLib;


namespace DeepNaiCore
{
    public class J_AliLoginForm : Form
    {
        public class Win32Mouse
        {
            public static readonly int MOUSEEVENTF_MOVE = 1;

            public static readonly int MOUSEEVENTF_ABSOLUTE = 32768;

            public static readonly int MOUSEEVENTF_LEFTDOWN = 2;

            public static readonly int MOUSEEVENTF_LEFTUP = 4;

            public static readonly int MOUSEEVENTF_RIGHTDOWN = 8;

            public static readonly int MOUSEEVENTF_RIGHTUP = 16;

            public static readonly int MOUSEEVENTF_MIDDLEDOWN = 32;

            public static readonly int MOUSEEVENTF_MIDDLEUP = 64;

            [DllImport("user32.dll")]
            public static extern bool GetCursorPos(out System.Drawing.Point pt);

            [DllImport("user32.dll")]
            public static extern void SetCursorPos(int x, int y);

            [DllImport("user32")]
            public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        }

        private DateTime starttime = DateTime.Now;

        private string starturl = "https://login.taobao.com/member/login.jhtml?style=mini&newMini2=true&css_style=alimama&from=alimama&redirectURL=http%3A%2F%2Fwww.alimama.com&full_redirect=true";

        private IContainer components = null;

        private WebBrowser webBrowser1;

        private Button button1;

        private CheckBox checkBox1;

        private Button button2;

        private Button button3;

        private Button button4;

        private System.Windows.Forms.Timer timer1;

        public J_AliLoginForm()
        {
            this.InitializeComponent();
          
            this.webBrowser1.Navigate(this.starturl);
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.starttime = DateTime.Now;
            this.timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Thread(delegate(object o)
            {
                Thread.Sleep(1000);
                IntPtr intPtr = this.webBrowser1.Handle;
                StringBuilder stringBuilder = new StringBuilder(100);
                while (stringBuilder.ToString() != "Internet Explorer_Server")
                {
                    intPtr = J_AliLoginForm.GetWindow(intPtr, 5);
                    J_AliLoginForm.GetClassName(intPtr, stringBuilder, stringBuilder.Capacity);
                }
                J_AliLoginForm.SendMessage(intPtr, 513, 1, this.MakeLParam(65, 135));
                J_AliLoginForm.SendMessage(intPtr, 512, 1, this.MakeLParam(65, 135));
                Thread.Sleep(10);
                J_AliLoginForm.SendMessage(intPtr, 512, 1, this.MakeLParam(285, 135));
                J_AliLoginForm.SendMessage(intPtr, 514, 1, this.MakeLParam(275, 135));
            })
            {
                IsBackground = true
            }.Start();
        }

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public int MakeLParam(int LoWord, int HiWord)
        {
            return HiWord << 16 | (LoWord & 65535);
        }

        private void AliLoginForm_Load(object sender, EventArgs e)
        {
            base.AutoScaleMode = AutoScaleMode.Font;
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }

        [DllImport("User32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, int wCmd);

        [DllImport("User32.Dll")]
        public static extern void GetClassName(IntPtr hwnd, StringBuilder s, int nMaxCount);
        /// <summary>
        /// 定时期执行逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.AddSeconds(-120.0) >= this.starttime)
            {
                this.timer1.Stop();
                //MainForm.AliIsLogin = false;
                base.Close();
            }
            else if (!this.webBrowser1.IsBusy)//浏览器是否加载新文档
            {
                if (!(this.webBrowser1.Document == null))//	获取一个 HtmlDocument，它表示当前显示在 WebBrowser 控件中的网页
                {
                    if (this.webBrowser1.Url != null && this.webBrowser1.Url.ToString().Contains("www.alimama.com/index.htm"))
                    {
                        this.timer1.Stop();
                        XJHTTP xJHTTP = new XJHTTP();
                        DT.Cookie = xJHTTP.GetCookieByWininet("http://pub.alimama.com/myunion.htm");
                        bool flag = DT.Cookie != null && GetToken(DT.Cookie).Count > 0;
                        if (flag)
                        {
                            DT.Token = this.GetToken(DT.Cookie)[0];
                            this.test_mm();
                            base.Close();
                        }
                    }
                    else if (this.webBrowser1.Document.GetElementById("ra-0") != null && this.webBrowser1.Document.GetElementById("ra-0").GetAttribute("value") != null && this.webBrowser1.Document.GetElementById("ra-0").GetAttribute("value").StartsWith("cntaobao") && DT.AliLoginType == "快捷")//&& MainForm.AliLoginType == 1
                    {
                        if (this.webBrowser1.Document.GetElementById("J_SubmitQuick") != null)
                        {
                            try
                            {
                                this.webBrowser1.Document.GetElementById("J_SubmitQuick").Focus();
                            }
                            catch
                            {
                            }
                            HtmlElement elementById = this.webBrowser1.Document.GetElementById("J_SubmitQuick");
                            System.Drawing.Point offset = this.GetOffset(elementById);
                            if (offset.X > 0 && offset.Y > 0)
                            {
                                IntPtr intPtr = this.webBrowser1.Handle;
                                StringBuilder stringBuilder = new StringBuilder(100);
                                while (stringBuilder.ToString() != "Internet Explorer_Server")
                                {
                                    intPtr = J_AliLoginForm.GetWindow(intPtr, 5);
                                    J_AliLoginForm.GetClassName(intPtr, stringBuilder, stringBuilder.Capacity);
                                }
                                int loWord = offset.X + new Random().Next(20, 100);
                                int hiWord = offset.Y + new Random().Next(10, 30);
                                J_AliLoginForm.SendMessage(intPtr, 513, 1, this.MakeLParam(loWord, hiWord));
                                J_AliLoginForm.SendMessage(intPtr, 514, 1, this.MakeLParam(loWord, hiWord));
                                //	ShowLog.Show("检测到已登录千牛[" + this.webBrowser1.Document.GetElementById("ra-0").GetAttribute("value").Replace("cntaobao", string.Empty) + "]优先选择该帐号登录!", System.Drawing.Color.Green, string.Empty, string.Empty);
                            }
                        }
                    }
                    else
                    {
                        if (this.webBrowser1.Document.GetElementById("J_LoginBox") != null && this.webBrowser1.Document.GetElementById("J_LoginBox").GetAttribute("classname") != null && this.webBrowser1.Document.GetElementById("J_LoginBox").GetAttribute("classname").ToString().Contains("module-quick"))
                        {
                            if (this.webBrowser1.Document.GetElementById("J_Quick2Static") != null)
                            {
                                this.webBrowser1.Document.GetElementById("J_Quick2Static").InvokeMember("click");
                                //ShowLog.Show("切换到帐号输入状态...", System.Drawing.Color.Green, string.Empty, string.Empty);
                                return;
                            }
                        }
                        if ((this.webBrowser1.Document.GetElementById("nc_1_clickCaptcha") != null && this.webBrowser1.Document.GetElementById("nc_1_clickCaptcha").Style != null && !this.webBrowser1.Document.GetElementById("nc_1_clickCaptcha").Style.Contains("-")) || (this.webBrowser1.Document.GetElementById("nc_1_imgCaptcha") != null && this.webBrowser1.Document.GetElementById("nc_1_imgCaptcha").Style != null && !this.webBrowser1.Document.GetElementById("nc_1_imgCaptcha").Style.Contains("-")) || (this.webBrowser1.Document.GetElementById("nc_1__voicebtn") != null && this.webBrowser1.Document.GetElementById("nc_1__voicebtn").Style != null && !this.webBrowser1.Document.GetElementById("nc_1__voicebtn").Style.ToLower().Contains("display: none")))
                        {
                            string style = this.webBrowser1.Document.GetElementById("nc_1_clickCaptcha").Style;
                            string style2 = this.webBrowser1.Document.GetElementById("nc_1_imgCaptcha").Style;
                            string style3 = this.webBrowser1.Document.GetElementById("nc_1__voicebtn").Style;
                            this.Text = "请手动验证并点击登录!";
                        }
                        else
                        {
                            if (this.webBrowser1.Document.GetElementById("nocaptcha") != null && this.webBrowser1.Document.GetElementById("nocaptcha").Style != null)
                            {
                                if (!(this.webBrowser1.Document.GetElementById("nc_1_n1z") != null) || !(this.webBrowser1.Document.GetElementById("nc_1_n1z").GetAttribute("classname") == "nc_iconfont btn_ok"))
                                {
                                    if (this.webBrowser1.Document.GetElementById("nc_1__scale_text") != null && this.webBrowser1.Document.GetElementById("nc_1__scale_text").OuterText == "请按住滑块，拖动到最右边")
                                    {
                                        HtmlElement elementById = this.webBrowser1.Document.GetElementById("nocaptcha");
                                        System.Drawing.Point offset = this.GetOffset(elementById);
                                        IntPtr intPtr = this.webBrowser1.Handle;
                                        StringBuilder stringBuilder = new StringBuilder(100);
                                        while (stringBuilder.ToString() != "Internet Explorer_Server")
                                        {
                                            intPtr = J_AliLoginForm.GetWindow(intPtr, 5);
                                            J_AliLoginForm.GetClassName(intPtr, stringBuilder, stringBuilder.Capacity);
                                        }
                                        int num = 65;
                                        int num2 = 285;
                                        int num3 = 20;
                                        J_AliLoginForm.SendMessage(intPtr, 513, 1, this.MakeLParam(num, offset.Y + 20));
                                        int num4 = num + (num2 - num) / num3;
                                        for (int i = 0; i < num3; i++)
                                        {
                                            J_AliLoginForm.SendMessage(intPtr, 512, 1, this.MakeLParam(num + i * ((num2 - num) / num3), offset.Y + 20));
                                            Thread.Sleep(20);
                                        }
                                        J_AliLoginForm.SendMessage(intPtr, 514, 1, this.MakeLParam(275, offset.Y + 20));
                                    }
                                    return;
                                }
                            }
                            if (this.webBrowser1.Document.GetElementById("TPL_username_1") != null)
                            {
                            //    this.webBrowser1.Document.GetElementById("TPL_username_1").SetAttribute("value", DT.TBName);
                            }
                            if (this.webBrowser1.Document.GetElementById("TPL_password_1") != null)
                            {
                             //   this.webBrowser1.Document.GetElementById("TPL_password_1").SetAttribute("value", DT.TBPsw);
                            }
                            if (this.webBrowser1.Document.GetElementById("J_SubmitStatic") != null)
                            {
                                try
                                {
                                    this.webBrowser1.Document.GetElementById("J_SubmitStatic").Focus();
                                }
                                catch
                                {
                                }
                                HtmlElement elementById = this.webBrowser1.Document.GetElementById("J_SubmitStatic");
                                System.Drawing.Point offset = this.GetOffset(elementById);
                                if (offset.X > 0 && offset.Y > 0)
                                {
                                    IntPtr intPtr = this.webBrowser1.Handle;
                                    StringBuilder stringBuilder = new StringBuilder(100);
                                    while (stringBuilder.ToString() != "Internet Explorer_Server")
                                    {
                                        intPtr = J_AliLoginForm.GetWindow(intPtr, 5);
                                        J_AliLoginForm.GetClassName(intPtr, stringBuilder, stringBuilder.Capacity);
                                    }
                                    int loWord = offset.X + new Random().Next(20, 100);
                                    int hiWord = offset.Y + new Random().Next(10, 30);
                                    J_AliLoginForm.SendMessage(intPtr, 513, 1, this.MakeLParam(loWord, hiWord));
                                    J_AliLoginForm.SendMessage(intPtr, 514, 1, this.MakeLParam(loWord, hiWord));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!(this.webBrowser1.Document == null))
            {
                if (this.webBrowser1.Document.GetElementById("TPL_username_1") != null)
                {
                  //  this.webBrowser1.Document.GetElementById("TPL_username_1").SetAttribute("value", DT.TBName);
                }
                if (this.webBrowser1.Document.GetElementById("TPL_password_1") != null)
                {
                 //   this.webBrowser1.Document.GetElementById("TPL_password_1").SetAttribute("value", DT.TBPsw);
                }
            }
        }

        public System.Drawing.Point GetOffset(HtmlElement el)
        {
            System.Drawing.Point result = new System.Drawing.Point(el.OffsetRectangle.Left, el.OffsetRectangle.Top);
            HtmlElement offsetParent = el.OffsetParent;
            while (offsetParent != null)
            {
                result.X += offsetParent.OffsetRectangle.Left;
                result.Y += offsetParent.OffsetRectangle.Top;
                offsetParent = offsetParent.OffsetParent;
            }
            return result;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.timer1.Enabled = !this.checkBox1.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(this.starturl);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            XJHTTP xJHTTP = new XJHTTP();
            DT.Cookie = xJHTTP.GetCookieByWininet("http://pub.alimama.com/myunion.htm");
            bool flag = DT.Cookie != null && GetToken(DT.Cookie).Count > 0;
            if (flag)
            {
                DT.Token = this.GetToken(DT.Cookie)[0];
                this.test_mm();
                base.Close();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.webBrowser1.Refresh();
        }

        private void AliLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Stop();
            IniHelper.FilePath = DT.ConfigPath;
            IniHelper.WriteIniKey("MinOrMax", "way", "normal");
        }
        private void test_mm()
        {
            IniHelper.FilePath = DT.ConfigPath;
            string cookie = DT.Cookie;
       
            HttpHelpers httpHelpers = new HttpHelpers();
            HttpResults httpResults = new HttpResults();
            string reString = string.Empty;
            string uRL = "http://pub.alimama.com/common/getUnionPubContextInfo.json";
            httpResults = httpHelpers.GetHtml(new HttpItems
            {
                URL = uRL,
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0",
                Cookie = cookie,
                Allowautoredirect = true,
                ContentType = "application/x-www-form-urlencoded"
            });
            reString = httpResults.Html;
            try
            {
               
            }
            catch (Exception)
            {
                // throw;
                //  MessageBox.Show("系统繁忙，请稍后再试");
                base.Close();
            }
        }
        private Regex reg;
        private List<string> GetToken(string reString)
        {
            List<string> list = new List<string>();
            string pattern = "_tb_token_=(.*?);";
            this.reg = new Regex(pattern);
            MatchCollection matchCollection = this.reg.Matches(reString);
            for (int i = 0; i < matchCollection.Count; i++)
            {
                GroupCollection groups = matchCollection[i].Groups;
                for (int j = 1; j < groups.Count; j++)
                {
                    string value = groups[j].Value;
                    bool flag = !string.IsNullOrEmpty(value);
                    if (flag)
                    {
                        list.Add(value);
                    }
                }
            }
            return list;
        }

        private List<string> GetIp(string reString)
        {
            List<string> list = new List<string>();
            string pattern = "\"ip\":\"(\\d+.\\d+.\\d+.\\d+)\"";
            this.reg = new Regex(pattern);
            MatchCollection matchCollection = this.reg.Matches(reString);
            for (int i = 0; i < matchCollection.Count; i++)
            {
                GroupCollection groups = matchCollection[i].Groups;
                for (int j = 1; j < groups.Count; j++)
                {
                    string value = groups[j].Value;
                    bool flag = !string.IsNullOrEmpty(value);
                    if (flag)
                    {
                        list.Add(value);
                    }
                }
            }
            return list;
        }

        private List<string> GetTaoName(string reString)
        {
            List<string> list = new List<string>();
            string pattern = "\"mmNick\":\"(.*?)\"";
            this.reg = new Regex(pattern);
            MatchCollection matchCollection = this.reg.Matches(reString);
            for (int i = 0; i < matchCollection.Count; i++)
            {
                GroupCollection groups = matchCollection[i].Groups;
                for (int j = 1; j < groups.Count; j++)
                {
                    string value = groups[j].Value;
                    bool flag = !string.IsNullOrEmpty(value);
                    if (flag)
                    {
                        list.Add(value);
                    }
                }
            }
            return list;
        }

        private List<string> GetMemberid(string reString)
        {
            List<string> list = new List<string>();
            string pattern = "\"memberid\":(\\d+),";
            this.reg = new Regex(pattern);
            MatchCollection matchCollection = this.reg.Matches(reString);
            for (int i = 0; i < matchCollection.Count; i++)
            {
                GroupCollection groups = matchCollection[i].Groups;
                for (int j = 1; j < groups.Count; j++)
                {
                    string value = groups[j].Value;
                    bool flag = !string.IsNullOrEmpty(value);
                    if (flag)
                    {
                        list.Add(value);
                    }
                }
            }
            return list;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(J_AliLoginForm));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(374, 424);
            this.webBrowser1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(275, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(0, 399);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 16);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "本次暂停登录";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(96, 430);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "填写帐号";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(163, 430);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(85, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "重新开始登录";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(248, 430);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(105, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "已经登录了";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // J_AliLoginForm
            // 
            this.ClientSize = new System.Drawing.Size(374, 456);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.webBrowser1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "J_AliLoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "阿里妈妈登录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AliLoginForm_FormClosing);
            this.Load += new System.EventHandler(this.AliLoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
