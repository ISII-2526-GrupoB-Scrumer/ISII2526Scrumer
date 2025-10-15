using System;


[PrimaryKey(nameof(BookingId), nameof(MaintenanceId))]
public class BookingItem
{
    // Propiedades

    [Required]
    public int BookingId { get; set; }

    [StringLength(200)]
    public string Comment { get; set; }

    [Required,Key]
    public int MaintenanceId { get; set; }

    //Relaciones
    [ForeignKey("BookingId")]
    public Booking Booking { get; set; }

    [ForeignKey("MantId")]
    public Maintenance Maintenance { get; set; }

    // Constructor por defecto
    public BookingItem()
    {
    }

    // Constructor con parámetros
    public BookingItem(int bookingId, string comment, int mantID)
    {
        BookingId = bookingId;
        Comment = comment;
        MaintenanceId = mantID;
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

