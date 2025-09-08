namespace RuntimeTranscriber.RuntimeObjects
{
    public class TYPEMEMBER
    {
        public int ID { get; set; }
        public int PARENTTYPEID { get; set; }
        public string NAME { get; set; }
        public int? DATASIZE { get; set; }
        public int? DATATYPE { get; set; }
        public int? OFFSET { get; set; }
        public int? MASK { get; set; }
        public int? OBJECTTYPEID { get; set; }
        public string EXPRESSION { get; set; }
        public int? ALLOWWRITES { get; set; }
    }
}
