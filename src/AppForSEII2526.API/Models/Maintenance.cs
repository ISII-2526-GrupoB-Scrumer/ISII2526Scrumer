using System;

public class Maintenance
{
    // Propiedades

    [Key]
    public int Id { get; set; }

    [Required,StringLength(100)]
    public string Name { get; set; }

    [Range(1,365)]
    public int NumberOfDays { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]   
    [Precision(10,2)]
    public decimal Price { get; set; }

    // Relaciones
    public IList<BookingItem> Bookings { get; set; }
    public IList<MaintenanceType> MaintenanceTypes { get; set; }

    // Constructor por defecto
    public Maintenance()
    {
    }

    // Constructor con parámetros
    public Maintenance(int id, string name, int numberOfDays, decimal price)
    {
        Id = id;
        Name = name;
        NumberOfDays = numberOfDays;
        Price = price;
    }

    // Método Equals
    public override bool Equals(object obj)
    {
        if (obj is Maintenance other)
        {
            return Id == other.Id;
        }
        return false;
    }

    // Método GetHashCode
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
