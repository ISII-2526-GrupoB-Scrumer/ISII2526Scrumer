using System;

public class Car
{

    //Atributos
    public string CarClass { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
    public string EngDisplacement { get; set; }
    public string FuelType { get; set; }
    public int Id { get; set; }
    public string MaintenanceTypes { get; set; }
    public string Manufacturer { get; set; }
    public decimal PurchasingPrice { get; set; }
    public int QuantityForPurchasing { get; set; }
    public int QuantityForRenting { get; set; }
    public decimal RentingPrice { get; set; }
    public int RimSize { get; set; }


    //Relaciones
    public Model Model { get; set; }
    public IList<PurchaseItem> PurchaseItems { get; set; }
    public IList<RentalItem> RentalItems { get; set; }
    public IList<ReviewItem> ReviewItems { get; set; }

    public Car()
    {
    }

    public Car(string carClass, string color, string description, string engDisplacement, string fuelType, int id, string maintenanceTypes, string manufacturer, decimal purchasingPrice, int quantityForPurchasing, int quantityForRenting, decimal rentingPrice, int rimSize, Model model)
    {
        CarClass = carClass;
        Color = color;
        Description = description;
        EngDisplacement = engDisplacement;
        FuelType = fuelType;
        Id = id;
        MaintenanceTypes = maintenanceTypes;
        Manufacturer = manufacturer;
        PurchasingPrice = purchasingPrice;
        QuantityForPurchasing = quantityForPurchasing;
        QuantityForRenting = quantityForRenting;
        RentingPrice = rentingPrice;
        RimSize = rimSize;
        Model = model;
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
