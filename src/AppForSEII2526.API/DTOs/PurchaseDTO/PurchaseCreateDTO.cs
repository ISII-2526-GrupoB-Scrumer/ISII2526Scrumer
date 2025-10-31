namespace AppForSEII2526
{
    public class PurchaseCreateDTO
    {
        public DateTime PurchasingDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string DriverType { get; set; } = string.Empty;
        public string DeliveryCarDealer { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public decimal PurchasingPrice { get; set; }

        public List<PurchaseItemDTO> PurchaseItems { get; set; }

        public PurchaseCreateDTO()
        {
            PurchaseItems = new List<PurchaseItemDTO>();
        }
    }
}

