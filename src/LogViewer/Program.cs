using LogViewer;

string topic = args.Length > 0 ? args[0] : "logs.*"; // todos los topics

Console.WriteLine($"LogViewer suscrito al topic: {topic}");

var subscriber = new Subscriber(topic);
subscriber.Start();

Console.WriteLine("Pulsa ENTER para salir.");
Console.ReadLine();