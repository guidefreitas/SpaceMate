using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMate.Common
{
    public class ConfigureSocket
    {
        public static void ConfigureTcpSocket(Socket tcpSocket)
        {
            //tcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // Don't allow another socket to bind to this port.
            //tcpSocket.ExclusiveAddressUse = true;

            // The socket will linger for 10 seconds after 
            // Socket.Close is called.
            // tcpSocket.LingerState = new LingerOption(true, 10);

            // Disable the Nagle Algorithm for this tcp socket.
            tcpSocket.NoDelay = true;

            // Set the receive buffer size to 8k
            tcpSocket.ReceiveBufferSize = 512;

            // Set the timeout for synchronous receive methods to 
            // 1 second (1000 milliseconds.)
            //tcpSocket.ReceiveTimeout = 1000;

            // Set the send buffer size to 8k.
            tcpSocket.SendBufferSize = 512;

            // Set the timeout for synchronous send methods
            // to 1 second (1000 milliseconds.)			
            //tcpSocket.SendTimeout = 1000;

            // Set the Time To Live (TTL) to 42 router hops.
            //tcpSocket.Ttl = 42;
        }
    }
}
