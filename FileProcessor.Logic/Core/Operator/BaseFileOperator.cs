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
    private readonly Timer _timer;
    private readonly MetaData _meta;
    private object _lock;
    protected CsvConfiguration? Config { get; init; }

    protected BaseFileOperator(string source, string result, string type)
    {
        _lock = new object();
        _result = new DirectoryInfo(result);
        _meta = new MetaData();
        MetaData.SubDir = _result.CreateSubdirectory(DateOnly.FromDateTime(DateTime.Now).ToShortDateString());
        _watcher = new FileSystemWatcher(source, type);
        _watcher.Created += WatchOnCreated;
        _watcher.EnableRaisingEvents = true;
        
        _timer = new Timer(DataWriter.WriteMetaLog, _meta,
            DateTime.Today.AddDays(1) - DateTime.Now, TimeSpan.FromDays(1));
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
        var writer = new DataWriter(MetaData.SubDir + $"\\{Guid.NewGuid()}.json");

        var reader = TryStartStream(file);
        if (reader is null) return default;
            
        var invalid = await ReadFileAsync(reader, processor);
        File.Delete(file);

        var transactions = processor.GetTransactions();
        await writer.WriteToJsonAsync(transactions);

        return (true, 0, 0);
    }

    private async Task<int> ReadFileAsync(StreamReader? sr, DataProcessor processor)
    {
        var invalid = 0;
        using (sr)
        {
            using var reader = new CsvReader(sr, Config);
            reader.Context.RegisterClassMap<TransactionMap>();
            var parser = reader.Parser;

            while (await reader.ReadAsync())
            {
                TransactionCsv record;
                try
                {                    
                    record = reader.GetRecord<TransactionCsv>()!;
                }
                catch (ValidationException)
                {
                    // Logger.LogError("Invalid {Type} record at {Index}: {Row} \n" +
                    //                 "with error: {Error}",
                    //     _type, parser.Row, parser.RawRecord, ex.Message);
                    invalid++;
                    continue;
                }

                // Logger.LogInformation("Valid {Type} record at   {Index}: {Row}",
                //     _type, parser.Row, parser.RawRecord);

                await processor.ProcessAsync(record);
            }
        }

        return invalid;
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
        _watcher.Dispose();
        _timer.Dispose();
    }
}