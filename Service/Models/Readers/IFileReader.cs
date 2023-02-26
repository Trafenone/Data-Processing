namespace Service.Models.Readers
{
    public interface IFileReader
    {
        Task<ReadingResult> ReadFileAsync();
    }
}