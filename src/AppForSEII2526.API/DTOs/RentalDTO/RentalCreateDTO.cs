using System.Drawing;

namespace AppForSEII2526
{
    public class RentalCreateDTO
    {

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }

        public string ClientId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;


        public string DeliveryCarDealer { get; set; } = string.Empty;
        public DateTime RentingDate { get; set; }

        public List<RentalItemDTO> RentalItems { get; set; }

        public RentalCreateDTO()
        {
            RentalItems = new List<RentalItemDTO>();
        }

        public RentalCreateDTO(string clientid, string name, string surname, string address, string paymentMethod,DateTime rentingdate, DateTime startDate, DateTime endDate, decimal totalPrice,string deliverycardealer, List<RentalItemDTO> rentalItems)
        {
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
            return obj is RentalCreateDTO dTO &&
                   ClientId == dTO.ClientId &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname &&
                   Address == dTO.Address &&
                   StartDate == dTO.StartDate &&
                   EndDate == dTO.EndDate &&
                   PaymentMethod == dTO.PaymentMethod &&
                   TotalPrice == dTO.TotalPrice &&
                   RentalItems.SequenceEqual(dTO.RentalItems);
        }
    }
}
