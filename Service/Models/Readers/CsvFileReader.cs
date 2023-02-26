namespace Service.Models.Readers
{
    public class CsvFileReader : IFileReader
    {
        private readonly ILogger _logger;
        private readonly string _filePath;

        public CsvFileReader(string filePath)
        {
            _filePath = filePath;
            _logger = Core.GetLogger("CsvFileReader");
        }

        public async Task<ReadingResult> ReadFileAsync()
        {
            List<InputTransaction> result = new List<InputTransaction>();

            int invalidLines = 0;
            int currentLine = 0;

            if (File.Exists(_filePath))
            {
                using (StreamReader sr = new StreamReader(_filePath))
                {
                    string? line;

                    _logger.LogInformation($"The Service reading: {_filePath}");

                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        if (line.Contains("first"))
                            continue;

                        currentLine++;

                        var transaction = InputTransaction.GetTransaction(line);

                        if (transaction != null)
                            result.Add(transaction);
                        else
                            invalidLines++;
                    }

                    _logger.LogInformation($"The service stopped reading: {_filePath}");
                }
            }

            return new ReadingResult
            {
                Transactions = result,
                InvalidLines = invalidLines,
                Lines = currentLine,
                NameFile = _filePath
            };
        }
    }
}
