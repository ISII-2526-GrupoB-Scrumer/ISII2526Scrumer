namespace AppForSEII2526.API.DTOs.RentalDTO
{
    public class RentalDetailDTO : RentalCreateDTO
    {

        public int Id { get; set; }

        public RentalDetailDTO() { }

        public RentalDetailDTO(int id, DateTime startDate, DateTime endDate, string paymentMethod, string deliveryCarDealer, string clientId, decimal totalPrice, List<RentalItemDTO> rentalItems)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            PaymentMethod = paymentMethod;
            DeliveryCarDealer = deliveryCarDealer;
            ClientId = clientId;
            TotalPrice = totalPrice;
            RentalItems = rentalItems;
        }

    }
}
