using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace ChatbookUser
{
    class User
    {
        private string Ip = "127.251.251.5";
        private int port = 8888;
        string regOrLogin, userName, userPassword, checkPassword;
        string info;
        string message;
        TcpClient client;
        NetworkStream stream;
        StreamWriter writer;
        StreamReader reader;
        Thread reciveThread;

        public void ChatStart()
        {
            try
            {
                client = new TcpClient();
                client.Connect(Ip, port);
                stream = client.GetStream();
                Console.WriteLine("Добро пожаловать в чат");
                Chating();
                SendMessage();
                Console.ReadLine();
            }
            catch
            {
                Console.WriteLine("Сервер не работает . . .");
                Console.ReadLine();
            }
        }
        public void Chating()
        {
            try
            {
                do
                {
                    Console.WriteLine("Нажмите Y что бы залогиниться. N для регистрации. (Y or N) ");
                    regOrLogin = Console.ReadLine();
                    if (regOrLogin == "Y" || regOrLogin == "y" || message == "regSuccess")
                    {
                        Console.WriteLine("Авторизуйтесь пожалуйста ");
                        Console.WriteLine("Ведите логин");
                        userName = Console.ReadLine();
                        Console.WriteLine("Ведите пароль");
                        userPassword = Console.ReadLine();
                        info = "#log" + ";" + userName + ";" + userPassword;
                        regOrLogin = "";
                    }
                    else
                    {
                        Console.WriteLine("Регестрация . . .");
                        Console.WriteLine("Ведите логин");
                        userName = Console.ReadLine();
                        Console.WriteLine("Ведите пароль");
                        userPassword = Console.ReadLine();
                        Console.WriteLine("Повторите пароль еще раз ");
                        checkPassword = Console.ReadLine();
                        if (!ChekPass(userPassword, checkPassword))
                        {
                            Console.WriteLine("Пароли не совпадают ");
                            continue;
                        }
                        info = "#reg" + ";" + userName + ";" + userPassword;
                    }
                    writer = new StreamWriter(stream);
                    info = info.ToLower();
                    writer.WriteLine(info);
                    writer.Flush();
                    reader = new StreamReader(stream);
                    message = reader.ReadLine();
                    Console.WriteLine(message);
                } while (message != "success");
                reciveThread = new Thread(ReadMessage);
                reciveThread.Start();
                Console.Clear();
                Console.WriteLine("Привет {0} ,теперь ты можешь общаться =)", userName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        void ReadMessage()
        {
            while (true)
            {
                reader = new StreamReader(stream);
                message = reader.ReadLine();
                Console.WriteLine(message);
            }
        }

        private void SendMessage()
        {
            Console.WriteLine("Введите сообщение");
            while (true)
            {
                string message = "#mes" + ";" + userName + ";";
                message += Console.ReadLine();
                writer = new StreamWriter(stream);
                writer.WriteLine(message);
                writer.Flush();
            }
        }

        private bool ChekPass(string userPassword, string checkPassword)
        {
            return userPassword == checkPassword;
        }

        void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            Environment.Exit(0);
        }
    }
}
