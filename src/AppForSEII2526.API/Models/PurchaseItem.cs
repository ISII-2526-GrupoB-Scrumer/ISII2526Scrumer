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
        public int CarId { get; set; }

        /// <summary>
        /// ID de la compra asociada
        /// </summary>
        public int PurchaseId { get; set; }

        /// <summary>
        /// Cantidad de coches en esta línea de compra
        /// </summary>
        public int Quantity { get; set; }

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
