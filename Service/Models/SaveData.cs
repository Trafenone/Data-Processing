namespace Service.Models
{
    public class SaveData
    {
        private readonly FileData _fileData;

        public SaveData(FileData fileData)
        {
            _fileData = fileData;
        }

        public OutputTransaction Transform(List<InputTransaction> transactions)
        {
            throw new ArgumentException(nameof(transactions));
        }
    }
}
