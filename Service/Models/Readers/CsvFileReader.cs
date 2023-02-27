namespace Service.Models.Readers
{
    public class CsvFileReader : FileReaderBase
    {
        public CsvFileReader(string filePath) : base(filePath, "CsvFileReader")
        {
        }

        protected override bool ShouldSkipLine(string line)
        {
            return line.Contains("first") || line.Contains("address");
        }
    }
}
