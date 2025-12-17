using AppForSEII2526;
using AppForSEII2526.Web.API;
using System.Linq;
using System.Collections.Generic;

namespace AppForSEII2526.Web
{
    public class MaintenanceStateContainer
    {
        // Instanciamos un MaintenanceCreateDTO vacío al iniciar
        public MaintenanceCreateDTO Maintenance { get; private set; } = new MaintenanceCreateDTO()
        {
            MaintenanceItems = new List<MaintenanceItemDTO>()
        };

        // Calculamos el precio total
        public decimal TotalPrice
        {
            get
            {
                // Si los items de mantenimiento están vacíos o no existen, el total es 0
                if (Maintenance.MaintenanceItems == null || !Maintenance.MaintenanceItems.Any())
                    return 0;

                // El precio total lo calculamos en función de los precios de los ítems de mantenimiento y los días
                // El precio total lo calculamos en función de los precios de los ítems de mantenimiento y los días
                return Maintenance.MaintenanceItems.Sum(
                    mi => (decimal)(mi.Price * mi.NumberOfDays) // Asegúrate de convertir el resultado a decimal
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
            // Comprobamos si ya existe un item con ese MaintenanceId
            var existingItem = Maintenance.MaintenanceItems.FirstOrDefault(mi => mi.MaintenanceId == maintenance.Id);

            if (existingItem == null)
            {
                Maintenance.MaintenanceItems.Add(new MaintenanceItemDTO()
                {
                    MaintenanceId = maintenance.Id,
                    MaintenanceName = maintenance.Name,
                    Type = maintenance.Type,
                    Price = maintenance.Price,
                    NumberOfDays = 1 // Por defecto 1 día
                });
            }
            else
            {
                // Si ya existe, simplemente aumentamos la cantidad de días
                existingItem.NumberOfDays += 1;
            }

            NotifyStateChanged();
        }

        public void RemoveMaintenanceItem(MaintenanceItemDTO item)
        {
            Maintenance.MaintenanceItems.Remove(item);
            NotifyStateChanged();
        }

        public void Clear()
        {
            Maintenance.MaintenanceItems.Clear();
            NotifyStateChanged();
        }

        // Cuando el mantenimiento se haya procesado en el backend:
        public void MaintenanceProcessed()
        {
            Maintenance = new MaintenanceCreateDTO()
            {
                MaintenanceItems = new List<MaintenanceItemDTO>()
            };

            NotifyStateChanged();
        }
    }
}
