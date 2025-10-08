namespace AppForSEII2526.API.Models
{
    /// <summary>
    /// Representa una compra en el sistema del concesionario
    /// </summary>
    public class Purchase
    {
        /// <summary>
        /// Identificador único de la compra
        /// </summary>

        [Key]
        public int Id { get; set; }



        /// <summary>
        /// Fecha de la compra
        /// </summary>

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [Required]
        public DateTime PurchasingDate { get; set; }

        /// <summary>
        /// Precio total de la compra
        /// </summary>

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Precision(10,2)]
        public decimal PurchasingPrice { get; set; }

        /// <summary>
        /// Lista de items comprados
        /// </summary>
        public List<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

        
        public ApplicationUser Client { get; set; }

        public Purchase()
        {
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Purchase other = (Purchase)obj;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}