using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class ReviewStateContainer
    {
        // Instancia de Review cuando se crea el ReviewStateContainer
        public ReviewCreateDTO Review { get; private set; } = new ReviewCreateDTO()
        {
            ReviewItems = new List<ReviewItemDTO>()
        };

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        // Añadir un coche a la review
        public void AddCarToReview(ReviewSelectDTO car)
        {
            // evitar duplicados
            if (!Review.ReviewItems.Any(ri => ri.CarId == car.Id))
            {
                Review.ReviewItems.Add(new ReviewItemDTO()
                {
                    CarId = car.Id,
                    CarModel = car.ModelName,
                    Manufacturer = car.Manufacturer,
                    FuelType = car.FuelType,
                    Color = car.Color,
                    Description = string.Empty,  // lo rellenará el usuario
                    Rating = 0                   // lo rellenará el usuario
                });
            }

            NotifyStateChanged();
        }


        // Eliminar un ítem de review
        public void RemoveReviewItem(ReviewItemDTO item)
        {
            Review.ReviewItems.Remove(item);
            NotifyStateChanged();
        }

        // Vaciar todos los ítems de la review
        public void ClearReview()
        {
            Review.ReviewItems.Clear();
            NotifyStateChanged();
        }

        // Cuando ya hemos procesado la review, reiniciamos el objeto
        public void ReviewProcessed()
        {
            Review = new ReviewCreateDTO()
            {
                ReviewItems = new List<ReviewItemDTO>()
            };

            NotifyStateChanged();
        }
    }
}
