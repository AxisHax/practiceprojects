namespace RuntimeTranscriber.RuntimeObjects
{
    public class DbReader
    {
        public List<Query> Queries { get; set; }
        public int PollRate { get; set; }
        public string RtdbHost { get; set; }
        public int RtdbPort { get; set; }
        public string DatabaseProvider { get; set; }
    }
}
