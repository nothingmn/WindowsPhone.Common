using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPhone.Contracts.Communication;

namespace WindowsPhone.Common.Communication.Channels
{
    public class ByteArrayChannel : IChannel 
    {

        public object ConvertTo(byte[] input)
        {
            return (object) input;
        }

        public byte[] ConvertFrom(object data)
        {
            return (byte[]) data;
        }
    }
}
