using AppForSEII2526;
using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class RentalStateContainer
    {
        // Instanciamos un RentalCreateDTO vacío al iniciar
        public RentalCreateDTO Rental { get; private set; } = new RentalCreateDTO()
        {
            RentalItems = new List<RentalItemDTO>()
        };

        // Calculamos el precio total
        public decimal TotalPrice
        {
            get
            {
                // Si falta fecha o la diferencia es inválida, el total es 0
                if (Rental.StartDate == default || Rental.EndDate == default || Rental.EndDate <= Rental.StartDate)
                    return 0;

                int days = (Rental.EndDate - Rental.StartDate).Days;
                if (days < 1) days = 1;

                return Rental.RentalItems.Sum(
                    ri => (decimal)ri.RentingPrice * (decimal)ri.Quantity * (decimal)days
                );


            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        // --------------------------------------
        //  MÉTODOS DE MANEJO DE CARRITO DE ALQUILER
        // --------------------------------------

        public void AddCarToRental(RentalSelectDTO car)
        {
            // Comprobamos si ya existe un item con ese CarId
            var existingItem = Rental.RentalItems.FirstOrDefault(ri => ri.CarId == car.Id);

            if (existingItem == null)
            {
                Rental.RentalItems.Add(new RentalItemDTO()
                {
                    CarId = car.Id,
                    CarModel = car.ModelName,
                    RentingPrice = car.RentingPrice,
                    Quantity = 1,
                    Manufacturer = car.Manufacturer
                });
            }
            else
            {
                // Si ya existe, simplemente aumentamos la cantidad
                existingItem.Quantity += 1;
            }

            NotifyStateChanged();
        }

        public void RemoveRentalItem(RentalItemDTO item)
        {
            Rental.RentalItems.Remove(item);
            NotifyStateChanged();
        }

        public void Clear()
        {
            Rental.RentalItems.Clear();
            NotifyStateChanged();
        }

        // Cuando el alquiler se haya procesado en el backend:
        public void RentalProcessed()
        {
            Rental = new RentalCreateDTO()
            {
                RentalItems = new List<RentalItemDTO>()
            };

            NotifyStateChanged();
        }
    }
}
