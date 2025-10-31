namespace AppForSEII2526
{
    public class RentalSelectDTO
    {

        public int Id { get; set; }
        public string ModelName { get; set; }
        public string Manufacturer { get; set; }
        public string FuelType { get; set; }
        public decimal RentingPrice { get; set; }
        public string Color { get; set; }

        public RentalSelectDTO(int id, string modelName, string manufacturer, string fuelType, decimal rentingPrice, string color)
        {
            Id = id;
            ModelName = modelName;
            Manufacturer = manufacturer;
            FuelType = fuelType;
            RentingPrice = rentingPrice;
            Color = color;
        }
    }
}
