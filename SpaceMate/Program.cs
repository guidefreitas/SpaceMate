using System;
using System.Linq;
using System.Windows.Forms;

namespace SpaceMate
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            /*
            bool isHost = true;
            String sessionUUID = "";
            String serverIpAddress = "";
            Int32 serverPort = 0;

            if(args.Length > 0)
            {
                if(args.Where(m => m.Contains("-host")).Any())
                {
                    isHost = true;
                }

                if(args.Where(m => m.Contains("-session")).Any())
                {
                    isHost = false;
                    int indexOfSession = args.Select((v, i) => new { parameter = v, index = i }).First(m => m.parameter.Contains("-session")).index;
                    sessionUUID = args[indexOfSession + 1];
                    Console.WriteLine("Connecting to Session: " + sessionUUID);
                }
            }

            int indexOfIp = args.Select((v, i) => new { parameter = v, index = i }).First(m => m.parameter.Contains("-ip")).index;
            int indexOfPort = args.Select((v, i) => new { parameter = v, index = i }).First(m => m.parameter.Contains("-port")).index;
            serverIpAddress = args[indexOfIp + 1];
            serverPort = Convert.ToInt32(args[indexOfPort + 1]);

            using (var game = new SpaceMateGame(isHost, sessionUUID, serverIpAddress, serverPort))
            {
                game.Run();
            }
            */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SpaceMateClient());
            
        }
    }
}
