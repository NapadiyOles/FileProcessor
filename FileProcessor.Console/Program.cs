using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FileProcessor.Logic;
using FileProcessor.Logic.Exceptions;

// Console.WriteLine("Press ESC to stop");


ProcessingService? service = default;

try
{
    service = new ProcessingService();
}
catch (InvalidConfigException)
{
    Console.WriteLine("Quiting");
    return;
}

using (service)
{
    service.Start();

    do
    {
        Console.WriteLine("Press Q to quit.");
    } while (Console.ReadKey(true).Key != ConsoleKey.Q);
}

// while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)) ;
Console.WriteLine("Quiting");

//
// while (!(Console.KeyAvailable && 
//          Console.ReadKey(true).Key == ConsoleKey.Q))
// {
//     Console.WriteLine("Doing stuff...");
//     Thread.Sleep(500);
// }
