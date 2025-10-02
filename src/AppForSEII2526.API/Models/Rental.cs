public class Rental
{

    //Atributos
    public string DeliveryCarDealer { get; set; }
    public DateTime EndDate { get; set; }
    public int Id { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime RentingDate { get; set; }
    public DateTime StartDate { get; set; }
    public decimal TotalPrice { get; set; }


    //Metodos

    public Rental() { }

    public Rental(string deliveryCarDealer, DateTime endDate, int id, string paymentMethod, DateTime rentingDate, DateTime startDate, decimal totalPrice)
    {
        DeliveryCarDealer = deliveryCarDealer;
        EndDate = endDate;
        Id = id;
        PaymentMethod = paymentMethod;
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