using SpaceMate.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServerSync
{
    class Program
    {
        public static ServerScreen serverScreen;        
        [STAThread]
        static void Main(string[] args)
        {
            /*
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
            */

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            serverScreen = new ServerScreen();
            Application.Run(serverScreen);


        }
    }
}
