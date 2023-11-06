using Gems.Utilities;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Gems.Flows
{
    /// <summary>
    /// Read items from a db and adds them to a concurrent queue.
    /// Queue items are processed by multiple threads while the db queries continues.
    /// Delays are simulated with Task.Delay(...)
    /// </summary>
    public class ConcurrentQueueFlow
    {
        private readonly ConcurrentQueue<string> _queue = new();
        private bool _readFromDbCompleted = false;
        private int _dbCount = 0;
        private int _processCount = 0;

        private async Task<string> QueryDbItem()
        {
            await Task.Delay(100);
            Interlocked.Increment(ref _dbCount);

            var result = _dbCount + " : " + new Random().Next(0, 1000000).ToString();

            return result;
        }

        private void SpinUpProcessQueue(int threads)
        {
            for (int i = 0; i < threads; i++)
            {
                Task.Run(() => ProcessQueue());
            }
        }

        private async Task ProcessQueue()
        {
            while (true)
            {
                if (_queue.TryDequeue(out string data))
                {
                    await Task.Delay(500);
                    Console.WriteLine($"Processed {data}");
                    Diagnostics.PrintProcessThreadCount();
                    Diagnostics.PrintThreadPoolCount();
                    Console.WriteLine("---");
                    Interlocked.Increment(ref _processCount);
                }
                else
                {
                    if (_readFromDbCompleted)
                        break;
                }
            }
        }

        private async Task CheckIfProcessCompleted()
        {
            while (true)
            {
                if (_dbCount == _processCount)
                    break;

                await Task.Delay(1000);
            }
        }

        public async Task Invoke()
        {
            var sw = new Stopwatch();
            sw.Start();
            SpinUpProcessQueue(threads: 4);

            for (int i = 0; i < 20; i++)
            {
                var item = await QueryDbItem();
                _queue.Enqueue(item);
            }

            _readFromDbCompleted = true;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n Done reading from Db \n");
            Console.ForegroundColor = ConsoleColor.Gray;

            await CheckIfProcessCompleted();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n Done processing queue \n");
            Console.ForegroundColor = ConsoleColor.Gray;

            sw.Stop();
            var elapsed = sw.Elapsed.Seconds;
            Console.WriteLine($"Seconds to complete: {elapsed}");
        }
    }
}