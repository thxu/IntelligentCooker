namespace IntelligentCooker
{
    partial class formMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabCtrl_Server = new System.Windows.Forms.TabControl();
            this.tab_Server = new System.Windows.Forms.TabPage();
            this.panel8 = new System.Windows.Forms.Panel();
            this.txtShow = new System.Windows.Forms.TextBox();
            this.tab_UserManage = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtbox_UserName = new System.Windows.Forms.TextBox();
            this.btn_SerchUser = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox_Manage = new System.Windows.Forms.GroupBox();
            this.lab_UserName = new System.Windows.Forms.Label();
            this.lab_UserID = new System.Windows.Forms.Label();
            this.txtBox_Name = new System.Windows.Forms.TextBox();
            this.btn_SendAll = new System.Windows.Forms.Button();
            this.lab_Pwd = new System.Windows.Forms.Label();
            this.txtBox_SendMsg = new System.Windows.Forms.TextBox();
            this.txtBox_Pwd = new System.Windows.Forms.TextBox();
            this.btn_Del = new System.Windows.Forms.Button();
            this.lab_UserMsg = new System.Windows.Forms.Label();
            this.btn_Change = new System.Windows.Forms.Button();
            this.txtBox_UserMsg = new System.Windows.Forms.TextBox();
            this.btn_Insert = new System.Windows.Forms.Button();
            this.tab_DeviceManage = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtBox_SerchDeviceID = new System.Windows.Forms.TextBox();
            this.btn_SerchDevice = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lab_User = new System.Windows.Forms.Label();
            this.btn_DeviceChange = new System.Windows.Forms.Button();
            this.btn_DeviceDel = new System.Windows.Forms.Button();
            this.txtBox_User = new System.Windows.Forms.TextBox();
            this.txtBox_FireFlg = new System.Windows.Forms.TextBox();
            this.lab_FireFlg = new System.Windows.Forms.Label();
            this.txtBox_IsDel = new System.Windows.Forms.TextBox();
            this.lab_IsDel = new System.Windows.Forms.Label();
            this.txtBox_DevicePwd = new System.Windows.Forms.TextBox();
            this.lab_DevicePwd = new System.Windows.Forms.Label();
            this.txtBox_DeviceID = new System.Windows.Forms.TextBox();
            this.lab_DeviceID = new System.Windows.Forms.Label();
            this.dgv_User = new System.Windows.Forms.DataGridView();
            this.dgv_Device = new System.Windows.Forms.DataGridView();
            this.tabCtrl_Server.SuspendLayout();
            this.tab_Server.SuspendLayout();
            this.panel8.SuspendLayout();
            this.tab_UserManage.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox_Manage.SuspendLayout();
            this.tab_DeviceManage.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_User)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Device)).BeginInit();
            this.SuspendLayout();
            // 
            // tabCtrl_Server
            // 
            this.tabCtrl_Server.Controls.Add(this.tab_Server);
            this.tabCtrl_Server.Controls.Add(this.tab_UserManage);
            this.tabCtrl_Server.Controls.Add(this.tab_DeviceManage);
            this.tabCtrl_Server.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrl_Server.Location = new System.Drawing.Point(0, 0);
            this.tabCtrl_Server.Name = "tabCtrl_Server";
            this.tabCtrl_Server.SelectedIndex = 0;
            this.tabCtrl_Server.Size = new System.Drawing.Size(953, 631);
            this.tabCtrl_Server.TabIndex = 0;
            // 
            // tab_Server
            // 
            this.tab_Server.Controls.Add(this.panel8);
            this.tab_Server.Location = new System.Drawing.Point(4, 21);
            this.tab_Server.Name = "tab_Server";
            this.tab_Server.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Server.Size = new System.Drawing.Size(945, 606);
            this.tab_Server.TabIndex = 0;
            this.tab_Server.Text = "通信服务";
            this.tab_Server.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.txtShow);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(3, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(939, 600);
            this.panel8.TabIndex = 7;
            // 
            // txtShow
            // 
            this.txtShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtShow.Location = new System.Drawing.Point(0, 0);
            this.txtShow.Multiline = true;
            this.txtShow.Name = "txtShow";
            this.txtShow.Size = new System.Drawing.Size(939, 600);
            this.txtShow.TabIndex = 6;
            // 
            // tab_UserManage
            // 
            this.tab_UserManage.Controls.Add(this.panel2);
            this.tab_UserManage.Controls.Add(this.panel1);
            this.tab_UserManage.Location = new System.Drawing.Point(4, 21);
            this.tab_UserManage.Name = "tab_UserManage";
            this.tab_UserManage.Padding = new System.Windows.Forms.Padding(3);
            this.tab_UserManage.Size = new System.Drawing.Size(945, 606);
            this.tab_UserManage.TabIndex = 1;
            this.tab_UserManage.Text = "管理用户";
            this.tab_UserManage.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgv_User);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(939, 332);
            this.panel2.TabIndex = 20;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtbox_UserName);
            this.panel3.Controls.Add(this.btn_SerchUser);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(939, 45);
            this.panel3.TabIndex = 20;
            // 
            // txtbox_UserName
            // 
            this.txtbox_UserName.Location = new System.Drawing.Point(19, 12);
            this.txtbox_UserName.Name = "txtbox_UserName";
            this.txtbox_UserName.Size = new System.Drawing.Size(140, 21);
            this.txtbox_UserName.TabIndex = 23;
            this.txtbox_UserName.Text = "请在此处输入用户名";
            this.txtbox_UserName.Click += new System.EventHandler(this.txtbox_UserName_Click);
            // 
            // btn_SerchUser
            // 
            this.btn_SerchUser.Location = new System.Drawing.Point(176, 10);
            this.btn_SerchUser.Name = "btn_SerchUser";
            this.btn_SerchUser.Size = new System.Drawing.Size(75, 23);
            this.btn_SerchUser.TabIndex = 22;
            this.btn_SerchUser.Text = "搜索用户";
            this.btn_SerchUser.UseVisualStyleBackColor = true;
            this.btn_SerchUser.Click += new System.EventHandler(this.btn_SerchUser_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox_Manage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 335);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(939, 268);
            this.panel1.TabIndex = 19;
            // 
            // groupBox_Manage
            // 
            this.groupBox_Manage.Controls.Add(this.lab_UserName);
            this.groupBox_Manage.Controls.Add(this.lab_UserID);
            this.groupBox_Manage.Controls.Add(this.txtBox_Name);
            this.groupBox_Manage.Controls.Add(this.btn_SendAll);
            this.groupBox_Manage.Controls.Add(this.lab_Pwd);
            this.groupBox_Manage.Controls.Add(this.txtBox_SendMsg);
            this.groupBox_Manage.Controls.Add(this.txtBox_Pwd);
            this.groupBox_Manage.Controls.Add(this.btn_Del);
            this.groupBox_Manage.Controls.Add(this.lab_UserMsg);
            this.groupBox_Manage.Controls.Add(this.btn_Change);
            this.groupBox_Manage.Controls.Add(this.txtBox_UserMsg);
            this.groupBox_Manage.Controls.Add(this.btn_Insert);
            this.groupBox_Manage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Manage.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Manage.Name = "groupBox_Manage";
            this.groupBox_Manage.Size = new System.Drawing.Size(939, 268);
            this.groupBox_Manage.TabIndex = 20;
            this.groupBox_Manage.TabStop = false;
            this.groupBox_Manage.Text = "管理员操作";
            // 
            // lab_UserName
            // 
            this.lab_UserName.AutoSize = true;
            this.lab_UserName.Location = new System.Drawing.Point(42, 32);
            this.lab_UserName.Name = "lab_UserName";
            this.lab_UserName.Size = new System.Drawing.Size(53, 12);
            this.lab_UserName.TabIndex = 3;
            this.lab_UserName.Text = "用户名：";
            // 
            // lab_UserID
            // 
            this.lab_UserID.AutoSize = true;
            this.lab_UserID.Location = new System.Drawing.Point(8, 32);
            this.lab_UserID.Name = "lab_UserID";
            this.lab_UserID.Size = new System.Drawing.Size(0, 12);
            this.lab_UserID.TabIndex = 14;
            // 
            // txtBox_Name
            // 
            this.txtBox_Name.Location = new System.Drawing.Point(101, 29);
            this.txtBox_Name.Name = "txtBox_Name";
            this.txtBox_Name.Size = new System.Drawing.Size(100, 21);
            this.txtBox_Name.TabIndex = 4;
            // 
            // btn_SendAll
            // 
            this.btn_SendAll.Location = new System.Drawing.Point(745, 122);
            this.btn_SendAll.Name = "btn_SendAll";
            this.btn_SendAll.Size = new System.Drawing.Size(75, 23);
            this.btn_SendAll.TabIndex = 13;
            this.btn_SendAll.Text = "发给所有人";
            this.btn_SendAll.UseVisualStyleBackColor = true;
            this.btn_SendAll.Click += new System.EventHandler(this.btn_SendAll_Click);
            // 
            // lab_Pwd
            // 
            this.lab_Pwd.AutoSize = true;
            this.lab_Pwd.Location = new System.Drawing.Point(217, 32);
            this.lab_Pwd.Name = "lab_Pwd";
            this.lab_Pwd.Size = new System.Drawing.Size(41, 12);
            this.lab_Pwd.TabIndex = 5;
            this.lab_Pwd.Text = "密码：";
            // 
            // txtBox_SendMsg
            // 
            this.txtBox_SendMsg.Location = new System.Drawing.Point(471, 66);
            this.txtBox_SendMsg.Multiline = true;
            this.txtBox_SendMsg.Name = "txtBox_SendMsg";
            this.txtBox_SendMsg.Size = new System.Drawing.Size(268, 107);
            this.txtBox_SendMsg.TabIndex = 12;
            this.txtBox_SendMsg.Text = "请在此处写下发给所有用户的信息";
            this.txtBox_SendMsg.Click += new System.EventHandler(this.txtBox_SendMsg_Click);
            // 
            // txtBox_Pwd
            // 
            this.txtBox_Pwd.Location = new System.Drawing.Point(264, 29);
            this.txtBox_Pwd.Name = "txtBox_Pwd";
            this.txtBox_Pwd.Size = new System.Drawing.Size(100, 21);
            this.txtBox_Pwd.TabIndex = 6;
            // 
            // btn_Del
            // 
            this.btn_Del.Location = new System.Drawing.Point(356, 151);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(75, 23);
            this.btn_Del.TabIndex = 11;
            this.btn_Del.Text = "删除";
            this.btn_Del.UseVisualStyleBackColor = true;
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // lab_UserMsg
            // 
            this.lab_UserMsg.AutoSize = true;
            this.lab_UserMsg.Location = new System.Drawing.Point(30, 69);
            this.lab_UserMsg.Name = "lab_UserMsg";
            this.lab_UserMsg.Size = new System.Drawing.Size(65, 12);
            this.lab_UserMsg.TabIndex = 7;
            this.lab_UserMsg.Text = "用户消息：";
            // 
            // btn_Change
            // 
            this.btn_Change.Location = new System.Drawing.Point(356, 122);
            this.btn_Change.Name = "btn_Change";
            this.btn_Change.Size = new System.Drawing.Size(75, 23);
            this.btn_Change.TabIndex = 10;
            this.btn_Change.Text = "修改";
            this.btn_Change.UseVisualStyleBackColor = true;
            this.btn_Change.Click += new System.EventHandler(this.btn_Change_Click);
            // 
            // txtBox_UserMsg
            // 
            this.txtBox_UserMsg.Location = new System.Drawing.Point(102, 69);
            this.txtBox_UserMsg.Multiline = true;
            this.txtBox_UserMsg.Name = "txtBox_UserMsg";
            this.txtBox_UserMsg.Size = new System.Drawing.Size(248, 107);
            this.txtBox_UserMsg.TabIndex = 8;
            // 
            // btn_Insert
            // 
            this.btn_Insert.Location = new System.Drawing.Point(356, 93);
            this.btn_Insert.Name = "btn_Insert";
            this.btn_Insert.Size = new System.Drawing.Size(75, 23);
            this.btn_Insert.TabIndex = 9;
            this.btn_Insert.Text = "新增";
            this.btn_Insert.UseVisualStyleBackColor = true;
            this.btn_Insert.Click += new System.EventHandler(this.btn_Insert_Click);
            // 
            // tab_DeviceManage
            // 
            this.tab_DeviceManage.Controls.Add(this.panel5);
            this.tab_DeviceManage.Controls.Add(this.panel4);
            this.tab_DeviceManage.Location = new System.Drawing.Point(4, 21);
            this.tab_DeviceManage.Name = "tab_DeviceManage";
            this.tab_DeviceManage.Padding = new System.Windows.Forms.Padding(3);
            this.tab_DeviceManage.Size = new System.Drawing.Size(945, 606);
            this.tab_DeviceManage.TabIndex = 2;
            this.tab_DeviceManage.Text = "管理设备";
            this.tab_DeviceManage.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgv_Device);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(939, 407);
            this.panel5.TabIndex = 21;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtBox_SerchDeviceID);
            this.panel6.Controls.Add(this.btn_SerchDevice);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(939, 54);
            this.panel6.TabIndex = 23;
            // 
            // txtBox_SerchDeviceID
            // 
            this.txtBox_SerchDeviceID.Location = new System.Drawing.Point(9, 17);
            this.txtBox_SerchDeviceID.Name = "txtBox_SerchDeviceID";
            this.txtBox_SerchDeviceID.Size = new System.Drawing.Size(185, 21);
            this.txtBox_SerchDeviceID.TabIndex = 23;
            this.txtBox_SerchDeviceID.Text = "请输入要搜索的设备号";
            this.txtBox_SerchDeviceID.Click += new System.EventHandler(this.txtBox_SerchDeviceID_Click);
            // 
            // btn_SerchDevice
            // 
            this.btn_SerchDevice.Location = new System.Drawing.Point(211, 17);
            this.btn_SerchDevice.Name = "btn_SerchDevice";
            this.btn_SerchDevice.Size = new System.Drawing.Size(75, 23);
            this.btn_SerchDevice.TabIndex = 22;
            this.btn_SerchDevice.Text = "搜索设备";
            this.btn_SerchDevice.UseVisualStyleBackColor = true;
            this.btn_SerchDevice.Click += new System.EventHandler(this.btn_SerchDevice_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(3, 410);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(939, 193);
            this.panel4.TabIndex = 20;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lab_User);
            this.groupBox1.Controls.Add(this.btn_DeviceChange);
            this.groupBox1.Controls.Add(this.btn_DeviceDel);
            this.groupBox1.Controls.Add(this.txtBox_User);
            this.groupBox1.Controls.Add(this.txtBox_FireFlg);
            this.groupBox1.Controls.Add(this.lab_FireFlg);
            this.groupBox1.Controls.Add(this.txtBox_IsDel);
            this.groupBox1.Controls.Add(this.lab_IsDel);
            this.groupBox1.Controls.Add(this.txtBox_DevicePwd);
            this.groupBox1.Controls.Add(this.lab_DevicePwd);
            this.groupBox1.Controls.Add(this.txtBox_DeviceID);
            this.groupBox1.Controls.Add(this.lab_DeviceID);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(939, 193);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "管理员操作";
            // 
            // lab_User
            // 
            this.lab_User.AutoSize = true;
            this.lab_User.Location = new System.Drawing.Point(16, 32);
            this.lab_User.Name = "lab_User";
            this.lab_User.Size = new System.Drawing.Size(41, 12);
            this.lab_User.TabIndex = 8;
            this.lab_User.Text = "用户：";
            // 
            // btn_DeviceChange
            // 
            this.btn_DeviceChange.Location = new System.Drawing.Point(18, 69);
            this.btn_DeviceChange.Name = "btn_DeviceChange";
            this.btn_DeviceChange.Size = new System.Drawing.Size(75, 23);
            this.btn_DeviceChange.TabIndex = 15;
            this.btn_DeviceChange.Text = "修改";
            this.btn_DeviceChange.UseVisualStyleBackColor = true;
            this.btn_DeviceChange.Click += new System.EventHandler(this.btn_DeviceChange_Click);
            // 
            // btn_DeviceDel
            // 
            this.btn_DeviceDel.Location = new System.Drawing.Point(171, 69);
            this.btn_DeviceDel.Name = "btn_DeviceDel";
            this.btn_DeviceDel.Size = new System.Drawing.Size(75, 23);
            this.btn_DeviceDel.TabIndex = 14;
            this.btn_DeviceDel.Text = "删除";
            this.btn_DeviceDel.UseVisualStyleBackColor = true;
            this.btn_DeviceDel.Click += new System.EventHandler(this.btn_DeviceDel_Click);
            // 
            // txtBox_User
            // 
            this.txtBox_User.Location = new System.Drawing.Point(63, 29);
            this.txtBox_User.Name = "txtBox_User";
            this.txtBox_User.Size = new System.Drawing.Size(100, 21);
            this.txtBox_User.TabIndex = 3;
            // 
            // txtBox_FireFlg
            // 
            this.txtBox_FireFlg.Location = new System.Drawing.Point(780, 29);
            this.txtBox_FireFlg.Name = "txtBox_FireFlg";
            this.txtBox_FireFlg.Size = new System.Drawing.Size(100, 21);
            this.txtBox_FireFlg.TabIndex = 4;
            // 
            // lab_FireFlg
            // 
            this.lab_FireFlg.AutoSize = true;
            this.lab_FireFlg.Location = new System.Drawing.Point(697, 32);
            this.lab_FireFlg.Name = "lab_FireFlg";
            this.lab_FireFlg.Size = new System.Drawing.Size(77, 12);
            this.lab_FireFlg.TabIndex = 12;
            this.lab_FireFlg.Text = "是否有火灾：";
            // 
            // txtBox_IsDel
            // 
            this.txtBox_IsDel.Location = new System.Drawing.Point(591, 29);
            this.txtBox_IsDel.Name = "txtBox_IsDel";
            this.txtBox_IsDel.Size = new System.Drawing.Size(100, 21);
            this.txtBox_IsDel.TabIndex = 5;
            // 
            // lab_IsDel
            // 
            this.lab_IsDel.AutoSize = true;
            this.lab_IsDel.Location = new System.Drawing.Point(508, 32);
            this.lab_IsDel.Name = "lab_IsDel";
            this.lab_IsDel.Size = new System.Drawing.Size(77, 12);
            this.lab_IsDel.TabIndex = 11;
            this.lab_IsDel.Text = "是否已删除：";
            // 
            // txtBox_DevicePwd
            // 
            this.txtBox_DevicePwd.Location = new System.Drawing.Point(402, 29);
            this.txtBox_DevicePwd.Name = "txtBox_DevicePwd";
            this.txtBox_DevicePwd.Size = new System.Drawing.Size(100, 21);
            this.txtBox_DevicePwd.TabIndex = 6;
            // 
            // lab_DevicePwd
            // 
            this.lab_DevicePwd.AutoSize = true;
            this.lab_DevicePwd.Location = new System.Drawing.Point(334, 32);
            this.lab_DevicePwd.Name = "lab_DevicePwd";
            this.lab_DevicePwd.Size = new System.Drawing.Size(65, 12);
            this.lab_DevicePwd.TabIndex = 10;
            this.lab_DevicePwd.Text = "设备密码：";
            // 
            // txtBox_DeviceID
            // 
            this.txtBox_DeviceID.Location = new System.Drawing.Point(228, 29);
            this.txtBox_DeviceID.Name = "txtBox_DeviceID";
            this.txtBox_DeviceID.ReadOnly = true;
            this.txtBox_DeviceID.Size = new System.Drawing.Size(100, 21);
            this.txtBox_DeviceID.TabIndex = 7;
            // 
            // lab_DeviceID
            // 
            this.lab_DeviceID.AutoSize = true;
            this.lab_DeviceID.Location = new System.Drawing.Point(169, 32);
            this.lab_DeviceID.Name = "lab_DeviceID";
            this.lab_DeviceID.Size = new System.Drawing.Size(53, 12);
            this.lab_DeviceID.TabIndex = 9;
            this.lab_DeviceID.Text = "设备号：";
            // 
            // dgv_User
            // 
            this.dgv_User.AllowUserToAddRows = false;
            this.dgv_User.AllowUserToDeleteRows = false;
            this.dgv_User.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_User.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_User.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_User.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_User.Location = new System.Drawing.Point(0, 45);
            this.dgv_User.Name = "dgv_User";
            this.dgv_User.ReadOnly = true;
            this.dgv_User.RowTemplate.Height = 23;
            this.dgv_User.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_User.Size = new System.Drawing.Size(939, 287);
            this.dgv_User.TabIndex = 21;
            this.dgv_User.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_User_CellClick);
            // 
            // dgv_Device
            // 
            this.dgv_Device.AllowUserToAddRows = false;
            this.dgv_Device.AllowUserToDeleteRows = false;
            this.dgv_Device.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Device.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_Device.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Device.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Device.GridColor = System.Drawing.SystemColors.Control;
            this.dgv_Device.Location = new System.Drawing.Point(0, 54);
            this.dgv_Device.Name = "dgv_Device";
            this.dgv_Device.ReadOnly = true;
            this.dgv_Device.RowTemplate.Height = 23;
            this.dgv_Device.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Device.Size = new System.Drawing.Size(939, 353);
            this.dgv_Device.TabIndex = 24;
            this.dgv_Device.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Device_CellClick);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 631);
            this.Controls.Add(this.tabCtrl_Server);
            this.Name = "formMain";
            this.Text = "智能电饭锅服务器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabCtrl_Server.ResumeLayout(false);
            this.tab_Server.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.tab_UserManage.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox_Manage.ResumeLayout(false);
            this.groupBox_Manage.PerformLayout();
            this.tab_DeviceManage.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_User)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Device)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCtrl_Server;
        private System.Windows.Forms.TabPage tab_Server;
        private System.Windows.Forms.TabPage tab_UserManage;
        private System.Windows.Forms.TabPage tab_DeviceManage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox_Manage;
        private System.Windows.Forms.Label lab_UserName;
        private System.Windows.Forms.Label lab_UserID;
        private System.Windows.Forms.TextBox txtBox_Name;
        private System.Windows.Forms.Button btn_SendAll;
        private System.Windows.Forms.Label lab_Pwd;
        private System.Windows.Forms.TextBox txtBox_SendMsg;
        private System.Windows.Forms.TextBox txtBox_Pwd;
        private System.Windows.Forms.Button btn_Del;
        private System.Windows.Forms.Label lab_UserMsg;
        private System.Windows.Forms.Button btn_Change;
        private System.Windows.Forms.TextBox txtBox_UserMsg;
        private System.Windows.Forms.Button btn_Insert;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtbox_UserName;
        private System.Windows.Forms.Button btn_SerchUser;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lab_User;
        private System.Windows.Forms.Button btn_DeviceChange;
        private System.Windows.Forms.Button btn_DeviceDel;
        private System.Windows.Forms.TextBox txtBox_User;
        private System.Windows.Forms.TextBox txtBox_FireFlg;
        private System.Windows.Forms.Label lab_FireFlg;
        private System.Windows.Forms.TextBox txtBox_IsDel;
        private System.Windows.Forms.Label lab_IsDel;
        private System.Windows.Forms.TextBox txtBox_DevicePwd;
        private System.Windows.Forms.Label lab_DevicePwd;
        private System.Windows.Forms.TextBox txtBox_DeviceID;
        private System.Windows.Forms.Label lab_DeviceID;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox txtBox_SerchDeviceID;
        private System.Windows.Forms.Button btn_SerchDevice;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TextBox txtShow;
        private System.Windows.Forms.DataGridView dgv_User;
        private System.Windows.Forms.DataGridView dgv_Device;
    }
}

