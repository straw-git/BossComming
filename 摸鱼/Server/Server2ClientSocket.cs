using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace 摸鱼.Server
{
    public class Server2ClientSocket
    {
        //客户端socket
        private Socket m_Socket;
        private Thread m_ReveiveThread;

        public string Name = "";

        #region 接收消息所需变量

        //接收数据包的缓冲字节
        private byte[] m_ReceiveBuffer = new byte[2048];

        //接收数据包的缓冲数据流
        private ServerMemoryStream m_ReceiveMS = new ServerMemoryStream();

        //接收消息的队列
        private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]>();

        private int m_ReceiveCount = 0;

        #endregion

        #region 发送消息所需变量

        //发送消息队列
        private Queue<byte[]> m_SendQueue = new Queue<byte[]>();

        //检查队列的委托
        private Action m_CheckSendQuene;

        #endregion

        public Server2ClientSocket() { }

        public string SocketID;

        public Server2ClientSocket(Socket socket)
        {
            SocketID = Guid.NewGuid().ToString();

            m_Socket = socket;
            //启动线程 接收数据
            m_ReveiveThread = new Thread(ReceiveMsg);

            m_ReveiveThread.Start();

            m_CheckSendQuene = OnCheckQueueCallBack;

        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnect()
        {
            if (m_Socket != null && m_Socket.Connected)
            {
                m_Socket.Shutdown(SocketShutdown.Both);
                m_Socket.Close();
            }
        }

        //=============================================接收数据=================================================

        #region 接收数据

        /// <summary>
        /// 接收数据方法
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveMsg()
        {
            //异步接收数据
            m_Socket.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Socket);
        }

        #endregion

        #region ReceiveCallBack 接收数据回调
        /// <summary>
        /// 接收数据回调
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int len = m_Socket.EndReceive(ar);

                if (len > 0)
                {
                    //已经接收到数据

                    //把接收到数据 写入缓冲数据流的尾部
                    m_ReceiveMS.Position = m_ReceiveMS.Length;

                    //把指定长度的字节 写入数据流
                    m_ReceiveMS.Write(m_ReceiveBuffer, 0, len);

                    //如果缓存数据流的长度>2 说明至少有个不完整的包过来了
                    //为什么这里是2 因为我们客户端封装数据包 用的ushort 长度就是2
                    if (m_ReceiveMS.Length > 2)
                    {
                        //进行循环 拆分数据包
                        while (true)
                        {
                            //把数据流指针位置放在0处
                            m_ReceiveMS.Position = 0;

                            //currMsgLen = 包体的长度
                            int currMsgLen = m_ReceiveMS.ReadUShort();

                            //currFullMsgLen 总包的长度=包头长度+包体长度
                            int currFullMsgLen = 2 + currMsgLen;

                            //如果数据流的长度>=整包的长度 说明至少收到了一个完整包
                            if (m_ReceiveMS.Length >= currFullMsgLen)
                            {
                                //至少收到一个完整包

                                //定义包体的byte[]数组
                                byte[] buffer = new byte[currMsgLen];

                                //把数据流指针放到2的位置 也就是包体的位置
                                m_ReceiveMS.Position = 2;

                                //把包体读到byte[]数组
                                m_ReceiveMS.Read(buffer, 0, currMsgLen);

                                lock (m_ReceiveQueue)
                                {
                                    m_ReceiveQueue.Enqueue(buffer);
                                }

                                //==============处理剩余字节数组===================

                                #region 处理剩余字节

                                //剩余字节长度
                                int remainLen = (int)m_ReceiveMS.Length - currFullMsgLen;
                                if (remainLen > 0)
                                {
                                    //把指针放在第一个包的尾部
                                    m_ReceiveMS.Position = currFullMsgLen;

                                    //定义剩余字节数组
                                    byte[] remainBuffer = new byte[remainLen];

                                    //把数据流读到剩余字节数组
                                    m_ReceiveMS.Read(remainBuffer, 0, remainLen);

                                    //清空数据流
                                    m_ReceiveMS.Position = 0;
                                    m_ReceiveMS.SetLength(0);

                                    //把剩余字节数组重新写入数据流
                                    m_ReceiveMS.Write(remainBuffer, 0, remainBuffer.Length);

                                    remainBuffer = null;

                                    #endregion

                                }
                                else
                                {
                                    //没有剩余字节
                                    //清空数据流
                                    m_ReceiveMS.Position = 0;
                                    m_ReceiveMS.SetLength(0);

                                    break;
                                }
                            }
                            else
                            {
                                //还没有收到完整包
                                break;
                            }
                        }
                    }

                    //进行下一次接收数据包
                    ReceiveMsg();
                    //同时发布接收数据协议
                    OnReceiveDataDis();
                }
                else
                {
                    Console.WriteLine($"[内部]客户端 [{m_Socket.RemoteEndPoint}] 断开链接", true);
                    try
                    {

                        Global.Main.SocketOut(this);
                    }
                    catch { }

                    Thread.CurrentThread.Abort();
                }
            }
            catch
            {
                //GlobalData.MainWindow.Log($"[内部]客户端 [{ m_Socket.RemoteEndPoint}] 断开链接", true);
                try
                {

                    Global.Main.SocketOut(this);
                }
                catch { }
                Thread.CurrentThread.Abort();
            }
        }
        #endregion

        #region 检查接收数据包

        /// <summary>
        /// 检查接收到的数据包 
        /// </summary>
        public void OnReceiveDataDis()
        {
            #region 从队列中获取数据

            if (m_ReceiveCount <= 5)
            {
                m_ReceiveCount++;
                lock (m_ReceiveQueue)
                {
                    if (m_ReceiveQueue.Count > 0)
                    {
                        //得到队列中的数据包
                        byte[] buffer = m_ReceiveQueue.Dequeue();

                        //异或之后的数组
                        byte[] bufferNew = new byte[buffer.Length - 3];

                        bool isCompress = false;
                        ushort crc = 0;

                        using (ServerMemoryStream ms = new ServerMemoryStream(buffer))
                        {
                            isCompress = ms.ReadBool();
                            crc = ms.ReadUShort();
                            ms.Read(bufferNew, 0, bufferNew.Length);
                        }

                        //先crc
                        int newCrc = Crc16.CalculateCrc16(bufferNew);

                        if (newCrc == crc)
                        {
                            //异或 得到原始数据
                            bufferNew = SecurityUtil.Xor(bufferNew);

                            if (isCompress)
                            {
                                bufferNew = ZlibHelper.DeCompressBytes(bufferNew);
                            }

                            ushort protoCode = 0;
                            byte[] protoContent = new byte[bufferNew.Length - 2];
                            using (ServerMemoryStream ms = new ServerMemoryStream(bufferNew))
                            {
                                //协议编号
                                protoCode = ms.ReadUShort();
                                ms.Read(protoContent, 0, protoContent.Length);
                                //交给观察者分配处理
                                ServerEventObserver.Instance.Dispatch(protoCode, protoContent, this);

                            }
                        }
                    }
                }
            }
            else
            {
                m_ReceiveCount = 0;
                OnReceiveDataDis();
            }
            #endregion

        }

        #endregion


        //================================================发送数据==============================================

        #region OnCheckSendQueueCallBack 检查队列的委托回调
        /// <summary>
        /// 检查队列的委托回调
        /// </summary>
        private void OnCheckQueueCallBack()
        {
            //检查发送队列中是否有数据
            lock (m_SendQueue)
            {
                //如果队列中有数据包 则发送数据包
                if (m_SendQueue.Count > 0)
                {
                    //发送数据包
                    SendMsg(m_SendQueue.Dequeue());
                }
            }

            //检查接收数据对列中是否有数据
            lock (m_ReceiveQueue)
            {
                if (m_ReceiveQueue.Count > 0)
                {
                    OnReceiveDataDis();
                }
            }
        }
        #endregion

        #region Send 发送消息 把消息加入队列
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="buffer"></param>
        public void Send(byte[] buffer)
        {
            //得到封装后的数据包
            byte[] sendBuffer = ByteTransmit.SendByte(buffer);

            //lock (m_SendQueue)
            //{
            //把数据包加入队列
            m_SendQueue.Enqueue(sendBuffer);

            if (m_CheckSendQuene != null)
                //启动委托（执行委托）
                m_CheckSendQuene.BeginInvoke(null, null);
            //}
        }
        #endregion

        #region SendSendMsg 真正发送数据包到服务器
        /// <summary>
        /// 真正发送数据包到服务器
        /// </summary>
        /// <param name="buffer"></param>
        private void SendMsg(byte[] buffer)
        {
            m_Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, m_Socket);
        }
        #endregion

        #region SendCallBack 发送数据包的回调
        /// <summary>
        /// 发送数据包的回调
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallBack(IAsyncResult ar)
        {
            try
            {
                m_Socket.EndSend(ar);

                //继续检查队列
                OnCheckQueueCallBack();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ManagerSocket.SendCallBack]检查队列出现问题:{ex.Message}", true);
            }
        }
        #endregion
    }
}
