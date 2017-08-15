using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SpaceMate.Common;
using System.Diagnostics;

namespace SpaceMate
{
    
    public class NetworkClient
    {
        //private Socket socket;
        public TcpClient tcpClient;
        private NetworkStream networkStream;
        private System.IO.StreamReader streamReader;
        private System.IO.StreamWriter streamWriter;

        private String sessionUUID;

        bool isConnected = false;

        public NetworkClient()
        {
            
           
        }

        public String Connect(String ip, int port)
        {
            String clientUUID = "";
            //socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            //socket.NoDelay = true;
            //socket.Connect(IPAddress.Parse(ip), port);
            tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse(ip), port);
            tcpClient.NoDelay = true;
            ConfigureSocket.ConfigureTcpSocket(tcpClient.Client);
            //socketForServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
            networkStream = tcpClient.GetStream();
            streamReader = new System.IO.StreamReader(networkStream);
            streamWriter = new System.IO.StreamWriter(networkStream);
            isConnected = true;
            while (true)
            {
                String serverData = streamReader.ReadLine();
                if (serverData.StartsWith("connect_ok"))
                {
                    String[] data = serverData.Split('|');
                    clientUUID = data[1];
                    break;
                }
            }

            Debug.WriteLine("CLIENT: Connected to the server at {0}:{1}", ip, port);
            return clientUUID;
        }

        public String CreateSession(String sessionName)
        {
            streamWriter.WriteLine(String.Format("create_session|{0}", sessionName));
            streamWriter.Flush();
            while (true)
            {
                String returnServer = streamReader.ReadLine();
                if (returnServer.StartsWith("create_session_ok"))
                {
                    String[] data = returnServer.Split('|');
                    sessionUUID = data[1];
                    break;
                }else if (returnServer.StartsWith("create_session_error"))
                {
                    throw new Exception("Session create error");
                }
            }
            Debug.WriteLine("CLIENT: Session created");
            return sessionUUID;
        }

        public String SubscribeSession(String sessionUUID)
        {
            streamWriter.WriteLine(String.Format("subscribe_session|{0}", sessionUUID));
            streamWriter.Flush();
            while (true)
            {
                String returnServer = streamReader.ReadLine();
                if (returnServer.StartsWith("subscribe_session_ok"))
                {
                    String[] data = returnServer.Split('|');
                    break;
                }
                else if (returnServer.StartsWith("subscribe_session_error"))
                {
                    throw new Exception("Session create error");
                }
            }
            Debug.WriteLine("CLIENT: Session subscribed");
            return sessionUUID;
        }

        public List<String> GetSubscribers(String sessionUUID)
        {
            List<String> connectedClientsUUID = new List<string>();

            streamWriter.WriteLine(String.Format("get_subscribers|{0}", sessionUUID));
            streamWriter.Flush();
            while (true)
            {
                String returnServer = streamReader.ReadLine();
                if (returnServer.StartsWith("get_subscribers_ok"))
                {
                    String[] data = returnServer.Split('|');
                    String receivedSessionUUID = data[1];
                    if(receivedSessionUUID == sessionUUID)
                    {
                        String message = data[2];
                        String[] messageData = message.Split('#');
                        connectedClientsUUID = messageData.ToList();
                    }
                    break;
                }
                else if (returnServer.StartsWith("get_subscribers_error"))
                {
                    throw new Exception("Get subscribers error");
                }
            }

            Debug.WriteLine("CLIENT: Get subscribers succeded");
            return connectedClientsUUID;
        }

        public List<String> GetSessions()
        {
            List<String> sessionsUUIDs = new List<string>();

            streamWriter.WriteLine(String.Format("get_sessions"));
            streamWriter.Flush();
            while (true)
            {
                String returnServer = streamReader.ReadLine();
                if (returnServer.StartsWith("get_sessions_ok"))
                {
                    String[] data = returnServer.Split('|');
                    String message = data[1];
                    String[] messageData = message.Split('#');
                    sessionsUUIDs = messageData.ToList();
                    break;
                }
                else if (returnServer.StartsWith("get_sessions_error"))
                {
                    throw new Exception("Get sessions error");
                }
            }
            Debug.WriteLine("CLIENT: Get sessions succeded");
            return sessionsUUIDs;
        }

        public String Read()
        {
            String receivedString = "";
            if (isConnected)
            {
                try
                {
                    if (networkStream.DataAvailable)
                    {
                        receivedString = streamReader.ReadLine();
                    }
                    
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Read: Network error - " + ex.Message);
                    isConnected = false;
                }
                
            }
            return receivedString;
        }

        public void Send(String data)
        {
            if (isConnected)
            {
                try
                {
                    streamWriter.WriteLine(data);
                    streamWriter.Flush();
                    Debug.WriteLine("CLIENT: Send: " + data.Replace(Environment.NewLine, ""));
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Send: Network error - " + ex.Message);
                    isConnected = false;
                }
                
                //streamWriter.Flush();
                //string serverReturn = Read();
                //return serverReturn;
            }

            //return "ERROR";
        }

        public void Close()
        {
            networkStream.Close();
        }


    }
}
