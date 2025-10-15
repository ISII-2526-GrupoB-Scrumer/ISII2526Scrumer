using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {


    //Atributos clase Booking
    [Required]
    public string ClientAddress { get; set; }
    [Required,Phone]
    public string ClientPhoneNumber { get; set; }
    [Required]
    

    //Atributos clase Retal
    public string Name { get; set; }
    [Required,StringLength(100)]
    public string Surname { get; set; }

    //Atrubitos clase Review
    
    public string UserName { get; set; }
    

    //Atributos clase Purchase


    //Relaciones
    public IList<Rental> Rentals { get; set; }

    public IList<Review> Reviews { get; set; }
    
    public IList<Booking> Bookings { get; set; }

    public IList<Purchase> Purchases { get; set; }

    //Metodos

    public ApplicationUser() { }

    public ApplicationUser(string clientAddress, string clientPhoneNumber, string paymentMethod, string name, string surname, string country, string userName, string driverType, string deliveryCarDealer)
    {
        ClientAddress = clientAddress;
        ClientPhoneNumber = clientPhoneNumber;
        Name = name;
        Surname = surname;
        UserName = userName;
        
        
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