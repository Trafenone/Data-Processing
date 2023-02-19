using System.Globalization;

namespace Service.Models
{
    public class InputTransaction
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public long AccountNumber { get; set; }
        public string Service { get; set; }

        public static InputTransaction? GetTransaction(string line)
        {
            InputTransaction? transaction = new InputTransaction();

            try
            {
                string[] strings = line.Replace("\"", string.Empty).Split(",", StringSplitOptions.TrimEntries);

                if (strings.Length != 9 || strings.Any(str => string.IsNullOrWhiteSpace(str)))
                    throw new ArgumentException("");

                transaction.FirstName = strings[0];
                transaction.LastName = strings[1];
                transaction.Address = strings[2] + ", " + strings[3] + ", " + strings[4];
                transaction.Payment = decimal.Parse(strings[5], new NumberFormatInfo() { NumberDecimalSeparator = "." });
                transaction.Date = DateTime.ParseExact(strings[6], "yyyy-dd-MM", null);
                transaction.AccountNumber = long.Parse(strings[7]);
                transaction.Service = strings[8];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                transaction = null;
            }

            return transaction;
        }
    }
}
