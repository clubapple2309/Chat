using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbook
{
    class BetterThenSQL
    {
        FileInfo f = new FileInfo(@"D:\Test.txt");
        StreamReader reader;
        StreamWriter writer;
        FileStream fs;
        string info;
        public BetterThenSQL(string name,string password)
        {
            if (f.Exists && !Server.cl.Any(x=>x.Key == name))
            {
                info = "#"+ name + ";" + password;
                writer = new StreamWriter(@"D:\Test.txt", true);
                writer.WriteLine(info);
                writer.Close();
                reader = new StreamReader(@"D:\Test.txt", true);
                string test = reader.ReadToEnd();
                Console.WriteLine(test);
                reader.Close();

            }
            else if(!f.Exists)
            {
                fs = f.Create();
                info = "#" + name + ";" + password;
                writer = new StreamWriter(@"D:\Test.txt", true);
                writer.WriteLine(info);
                writer.Close();
                reader = new StreamReader(@"D:\Test.txt", true);
                string test = reader.ReadToEnd();
                Console.WriteLine(test);
                reader.Close();
                fs.Close();
            }
        }
    }
}
