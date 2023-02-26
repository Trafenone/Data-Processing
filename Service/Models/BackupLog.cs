namespace Service.Models
{
    public class BackupLog
    {
        public int ParsedFiles { get; set; }
        public int ParsedLines { get; set; }
        public int FoundErrors { get; set; }
        public List<string> InvalidFiles { get; set; }
    }
}
