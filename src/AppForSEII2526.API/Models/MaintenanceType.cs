using System;

public class MaintenanceType
{
    // Propiedades
    public int id { get; set; }
    public string Type { get; set; }

    //Relaciones
    public Maintenance Maintenance { get; set; }


    // Constructor por defecto
    public MaintenanceType()
    {
    }

    // Constructor con par�metros
    public MaintenanceType(int id, string type)
    {
        this.id = id;
        Type = type;
    }

    // M�todo Equals
    public override bool Equals(object obj)
    {
        if (obj is MaintenanceType other)
        {
            return this.id == other.id;
        }
        return false;
    }

    // M�todo GetHashCode
    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}
