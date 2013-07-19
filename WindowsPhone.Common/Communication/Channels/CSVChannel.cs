    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using WindowsPhone.Contracts.Communication;

namespace WindowsPhone.Common.Communication.Channels
{
    public class CSVChannel : StringChannel, IChannel
    {
        public CSVChannel(){}
        public CSVChannel(char seperator = ',')
        {
            Seperator = seperator;
        }
        public char Seperator { get; set; }


        public object ConvertTo(byte[] input)
        {
            return ((string)base.ConvertTo(input)).Split(Seperator);
        }

        public byte[] ConvertFrom(object data)
        {
            var sList = (string[])data;
            var full = string.Join(Seperator.ToString(), sList);
            return base.ConvertFrom(full);
        }
    }
}
