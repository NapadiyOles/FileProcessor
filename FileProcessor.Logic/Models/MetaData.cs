namespace FileProcessor.Logic.Models;

internal sealed class MetaData
{
    public int ParsedFiles { get; set; }
    public int ParsedLines { get; set; }
    public int FoundErrors { get; set; }
    public List<string> InvalidFiles { get; set; } = new();
    
    public override string ToString() =>
        $"parsed_files: {ParsedFiles}\n" +
        $"parsed_lines: {ParsedLines}\n" +
        $"found_errors: {FoundErrors}\n" +
        $"invalid_files: [{InvalidFiles
            .Aggregate((a, b) => a + ", " + b)}]";
}