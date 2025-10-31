namespace AppForSEII2526
{
    public class PurchaseDetailDTO : PurchaseCreateDTO
    {
        public int Id { get; set; }

        public PurchaseDetailDTO() { }

        public PurchaseDetailDTO(int id, DateTime purchasingDate, string paymentMethod,
            string driverType, string deliveryCarDealer, string country,
            string clientId, decimal purchasingPrice, List<PurchaseItemDTO> purchaseItems)
        {
            Id = id;
            PurchasingDate = purchasingDate;
            PaymentMethod = paymentMethod;
            DriverType = driverType;
            DeliveryCarDealer = deliveryCarDealer;
            Country = country;
            ClientId = clientId;
            PurchasingPrice = purchasingPrice;
            PurchaseItems = purchaseItems;
        }
    }
}
