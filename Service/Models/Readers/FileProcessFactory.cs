namespace Service.Models.Readers
{
    public class FileProcessFactory
    {
        public IFileReader? GetFileReader(FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            switch (file.Extension.ToLower())
            {
                case ".txt":
                    return new TextFileReader(file.FullName);
                case ".csv":
                    return new CsvFileReader(file.FullName);
                default:
                    return null; 
            }
        }
    }
}
