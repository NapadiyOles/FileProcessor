namespace FileProcessor.Logic.Models.Output;

public class Payer
{
    public string? Name { get; set; }
    public decimal Payment { get; set; }
    public DateOnly Date { get; set; }
    public long AccountNumber { get; set; }
}