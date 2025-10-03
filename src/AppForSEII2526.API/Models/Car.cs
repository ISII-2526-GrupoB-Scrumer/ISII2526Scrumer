using System;

public class Car
{
    public string CarClass { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
    public string EngDisplacement { get; set; }
    public string FuelType { get; set; }
    public int Id { get; set; }
    public string MaintenanceTypes { get; set; }
    public string Manufacturer { get; set; }
    public string PurchaseItems { get; set; }
    public decimal PurchasingPrice { get; set; }
    public int QuantityForPurchasing { get; set; }
    public int QuantityForRenting { get; set; }
    public string RentalItems { get; set; }
    public decimal RentingPrice { get; set; }
    public int RimSize { get; set; }

    public Car()
    {
    }

    public Car(int id, Model model, string carClass, string color, string description, string engDisplacement,
               string fuelType, string maintenanceTypes, string manufacturer, string purchaseItems,
               decimal purchasingPrice, int quantityForPurchasing, int quantityForRenting,
               string rentalItems, decimal rentingPrice, int rimSize)
    {
        Id = id;
        CarClass = carClass;
        Color = color;
        Description = description;
        EngDisplacement = engDisplacement;
        FuelType = fuelType;
        MaintenanceTypes = maintenanceTypes;
        Manufacturer = manufacturer;
        PurchaseItems = purchaseItems;
        PurchasingPrice = purchasingPrice;
        QuantityForPurchasing = quantityForPurchasing;
        QuantityForRenting = quantityForRenting;
        RentalItems = rentalItems;
        RentingPrice = rentingPrice;
        RimSize = rimSize;
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
