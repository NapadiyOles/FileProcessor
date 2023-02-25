using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using FileProcessor.Logic.Models.Input;
using Microsoft.Extensions.Logging;
using static FileProcessor.Logic.Utilities.LoggingManager;

namespace FileProcessor.Logic.Core.Operators;

internal sealed class TxtFileOperator : BaseFileOperator
{
    public TxtFileOperator(string folder) : base(folder, FileType.Txt)
    {
        Config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            TrimOptions = TrimOptions.Trim,
        };
    }

    protected override async void WatchOnCreated(object sender, FileSystemEventArgs e)
    {
        await Task.Run(() =>
        {
            var file = new FileInfo(e.FullPath);

            // Logger.LogInformation("Txt event runs on thread {ID}", Thread.CurrentThread.ManagedThreadId);
            
            using var stream = file.OpenText();
            using var csv = new CsvReader(stream, Config);
            csv.Context.RegisterClassMap<TransactionMap>();

            var list = new List<Transaction>();

            var parser = csv.Parser;
            while (csv.Read())
            {
                try
                {
                    var record = csv.GetRecord<Transaction>();
                }
                catch (ValidationException)
                {
                    Logger.LogError("Invalid record at {Index}: {Row}",
                        parser.Row, parser.RawRecord);
                    continue;
                }
                
                Logger.LogInformation("Valid record at   {Index}: {Row}",
                    parser.Row, parser.RawRecord);
            }
        });
    }
}