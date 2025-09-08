namespace RuntimeTranscriber.RuntimeObjects
{
    public class HealthTag
    {
        public string Tag { get; set; }
        public string AlarmTag { get; set; }
        public string HealthRegex { get; set; }
        public string RegexGroup { get; set; }
        public string HealthyState { get; set; }
    }
}
