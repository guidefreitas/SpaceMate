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


        public SpaceMateClient()
        {
            InitializeComponent();
            tbServerAddress.Text = "127.0.0.1";
            tbServerPort.Text = "11000";
            cbHost.Checked = true;

            lvRooms.Columns.Add("Name", -2, HorizontalAlignment.Left);
        }

        public void RefreshListViewRoomNames()
        {
            serverPort = Convert.ToInt32(tbServerPort.Text);
            serverIpAddress = tbServerAddress.Text;
            NetworkClient client = new NetworkClient();
            client.Connect(serverIpAddress, serverPort);

            lvRooms.Items.Clear();
            List<String> rooms = client.GetSessions();

            foreach (var name in rooms)
            {
                var item = new ListViewItem(new[] { name });
                lvRooms.Items.Add(item);
            }
            
        }

        public void StartGame()
        {
            var game = new SpaceMateGame(isHost, sessionUUID, serverIpAddress, serverPort);
            game.Run();
        }

        void ExceptionHandler(Task task)
        {
            var exception = task.Exception;
            MessageBox.Show("Ocorreu um problema: " + exception.Message);
            if (tbSessionID.InvokeRequired)
            {
                tbSessionID.Invoke(new MethodInvoker(delegate { button1.Enabled = true; }));
            }
            
        }

        private void create_Click(object sender, EventArgs e)
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

            if (String.IsNullOrEmpty(tbSessionID.Text))
            {
                MessageBox.Show("Informe o nome da sessão");
                return;
            }

            serverPort = Convert.ToInt32(tbServerPort.Text);
            serverIpAddress = tbServerAddress.Text;
            isHost = cbHost.Checked;
            sessionUUID = tbSessionID.Text;

            StartGameTask();
        }

        private void StartGameTask()
        {
            Task task = new Task(this.StartGame);
            task.ContinueWith(this.ExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            task.Start();

            gbCreateRoom.Enabled = false;
            gbJoinRoom.Enabled = false;
            gbServerInfo.Enabled = false;
        }

        private void join_Click(object sender, EventArgs e)
        {
            if(lvRooms.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Selecione uma sala");
            }
            var roomName = lvRooms.SelectedItems[0].Text;
            serverPort = Convert.ToInt32(tbServerPort.Text);
            serverIpAddress = tbServerAddress.Text;
            isHost = false;
            sessionUUID = roomName.ToString();
            StartGameTask();
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbServerAddress.Text))
            {
                MessageBox.Show("Informe o IP do servidor");
                return;
            }

            if (String.IsNullOrEmpty(tbServerPort.Text))
            {
                MessageBox.Show("Informe a porta do servidor");
                return;
            }
            RefreshListViewRoomNames();
        }
    }
}
