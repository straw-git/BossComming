using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 摸鱼.Protocol
{
    public class C2S_Close
    {
        public ushort ProtoCode { get { return ProtocolCodes.C2S_Close; } }

        public byte[] ToByte()
        {
            using (ServerMemoryStream ms = new ServerMemoryStream())
            {
                ms.WriteUShort(ProtoCode);
                return ms.ToArray();
            }
        }
    }
}
