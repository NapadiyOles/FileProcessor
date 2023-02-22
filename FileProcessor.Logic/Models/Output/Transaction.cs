namespace FileProcessor.Logic.Models.Output;

public class Transaction
{
    public string? City { get; set; }
    public List<Service>? Services { get; set; }
    public decimal Total { get; set; }
}