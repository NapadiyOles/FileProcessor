using FileProcessor.Logic;
using FileProcessor.Logic.Exceptions;

ProcessingService? service = default;

try
{
    service = new ProcessingService();
}
catch (InvalidConfigException)
{
    Console.WriteLine("Quiting...");
    return;
}

Console.WriteLine("Press key to (1: start, 2: stop, 3: restart)");

ConsoleKey prevKey = default;

do
{
    var currKey = Console.ReadKey(true).Key;

    switch (currKey)
    {
        case ConsoleKey.D1:
            if (prevKey == ConsoleKey.D1 || prevKey == ConsoleKey.D3)
            {
                Console.WriteLine("Service is already running");
                continue;
            }
            Console.WriteLine("Starting...");
            service.Start();
            break;
        case ConsoleKey.D2:
            Console.WriteLine("Quiting...");
            service.Dispose();
            return;
        case ConsoleKey.D3:
            Console.WriteLine("Restart...");
            service.Dispose();
            service = new ProcessingService();
            service.Start();
            break;
    }
    prevKey = currKey;
    
} while (true);
