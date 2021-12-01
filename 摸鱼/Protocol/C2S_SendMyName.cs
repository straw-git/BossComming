using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 摸鱼.Protocol
{
    public class C2S_SendMyName
    {
        public ushort ProtoCode { get { return ProtocolCodes.C2S_SendMyName; } }

        public string Name = "";

        public byte[] ToByte()
        {
            using (ServerMemoryStream ms = new ServerMemoryStream())
            {
                ms.WriteUShort(ProtoCode);

                ms.WriteUTF8String(Name);
                return ms.ToArray();
            }
        }
        public C2S_SendMyName ToProtocol(byte[] buffer)
        {
            C2S_SendMyName pro = new C2S_SendMyName();
            using (ServerMemoryStream ms = new ServerMemoryStream(buffer))
            {
                pro.Name = ms.ReadUTF8String();
                return pro;
            }
        }
    }
}
