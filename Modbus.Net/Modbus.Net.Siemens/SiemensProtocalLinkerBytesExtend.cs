﻿using System;

namespace Modbus.Net.Siemens
{
    /// <summary>
    ///     西门子Tcp协议扩展
    /// </summary>
    public class SiemensTcpProtocalLinkerBytesExtend : IProtocalLinkerBytesExtend
    {
        public byte[] BytesExtend(byte[] content)
        {
            Array.Copy(new byte[] {0x03, 0x00, 0x00, 0x00, 0x02, 0xf0, 0x80}, 0, content, 0, 7);
            Array.Copy(BigEndianValueHelper.Instance.GetBytes((ushort) content.Length), 0, content, 2, 2);
            return content;
        }

        public byte[] BytesDecact(byte[] content)
        {
            var newContent = new byte[content.Length - 7];
            Array.Copy(content, 7, newContent, 0, newContent.Length);
            return newContent;
        }
    }

    /// <summary>
    ///     西门子Ppi协议扩展
    /// </summary>
    public class SiemensPpiProtocalLinkerBytesExtend : IProtocalLinkerBytesExtend
    {
        public byte[] BytesExtend(byte[] content)
        {
            var newContent = new byte[content.Length + 2];
            Array.Copy(content, 0, newContent, 0, content.Length);
            Array.Copy(new byte[] {0x68, (byte) (content.Length - 4), (byte) (content.Length - 4), 0x68}, 0, newContent,
                0, 4);
            var check = 0;
            for (var i = 4; i < newContent.Length - 2; i++)
                check += newContent[i];
            check = check % 256;
            newContent[newContent.Length - 2] = (byte) check;
            newContent[newContent.Length - 1] = 0x16;
            return newContent;
        }

        public byte[] BytesDecact(byte[] content)
        {
            var newContent = new byte[content.Length - 9];
            Array.Copy(content, 7, newContent, 0, newContent.Length);
            return newContent;
        }
    }
}