using Service.Models;
using Service.Models.Readers;
using System.Collections.Concurrent;

namespace Service
{
    public class Watcher
    {
        private readonly ConcurrentQueue<string> _queue;
        private readonly FileSystemWatcher _watcher;
        private readonly SaveJson _saveService;
        private readonly FileData _fileData;
        private bool _enabled = true;


        public Watcher(FileData fileData)
        {
            _fileData = fileData;
            _queue = new ConcurrentQueue<string>();
            _watcher = new FileSystemWatcher(_fileData.PathA);
            _saveService = new SaveJson(_fileData);
            _watcher.Created += Watcher_Created;
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
                    var inputData = await reader.ReadFileAsync();
                    await _saveService.SaveAsync(OutputTransaction.Transform(inputData));
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
            Console.WriteLine("З'явився новий файл. Шлях: " + e.FullPath);

            string extension = new FileInfo(e.FullPath).Extension;

            if (extension == ".txt" || extension == ".csv")
                _queue.Enqueue(e.FullPath);
        }
    }
}
