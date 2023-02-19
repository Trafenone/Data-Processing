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
                .Build();

            host.Run();
        }
    }
}