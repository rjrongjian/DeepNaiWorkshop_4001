using RegeditActivity;
using Microsoft.Win32;
using RegeditActivity.Bean;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyTools;
using DeepNaiWorkshop_4001;

namespace RegeditActivity
{
    public partial class ActivityForm : Form
    {
        private SoftRegister sr ;
        private MainForm mainForm;

        public ActivityForm()
        {
            InitializeComponent();
            sr = new DefaultSoftRegister();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Text = this.Text + "-版本" + ActivityConst.VERSION;
            this.textBox1.Text = SoftRegister.getMNum();
            RegistryKey fatherKey =  SoftRegister.getFatherKey();
            this.textBox2.Text = fatherKey.GetValue(ActivityConst.VALUE_NAME_FOR_VALIDATE_IN_REGISTRY, "").ToString();

            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width-this.Width)/2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
            //Console.WriteLine("获取的机器码：" + SoftRegister.getMNum());
            //Console.WriteLine("生成的注册码：" + sr.generateRegistCode("BFEBFBFF000906E93CFE0AB5"));
            //Console.WriteLine("生成的注册码2：" + sr.generateRegistCode(SoftRegister.getMNum()));
            //RespMessage respMessage = sr.checkReg(sr.generateRegistCode(SoftRegister.getMNum()));
            //Console.WriteLine("获取的注册信息："+respMessage.message);

            //测试发邮件
            //Console.WriteLine("发送前");
            //MailTool email = new MailTool("1633545776@qq.com", "测试邮件2", "测试内容");
            //email.Send();
            //Console.WriteLine("发送没");

            //测试正则
            //Console.WriteLine("发送前");
            //Regex reg = new Regex(".*(\\[.*\\]).*");
            //Match m = reg.Match("kfjs[fds]jgj");
            //Console.WriteLine(m.Groups[1].Value);

            //Console.WriteLine("发送后");
        }
        //复制机器码按钮
        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.textBox1.Text);
            this.label3.Text = "复制成功，操作时间："+DateTool.getCurrentTimeStr();
        }
        //激活软件按钮
        private void button1_Click(object sender, EventArgs e)
        {
            this.label3.Text = "正在校验注册码...";
            this.button1.Enabled = false;
            RespMessage respMessage = sr.checkReg(this.textBox2.Text);
            if (respMessage.code == 1)//激活成功，跳转到mainForm
            {
                //隐藏激活页面
                this.Hide();

                Point activityFormLocation = this.Location;
                //显示功能页面
                mainForm = new MainForm();
                //调整页面位置
                mainForm.Location = activityFormLocation;
                mainForm.Text = mainForm.Text + "-版本" + ActivityConst.VERSION;
                mainForm.Show();
                this.button1.Enabled = true;
            }
            else
            {
                this.label3.Text = "注册码错误！！！";
                this.button1.Enabled = true;
                MessageBox.Show(respMessage.message);
            }
        }

        private void ActivityForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
