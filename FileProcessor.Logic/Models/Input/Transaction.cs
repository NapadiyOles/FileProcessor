using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace FileProcessor.Logic.Models.Input;

internal class Transaction
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public decimal Payment { get; set; }

    public string Date { get; set; }

    public long AccountNumber { get; set; }

    public string? Service { get; set; }
}

internal sealed class TransactionMap : ClassMap<Transaction>
{
    public TransactionMap()
    {
        Map(e => e.FirstName).Index(0)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));

        Map(e => e.LastName).Index(1)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));

        Map(e => e.Address).Index(2)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));

        Map(e => e.Payment).Index(3)
            .Validate(args => float.TryParse(args.Field, new CultureInfo("en-US"), out _));

        Map(e => e.Date).Index(4)
            .Validate(args => DateOnly.TryParseExact(args.Field, "yyyy-dd-MM", out _));

        Map(e => e.AccountNumber).Index(5)
            .Validate(args => long.TryParse(args.Field, out _));

        Map(e => e.Service).Index(6)
            .Validate(args => !string.IsNullOrWhiteSpace(args.Field));
    }
}