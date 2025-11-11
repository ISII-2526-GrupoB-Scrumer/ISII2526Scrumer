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
}