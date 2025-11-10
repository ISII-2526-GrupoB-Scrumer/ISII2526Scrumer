namespace AppForSEII2526
{
    public class ReviewCreateDTO
    {
        public DateTime Created { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string DriverType { get; set; } = string.Empty;

        public List<ReviewItemDTO> ReviewItems { get; set; }

        public ReviewCreateDTO()
        {
            ReviewItems = new List<ReviewItemDTO>();
        }

        public ReviewCreateDTO(string name, string country, string driverType, List<ReviewItemDTO> reviewItems)
        {
            Name = name;
            Country = country;
            DriverType = driverType;
            ReviewItems = reviewItems;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReviewCreateDTO dTO &&
                Created == dTO.Created &&
                Name == dTO.Name &&
                Country == dTO.Country &&
                DriverType == dTO.DriverType &&
                ReviewItems.SequenceEqual(dTO.ReviewItems);
        }
    }
}
