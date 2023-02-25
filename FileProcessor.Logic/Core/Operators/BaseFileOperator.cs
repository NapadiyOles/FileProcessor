using CsvHelper;
using CsvHelper.Configuration;

namespace FileProcessor.Logic.Core.Operators;

internal abstract class BaseFileOperator : IDisposable
{
    private readonly FileSystemWatcher _watcher;
    public CsvConfiguration? Config { get; init; }    
    protected BaseFileOperator(string folder, string type)
    {
        _watcher = new FileSystemWatcher(folder, type);
        _watcher.Created += WatchOnCreated;
        _watcher.EnableRaisingEvents = true;
    }

    protected abstract void WatchOnCreated(object sender, FileSystemEventArgs e);

    protected bool RowIsValid(IReaderRow row)
    {
        if (!row.TryGetField<string>(0, out var firstName))
            return false;

        if (string.IsNullOrWhiteSpace(firstName))
            return false;

        if (!row.TryGetField<string>(1, out var lastName))
            return false;
        
        if (string.IsNullOrWhiteSpace(lastName))
            return false;
        
        if (!row.TryGetField<string>(2, out _))
            return false;

        if (!row.TryGetField<decimal>(3, out _))
            return false;
        
        if (!row.TryGetField<string>(4, out var date))
            return false;

        if (!DateOnly.TryParseExact(date, "yyyy-dd-MM", out _))
            return false;
        
        if (!row.TryGetField<long>(5, out _))
            return false;
        
        if (!row.TryGetField<string>(6, out _))
            return false;

        return true;
    }
    
    public void Dispose()
    {
        _watcher.Dispose();
    }
}