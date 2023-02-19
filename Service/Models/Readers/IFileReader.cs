namespace Service.Models.Readers
{
    public interface IFileReader
    {
        Task<List<InputTransaction>> ReadFileAsync();
    }
}