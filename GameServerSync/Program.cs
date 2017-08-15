using SpaceMate.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerSync
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Command cmd = new Command();
            cmd.CommandType = CommandType.CREATE_SESSION;
            cmd.Content = "TESTE123";

            byte[] data = SerializerUtil.Serialize<Command>(cmd);
            object returnObj = SerializerUtil.Deserialize(data);
            var otherCmd = returnObj as Command;
            Console.WriteLine("Command: {0}", otherCmd.Content);
            Console.ReadLine();
            */

            int indexOfIp = args.Select((v, i) => new { parameter = v, index = i }).First(m => m.parameter.Contains("-ip")).index;
            int indexOfPort = args.Select((v, i) => new { parameter = v, index = i }).First(m => m.parameter.Contains("-port")).index;

            String ipAddress = args[indexOfIp + 1];
            int port = Convert.ToInt32(args[indexOfPort + 1]);

            if (args.Length > 0)
            {
                GameServer server = new GameServer(ipAddress, port);
                server.Start();
            }
            else
            {
                Console.WriteLine("Informe o IP e porta por parametro");
                Console.ReadLine();
            }

            

        }
    }
}
