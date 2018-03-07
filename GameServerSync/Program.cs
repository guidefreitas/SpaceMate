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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            serverScreen = new ServerScreen();
            Application.Run(serverScreen);
        }
    }
}
