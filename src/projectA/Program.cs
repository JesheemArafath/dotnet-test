using System;
using System.Threading.Tasks;

namespace projectA
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RunCounterAsync(args);
        }

        static async Task RunCounterAsync(string[] args)
        {   
            Console.WriteLine("Counter starts hear..");
            var counter = 0;
            var max = args.Length != 0 ? Convert.ToInt32(args[0]) : -1;
            while (max == -1 || counter < max)
            {
                Console.WriteLine($"Counter: {++counter}");
                await Task.Delay(TimeSpan.FromSeconds(1)); // Using TimeSpan.FromSeconds instead of TimeSpan.FromMilliseconds
            }
        }
    }
}