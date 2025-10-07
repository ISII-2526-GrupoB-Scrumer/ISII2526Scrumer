using System;

public class Booking
{
    // Propiedades
    public DateTime Date { get; set; }
    public int Id { get; set; }

    //Relaciones
    public IList<BookingItem> Items { get; set; }
    public ApplicationUser Client { get; set; }

    // Constructor por defecto
    public Booking()
    {
    }

    // Constructor con par�metros
    public Booking(int id, DateTime date)
    {
        Id = id;
        Date = date;
    }

    // M�todo Equals
    public override bool Equals(object obj)
    {
        if (obj is Booking otherBooking)
        {
            return this.Id == otherBooking.Id;
        }
        return false;
    }

    // M�todo GetHashCode
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
