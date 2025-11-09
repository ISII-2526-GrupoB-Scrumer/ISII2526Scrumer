using System.Net;

namespace AppForSEII2526
{
    public class RentalDetailDTO : RentalCreateDTO
    {

        public int Id { get; set; }

        public RentalDetailDTO() { }

        public RentalDetailDTO(int id, string name, string surname, string address, string paymentMethod,DateTime rentingdate, DateTime startDate, DateTime endDate, decimal totalPrice,string deliverycardealer, List<RentalItemDTO> rentalItems)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Address = address;
            PaymentMethod = paymentMethod;
            RentingDate = rentingdate;
            StartDate = startDate;
            EndDate = endDate;
            TotalPrice = totalPrice;
            DeliveryCarDealer = deliverycardealer;
            RentalItems = rentalItems;
        }

    }
}
