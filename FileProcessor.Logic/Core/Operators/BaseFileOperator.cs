namespace FileProcessor.Logic.Core.Operators;

internal abstract class BaseFileOperator : IDisposable
{
    private readonly FileSystemWatcher _watcher;
    
    protected BaseFileOperator(string folder, string type)
    {
        _watcher = new FileSystemWatcher(folder, type);
        _watcher.Created += WatcherOnCreated;
        _watcher.EnableRaisingEvents = true;
    }

    protected abstract void WatcherOnCreated(object sender, FileSystemEventArgs e);
    
    public void Dispose()
    {
        _watcher.Dispose();
    }
}