using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FileProcessor.Logic;

var services = new ServiceCollection();

services
    .AddLogging(c => c.AddConsole())
    .AddSingleton<ProcessingService>();

using (var provider = services.BuildServiceProvider())
{
    var service = provider.GetService<ProcessingService>();
    service?.Start();
}

Console.WriteLine("Press ESC to stop");

//
// while (!(Console.KeyAvailable && 
//          Console.ReadKey(true).Key == ConsoleKey.Q))
// {
//     Console.WriteLine("Doing stuff...");
//     Thread.Sleep(500);
// }
