using AppForSEII2526.Web.API;
using System.Linq;
using System.Collections.Generic;

namespace AppForSEII2526.Web
{
    public class MaintenanceStateContainer
    {
        // Instanciamos un MaintenanceForCreateDTO vacío al iniciar
        public MaintenanceForCreateDTO Maintenance { get; private set; } = new MaintenanceForCreateDTO()
        {
            ReservaItems = new List<ReservaItemDTO>()
        };

        // Calculamos el precio total
        public decimal TotalPrice
        {
            get
            {
                if (Maintenance.ReservaItems == null || !Maintenance.ReservaItems.Any())
                    return 0;

                return Maintenance.ReservaItems.Sum(
                    mi => (decimal)(mi.Price * mi.NumberOfDays)
                );
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        // --------------------------------------
        // MÉTODOS DE MANEJO DE MANTENIMIENTO
        // --------------------------------------

        public void AddMaintenanceToCreate(MaintenanceSelectDTO maintenance)
        {
            var existingItem = Maintenance.ReservaItems
                .FirstOrDefault(mi => mi.ReservaId == maintenance.Id);

            if (existingItem == null)
            {
                Maintenance.ReservaItems.Add(new ReservaItemDTO()
                {
                    ReservaId = maintenance.Id,
                    Name = maintenance.Name,
                    Price = maintenance.Price,
                    NumberOfDays = 1
                });
            }
            else
            {
                existingItem.NumberOfDays += 1;
            }

            NotifyStateChanged();
        }

        public void RemoveMaintenanceItem(ReservaItemDTO item)
        {
            Maintenance.ReservaItems.Remove(item);
            NotifyStateChanged();
        }

        public void Clear()
        {
            Maintenance.ReservaItems.Clear();
            NotifyStateChanged();
        }

        // Cuando el mantenimiento se haya procesado en el backend
        public void MaintenanceProcessed()
        {
            Maintenance = new MaintenanceForCreateDTO()
            {
                ReservaItems = new List<ReservaItemDTO>()
            };

            NotifyStateChanged();
        }
    }
}
