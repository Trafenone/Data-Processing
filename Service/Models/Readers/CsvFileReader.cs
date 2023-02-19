namespace Service.Models.Readers
{
    public class CsvFileReader : IFileReader
    {
        private readonly string _filePath;

        public CsvFileReader(string filePath)
        {
            _filePath = filePath;
        }

        public Task<List<InputTransaction>> ReadFileAsync()
        {
            throw new NotImplementedException();
        }
    }
}
