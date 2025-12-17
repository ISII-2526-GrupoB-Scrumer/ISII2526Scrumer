using System;
using System.Collections.Generic;
using System.Linq;
// Este es el namespace que aparece en tus archivos MaintenanceCreateDTO.cs y ReservaItemDTO.cs
using AppForSEII2526;

namespace AppForSEII2526
{
    public class MaintenanceStateContainer
    {
        public MaintenanceForCreateDTO CurrentBooking { get; private set; } = new MaintenanceForCreateDTO
        {
            ReservaItems = new List<ReservaItemDTO>(),
            Date = DateTime.Now
        };

        public event Action OnChange;

        public void AddMaintenance(MantenimientoDTO maintenance)
        {
            var existing = CurrentBooking.ReservaItems
                .FirstOrDefault(i => i.ReservaId == maintenance.Id);

            if (existing == null)
            {
                var newItem = new ReservaItemDTO(
                    maintenance.Id,
                    maintenance.Nombre,
                    maintenance.Precio,
                    maintenance.NumeroDias,
                    ""
                );

                CurrentBooking.ReservaItems.Add(newItem);
                NotifyStateChanged();
            }
        }

        public void SetClientData(string applicationUser, string address, string payment, string phone)
        {
            CurrentBooking.ApplicationUser = applicationUser;
            CurrentBooking.ClientAddress = address;
            CurrentBooking.PaymentMethod = payment;
            CurrentBooking.PhoneNumber = phone;
            NotifyStateChanged();
        }

        public void UpdateItemComment(int maintenanceId, string comment)
        {
            var item = CurrentBooking.ReservaItems.FirstOrDefault(i => i.ReservaId == maintenanceId);
            if (item != null)
            {
                item.Comentarios = comment;
                NotifyStateChanged();
            }
        }

        public void RemoveMaintenance(int maintenanceId)
        {
            var item = CurrentBooking.ReservaItems.FirstOrDefault(i => i.ReservaId == maintenanceId);
            if (item != null)
            {
                CurrentBooking.ReservaItems.Remove(item);
                NotifyStateChanged();
            }
        }

        public void Reset()
        {
            CurrentBooking = new MaintenanceForCreateDTO
            {
                ReservaItems = new List<ReservaItemDTO>(),
                Date = DateTime.Now
            };
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}