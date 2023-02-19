namespace Service
{
    public class FileData
    {
        private readonly IConfiguration _configuration;
        public IConfiguration Configuration => _configuration;

        public string PathA { get; }
        public string PathB { get; }
        public string PathC { get; }


        public FileData(IConfiguration configuration)
        {
            _configuration = configuration;

            PathA = _configuration["Paths:FolderA"];
            PathB = _configuration["Paths:FolderB"];
            PathC = _configuration["Paths:FolderC"];

            if (!ValidateDirectoriesExists())
                Environment.Exit(0);
        }

        public bool ValidateDirectoriesExists() =>
            Directory.Exists(PathA) && Directory.Exists(PathB) && Directory.Exists(PathC);
    }
}
