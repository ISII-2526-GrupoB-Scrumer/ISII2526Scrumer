namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    // DTO utilizado para devolver una reseña completa desde la API.
    // Hereda de ReviewCreateDTO e incluye información adicional como el Id.
    public class ReviewDetailDTO : ReviewCreateDTO
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }

        public ReviewDetailDTO() { }

        public ReviewDetailDTO(int id, string clientid, DateTime created, string name, string country, string driverType, List<ReviewItemDTO> reviewItems)
        {
            Id = id;
            ClientId = clientid;
            Created = created;
            Name = name;
            Country = country;
            DriverType = driverType;
            ReviewItems = reviewItems;
        }

        // Compara tanto los campos propios como los heredados
        public override bool Equals(object? obj)
        {
            return obj is ReviewDetailDTO dTO &&
                base.Equals(obj) &&
                Created == dTO.Created &&
                Name == dTO.Name &&
                Country == dTO.Country &&
                DriverType == dTO.DriverType &&
                Id == dTO.Id;
        }
    }
}
