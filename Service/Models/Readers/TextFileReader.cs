namespace Service.Models.Readers
{
    public class TextFileReader : IFileReader
    {
        private readonly string _filePath;
        private readonly ILogger _logger;

        public TextFileReader(string filePath)
        {
            _filePath = filePath;
            _logger = Core.GetLogger("TextFileReader");
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
