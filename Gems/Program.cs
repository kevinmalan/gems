using Gems.Flows;

namespace Gems
{
    public class Program
    {
        public static async Task Main()
        {
            var concurrentQueueFlow = new ConcurrentQueueFlow();
            await concurrentQueueFlow.Invoke();
        }
    }
}