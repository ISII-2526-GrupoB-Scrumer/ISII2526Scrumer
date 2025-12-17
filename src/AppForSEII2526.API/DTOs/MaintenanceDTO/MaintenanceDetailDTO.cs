namespace AppForSEII2526
{
    public class MaintenanceDetailDTO : MaintenanceForCreateDTO
    {
        public int Id { get; set; }

        public MaintenanceDetailDTO(int id, string applicationUser, string clientAddress, string paymentMethod, DateTime date, IList<ReservaItemDTO> reservaItems)
        : base(applicationUser, clientAddress, paymentMethod, date, reservaItems)
        {
            Id = id;
        }
        public override bool Equals(object? obj)
        {
            return obj is MaintenanceDetailDTO dTO &&
                Id == dTO.Id &&
                base.Equals(obj) &&
                TotalPrice == dTO.TotalPrice;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id);
        }
    }
}