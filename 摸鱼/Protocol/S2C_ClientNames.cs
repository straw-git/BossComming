using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 摸鱼.Protocol
{
    public class S2C_ClientNames
    {
        public ushort ProtoCode { get { return ProtocolCodes.S2C_ClientNames; } }

        public int DataCount = 0;
        public List<string> Names;

        public byte[] ToByte()
        {
            using (ServerMemoryStream ms = new ServerMemoryStream())
            {
                ms.WriteUShort(ProtoCode);

                ms.WriteInt(DataCount);
                for (int i = 0; i < DataCount; i++)
                {
                    ms.WriteUTF8String(Names[i]);
                }
                return ms.ToArray();
            }
        }
        public S2C_ClientNames ToProtocol(byte[] buffer)
        {
            S2C_ClientNames pro = new S2C_ClientNames();
            using (ServerMemoryStream ms = new ServerMemoryStream(buffer))
            {
                pro.DataCount = ms.ReadInt();
                pro.Names = new List<string>();
                for (int i = 0; i < pro.DataCount; i++)
                {
                    pro.Names.Add(ms.ReadUTF8String());
                }
                return pro;
            }
        }
    }
}
