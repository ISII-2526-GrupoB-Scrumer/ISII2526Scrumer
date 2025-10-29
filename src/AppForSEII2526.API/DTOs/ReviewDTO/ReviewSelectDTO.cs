namespace AppForSEII2526
{
    public class ReviewSelectDTO
    {

        public int Id { get; set; }
        public string ModelName { get; set; }
        public string CarClass { get; set; }
        public string Manufacturer { get; set; }
        public string FuelType { get; set; }
        public string Color { get; set; }

        public ReviewSelectDTO(int id, string modelName, string carClass, string manufacturer, string fuelType, string color)
        {
            Id = id;
            ModelName = modelName;
            CarClass = carClass;
            Manufacturer = manufacturer;
            FuelType = fuelType;
            Color = color;
        }
    }
}
