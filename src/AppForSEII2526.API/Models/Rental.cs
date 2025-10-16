public class Rental
{

    //Atributos
    [Required,StringLength(100)]
    public string DeliveryCarDealer { get; set; }

    [Required]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
    public DateTime EndDate { get; set; }

    [Key]
    public int Id { get; set; }

    [Required]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
    public DateTime RentingDate { get; set; }

    [Required]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required,Precision(10,2)]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
    public decimal TotalPrice { get; set; }

    [Required]
    public string PaymentMethod { get; set; }

    //Relaciones
    public IList<RentalItem> RentalItems { get; set; }
    public ApplicationUser Client { get; set; }


    //Metodos

    public Rental() { }

    public Rental(string deliveryCarDealer, DateTime endDate, int id, DateTime rentingDate, DateTime startDate, decimal totalPrice)
    {
        DeliveryCarDealer = deliveryCarDealer;
        EndDate = endDate;
        Id = id;
        RentingDate = rentingDate;
        StartDate = startDate;
        TotalPrice = totalPrice;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();

    }

}