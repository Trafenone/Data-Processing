namespace Service.Models.Readers
{
    public class TextFileReader : FileReaderBase
    {
        public TextFileReader(string filePath) : base(filePath, "TextFileReader") { }

        protected override bool ShouldSkipLine(string line)
        {
            return false;
        }
    }
}
