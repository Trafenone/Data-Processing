using Service.Models;
using Service.Models.Readers;
using System.Collections.Concurrent;

namespace Service
{
    public class Watcher
    {
        private readonly ConcurrentQueue<string> _queue;
        private readonly FileSystemWatcher _watcher;
        private readonly FileLogger _fileLogger;
        private readonly SaveJson _saveService;
        private readonly FileData _fileData;
        private readonly ILogger _logger;
        private bool _enabled = true;


        public Watcher(FileData fileData)
        {
            _fileData = fileData;
            _queue = new ConcurrentQueue<string>();
            _watcher = new FileSystemWatcher(_fileData.PathA);
            _saveService = new SaveJson(_fileData);
            _watcher.Created += Watcher_Created;
            _logger = Core.GetLogger("Watcher")!;
            _fileLogger = new FileLogger(_fileData);
            _fileLogger.BackUp();
        }

        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
            while (_enabled)
            {
                Parallel.ForEach(_queue, async (item) =>
                {
                    _queue.TryDequeue(out _);
                    var reader = new FileProcessFactory().GetFileReader(new FileInfo(item));
                    var readingResult = await reader!.ReadFileAsync();
                    _fileLogger.SaveOneLog(readingResult);
                    _saveService.Save(OutputTransaction.Transform(readingResult.Transactions));
                });
            }
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            _enabled = false;
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("New file detected. Path: " + e.FullPath);

            string extension = new FileInfo(e.FullPath).Extension;

            if (extension == ".txt" || extension == ".csv")
                _queue.Enqueue(e.FullPath);
        }
    }
}
