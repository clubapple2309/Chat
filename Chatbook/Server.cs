using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Chatbook
{
    class Server
    {
        TcpListener listener;
        TcpClient client;
        static StreamReader reader;
        public static List<User> clients = new List<User>();
        public static Dictionary<string, string> cl = new Dictionary<string, string>();

        internal void Starting()
        {
            try
            {
            listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();
            Console.WriteLine("Сервер запущен. Ожидает подключений . . .");
            Server server = new Server();
            server.Listener(listener);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }
        internal static void ReadFromBd()
        {
            try
            {
                int i = 0;
                reader = new StreamReader(@"D:\Test.txt", true);
                string test = reader.ReadToEnd();
                foreach (string user in test.Split('#'))
                {
                    if (user != "")
                    {
                        cl.Add(user.Split(';')[0], user.Split(';')[1].TrimEnd());
                    }
                }
                Console.WriteLine(test);
                reader.Close();
            }
            catch { }
        }

        protected void Listener(TcpListener listener)
        {
            try
            {
                while (true)
            {
                client=listener.AcceptTcpClient();
                Client newClient = new Client(client,this);
                Thread clientThread = new Thread(newClient.ListenerForClient);
                clientThread.Start();
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        internal static void RemoveConnection(string name)
        {
           User client = clients.FirstOrDefault(c => c.Name == name);
            if (client != null)
                clients.Remove(client);
        }

        public static void SendMessage(string message,string name)
        {
            try
            {
            for (int i = 0; i < clients.Count; i++)
            {
                if(clients[i].Name != name)
                {
                clients[i].Writer.WriteLine("[ "+name+" ] "+message);
                clients[i].Writer.Flush();
                }
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Server s = new Server();
                s.Disconnect();
            }
        }

        public string[] SplitAnswer(string mes)
        {
            string[] answer= new string[3];
            int i = 0;
            foreach (var s in mes.Split(';'))
            {
                answer[i] = s;
                i++;
            }
            return answer;
        }

        public void Disconnect()
        {
            listener.Stop();
            for(int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
                
            }
        }
    }

