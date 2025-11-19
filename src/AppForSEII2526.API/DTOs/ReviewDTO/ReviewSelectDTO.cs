namespace AppForSEII2526
{
    // DTO usado para devolver información resumida de un coche asociado a una review
    // Se emplea principalmente en consultas donde no se requieren detalles completos.
    public class ReviewSelectDTO
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string CarClass { get; set; }
        public string Manufacturer { get; set; }
        public string FuelType { get; set; }
        public string Color { get; set; }

        public ReviewSelectDTO() { }

        public ReviewSelectDTO(int id, string modelName, string carClass, string manufacturer, string fuelType, string color)
        {
            Id = id;
            ModelName = modelName;
            CarClass = carClass;
            Manufacturer = manufacturer;
            FuelType = fuelType;
            Color = color;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReviewSelectDTO dTO &&
                Id == dTO.Id &&
                ModelName == dTO.ModelName &&
                CarClass == dTO.CarClass &&
                Manufacturer == dTO.Manufacturer &&
                FuelType == dTO.FuelType &&
                Color == dTO.Color;
        }
    }
}
