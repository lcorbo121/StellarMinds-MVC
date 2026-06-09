using System.ComponentModel.DataAnnotations;

namespace StellarMinds.Models.Equipos
{
    public class CamaraViewModel
    {
        public int Id { get; set; }
        [Required, Display(Name = "Marca")] public string Marca { get; set; } = "";
        [Required, Display(Name = "Modelo")] public string Modelo { get; set; } = "";
        [Required, Display(Name = "Tipo de sensor")] public string TipoSensor { get; set; } = "";
        [Required, Display(Name = "Resolución")] public string Resolucion { get; set; } = "";
        [Required, Display(Name = "Tamaño de píxel (µm)"), Range(0.01, 999)] public decimal TamanoPixel { get; set; }
        [Required, Display(Name = "Cantidad disponible"), Range(0, 9999)] public int CantidadDisponible { get; set; }
    }
}
