
namespace 摸鱼
{
    partial class Main
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.txtKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lProcess = new System.Windows.Forms.ListBox();
            this.lblLocalIp = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lFriends = new System.Windows.Forms.ListBox();
            this.nicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cbFirstKey = new System.Windows.Forms.ComboBox();
            this.RefProcess = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.btnConnectionServer = new System.Windows.Forms.Button();
            this.txtLocalPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServerIp = new System.Windows.Forms.TextBox();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(172, 410);
            this.txtKey.Name = "txtKey";
            this.txtKey.ReadOnly = true;
            this.txtKey.Size = new System.Drawing.Size(275, 21);
            this.txtKey.TabIndex = 0;
            this.txtKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKey_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 415);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "关闭快捷键";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(372, 455);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 4;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lProcess
            // 
            this.lProcess.FormattingEnabled = true;
            this.lProcess.ItemHeight = 12;
            this.lProcess.Location = new System.Drawing.Point(12, 84);
            this.lProcess.Name = "lProcess";
            this.lProcess.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lProcess.Size = new System.Drawing.Size(205, 304);
            this.lProcess.TabIndex = 5;
            // 
            // lblLocalIp
            // 
            this.lblLocalIp.AutoSize = true;
            this.lblLocalIp.Location = new System.Drawing.Point(18, 455);
            this.lblLocalIp.Name = "lblLocalIp";
            this.lblLocalIp.Size = new System.Drawing.Size(71, 12);
            this.lblLocalIp.TabIndex = 7;
            this.lblLocalIp.Text = "192.168.1.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "网络名称";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(90, 13);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(70, 21);
            this.txtName.TabIndex = 8;
            this.txtName.Text = "xiaoer";
            // 
            // lFriends
            // 
            this.lFriends.FormattingEnabled = true;
            this.lFriends.ItemHeight = 12;
            this.lFriends.Location = new System.Drawing.Point(242, 84);
            this.lFriends.Name = "lFriends";
            this.lFriends.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lFriends.Size = new System.Drawing.Size(205, 304);
            this.lFriends.TabIndex = 10;
            // 
            // nicon
            // 
            this.nicon.Icon = ((System.Drawing.Icon)(resources.GetObject("nicon.Icon")));
            this.nicon.Text = "notifyIcon1";
            this.nicon.Visible = true;
            this.nicon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.nicon_MouseClick);
            // 
            // cbFirstKey
            // 
            this.cbFirstKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFirstKey.FormattingEnabled = true;
            this.cbFirstKey.Items.AddRange(new object[] {
            "无组合键",
            "Alt",
            "Ctrl",
            "Win",
            "Shift"});
            this.cbFirstKey.Location = new System.Drawing.Point(88, 410);
            this.cbFirstKey.Name = "cbFirstKey";
            this.cbFirstKey.Size = new System.Drawing.Size(78, 20);
            this.cbFirstKey.TabIndex = 11;
            this.cbFirstKey.SelectedIndexChanged += new System.EventHandler(this.cbFirstKey_SelectedIndexChanged);
            // 
            // RefProcess
            // 
            this.RefProcess.Location = new System.Drawing.Point(263, 455);
            this.RefProcess.Name = "RefProcess";
            this.RefProcess.Size = new System.Drawing.Size(98, 23);
            this.RefProcess.TabIndex = 12;
            this.RefProcess.Text = "刷新应用程序";
            this.RefProcess.UseVisualStyleBackColor = true;
            this.RefProcess.Click += new System.EventHandler(this.RefProcess_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(154, 455);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(98, 23);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "完全退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(215, 45);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(114, 23);
            this.btnStartServer.TabIndex = 14;
            this.btnStartServer.Text = "开启本机端口服务";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // btnConnectionServer
            // 
            this.btnConnectionServer.Location = new System.Drawing.Point(348, 45);
            this.btnConnectionServer.Name = "btnConnectionServer";
            this.btnConnectionServer.Size = new System.Drawing.Size(98, 23);
            this.btnConnectionServer.TabIndex = 15;
            this.btnConnectionServer.Text = "加入控制";
            this.btnConnectionServer.UseVisualStyleBackColor = true;
            this.btnConnectionServer.Click += new System.EventHandler(this.btnConnectionServer_Click);
            // 
            // txtLocalPort
            // 
            this.txtLocalPort.Location = new System.Drawing.Point(90, 46);
            this.txtLocalPort.Name = "txtLocalPort";
            this.txtLocalPort.Size = new System.Drawing.Size(70, 21);
            this.txtLocalPort.TabIndex = 16;
            this.txtLocalPort.Text = "1234";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "本机端口";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(215, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "加入服务";
            // 
            // txtServerIp
            // 
            this.txtServerIp.Location = new System.Drawing.Point(286, 12);
            this.txtServerIp.Name = "txtServerIp";
            this.txtServerIp.Size = new System.Drawing.Size(98, 21);
            this.txtServerIp.TabIndex = 19;
            this.txtServerIp.Text = "192.168.255.255";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(390, 12);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(56, 21);
            this.txtServerPort.TabIndex = 21;
            this.txtServerPort.Text = "1234";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 492);
            this.Controls.Add(this.txtServerPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtServerIp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLocalPort);
            this.Controls.Add(this.btnConnectionServer);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.RefProcess);
            this.Controls.Add(this.cbFirstKey);
            this.Controls.Add(this.lFriends);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblLocalIp);
            this.Controls.Add(this.lProcess);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKey);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Boss Coming";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.ListBox lProcess;
        private System.Windows.Forms.Label lblLocalIp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ListBox lFriends;
        private System.Windows.Forms.NotifyIcon nicon;
        private System.Windows.Forms.ComboBox cbFirstKey;
        private System.Windows.Forms.Button RefProcess;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Button btnConnectionServer;
        private System.Windows.Forms.TextBox txtLocalPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServerIp;
        private System.Windows.Forms.TextBox txtServerPort;
    }
}

