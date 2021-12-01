using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 摸鱼.Client;
using 摸鱼.Protocol;
using 摸鱼.Server;

namespace 摸鱼
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            Global.Main = this;

            //指定不再捕获对错误线程的调用
            Control.CheckForIllegalCrossThreadCalls = false;

            cbFirstKey.SelectedIndex = 1;
            txtKey.Text = "S";

            //服务端收到客户端的
            ServerEventObserver.Instance.AddEventListener(ProtocolCodes.C2S_SendMyName, OnClientSendName);//有客户端向我发送名称
            ServerEventObserver.Instance.AddEventListener(ProtocolCodes.C2S_Close, OnClientClose);//客户端发送停止

            //客户端收到服务端的
            ClientEventObserver.Instance.AddEventListener(ProtocolCodes.S2C_ClientNames, OnNamesComming);//当服务端发来所有的名字
            ClientEventObserver.Instance.AddEventListener(ProtocolCodes.S2C_Close, Kill);//停止
        }

        private void Kill(byte[] pxx, Client2ServerSocket z)
        {
            if (selectedProNames.Count > 0)
            {
                foreach (var item in selectedProNames)
                {
                    foreach (Process p in Process.GetProcessesByName(item))//根据名称关闭
                    {
                        if (!p.CloseMainWindow())
                        {
                            p.Kill();
                        }
                    }
                }
            }
        }

        private void OnClientClose(byte[] p, Server2ClientSocket z)
        {
            foreach (var item in Clients.Sockets)
            {
                //向每个人发送停止
                item.Send(new S2C_Close().ToByte());
            }
        }

        bool isConnectionServer = false;
        List<string> selectedProNames = new List<string>();

        private void OnNamesComming(byte[] p, Client2ServerSocket z)
        {
            lock (lFriends) //防止多线程操作显示错误
            {
                //更新列表
                lFriends.Items.Clear();

                var namesPro = new S2C_ClientNames().ToProtocol(p);
                for (int i = 0; i < namesPro.DataCount; i++)
                {
                    lFriends.Items.Add(namesPro.Names[i]);
                }
            }
        }

        private void OnClientSendName(byte[] p, Server2ClientSocket z)
        {
            //获取到名称加入到客户端列表
            Clients.Sockets.First(c => c.SocketID == z.SocketID).Name = new C2S_SendMyName().ToProtocol(p).Name;

            List<string> names = new List<string>();
            foreach (var item in Clients.Sockets)
            {
                names.Add(item.Name);
            }

            foreach (var item in Clients.Sockets)
            {
                //向每个人更新名单
                item.Send(new S2C_ClientNames() { DataCount = names.Count, Names = names }.ToByte());
            }
        }

        Socket m_socket;
        Thread mThread;

        private void Main_Load(object sender, EventArgs e)
        {
            #region 获取本机Ip

            string ip = "";
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);

            foreach (IPAddress ipa in ipadrlist) if (ipa.AddressFamily == AddressFamily.InterNetwork) ip = ipa.ToString();

            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("网卡信息错误");
                return;
            }
            lblLocalIp.Text = ip;

            #endregion

            RefRunningItems();
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {

            btnStartServer.Enabled = false;//开启后 按钮不可用

            int port = 0;

            if (!int.TryParse(txtLocalPort.Text, out port))
            {
                MessageBox.Show("请正确输入服务端口");
                txtLocalPort.Focus();
                txtLocalPort.SelectAll();
                btnStartServer.Enabled = true;
                return;
            }

            #region 开启服务端

            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                m_socket.Bind(new IPEndPoint(IPAddress.Parse(lblLocalIp.Text), port));
                m_socket.Listen(port);
            }
            catch
            {
                //配置冲突，端口被占用
                MessageBox.Show("配置冲突，请更换端口");
                txtLocalPort.Focus();
                txtLocalPort.SelectAll();
                btnStartServer.Enabled = true;
                return;
            }

            mThread = new Thread(ListenCallBack);
            mThread.Start();

            #endregion

            txtServerIp.Text = lblLocalIp.Text;
            txtServerPort.Text = port.ToString();


            MessageBox.Show($"开启成功服务 {lblLocalIp.Text}:{port}");
            //模拟触发
            btnConnectionServer_Click(null, null);
        }

        private void btnConnectionServer_Click(object sender, EventArgs e)
        {
            int port = 0;

            btnConnectionServer.Enabled = false;//开启后 不可加入其它

            if (!int.TryParse(txtLocalPort.Text, out port))
            {
                MessageBox.Show("请正确输入端口");
                txtServerIp.Focus();
                txtServerPort.SelectAll();
                btnConnectionServer.Enabled = true;
                return;
            }

            //连接服务端
            if (Client2ServerSocket.Instance.Connect(txtServerIp.Text, port))
            {
                //有服务端 客户端已连接上了 向服务端发送自己的名字
                Client2ServerSocket.Instance.Send(new C2S_SendMyName() { Name = txtName.Text }.ToByte());

                MessageBox.Show($"已连接服务  {txtServerIp.Text}:{port}");
                isConnectionServer = true;
            }
            else
            {
                MessageBox.Show($"连接失败");
                txtServerIp.Focus();
                txtServerPort.SelectAll();
                btnConnectionServer.Enabled = true;
                return;
            }
        }

        internal void SocketOut(Server2ClientSocket server2ClientSocket)
        {
            Clients.Sockets.Remove(server2ClientSocket);

            List<string> names = new List<string>();
            foreach (var item in Clients.Sockets)
            {
                names.Add(item.Name);
            }

            foreach (var item in Clients.Sockets)
            {
                //向每个人更新名单
                item.Send(new S2C_ClientNames() { DataCount = names.Count, Names = names }.ToByte());
            }
        }

        private void ListenCallBack()
        {
            while (true)
            {
                //获取 客户端socket
                Socket clientSocket = m_socket.Accept();
                Server2ClientSocket s2c = new Server2ClientSocket(clientSocket);

                //有人进来了 

                List<string> names = new List<string>();
                foreach (var item in Clients.Sockets)
                {
                    names.Add(item.Name);
                }

                //向这个人发送当前服务端所有的人员
                s2c.Send(new S2C_ClientNames() { DataCount = names.Count, Names = names }.ToByte());

                //将这个人加入到列表
                Clients.Sockets.Add(s2c);
                //1s线程休息
                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// 服务端断开连接
        /// </summary>
        public void ServerDisConnection()
        {
            MessageBox.Show("服务端已掉线");

            isConnectionServer = false;
            btnConnectionServer.Enabled = true;
            btnStartServer.Enabled = true;
        }

        #region 热键操作及最小化到托盘

        IntPtr currHandle = new IntPtr(0);//当前的窗体Handle
        bool currHandleRegister = false;//当前Handle是否已注册

        Keys currSecondKey = Keys.S;//当前的热键
        /// <summary>
        /// 更新热键
        /// </summary>
        /// <param name="enforce">是否强制更新（如 没有更换窗体 必须强制更新）</param>
        private void UpdateHotKey(bool enforce = false)
        {
            IntPtr _handle = this.Handle;
            if (currHandleRegister)
            {
                //之前注册过
                if (_handle != currHandle || enforce)
                {
                    UnregisterHotKey(currHandle, 11);
                    UnregisterHotKey(currHandle, 10);

                    MODKEY _focusKey = MODKEY.None;
                    switch (cbFirstKey.SelectedIndex)
                    {
                        case 0:
                            break;
                        case 1:
                            _focusKey = MODKEY.ALT;
                            break;
                        case 2:
                            _focusKey = MODKEY.CTRL;
                            break;
                        case 3:
                            _focusKey = MODKEY.WIN;
                            break;
                        case 4:
                            _focusKey = MODKEY.SHIFT;
                            break;
                        default:
                            MessageBox.Show("未处理的热键");
                            return;
                    }

                    RegisterHotKey(_handle, 11, _focusKey, currSecondKey);
                    RegisterHotKey(_handle, 10, MODKEY.None, Keys.Tab);

                    currHandle = _handle;
                }
            }
            else
            {
                //之前没注册过
                MODKEY _focusKey = MODKEY.None;
                switch (cbFirstKey.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
                        _focusKey = MODKEY.ALT;
                        break;
                    case 2:
                        _focusKey = MODKEY.CTRL;
                        break;
                    case 3:
                        _focusKey = MODKEY.WIN;
                        break;
                    case 4:
                        _focusKey = MODKEY.SHIFT;
                        break;
                    default:
                        MessageBox.Show("未处理的热键");
                        return;
                }

                RegisterHotKey(_handle, 11, _focusKey, currSecondKey);
                RegisterHotKey(_handle, 10, MODKEY.None, Keys.Tab);
                currHandleRegister = true;
                currHandle = _handle;
            }
        }

        //重写Closing（），关闭窗口时不退出程序，而是显示在托盘图标
        //需要先在界面中添加一个 NotifyIcon控件，然后设置一个图标
        //有右键菜单，则再添加一个ContextMenuStrip
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; //不退出程序
            Clipboard.Clear();//先清空剪贴板防止剪贴板里面先复制了其他内容
            this.WindowState = FormWindowState.Minimized; //最小化
            this.ShowInTaskbar = false; //在任务栏中不显示窗体
            this.nicon.Visible = true; //托盘图标可见
            UpdateHotKey();
        }

        private void nicon_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                UpdateHotKey();
            }
            this.Focus();
        }

        #region 热键唤醒

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                switch (m.WParam.ToInt32())
                {
                    case 11:
                        //触发了快捷键
                        OnHotKeyEnter();
                        break;
                    case 10:
                        nicon_MouseClick(null, null);
                        break;
                    default:
                        break;
                }
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 热键被触发
        /// </summary>
        private void OnHotKeyEnter()
        {
            if (!isConnectionServer)
            {
                MessageBox.Show("没有连接服务或开启服务");
                return;
            }
            Client2ServerSocket.Instance.Send(new C2S_Close().ToByte());
        }

        //要定义热键的窗口的句柄
        //定义热键ID（不能与其它ID重复）int id, 
        //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效KeyModifiers fsModifiers,Keys vk                    
        //定义热键的内容
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr wnd, int id, MODKEY mode, Keys vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr wnd, int id);

        [Flags()]
        public enum MODKEY
        {
            None = 0,
            ALT = 0x0001,
            CTRL = 0x0002,
            SHIFT = 0x0004,
            WIN = 0x0008,
        }

        #endregion


        private void txtKey_KeyDown(object sender, KeyEventArgs e)
        {
            currSecondKey = e.KeyCode;
            txtKey.Text = currSecondKey.ToString();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            UpdateHotKey(true);
            MessageBox.Show("快捷键设置成功,只需要在关键时刻按下快捷键你的同伴电脑就会收到通知并关闭电脑上的程序");

            selectedProNames.Clear();
            foreach (var item in lProcess.SelectedItems)
            {
                string fullName= item.ToString();
                fullName = fullName.Substring(0, fullName.IndexOf('=')) ;

                selectedProNames.Add(fullName);
            }

            this.Close();
        }

        private void cbFirstKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFirstKey.SelectedIndex == 0)
            {
                MessageBox.Show("如果选择无组合键,之后的按键必须是字母(如S),其它按键无效");
            }
        }

        #endregion 

        /// <summary>
        /// 刷新正在运行的进程
        /// </summary>
        private void RefRunningItems()
        {
            lock (lProcess)
            {
                lProcess.Items.Clear();

                Process[] ps = Process.GetProcesses();
                foreach (Process p in ps)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(p.MainWindowTitle)) continue;
                        lProcess.Items.Add($"{p.ProcessName}={p.MainWindowTitle}");
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void RefProcess_Click(object sender, EventArgs e)
        {
            RefRunningItems();
        }

    }
}
