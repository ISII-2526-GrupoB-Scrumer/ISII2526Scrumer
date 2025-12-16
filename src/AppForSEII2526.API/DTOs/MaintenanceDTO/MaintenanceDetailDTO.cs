namespace AppForSEII2526.API.DTOs.MaintenanceDTO
{
    public class MaintenanceDetailDTO : MaintenanceCreateDTO
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public List<MaintenanceItemDTO> Maintenances { get; set; }

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
        public override bool Equals(object obj)
        {
            if (obj is MaintenanceDetailDTO other)
            {
                return this.Id == other.Id &&
                       this.ClientId == other.ClientId &&
                       this.PaymentMethod == other.PaymentMethod &&
                       this.TotalPrice == other.TotalPrice &&
                       this.Maintenances.SequenceEqual(other.Maintenances);
            }
            return false;
        }
    }
}