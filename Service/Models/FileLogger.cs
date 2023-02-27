using Service.Models.Readers;

namespace Service.Models
{
    public class FileLogger
    {
        private readonly FileData _fileData;
        private readonly object _locker;

        public FileLogger(FileData fileData)
        {
            _fileData = fileData;
            _locker = new object();
            CreateFileLog();
        }

        public void SaveOneLog(ReadingResult result)
        {
            lock (_locker)
            {
                using (StreamWriter writer = new StreamWriter(GetCurrentFile(), true))
                {
                    writer.WriteLine(result.NameFile + "\t\t\t" + result.Lines + '\t' + result.InvalidLines);
                }
            }
        }

        public void BackUp()
        {
            var data = ReadLog();

            Directory.CreateDirectory(GetBackupDirectory());

            using (StreamWriter writer = new StreamWriter(GetPathBackup(), false))
            {
                writer.WriteLine("parsed_files: " + data.ParsedFiles);
                writer.WriteLine("parsed_lines: " + data.ParsedLines);
                writer.WriteLine("found_errors: " + data.FoundErrors);
                writer.Write("invalid_files: [ ");

                foreach (var file in data.InvalidFiles)
                    writer.Write(file + ", ");

                writer.Write("]");
            }
        }

        private BackupLog ReadLog()
        {
            List<string> invalidFiles = new List<string>();
            int parsedFiles = 0;
            int parsedLines = 0;
            int foundErrors = 0;

            using (StreamReader reader = new StreamReader(GetCurrentFile()))
            {
                string? line;
                string[] values;

                while ((line = reader.ReadLine()) != null)
                {
                    parsedFiles++;

                    values = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);

                    int errors = int.Parse(values[2]);

                    if (errors > 0)
                        invalidFiles.Add(values[0]);

                    parsedLines += int.Parse(values[1]);
                    foundErrors += errors;
                }
            }

            return new BackupLog
            {
                InvalidFiles = invalidFiles,
                ParsedFiles = parsedFiles,
                ParsedLines = parsedLines,
                FoundErrors = foundErrors
            };
        }

        public void CreateFileLog()
        {
            string pathDirectory = _fileData.PathB + "\\" + GetCurrentDirectory();

            CheckFolder(pathDirectory);

            string pathFile = GetCurrentFile();

            if (!File.Exists(pathFile))
                File.Create(pathFile);
        }

        private string GetPathBackup() => GetBackupDirectory() + "\\meta.log";
        private string GetBackupDirectory() => _fileData.PathC + "\\" + GetCurrentDirectory();
        private string GetCurrentFile() => _fileData.PathB + "\\" + GetCurrentDirectory() + "\\meta.log";
        private string GetCurrentDirectory() => DateTime.Now.ToString("MM-dd-yyyy");
        private void CheckFolder(string pathFolder)
        {
            if (!Directory.Exists(pathFolder))
                Directory.CreateDirectory(pathFolder);
        }
    }
}
