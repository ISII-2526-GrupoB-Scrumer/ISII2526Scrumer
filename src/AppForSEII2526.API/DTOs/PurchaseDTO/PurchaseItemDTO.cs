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
    }
}

