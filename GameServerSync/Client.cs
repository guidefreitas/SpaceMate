using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServerSync
{
    public class Client
    {
        public Client(Socket socket)
        {
            UUID = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
            this.Socket = socket;
            this.NetworkStream = new NetworkStream(this.Socket);
            this.StreamWriter = new System.IO.StreamWriter(this.NetworkStream);
            this.StreamReader = new System.IO.StreamReader(this.NetworkStream);
        }
        public String UUID { get; set; }
        public Socket Socket { get; set; }
        public NetworkStream NetworkStream { get; set; }
        public System.IO.StreamWriter StreamWriter { get; set; }
        public System.IO.StreamReader StreamReader { get; set; }
    }
}
