namespace AppForSEII2526
{
    public class MaintenanceForCreateDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca un nombre de usuario.")]
        public string ApplicationUser { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca una dirección.")]
        public string ClientAddress { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca una dirección.")]
        public string PaymentMethod { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string? PhoneNumber { get; set; }
        public IList<ReservaItemDTO> ReservaItems { get; set; }
        [Display(Name = "Total Price")]
        [JsonPropertyName("TotalPrice")]
        public double TotalPrice
        {
            get
            {
                return (double)ReservaItems.Sum(ri => ri.Price * ri.NumberOfDays);
            }
        }
        public MaintenanceForCreateDTO(string applicationUser, string clientAddress, string paymentMethod, DateTime date, IList<ReservaItemDTO> reservaItems)
        {
            if (applicationUser is null) throw new ArgumentNullException(nameof(applicationUser));
            if (clientAddress is null) throw new ArgumentNullException(nameof(clientAddress));
            if (reservaItems is null) throw new ArgumentNullException(nameof(reservaItems));

            ApplicationUser = applicationUser;
            ClientAddress = clientAddress;
            PaymentMethod = paymentMethod;
            Date = date;
            ReservaItems = reservaItems;
        }
        public MaintenanceForCreateDTO(string applicationUser, string clientAddress, string paymentMethod, DateTime date, string? phonenumber, IList<ReservaItemDTO> reservaItems)
        {
            if (applicationUser is null) throw new ArgumentNullException(nameof(applicationUser));
            if (clientAddress is null) throw new ArgumentNullException(nameof(clientAddress));
            if (reservaItems is null) throw new ArgumentNullException(nameof(reservaItems));

            ApplicationUser = applicationUser;
            ClientAddress = clientAddress;
            PaymentMethod = paymentMethod;
            Date = date;
            PhoneNumber = phonenumber;
            ReservaItems = reservaItems;
        }
        public MaintenanceForCreateDTO(IList<ReservaItemDTO> reservaItems)
        {
            ReservaItems = reservaItems;
        }

        public MaintenanceForCreateDTO()
        {

        }

        public override bool Equals(object? obj)
        {
            return obj is MaintenanceForCreateDTO dTO &&
                   ApplicationUser == dTO.ApplicationUser &&
                   ClientAddress == dTO.ClientAddress &&
                   PaymentMethod == dTO.PaymentMethod &&
                   Date.Equals(dTO.Date) &&
                   PhoneNumber == dTO.PhoneNumber &&
                   ReservaItems.SequenceEqual(dTO.ReservaItems) &&
                   TotalPrice == dTO.TotalPrice;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ApplicationUser, ClientAddress, PaymentMethod, Date, PhoneNumber, ReservaItems, TotalPrice);
        }
    }
}