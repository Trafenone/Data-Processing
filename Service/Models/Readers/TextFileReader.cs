namespace Service.Models.Readers
{
    public class TextFileReader : IFileReader
    {
        private readonly string _filePath;

        public TextFileReader(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<InputTransaction>> ReadFileAsync()
        {
            List<InputTransaction> result = new List<InputTransaction>();

            int invalidLines = 0;
            int currentLine = 0;

            if (File.Exists(_filePath))
            {
                using (StreamReader sr = new StreamReader(_filePath))
                {
                    string? line;

                    Console.WriteLine($"Читаю -> {_filePath}");

                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        currentLine++;
                        var transaction = InputTransaction.GetTransaction(line);

                        if (transaction != null)
                        {
                            result.Add(transaction);
                        }
                        else
                        {
                            Console.WriteLine($"Помилка у рядку {currentLine}");
                            invalidLines++;
                        }
                    }
                    Console.WriteLine($"Закінчив читати -> {_filePath}");
                }
            }

            return result;
        }
    }
}
