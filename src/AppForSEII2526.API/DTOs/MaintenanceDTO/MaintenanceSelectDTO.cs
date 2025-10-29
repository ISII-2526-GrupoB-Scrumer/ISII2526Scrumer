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

    }
}
