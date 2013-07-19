using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace WindowsPhone.Contracts.Communication
{
    public delegate void Received(object data, StreamSocket socket, IChannel channel, DateTime timestamp);

    /// <summary>
    /// Responsible for establishing a connection with the target machine
    /// Will delegate message handling to the IChannel
    /// </summary>
    public interface IConnection
    {
        event Received OnReceived;

        /// <summary>
        /// open our connection
        /// </summary>
        void Open(string peer);

        /// <summary>
        /// Close the connection
        /// </summary>
        void Close();

        /// <summary>
        /// Properly dispose
        /// </summary>
        void Dispose();

        /// <summary>
        /// Send data, which is an object which the Channel can interpret and convert to a byte[]
        /// </summary>
        /// <param name="data"></param>
        void Send(object data);


    }
}
