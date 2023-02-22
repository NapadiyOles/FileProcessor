namespace FileProcessor.Logic.Models.Output;

public class Service
{
    public string? Name { get; set; }
    public List<Payer>? Payers { get; set; }
    public decimal Total { get; set; }
}