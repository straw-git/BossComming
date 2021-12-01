using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


/// <summary>
/// 数据包处理
/// </summary>
public class ByteTransmit
{
    //压缩数组的长度界限
    private const int m_CompressLen = 200;

    /// <summary>
    /// 接收数据后的数据封包
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] SendByte(byte[] data)
    {
        byte[] retBuffer = null;

        //1.如果数据包的长度 大于了m_CompressLen 则进行压缩
        bool isCompress = data.Length > m_CompressLen ? true : false;
        if (isCompress)
        {
            data = ZlibHelper.CompressBytes(data);
        }

        //2.异或
        data = SecurityUtil.Xor(data);

        //3.Crc校验 压缩后的
        ushort crc = Crc16.CalculateCrc16(data);

        using (ServerMemoryStream ms = new ServerMemoryStream())
        {
            ms.WriteUShort((ushort)(data.Length + 3));
            ms.WriteBool(isCompress);
            ms.WriteUShort(crc);
            ms.Write(data, 0, data.Length);

            retBuffer = ms.ToArray();
        }
        return retBuffer;
    }

    /// <summary>  
    /// 截取字节数组  
    /// </summary>  
    /// <param name="srcBytes">要截取的字节数组</param>  
    /// <param name="startIndex">开始截取位置的索引</param>  
    /// <param name="length">要截取的字节长度</param>  
    /// <returns>截取后的字节数组</returns>  
    public static byte[] SubByte(byte[] srcBytes, int startIndex, int length)
    {
        using (ServerMemoryStream bufferStream = new ServerMemoryStream())
        {
            byte[] returnByte = new byte[] { };

            if (srcBytes == null) return returnByte;

            if (startIndex < 0) startIndex = 0;

            if (startIndex < srcBytes.Length)
            {
                if (length < 1 || length > srcBytes.Length - startIndex) { length = srcBytes.Length - startIndex; }

                bufferStream.Write(srcBytes, startIndex, length);
                returnByte = bufferStream.ToArray();
                bufferStream.SetLength(0);
                bufferStream.Position = 0;
            }
            return returnByte;
        }
    }
}
