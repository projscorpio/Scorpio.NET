namespace Scorpio.CanOpenEds
{
    public class FileRepositoryConfiguration
    {
        public string MiControlJsonEdsPath { get; set; }
        public string ScorpioCanJsonEdsPath { get; set; }

        public FileRepositoryConfiguration WithMiControlPath(string miControlJsonEdsPath)
        {
            MiControlJsonEdsPath = miControlJsonEdsPath;
            return this;
        }

        public FileRepositoryConfiguration WithScorpioEdsPath(string scorpioCanJsonEdsPath)
        {
            ScorpioCanJsonEdsPath = scorpioCanJsonEdsPath;
            return this;
        }
    }
}
