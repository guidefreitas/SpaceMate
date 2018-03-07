using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using SpaceMate.Common;
using System.Diagnostics;

namespace GameServerSync
{
    public class GameServer
    {
        public List<GameSession> Sessions { get; set; }
        public List<Client> Clients { get; set; }

        private TcpListener tcpListener;
        
        //Variaveis de controle (lock) para concorrência entre as threads
        private System.Object sessionsLock = new System.Object();
        private System.Object clientsLock = new System.Object();

        private String serverIp = "";
        private Int32 serverPort = 0;

        private bool done = false;

        public GameServer(String ip, int port)
        {
            serverIp = ip;
            serverPort = port;
            Sessions = new List<GameSession>();
            Clients = new List<Client>();
            
            //Cria o socket e faz o binding no ip e porta especificados
            tcpListener = new TcpListener(IPAddress.Parse(ip), port);

        }
        
        public void CreateSession(String command, Client client)
        {
            try
            {
                String[] data = command.Split('|');
                GameSession session = new GameSession();
                session.Name = data[1];
                lock (sessionsLock)
                {
                    session.Subscribers.Add(client);
                    Sessions.Add(session);
                }
                Program.serverScreen.ShowMessage(String.Format("Session created: UUID: {0}, Name: {1}", session.UUID, session.Name));
                String returnData = String.Format("create_session_ok|{0}", session.UUID);
                client.StreamWriter.WriteLine(returnData);
                client.StreamWriter.Flush();
            }
            catch (Exception ex)
            {
                client.StreamWriter.WriteLine("create_session_error|{0}", ex.Message);
                client.StreamWriter.Flush();
            }
        }

        public void SubscribeSession(String command, Client client, Socket socketForClient)
        {
            try
            {
                String[] data = command.Split('|');
                String sessionUUID = data[1];
                GameSession session = Sessions.Where(m => m.UUID == sessionUUID).FirstOrDefault();

                if(session == null)
                {
                    client.StreamWriter.WriteLine("subscribe_session_error|Invalid session UUID");
                    client.StreamWriter.Flush();
                    return;
                }

                lock (sessionsLock)
                {
                    if (session.Subscribers.Contains(client))
                        return;

                    session.Subscribers.Add(client);
                }

                
                //Avisa todos os inscritos que um novo cliente se inscreveu
                foreach (var clientSub in session.Subscribers)
                {
                    if (clientSub != client)
                    {
                        NetworkStream subsNetworkStream = new NetworkStream(clientSub.Socket);
                        System.IO.StreamWriter subsStreamWriter = new System.IO.StreamWriter(subsNetworkStream);
                        subsStreamWriter.WriteLine("new_subscriber|{0}|{1}", session.UUID, client.UUID);
                        subsStreamWriter.Flush();
                    }
                }


                Program.serverScreen.ShowMessage(String.Format("Client {0} subscribed to session {1}", socketForClient.RemoteEndPoint, sessionUUID));
                client.StreamWriter.WriteLine("subscribe_session_ok");
                client.StreamWriter.Flush();
            }
            catch (Exception ex)
            {
                client.StreamWriter.WriteLine("subscribe_session_error|{0}", ex.Message);
                client.StreamWriter.Flush();
            }
        }


        public void GetSessions(String command, Client client, Socket socketForClient)
        {
            try
            {
                String[] data = command.Split('|');

                if (Sessions.Count == 0)
                {
                    client.StreamWriter.WriteLine("get_sessions_error|No sessions on the server");
                    client.StreamWriter.Flush();
                    return;
                }

                String sessionsData = "";

                for (int i = 0; i < Sessions.Count; i++)
                {
                    sessionsData += Sessions.ElementAt(i).UUID;
                    if (i != Sessions.Count - 1)
                    {
                        sessionsData += "#";
                    }
                }

                client.StreamWriter.WriteLine($"get_sessions_ok|{sessionsData}");
                client.StreamWriter.Flush();
            }
            catch (Exception ex)
            {
                client.StreamWriter.WriteLine("get_sessions_error|{0}", ex.Message);
                client.StreamWriter.Flush();
            }
        }

        public void GetSubscribers(String command, Client client, Socket socketForClient)
        {
            try
            {
                String[] data = command.Split('|');
                String sessionUUID = data[1];
                GameSession session = Sessions.Where(m => m.UUID == sessionUUID).FirstOrDefault();

                if (session == null)
                {
                    client.StreamWriter.WriteLine("get_subscribers_error|Invalid session UUID");
                    client.StreamWriter.Flush();
                    return;
                }

                String subsData = "";
                
                for(int i = 0;i< session.Subscribers.Count; i++)
                {
                    subsData += session.Subscribers.ElementAt(i).UUID;
                    if(i != session.Subscribers.Count - 1)
                    {
                        subsData += "#";
                    }
                }

                client.StreamWriter.WriteLine($"get_subscribers_ok|{sessionUUID}|{subsData}");
                client.StreamWriter.Flush();
            }
            catch (Exception ex)
            {
                client.StreamWriter.WriteLine("subscribe_session_error|{0}", ex.Message);
                client.StreamWriter.Flush();
            }
        }

