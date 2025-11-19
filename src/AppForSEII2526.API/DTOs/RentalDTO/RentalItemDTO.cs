namespace AppForSEII2526
{

    // DTO que representa cada coche incluido dentro de un alquiler.
    // Contiene información del coche asociado (modelo, precio, fabricante, cantidad, etc.).
    // Se utiliza tanto al enviar como al recibir datos de los items del alquiler.
    public class RentalItemDTO
    {

        public int CarId { get; set; }
        public string CarModel { get; set; } = string.Empty;
        public decimal RentingPrice { get; set; }
        public int Quantity { get; set; }
        public string Manufacturer { get; set; } = string.Empty;

        public RentalItemDTO() { }

        public RentalItemDTO(int carId, string carModel, decimal rentingPrice, int quantity, string manufacturer)
        {
            CarId = carId;
            CarModel = carModel;
            RentingPrice = rentingPrice;
            Quantity = quantity;
            Manufacturer = manufacturer;
        }

        public override bool Equals(object? obj)
        {
            return obj is RentalItemDTO dTO &&
                   CarId == dTO.CarId &&
                   CarModel == dTO.CarModel &&
                   RentingPrice == dTO.RentingPrice &&
                   Quantity == dTO.Quantity &&
                   Manufacturer == dTO.Manufacturer;
        }
    }
}
