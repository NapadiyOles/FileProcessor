using System.Globalization;
using CsvHelper.Configuration;

namespace FileProcessor.Logic.Core.Operator;

internal sealed class TxtFileOperator : BaseFileOperator
{
    public TxtFileOperator(string source, string result) : base(source, result, FileType.Txt)
    {
        Config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            TrimOptions = TrimOptions.Trim,
        };
    }
}