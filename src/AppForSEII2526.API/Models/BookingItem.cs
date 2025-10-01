using System;

public class BookingItem
{
    // Propiedades
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    // Constructor por defecto
    public BookingItem()
    {
    }

    // Constructor con par�metros
    public BookingItem(int id, string description, decimal price, int quantity)
    {
        Id = id;
        Description = description;
        Price = price;
        Quantity = quantity;
    }

    // M�todo Equals
    public override bool Equals(object obj)
    {
        if (obj is BookingItem other)
        {
            return Id == other.Id;
        }
        return false;
    }

    // M�todo GetHashCode
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
