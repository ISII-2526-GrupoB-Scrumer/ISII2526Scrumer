namespace AppForSEII2526
{
    public class ReviewItemDTO
    {
        public int CarId { get; set; }
        public string CarModel { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Rating { get; set; }

        public ReviewItemDTO() { }

        public ReviewItemDTO(int carId, string carModel, string manufacturer, string fuelType, string color, string description, int rating)
        {
            CarId = carId;
            CarModel = carModel;
            Manufacturer = manufacturer;
            FuelType = fuelType;
            Color = color;
            Description = description;
            Rating = rating;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReviewItemDTO dTO &&
                CarId == dTO.CarId &&
                CarModel == dTO.CarModel &&
                Manufacturer == dTO.Manufacturer &&
                FuelType == dTO.FuelType &&
                Color == dTO.Color &&
                Description == dTO.Description &&
                Rating == dTO.Rating;
        }
    }
}
