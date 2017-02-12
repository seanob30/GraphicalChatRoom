using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Program
    {
        public static ChatService server;

        static void Main(string[] args)
        {
            server = new ChatService();
            using (ServiceHost host = new ServiceHost(server))
            {
                host.Open();
                Console.WriteLine("Server is functional..");
                Console.ReadLine();
            }
        }
    }
}
