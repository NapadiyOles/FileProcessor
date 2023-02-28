using System.Text.Json;
using FileProcessor.Logic.Models;
using FileProcessor.Logic.Models.Output;

namespace FileProcessor.Logic.Core;

internal sealed class DataWriter
{
    private readonly string _path;

    public DataWriter(string path)
    {
        _path = path;
    }

    public async Task WriteToJsonAsync(IEnumerable<Transaction> transactions)
    {
        await using var stream = File.OpenWrite(_path);
        await JsonSerializer.SerializeAsync(stream, transactions);
    }

    public static void WriteMetaLog(object? meta)
    {
        if(meta is not MetaData data) return;
        using var writer = File.CreateText(MetaData.SubDir + "\\meta.log");
        writer.WriteAsync(data.ToString());
        MetaData.SubDir = MetaData.SubDir?.Parent?
            .CreateSubdirectory(DateOnly.FromDateTime(DateTime.Now).ToShortDateString()) ?? default;
    }
}