using FileProcessor.Logic.Models.Input;
using FileProcessor.Logic.Models.Output;

namespace FileProcessor.Logic.Core;

internal sealed class DataProcessor
{
    private Dictionary<string, Dictionary<string, List<Payer>>> _records;

    public DataProcessor()
    {
        _records = new();
    }

    public async Task ProcessAsync(TransactionCsv rec)
    {
        await Task.Run(() =>
        {
            var city = rec.Address!.Split(',')[0];
            var service = rec.Service!;

            if (!_records.ContainsKey(city))
            {
                _records.Add(city, new());
            }

            if (!_records[city].ContainsKey(service))
            {
                _records[city].Add(service, new());
            }

            _records[city][service].Add(new Payer
            {
                Name = $"{rec.FirstName} {rec.LastName}",
                Payment = rec.Payment,
                Date = DateOnly.ParseExact(rec.Date!, "yyyy-dd-MM"),
                AccountNumber = rec.AccountNumber
            });
        });
    }
    
    public IEnumerable<Transaction> GetTransactions()
    {
        return _records.Select(city => new Transaction
        {
            City = city.Key,
            Services = city.Value.Select(service => new Service
            {
                Name = service.Key,
                Payers = service.Value,
                Total = service.Value.Count
            }),
            Total = city.Value.Count
        });
    }
}