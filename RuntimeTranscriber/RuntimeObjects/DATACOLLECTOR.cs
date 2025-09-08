namespace RuntimeTranscriber.RuntimeObjects
{
    public class DATACOLLECTOR
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public int UPDATE_RATE_MS { get; set; }
        public int PORT_NUMBER { get; set; }
        public int FAILOVER_DATACOLLECTOR_ID { get; set; }
        public string IP_ADDRESS { get; set; }
        public int PRIORITY { get; set; }
        public int RTDB_SERVER_ID { get; set; }
    }
}
