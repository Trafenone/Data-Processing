using System.Text.Json;
using System.Text;

namespace Service.Models
{
    public class SaveJson
    {
        private readonly FileData _fileData;

        public SaveJson(FileData fileData)
        {
            _fileData = fileData;
        }

        public async Task SaveAsync(OutputTransaction output)
        {
            string path = _fileData.PathB + "\\output" + GetCountFiles();

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };

                string json = JsonSerializer.Serialize(output, options);

                byte[] input = Encoding.Default.GetBytes(json);

                await fs.WriteAsync(input);
            }
        }

        private int GetCountFiles() => Directory.GetFiles(_fileData.PathB).Length;        
    }
}
