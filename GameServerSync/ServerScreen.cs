using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServerSync
{
    public partial class ServerScreen : Form
    {
        GameServer server;
        Thread serverThread;
        delegate void SetTextCallback(string text);

        public ServerScreen()
        {
            InitializeComponent();
            btStop.Enabled = false;
        }

        public void ShowMessage(String message)
        {
            if (this.tbMessages.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(ShowMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                tbMessages.Text += message + Environment.NewLine;
            }
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            try
            {
                tbMessages.Text = "";

                String ipAddress = tbAdress.Text;
                if (String.IsNullOrWhiteSpace(ipAddress))
                    throw new Exception("Informe o endereço IP");

                String portS = tbPort.Text;
                if (String.IsNullOrWhiteSpace(portS))
                    throw new Exception("Infome o número da porta");

                int port = 0;
                if (!Int32.TryParse(portS, out port))
                    throw new Exception("Porta inválida");

                serverThread = new Thread(() => {
                    server = new GameServer(ipAddress, port);
                    server.Start();
                });
                serverThread.Start();
                
                btStart.Enabled = false;
                btStop.Enabled = true;
                ShowMessage("Servidor iniciado com sucesso.");
            }catch(Exception ex)
            {
                ShowMessage("Ocorreu um problema");
                MessageBox.Show(ex.Message);
            }
        }

        private void StopServer()
        {
            if (server != null)
            {
                server.Stop();
                btStop.Enabled = false;
                btStart.Enabled = true;
            }
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            try
            {
                StopServer();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void tbMessages_TextChanged(object sender, EventArgs e)
        {
            tbMessages.SelectionStart = tbMessages.Text.Length;
            tbMessages.ScrollToCaret();
        }

        private void ServerScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }
    }
}
