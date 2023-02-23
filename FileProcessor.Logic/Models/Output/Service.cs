namespace FileProcessor.Logic.Models.Output;

internal class Service
{
    public string? Name { get; set; }
    public List<Payer>? Payers { get; set; }
    public decimal Total { get; set; }
}