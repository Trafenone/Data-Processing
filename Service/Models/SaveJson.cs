using System.Text;
using System.Text.Json;

namespace Service.Models
{
    public class SaveJson
    {
        private readonly FileData _fileData;
        private readonly object _locker;

        public SaveJson(FileData fileData)
        {
            _fileData = fileData;
            _locker = new object();
        }

        public void Save(OutputTransaction output)
        {
            lock (_locker)
            {
                string pathDirectory = _fileData.PathB + "\\" + GetCurrentDirectory();

                CheckFolder(pathDirectory);

                string pathFile = pathDirectory + "\\output" + GetCountFiles(pathDirectory) + ".json";

                using (FileStream fs = new FileStream(pathFile, FileMode.Create))
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    };

                    string json = JsonSerializer.Serialize(output, options);

                    byte[] input = Encoding.Default.GetBytes(json);

                    fs.Write(input);
                }
            }
        }

        private int GetCountFiles(string path) => Directory.GetFiles(path).Length;
        private string GetCurrentDirectory() => DateTime.Now.ToString("MM-dd-yyyy");
        private void CheckFolder(string pathFolder)
        {
            if (!Directory.Exists(pathFolder))
                Directory.CreateDirectory(pathFolder);
        }
    }
}
