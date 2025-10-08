namespace AppForSEII2526.API.Models
{
    /// <summary>
    /// Representa un ítem de compra en el sistema
    /// </summary>
    public class PurchaseItem
    {
        /// <summary>
        /// ID del coche asociado
        /// </summary>

        [Required,Key]
        public int CarId { get; set; }

        /// <summary>
        /// ID de la compra asociada
        /// </summary>

        [Required]
        public int PurchaseId { get; set; }

        /// <summary>
        /// Cantidad de coches en esta línea de compra
        /// </summary>

        [Range(1,int.MaxValue)]
        public int Quantity { get; set; }

        
        public Purchase Purchase { get; set; }

        
        public Car Car { get; set; }

        public PurchaseItem()
        {
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            PurchaseItem other = (PurchaseItem)obj;
            return CarId == other.CarId && PurchaseId == other.PurchaseId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CarId, PurchaseId);
        }
    }
}
