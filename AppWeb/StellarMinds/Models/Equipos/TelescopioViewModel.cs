using System.ComponentModel.DataAnnotations;

namespace StellarMinds.Models.Equipos
{
    public class TelescopioViewModel
    {
        public int Id { get; set; }
        [Required, Display(Name = "Marca")] public string Marca { get; set; } = "";
        [Required, Display(Name = "Modelo")] public string Modelo { get; set; } = "";
        [Required, Display(Name = "Apertura (mm)"), Range(0.01, 9999)] public decimal Apertura { get; set; }
        [Required, Display(Name = "Relación focal"), Range(0, 9999)] public decimal RelacionFocal { get; set; }
        [Required, Display(Name = "Distancia focal (mm)"), Range(0, 99999)] public decimal DistanciaFocal { get; set; }
        [Required, Display(Name = "Peso (kg)"), Range(0, 9999)] public double Peso { get; set; }
        [Required, Display(Name = "Cantidad disponible"), Range(0, 9999)] public int CantidadDisponible { get; set; }
    }
}
