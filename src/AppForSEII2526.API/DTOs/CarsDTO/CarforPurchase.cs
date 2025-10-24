namespace AppForSEII2526.API.DTOs.CarsDTO
{
    public class CarforPurchase
    {

        public int Id { get; set; }

        public string Model { get; set; }

        public string FuelType { get; set; }

        public string Manufacturer { get; set; }

        public decimal PurchasingPrice { get; set; }

        public string Color { get; set; }


        public CarforPurchase() { }
        public CarforPurchase(int id, string model, string fuelType, string manufacturer, decimal purchasingPrice, string color)
        {
            Id = id;
            Model = model;
            FuelType = fuelType;
            Manufacturer = manufacturer;
            PurchasingPrice = purchasingPrice;
            Color = color;
        }
    }
}
