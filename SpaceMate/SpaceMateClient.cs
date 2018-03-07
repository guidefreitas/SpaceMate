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

namespace SpaceMate
{
    public partial class SpaceMateClient : Form
    {
        bool isHost = true;
        String sessionUUID = "";
        String serverIpAddress = "";
        Int32 serverPort = 0;
        Thread gameThread;

        public SpaceMateClient()
        {
            InitializeComponent();
            tbServerAddress.Text = "127.0.0.1";
            tbServerPort.Text = "11000";
            cbHost.Checked = true;
            tbSessionID.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbServerAddress.Text))
            {
                MessageBox.Show("Informe o IP do servidor");
                tbServerAddress.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(tbServerPort.Text))
            {
                MessageBox.Show("Informe a porta do servidor");
                tbServerPort.Focus();
                return;
            }
            else
            {
                if(!Int32.TryParse(tbServerPort.Text, out serverPort))
                {
                    MessageBox.Show("Informe um número para a porta");
                    tbServerPort.Focus();
                    return;
                }
            }
            
            if(!cbHost.Checked && String.IsNullOrWhiteSpace(tbSessionID.Text))
            {
                MessageBox.Show("Informe se é um HOST ou o ID da sessão");
                return;
            }

            serverPort = Convert.ToInt32(tbServerPort.Text);
            serverIpAddress = tbServerAddress.Text;
            isHost = cbHost.Checked;
            sessionUUID = tbSessionID.Text;

            gameThread = new Thread(() => {
                var game = new SpaceMateGame(isHost, sessionUUID, serverIpAddress, serverPort);
                game.Run();
            });

            gameThread.Start();
            button1.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbHost.Checked)
            {
                tbSessionID.Enabled = false;
            }
            else
            {
                tbSessionID.Enabled = true;
            }
        }
    }
}
