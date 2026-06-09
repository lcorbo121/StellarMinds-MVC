using System.ComponentModel.DataAnnotations;

namespace StellarMinds.Models.Equipos
{
    public class MonturaViewModel
    {
        public int Id { get; set; }
        [Required, Display(Name = "Marca")] public string Marca { get; set; } = "";
        [Required, Display(Name = "Modelo")] public string Modelo { get; set; } = "";
        [Required, Display(Name = "Tipo de montura")] public string TipoMontura { get; set; } = "";
        [Required, Display(Name = "Carga útil (kg)"), Range(0.01, 9999)] public double CargaUtil { get; set; }
        [Display(Name = "GoTo (computarizada)")] public bool EsGoTo { get; set; }
        [Required, Display(Name = "Cantidad disponible"), Range(0, 9999)] public int CantidadDisponible { get; set; }
    }
}
