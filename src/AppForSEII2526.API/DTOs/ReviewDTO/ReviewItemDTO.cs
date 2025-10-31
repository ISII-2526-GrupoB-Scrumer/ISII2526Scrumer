namespace AppForSEII2526
{
    public class ReviewItemDTO
    {
        public int CarId { get; set; }
        public string CarModel { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Rating { get; set; }

        public ReviewItemDTO() { }

        public ReviewItemDTO(int carId, string carModel, string description, int rating)
        {
            CarId = carId;
            CarModel = carModel;
            Description = description;
            Rating = rating;
        }
    }
}

