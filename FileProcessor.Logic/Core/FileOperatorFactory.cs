using FileProcessor.Logic.Core.Operator;
using Microsoft.Extensions.Logging;
using static FileProcessor.Logic.Utilities.LoggingManager;

namespace FileProcessor.Logic.Core;

internal sealed class FileOperatorFactory : IDisposable
{
    private readonly string _source, _result;
    private List<BaseFileOperator>? _operators;

    public FileOperatorFactory(string source, string result)
    {
        _source = source;
        _result = result;
    }

    public void InitAll()
    {
        _operators = new List<BaseFileOperator>
        {
            new TxtFileOperator(_source, _result),
            new CsvFileOperator(_source, _result)
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
                    _operators.Add(new TxtFileOperator(_source, _result));
                    break;
                case FileType.Csv:
                    _operators.Add(new CsvFileOperator(_source, _result));
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