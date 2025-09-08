namespace RuntimeTranscriber.RuntimeObjects
{
    public class DbWriter
    {
        public RabbitMq RabbitMq { get; set; }
        public string Query { get; set; }
        public List<string> QueryParameters { get; set; }
        public List<string> DatabaseTables { get; set; }
        public string DatabaseProvider { get; set; }
    }
}
