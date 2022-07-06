namespace ChamiUI.BusinessLayer.EnvironmentHealth
{
    public class EnvironmentHealthCheckerConfiguration
    {
        public double MaxScore { get; set; }
        public double MismatchPenalty { get; set; }
        public double CheckInterval { get; set; }
    }
}