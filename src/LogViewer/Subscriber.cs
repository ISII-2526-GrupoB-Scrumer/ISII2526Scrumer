using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace LogViewer;

public class Subscriber
{
    private const string ExchangeName = "logs_topic";    // exchange de la API
    private readonly string _topic;

    private readonly IConnection _connection;
    private readonly IModel _channel;

    public Subscriber(string topic)
    {
        _topic = topic;

        var factory = new ConnectionFactory
        {
            HostName = "10.89.49.63",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declarar exchange tipo topic
        _channel.ExchangeDeclare(
            exchange: ExchangeName,
            type: ExchangeType.Topic,
            durable: true
        );
    }

    public void Start()
    {
        // Cola efímera
        var queue = _channel.QueueDeclare().QueueName;

        // Bind según el topic del usuario
        _channel.QueueBind(
            queue: queue,
            exchange: ExchangeName,
            routingKey: _topic
        );

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (sender, args) =>
        {
            try
            {
                string json = Encoding.UTF8.GetString(args.Body.ToArray());

                var log = JsonSerializer.Deserialize<LogEntry>(json);

                if (log != null)
                {
                    PrintLog(log);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error procesando mensaje: {ex.Message}");
            }
        };

        _channel.BasicConsume(
            queue: queue,
            autoAck: true,
            consumer: consumer
        );
    }

    private static void PrintLog(LogEntry log)
    {
        Console.WriteLine(
            $"[{log.Timestamp:yyyy-MM-dd HH:mm:ss}] " +
            $"[{log.LogLevel}] " +
            $"({log.Category}) " +
            $"{log.Message}"
        );

        if (!string.IsNullOrEmpty(log.Exception))
        {
            Console.WriteLine($"   ⚠️ Exception: {log.Exception}");
        }

        Console.WriteLine();
    }
}

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string? LogLevel { get; set; }
    public string? Category { get; set; }
    public int EventId { get; set; }
    public string? EventName { get; set; }
    public string? Message { get; set; }
    public string? Exception { get; set; }
}
