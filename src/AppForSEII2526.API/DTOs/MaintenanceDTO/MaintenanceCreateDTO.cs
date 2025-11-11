public class MaintenanceCreateDTO
{
    public string ClientId { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<MaintenanceItemDTO> Maintenances { get; set; }

    public MaintenanceCreateDTO()
    {
        Maintenances = new List<MaintenanceItemDTO>();
    }
}