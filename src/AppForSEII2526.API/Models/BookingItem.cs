using System;

public class BookingItem
{
    // Propiedades

    [Required]
    public int BookingId { get; set; }

    [StringLength(200)]
    public string Comment { get; set; }

    [Required]
    public int MantId { get; set; }

    //Relaciones
    [ForeignKey("BookingId")]
    public Booking Booking { get; set; }

    [ForeignKey("MantId")]
    public Maintenance Maintenance { get; set; }

    // Constructor por defecto
    public BookingItem()
    {
    }

    // Constructor con par�metros
    public BookingItem(int bookingId, string comment, int mantID)
    {
        BookingId = bookingId;
        Comment = comment;
        MantId = mantID;
    }

    // M�todo Equals
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

