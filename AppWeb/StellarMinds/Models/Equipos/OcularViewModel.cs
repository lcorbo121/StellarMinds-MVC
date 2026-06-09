using System.ComponentModel.DataAnnotations;

namespace StellarMinds.Models.Equipos
{
    public class OcularViewModel
    {
        public int Id { get; set; }
        [Required, Display(Name = "Marca")] public string Marca { get; set; } = "";
        [Required, Display(Name = "Modelo")] public string Modelo { get; set; } = "";
        [Required, Display(Name = "Diámetro (mm)"), Range(0.01, 999)] public double Diametro { get; set; }
        [Required, Display(Name = "Ángulo de visión (°)"), Range(0.01, 360)] public double AnguloVision { get; set; }
        [Required, Display(Name = "Cantidad disponible"), Range(0, 9999)] public int CantidadDisponible { get; set; }
    }
}
