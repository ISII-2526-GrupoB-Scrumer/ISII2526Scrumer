
[PrimaryKey(nameof(RentalId), nameof(CarId))]
public class RentalItem
{

    //Atributos
    [Required]
    public int CarId { get; set; }

    [Range(1,int.MaxValue)]
    public int Quantity { get; set; }

    [Required,Key]
    public int RentalId { get; set; }


    //Relaciones
    [ForeignKey("CarId")]
    public Car Car { get; set; }

    [ForeignKey("RentalId")]
    public Rental Rental { get; set; }
    //Métodos
    public RentalItem() { }


    public RentalItem(int carId, int quantity, int rentalId)
    {
        CarId = carId;
        Quantity = quantity;
        RentalId = rentalId;
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