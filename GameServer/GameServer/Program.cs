using System;

// Stolen from https://youtu.be/uh8XaC0Y5MA?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5
namespace GameServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Game Server";

            // TODO: find an appropriate port - watch https://youtu.be/uh8XaC0Y5MA?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5&t=423
            Server.Start(50, 26950);
            Console.ReadKey();
        }
    }
}
