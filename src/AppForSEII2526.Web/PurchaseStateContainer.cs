using AppForSEII2526;
using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        // Instanciamos un PurchaseCreateDTO vacío al iniciar
        public PurchaseCreateDTO Purchase { get; private set; } = new PurchaseCreateDTO()
        {
            PurchaseItems = new List<PurchaseItemDTO>()
        };

        // --------------------------------------
        //  CÁLCULO DEL PRECIO TOTAL
        // --------------------------------------
        public double TotalPrice
        {
            get
            {
                return Purchase.PurchaseItems.Sum(
                    pi => pi.PurchasingPrice * pi.Quantity
                );
            }
        }

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();


        // --------------------------------------
        //  MÉTODOS DE MANEJO DEL CARRITO DE COMPRA
        // --------------------------------------

        public void AddCarToPurchase(PurchaseSelectDTO car)
        {
            // ¿Existe ya este coche en el carrito?
            var existingItem = Purchase.PurchaseItems
                .FirstOrDefault(pi => pi.CarId == car.Id);

            if (existingItem == null)
            {
                // Añadimos nuevo item con Quantity = 1
                Purchase.PurchaseItems.Add(new PurchaseItemDTO()
                {
                    CarId = car.Id,
                    CarModel = car.ModelName,
                    PurchasingPrice = car.PurchasingPrice,
                    Quantity = 1
                });
            }
            else
            {
                // Ya existe → se incrementa su cantidad
                existingItem.Quantity += 1;
            }

            NotifyStateChanged();
        }

        public void RemovePurchaseItem(PurchaseItemDTO item)
        {
            Purchase.PurchaseItems.Remove(item);
            NotifyStateChanged();
        }

        public void Clear()
        {
            Purchase.PurchaseItems.Clear();
            NotifyStateChanged();
        }

        // Cuando la compra se haya procesado en el backend:
        public void PurchaseProcessed()
        {
            Purchase = new PurchaseCreateDTO()
            {
                PurchaseItems = new List<PurchaseItemDTO>()
            };

            NotifyStateChanged();
        }
    }
}
