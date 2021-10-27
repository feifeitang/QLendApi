using System;

namespace QLendApi.Helpers
{
    public class Logger
    {
        public void Info(string Api, string content, object data)
        {
            Console.WriteLine("[{0}] | Info | {1} | {2} | {3}", DateTime.UtcNow, Api, content, data);
        }
        public void Warn(string Api, string content, object data)
        {
            Console.WriteLine("[{0}] | Warn | {1} | {2} | {3}", DateTime.UtcNow, Api, content, data);
        }
        public void Error(string Api, string content, object data)
        {
            Console.WriteLine("[{0}] | Error | {1} | {2} | {3}", DateTime.UtcNow, Api, content, data);
        }
    }
}