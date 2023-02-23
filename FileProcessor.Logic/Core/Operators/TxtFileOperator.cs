using Microsoft.Extensions.Logging;
using static FileProcessor.Logic.Utilities.LoggingManager;

namespace FileProcessor.Logic.Core.Operators;

internal sealed class TxtFileOperator : BaseFileOperator
{
    public TxtFileOperator(string folder) : base(folder, FileType.Txt)
    {
    }

    protected override async void WatcherOnCreated(object sender, FileSystemEventArgs e)
    {
        await Task.Run(() =>
        {
            var file = new FileInfo(e.FullPath);
            // Logger.LogInformation("New file detected: {File}", file.Name);
            Logger.LogInformation("Txt event runs on thread {ID}", Thread.CurrentThread.ManagedThreadId);
        });
    }
}