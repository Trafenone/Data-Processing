namespace Service.Models.Readers
{
    public class FileProcessFactory
    {
        public IFileReader? GetFileReader(FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            return file.Extension.ToLower() switch
            {
                ".txt" => new TextFileReader(file.FullName),
                ".csv" => new CsvFileReader(file.FullName),
                _ => null,
            };
        }
    }
}
