using System;
using System.Threading;

namespace NightMaresServer
{
    class Program
    {
        public static bool isRunning = false;
        static void Main(string[] args)
        {
            Console.Title = "Nightmares Game Server";
            isRunning = true;
            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();
            Server.Start(4, 27005);
        }
        private static void MainThread() 
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second");
            DateTime _nextLoop = DateTime.Now;

            while (isRunning) 
            {
                while( _nextLoop < DateTime.Now)
                {
                    GameLogic.Update();
                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);
                    if(_nextLoop > DateTime.Now) 
                    {
                        Thread.Sleep(_nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}
