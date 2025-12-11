namespace AppForSEII2526
{
    // DTO utilizado para crear una reseña desde el cliente.
    // Contiene los datos del usuario y la lista de coches a reseñar.
    public class ReviewCreateDTO
    {
        public string ClientId { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string DriverType { get; set; } = string.Empty;

        // Lista de coches reseñados, cada uno con puntuación y descripción
        public List<ReviewItemDTO> ReviewItems { get; set; }

        // Constructor vacío necesario para deserialización y binding desde el controlador
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
