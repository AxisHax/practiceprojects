namespace RuntimeTranscriber.RuntimeObjects
{
    public class Query
    {
        public List<InputParameter> InputParameters { get; set; }
        public List<OutputParameter> OutputParameters { get; set; }
        public string QueryText { get; set; }
    }
}
