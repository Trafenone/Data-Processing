using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.Readers
{
    public abstract class FileReaderBase : IFileReader
    {
        protected readonly ILogger _logger;
        protected readonly string _filePath;

        protected FileReaderBase(string filePath, string loggerName)
        {
            _filePath = filePath;
            _logger = Core.GetLogger(loggerName);
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
                        if (!ShouldSkipLine(line))
                        {
                            currentLine++;
                            var transaction = InputTransaction.GetTransaction(line);

                            if (transaction != null)
                                result.Add(transaction);
                            else
                                invalidLines++;
                        }
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

        protected abstract bool ShouldSkipLine(string line);
    }
}
