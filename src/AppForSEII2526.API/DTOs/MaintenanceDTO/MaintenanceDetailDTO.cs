public class MaintenanceDetailDTO : MaintenanceCreateDTO
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }

    public MaintenanceDetailDTO() { }

    public MaintenanceDetailDTO(int id, string clientId, string paymentMethod, DateTime date, decimal totalPrice, List<MaintenanceItemDTO> maintenances)
    {
        Id = id;
        ClientId = clientId;
        PaymentMethod = paymentMethod;
        Date = date;
        TotalPrice = totalPrice;
        Maintenances = maintenances;
    }
}