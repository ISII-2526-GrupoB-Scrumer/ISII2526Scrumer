using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace LogViewer;

public class Subscriber
{
    private const string ExchangeName = "logs_topic";    // exchange de la API
    private readonly string _topic;   //nombre del topic a suscribirse

    private readonly IConnection _connection; //conexion al broker
    private readonly IModel _channel; //canal de comunicacion

    public Subscriber(string topic)
    {
        _topic = topic; //asignar el topic

        var factory = new ConnectionFactory
        {
            HostName = "host.docker.internal", //ip para que funcione 
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection(); //crear conexion
        _channel = _connection.CreateModel(); //crear canal

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

        var consumer = new EventingBasicConsumer(_channel); //crear consumidor

        // Evento al recibir un mensaje
        consumer.Received += (sender, args) =>
        {
            try
            {
                string json = Encoding.UTF8.GetString(args.Body.ToArray()); //decodificar mensaje

                var log = JsonSerializer.Deserialize<LogEntry>(json);  //deserializar mensaje

                if (log != null)  //si el log no es nulo
                {
                    PrintLog(log);
                }
            }
            catch (Exception ex) //si hay error al procesar el mensaje
            {
                Console.WriteLine($"Error procesando mensaje: {ex.Message}");
            }
        };

        // Iniciar consumo de mensajes
        _channel.BasicConsume(
            queue: queue,
            autoAck: true,
            consumer: consumer
        );
    }

    // Imprimir log en consola
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
            Console.WriteLine($"Exception: {log.Exception}");
        }

        Console.WriteLine();
    }
}

public class LogEntry
{
    public DateTime Timestamp { get; set; } // Fecha y hora del log
    public string? LogLevel { get; set; } // Nivel de log
    public string? Category { get; set; } // Categoria del log
    public int EventId { get; set; } // ID del evento
    public string? EventName { get; set; }  // Nombre del evento
    public string? Message { get; set; } // Mensaje del log
    public string? Exception { get; set; } // Excepcion si existe
}
