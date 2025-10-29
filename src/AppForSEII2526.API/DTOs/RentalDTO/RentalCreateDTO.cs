namespace AppForSEII2526
{
    public class RentalCreateDTO
    {

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string DeliveryCarDealer { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }

        public List<RentalItemDTO> RentalItems { get; set; } = new();

    }
}
