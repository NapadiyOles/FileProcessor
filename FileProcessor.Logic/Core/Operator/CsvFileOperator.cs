using System.Globalization;
using CsvHelper.Configuration;

namespace FileProcessor.Logic.Core.Operator;

internal sealed class CsvFileOperator : BaseFileOperator
{
    public CsvFileOperator(string source, string result) : base(source, result, FileType.Csv)
    {
        Config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
        };
    }
}