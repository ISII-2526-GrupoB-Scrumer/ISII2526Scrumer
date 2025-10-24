public class CarforMaintenance
{
    public string Name { get; set; }         
    public string Type { get; set; }         
    public decimal Price { get; set; }       
    public int NumberOfDays { get; set; } 


    public CarforMaintenance( string name, string type, decimal price, int numberofdays)
    {
        
        Name = name;
        Type = type;
        Price = price;
        NumberOfDays = numberofdays;
    }
}

