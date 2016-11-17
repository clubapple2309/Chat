using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace Chatbook
{
    class User : Client
    {
        BetterThenSQL sql;

        public User(TcpClient client, Server server, string name, string password, StreamWriter writer, StreamReader reader) : base(client, server)
        {
            Reader = reader;
            Writer = writer;
            Client = client;
            Server = server;
            Name = name;
            Password = password;
            Server.clients.Add(this);
            sql = new BetterThenSQL(name, password);
        }
        public Server Server;
        public TcpClient Client;
        public string Name { get; set; }
        public string Password { get; set; }
        public StreamWriter Writer;
        public StreamReader Reader;


    }
}
