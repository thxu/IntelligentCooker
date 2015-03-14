using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Cooker.SystemUtils;

namespace IntelligentCooker
{
    public partial class formMain : Form
    {
        public formMain()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;

        }
        private TcpListener MyListener;
        private int port = 8080;
        private void InitListen()
        {
            try
            {
                MyListener = new TcpListener(port);
                MyListener.Start();
                Thread th = new Thread(new ThreadStart(StartListen));
                th.IsBackground = true;
                th.Start();
                txtShow.AppendText("监听开始" + "\r\n");
            }
            catch (System.Exception ex)
            {
                ShowErr("", ex);
                Logger.LogException(ex);
            }
        }

        private void StartListen()
        {
            while (true)
            {
                try
                {
                    Socket sokConnection = MyListener.AcceptSocket();
                    Thread myth = new Thread(HandleDatas);
                    myth.IsBackground = true;//不让它后台执行
                    myth.Start(sokConnection);
                }
                catch (System.Exception ex)
                {
                    ShowErr("", ex);
                    Logger.LogException(ex);
                }
            }
        }
        /// <summary>
        /// 处理数据线程开始处理数据
        /// </summary>
        /// <param name="sokconnection"></param>
        public void HandleDatas(object sokconnection)
        {
            Socket sokConnection = (Socket)sokconnection;
            try
            {
                if (sokConnection.Connected)
                {
                    SendMsg("@connect#", ref sokConnection);
                    Logger.LogMessage("有设备连接，返回@connect#", "");
                    byte[] byteReceiveMsg = new byte[1024];
                    int L = sokConnection.Receive(byteReceiveMsg, byteReceiveMsg.Length, 0);
                    if (L > 0)
                    {
                        string strReceiveMsg = Encoding.ASCII.GetString(byteReceiveMsg);
                        string strEffectiveData = GetEffectiveData(strReceiveMsg);
                        if (strEffectiveData == "invalid")
                        {
                            Logger.LogMessage("接受到无效数据，sok关闭", "");
                            sokConnection.Close();
                            return;
                        }
                        Logger.LogMessage("接收到有效数据，数据为：", strEffectiveData);
                        txtShow.AppendText("接收到的有效数据为:" + strEffectiveData + "\r\n");


                        string StrType = strEffectiveData.Substring(0, 1);
                        switch(StrType)
                        {
                            case "A":/*处理手机端*/
                                {
                                    Logger.LogMessage("开始调用处理手机端函数","");
                                    HandleCellphoneData(strEffectiveData, ref sokConnection);
                                    break;
                                }
                            case "B":/*处理下位机端*/
                                {
                                    Logger.LogMessage("开始调用处理下位机端函数", "");
                                    string Tmptype = strEffectiveData.Substring(1,1);
                                    
                                    if(Tmptype=="Z")/*下位机查询数据*/
                                    {
                                        string DeviceId = strEffectiveData.Substring(2, 15);
                                        string FireFlg = strEffectiveData.Substring(17, 1);
                                        SendtoDevice(DeviceId, FireFlg, ref sokConnection);
                                    }
                                    else if(Tmptype=="A"||Tmptype=="B"||Tmptype=="C")/*下位机返回数据接收情况*/
                                    {
                                        string deviceid = strEffectiveData.Substring(7,15);
                                        DeviceDataSetSendFlg(deviceid, ref sokConnection);
                                    }
                                    else if(Tmptype == "W")/*下位机正在工作*/
                                    {
                                        //string deviceid = strEffectiveData.Substring(7, 15);
                                        DeviceDataSetWorkFlg(strEffectiveData, ref sokConnection);
                                    }
                                    else
                                    {
                                        //DeviceDataSetSendFlg(DeviceId, ref sokConnection);
                                    }
                                    break;
                                }
                            default:
                                {
                                    Logger.LogMessage("未知信息，sok关闭", "");
                                    break;
                                }
                        }
                        sokConnection.Shutdown(SocketShutdown.Both);
                        sokConnection.Close();
                    }
                    else
                    {
                        Logger.LogMessage("未接收到数据，sok关闭", "");
                        sokConnection.Shutdown(SocketShutdown.Both);
                        sokConnection.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ShowErr("", ex);
                sokConnection.Shutdown(SocketShutdown.Both);
                sokConnection.Close();
                Logger.LogException(ex);
            }
        }
        public void HandleCellphoneData(string strEffectiveData,ref Socket sokConnection)
        {
            string[] strdata = strEffectiveData.Split('+');
            switch (strdata[0])
            {
                case "A":
                    {
                        if (strdata[1] == "R")/*用户注册*/
                        {
                            Logger.LogMessage("开始调用处理注册函数", "");
                            RegistUser(strdata[2], strdata[3], ref sokConnection);
                        }
                        else if (strdata[1] == "L")/*用户登录*/
                        {
                            Logger.LogMessage("开始调用处理登陆函数", "");
                            Login(strdata[2], strdata[3], ref sokConnection);
                        }
                        else if (strdata[1] == "A")/*用户添加绑定设备*/
                        {
                            Logger.LogMessage("开始调用绑定设备函数", "");
                            AddDevice(strdata[2], strdata[3], strdata[4], ref sokConnection);
                        }
                        else if (strdata[1] == "D")/*用户删除绑定设备*/
                        {
                            Logger.LogMessage("开始调用删除绑定设备函数", "");
                            DelDevice(strdata[2], strdata[3], strdata[4], ref sokConnection);
                        }
                        else if (strdata[1] == "S")/*用户向设备发送数据*/
                        {
                            Logger.LogMessage("开始调用设置米量，水量等的函数", "");
                            SetData(strdata[2], strdata[3], strdata[4], ref sokConnection);
                        }
                        else if (strdata[1] == "N")/*心跳包，查看有无新数据  A+N+用户名+设备号*/
                        {
                            Logger.LogMessage("开始调用处理用户心跳包的函数", "");
                            NormalAction(strdata[2],strdata[3], ref sokConnection);
                        }
                        break;
                    }
                default:
                    break;
            }
        }
        public void NormalAction(string name,string deviceid,ref Socket mysok)
        {
            try
            {
                string strsql = "select UserMsg,SendFlg from MyCookUser where UserName=@name";/*用户表中查找应该发送的信息*/
                SqlParameter para = new SqlParameter("@name", name);
                Logger.LogMessage("用户心跳：", "SQL语句，参数初始化完毕");
                DataTable dt = SqlHelper.MyHandleSelectSql(strsql, para);
                if (dt.Rows.Count > 0)
                {
                    string SendInfo = string.Empty;
                    string UserMsg = string.Empty;
                    string FirDevice = string.Empty;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["SendFlg"]) == 0)/*如果该条信息还未发送给用户*/
                        {
                            Logger.LogMessage("用户心跳：", "用户有未发送信息，开始查找该用户下有无报警设备");
                            UserMsg = dr["UserMsg"].ToString().Trim();
                            string strSql1 = "select DeviceID,FireFlg from MyCookDevice where UserName=@name";/*在设备表中找该用户下的设备有无报警*/
                            SqlParameter para1 = new SqlParameter("@name", name);
                            DataTable dt1 = SqlHelper.MyHandleSelectSql(strSql1, para1);
                            foreach (DataRow dr1 in dt1.Rows)
                            {
                                if (Convert.ToInt32(dr1["FireFlg"]) == 1)/*如果该设备报警*/
                                {
                                    Logger.LogMessage("用户心跳", "找到一个报警设备");
                                    FirDevice += dr1["DeviceID"].ToString().Trim() + "+";
                                }
                            }
                            SendInfo = "A+" + UserMsg + "+" + FirDevice;
                            Logger.LogMessage("用户心跳：", "用户有未发送信息，发送A+信息+报警设备号");
                            break;
                        }
                        else
                        {
                            SendInfo = "B";
                            Logger.LogMessage("用户心跳：", "用户没有未发送信息，返回B");
                            break;
                        }
                    }
                    string strWorkFlg = GetDeviceWorkFlg(deviceid);  /*取得下位机的煮饭状态*/
                    if(strWorkFlg != "NO")
                    {
                        SendInfo = SendInfo + strWorkFlg;
                    }
                    SendMsgToPhone(SendInfo, ref mysok);
                    //string strSql2 = "update MyCookUser set SendFlg=0 where UserName=@name";/*将发送标志修改为已发送*/
                    //SqlParameter para2 = new SqlParameter("@name", name);
                    //Logger.LogMessage("用户心跳：", "信息发送成功，标记该用户已发送过信息");
                    //SqlHelper.MyHandleUpdateSql(strSql2, para2);
                }
                else
                {
                    SendMsg("B", ref mysok);
                    Logger.LogMessage("用户心跳：", "没有该用户，返回B");
                }
                
            }
            catch(Exception ex)
            {
                ShowErr("", ex);
                SendMsg("B", ref mysok);
                Logger.LogMessage("用户心跳：", "执行SQL语句报错，返回B");
                Logger.LogException(ex);
            }
        }
        public string GetDeviceWorkFlg(string deviceid)
        {
            try
            {
                string strSql = "select WorkFlg from MyCookDevice where DeviceID=@deviceid";      /*查找该设备的煮饭状态*/
                SqlParameter para = new SqlParameter("@deviceid", deviceid);
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, para);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        return dr["WorkFlg"].ToString().Trim();
                    }
                    return "NO";
                }
                else
                {
                    return "NO";
                }
            }
            catch (Exception ex)
            {
                ShowErr("", ex);
                return "NO";
            }
        }
        public void DeviceDataSetSendFlg(string deviceid,ref Socket mysok)
        {
            try
            {
                string strSql = "update MyCookData set SendFlg=1 where DeviceID=@deviceid";
                SqlParameter para = new SqlParameter("@deviceid", deviceid);
                SqlHelper.MyHandleUpdateSql(strSql, para);
            }
            catch (Exception ex)
            {
                Logger.LogMessage("下位机返回设置成功","SQL语句执行报错，发送标记重置为0失败");
                Logger.LogException(ex);
            }
        }
        public void DeviceDataSetWorkFlg(string strEffectiveData, ref Socket mysok)
        {
            try              /*接收到的信息为@BW0+Z014101000000001#之类 0代表煮饭结束  1代表煮第一顿饭*/
            {
                string[] strdata = strEffectiveData.Split('+');
                string strCookFlg = strdata[0].Substring(2, 1);
                string strDeviceid = strdata[1].Substring(1);
                string strSql = "update MyCookDevice set WorkFlg=@strCookFlg where DeviceID=@deviceid";
                SqlParameter[] paras ={
                                         new SqlParameter("@strCookFLg",strCookFlg),
                                         new SqlParameter("@deviceid",strDeviceid)
                                     };
                Logger.LogMessage("处理下位机煮饭状态", "sql语句初始化完毕");
                SqlHelper.MyHandleUpdateSql(strSql, paras);
                SendMsg("@BW_OK#", ref mysok);
                Logger.LogMessage("处理下位机煮饭状态", "状态保存成功  返回BW_OK");
            }
            catch (Exception ex)
            {
                Logger.LogMessage("处理下位机煮饭状态", "SQL语句执行报错，发送标记重置为0失败");
                Logger.LogException(ex);
            }
        }
        /// <summary>
        /// 注册设备和向设备发送煮饭数据
        /// </summary>
        /// <param name="deviceid"></param>
        /// <param name="fireflg"></param>
        /// <param name="mysok"></param>
        public void SendtoDevice(string deviceid, string fireflg, ref Socket mysok)
        {
            try
            {
                string strSql = "select * from MyCookData where DeviceID=@deviceid";/*在数据库中查找该设备的所有信息*/
                SqlParameter para = new SqlParameter("@deviceid", deviceid);
                Logger.LogMessage("下位机信息处理：", "SQL语句，参数初始化完毕");
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, para);
                string strTime = GetTime();
                string strSendInfo = string.Empty;
                if (dt.Rows.Count > 0)/*数据库中有该设备*/
                {
                    Logger.LogMessage("下位机信息处理：", "数据库中 有 该设备");
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["SendFlg"])==0)/*该数据还未发送给下位机*/
                        {
                            if (Convert.ToInt32(dr["TmpdataFlg"]) == 1)/*临时数据标志位为真  发送临时数据*/
                            {
                                strSendInfo = "@BA" + strTime + dr["TmpData"].ToString() + "#";
                            }
                            else
                            {
                                strSendInfo = "@BC" + strTime + dr["RepateData"].ToString() + "#";
                            }
                           
                        }
                        else
                        {
                            strSendInfo = "@BB"+strTime+"#";
                        }
                        SendMsg(strSendInfo, ref mysok);
                        Logger.LogMessage("下位机信息处理；", "返回用户设置的数据");
                        break;
                    }
                }
                else/*数据库中没有该设备*/
                {
                    Logger.LogMessage("下位机信息处理：", "数据库中 没有 该设备");
                    string strSql1 = "insert into MyCookData(DeviceID) values(@deviceid)";
                    string strSql3 = "insert into MyCookDevice(DeviceID) values(@deviceid)";
                    SqlParameter para1 = new SqlParameter("@deviceid", deviceid);
                    SqlParameter para4 = new SqlParameter("@deviceid", deviceid);
                    SqlHelper.MyHandleUpdateSql(strSql1, para1);
                    SqlHelper.MyHandleUpdateSql(strSql3, para4);
                    SendMsg("@BB" + strTime + "#", ref mysok);
                    Logger.LogMessage("下位机信息处理：", "在数据库中新录入该设备，返回@BB+时间#");
                }
                Logger.LogMessage("下位机信息处理：", "开始处理 下位机 报警信息");
                string strSql2 = string.Empty;
                if (fireflg == "A")/*A代表有煤气泄漏等*/
                {
                    Logger.LogMessage("下位机信息处理：", "下位机 有 煤气泄漏");
                    strSql2 = "update MyCookDevice set FireFlg=1 where DeviceID=@deviceid";
                    string strSql4 = "select UserName from MyCookDevice where DeviceID=@deviceid";/*找到该设备对应的用户*/
                    SqlParameter para2 = new SqlParameter("@deviceid", deviceid);
                    DataTable dt1 = SqlHelper.MyHandleSelectSql(strSql4, para2);
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in dt1.Rows)
                        {
                            string strName = dr1["UserName"].ToString().Trim();
                            if (strName != "" || strName != null)
                            {
                                string strSql5 = "update MyCookUser set SendFlg=0 where UserName=@name";/*将该用户的发送标志改为未发送*/
                                SqlParameter para3 = new SqlParameter("@name", strName);
                                SqlHelper.MyHandleUpdateSql(strSql5, para3);
                                Logger.LogMessage("下位机信息处理：", "找到该设备对应的用户，将该用户的消息发送标记改为未发送");
                            }
                            break;
                        }
                    }
                }
                else
                {
                    strSql2 = "update MyCookDevice set FireFlg=0 where DeviceID=@deviceid";
                    Logger.LogMessage("下位机信息处理：", "下位机 没有 煤气泄漏");
                }
                SqlParameter paras2 = new SqlParameter("@deviceid",deviceid);
                SqlHelper.MyHandleUpdateSql(strSql2, paras2);

            }
            catch (Exception ex)
            {
                ShowErr("", ex);
                Logger.LogMessage("下位机信息处理：", "SQL执行报错");
                Logger.LogException(ex);
            }
        }
        public string GetTime()
        {
            try
            {
                string strH = DateTime.Now.Hour.ToString().Trim();
                string strM = DateTime.Now.Minute.ToString().Trim();
                string strS = DateTime.Now.Second.ToString().Trim();
                if (strH.Length == 1)
                {
                    strH = "0" + strH;
                }
                if (strM.Length == 1)
                {
                    strM = "0" + strM;
                }
                if (strS.Length == 1)
                {
                    strS = "0" + strS;
                }
                string myTime = string.Empty;
                myTime = strH + ":" + strM + ":" + strS;
                return myTime;
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }
        /// <summary>
        /// 保存用户的煮饭数据
        /// </summary>
        /// <param name="deviceid"></param>
        /// <param name="datatype"></param>
        /// <param name="cookdata"></param>
        /// <param name="mysok"></param>
        public void SetData(string deviceid, string datatype, string cookdata, ref Socket mysok)
        {
            try
            {
                string strSqlworkFlg = "select WorkFlg from MyCookDevice where DeviceID=@deviceid";
                SqlParameter paraWorkFlg = new SqlParameter("@deviceid", deviceid);
                DataTable dt1 = SqlHelper.MyHandleSelectSql(strSqlworkFlg, paraWorkFlg);
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr1 in dt1.Rows)
                    {
                        if (dr1["WorkFlg"].ToString().Trim() == "1")
                        {
                            SendMsg("D", ref mysok);
                            return;
                        }
                    }
                }
                string strSql = "";
                if (datatype == "A")   /*A:用户临时设置   B：用户重复设置*/
                {
                    strSql = "update MyCookData set TmpData=@cookdata,TmpdataFlg=1,SendFlg=0 where DeviceID=@deviceid";
                }
                else
                {
                    strSql = "update MyCookData set RepateData=@cookdata,TmpdataFlg=0,SendFlg=0 where DeviceID=@deviceid";
                }
                SqlParameter[] paras = {
                                           new SqlParameter("@cookdata",cookdata),
                                           new SqlParameter("@deviceid",deviceid)
                                       };
                Logger.LogMessage("设置数据函数：", "SQL语句，参数初始化完毕");
                int res = -1;
                res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                if (res > 0)
                {
                    SendMsg("A", ref mysok);
                    Logger.LogMessage("设置数据函数：", "数据库1行受影响，设置数据成功，返回A");
                }
                else
                {
                    SendMsg("B", ref mysok);
                    Logger.LogMessage("设置数据函数：", "数据库0行受影响，设置数据失败，返回B");
                }
            }
            catch (Exception ex)
            {
                ShowErr("", ex);
                SendMsg("B", ref mysok);
                Logger.LogMessage("设置数据函数：", "SQL语句执行报错，设置失败，返回B");
                Logger.LogException(ex);
            }
        }
        /// <summary>
        /// 解绑用户绑定的设备
        /// </summary>
        /// <param name="name"></param>
        /// <param name="deveiceid"></param>
        /// <param name="devicepwd"></param>
        /// <param name="mysok"></param>
        public void DelDevice(string name, string deveiceid, string devicepwd, ref Socket mysok)
        {
            try
            {
                string strSql = "select DevicePwd from MyCookDevice where DeviceID=@deviceid";
                SqlParameter para = new SqlParameter("@deviceid", deveiceid);
                Logger.LogMessage("删除绑定设备函数：", "SQL语句，参数初始化完毕");
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, para);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["DevicePwd"].ToString().Trim() == devicepwd)  /*判定用户输入的设备密码是否正确*/
                        {
                            Logger.LogMessage("删除绑定设备函数：", "设备密码正确，开始清除该设备下的用户名");
                            string strSql1 = "update MyCookDevice set UserName='' where DeviceID=@deviceid";
                            SqlParameter paras = new SqlParameter("@deviceid", deveiceid);
                            int res = -1;
                            res = SqlHelper.MyHandleUpdateSql(strSql1, paras); /*用户绑定设备*/
                            if (res > 0)
                            {
                                txtShow.AppendText("删除成功\r\n");
                                SendMsg("A", ref mysok);
                                Logger.LogMessage("删除绑定设备函数：", "数据库1行受影响，删除绑定成功，返回A");
                            }
                            else
                            {
                                SendMsg("B", ref mysok);
                                Logger.LogMessage("删除绑定设备函数：", "数据库0行受影响，删除绑定失败，返回B");
                            }
                        }
                        else
                        {
                            SendMsg("B", ref mysok);
                            Logger.LogMessage("删除绑定设备函数：", "设备密码错误，删除绑定失败，返回B");
                        }
                        break;
                    }
                }
                else
                {
                    SendMsg("B", ref mysok);
                    Logger.LogMessage("删除绑定设备函数：", "没有找到该设备错误，删除绑定失败，返回B");
                }
            }
            catch (Exception ex)
            {
                ShowErr("", ex);
                SendMsg("B", ref mysok);
                Logger.LogMessage("删除绑定设备函数：", "SQL语句执行报错，删除绑定失败，返回B");
                Logger.LogException(ex);
            }
        }
        /// <summary>
        /// 用户绑定设备
        /// </summary>
        /// <param name="name"></param>
        /// <param name="deveiceid"></param>
        /// <param name="devicepwd"></param>
        /// <param name="mysok"></param>
        public void AddDevice(string name, string deveiceid, string devicepwd, ref Socket mysok)
        {
            try
            {
                string strSql = "select DevicePwd,IsDel from MyCookDevice where DeviceID=@deviceid";
                SqlParameter para = new SqlParameter("@deviceid", deveiceid);
                Logger.LogMessage("绑定设备函数：", "SQL语句，参数初始化完毕");
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, para);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if(dr["IsDel"].ToString().Trim() == "1")
                        {
                            SendMsg("B", ref mysok);
                            Logger.LogMessage("绑定设备函数：", "设备已删除，返回绑定失败");
                            break;
                        }
                        if (dr["DevicePwd"].ToString().Trim() == devicepwd)  /*判定用户输入的设备密码是否正确*/
                        {
                            Logger.LogMessage("绑定设备函数：", "设备密码正确，开始将该设备的用户名设置为该用户");
                            string strSql1 = "update MyCookDevice set UserName=@name where DeviceID=@deviceid";
                            SqlParameter[] paras = {
                                                   new SqlParameter("@name",name),
                                                   new SqlParameter("@deviceid",deveiceid)
                                               };
                            int res = -1;
                            res = SqlHelper.MyHandleUpdateSql(strSql1, paras); /*用户绑定设备*/
                            if (res > 0)
                            {
                                txtShow.AppendText("绑定成功\r\n");
                                SendMsg("A", ref mysok);
                                Logger.LogMessage("绑定设备函数：", "数据库1行受影响，绑定成功，返回A");
                            }
                            else
                            {
                                SendMsg("B", ref mysok);
                                Logger.LogMessage("绑定设备函数：", "数据库0行受影响，绑定失败，返回B");
                            }
                        }
                        else
                        {
                            SendMsg("B", ref mysok);
                            Logger.LogMessage("绑定设备函数：", "设备密码错误，绑定失败，返回B");
                        }
                        break;
                    }
                }
                else
                {
                    SendMsg("B", ref mysok);
                    Logger.LogMessage("绑定设备函数：", "没有找到该设备，绑定失败，返回B");
                }
            }
            catch (Exception ex)
            {
                ShowErr("", ex);
                SendMsg("B", ref mysok);
                Logger.LogMessage("绑定设备函数：", "执行SQL语句报错，绑定失败，返回B");
                Logger.LogException(ex);
            }
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="mysok"></param>
        public void Login(string name, string pwd, ref Socket mysok)/*登陆成功返回A+设备号   否则返回B*/
        {
            try
            {
                string strSql = "select UserPwd from MyCookUser where UserName=@name";
                SqlParameter paras = new SqlParameter("@name", name);
                Logger.LogMessage("登陆函数：", "SQL语句和参数初始化完毕");
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, paras);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["UserPwd"].ToString().Trim() == pwd)   /*判断用户的密码是否正确*/
                        {
                            Logger.LogMessage("登陆函数：", "密码正确，开始查找该用户的设备");
                            string strSql1 = "select DeviceID,IsDel from MyCookDevice where Username=@name";
                            SqlParameter paras1 = new SqlParameter("@name", name);
                            DataTable dt1 = SqlHelper.MyHandleSelectSql(strSql1, paras1);
                            string SendInfo = "A+";
                            foreach (DataRow dr1 in dt1.Rows)   /*找到该用户的所有设备*/
                            {
                                if (dr1["IsDel"].ToString().Trim() == "0")
                                {
                                    Logger.LogMessage("登陆函数：", "查找到一个该用户使用的设备");
                                    SendInfo = SendInfo + dr1["DeviceID"].ToString().Trim() + "+";
                                }
                            }
                            SendMsg(SendInfo, ref mysok);
                            Logger.LogMessage("登陆函数：", "登陆成功，返回A+设备号");
                        }
                        else
                        {
                            Logger.LogMessage("登陆函数：", "密码错误，登陆失败，返回B");
                            SendMsg("B", ref mysok);
                        }
                        break;
                    }
                }
                else
                {
                    SendMsg("B", ref mysok);
                    Logger.LogMessage("登陆函数：", "没有找到该用户，登陆失败，返回B");
                }
            }
            catch (System.Exception ex)
            {
                ShowErr("", ex);
                SendMsg("B", ref mysok);
                Logger.LogMessage("登陆函数：", "SQL语句执行报错，登陆失败，返回B");
                Logger.LogException(ex);
            }
        }
        /// <summary>
        /// 用户注册  注册成功返回A  否则返回B
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="mysok"></param>
        public void RegistUser(string name, string pwd, ref Socket mysok)
        {
            try
            {
                string strSql = "insert into MyCookUser(UserName,UserPwd) values(@Name,@Pwd)";
                SqlParameter[] paras = {
                                           new SqlParameter("@Name",name),
                                           new SqlParameter("@Pwd",pwd)
                                       };
                Logger.LogMessage("注册函数：", "SQL语句和参数初始化完毕");
                int res = -1;
                res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                if (res > 0)
                {
                    txtShow.AppendText("注册成功\r\n");
                    SendMsg("A", ref mysok);
                    Logger.LogMessage("注册函数：", "数据库1行受影响，用户注册成功，返回A");
                }
                else
                {
                    SendMsg("B", ref mysok);
                    Logger.LogMessage("注册函数", "数据库0行受影响，用户注册失败，返回B");
                }
            }
            catch (System.Exception ex)
            {
                ShowErr("", ex);
                SendMsg("B", ref mysok);
                Logger.LogMessage("注册函数", "数据库执行SQL报错，用户注册失败，返回B");
                Logger.LogException(ex);
            }
        }
        /// <summary>
        /// 从字符串中截取有效数据
        /// </summary>
        /// <param name="receivedata"></param>
        /// <returns></returns>
        public string GetEffectiveData(string receivedata)
        {
            try
            {

                int iStartPos = 0, iEndPos = 0;
                iStartPos = receivedata.IndexOf("@", 0);
                iEndPos = receivedata.IndexOf("#", iStartPos);
                if (iEndPos == -1 || iStartPos == -1)
                {
                    return "invalid";
                }
                return receivedata.Substring(iStartPos + 1, (iEndPos - iStartPos) - 1);
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
                return "invalid";
            }
        }
        public void SendMsgToPhone(string sData,ref Socket mySocket)
        {
            SendMsg(Encoding.UTF8.GetBytes(sData), ref mySocket);
        }
        public void SendMsg(String sData, ref Socket mySocket)
        {
            SendMsg(Encoding.ASCII.GetBytes(sData), ref mySocket);
        }
        public void SendMsg(Byte[] bSendData, ref Socket mySocket)
        {
            int numBytes = 0;
            try
            {
                if (mySocket.Connected)
                {
                    if ((numBytes = mySocket.Send(bSendData, bSendData.Length, 0)) == -1)
                    {
                        txtShow.AppendText("信息发送失败\r\n");
                    }
                    else
                    {
                        txtShow.AppendText("信息发送成功:" + Encoding.UTF8.GetString(bSendData) + "\r\n");
                        Logger.LogMessage("信息发送成功","");
                    }
                }
                else
                {
                    txtShow.AppendText("连接失败....\r\n");
                    Logger.LogMessage("连接断开，发送失败","");
                }
            }
            catch (Exception ex)
            {
                ShowErr("发生错误", ex);
                Logger.LogException(ex);
            }
        }
        #region 错误处理
        public void ShowErr(string msg, Exception ex)
        {
            ShowMsg("---------------------begin------------");
            ShowMsg(msg + "" + ex.Message);
            ShowMsg("---------------------end--------------");
        }
        public void ShowMsg(string msg)
        {
            txtShow.AppendText(msg + "\r\n");
        }
        #endregion

        private void txtbox_UserName_Click(object sender, EventArgs e)
        {
            if (txtbox_UserName.Text.Trim() == "请在此处输入用户名")
            {
                txtbox_UserName.Text = "";
            }
        }

        private void txtBox_SendMsg_Click(object sender, EventArgs e)
        {
            if (txtBox_SendMsg.Text.Trim() == "请在此处写下发给所有用户的信息")
            {
                txtBox_SendMsg.Text = "";
            }
        }

        private void btn_SerchUser_Click(object sender, EventArgs e)
        {
            try
            {
                string strSql = "select * from MyCookUser where UserName=@name";
                SqlParameter para = new SqlParameter("@name", txtbox_UserName.Text.Trim());
                Logger.LogMessage("管理员在搜索指定用户", "");
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, para);
                dgv_User.DataSource = dt;
            }
            catch (Exception ex)
            {
                Logger.LogMessage("管理员在搜索指定用户", "SQL语句执行报错，搜索失败");
                Logger.LogException(ex);
            }
        }

        private void dgv_User_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                int rowIndex = e.RowIndex;
                if (rowIndex >= 0)
                {
                    Logger.LogMessage("管理员在datagridview上选中了一行", "");
                    string strID = dgv_User.Rows[rowIndex].Cells[0].Value.ToString().Trim();
                    string strName = dgv_User.Rows[rowIndex].Cells[1].Value.ToString().Trim();
                    string strPwd = dgv_User.Rows[rowIndex].Cells[2].Value.ToString().Trim();
                    string strMsg = dgv_User.Rows[rowIndex].Cells[3].Value.ToString().Trim();
                    lab_UserID.Text = strID;
                    txtBox_Name.Text = strName;
                    txtBox_Pwd.Text = strPwd;
                    txtBox_UserMsg.Text = strMsg;
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            try
            {

                string strUserName = txtBox_Name.Text.Trim();
                string strUserPwd = txtBox_Pwd.Text.Trim();
                string strUserMsg = txtBox_UserMsg.Text.Trim();
                string strSql = "insert into MyCookUser(UserName,UserPwd,UserMsg) values(@Name,@Pwd,@Msg)";
                SqlParameter[] paras = { 
                                   new SqlParameter("@Name",strUserName),
                                   new SqlParameter("@Pwd",strUserPwd),
                                   new SqlParameter("@Msg",strUserMsg)
                                   };
                Logger.LogMessage("管理员新增用户", "SQL语句，参数初始化完毕");
                int res = -1;
                try
                {
                    res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                    if (res > 0)
                    {
                        MessageBox.Show("新增用户成功");
                        Logger.LogMessage("管理员新增用户", "数据库返回1行受影响，插入成功");
                        UpdataDGV_User();
                    }
                    else
                    {
                        MessageBox.Show("新增用户失败");
                        Logger.LogMessage("管理员新增用户", "数据库返回0行受影响，插入失败");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("新增用户失败，用户名重复");
                    Logger.LogMessage("管理员新增用户", "SQL语句执行报错，新增用户失败");
                    Logger.LogException(ex);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private void btn_Change_Click(object sender, EventArgs e)
        {
            try
            {
                string strUserName = txtBox_Name.Text.Trim();
                string strUserPwd = txtBox_Pwd.Text.Trim();
                string strUserMsg = txtBox_UserMsg.Text.Trim();
                string strUserID = lab_UserID.Text.Trim();
                string strSql = "update MyCookUser set UserName=@username,UserPwd=@userpwd,UserMsg=@usermsg where UserID=@userid";
                SqlParameter[] paras = { 
                                   new SqlParameter("@username",strUserName),
                                   new SqlParameter("@userpwd",strUserPwd),
                                   new SqlParameter("@usermsg",strUserMsg),
                                   new SqlParameter("@userid",strUserID)
                                   };
                Logger.LogMessage("管理员修改用户", "SQL语句，参数初始化完毕");
                int res = -1;
                try
                {
                    res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("修改用户信息失败，用户名重复");
                    Logger.LogMessage("管理员修改用户", "SQL语句执行报错，修改失败，错误原因可能为用户名重复");
                    Logger.LogException(ex);
                }
                if (res > 0)
                {
                    MessageBox.Show("更新用户成功");
                    Logger.LogMessage("管理员修改用户", "数据库返回1行受影响，修改成功");
                    UpdataDGV_User();
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void btn_Del_Click(object sender, EventArgs e)
        {
            try
            {
                string strUserID = lab_UserID.Text.Trim();
                string strSql = "delete from MyCookUser where UserID=@userid";
                SqlParameter para = new SqlParameter("@userid", strUserID);
                Logger.LogMessage("管理员删除用户", "SQL语句，参数初始化完毕");
                int res = -1;
                res = SqlHelper.MyHandleUpdateSql(strSql, para);
                if (res > 0)
                {
                    MessageBox.Show("删除成功");
                    Logger.LogMessage("管理员删除用户", "数据库返回1行受影响，删除成功");
                    UpdataDGV_User();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("删除失败");
                Logger.LogMessage("管理员删除用户", "SQL语句执行报错，删除失败");
                Logger.LogException(ex);
            }
        }

        private void btn_SendAll_Click(object sender, EventArgs e)
        {
            try
            {
                string strMsg = txtBox_SendMsg.Text.Trim();
                string strSql = "update MyCookUser set UserMsg=@msg where UserID=@userid";
                string strSql1 = "select UserID from MyCookUser";
                Logger.LogMessage("管理员给所有用户发送消息", "SQL语句，参数初始化完毕");
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql1);
                foreach (DataRow dr in dt.Rows)
                {
                    SqlParameter[] para = 
                                {   
                                     new SqlParameter("@msg",strMsg),
                                     new SqlParameter("@userid", dr["UserID"].ToString().Trim())
                                };
                    SqlHelper.MyHandleUpdateSql(strSql, para);
                    Logger.LogMessage("管理员给所有用户发送信息", "数据库更新成功，发送成功");
                    MessageBox.Show("已发送给所有用户");
                    UpdataDGV_User();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("发送失败");
                Logger.LogException(ex);
            }
        }
        private void UpdataDGV_User()
        {
            try
            {
                string strSql = "select * from MyCookUser";
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql);
                dgv_User.DataSource = dt;
                dgv_User.Refresh();
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private void UpdataDGV_Device()
        {
            try
            {
                string strSql = "select * from MyCookDevice";
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql);
                dgv_Device.DataSource = dt;
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void txtBox_SerchDeviceID_Click(object sender, EventArgs e)
        {
            if (txtBox_SerchDeviceID.Text.Trim() == "请输入要搜索的设备号")
            {
                txtBox_SerchDeviceID.Text = "";
            }
        }

        private void btn_SerchDevice_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeviceID = txtBox_SerchDeviceID.Text.Trim();
                string strSql = "select * from MyCookDevice where DeviceID=@deviceid";
                SqlParameter para = new SqlParameter("@deviceid", strDeviceID);
                Logger.LogMessage("管理员搜索设备号", "SQL语句，参数初始化完毕");
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, para);
                Logger.LogMessage("管理员搜索设备号", "搜索成功");
                dgv_Device.DataSource = dt;
            }
            catch (System.Exception ex)
            {
                Logger.LogMessage("管理员搜索设备号", "SQL语句执行报错，搜索失败");
                Logger.LogException(ex);
            }
        }

        private void dgv_Device_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                if (rowIndex >= 0)
                {
                    Logger.LogMessage("管理员选中了datagridview的某一行", "");
                    string strUser = dgv_Device.Rows[rowIndex].Cells[0].Value.ToString().Trim();
                    string strDeviceID = dgv_Device.Rows[rowIndex].Cells[1].Value.ToString().Trim();
                    string strDevicePwd = dgv_Device.Rows[rowIndex].Cells[2].Value.ToString().Trim();
                    string strIsDel = dgv_Device.Rows[rowIndex].Cells[3].Value.ToString().Trim();
                    string strFireFlg = dgv_Device.Rows[rowIndex].Cells[4].Value.ToString().Trim();
                    txtBox_User.Text = strUser;
                    txtBox_DeviceID.Text = strDeviceID;
                    txtBox_DevicePwd.Text = strDevicePwd;
                    if(strIsDel=="0")
                    {
                        txtBox_IsDel.Text = "否";
                    }
                    else
                    {
                        txtBox_IsDel.Text = "是";
                    }
                    if (strFireFlg=="0")
                    {
                        txtBox_FireFlg.Text = "否";
                    }
                    else
                    {
                        txtBox_FireFlg.Text = "是";
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogMessage("管理员点击DGV_Device", "取datagridview值出错");
                Logger.LogException(ex);
            }
        }

        private void btn_DeviceChange_Click(object sender, EventArgs e)
        {
            try
            {
                string strUser = txtBox_User.Text.Trim();
                string strDeviceID = txtBox_DeviceID.Text.Trim();
                string strDevicePwd = txtBox_DevicePwd.Text.Trim();
                string strIsDel = txtBox_IsDel.Text.Trim();
                string strFireFlg = txtBox_FireFlg.Text.Trim();
                if(strIsDel=="是")
                {
                    strIsDel = "1";
                }
                else if(strIsDel=="否")
                {
                    strIsDel = "0";
                }
                else
                {
                    MessageBox.Show("是否已经删除只能填‘是’或者‘否’");
                    return;
                }
                if(strFireFlg=="是")
                {
                    strFireFlg = "1";
                }
                else if(strFireFlg=="否")
                {
                    strFireFlg = "0";
                }
                else
                {
                    MessageBox.Show("是否有火灾或者煤气泄漏只能填‘是’或者‘否’");
                    return;
                }
                string strSql = "update MyCookDevice set UserName=@username,DevicePwd=@devicepwd,IsDel=@isdel,FireFlg=@fireflg where DeviceID=@deviceid";
                SqlParameter[] paras = { 
                                   new SqlParameter("@username",strUser),
                                   new SqlParameter("@deviceid",strDeviceID),
                                   new SqlParameter("@devicepwd",strDevicePwd),
                                   new SqlParameter("@isdel",strIsDel),
                                   new SqlParameter("@fireflg",strFireFlg)
                                   };
                Logger.LogMessage("管理员修改设备", "SQL语句，参数初始化完毕");
                int res = -1;
                res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                if (res > 0)
                {
                    MessageBox.Show("修改成功");
                    Logger.LogMessage("管理员修改设备", "数据库1行受影响，修改成功");
                    string strSql1 = "update MyCookData Set SendFlg=0 where DeviceID=@deviceid";
                    SqlParameter para1 = new SqlParameter("@deviceid", strDeviceID);
                    SqlHelper.MyHandleUpdateSql(strSql1,para1);
                    UpdataDGV_Device();
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogMessage("管理员修改设备", "SQL语句执行报错，修改失败");
                Logger.LogException(ex);
            }
        }

        private void btn_DeviceDel_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeviceID = txtBox_DeviceID.Text.Trim();
                string strSql = "update MyCookDevice set IsDel=1 where DeviceID=@deviceid";
                SqlParameter paras = new SqlParameter("@deviceid", strDeviceID);
                Logger.LogMessage("管理员删除设备", "SQL语句，参数初始化完毕");
                int res = -1;
                res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                if (res > 0)
                {
                    MessageBox.Show("删除成功");
                    Logger.LogMessage("管理员删除设备", "数据库1行受影响，删除成功");
                    UpdataDGV_Device();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("删除失败");
                Logger.LogMessage("管理员删除设备", "SQL语句执行报错，删除失败");
                Logger.LogException(ex);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                InitListen();
                //this.tab_DeviceManage.Parent = null;
                this.tab_UserManage.Parent = null;
                UpdataDGV_Device();
                Setdgv_Device_HeadText();
                if (dgv_Device.RowCount > 0)
                {
                    string strUser = dgv_Device.Rows[0].Cells[0].Value.ToString().Trim();
                    string strDeviceID = dgv_Device.Rows[0].Cells[1].Value.ToString().Trim();
                    string strDevicePwd = dgv_Device.Rows[0].Cells[2].Value.ToString().Trim();
                    string strIsDel = dgv_Device.Rows[0].Cells[3].Value.ToString().Trim();
                    string strFireFlg = dgv_Device.Rows[0].Cells[4].Value.ToString().Trim();
                    txtBox_User.Text = strUser;
                    txtBox_DeviceID.Text = strDeviceID;
                    txtBox_DevicePwd.Text = strDevicePwd;
                    txtBox_IsDel.Text = strIsDel;
                    txtBox_FireFlg.Text = strFireFlg;
                    if (strIsDel == "0")
                    {
                        txtBox_IsDel.Text = "否";
                    }
                    else
                    {
                        txtBox_IsDel.Text = "是";
                    }
                    if (strFireFlg == "0")
                    {
                        txtBox_FireFlg.Text = "否";
                    }
                    else
                    {
                        txtBox_FireFlg.Text = "是";
                    }
                }

                UpdataDGV_User();
                Setdgv_User_HeadText();
                if (dgv_User.RowCount > 0)
                {
                    string strID = dgv_User.Rows[0].Cells[0].Value.ToString().Trim();
                    string strName = dgv_User.Rows[0].Cells[1].Value.ToString().Trim();
                    string strPwd = dgv_User.Rows[0].Cells[2].Value.ToString().Trim();
                    string strMsg = dgv_User.Rows[0].Cells[3].Value.ToString().Trim();
                    lab_UserID.Text = strID;
                    txtBox_Name.Text = strName;
                    txtBox_Pwd.Text = strPwd;
                    txtBox_UserMsg.Text = strMsg;
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        public void Setdgv_User_HeadText()
        {
            dgv_User.Columns[0].HeaderText = "行号";
            dgv_User.Columns[1].HeaderText = "用户名";
            dgv_User.Columns[2].HeaderText = "密码";
            dgv_User.Columns[3].HeaderText = "用户消息";
            dgv_User.Columns[4].HeaderText = "是否已经向用户发送信息";
        }
        public void Setdgv_Device_HeadText()
        {
            dgv_Device.Columns[0].HeaderText = "用户名";
            dgv_Device.Columns[1].HeaderText = "设备号";
            dgv_Device.Columns[2].HeaderText = "设备密码密码";
            dgv_Device.Columns[3].HeaderText = "是否已经删除该设备";
            dgv_Device.Columns[4].HeaderText = "是否有煤气泄漏";
        }

        private void btn_UpdateUserDgv_Click(object sender, EventArgs e)
        {
            UpdataDGV_User();
        }

        private void btn_UpdateDeviceDgv_Click(object sender, EventArgs e)
        {
            UpdataDGV_Device();
        }

        private void btn_SendMsgToSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_User.DataSource == null || dgv_User.CurrentRow == null)
                {
                    return;
                }
                else
                {
                    if (dgv_User.SelectedRows.Count > 0)
                    {
                        Logger.LogMessage("管理员给选中的用户发送信息", "管理员已经选中了多个用户");
                        string strMsg = txtBox_SendMsg.Text.Trim(); 
                        foreach (DataGridViewRow dgvrow in dgv_User.SelectedRows)
                        {
                            string strUserID = dgvrow.Cells[0].Value.ToString().Trim();
                            string strSql = "update MyCookUser set UserMsg=@msg where UserID=@userid";
                            SqlParameter[] paras ={
                                                  new SqlParameter("@msg",strMsg),
                                                  new SqlParameter("@userid",strUserID)
                                              };
                            SqlHelper.MyHandleUpdateSql(strSql, paras);
                        }
                        MessageBox.Show("已发送给选中的用户");
                        Logger.LogMessage("管理员给选中的用户发送消息", "数据库更新成功，发送完毕");
                        UpdataDGV_User();
                    }
                    else
                    {
                        MessageBox.Show("你还没有选择用户");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErr("", ex);
                Logger.LogMessage("管理员给选中的用户发送信息", "SQL语句执行报错，发送失败");
                Logger.LogException(ex);
            }
        }


    }
}
