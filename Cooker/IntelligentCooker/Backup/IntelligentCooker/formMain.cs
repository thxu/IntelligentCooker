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
                    //txtShow.AppendText("have Connected \r\n");
                    Thread myth = new Thread(HandleDatas);
                    myth.IsBackground = true;
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
                    byte[] byteReceiveMsg = new byte[1024];
                    int L = sokConnection.Receive(byteReceiveMsg, byteReceiveMsg.Length, 0);
                    if (L > 0)
                    {
                        string strReceiveMsg = Encoding.ASCII.GetString(byteReceiveMsg);
                        txtShow.AppendText("SrverReceive:" + strReceiveMsg + "\r\n");
                        string strEffectiveData = GetEffectiveData(strReceiveMsg);
                        if (strEffectiveData == "invalid")
                        {
                            sokConnection.Close();
                            return;
                        }
                        string[] strdata = strEffectiveData.Split('+');
                        switch (strdata[0])
                        {
                            case "A":
                                {
                                    if (strdata[1] == "R")
                                    {
                                        RegistUser(strdata[2], strdata[3], ref sokConnection);
                                    }
                                    else if (strdata[1] == "L")
                                    {
                                        Login(strdata[2], strdata[3], ref sokConnection);
                                    }
                                    else if (strdata[1] == "A")
                                    {
                                        AddDevice(strdata[2], strdata[3], strdata[4], ref sokConnection);
                                    }
                                    else if (strdata[1] == "D")
                                    {
                                        DelDevice(strdata[2], strdata[3], strdata[4], ref sokConnection);
                                    }
                                    else if (strdata[1] == "S")
                                    {
                                        SaveData(strdata[2], strdata[3], strdata[4], ref sokConnection);
                                    }
                                    else if (strdata[1] == "N")
                                    {
                                        ;
                                    }
                                    break;
                                }
                            case "B":
                                {
                                    SendtoDevice(strdata[1], strdata[2], ref sokConnection);
                                    break;
                                }
                            default:
                                sokConnection.Close();
                                break;
                        }
                    }
                    else
                    {
                        sokConnection.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ShowErr("", ex);
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
                string strSql = "select * from MyCookData where DeviceID=@deviceid";
                SqlParameter para = new SqlParameter("@deviceid", deviceid);
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, para);
                string strTime = GetTime();
                string strSendInfo = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["TmpdataFlg"]) == 1)
                        {
                            strSendInfo = "@BC" + strTime + dr["TmpData"].ToString() + "#";
                        }
                        else
                        {
                            strSendInfo = "@BA" + strTime + dr["RepateData"].ToString() + "#";
                        }
                        SendMsg(strSendInfo, ref mysok);
                        mysok.Close();
                        break;
                    }
                }
                else
                {
                    string strSql1 = "insert into MyCookData(DeviceID) values(@deviceid)";
                    SqlParameter para1 = new SqlParameter("@deviceid", deviceid);
                    int res = -1;
                    res = SqlHelper.MyHandleUpdateSql(strSql1, para1);
                    if (res > 0)
                    {
                        SendMsg("@BB" + strTime + "#", ref mysok);
                    }
                    mysok.Close();
                }
                string strSql2 = string.Empty;
                if (fireflg == "A")
                {
                    strSql2 = "update MyCookDevice set FireFlg=1 where DeviceID=@deviceid";
                }
                else
                {
                    strSql2 = "update MyCookDevice set FireFlg=0 where DeviceID=@deviceid";
                }
                SqlParameter[] paras2 = { 
                                                new SqlParameter("@deviceid",deviceid)
                                            };
                int res1 = -1;
                res1 = SqlHelper.MyHandleUpdateSql(strSql2, paras2);

            }
            catch (Exception ex)
            {
                ShowErr("", ex);
                mysok.Close();
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
        public void SaveData(string deviceid, string datatype, string cookdata, ref Socket mysok)
        {
            try
            {
                string strSql = "";
                if (datatype == "A")   /*判定用户是临时设置还是重复设置*/
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
                int res = -1;
                res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                if (res > 0)
                {
                    SendMsg("``````OK````", ref mysok);
                }
                else
                {
                    SendMsg("````NO````", ref mysok);
                }
                mysok.Close();
            }
            catch (Exception ex)
            {
                ShowErr("", ex);
                SendMsg("````NO````", ref mysok);
                mysok.Close();
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
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, para);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["DevicePwd"].ToString().Trim() == devicepwd)  /*判定用户输入的设备密码是否正确*/
                    {
                        string strSql1 = "update MyCookDevice set UserName='' where DeviceID=@deviceid";
                        SqlParameter paras = new SqlParameter("@deviceid", deveiceid);
                        int res = -1;
                        res = SqlHelper.MyHandleUpdateSql(strSql1, paras); /*用户绑定设备*/
                        if (res > 0)
                        {
                            txtShow.AppendText("删除成功\r\n");
                            SendMsg("```````ok`````", ref mysok);

                        }
                        else
                        {
                            SendMsg("```````NO`````", ref mysok);
                        }
                    }
                    else
                    {
                        SendMsg("````111NO111````", ref mysok);
                    }
                    mysok.Close();
                    break;
                }
            }
            catch (Exception ex)
            {
                ShowErr("", ex);
                SendMsg("删除失败", ref mysok);
                mysok.Close();
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
                string strSql = "select DevicePwd from MyCookDevice where DeviceID=@deviceid";
                SqlParameter para = new SqlParameter("@deviceid", deveiceid);
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, para);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["DevicePwd"].ToString().Trim() == devicepwd)  /*判定用户输入的设备密码是否正确*/
                    {
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
                            SendMsg("```````ok`````", ref mysok);

                        }
                        else
                        {
                            SendMsg("```````NO`````", ref mysok);
                        }
                    }
                    else
                    {
                        SendMsg("````111NO111````", ref mysok);
                    }
                    mysok.Close();
                    break;
                }
            }
            catch (Exception ex)
            {
                ShowErr("", ex);
                SendMsg("绑定失败", ref mysok);
                mysok.Close();
                Logger.LogException(ex);
            }
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="mysok"></param>
        public void Login(string name, string pwd, ref Socket mysok)
        {
            try
            {
                string strSql = "select UserPwd from MyCookUser where UserName=@name";
                SqlParameter paras = new SqlParameter("@name", name);
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, paras);
                foreach (DataRow dr in dt.Rows)
                {
                    // txtShow.AppendText(dr["UserPwd"].ToString());
                    if (dr["UserPwd"].ToString().Trim() == pwd)   /*判断用户的密码是否正确*/
                    {
                        string strSql1 = "select DeviceID,IsDel from MyCookDevice where Username=@name";
                        SqlParameter paras1 = new SqlParameter("@name", name);
                        DataTable dt1 = SqlHelper.MyHandleSelectSql(strSql1, paras1);
                        string SendInfo = string.Empty;
                        foreach (DataRow dr1 in dt1.Rows)   /*找到该用户的所有设备*/
                        {
                            if (dr1["IsDel"].ToString().Trim() == "0")
                            {
                                SendInfo = SendInfo + dr1["DeviceID"].ToString().Trim() + "+";
                            }

                        }
                        if (SendInfo == string.Empty)
                        {
                            SendMsg("````NOdeviceID````", ref mysok);
                        }
                        else
                        {
                            SendMsg(SendInfo, ref mysok);
                        }
                    }
                    else
                    {
                        SendMsg("```````Log_NO``````", ref mysok);
                    }
                    mysok.Close();
                    break;
                }
            }
            catch (System.Exception ex)
            {
                ShowErr("", ex);
                Logger.LogException(ex);
            }
        }
        /// <summary>
        /// 用户注册
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
                int res = -1;
                res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                if (res > 0)
                {
                    txtShow.AppendText("注册成功");
                    SendMsg("```````OK`````````", ref mysok);
                    mysok.Close();
                }
            }
            catch (System.Exception ex)
            {
                ShowErr("", ex);
                SendMsg("```````NO```````````", ref mysok);
                mysok.Close();
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
                        txtShow.AppendText("Socket Error cannot Send Packet\r\n");
                    }
                    else
                    {
                        txtShow.AppendText("bytes send\r\n");
                    }
                }
                else
                {
                    txtShow.AppendText("连接失败....\r\n");
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
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, para);
                dgv_User.DataSource = dt;
            }
            catch (Exception ex)
            {
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
                int res = -1;
                try
                {
                    res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                    if (res > 0)
                    {
                        MessageBox.Show("插入用户成功");
                        UpdataDGV_User();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("用户名重复");
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
                int res = -1;
                try
                {
                    res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("用户名重复");
                }
                if (res > 0)
                {
                    MessageBox.Show("更新用户成功");
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
                int res = -1;
                res = SqlHelper.MyHandleUpdateSql(strSql, para);
                if (res > 0)
                {
                    MessageBox.Show("删除成功");
                    UpdataDGV_User();
                }
            }
            catch (System.Exception ex)
            {
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
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql1);
                foreach (DataRow dr in dt.Rows)
                {
                    SqlParameter[] para = 
                {   
                    new SqlParameter("@msg",strMsg),
                    new SqlParameter("@userid", dr["UserID"].ToString().Trim())
                };
                    SqlHelper.MyHandleUpdateSql(strSql, para);
                    UpdataDGV_User();
                }
            }
            catch (System.Exception ex)
            {
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
                DataTable dt = SqlHelper.MyHandleSelectSql(strSql, para);
                dgv_Device.DataSource = dt;
            }
            catch (System.Exception ex)
            {
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
                    string strUser = dgv_Device.Rows[rowIndex].Cells[0].Value.ToString().Trim();
                    string strDeviceID = dgv_Device.Rows[rowIndex].Cells[1].Value.ToString().Trim();
                    string strDevicePwd = dgv_Device.Rows[rowIndex].Cells[2].Value.ToString().Trim();
                    string strIsDel = dgv_Device.Rows[rowIndex].Cells[3].Value.ToString().Trim();
                    string strFireFlg = dgv_Device.Rows[rowIndex].Cells[4].Value.ToString().Trim();
                    txtBox_User.Text = strUser;
                    txtBox_DeviceID.Text = strDeviceID;
                    txtBox_DevicePwd.Text = strDevicePwd;
                    txtBox_IsDel.Text = strIsDel;
                    txtBox_FireFlg.Text = strFireFlg;
                }
            }
            catch (System.Exception ex)
            {
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
                string strSql = "update MyCookDevice set UserName=@username,DevicePwd=@devicepwd,IsDel=@isdel,FireFlg=@fireflg where DeviceID=@deviceid";
                SqlParameter[] paras = { 
                                   new SqlParameter("@username",strUser),
                                   new SqlParameter("@deviceid",strDeviceID),
                                   new SqlParameter("@devicepwd",strDevicePwd),
                                   new SqlParameter("@isdel",strIsDel),
                                   new SqlParameter("@fireflg",strFireFlg)
                                   };
                int res = -1;
                res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                if (res > 0)
                {
                    MessageBox.Show("修改成功");
                    UpdataDGV_Device();
                }
            }
            catch (System.Exception ex)
            {
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
                int res = -1;
                res = SqlHelper.MyHandleUpdateSql(strSql, paras);
                if (res > 0)
                {
                    MessageBox.Show("删除成功");
                    UpdataDGV_Device();
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                InitListen();
                UpdataDGV_Device();
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
                }

                UpdataDGV_User();
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
    }
}
