using System.Diagnostics;

namespace Gems.Utilities
{
    public class Diagnostics
    {
        public static int GetProcessThreadCount()
            => Process.GetCurrentProcess().Threads.Count;

        public static int GetThreadPoolCount()
            => ThreadPool.ThreadCount;

        public static void PrintProcessThreadCount()
        {
            Console.WriteLine($"Number of process threads: {GetProcessThreadCount()}");
        }

        public static void PrintThreadPoolCount()
        {
            Console.WriteLine($"Number of threads in thread pool: {GetThreadPoolCount()}");
        }
    }
}