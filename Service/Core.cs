namespace Service
{
    public class Core : BackgroundService
    {
        private readonly FileData _fileData;
        private readonly Watcher _watcher;

        public Core(FileData fileData)
        {
            _fileData = fileData;
            _watcher = new(_fileData);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Thread watchThread = new Thread(new ThreadStart(_watcher.Start));
            watchThread.Start();

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _watcher.Stop();

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine("Прога працює");

                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Помилка: " + e.Message);
            }
        }
    }
}
