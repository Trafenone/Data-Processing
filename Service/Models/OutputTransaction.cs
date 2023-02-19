namespace Service.Models
{
    public static class OutputTransaction
    {
        public static List<Transaction> Transform(List<InputTransaction> transactions)
        {
            List<Transaction> result = new List<Transaction>();
            List<Service> listServices = new List<Service>();
            List<Payer> listPayers = new List<Payer>();

            var groupCities = transactions.GroupBy(x => x.Address.Split(",")[0]);

            foreach (var c in groupCities)
            {
                Transaction transaction = new();
                listServices.Clear();
                transaction.City = c.Key;
                transaction.Total = c.Sum(x => x.Payment);

                var groupServices = c.GroupBy(x => x.Service);

                foreach (var s in groupServices)
                {
                    Service service = new();
                    listPayers.Clear();
                    service.Name = s.Key;
                    service.Total = s.Sum(x => x.Payment);

                    foreach (var payer in s)
                        listPayers.Add(new Payer()
                        {
                            Name = payer.FirstName + " " + payer.LastName,
                            Paymant = payer.Payment,
                            Date = payer.Date,
                            AccountNumber = payer.AccountNumber
                        });

                    service.Payers = listPayers.ToArray();
                    listServices.Add(service);
                }
                transaction.Services = listServices.ToArray();
                result.Add(transaction);
            }

            return result;
        }
    }

    public class Transaction
    {
        public string City { get; set; }
        public Service[] Services { get; set; }
        public decimal Total { get; set; }
    }

    public class Service
    {
        public string Name { get; set; }
        public Payer[] Payers { get; set; }
        public decimal Total { get; set; }
    }

    public class Payer
    {
        public string Name { get; set; }
        public decimal Paymant { get; set; }
        public DateTime Date { get; set; }
        public long AccountNumber { get; set; }
    }
}
