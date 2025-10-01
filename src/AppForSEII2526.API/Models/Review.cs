using System;

public class Review
{
    public string Country { get; set; }
    public DateTime Created { get; set; }
    public string DriverType { get; set; }
    public int Id { get; set; }

    public Review()
    {
    }

    public Review(int id, string driverType, string country, DateTime created)
    {
        Id = id;
        DriverType = driverType;
        Country = country;
        Created = created;
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
