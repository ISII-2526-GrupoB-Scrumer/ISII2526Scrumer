namespace AppForSEII2526
{
    public class PurchaseItemDTO
    {
        public int CarId { get; set; }
        public string CarModel { get; set; } = string.Empty;
        public decimal PurchasingPrice { get; set; }
        public int Quantity { get; set; }

        public PurchaseItemDTO() { }

        public PurchaseItemDTO(int carId, string carModel, decimal purchasingPrice, int quantity)
        {
            CarId = carId;
            CarModel = carModel;
            PurchasingPrice = purchasingPrice;
            Quantity = quantity;
        }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseItemDTO dTO &&
                   CarId == dTO.CarId &&
                   CarModel == dTO.CarModel &&
                   PurchasingPrice == dTO.PurchasingPrice &&
                   Quantity == dTO.Quantity;
        }
    }
}