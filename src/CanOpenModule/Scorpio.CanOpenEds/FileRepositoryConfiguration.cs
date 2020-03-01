namespace Scorpio.CanOpenEds
{
    public class FileRepositoryConfiguration
    {
        public string JsonEdsPath { get; set; }

        public FileRepositoryConfiguration WithPath(string jsonEdsPath)
        {
            JsonEdsPath = jsonEdsPath;
            return this;
        }
    }
}