        public void UnsubscribeSession(String command, Client client, Socket socketForClient)
        {
            try
            {
                String[] data = command.Split('|');
                String sessionUUID = data[1];
                GameSession session = Sessions.Where(m => m.UUID == sessionUUID).FirstOrDefault();
                if (session == null)
                {
                    client.StreamWriter.WriteLine("unsubscribe_session_error|Invalid session UUID");
                    client.StreamWriter.Flush();
                    return;
                }

                if (session.Subscribers.Contains(client))
                {
                    lock (sessionsLock)
                    {
                        session.Subscribers.Remove(client);
                    }

                    Program.serverScreen.ShowMessage(String.Format("Client {0} unsubscribed to session {1}", socketForClient.RemoteEndPoint, sessionUUID));
                    client.StreamWriter.WriteLine("unsubscribe_session_ok");
                    client.StreamWriter.Flush();
                }
                else
                {
                    client.StreamWriter.WriteLine("unsubscribe_session_error|Client not subscribed to session UUID {0}", sessionUUID);
                    client.StreamWriter.Flush();
                    return;
                }
            }
            catch (Exception ex)
            {
                client.StreamWriter.WriteLine("unsubscribe_session_error|{0}", ex.Message);
                client.StreamWriter.Flush();
            }
        }

        public void SendData(String command, Client client)
        {
            try
            {
                String[] data = command.Split('|');
                String sessionUUID = data[1];
                String senderClientUUID = data[2];
                String message = command.Replace(String.Format("send_data|{0}|{1}|", sessionUUID, senderClientUUID), "");
                var session = Sessions.Where(m => m.UUID == sessionUUID).FirstOrDefault();
                if (session == null)
                {
                    //Não existe nenhuma sessão com o UUID informado
                    client.StreamWriter.WriteLine("send_data_error|Invalid session UUID");
                    client.StreamWriter.Flush();
                    return;
                }

                if (!session.Subscribers.Contains(client))
                {
                    //O usuário que enviou a informação não está inscrito na sessao informada
                    client.StreamWriter.WriteLine("send_data_error|Client not subscribed to this session");
                    client.StreamWriter.Flush();
                    return;
                }

                foreach (var clientSub in session.Subscribers)
                {
                    //Envia os dados para todos os inscritos exceto o cliente que enviou o dado originalmente
                    if (clientSub != client)
                    {
                        clientSub.StreamWriter.WriteLine("data_received|{0}|{1}|{2}", sessionUUID, senderClientUUID, message);
                        clientSub.StreamWriter.Flush();
                    }
                    
                    //streamWriter.WriteLine("send_data_ok|{0}", sessionUUID);
                }

                
            }
            catch (Exception ex)
            {
                client.StreamWriter.WriteLine("send_data_error|{0}", ex.Message);
                client.StreamWriter.Flush();
            }
        }

        public void ExitClient(Client client)
        {
            lock (clientsLock)
            {
                Clients.Remove(client);
                foreach (var session in Sessions)
                {
                    foreach (var clientSub in session.Subscribers)
                    {
                        if (clientSub == client)
                        {
                            session.Subscribers.Remove(client);
                        }
                    }
                }
            }

        }

        public void Listeners(Socket socketForClient)
        {

            if (socketForClient.Connected)
            {
                //Se um novo cliente se conectar cria um novo objeto do tipo Client
                //com um identificador único e associa o socket aberto a esse objeto
                Client client = new Client(socketForClient);

                lock (clientsLock)
                {
                    Clients.Add(client);
                }

                Program.serverScreen.ShowMessage("Client:" + socketForClient.RemoteEndPoint + " now connected to server.");
                NetworkStream networkStream = new NetworkStream(socketForClient);


                //Retorna para o cliente que acabou de se conectar a sua UUID
                client.StreamWriter.WriteLine("connect_ok|{0}", client.UUID);
                client.StreamWriter.Flush();

                
                while (true)
                {
                    string command = "";

                    try
                    {
                        command = client.StreamReader.ReadLine();
                        if (String.IsNullOrEmpty(command))
                            continue;

                        Program.serverScreen.ShowMessage("SERVER: Message recieved by client:" + command);
                    }catch(Exception ex)
                    {
                        Program.serverScreen.ShowMessage(String.Format("SERVER: Error: {0}", ex.Message));
                        break;
                    }



                    if (command.StartsWith("create_session"))
                    {
                        CreateSession(command, client);
                    }
                    else if (command.StartsWith("subscribe_session"))
                    {
                        SubscribeSession(command, client, socketForClient);

                    }
                    else if (command.StartsWith("unsubscribe_session"))
                    {
                        UnsubscribeSession(command, client, socketForClient);

                    }
                    else if (command.StartsWith("get_subscribers"))
                    {
                        GetSubscribers(command, client, socketForClient);
                    }else if (command.StartsWith("get_sessions"))
                    {
                        GetSessions(command, client, socketForClient);
                    }
                    else if (command.StartsWith("send_data"))
                    {
                        SendData(command, client);
                    }
                    /* else if (command == "exit")
                    {
                        ExitClient(client);
                        break;
                    }*/
                    else
                    {
                        client.StreamWriter.WriteLine("ERROR|Invalid command");
                        client.StreamWriter.Flush();
                    }
                        
                }

                /*
                try
                {
                    streamReader.Close();
                    networkStream.Close();
                    streamWriter.Close();
                }
                catch { }
                */
            }

            Program.serverScreen.ShowMessage("SERVER: Client:" + socketForClient.RemoteEndPoint + " disconected from the server.");
            socketForClient.Close();
            
            //Console.ReadKey();
        }

        public void Start()
        {
            tcpListener.Start();
            Program.serverScreen.ShowMessage(String.Format("SERVER: Server started at {0}:{1}", serverIp, serverPort));

            while (!done)
            {
                if (tcpListener.Pending())
                {
                    Socket socket = tcpListener.AcceptSocket();
                    ConfigureSocket.ConfigureTcpSocket(socket);
                    Program.serverScreen.ShowMessage("SERVER: Client connected");
                    Thread newThread = new Thread(() => Listeners(socket));
                    newThread.Start();
                }
                
            }

            Program.serverScreen.ShowMessage("SERVER: Server finished");
            tcpListener.Stop();
        }

        public void Stop()
        {
            done = true;
        }


    }
}
