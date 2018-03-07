using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServerSync
{
    public class GameSession
    {
        public String UUID { get; set; }
        public String Name { get; set; }
        public List<Client> Subscribers { get; set; }

        public GameSession()
        {
            Subscribers = new List<Client>();
            UUID = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
        }
    }
}
