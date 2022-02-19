using System;
using System.Threading;

// Stolen from https://youtu.be/uh8XaC0Y5MA?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5
namespace GameServer
{
    class Program
    {
        private static bool isRunning = false;
        public static void Main(string[] args)
        {
            Console.Title = "Game Server";
            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            // TODO: find an appropriate port - watch https://youtu.be/uh8XaC0Y5MA?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5&t=423
            Server.Start(50, 26950);
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main threa started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    GameLogic.Update();

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (_nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(_nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}
