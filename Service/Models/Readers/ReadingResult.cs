namespace Service.Models.Readers
{
    public class ReadingResult
    {
        public int InvalidLines { get; set; }
        public int Lines { get; set; }
        public string NameFile { get; set; }
        public List<InputTransaction> Transactions { get; set; }
    }
}
