using CsvHelper;
using CsvHelper.Configuration;
using FileProcessor.Logic.Models;
using FileProcessor.Logic.Models.Input;
using Microsoft.Extensions.Logging;
using static FileProcessor.Logic.Utilities.LoggingManager;


namespace FileProcessor.Logic.Core.Operator;

internal abstract class BaseFileOperator : IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private readonly DirectoryInfo _result;
    private DirectoryInfo _subdir;
    private readonly Timer _timer;
    private readonly MetaData _meta;
    
    protected CsvConfiguration? Config { get; init; }
    
    protected BaseFileOperator(string source, string result, string type)
    {
        _result = new DirectoryInfo(result);
        _subdir = _result.CreateSubdirectory(DateOnly.FromDateTime(DateTime.Now).ToShortDateString());
        _meta = new MetaData();
        _watcher = new FileSystemWatcher(source, type);
        _watcher.Created += WatchOnCreated;
        _watcher.EnableRaisingEvents = true;

        _timer = new Timer(TimerCallback, _meta,
            DateTime.Today.AddDays(1) - DateTime.Now, TimeSpan.FromDays(1));
    }
    
    private void TimerCallback(object? meta)
    {
        _subdir = _result.CreateSubdirectory(DateOnly.FromDateTime(DateTime.Now).ToShortDateString());
        DataWriter.WriteMetaLog(meta, _subdir.FullName);
    }

    private async void WatchOnCreated(object sender, FileSystemEventArgs e)
    {
        var result = await Operate(e.FullPath);

        if (!result.success)
        {
            _meta.InvalidFiles.Add(e.FullPath);
            return;
        }

        _meta.FoundErrors += result.errors;
        _meta.ParsedLines += result.parsed;
        _meta.ParsedFiles++;
    }

    protected virtual async Task<(bool success, int parsed, int errors)> Operate(string file)
    {
        var processor = new DataProcessor();
        var writer = new DataWriter(_subdir + $"\\{Guid.NewGuid()}.json");

        var reader = TryStartStream(file);
        if (reader is null) return default;
            
        var (parsed, errors) = await ReadFileAsync(reader, processor);
        File.Delete(file);

        var transactions = processor.GetTransactions();
        await writer.WriteToJsonAsync(transactions);

        return (true, parsed, errors);
    }

    private async Task<(int parsed, int errors)> ReadFileAsync(StreamReader? sr, DataProcessor processor)
    {
        (int parsed, int errors) results = default;
        using (sr)
        {
            using var reader = new CsvReader(sr, Config);
            reader.Context.RegisterClassMap<TransactionMap>();

            while (await reader.ReadAsync())
            {
                TransactionCsv record;
                try
                {                    
                    record = reader.GetRecord<TransactionCsv>()!;
                }
                catch (ValidationException)
                {
                    results.errors++;
                    continue;
                }

                results.parsed++;
                await processor.ProcessAsync(record);
            }
        }

        return results;
    }

    private StreamReader? TryStartStream(string path)
    {
        double repeats = 0;
        do
        {
            try
            {
                return new StreamReader(path);
            }
            catch (IOException)
            {
                Thread.Sleep(250);
                repeats++;
            }
        } while (repeats < 5);

        Logger.LogError("Unable to read file {File}", path);
        return null;
    } 

    public void Dispose()
    {
        DataWriter.WriteMetaLog(_meta, _subdir.FullName);
        _watcher.Dispose();
        _timer.Dispose();
    }
}