namespace StellarMinds.Models.Equipos
{
    // Detalle de un equipo. Trae campos de los 4 tipos; según el tipo solo vienen los suyos
    // (los demás quedan en null). El tipo se pasa por ViewBag.Tipo en la vista.
    public class EquipoDetalleItem
    {
        public int Id { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int? CantidadDisponible { get; set; }

        // Telescopio
        public decimal? Apertura { get; set; }
        public decimal? RelacionFocal { get; set; }
        public decimal? DistanciaFocal { get; set; }
        public double? Peso { get; set; }

        // Montura
        public string? TipoMontura { get; set; }
        public double? CargaUtil { get; set; }
        public bool? EsGoTo { get; set; }

        // Cámara
        public string? TipoSensor { get; set; }
        public string? Resolucion { get; set; }
        public decimal? TamanoPixel { get; set; }

        // Ocular
        public double? Diametro { get; set; }
        public double? AnguloVision { get; set; }
    }
}
