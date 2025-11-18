namespace AppForSEII2526.API.DTOs.MaintenanceDTO
{
    public class MaintenanceCreateDTO
    {
        public string ClientId { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public List<MaintenanceItemDTO> MaintenanceItems { get; set; }

        public MaintenanceCreateDTO()
        {
            MaintenanceItems = new List<MaintenanceItemDTO>();
        }
    }
}
