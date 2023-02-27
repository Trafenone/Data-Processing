namespace Service
{
    public class Core : BackgroundService
    {
        private readonly FileData _fileData;
        private readonly Watcher _watcher;
        private readonly ILogger<Core> _logger;

        public Core(FileData fileData, ILogger<Core> logger)
        {
            _fileData = fileData;
            _watcher = new(_fileData);
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The service is starting");

            Thread watchThread = new(new ThreadStart(_watcher.Start));
            watchThread.Start();

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The Service is stopping");

            _watcher.Stop();

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("The Service is working");

                    await Task.Delay(10000, stoppingToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public async Task RestartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The Service is restarting");

            await StopAsync(cancellationToken);

            Thread.Sleep(5000);

            await StartAsync(cancellationToken);
        }

        public static ILogger GetLogger(string categoryName)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();
            });

            ILogger logger = loggerFactory.CreateLogger(categoryName);

            return logger;
        }
    }
}
