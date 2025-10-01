using System;

public class Booking
{
    // Propiedades
    public string clientAddress { get; set; }
    public string clientName { get; set; }
    public string clientPhoneNum { get; set; }
    public string clientSurname { get; set; }
    public DateTime Date { get; set; }
    public int Id { get; set; }
    public string PaymentMethod { get; set; }

    // Constructor por defecto
    public Booking()
    {
    }

    // Constructor con par�metros
    public Booking(int id, string clientName, string clientSurname, string clientAddress,
                   string clientPhoneNum, DateTime date, string paymentMethod)
    {
        Id = id;
        this.clientName = clientName;
        this.clientSurname = clientSurname;
        this.clientAddress = clientAddress;
        this.clientPhoneNum = clientPhoneNum;
        Date = date;
        PaymentMethod = paymentMethod;
    }

    // M�todo Equals
    
    public override bool Equals(object obj)
    {
        if (obj is Booking otherBooking)
        {
            return this.Id == otherBooking.Id;
        }
        return false;
    }

    // M�todo GetHashCode
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
