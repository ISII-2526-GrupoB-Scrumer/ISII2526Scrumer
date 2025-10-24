namespace AppForSEII2526.API.DTOs.CarsDTO
{
    public class CarforReview
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string CarClass { get; set; }
        public string Manufacturer { get; set; }
        public string FuelType { get; set; }
        public string Color { get; set; }

        public CarforReview() { }

        public CarforReview(int id, string model, string carClass, string manufacturer, string fuelType, string color)
        {
            Id = id;
            Model = model;
            CarClass = carClass;
            Manufacturer = manufacturer;
            FuelType = fuelType;
            Color = color;
        }
    }
}

