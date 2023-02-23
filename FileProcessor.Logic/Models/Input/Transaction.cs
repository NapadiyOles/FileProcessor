namespace FileProcessor.Logic.Models.Input;

internal class Transaction
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public decimal Payment { get; set; }
    public DateOnly Date { get; set; }
    public long AccountNumber { get; set; }
    public string? Service { get; set; }
}