namespace AppForSEII2526
{
    public class ReviewCreateDTO
    {
        public DateTime Created { get; set; }
        public string Country { get; set; } = string.Empty;
        public string DriverType { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;

        public List<ReviewItemDTO> ReviewItems { get; set; }

        public ReviewCreateDTO()
        {
            ReviewItems = new List<ReviewItemDTO>();
        }

        public override bool Equals(object? obj)
        {
            return obj is ReviewCreateDTO dTO &&
                   Created == dTO.Created &&
                   Country == dTO.Country &&
                   DriverType == dTO.DriverType &&
                   ClientId == dTO.ClientId;  
        }
    }
}
