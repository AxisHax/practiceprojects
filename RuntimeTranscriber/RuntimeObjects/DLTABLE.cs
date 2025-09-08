namespace RuntimeTranscriber.RuntimeObjects
{
    public class DLTABLE
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string TIME_STAMP_COLUMN_NAME { get; set; }
        public string DEVICE_TYPE_COLUMN_NAME { get; set; }
        public string DEVICE_NAME_COLUMN { get; set; }
        public int UPDATE_RATE_SECONDS { get; set; }
        public int NOTIFY_ODSS { get; set; }
    }
}
