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

        public override bool Equals(object? obj)
        {
            return obj is RentalSelectDTO dTO &&
                   Id == dTO.Id &&
                   ModelName == dTO.ModelName &&
                   Manufacturer == dTO.Manufacturer &&
                   FuelType == dTO.FuelType &&
                   RentingPrice == dTO.RentingPrice &&
                   Color == dTO.Color;
        }
    }
}
