//===================================================
//创建时间：2020-05-19 13:18:03
//备    注：zyue
//===================================================
using System.Collections.Generic;

namespace 摸鱼.Protocol
{
    /// <summary>
    /// 协议编号定义
    /// </summary>
    public struct ProtocolCodes
    {
        public const int C2S_SendMyName = 1001;
        public const int C2S_Close = 1002;

        public const int S2C_ClientNames = 2001;
        public const int S2C_Close = 2002;
        //所有的键集合
        public static List<int> AllCode = new List<int>() { 1001, 1002, 2001,2002 };
    }
}
