
namespace AppForSEII2526
{
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