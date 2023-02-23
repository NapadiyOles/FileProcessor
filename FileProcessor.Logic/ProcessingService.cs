using FileProcessor.Logic.Core;
using FileProcessor.Logic.Core.Operators;
using FileProcessor.Logic.Utilities;
using Microsoft.Extensions.Logging;
using static FileProcessor.Logic.Utilities.LoggingManager;

namespace FileProcessor.Logic;

public sealed class ProcessingService : IDisposable
{
    private readonly DirectoryInfo _sources, _results;
    private readonly FileOperatorFactory _factory;
    public ProcessingService()
    {
        var path = ConfigManager.GetPath();
        var folders = ProcessFolderManager.GetFolders(path);
        (_sources, _results) = folders;
        _factory = new FileOperatorFactory(_sources.FullName);
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