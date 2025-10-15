using System;


[PrimaryKey(nameof(BookingId), nameof(MaintenanceID))]
public class BookingItem
{
    // Propiedades

    [Required]
    public int BookingId { get; set; }

    [StringLength(200)]
    public string Comment { get; set; }

    [Required,Key]
    public int MaintenanceID { get; set; }

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
    public BookingItem(int bookingId, string comment, int maintenanceID)
    {
        BookingId = bookingId;
        Comment = comment;
        MaintenanceID = maintenanceID;
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

