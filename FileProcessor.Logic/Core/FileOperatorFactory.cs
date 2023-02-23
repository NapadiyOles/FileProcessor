using FileProcessor.Logic.Core.Operators;
using Microsoft.Extensions.Logging;
using static FileProcessor.Logic.Utilities.LoggingManager;

namespace FileProcessor.Logic.Core;

internal sealed class FileOperatorFactory : IDisposable
{
    private readonly string _path;
    private List<BaseFileOperator>? _operators;

    public FileOperatorFactory(string path)
    {
        _path = path;
    }

    public void InitAll()
    {
        _operators = new List<BaseFileOperator>
        {
            new TxtFileOperator(_path),
            new CsvFileOperator(_path)
        };
    }
    
    public void InitByType(params string[] types)
    {
        _operators = new List<BaseFileOperator>();
        
        foreach (var type in types)
        {
            switch (type)
            {
                case FileType.Txt:
                    _operators.Add(new TxtFileOperator(_path));
                    break;
                case FileType.Csv:
                    _operators.Add(new CsvFileOperator(_path));
                    break;
                default:
                    Logger.LogError("Unsupported file type: {Type}", type);
                    break;
            }    
        }
    }

    public void Dispose()
    {
        if (_operators is null) return;
        
        foreach (var fileOperator in _operators)
        {
            fileOperator.Dispose();
        }
    }
}