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

        public List<RentalItemDTO> RentalItems { get; set; }

        public RentalCreateDTO()
        {
            RentalItems = new List<RentalItemDTO>();
        }

        public RentalCreateDTO(DateTime startDate, DateTime endDate, string paymentMethod, string deliveryCarDealer, string clientId, decimal totalPrice, List<RentalItemDTO> rentalItems)
        {
            StartDate = startDate;
            EndDate = endDate;
            PaymentMethod = paymentMethod;
            DeliveryCarDealer = deliveryCarDealer;
            ClientId = clientId;
            TotalPrice = totalPrice;
            RentalItems = rentalItems;
        }

        public override bool Equals(object? obj)
        {
            return obj is RentalCreateDTO dTO &&
                   StartDate == dTO.StartDate &&
                   EndDate == dTO.EndDate &&
                   PaymentMethod == dTO.PaymentMethod &&
                   DeliveryCarDealer == dTO.DeliveryCarDealer &&
                   ClientId == dTO.ClientId &&
                   TotalPrice == dTO.TotalPrice &&
                   RentalItems.SequenceEqual(dTO.RentalItems);
        }
    }
}
