namespace AppForSEII2526.API.DTOs.CarsDTO
{
    public class CarforRental
    {

        public int Id { get; set; }

        public string Model { get; set; }

        public string FuelType { get; set; }

        public string Manufacturer { get; set; }

        public decimal RentingPrice { get; set; }

        public string Color { get; set; }


        public CarforRental() { }
        public CarforRental(int id, string model, string fuelType, string manufacturer, decimal rentingPrice, string color)
        {
            Id = id;
            Model = model;
            FuelType = fuelType;
            Manufacturer = manufacturer;
            RentingPrice = rentingPrice;
            Color = color;
        }
    }
}
