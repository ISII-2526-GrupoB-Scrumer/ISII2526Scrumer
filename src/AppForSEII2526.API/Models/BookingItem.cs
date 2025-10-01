using System;

public class BookingItem
{
    // Propiedades
    public int BookingId { get; set; }
    public string Comment { get; set; }
    public int MantId { get; set; }

    // Constructor por defecto
    public BookingItem()
    {
    }

    // Constructor con par�metros
    public BookingItem(int bookingId, string comment, int mantID)
    {
        BookingId = bookingId;
        Comment = comment;
        MantID = mantID;
    }

    // M�todo Equals
    public override bool Equals(object obj)
    {
        if (obj is BookingItem other)
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
