using AppForSEII2526;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII2526.Web
{
    public class MaintenanceStateContainer
    {
        // El DTO real en tu archivo se llama MaintenanceForCreateDTO
        public MaintenanceForCreateDTO Maintenance { get; private set; } = new MaintenanceForCreateDTO()
        {
            // La propiedad en tu DTO se llama ReservaItems, no MaintenanceItems
            ReservaItems = new List<ReservaItemDTO>()
        };

        // El precio total debe calcularse sobre ReservaItems
        public double TotalPrice
        {
            get
            {
                if (Maintenance.ReservaItems == null || !Maintenance.ReservaItems.Any())
                    return 0;

                return Maintenance.ReservaItems.Sum(ri => ri.Price * ri.NumberOfDays);
            }
        }

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        // Este método recibe MantenimientoDTO (el de tu API de selección)
        public void AddMaintenanceToCreate(MantenimientoDTO maintenance)
        {
            // Buscamos si ya existe el servicio por ID
            var existingItem = Maintenance.ReservaItems.FirstOrDefault(ri => ri.ReservaId == maintenance.Id);

            if (existingItem == null)
            {
                Maintenance.ReservaItems.Add(new ReservaItemDTO(
                    maintenance.Id,       // ReservaId
                    maintenance.Nombre,   // Name
                    maintenance.Precio,   // Price
                    maintenance.NumeroDias, // NumberOfDays
                    string.Empty          // Comentarios (opcional, se pasa como string vacío)
                ));
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
