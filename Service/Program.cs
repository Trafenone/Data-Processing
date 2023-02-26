namespace Service
{
    public class Program
    {
        public static void Main(string[] args)
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

            host.Run();
        }
    }
}