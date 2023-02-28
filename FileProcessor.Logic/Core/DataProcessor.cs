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

        // var _ = transactions.GroupBy(
        //     e => e.Address!.Split(',')[0],
        //     e => new
        //     {
        //         Service = e.Service,
        //         Name = $"{e.FirstName} {e.LastName}",
        //         Payment = e.Payment,
        //         Date = DateOnly.ParseExact(e.Date!, "yyyy-dd-MM"),
        //         AccountNumber = e.AccountNumber
        //     },
        //     (city, services) =>
        //     {
        //         var serviceList = services.ToList();
        //         return new Transaction
        //         {
        //             City = city,
        //             Services = serviceList.GroupBy(
        //                 e => e.Service,
        //                 e => new Payer
        //                 {
        //                     Name = e.Name,
        //                     Payment = e.Payment,
        //                     Date = e.Date,
        //                     AccountNumber = e.AccountNumber
        //                 },
        //                 (service, payers) =>
        //                 {
        //                     var list = payers.ToList();
        //                     return new Service
        //                     {
        //                         Name = service,
        //                         Payers = list,
        //                         Total = list.Count
        //                     };
        //                 }).ToList(),
        //             Total = serviceList.Count
        //         };
        //     });
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