namespace AppForSEII2526
{
    public class MaintenanceSelectDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int NumberOfDays { get; set; }

        public MaintenanceSelectDTO(int id, string name, string type, decimal price, int numberOfDays)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = price;
            NumberOfDays = numberOfDays;
        }
        public override bool Equals(object? obj)
        {
            if (obj is not MaintenanceSelectDTO other)
                return false;

            return Id == other.Id &&
                   Name == other.Name &&
                   Type == other.Type &&
                   Price == other.Price &&
                   NumberOfDays == other.NumberOfDays;
        }

    }
}
