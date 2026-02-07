namespace AppForSEII2526
{
    public class MaintenanceSelectDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, 500, ErrorMessage = "El precio debe estar entre 1 y 500 euros.")]
        [Display(Name = "Precio")]
        public double Price { get; set; }
        [Required]
        public int NumberOfDays { get; set; }
        public MaintenanceSelectDTO(int id, string name, string type, double price, int numberofDays)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = price;
            NumberOfDays = numberofDays;
        }

        public override bool Equals(object? obj)
        {
            return obj is MaintenanceSelectDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Type == dTO.Type &&
                   Price == dTO.Price &&
                   NumberOfDays == dTO.NumberOfDays;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Type, Price, NumberOfDays);
        }
    }

    // Mantener la clase antigua para compatibilidad hacia atrás si es necesario
    public class MantenimientoDTO
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, 500, ErrorMessage = "El precio debe estar entre 1 y 500 euros.")]
        [Display(Name = "Precio")]
        public double Precio { get; set; }
        [Required]
        public int NumeroDias { get; set; }
        public MantenimientoDTO(int id, string nombre, string tipo, double precio, int numeroDias)
        {
            Id = id;
            Nombre = nombre;
            Tipo = tipo;
            Precio = precio;
            NumeroDias = numeroDias;
        }

        public override bool Equals(object? obj)
        {
            return obj is MantenimientoDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Tipo == dTO.Tipo &&
                   Precio == dTO.Precio &&
                   NumeroDias == dTO.NumeroDias;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Tipo, Precio, NumeroDias);
        }
    }
}