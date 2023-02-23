using Microsoft.Extensions.Logging;
using static FileProcessor.Logic.Utilities.LoggingManager;

namespace FileProcessor.Logic.Core.Operators;

internal sealed class CsvFileOperator : BaseFileOperator
{
    public CsvFileOperator(string folder) : base(folder, FileType.Csv)
    {
    }

    protected override async void WatcherOnCreated(object sender, FileSystemEventArgs e)
    {
        await Task.Run(() =>
        {
            var file = new FileInfo(e.FullPath);
            // Logger.LogInformation("New file detected: {File}", file.Name);
            Logger.LogInformation("Csv event runs on thread {ID}", Thread.CurrentThread.ManagedThreadId);
        });
    }
}