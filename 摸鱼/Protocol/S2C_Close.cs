using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 摸鱼.Protocol
{
    public class S2C_Close
    {
        public ushort ProtoCode { get { return ProtocolCodes.S2C_Close; } }

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
