using System;
using System.Collections.Generic;

public class SecurityUtil
{

    #region 亦或因子

    private static readonly byte[] xorScale = new byte[] {
     45, 66, 38, 55, 23, 254, 9, 165, 90, 19, 41, 45, 201, 58, 55, 37, 254, 185, 165, 168, 19, 171
    };

    #endregion

    /// <summary>
    /// 私有构造函数 防止其被实例化
    /// </summary>
    private SecurityUtil() { }

    /// <summary>
    /// 亦或
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static byte[] Xor(byte[] buffer)
    {

        int iScaleLen = xorScale.Length;
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)(buffer[i] ^ xorScale[i % iScaleLen]);
        }
        return buffer;
    }
}

