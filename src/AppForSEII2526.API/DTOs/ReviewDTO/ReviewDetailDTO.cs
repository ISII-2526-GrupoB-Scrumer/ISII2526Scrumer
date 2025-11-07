namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    public class ReviewDetailDTO : ReviewCreateDTO
    {
        public int Id { get; set; }

        public ReviewDetailDTO() { }

        public ReviewDetailDTO(int id, DateTime created, string country, string driverType, string clientId, List<ReviewItemDTO> reviewItems)
        {
            Id = id;
            Created = created;
            Country = country;
            DriverType = driverType;
            ClientId = clientId;
            ReviewItems = reviewItems;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReviewDetailDTO dTO &&
                   base.Equals(obj) &&
                   Created == dTO.Created &&
                   Country == dTO.Country &&
                   DriverType == dTO.DriverType &&
                   ClientId == dTO.ClientId &&
                   Id == dTO.Id;
        }
    }
}
