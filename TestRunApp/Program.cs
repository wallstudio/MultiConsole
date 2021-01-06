using System;
using System.Linq;
using System.Threading;

namespace TestRunApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!int.TryParse(args.ElementAtOrDefault(0) ?? "", out var count))
            {
                count = new Random().Next() % 8;
            }
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Console.WriteLine($"[{DateTime.Now:hh:mm:ss.ff}] {i}/{count}");
            }
        }
    }
}
