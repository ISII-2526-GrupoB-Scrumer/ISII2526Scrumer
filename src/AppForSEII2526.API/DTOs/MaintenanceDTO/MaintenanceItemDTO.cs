namespace AppForSEII2526.API.DTOs.MaintenanceDTO
{
    public class MaintenanceItemDTO
    {
        public int MaintenanceId { get; set; }
        public string MaintenanceName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int NumberOfDays { get; set; }
        public string Comment { get; set; } = string.Empty;

        public MaintenanceItemDTO() { }

        public MaintenanceItemDTO(int maintenanceId, string maintenanceName, string type, decimal price, int numberOfDays, string comment)
        {
            MaintenanceId = maintenanceId;
            MaintenanceName = maintenanceName;
            Type = type;
            Price = price;
            NumberOfDays = numberOfDays;
            Comment = comment;
        }
        public override bool Equals(object obj)
        {
            if (obj is MaintenanceItemDTO other)
            {
                return this.MaintenanceId == other.MaintenanceId &&
                       this.MaintenanceName == other.MaintenanceName &&
                       this.Type == other.Type &&
                       this.Price == other.Price &&
                       this.NumberOfDays == other.NumberOfDays &&
                       this.Comment == other.Comment;
            }
            return false;
        }
    }
}
