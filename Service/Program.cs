namespace Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<FileData>();
                    services.AddHostedService<Core>();
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConfiguration(
                        context.Configuration.GetSection("Logging"));
                })
                .Build();

            var core = host.Services.GetService<Core>();

            //Thread t = new(() => ConsoleWatcher(core)); 
            //t.Start();

            await host.RunAsync();
        }

        private static void ConsoleWatcher(Core core)
        {
            Console.WriteLine("Press CTRL+R to restart the service.");

            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.R && keyInfo.Modifiers == ConsoleModifiers.Control)
                {
                    core.RestartAsync(CancellationToken.None);

                    Console.WriteLine("The service has been restarted.");
                }
            }
        }
    }
}