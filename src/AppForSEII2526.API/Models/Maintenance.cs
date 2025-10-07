using System;

public class Maintenance
{
    // Propiedades
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfDays { get; set; }
    public decimal Price { get; set; }

    // Relaciones
    public IList<BookingItem> Bookings { get; set; }
    public IList<MaintenanceType> MaintenanceTypes { get; set; }

    // Constructor por defecto
    public Maintenance()
    {
    }

    // Constructor con par�metros
    public Maintenance(int id, string name, int numberOfDays, decimal price)
    {
        Id = id;
        Name = name;
        NumberOfDays = numberOfDays;
        Price = price;
    }

    // M�todo Equals
    public override bool Equals(object obj)
    {
        if (obj is Maintenance other)
        {
            return Id == other.Id;
        }
        return false;
    }

    // M�todo GetHashCode
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
