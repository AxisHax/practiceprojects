namespace RuntimeTranscriber.RuntimeObjects
{
    public class SortMonitor
    {
        public string RtdbHost { get; set; }
        public int RtdbPort { get; set; }
        public string SortStateTag { get; set; }
        public List<string> SortResetTags { get; set; }
        public int PollRate { get; set; }
        public string TagValue { get; set; }
    }
}
