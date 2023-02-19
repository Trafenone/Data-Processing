using System.Collections.Concurrent;

namespace Service
{
    public class Watcher
    {
        private readonly ConcurrentQueue<string> _queue;
        private readonly FileSystemWatcher _watcher;
        private readonly FileData _fileData;
        private bool _enabled = true;


        public Watcher(FileData fileData)
        {
            _fileData = fileData;
            _queue = new ConcurrentQueue<string>();
            _watcher = new FileSystemWatcher(_fileData.PathA);
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
                    var a = await reader.ReadFileAsync();
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
