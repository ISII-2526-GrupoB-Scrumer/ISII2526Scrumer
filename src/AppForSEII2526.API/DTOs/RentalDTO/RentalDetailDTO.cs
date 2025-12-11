using System.Net;

namespace AppForSEII2526
{

    // DTO detallado para mostrar información completa de un alquiler
    // Hereda de RentalCreateDTO para reutilizar los mismos atributos del modelo de creación
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

    }
}
