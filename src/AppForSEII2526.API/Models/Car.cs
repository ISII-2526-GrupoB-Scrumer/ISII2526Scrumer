using System;

public class Car
{

    //Atributos
    [Required,StringLength(50)]
    public string CarClass { get; set; }

    [Required, StringLength(30)]
    public string Color { get; set; }

    [StringLength(200)]
    public string Description { get; set; }

    [Required, StringLength(20)]
    public string EngDisplacement { get; set; }

    [Required, StringLength(20)]
    public string FuelType { get; set; }
    [Key]
    public int Id { get; set; }
    public string MaintenanceTypes { get; set; }

    [Required,StringLength(50)]
    public string Manufacturer { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
    [Precision(10, 2)]
    public decimal PurchasingPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int QuantityForPurchasing { get; set; }

    [Range(0, int.MaxValue)]
    public int QuantityForRenting { get; set; }

    [Precision(10, 2)]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
    public decimal RentingPrice { get; set; }
    public int RimSize { get; set; }


    //Relaciones
    [ForeignKey("ModelId")]
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
