using FileProcessor.Logic.Core;
using FileProcessor.Logic.Utilities;

namespace FileProcessor.Logic;

public sealed class ProcessingService : IDisposable
{
    // private readonly DirectoryInfo _sources, _results;
    private readonly FileOperatorFactory _factory;
    public ProcessingService()
    {
        var cfg = ConfigManager.GetConfig();
        var (sources, results) = ProcessFolderManager.GetFolders(cfg);
        _factory = new FileOperatorFactory(sources.FullName, results.FullName);
    }

    public void Start()
    {
        _factory.InitByType(FileType.Txt, FileType.Csv);
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}