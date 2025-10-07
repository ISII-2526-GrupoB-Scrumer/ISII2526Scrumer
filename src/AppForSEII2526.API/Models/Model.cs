using System;

public class Model
{

    [Key,StringLength(50)]
    public string Id { get; set; }

    [Required,StringLength(100)]
    public string Name { get; set; }

    public IList<Car> Cars { get; set; }

    public Model()
    {
    }

    public Model(string id, string name)
    {
        Id = id;
        Name = name;
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
