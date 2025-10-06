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

    // Constructor con parámetros
    public BookingItem(int bookingId, string comment, int mantID)
    {
        BookingId = bookingId;
        Comment = comment;
        MantId = mantID;
    }

    // Método Equals
    public override bool Equals(object obj)
    {
        if (obj is BookingItem other)
        {
            return this.BookingId == other.BookingId;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return BookingId.GetHashCode();
    }
}

