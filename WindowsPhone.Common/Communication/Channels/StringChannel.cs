using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPhone.Contracts.Communication;

namespace WindowsPhone.Common.Communication.Channels
{
    public class StringChannel : IChannel
    {
        public StringChannel()
        {
        }

        public object ConvertTo(byte[] input)
        {
            return new string(System.Text.Encoding.UTF8.GetChars(input));
        }

        public byte[] ConvertFrom(object data)
        {
            return System.Text.Encoding.UTF8.GetBytes((string)data);
        }
    }
}