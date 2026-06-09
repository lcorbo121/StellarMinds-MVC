using System.ComponentModel.DataAnnotations;

namespace StellarMinds.Models.Prestamos
{
    public class PrestamoViewModel
    {
        [Required, Display(Name = "Socio")] public int UsuarioId { get; set; }
        [Required, Display(Name = "Telescopio")] public int TelescopioId { get; set; }
        [Required, Display(Name = "Montura")] public int MonturaId { get; set; }
        [Display(Name = "Cámara")] public int? CamaraId { get; set; }
        [Display(Name = "Ocular")] public int? OcularId { get; set; }
        [Required, Display(Name = "Fecha de inicio")] public DateTime FechaInicio { get; set; } = DateTime.Today;
        [Required, Display(Name = "Fecha de fin")] public DateTime FechaFin { get; set; } = DateTime.Today.AddDays(7);
    }

    public class PrestamoListaItem
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public int TelescopioId { get; set; }
        public string? TelescopioNombre { get; set; }
        public int MonturaId { get; set; }
        public string? MonturaNombre { get; set; }
        public int? CamaraId { get; set; }
        public string? CamaraNombre { get; set; }
        public int? OcularId { get; set; }
        public string? OcularNombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = "";
        public bool Atrasado { get; set; }
    }

    public class FiltroFechaViewModel
    {
        [Display(Name = "Mes")] public int Mes { get; set; } = DateTime.Today.Month;
        [Display(Name = "Año")] public int Anio { get; set; } = DateTime.Today.Year;
    }
}
