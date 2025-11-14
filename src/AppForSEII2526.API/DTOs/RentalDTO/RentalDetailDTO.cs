using System.Net;

namespace AppForSEII2526
{
    public class RentalDetailDTO : RentalCreateDTO
    {

        public int Id { get; set; }

        public RentalDetailDTO() { }

        public RentalDetailDTO(int id,string clientid, string name, string surname, string address, string paymentMethod,DateTime rentingdate, DateTime startDate, DateTime endDate, decimal totalPrice,string deliverycardealer, List<RentalItemDTO> rentalItems)
        {
            Id = id;
            ClientId = clientid;
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

        public override bool Equals(object? obj)
        {
            return obj is RentalDetailDTO dTO &&
                   base.Equals(obj) &&
                   StartDate == dTO.StartDate &&
                   EndDate == dTO.EndDate &&
                   PaymentMethod == dTO.PaymentMethod &&
                   TotalPrice == dTO.TotalPrice &&
                   ClientId == dTO.ClientId &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname &&
                   Address == dTO.Address &&
                   DeliveryCarDealer == dTO.DeliveryCarDealer &&
                   RentingDate == dTO.RentingDate &&
                   RentalItems.SequenceEqual(dTO.RentalItems) &&
                   Id == dTO.Id;
        }
    }
}
