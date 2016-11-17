using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Chatbook
{
    class Client
    {
        TcpClient client;
        Server server;
        NetworkStream stream;
        StreamReader reader;
        StreamWriter writer;
        User user;
        string clientName;

        public Client(TcpClient client, Server server)
        {
            this.client = client;
            this.server = server;
        }

        public void ListenerForClient()
        {
            try
            {
                stream = client.GetStream();
                while (true)
                {
                    string[] answer = new string[3];
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream);
                    string message = reader.ReadLine();
                    answer = server.SplitAnswer(message);
                    clientName = answer[1];
                    if (answer[0] == "#log")
                    {
                        Login(answer);
                        continue;
                    }
                    else if (answer[0] == "#reg")
                    {
                        Registration(answer);
                        continue;
                    }
                    else if (answer[0] == "#mes")
                    {
                        Console.WriteLine(answer[2]);
                        Server.SendMessage(answer[2], clientName);
                        continue;
                    }
                    Console.WriteLine("Обрабатываеться {0} ", answer[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Server.SendMessage(clientName + " отключился от чата ", clientName);
            }
            finally
            {
                Server.RemoveConnection(this.clientName);
                Close();
            }
        }

        public void Registration(string[] info)
        {
            if (Server.clients.Count == 0 || Server.clients.Any(x => x.Name != info[1]))
            {
                user = new User(client, server, info[1], info[2], writer, reader);
                writer.WriteLine("Регистрация прошла успешно");
            }
            else
            {
                writer.WriteLine("Такой логин уже существует ");
            }
            writer.Flush();
        }

        public void Login(string[] info)
        {
            bool loginSuccess = false;

            foreach (var client1 in BetterThenSQL.dateClients)
            {
                if (client1.Key == info[1] && client1.Value == info[2])
                {
                    user = new User(client, server, info[1], info[2], writer, reader);
                    loginSuccess = true;
                }
            }
            if (!loginSuccess)
            {
                foreach (var client1 in Server.clients)
                {
                    if (client1.Name == info[1] && client1.Password == info[2])
                    {
                        loginSuccess = true;
                    }

                }
            }
            if (loginSuccess)
            {
                writer.WriteLine("success");
                writer.Flush();
                Server.SendMessage(info[1] + " подключился к чату ", info[1]);
            }
            else
            {
                writer.WriteLine("Не верный логин или пароль ...");
                writer.Flush();
            }
        }

        public void Close()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }

    }
}
