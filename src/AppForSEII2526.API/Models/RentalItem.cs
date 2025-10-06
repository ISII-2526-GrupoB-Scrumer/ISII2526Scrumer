public class RentalItem
{

    //Atributos
    public int CarId { get; set; }
    public int Quantity { get; set; }
    public int RentalId { get; set; }


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