namespace AppForSEII2526
{
    public class PurchaseSelectDTO
    {

        public int Id { get; set; }
        public string ModelName { get; set; }
        public string Manufacturer { get; set; }
        public string Color { get; set; }
        public decimal PurchasingPrice { get; set; }

        public PurchaseSelectDTO(int id, string modelName, string manufacturer, string color, decimal purchasingPrice)
        {
            Id = id;
            ModelName = modelName;
            Manufacturer = manufacturer;
            Color = color;
            PurchasingPrice = purchasingPrice;
        }




    }
}
