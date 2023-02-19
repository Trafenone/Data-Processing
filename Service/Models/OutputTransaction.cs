namespace Service.Models
{
    public class OutputTransaction
    {
        public Transaction[] Transactions { get; set; }
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
