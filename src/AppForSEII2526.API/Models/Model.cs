using System;

public class Model
{
    public string Id { get; set; }
    public string Name { get; set; }

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
