using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {


    //Atributos clase Booking
    public string ClientAddress { get; set; }
    public string ClientPhoneNumber { get; set; }
    public string PaymentMethod { get; set; }

    //Atributos clase Retal
    public string Name { get; set; }
    public string Surname { get; set; }

    //Atrubitos clase Review
    public string Country { get; set; }
    public string UserName { get; set; }
    public string DriverType { get; set; }

    //Atributos clase Purchase
    public string DeliveryCarDealer { get; set; }



    public IList<Purchase> Purchases { get; set; }

    //Metodos

    public ApplicationUser() { }

    public ApplicationUser(string clientAddress, string clientPhoneNumber, string paymentMethod, string name, string surname, string country, string userName, string driverType, string deliveryCarDealer)
    {
        ClientAddress = clientAddress;
        ClientPhoneNumber = clientPhoneNumber;
        PaymentMethod = paymentMethod;
        Name = name;
        Surname = surname;
        Country = country;
        UserName = userName;
        DriverType = driverType;
        DeliveryCarDealer = deliveryCarDealer;
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);

    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }


}