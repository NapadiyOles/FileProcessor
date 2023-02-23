namespace FileProcessor.Logic.Models.Output;

internal class Transaction
{
    public string? City { get; set; }
    public List<Service>? Services { get; set; }
    public decimal Total { get; set; }
}