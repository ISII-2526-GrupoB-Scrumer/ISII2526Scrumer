namespace AppForSEII2526
{
    public class ReservaItemDTO
    {
        public int ReservaId { get; set; }
        public double Price { get; set; }
        public int NumberOfDays { get; set; }
        [Required(AllowEmptyStrings = true, ErrorMessage = "Por favor, introduzca una comentario.")]
        public string Comentarios { get; set; }
        public string Name { get; set; }

        public ReservaItemDTO(int reservaId, string name, double price, int numberofdays, string comentarios = "")
        {
            ReservaId = reservaId;
            Name = name;
            Price = price;
            NumberOfDays = numberofdays;
            Comentarios = comentarios;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReservaItemDTO dTO &&
                   ReservaId == dTO.ReservaId &&
                   Price == dTO.Price &&
                   NumberOfDays == dTO.NumberOfDays &&
                   Comentarios == dTO.Comentarios &&
                   Name == dTO.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReservaId, Price, NumberOfDays, Comentarios, Name);
        }
    }
}