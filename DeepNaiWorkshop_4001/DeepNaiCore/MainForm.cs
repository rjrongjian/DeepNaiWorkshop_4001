using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HttpCodeLib;
using System.Runtime.InteropServices;
using DeepNaiCore.Util;
using MySql.Data.MySqlClient;
using DeepNaiCore.HttpCode;
using MyTools;

namespace DeepNaiCore
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new J_AliLoginForm().ShowDialog();
            if (!string.IsNullOrEmpty(DT.Token))
            {
                label2.Text = "已登录";
                label2.ForeColor = Color.Green;
            }
        }



        private bool start = false;
        private XJHTTP xj = new XJHTTP();
        private string dbTable = null;
        private void Tongbu(int day1)
        {
            dbTable = txtTable.Text.Trim();
            string filepath = Application.StartupPath + "\\Orders";
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            filepath = filepath + "\\" + xj.EncryptMD5String(DateTime.Now.ToString()) + ".xls";
            start = true;
            string starttime = DateTime.Now.AddDays(-day1).ToShortDateString().Replace("/", "-");
            if (donging == false)
            {
                Addlogs("停止");
                return;
            }
            Addlogs("正在获取从 " + starttime + "至今 已同步数据库订单");



            DataTable data = DbHelperMySQL.Query(
                 "SELECT  `orderId`,`goodId`,`orderStatus`,`goodCount`,`goodUnitPrice`,`incomeRatio`,`dividedIntoRatio`,`effect`  FROM `" + txtDbName.Text.Trim() + "`.`" + dbTable + "`WHERE `createTime`>'" + starttime + " 00:00:00'").Tables[0];
            StringBuilder sb = new StringBuilder(1000);

            if (data.Rows.Count > 0)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    sb.Append(data.Rows[i]["orderId"] +"---"+ data.Rows[i]["goodId"] + "---" + data.Rows[i]["goodCount"] + "---" + data.Rows[i]["goodUnitPrice"] + "---" + data.Rows[i]["incomeRatio"] + "---" + data.Rows[i]["dividedIntoRatio"] + "---" + data.Rows[i]["effect"] + "---" + data.Rows[i]["orderStatus"]);
                }
            }
            if (donging == false)
            {
                Addlogs("停止");
                return;
            }
            Addlogs("从tbk_tb_order获取到" + data.Rows.Count + "条已同步订单");

            data = DbHelperMySQL.Query(
                "SELECT  `orderId`,`goodId`,`orderStatus`,`goodCount`,`goodUnitPrice`,`incomeRatio`,`dividedIntoRatio`,`effect`  FROM `" + txtDbName.Text.Trim() + "`.`" + dbTable + "`WHERE `createTime`>'" + starttime + " 00:00:00'").Tables[0];
            if (data.Rows.Count > 0)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    sb.Append(data.Rows[i]["orderId"] + "---" + data.Rows[i]["goodId"] + "---" + data.Rows[i]["goodCount"] + "---" + data.Rows[i]["goodUnitPrice"] + "---" + data.Rows[i]["incomeRatio"] + "---" + data.Rows[i]["dividedIntoRatio"] + "---" + data.Rows[i]["effect"] + "---" + data.Rows[i]["orderStatus"]);
                }
            }
            if (donging == false)
            {
                return;
            }
            if (donging == false)
            {
                Addlogs("停止");
                return;
            }
            //Addlogs("从" + dbTable + "获取到" + data.Rows.Count + "条已同步订单");
            string endtime = DateTime.Now.ToShortDateString().Replace("/", "-");
            string url = "http://pub.alimama.com/report/getTbkPaymentDetails.json?spm=a219t.7664554.1998457203.57.493a27d77xCKBm&queryType=1&payStatus=&DownloadID=DOWNLOAD_REPORT_INCOME_NEW&startTime=" +
                starttime + "&endTime=" + endtime;
            string Ua = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.108 Safari/537.36 2345Explorer/8.6.1.15524";
            CookieContainer cookieCollectionByString = GetCookieCollectionByString(DT.Cookie, "pub.alimama.com");
            if (donging == false)
            {
                Addlogs("停止");
                return;
            }




            Addlogs("正在下载 从" + starttime + "至今 淘宝客订单");
            DataTable dt3 = null;
            for (int i = 0; i < 10; i++)
            {
                string result = this.Downer(url, cookieCollectionByString, Ua, filepath);
                //  DataTable dt3 = ExcelHelper.InputFromExcel(filepath, "Page1");
                dt3 = ExcelHelper.InputFromExcel(filepath, "Page1");
                if (dt3 != null && dt3.Rows.Count > 0)
                {
                    break;
                }
                Addlogs("下载失败，正在重新下载...");
                Thread.Sleep(1000);
                if (i >= 5)
                {
                    loginnum = 30;
                    Thread.Sleep(100000);
                }
            }
            if (dt3 == null)
            {
                Addlogs("淘宝登陆状态失效或没有登录");
                start = false;
                return;
            }

            List<string> sqList = new List<string>();

            //string path = Application.StartupPath + @"\temp\orders.txt";
            //if (!Directory.Exists(Application.StartupPath + @"\temp"))
            //{
            //    Directory.CreateDirectory(Application.StartupPath + @"\temp");
            //}
            if (donging == false)
            {
                Addlogs("停止");
                return;
            }
            Addlogs("获取到" + dt3.Rows.Count + "条 淘宝客订单");
            int needUpdateOrderCount = 0;//需要更新的订单数量
            int needInsertOrderCount = 0;//需要插入的订单数量, dbTable
            String dbNameTemp = this.txtDbName.Text;
            string dbTableTemp = this.txtTable.Text;
            StringBuilder finalBatchInsertSql = new StringBuilder("INSERT INTO `"+ dbNameTemp + "`.`"+ dbTableTemp + "`(`createTime`,`clickTime`,`goodInfo`,`goodId`,`wangWangName`,`shopName`,`goodCount`,`goodUnitPrice`,`orderStatus`,`orderType`,`incomeRatio`,`dividedIntoRatio`,`paymentAmount`,`effect`,`settlementAmount`,`estimatedIncome`,`settlingTime`,`commissionRate`,`commissionAmount`,`subsidyRatio`,`subsidies`,`subsidiesType`,`transactionPlatform`,`thirdPartyServiceSource`,`orderId`,`catName`,`mediaSourceId`,`mediaSourceName`,`advertisingId`,`advertisingName`,`tbUnionAccountId`,`pid`) VALUES");
            //StringBuilder finalBatchUpdateSql = new StringBuilder("REPLACE INTO `" + dbNameTemp + "`.`" + dbTableTemp + "`(`createTime`,`clickTime`,`goodInfo`,`goodId`,`wangWangName`,`shopName`,`goodCount`,`goodUnitPrice`,`orderStatus`,`orderType`,`incomeRatio`,`dividedIntoRatio`,`paymentAmount`,`effect`,`settlementAmount`,`estimatedIncome`,`settlingTime`,`commissionRate`,`commissionAmount`,`subsidyRatio`,`subsidies`,`subsidiesType`,`transactionPlatform`,`thirdPartyServiceSource`,`orderId`,`catName`,`mediaSourceId`,`mediaSourceName`,`advertisingId`,`advertisingName`,`tbUnionAccountId`,`pid`) VALUES");
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                DataRow row = dt3.Rows[i];
                string create_time = row[0].ToString().Trim();//创建时间
                string click_time = row[1].ToString().Trim();//点击时间
                string goods_name = row[2].ToString().Replace("'", "").Trim();//商品名称
                string goods_id = row[3].ToString().Trim();//商品ID
                string wangwang = row[4].ToString().Replace("'", "").Trim();//旺旺
                string shop = row[5].ToString().Replace("'", "").Trim();//所属店铺
                string goods_number = row[6].ToString().Trim();//商品数量
                string goods_price = row[7].ToString().Trim();//商品价格
                string order_status = row[8].ToString().Trim();//订单状态
                string order_type = row[9].ToString().Trim();//淘宝/天猫


                string income_ratio = row[10].ToString().Replace("%","").Trim(); //收入比率
                string divided_ratio = row[11].ToString().Replace("%", "").Trim();//分成比率
                string order_amount = row[12].ToString().Trim();//付款金额
                string effect_prediction = row[13].ToString().Trim();//效果预估
                string balance_amount = row[14].ToString().Trim();//结算金额
                string estimated_revenue = row[15].ToString().Trim();//预估收入
                string balance_time = row[16].ToString().Trim();//结算时间
                string commission_ratio = row[17].ToString().Replace("%", "").Trim();//佣金比率
                string commission_amount = row[18].ToString().Trim();//佣金金额
                string subsidy_ratio = row[19].ToString().Replace("%", "").Trim();//补贴比率
                string subsidy_amount = row[20].ToString().Trim();//补贴金额
                string subsidy_type = row[21].ToString().Trim();//补贴类型
                string order_platform = row[22].ToString().Trim();//成交平台
                string serve_from = row[23].ToString().Trim();//第三方服务来源
                string order_sn = row[24].ToString().Trim();//订单编号
                string category = row[25].ToString().Trim();//类目名称
                string media_id = row[26].ToString().Trim();//来源媒体
                string media_name = row[27].ToString().Trim();//来源媒体名称
                string adv_id = row[28].ToString().Trim();//广告位ID
                string adv_name = row[29].ToString().Trim();//广告位名称
                string tbUnionAccountId = (String)this.tbUnionAccountCombox.SelectedValue;
                string pid = "mm_"+ tbUnionAccountId+"_"+ media_id+"_"+ adv_id;


                string table = string.Empty;
                if (textBox1.Text.Contains(media_id + "_" + adv_id))
                {
                    table = "" + dbTable + "";
                }
                else
                {
                    table = "tbk_tb_order";
                }

                if (order_status == "订单完成")
                {
                    order_status = "订单结算";
                    balance_time = DateTime.Now.ToShortDateString().Replace("/", "-") + " " + DateTime.Now.ToShortTimeString();
                   
                }
                if (string.IsNullOrEmpty(balance_time))
                {
                    balance_time = "0001-01-01";
                }
                /**
                string UpdataSql = string.Format(@"UPDATE `{33}`.`{32}`
	SET `createTime` = '{0}'
		,`clickTime` ='{1}'
		,`goodInfo` = '{2}'
		,`goodId` ='{3}'
		,`wangWangName` = '{4}'
		,`shopName` = '{5}'
		,`goodCount` ={6}
		,`goodUnitPrice` ={7}
		,`orderStatus` ='{8}'
		,`orderType` ='{9}'
		,`incomeRatio` = {10}
		,`dividedIntoRatio` ={11}
		,`paymentAmount` ={12}
		,`effect` = {13}
		,`settlementAmount` = {14}
		,`estimatedIncome` ={15}
		,`settlingTime` = '{16}'
		,`commissionRate` = {17}
		,`commissionAmount` ={18}
		,`subsidyRatio` = {19}
		,`subsidies` = {20}
		,`subsidiesType` = '{21}'
		,`transactionPlatform` ='{22}'
		,`thirdPartyServiceSource` ='{23}'
		,`id` = '{24}'
		,`catName` = '{25}'
		,`mediaSourceId` = '{26}'
		,`mediaSourceName` ='{27}'
		,`advertisingId` = '{28}'
		,`advertisingName` ='{29}'
        ,`tbUnionAccountId` ='{34}'
        ,`pid` ='{35}'
	WHERE createTime= '{30}' and id='{31}';", create_time, click_time, goods_name, goods_id, wangwang, shop, goods_number, goods_price, order_status, order_type, income_ratio, divided_ratio, order_amount, effect_prediction, balance_amount, estimated_revenue, balance_time, commission_ratio, commission_amount, subsidy_ratio, subsidy_amount, subsidy_type, order_platform, serve_from, order_sn, category, media_id, media_name, adv_id, adv_name, create_time, order_sn, table, dbTable, tbUnionAccountId,pid);

                string InserSql = string.Format(@"INSERT INTO `{31}`.`{30}`
		(`createTime`
		,`clickTime`
		,`goodInfo`
		,`goodId`
		,`wangWangName`
		,`shopName`
		,`goodCount`
		,`goodUnitPrice`
		,`orderStatus`
		,`orderType`
		,`incomeRatio`
		,`dividedIntoRatio`
		,`paymentAmount`
		,`effect`
		,`settlementAmount`
		,`estimatedIncome`
		,`settlingTime`
		,`commissionRate`
		,`commissionAmount`
		,`subsidyRatio`
		,`subsidies`
		,`subsidiesType`
		,`transactionPlatform`
		,`thirdPartyServiceSource`
		,`id`
		,`catName`
		,`mediaSourceId`
		,`mediaSourceName`
		,`advertisingId`
		,`advertisingName`
        ,`tbUnionAccountId`
        ,`pid`)
	VALUES
		('{0}'
		,'{1}'
		,'{2}'
		,'{3}'
		,'{4}'
		,'{5}'
		,{6}
		,{7}
		,'{8}'
		,'{9}'
		,{10}
		,{11}
		,{12}
		,{13}
		,{14}
		,{15}
		,'{16}'
		,{17}
		,{18}
		,{19}
		,{20}
		,'{21}'
		,'{22}'
		,'{23}'
		,'{24}'
		,'{25}'
		,'{26}'
		,'{27}'
		,'{28}'
		,'{29}'
        ,'{32}'
        ,'{33}');", create_time, click_time, goods_name, goods_id, wangwang, shop, goods_number, goods_price, order_status, order_type, income_ratio, divided_ratio, order_amount, effect_prediction, balance_amount, estimated_revenue, balance_time, commission_ratio, commission_amount, subsidy_ratio, subsidy_amount, subsidy_type, order_platform, serve_from, order_sn, category, media_id, media_name, adv_id, adv_name, table, txtDbName.Text.Trim(), tbUnionAccountId, pid);
        */
                //解决商品名称偶尔乱码
                //string UpdataSql = string.Format(@"UPDATE `{33}`.`{32}` SET `createTime` = '{0}',`clickTime` ='{1}',`goodInfo` = '"+goods_name+ "',`goodId` ='{3}',`wangWangName` = '{4}',`shopName` = '{5}',`goodCount` ={6},`goodUnitPrice` ={7},`orderStatus` ='{8}',`orderType` ='{9}',`incomeRatio` = {10},`dividedIntoRatio` ={11},`paymentAmount` ={12},`effect` = {13},`settlementAmount` = {14},`estimatedIncome` ={15},`settlingTime` = '{16}',`commissionRate` = {17},`commissionAmount` ={18},`subsidyRatio` = {19},`subsidies` = {20},`subsidiesType` = '{21}',`transactionPlatform` ='{22}',`thirdPartyServiceSource` ='{23}',`id` = '{24}',`catName` = '{25}',`mediaSourceId` = '{26}',`mediaSourceName` ='{27}',`advertisingId` = '{28}',`advertisingName` ='{29}',`tbUnionAccountId` ='{34}',`pid` ='{35}'  WHERE createTime= '{30}' and id='{31}';", create_time, click_time, goods_name, goods_id, wangwang, shop, goods_number, goods_price, order_status, order_type, income_ratio, divided_ratio, order_amount, effect_prediction, balance_amount, estimated_revenue, balance_time, commission_ratio, commission_amount, subsidy_ratio, subsidy_amount, subsidy_type, order_platform, serve_from, order_sn, category, media_id, media_name, adv_id, adv_name, create_time, order_sn, table, dbTable, tbUnionAccountId, pid);

                //string InserSql = string.Format(@"INSERT INTO `{31}`.`{30}`(`createTime`,`clickTime`,`goodInfo`,`goodId`,`wangWangName`,`shopName`,`goodCount`,`goodUnitPrice`,`orderStatus`,`orderType`,`incomeRatio`,`dividedIntoRatio`,`paymentAmount`,`effect`,`settlementAmount`,`estimatedIncome`,`settlingTime`,`commissionRate`,`commissionAmount`,`subsidyRatio`,`subsidies`,`subsidiesType`,`transactionPlatform`,`thirdPartyServiceSource`,`id`,`catName`,`mediaSourceId`,`mediaSourceName`,`advertisingId`,`advertisingName`,`tbUnionAccountId`,`pid`) VALUES('{0}','{1}','"+goods_name+"','{3}','{4}','{5}',{6},{7},'{8}','{9}',{10},{11},{12},{13},{14},{15},'{16}',{17},{18},{19},{20},'{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{32}' ,'{33}');", create_time, click_time, goods_name, goods_id, wangwang, shop, goods_number, goods_price, order_status, order_type, income_ratio, divided_ratio, order_amount, effect_prediction, balance_amount, estimated_revenue, balance_time, commission_ratio, commission_amount, subsidy_ratio, subsidy_amount, subsidy_type, order_platform, serve_from, order_sn, category, media_id, media_name, adv_id, adv_name, table, txtDbName.Text.Trim(), tbUnionAccountId, pid);
                string tempstr = order_sn + "---"+goods_id+"---" + DataUtil.DeleteFloatZero(goods_number) + "---" + DataUtil.DeleteFloatZero(goods_price) + "---" + DataUtil.DeleteFloatZero(income_ratio) + "---" + DataUtil.DeleteFloatZero(divided_ratio) + "---" + DataUtil.DeleteFloatZero(effect_prediction) + "---" + order_status;
                //string txtOrders = LogHelper.ReadTxtAllpath(path);
                string orderstr = sb.ToString();
                Console.WriteLine("总串："+ orderstr);
                
                if (orderstr.Contains(tempstr) == false && orderstr.Contains(order_sn + "---" + goods_id + "---" + goods_number + "---" + goods_price + "---" + income_ratio + "---" + divided_ratio + "---" + effect_prediction + "---"))//已经同步过  但是订单状态发生改变  需要更新
                {
                    string sql = "UPDATE `" + dbNameTemp + "`.`" + dbTableTemp + "SET paymentAmount="+ order_amount + ",effect="+ effect_prediction + ",settlementAmount="+ balance_amount + ",estimatedIncome="+ estimated_revenue + ",settlingTime='"+ balance_time + "',commissionAmount="+ commission_amount + ",orderStatus='"+order_status+ "' where orderId="+ order_sn + " and goodId="+ goods_id + " and goodCount="+ goods_number + " and goodUnitPrice="+ goods_price + " and incomeRatio="+ income_ratio + " and dividedIntoRatio="+ divided_ratio + " and effect="+ effect_prediction + ";";
                    sqList.Add(sql.Replace("%", ""));
                    //LogHelper.AppendTxtAllpath(tempstr + "\r\n", path);
                    needUpdateOrderCount++;

                    //finalBatchUpdateSql.Append(string.Format(@"('{0}', '{1}', '" + goods_name + "', '{3}', '{4}', '{5}',{ 6},{ 7},'{8}','{9}',{ 10},{ 11},{ 12},{ 13},{ 14},{ 15},'{16}',{ 17},{ 18},{ 19},{ 20},'{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}' ,'{31}'),", create_time, click_time, goods_name, goods_id, wangwang, shop, goods_number, goods_price, order_status, order_type, income_ratio, divided_ratio, order_amount, effect_prediction, balance_amount, estimated_revenue, balance_time, commission_ratio, commission_amount, subsidy_ratio, subsidy_amount, subsidy_type, order_platform, serve_from, order_sn, category, media_id, media_name, adv_id, adv_name, tbUnionAccountId, pid));
                    //finalBatchUpdateSql.Append(@"('"+create_time+"','"+click_time+"','" + goods_name + "','"+goods_id+"','"+wangwang+"','"+shop+"',"+goods_number+","+goods_price+",'"+order_status+"','"+order_type+"',"+income_ratio+","+divided_ratio+","+order_amount+","+effect_prediction+","+balance_amount+","+estimated_revenue+",'"+balance_time+"',"+commission_ratio+","+commission_amount+","+subsidy_ratio+","+subsidy_amount+",'"+subsidy_type+"','"+order_platform+"','"+serve_from+"','"+order_sn+"','"+category+"','"+media_id+"','"+media_name+"','"+adv_id+"','"+adv_name+"','"+tbUnionAccountId+"' ,'"+pid+"'),");
                }
                else if (orderstr.Contains(tempstr) == false)//没有同步过    需要添加
                {
                    //需要先获取数据库信息做一下判断
                    //bool exist = DbHelperMySQL.Exists("SELECT `id`FROM `taoke`.`ftxia_taoke_detail` WHERE order_sn='" + order_sn + "'");
                    //string sql = InserSql;
                    //if (exist)
                    //{
                    //    sql = UpdataSql;
                    //}

                    // sqList.Add(sql.Replace("%", ""));
                    //LogHelper.AppendTxtAllpath(tempstr + "\r\n", path);
                    //Console.WriteLine("商品名称："+ goods_name);
                    //Console.WriteLine("打印的sql语句："+ sql.Replace("%", ""));
                    Console.WriteLine("分串：" + tempstr);
                    needInsertOrderCount++;
                    finalBatchInsertSql.Append(@"('" + create_time + "','" + click_time + "','" + goods_name + "','" + goods_id + "','" + wangwang + "','" + shop + "'," + goods_number + "," + goods_price + ",'" + order_status + "','" + order_type + "'," + income_ratio + "," + divided_ratio + "," + order_amount + "," + effect_prediction + "," + balance_amount + "," + estimated_revenue + ",'" + balance_time + "'," + commission_ratio + "," + commission_amount + "," + subsidy_ratio + "," + subsidy_amount + ",'" + subsidy_type + "','" + order_platform + "','" + serve_from + "','" + order_sn + "','" + category + "','" + media_id + "','" + media_name + "','" + adv_id + "','" + adv_name + "','" + tbUnionAccountId + "' ,'" + pid + "'),");
                }
               
                
                if (donging == false)
                {
                    Addlogs("停止");
                    return;
                }
                /**
                if (sqList.Count == 10)
                {
                   
                        int count = DbHelperMySQL.ExecuteSqlTran(sqList);
                        Addlogs("更新" + count + " 条新数据");
                        sqList.Clear();
                        sqList = new List<string>();
                    
                    
                }
                */
            }
            //if (sqList.Count == 0)
            if(needUpdateOrderCount==0&&needInsertOrderCount==0)
            {
                Addlogs("订单状态没有变化无需更新");
            }
            else
            {
                /*
                    int count = DbHelperMySQL.ExecuteSqlTran(sqList);
                    Addlogs("更新" + count + " 条新数据");
                    sqList.Clear();
                    sqList = new List<string>();
                    Addlogs("本轮更新完成");
                    */
                if (needUpdateOrderCount > 0)
                {


                    DbHelperMySQL.ExecuteSqlTran(sqList);
                    Addlogs("此次更新了"+needUpdateOrderCount+"笔订单");
                    sqList.Clear();
                    sqList = new List<string>();
                }

                if (needInsertOrderCount > 0)
                {
                    string finalSql = finalBatchInsertSql.ToString();
                    finalSql = finalSql.Substring(0, finalSql.Length - 1);
                    finalSql += ";";
                    Console.WriteLine("insert:" + finalSql);
                    DbHelperMySQL.ExecuteSql(finalSql.Replace("%", ""));
                    
                    Addlogs("此次插入了" + needInsertOrderCount + "笔订单");
                }
                
        }
            start = false;
            //2018-01-18 会抛异常“System.Threading.ThreadAbortException”(位于 mscorlib.dll 中)
            System.Threading.Thread.CurrentThread.Abort();
            //System.Threading.Thread.CurrentThread.Join();

        }

        private string Downer(string url, CookieContainer cookieContainer_0, string UA, string file)
        {
            string result = null;
            while (result == null)
            {
                try
                {
                    HttpWebRequest expr_0C = (HttpWebRequest)WebRequest.Create(url);
                    expr_0C.Method = "get";
                    expr_0C.UserAgent = UA;
                    expr_0C.AllowAutoRedirect = true;
                    expr_0C.CookieContainer = cookieContainer_0;
                    expr_0C.Timeout = 15000;
                    HttpWebResponse httpWebResponse = (HttpWebResponse)expr_0C.GetResponse();
                    Stream responseStream = httpWebResponse.GetResponseStream();

                    string text = file;
                    FileStream fileStream = new FileStream(text, FileMode.Create);
                    int count = 2048;
                    byte[] buffer = new byte[2048];
                    for (int i = responseStream.Read(buffer, 0, count);
                        i > 0;
                        i = responseStream.Read(buffer, 0, count))
                    {
                        fileStream.Write(buffer, 0, i);
                    }
                    responseStream.Close();
                    fileStream.Close();
                    httpWebResponse.Close();
                    result = text;
                    Thread.Sleep(100);
                }
                catch
                {

                }
            }
            return result;
        }

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


        private void Addlogs(string log)
        {
            if (listBox1.Items.Count > 300)
            {
                listBox1.Items.Clear();
            }
            listBox1.Items.Add(DateTime.Now.ToLongTimeString() + "=>" + log);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //淘宝联盟账号 combox
            /*
            IList<TbUnionAccountBean> infoList = new List<TbUnionAccountBean>();
            String accountArr = IniHelper.GetIniKeyValue("淘宝联盟账号", "集合");
            if (String.IsNullOrEmpty(accountArr))
            {
                Addlogs("淘宝联盟账号读取失败，请检查Config.ini是否配置淘宝联盟账号-集合");
                return;
            }
            else
            {
                String[] account = accountArr.Split('#');
                foreach(string accInfo in account)
                {
                    if (!String.IsNullOrEmpty(accInfo))
                    {
                        String[] accInfoArr = accInfo.Split('|');
                        if (accInfoArr.Length == 2)
                        {
                            TbUnionAccountBean info1 = new TbUnionAccountBean() { Id = accInfoArr[1], Name = accInfoArr[0] };
                            infoList.Add(info1);
                        }

                    }
                }
            }
            IList<TbUnionAccountBean> infoList = DataUtil.infoList;
            if (infoList.Count == 0)
            {
                Addlogs("淘宝联盟账号读取失败，请检查Config.ini是否配置淘宝联盟账号-集合\n格式：账号1|账号1id#账号1|账号1id" );
                return;
            }
            
            this.tbUnionAccountCombox.DataSource = infoList;
            this.tbUnionAccountCombox.ValueMember = "Id";
            this.tbUnionAccountCombox.DisplayMember = "Name";
            */
            //淘宝联盟账号 combox
            this.tbUnionAccountCombox.ValueMember = "Id";
            this.tbUnionAccountCombox.DisplayMember = "Name";
            DataPassWithinForm.ComboBoxInMainForm = this.tbUnionAccountCombox;

            DbHelperMySQL.connectionString =
              "Database='" + txtTable.Text.Trim() + "';Data Source='" + txt_server.Text + "';User Id='" + txt_name.Text + "';Password='" + txt_pass.Text + "';port='" + txt_port.Text + "';charset='utf8';pooling=true";
            numericUpDown1.Text = IniHelper.GetIniKeyValue("定时", "秒");
            textBox1.Text = IniHelper.GetIniKeyValue("淘宝客", "推广位");
            txt_server.Text = IniHelper.GetIniKeyValue("数据库", "地址");
            txt_port.Text = IniHelper.GetIniKeyValue("数据库", "端口");
            txt_name.Text = IniHelper.GetIniKeyValue("数据库", "登录名");
            txt_pass.Text = IniHelper.GetIniKeyValue("数据库", "密码");
            txtDbName.Text = IniHelper.GetIniKeyValue("数据库", "数据库名");
            txtTable.Text = IniHelper.GetIniKeyValue("数据库", "表名");

            label11.Text = numericUpDown1.Text + " 秒";

            string filepath = Application.StartupPath + "\\Orders";
            if (Directory.Exists(filepath))
            {
                Directory.Delete(filepath, true);
            }
            else
            {
                Directory.CreateDirectory(filepath);
            }
            Directory.CreateDirectory(filepath);
        }

        private bool donging = false;
        Thread th;
        private void button2_Click(object sender, EventArgs e)
        {
            if (label6.Text != "连接成功")
            {
                Addlogs("请先测试连接数据库");
                return;
            }
            if (button2.Text == "开始同步")
            {
                button2.Text = "停止";
                donging = true;
            }
            else
            {
                button2.Text = "开始同步";
                th.Abort();
                donging = false;
                start = false;

                return;
            }
            DbHelperMySQL.connectionString =
              "Database='" + txtDbName.Text.Trim() + "';Data Source='" + txt_server.Text + "';User Id='" + txt_name.Text + "';Password='" + txt_pass.Text + "';port='" + txt_port.Text + "';Charset='utf8';pooling=true";
            int day = 0;
            if (checkBox1.Checked)
            {
                day = 30;
            }
            else if (checkBox2.Checked)
            {
                day = 60;
            }
            th = new Thread(() =>
            {

                Tongbu(day);
            }
                );
            th.IsBackground = true;
            th.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                label6.Text = "正在链接...";
                DbHelperMySQL.connectionString =
                    "Database='" + txtDbName.Text.Trim() + "';Data Source='" + txt_server.Text + "';User Id='" + txt_name.Text + "';Password='" + txt_pass.Text + "';port='" + txt_port.Text + "';charset='utf8';pooling=true";

                //if (DbHelperMySQL.Exists("SELECT `id`FROM `taoke`.`" + dbTable + "`ORDER BY `id` DESC LIMIT 10"))
                if (isConnectedOK())
                {
                    label6.Text = "连接成功";
                    label6.ForeColor = Color.Green;
                }
                else
                {
                    label6.Text = "连接失败";
                    label6.ForeColor = Color.Red;
                }
            }).Start();


        }


        private bool isConnectedOK()
        {
            MySqlConnection mysqlCon = null;
            string sqlCmd = string.Empty;
            sqlCmd = DbHelperMySQL.connectionString;
            mysqlCon = new MySqlConnection(sqlCmd);

            /* 检测数据库有没有连接成功 */
            bool isConnectedOk = false;
            try
            {
                mysqlCon.Open();
                isConnectedOk = true;
            }
            catch
            {
                isConnectedOk = false;

                MessageBox.Show(this, "远程连接数据库失败!");
            }
            finally
            {
                mysqlCon.Clone();
            }
            if (!isConnectedOk)
                return false;
            return true;
        }



        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.EndInit();
            IniHelper.WriteIniKey("定时", "秒", numericUpDown1.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            IniHelper.WriteIniKey("淘宝客", "推广位", textBox1.Text);
        }

        private void txt_server_TextChanged(object sender, EventArgs e)
        {
            IniHelper.WriteIniKey("数据库", "地址", txt_server.Text);
        }

        private void txt_port_TextChanged(object sender, EventArgs e)
        {
            IniHelper.WriteIniKey("数据库", "端口", txt_port.Text);
        }

        private void txt_name_TextChanged(object sender, EventArgs e)
        {
            IniHelper.WriteIniKey("数据库", "登录名", txt_name.Text);
        }

        private void txt_pass_TextChanged(object sender, EventArgs e)
        {
            IniHelper.WriteIniKey("数据库", "密码", txt_pass.Text);
        }

        private int stime = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (start || donging == false)
            {
                return;
            }
            stime++;
            int settime = Convert.ToInt32(numericUpDown1.Text);
            int yu = settime - stime;
            if (yu <= 0)
            {
                stime = 0;
                yu = settime;
                th = new Thread(() =>
                {
                    try
                    {
                        int day = 0;
                        if (checkBox1.Checked)
                        {
                            day = 30;
                        }
                        else if (checkBox2.Checked)
                        {
                            day = 60;
                        }
                        Tongbu(day);

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                    );
                th.IsBackground = true;
                th.Start();

            }
            label11.Text = yu + " 秒";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox2.Checked;
        }

        private int loginnum = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            ClearMemory();
            loginnum++;

            if (loginnum >= 30)
            {
                loginnum = 0;
                new J_AliLoginForm().ShowDialog();
            }
        }
        #region 内存回收
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        #endregion

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private int nums = 0;
        private string oldstr = string.Empty;
        private string newstr = string.Empty;
        private void timer3_Tick(object sender, EventArgs e)
        {
            nums++;
            if (nums % 3 == 0 && listBox1.Items.Count - 1 >= 0)
            {
                newstr = listBox1.Items[listBox1.Items.Count - 1].ToString() + label11.Text;
                if (newstr == oldstr)
                {
                    th.Abort();
                    start = false;
                }
                oldstr = newstr;
            }
        }

        private void txtDbName_TextChanged(object sender, EventArgs e)
        {
            IniHelper.WriteIniKey("数据库", "数据库名", txtDbName.Text);
        }

        private void txtTable_TextChanged(object sender, EventArgs e)
        {
            IniHelper.WriteIniKey("数据库", "表名", txtTable.Text);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)//测试创建pid
        {
            //解析cookie中用于参数的字段
            CookieContainer cookieCollectionByString = GetCookieCollectionByString(DT.Cookie, "pub.alimama.com");
              
            StringBuilder header = new StringBuilder();
            header.Append("User-Agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36\r\n");
            header.Append("Content-Type:application/x-www-form-urlencoded; charset=UTF-8\r\n");
            header.Append("Accept:application/json, text/javascript, */*; q=0.01\r\n");
            header.Append("Accept-Encoding:gzip, deflate\r\n");
            header.Append("Accept-Language:zh-CN,zh;q=0.8\r\n");
            header.Append("Host: pub.alimama.com\r\n");
            header.Append("Origin:http://pub.alimama.com\r\n");
            header.Append("X-Requested-With:XMLHttpRequest\r\n");
            header.Append("Cookie:" + DT.Cookie);
            Wininet wininet = new Wininet();
            StringBuilder postData = new StringBuilder("tag=29&gcid=0&siteid=40968507&selectact=add&newadzonename=test&t="+DateTool.GetCurrentTimeStamp()+ "&_tb_token_="+ MyCookie.GetCookie("_tb_token_", cookieCollectionByString));
            Console.WriteLine("当前post数据："+ postData.ToString());

            String responseBody = wininet.PostData("http://pub.alimama.com/common/adzone/selfAdzoneCreate.json", postData.ToString(), header);
            Console.WriteLine("响应内容："+ responseBody);
            
        }
    }
}
