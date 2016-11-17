using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Chatbook
{
    class Program
    {
        static Thread threadListener;
        static Server server;
        static void Main(string[] args)
        {
            BetterThenSQL context = new BetterThenSQL();
            context.ReadFromBd();
            server = new Server();
            threadListener = new Thread(server.Starting);
            threadListener.Start();
        }
    }
}
