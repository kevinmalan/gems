using Gems.Flows;

namespace Gems
{
    public class Program
    {
        public static async Task Main()
        {
            var validOptions = new string[] { "c", "b" };
            Console.WriteLine("Hello...");
            Console.WriteLine("c: ConcurrentQueue Flow");
            Console.WriteLine("b: Blob Flow");

            var response = Console.ReadLine();
            while (!validOptions.Contains(response))
            {
                Console.WriteLine("Try again...");
                response = Console.ReadLine();
            }

            if (response == "c")
            {
                var flow = new ConcurrentQueueFlow();
                await flow.Invoke();
            }
            else if (response == "b")
            {
                var flow = new BlobFlow();
                await flow.Invoke();
            }
            else
                throw new NotImplementedException();

            await Main();
        }
    }
}