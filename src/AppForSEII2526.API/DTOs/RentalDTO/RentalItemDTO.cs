namespace AppForSEII2526
{
    public class RentalItemDTO
    {

        public int CarId { get; set; }
        public string CarModel { get; set; } = string.Empty;
        public decimal RentingPrice { get; set; }
        public int Quantity { get; set; }

        public RentalItemDTO() { }

        public RentalItemDTO(int carId, string carModel, decimal rentingPrice, int quantity)
        {
            CarId = carId;
            CarModel = carModel;
            RentingPrice = rentingPrice;
            Quantity = quantity;
        }

    }
}
