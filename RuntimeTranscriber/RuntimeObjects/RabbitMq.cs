namespace RuntimeTranscriber.RuntimeObjects
{
    public class RabbitMq
    {
        public int Consumers { get; set; }
        public string Exchange { get; set; }
        public bool ExchangeDurable { get; set; }
        public string HostName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string Queue { get; set; }
        public bool QueueDurable { get; set; }
        public int ReconnectTimer { get; set; }
        public string RoutingKey { get; set; }
        public string UserName { get; set; }
        public string RetryExchange { get; set; }
        public string RetryQueue { get; set; }
        public int RetryTimer { get; set; }
        public int NumberOfRetries { get; set; }
    }
}
