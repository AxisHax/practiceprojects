namespace RuntimeTranscriber.RuntimeObjects
{
    public class TagMonitor
    {
        public string RtdbHost { get; set; }
        public int RtdbPort { get; set; }
        public List<HealthTag> HealthTags { get; set; }
        public List<TimestampTag> TimestampTags { get; set; }
        public int PollRate { get; set; }
    }
}
