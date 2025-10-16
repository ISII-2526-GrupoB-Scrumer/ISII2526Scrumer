using System;

public class Booking
{
    // Propiedades
    [Required]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
    public DateTime Date { get; set; }

    [Key]
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string PaymentMethod { get; set; }
    

    //Relaciones
    public IList<BookingItem> Items { get; set; }

    [ForeignKey("ClientId")]
    public ApplicationUser Client { get; set; }

    // Constructor por defecto
    public Booking()
    {
    }

    // Constructor con parámetros
    public Booking(int id, DateTime date,string paymentMethod)
    {
        Id = id;
        Date = date;
        PaymentMethod = paymentMethod;
    }

    // Método Equals
    public override bool Equals(object obj)
    {
        if (obj is Booking otherBooking)
        {
            return this.Id == otherBooking.Id;
        }
        return false;
    }

    // Método GetHashCode
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
