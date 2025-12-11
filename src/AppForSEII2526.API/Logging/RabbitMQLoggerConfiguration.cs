namespace AppForSEII2526;

public class RabbitMQLoggerConfiguration
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string Exchange { get; set; } = "logs_topic";
    public string ExchangeType { get; set; } = "topic";
    public bool Durable { get; set; } = true;
}
