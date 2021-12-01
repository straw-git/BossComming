using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace 摸鱼.Client
{
    /// <summary>
    /// 尝试查找局网内的服务端
    /// </summary>
    public class TryFindServer : Singleton<TryFindServer>
    {
        public Action<bool> OnCompleted;

        public void EachIps(string _ip)
        {
            string baseIp = _ip.Substring(0, _ip.LastIndexOf('.'));
            new Thread(() =>
            {
                for (int i = 1; i < 256; i++)
                {
                    Ping myPing = new Ping();
                    myPing.PingCompleted += new PingCompletedEventHandler(PingIpCompleted);
                    string currIp = $"{baseIp}.{i}";
                    myPing.SendAsync(currIp, 200, currIp);
                }

            }).Start();
        }

        public static int ActivityCount = 1;

        private void PingIpCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Reply.Status == IPStatus.Success)
            {
                string ip = e.Reply.Address.ToString();
                //能连接 开始测试链接
                if (Client2ServerSocket.Instance.Connect(ip, 1234))
                {
                    OnCompleted?.Invoke(true);
                }
                Interlocked.Increment(ref ActivityCount);
                if (ActivityCount == 255)
                {
                    OnCompleted?.Invoke(false);
                }
            }
        }
    }
}
